(function(){

var templates = [ 
	"leftside_text",
	"rightside_text",
	"task_text",
	"progress_text",
	"task_class"
];

function defaults(obj, std){
	for (var key in std)
		if (!obj[key])
			obj[key] = std[key];
	return obj;
}

//compatibility for new versions of gantt
if(!gantt.ajax){
	if(window.dhtmlxAjax){
		gantt.ajax = window.dhtmlxAjax;
	}else if(window.dhx4){
		gantt.ajax = window.dhx4.ajax;
	}
}

function mark_columns(base){
	var columns = base.config.columns;
	if (columns)
		for (var i = 0; i < columns.length; i++) {
			if (columns[i].template)
				columns[i].$template = true;
	}
}


function add_export_methods(gantt){
	var color_box = null;
	var color_hash = {};
	function get_styles(css){
		if (!color_box){
			var color_box = document.createElement("DIV");
			color_box.style.cssText = "position:absolute; display:none;";
			document.body.appendChild(color_box);
		}
		if (color_hash[css])
			return color_hash[css];

		color_box.className = css;	
		return (color_hash[css] = get_color(color_box, "color")+";"+get_color(color_box, "backgroundColor"));
	}
	gantt.exportToPDF = function(config){
		console.debug("COONFIG", config);
		if (config && config.raw){
			config = defaults(config, { 
				name:"gantt.pdf", data:this._serialize_html()
			});
		} else {
			console.debug("ELSE",this.config);
			delete this.config.start_date;
			delete this.config.end_date;
			delete this.config.original_start_date;
			delete this.config.original_end_date;
			console.debug("ELSE",this.config);
			config = defaults((config || {}), { 
				name:"gantt.pdf",
				data:this._serialize_all(),
				config:this.config
			});
			console.debug(config);
			fix_columns(gantt, config.config.columns);
		}

		config.version = this.version;
		this._send_to_export(config, "pdf");
	};

	gantt.exportToPNG = function(config){
		if (config && config.raw){
			config = defaults(config, { 
				name:"gantt.png", data:this._serialize_html()
			});
		} else {
			config = defaults((config || {}), { 
				name:"gantt.png",
				data:this._serialize_all(),
				config:this.config
			});
			fix_columns(gantt, config.config.columns);
		}

		config.version = this.version;
		this._send_to_export(config, "png");
	};

	gantt.exportToICal = function(config){
		config = defaults((config || {}), { 
			name:"gantt.ical",
			data:this._serialize_plain().data,
			version:this.version
		});
		this._send_to_export(config, "ical");
	};


	
	function eachTaskTimed(start, end){
		return function(code, parent, master){
			parent = parent || this.config.root_id;
			master = master || this;

			var branch = this.getChildren(parent);
			if (branch)
				for (var i=0; i<branch.length; i++){
					var item = this._pull[branch[i]];
					if ((!start || item.end_date > start) && (!end || item.start_date < end))
						code.call(master, item);
					
					if (this.hasChild(item.id))
						this.eachTask(code, item.id, master);
				}
		};
	}

	gantt.exportToExcel = function(config){
		config = config || {};

		var tasks, dates;
		var state, scroll;
		if (config.start || config.end){
			state = this.getState();
			dates = [ this.config.start_date, this.config.end_date ];
			scroll = this.getScrollState();
			var convert = this.date.str_to_date(this.config.api_date);
			tasks = this.eachTask;

			if (config.start)
				this.config.start_date = convert(config.start);
			if (config.end)
				this.config.end_date = convert(config.end);

			this.render();
			this.eachTask = eachTaskTimed(this.config.start_date, this.config.end_date);
		}

		this._no_progress_colors =config.visual === "base-colors";

		config = defaults(config, { 
			name:"gantt.xlsx",
			title:"Tasks",
			data:this._serialize_table(config).data,
			columns:this._serialize_columns({ rawDates: true }),
			version:this.version
		});

		if (config.visual)
			config.scales = this._serialize_scales(config);

		this._send_to_export(config, "excel");

		if (config.start || config.end){
			this.config.start_date = state.min_date;
			this.config.end_date = state.max_date;
			this.eachTask = tasks;

			this.render();
			this.scrollTo(scroll.x, scroll.y);

			this.config.start_date = dates[0];
			this.config.end_date = dates[1];
		}
	};

	gantt.exportToMSProject = function (config) {
		config = defaults((config || {}), { 
			name:"gantt.xml",
			data:this._msp_data(),
			config: this.config,
			columns:this._serialize_columns(),
			version:this.version
		});
		this._msp_config(config.config);
		this._send_to_export(config, "msproject");
	};

	gantt.exportToJSON = function(config){
		config = defaults((config || {}), { 
			name:"gantt.json",
			data:this._serialize_all(),
			config: this.config,
			columns:this._serialize_columns(),
			worktime : {
				hours : this._working_time_helper.hours,
				dates : this._working_time_helper.dates
			},
			version:this.version
		});
		this._send_to_export(config, "json");	
	};

	gantt._msp_config = function(config){
		
		if (config.project)
			for (var i in config.project){
				if (!config._custom_data)
					config._custom_data = {};
				config._custom_data[i] = config.project[i](this.config);
			}
			
		if (config.tasks)
			for (var j = 0; j < config.data.length; j++){
				var el = this.getTask(config.data[j].id);
				if (!el._custom_data)
					el._custom_data = {};
				for (var i in config.tasks)
					el._custom_data[i] = config.tasks[i](el, this.config);
			}
				
		delete config.project;
		delete config.tasks;

		config.time = {
			hours : gantt._working_time_helper.hours,
			dates : gantt._working_time_helper.dates
		};

		var p_dates = this.getSubtaskDates();
		var format = this.date.date_to_str("%d-%m-%Y %H:%i:%s");
		config.start_end = {
			start_date: format(p_dates.start_date),
			end_date: format(p_dates.end_date)
		};

	};

	gantt._msp_data = function(){
		var old_xml_format = this.templates.xml_format;
		this.templates.xml_format = this.date.date_to_str("%d-%m-%Y %H:%i:%s");
		
		var data = this._serialize_all();

		this.templates.xml_format = old_xml_format;
		return data;
	};

	gantt._ajax_to_export = function(data, type, callback){
		delete data.callback;

		var url = data.server || "https://export.dhtmlx.com/gantt";

		gantt.ajax.post(url,
			"type="+type+"&store=1&data="+encodeURIComponent(JSON.stringify(data)),
			function(loader){
				var fail = loader.xmlDoc.status > 400;
				var info = null;

				if (!fail){
					try{
						info = JSON.parse(loader.xmlDoc.responseText);

					}catch(e){}
				}
				console.debug("INFO",info);
				callback(info);
			}
		);
	};

	gantt._send_to_export = function(data, type){
		if (data.config)
			mark_columns(data, type);

		if (data.callback)
			return gantt._ajax_to_export(data, type, data.callback);

		var form = this._create_hidden_form();
		form.firstChild.action = data.server || "https://export.dhtmlx.com/gantt";
		form.firstChild.childNodes[0].value = JSON.stringify(data);
		form.firstChild.childNodes[1].value = type;
		form.firstChild.submit();
	};

	gantt._create_hidden_form = function(){
		if (!this._hidden_export_form){
			var t = this._hidden_export_form = document.createElement("div");
			t.style.display = "none";
			t.innerHTML = "<form method='POST' target='_blank'><textarea name='data' style='width:0px; height:0px;' readonly='true'></textarea><input type='hidden' name='type' value=''></form>";
			document.body.appendChild(t);
		}
		return this._hidden_export_form;
	};

	//patch broken json serialization in gantt 2.1
	var original = gantt.json._copyObject;
	function copy_object_base(obj){
		var copy = {};
		for (var key in obj){
			if (key.charAt(0) == "$")
				continue;
			copy[key] = obj[key];
		}
		console.debug("COPY", copy);
		console.debug("START_DATE",copy.start_date);
		console.debug("END_DATE", copy.end_date);
		copy.start_date = gantt.templates.xml_format(copy.start_date);
		if (copy.end_date)
			copy.end_date = gantt.templates.xml_format(copy.end_date);
		console.debug("COPY AFTER", copy);
		return copy;
	}

	function copy_object_plain(obj){
		var text = gantt.templates.task_text(obj.start_date, obj.end_date, obj);

		var copy = copy_object_base(obj);
		copy.text = text || copy.text;

		return copy;
	}

	function get_color(node, style){
		var value = node.currentStyle ? node.currentStyle[style] : getComputedStyle(node, null)[style];
		var rgb = value.replace(/\s/g,'').match(/^rgba?\((\d+),(\d+),(\d+)/i);
		return ((rgb && rgb.length === 4) ? 
		("0" + parseInt(rgb[1],10).toString(16)).slice(-2) +
		("0" + parseInt(rgb[2],10).toString(16)).slice(-2) +
		("0" + parseInt(rgb[3],10).toString(16)).slice(-2) : value).replace("#","");
	}

	function copy_object_table(obj){
		var copy = copy_object_columns(obj, copy_object_plain(obj));
		if (copy.start_date)
			copy.start_date = obj.start_date.valueOf();
		if (copy.end_date)
			copy.end_date = obj.end_date.valueOf();

		copy.$start = gantt._day_index_by_date(obj.start_date);
		copy.$end	= gantt._day_index_by_date(obj.end_date);
		copy.$level	= obj.$level;
		copy.$type	= obj.$rendered_type;

		var tmps = gantt.templates;
		copy.$text = tmps.task_text(obj.start, obj.end_date, obj);
		copy.$left = tmps.leftside_text ? tmps.leftside_text(obj.start, obj.end_date, obj) : "";
		copy.$right = tmps.rightside_text ? tmps.rightside_text(obj.start, obj.end_date, obj) : "";
	
		return copy;
	}

	function copy_object_colors(obj){
		var copy = copy_object_table(obj);

		var node = gantt.getTaskNode(obj.id);
		if (node && node.firstChild){
			var color = get_color((gantt._no_progress_colors ? node : node.firstChild), "backgroundColor");
			if (color == "363636")
				color = get_color(node, "backgroundColor");

			copy.$color = color;
		} else if (obj.color)
			copy.$color = obj.color;

		return copy;
	}

	function copy_object_columns(obj, copy){
		for(var i=0; i<gantt.config.columns.length; i++){
			var ct = gantt.config.columns[i].template;
			if (ct) {
				var val = ct(obj);
				if (val instanceof Date)
					val = gantt.templates.date_grid(val, obj);
				copy["_"+i] = val;
			}
		}
		return copy;
	}

	function copy_object_all(obj){
		var copy = copy_object_base(obj);

		//serialize all text templates
		for (var i = 0; i < templates.length; i++){
			var template = gantt.templates[templates[i]];
			if (template)
				copy["$"+i]	= template(obj.start_date, obj.end_date, obj);
		}

		copy_object_columns(obj, copy);
		copy.open = obj.$open;
		return copy;
	}

	function fix_columns(gantt, columns){
		for (var i = 0; i < columns.length; i++) {
			columns[i].label = columns[i].label || gantt.locale.labels["column_"+columns[i].name];
			if (typeof columns[i].width == "string") columns[i].width = columns[i].width*1;
		}
	}

	gantt._serialize_html = function(){
		return this.$container.parentNode.innerHTML;
	};

	gantt._serialize_all = function(){
		console.debug("COPY_OBJECT_ALL", copy_object_all);
		gantt.json._copyObject = copy_object_all;
		var data = export_serialize();
		gantt.json._copyObject = original;
		return data;
	};

	gantt._serialize_plain = function(){
		var config = gantt.templates.xml_format;
		gantt.templates.xml_format = gantt.date.date_to_str("%Y%m%dT%H%i%s", true);
		gantt.json._copyObject = copy_object_plain;

		var data = export_serialize();

		gantt.templates.xml_format = config;
		gantt.json._copyObject = original;

		delete data.links;
		return data;
	};

	gantt._serialize_table = function(config){
		gantt.json._copyObject = config.visual ? copy_object_colors : copy_object_table;
		var data = export_serialize();
		gantt.json._copyObject = original;

		delete data.links;

		if (config.cellColors){
			var css = this.templates.task_cell_class;
			if (css){
				var raw = this._scale_helpers.prepareConfigs(this._get_scales(), this.config.min_column_width, (this._get_resize_options().x ? Math.max(this.config.autosize_min_width, 0) : this.$task.offsetWidth ) , this.config.scale_height - 1);
				var steps = raw[0].trace_x;
				for (var i=1; i<raw.length; i++)
					if (raw[i].trace_x.length > steps.length)
						steps = raw[i].trace_x;

				for (var i=0; i<data.data.length; i++){
					data.data[i].styles = [];
					var task = this.getTask(data.data[i].id);
					for (var j = 0; j < steps.length; j++) {
						var date = steps[j];
						var cell_css = css(task, date);
						if (cell_css)
							data.data[i].styles.push({ index: j, styles: get_styles(cell_css) });
					}
				}
			}
		}
		return data;
	};

	gantt._serialize_scales = function(config){
		var scales = [];
		var raw = this._scale_helpers.prepareConfigs(this._get_scales(), this.config.min_column_width, (this._get_resize_options().x ? Math.max(this.config.autosize_min_width, 0) : this.$task.offsetWidth ) , this.config.scale_height - 1);
		
		var min = Infinity;
		var max = 0;
		for (var i=0; i<raw.length; i++) min = Math.min(min, raw[i].col_width);

		for (var i=0; i<raw.length; i++){
			var start = 0;
			var end = 0;
			var row = [];

			scales.push(row);
			var step = raw[i];
			max = Math.max(max, step.trace_x.length);
			var template = step.template || (step.date ? gantt.date.date_to_str(step.date) :  gantt.config.date_scale);

			for (var j = 0; j < step.trace_x.length; j++) {
				var date = step.trace_x[j];
				end = start + Math.round(step.width[j]/min);

				var scale_cell = { text: template(date), start: start, end: end };

				if (config.cellColors){
					var css = step.css || this.templates.scale_cell_class;
					if (css){
						var scale_css = css(date);
						if (scale_css)
							scale_cell.styles=get_styles(scale_css);
					}
				}

				row.push(scale_cell);
				start = end;
			}
		}

		return { width:max, height:scales.length, data:scales };
	};

	gantt._serialize_columns = function(config){
		gantt.exportMode = true;

		var columns = [];
		var cols = gantt.config.columns;

		var ccount = 0;
		for (var i = 0; i < cols.length; i++){
			if (cols[i].name == "add" || cols[i].name == "buttons") continue;

			columns[ccount] = {
				id:		((cols[i].template) ? ("_"+i) : cols[i].name),
				header:	cols[i].label || gantt.locale.labels["column_"+cols[i].name],
				width: 	(cols[i].width ? Math.floor(cols[i].width/4) : "")
			};

			if (cols[i].name == "duration")
				columns[ccount].type = "number";
			if (cols[i].name == "start_date" || cols[i].name == "end_date"){
				columns[ccount].type = "date";
				if (config && config.rawDates)
					columns[ccount].id = cols[i].name;
			}

			ccount++;
		}
		
		gantt.exportMode = false;
		return columns;
	};

	function export_serialize(){
		gantt.exportMode = true;
		var data = gantt.serialize();
		gantt.exportMode = false;
		return data;
	}
}

add_export_methods(gantt);
if (window.Gantt && Gantt.plugin)
	Gantt.plugin(add_export_methods);

})();