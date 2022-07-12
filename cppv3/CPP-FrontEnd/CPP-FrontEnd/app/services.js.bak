//'use strict';

angular.module('xenon.services', []).
	service('$menuItems',['localStorageService', function(localStorageService)
	{
		this.menuItems = [];
		console.log(localStorageService.get('authorizationData'));
		var $menuItemsRef = this;

		var menuItemObj = {
			parent: null,

			title: '',
			link: '', // starting with "./" will refer to parent link concatenation
			state: '', // will be generated from link automatically where "/" (forward slashes) are replaced with "."
			icon: '',

			isActive: false,
			label: null,

			menuItems: [],

			setLabel: function(label, color, hideWhenCollapsed)
			{
				if(typeof hideWhenCollapsed == 'undefined')
					hideWhenCollapsed = true;

				this.label = {
					text: label,
					classname: color,
					collapsedHide: hideWhenCollapsed
				};

				return this;
			},

			addItem: function(title, link, icon)
			{
				var parent = this,
					item = angular.extend(angular.copy(menuItemObj), {
						parent: parent,

						title: title,
						link: link,
						icon: icon
					});

				if(item.link)
				{
					if(item.link.match(/^\./))
						item.link = parent.link + item.link.substring(1, link.length);

					if(item.link.match(/^-/))
						item.link = parent.link + '-' + item.link.substring(2, link.length);


					item.state = $menuItemsRef.toStatePath(item.link);
				}

				this.menuItems.push(item);

				return item;
			}
		};

		this.addItem = function(title, link, icon)
		{
			var item = angular.extend(angular.copy(menuItemObj), {
				title: title,
				link: link,
				state: this.toStatePath(link),
				icon: icon

			});

			this.menuItems.push(item);

			return item;
		};

		this.getAll = function()
		{
			return this.menuItems;
		};

		this.prepareSidebarMenu = function() {
			var wbs = this.addItem('Program Navigation', '/app/wbs', 'linecons-database');
			//add dashboard back if we need it
		//	var dashboard    = this.addItem('Dashboard','/app/dashboard','linecons-note');
			//var baseline_project = this.addItem('Baseline Project','/app/baseline-project/105/1/13', 'linecons-note');
			// var project_location    = this.addItem('Project Location', 		'/app/project-location', 			'linecons-globe');
			//var const_timeline = this.addItem('Construction Timeline', '/app/const-timeline', 'linecons-params');
			var s = localStorageService.get('authorizationData');
			console.log(s);
			if (s) {
                if (localStorageService.get('authorizationData').role === "Admin") {
					var admin = this.addItem('Administration', '/app/admin', 'linecons-desktop');
					admin.addItem('Actuals Upload', '-/actuals-upload');
					admin.addItem('Application Security', '-/access-control'); // "-/" will append parents link
					admin.addItem('Client', '-/client'); // Commented by Manasi
					admin.addItem('Client POC', '-/clientPOC');  // Commented by Tanmay - 07-12-2021
					admin.addItem('Department', '-/project-class');
					//admin.addItem('Department to Phase Mapping', '-/project-class-phase');
					admin.addItem('Services', '-/service-class');
					admin.addItem('Services to Subservices Mapping', '-/service-to-subservice-mapping');
					admin.addItem('Document Type', '-/document-type');
					//admin.addItem('Location', '-/location'); //territoryCtrl // Aditya ---hide location from sidebar---
					admin.addItem('Organization', '-/organization');
					//admin.addItem('Project Approval Requirements', '-/approval-matrix'); //Manasi
                    admin.addItem('Trend Approval Requirements', '-/approval-matrix'); // Manasi
					admin.addItem('Subservices', '-/phase-code');
					//admin.addItem('Project Type', '-/project-type');   Commented Manasi
					admin.addItem('Trend Status Code', '-/trend-status-code');
					admin.addItem('User Management', '-/users');
					admin.addItem('Vendor', '-/vendor');
					admin.addItem('Manufacturer', '-/manufacturer');
					admin.addItem('Inventory', '-/inventory');
					admin.addItem('Work Breakdown Structure', '-/budget-categories'); // "-/" will append parents link					
					//admin.addItem('File Download', '-/filedownload'); Commented Manasi
					//admin.addItem('Whitelist', '-/whitelist'); Commented by Manasi

			
					var costSetting = this.addItem('Cost', '/app/admin', 'linecons-money');
					costSetting.addItem('Cost Overhead', '-/cost-overhead');
					costSetting.addItem('Employee', '-/employee');
					costSetting.addItem('Material', '-/material');
					costSetting.addItem('Material Category', '-/material-category');
					costSetting.addItem('ODC Type', '-/odc-type');
					costSetting.addItem('Position Titles', '-/positions'); // "-/" will append parents link
					costSetting.addItem('Subcontractor', '-/subcontractor');
					costSetting.addItem('Subcontractor Type', '-/subcontractor-type');
					costSetting.addItem('Unit Type', '-/unit-type');

                    var adminReport = this.addItem('Admin Report', '/app/admin-report', 'linecons-doc');

                    var statistic = this.addItem('Statistics', '/app/admin-Chart', 'glyphicon glyphicon-signal');   //Manasi 11-09-2020
				}
				else if (localStorageService.get('authorizationData').role === "Accounting")
				{
					var accounting = this.addItem('Purchase Order', '/app/po-Approval/', 'linecons-desktop');
				}
			}
			//admin.addItem('Login','-/login');
			//admin.addItem('Signup','-/signup');
			//var login =this.addItem('Login','/app/test','linecons-star');
			//login.addItem('login','-/login');
			//login.addItem('signup','-/signup');

			//var order = this.addItem('order','/app/order','linecons-star');
			/*

			 var layouts      = this.addItem('Layout & Skins',	'/app/layout-and-skins',	'linecons-desktop');
			 var ui_elements  = this.addItem('UI Elements', 		'/app/ui', 					'linecons-note');
			 var widgets  	 = this.addItem('Widgets', 			'/app/widgets', 			'linecons-star');
			 var mailbox  	 = this.addItem('Mailbox', 			'/app/mailbox', 			'linecons-mail').setLabel('5', 'secondary', false);
			 var tables  	 = this.addItem('Tables', 			'/app/tables', 				'linecons-database');
			 var forms  	 	 = this.addItem('Forms', 			'/app/forms', 				'linecons-params');
			 var extra  	 	 = this.addItem('Extra', 			'/app/extra', 				'linecons-beaker').setLabel('New Items', 'purple');
			 var charts  	 = this.addItem('Charts', 			'/app/charts', 				'linecons-globe');
			 var menu_lvls  	 = this.addItem('Menu Levels', 		'', 						'linecons-cloud');


			 // Subitems of Dashboard
			 dashboard.addItem('Dashboard 1', 	'-/variant-1'); // "-/" will append parents link
			 dashboard.addItem('Dashboard 2', 	'-/variant-2');
			 dashboard.addItem('Dashboard 3', 	'-/variant-3');
			 dashboard.addItem('Dashboard 4', 	'-/variant-4');
			 dashboard.addItem('Update Hightlights', '/app/update-highlights').setLabel('v1.3', 'pink');


			 // Subitems of UI Elements
			 ui_elements.addItem('Panels', 				'-/panels');
			 ui_elements.addItem('Buttons', 				'-/buttons');
			 ui_elements.addItem('Tabs & Accordions', 	'-/tabs-accordions');
			 ui_elements.addItem('Modals', 				'-/modals');
			 ui_elements.addItem('Breadcrumbs', 			'-/breadcrumbs');
			 ui_elements.addItem('Blockquotes', 			'-/blockquotes');
			 ui_elements.addItem('Progress Bars', 		'-/progress-bars');
			 ui_elements.addItem('Navbars', 				'-/navbars');
			 ui_elements.addItem('Alerts', 				'-/alerts');
			 ui_elements.addItem('Pagination', 			'-/pagination');
			 ui_elements.addItem('Typography', 			'-/typography');
			 ui_elements.addItem('Other Elements', 		'-/other-elements');


			 // Subitems of Mailbox
			 mailbox.addItem('Inbox', 			'-/inbox');
			 mailbox.addItem('Compose Message', 	'-/compose');
			 mailbox.addItem('View Message', 	'-/message');


			 // Subitems of Tables
			 tables.addItem('Basic Tables',		'-/basic');
			 tables.addItem('Responsive Tables',	'-/responsive');
			 tables.addItem('Data Tables',		'-/datatables');


			 // Subitems of Forms
			 forms.addItem('Native Elements',		'-/native');
			 forms.addItem('Advanced Plugins',		'-/advanced');
			 forms.addItem('Form Wizard',			'-/wizard');
			 forms.addItem('Form Validation',		'-/validation');
			 forms.addItem('Input Masks',			'-/input-masks');
			 forms.addItem('File Upload',			'-/file-upload');
			 forms.addItem('Editors',				'-/wysiwyg');
			 forms.addItem('Sliders',				'-/sliders');


			 // Subitems of Extra
			 var extra_icons = extra.addItem('Icons', 	'-/icons');
			 var extra_maps  = extra.addItem('Maps', 	'-/maps');
			 var members 	= extra.addItem('Members', 	'-/members').setLabel('New', 'warning');
			 extra.addItem('Gallery', 					'-/gallery');
			 extra.addItem('Calendar', 					'-/calendar');
			 extra.addItem('Profile', 					'-/profile');
			 extra.addItem('Login', 						'/login');
			 extra.addItem('Lockscreen', 				'/lockscreen');
			 extra.addItem('Login Light', 				'/login-light');
			 extra.addItem('Timeline', 					'-/timeline');
			 extra.addItem('Timeline Centered', 			'-/timeline-centered');
			 extra.addItem('Notes', 						'-/notes');
			 extra.addItem('Image Crop', 				'-/image-crop');
			 extra.addItem('Portlets', 					'-/portlets');
			 extra.addItem('Blank Page', 				'-/blank-page');
			 extra.addItem('Search', 					'-/search');
			 extra.addItem('Invoice', 					'-/invoice');
			 extra.addItem('404 Page', 					'-/page-404');
			 extra.addItem('Tocify', 					'-/tocify');
			 extra.addItem('Loading Progress', 			'-/loading-progress');
			 //extra.addItem('Page Loading Overlay', 		'-/page-loading-overlay'); NOT SUPPORTED IN ANGULAR
			 extra.addItem('Notifications', 				'-/notifications');
			 extra.addItem('Nestable Lists', 			'-/nestable-lists');
			 extra.addItem('Scrollable', 				'-/scrollable');

			 // Submenu of Extra/Icons
			 extra_icons.addItem('Font Awesome', 	'-/font-awesome');
			 extra_icons.addItem('Linecons', 		'-/linecons');
			 extra_icons.addItem('Elusive', 			'-/elusive');
			 extra_icons.addItem('Meteocons', 		'-/meteocons');

			 // Submenu of Extra/Maps
			 extra_maps.addItem('Google Maps', 		'-/google');
			 extra_maps.addItem('Advanced Map', 		'-/advanced');
			 extra_maps.addItem('Vector Map', 		'-/vector');

			 // Submenu of Members
			 members.addItem('Members List', '-/list');
			 members.addItem('Add Member', '-/add');


			 // Subitems of Charts
			 charts.addItem('Chart Variants', 		'-/variants');
			 charts.addItem('Range Selector', 		'-/range-selector');
			 charts.addItem('Sparklines', 			'-/sparklines');
			 charts.addItem('Map Charts', 			'-/map-charts');
			 charts.addItem('Circular Gauges', 		'-/gauges');
			 charts.addItem('Bar Gauges', 			'-/bar-gauges');



			 // Subitems of Menu Levels
			 var menu_lvl1 = menu_lvls.addItem('Menu Item 1.1');  // has to be referenced to add sub menu elements
			 menu_lvls.addItem('Menu Item 1.2');
			 menu_lvls.addItem('Menu Item 1.3');

			 // Sub Level 2
			 menu_lvl1.addItem('Menu Item 2.1');
			 var menu_lvl2 = menu_lvl1.addItem('Menu Item 2.2'); // has to be referenced to add sub menu elements
			 menu_lvl1.addItem('Menu Item 2.3');

			 // Sub Level 3
			 menu_lvl2.addItem('Menu Item 3.1');
			 menu_lvl2.addItem('Menu Item 3.2');

			 */


			return this;
		};

		this.prepareHorizontalMenu = function()
		{
			var wbs    = this.addItem('WBS', 		'/app/wbs', 			'linecons-database');
			//var cost_gantt    = this.addItem('Cost Gantt', 		'/app/cost-gantt', 			'linecons-note');
			var project_location    = this.addItem('Project Location', 		'/app/project-location', 			'linecons-globe');
			var const_timeline = this.addItem('Construction Timeline', 		'/app/const-timeline', 			'linecons-params');
			if (localStorageService.get('authorizationData').role === "Admin")
			{
				var admin    = this.addItem('Administration', 		'/app/admin', 			'linecons-note');

				admin.addItem('Access Control', 	'-/access-control'); // "-/" will append parents link
				admin.addItem('Budget Categories', 	'-/budget-categories'); // "-/" will append parents link
				admin.addItem('Positions', 	'-/positions'); // "-/" will append parents link
				// admin.addItem('Statuses', 	'-/statuses'); // "-/" will append parents link
				admin.addItem('Phase Code', '-/phase-code');
				admin.addItem('Fund Type', '-/fund-type');
				admin.addItem('Approval Matrix', '-/approval-matrix');
				admin.addItem('Organization','-/organization');
				admin.addItem('Asset Manager','-/asset-manager');
			}
			//var signup = this.addItem('Signup', '/app/test/signup','linecons-star');
			//var login = this.addItem('login','/app/test/login','linecons-note');
			/*

			 var layouts      = this.addItem('Layout',			'/app/layout-and-skins',	'linecons-desktop');
			 var ui_elements  = this.addItem('UI Elements', 		'/app/ui', 					'linecons-note');
			 var forms  	 	 = this.addItem('Forms', 			'/app/forms', 				'linecons-params');
			 var other  	 	 = this.addItem('Other', 			'/app/extra', 				'linecons-beaker');


			 // Subitems of Dashboard
			 dashboard.addItem('Dashboard 1', 	'-/variant-1'); // "-/" will append parents link
			 dashboard.addItem('Dashboard 2', 	'-/variant-2');
			 dashboard.addItem('Dashboard 3', 	'-/variant-3');
			 dashboard.addItem('Dashboard 4', 	'-/variant-4');


			 // Subitems of UI Elements
			 ui_elements.addItem('Panels', 				'-/panels');
			 ui_elements.addItem('Buttons', 				'-/buttons');
			 ui_elements.addItem('Tabs & Accordions', 	'-/tabs-accordions');
			 ui_elements.addItem('Modals', 				'-/modals');
			 ui_elements.addItem('Breadcrumbs', 			'-/breadcrumbs');
			 ui_elements.addItem('Blockquotes', 			'-/blockquotes');
			 ui_elements.addItem('Progress Bars', 		'-/progress-bars');
			 ui_elements.addItem('Navbars', 				'-/navbars');
			 ui_elements.addItem('Alerts', 				'-/alerts');
			 ui_elements.addItem('Pagination', 			'-/pagination');
			 ui_elements.addItem('Typography', 			'-/typography');
			 ui_elements.addItem('Other Elements', 		'-/other-elements');


			 // Subitems of Forms
			 forms.addItem('Native Elements',		'-/native');
			 forms.addItem('Advanced Plugins',		'-/advanced');
			 forms.addItem('Form Wizard',			'-/wizard');
			 forms.addItem('Form Validation',		'-/validation');
			 forms.addItem('Input Masks',			'-/input-masks');
			 forms.addItem('File Upload',			'-/file-upload');
			 forms.addItem('Editors',				'-/wysiwyg');
			 forms.addItem('Sliders',				'-/sliders');


			 // Subitems of Others
			 var widgets     = other.addItem('Widgets', 			'/app/widgets', 			'linecons-star');
			 var mailbox     = other.addItem('Mailbox', 			'/app/mailbox', 			'linecons-mail').setLabel('5', 'secondary', false);
			 var tables      = other.addItem('Tables', 			'/app/tables', 				'linecons-database');
			 var extra       = other.addItem('Extra', 			'/app/extra', 				'linecons-beaker').setLabel('New Items', 'purple');
			 var charts      = other.addItem('Charts', 			'/app/charts', 				'linecons-globe');
			 var menu_lvls   = other.addItem('Menu Levels', 		'', 						'linecons-cloud');


			 // Subitems of Mailbox
			 mailbox.addItem('Inbox', 			'-/inbox');
			 mailbox.addItem('Compose Message', 	'-/compose');
			 mailbox.addItem('View Message', 	'-/message');


			 // Subitems of Tables
			 tables.addItem('Basic Tables',		'-/basic');
			 tables.addItem('Responsive Tables',	'-/responsive');
			 tables.addItem('Data Tables',		'-/datatables');


			 // Subitems of Extra
			 var extra_icons = extra.addItem('Icons', 	'-/icons').setLabel(4, 'warning');
			 var extra_maps  = extra.addItem('Maps', 	'-/maps');
			 extra.addItem('Gallery', 					'-/gallery');
			 extra.addItem('Calendar', 					'-/calendar');
			 extra.addItem('Profile', 					'-/profile');
			 extra.addItem('Login', 						'/login');
			 extra.addItem('Lockscreen', 				'/lockscreen');
			 extra.addItem('Login Light', 				'/login-light');
			 extra.addItem('Timeline', 					'-/timeline');
			 extra.addItem('Timeline Centered', 			'-/timeline-centered');
			 extra.addItem('Notes', 						'-/notes');
			 extra.addItem('Image Crop', 				'-/image-crop');
			 extra.addItem('Portlets', 					'-/portlets');
			 extra.addItem('Blank Page', 				'-/blank-page');
			 extra.addItem('Search', 					'-/search');
			 extra.addItem('Invoice', 					'-/invoice');
			 extra.addItem('404 Page', 					'-/page-404');
			 extra.addItem('Tocify', 					'-/tocify');
			 extra.addItem('Loading Progress', 			'-/loading-progress');
			 //extra.addItem('Page Loading Overlay', 		'-/page-loading-overlay'); NOT SUPPORTED IN ANGULAR
			 extra.addItem('Notifications', 				'-/notifications');
			 extra.addItem('Nestable Lists', 			'-/nestable-lists');
			 extra.addItem('Scrollable', 				'-/scrollable');

			 // Submenu of Extra/Icons
			 extra_icons.addItem('Font Awesome', 	'-/font-awesome');
			 extra_icons.addItem('Linecons', 		'-/linecons');
			 extra_icons.addItem('Elusive', 			'-/elusive');
			 extra_icons.addItem('Meteocons', 		'-/meteocons');

			 // Submenu of Extra/Maps
			 extra_maps.addItem('Google Maps', 		'-/google');
			 extra_maps.addItem('Advanced Map', 		'-/advanced');
			 extra_maps.addItem('Vector Map', 		'-/vector');


			 // Subitems of Charts
			 charts.addItem('Chart Variants', 		'-/variants');
			 charts.addItem('Range Selector', 		'-/range-selector');
			 charts.addItem('Sparklines', 			'-/sparklines');
			 charts.addItem('Map Charts', 			'-/map-charts');
			 charts.addItem('Circular Gauges', 		'-/gauges');
			 charts.addItem('Bar Gauges', 			'-/bar-gauges');



			 // Subitems of Menu Levels
			 var menu_lvl1 = menu_lvls.addItem('Menu Item 1.1');  // has to be referenced to add sub menu elements
			 menu_lvls.addItem('Menu Item 1.2');
			 menu_lvls.addItem('Menu Item 1.3');

			 // Sub Level 2
			 menu_lvl1.addItem('Menu Item 2.1');
			 var menu_lvl2 = menu_lvl1.addItem('Menu Item 2.2'); // has to be referenced to add sub menu elements
			 menu_lvl1.addItem('Menu Item 2.3');

			 // Sub Level 3
			 menu_lvl2.addItem('Menu Item 3.1');
			 menu_lvl2.addItem('Menu Item 3.2');
			 */

			return this;
		};

		this.instantiate = function()
		{
			return angular.copy( this );
		};

		this.toStatePath = function(path)
		{
			return path.replace(/\//g, '.').replace(/^\./, '');
		};

		this.setActive = function(path)
		{
			this.iterateCheck(this.menuItems, this.toStatePath(path));
		};

		this.setActiveParent = function(item)
		{
			item.isActive = true;
			item.isOpen = true;

			if(item.parent)
				this.setActiveParent(item.parent);
		};

		this.iterateCheck = function(menuItems, currentState)
		{
			angular.forEach(menuItems, function(item)
			{
				if(item.state == currentState)
				{
					item.isActive = true;

					if(item.parent != null)
						$menuItemsRef.setActiveParent(item.parent);
				}
				else
				{
					item.isActive = false;
					item.isOpen = false;

					if(item.menuItems.length)
					{
						$menuItemsRef.iterateCheck(item.menuItems, currentState);
					}
				}
			});
		};
	}]);
