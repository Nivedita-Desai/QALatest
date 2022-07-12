//var treedata ={"name": "MyClass", "children": [{"name": "Topic 1", "children": [{"name": "Subtopic 1"}, {"name": "Subtopic 2"}, {"name": "Subtopic 3"}]}, {"name": "Topic 2", "children": [{"name": "Subtopic 4"}, {"name": "Subtopic 5"}, {"name": "Subtopic 6"}]}, {"name": "Topic 3", "children": [{"name": "Subtopic 7"}, {"name": "Subtopic 8"}, {"name": "Subtopic 9"}]}]}


d3.chart = d3.chart || {};

d3.chart.architectureTree = function ()
{
    var m = [20, 20, 20, 20],
        w = 800 - m[1] - m[3],
        h = 600 - m[0] - m[2],
        i = 0,
        r = 800,
        x = d3.scale.linear().domain([0, w]).range([0, w]),
        y = d3.scale.linear().domain([0, h]).range([0, h]),
        root, treedata, vis, tree, diagonal;

    function chart() {
        vis = d3.select("#tree").append("svg:svg")
                  .attr("viewBox", "0 0 600 600")
                  .attr("width", w + m[1] + m[3])
                  .attr("height", h + m[0] + m[2])
                  .append("svg:g")
                  //.attr("pointer-events", "all")
                  .attr("transform", "translate(" + m[3] + "," + m[0] + ")")
                  //.call(d3.behavior.zoom().scaleExtent([1,8]).on("zoom",zoom));
                  .call(d3.behavior.zoom().x(x).y(y).scaleExtent([1, 8]).on("zoom", zoom));

        vis.append("rect")
          .attr("class", "overlay")
          .attr("width", w + m[1] + m[3])
          .attr("height", h + m[0] + m[2])
          .attr("opacity", 0);

        tree = d3.layout.tree()
          .size([h, w]);

        var diagonal = d3.svg.diagonal()
          .projection(function (d) { return [d.y, d.x]; });
    }

    var nodes = tree.nodes(treeData),
        links = tree.links(nodes);
    activeNode = null;
    svg.call(updateData, nodes, links);


    var updateData = function (container, nodes, links) {

        // Enrich data
        addDependents(nodes);
        nodes.map(function (node) {
            addIndex(node);
        });

        var diagonal = d3.svg.diagonal.radial()
            .projection(function (d) { return [d.y, d.x / 180 * Math.PI]; });

        var linkSelection = svg.selectAll(".link").data(links, function (d) {
            return d.source.name + d.target.name + Math.random();
        });
        linkSelection.exit().remove();

        linkSelection.enter().append("path")
            .attr("class", "link")
            .attr("d", diagonal);

        var nodeSelection = container.selectAll(".node").data(nodes, function (d) {
            return d.name + Math.random();  // always update node
        });
        nodeSelection.exit().remove();

        var node = nodeSelection.enter().append("g")
            .attr("class", "node")
            .attr("transform", function (d) { return "rotate(" + (d.x - 90) + ")translate(" + d.y + ")"; })
            .on('mouseover', function (d) {
                if (activeNode !== null) {
                    return;
                }
                fade(0.1)(d);
                document.querySelector('#panel').dispatchEvent(
                    new CustomEvent("hoverNode", { "detail": d.name })
                );
            })
            .on('mouseout', function (d) {
                if (activeNode !== null) {
                    return;
                }
                fade(1)(d);
            })
            .on('click', function (d) {
                select(d.name);
            });

        node.append("circle")
            .attr("r", function (d) { return 4.5 * (d.size || 1); })
            .style('stroke', function (d) {
                return d3.scale.linear()
                    .domain([1, 0])
                    .range(["steelblue", "red"])(typeof d.satisfaction !== "undefined" ? d.satisfaction : 1);
            })
            .style('fill', function (d) {
                if (typeof d.satisfaction === "undefined") return '#fff';
                return d3.scale.linear()
                    .domain([1, 0])
                    .range(["white", "#f66"])(typeof d.satisfaction !== "undefined" ? d.satisfaction : 1);
            });

        node.append("text")
            .attr("dy", ".31em")
            .attr("text-anchor", function (d) { return d.x < 180 ? "start" : "end"; })
            .attr("transform", function (d) { return d.x < 180 ? "translate(8)" : "rotate(180)translate(-8)"; })
            .text(function (d) {
                return d.name;
            });
    };

    var addDependents = function (nodes) {
        var dependents = [];
        nodes.forEach(function (node) {
            if (node.dependsOn) {
                node.dependsOn.forEach(function (dependsOn) {
                    if (!dependents[dependsOn]) {
                        dependents[dependsOn] = [];
                    }
                    dependents[dependsOn].push(node.name);
                });
            }
        });
        nodes.forEach(function (node, index) {
            if (dependents[node.name]) {
                nodes[index].dependents = dependents[node.name];
            }
        });
    };

    
    var addIndex = function (node) {
        node.index = {
            relatedNodes: [],
            technos: [],
            hosts: []
        };
        var dependsOn = getDetailCascade(node, 'dependsOn');
        if (dependsOn.length > 0) {
            node.index.relatedNodes = node.index.relatedNodes.concat(dependsOn);
        }
        if (node.dependents) {
            node.index.relatedNodes = node.index.relatedNodes.concat(node.dependents);
        }
        var technos = getDetailCascade(node, 'technos');
        if (technos.length > 0) {
            node.index.technos = technos;
        }
        var hosts = getHostsCascade(node);
        if (hosts.length > 0) {
            node.index.hosts = hosts;
        }
    };
    var getDetailCascade = function (node, detailName) {
        var values = [];
        if (node[detailName]) {
            node[detailName].forEach(function (value) {
                values.push(value);
            });
        }
        if (node.parent) {
            values = values.concat(getDetailCascade(node.parent, detailName));
        }
        return values;
    };
    var getHostsCascade = function (node) {
        var values = [];
        if (node.host) {
            for (var i in node.host) {
                values.push(i);
            }
        }
        if (node.parent) {
            values = values.concat(getHostsCascade(node.parent));
        }
        return values;
    };

    var fade = function (opacity) {
        return function (node) {
            //if (!node.dependsOn || !(node.parent && node.parent.dependsOn)) return;
            svg.selectAll(".node")
                .filter(function (d) {
                    if (d.name === node.name) return false;
                    return node.index.relatedNodes.indexOf(d.name) === -1;
                })
                .transition()
                .style("opacity", opacity);
        };
    };

    var filters = {
        name: '',
        technos: [],
        hosts: []
    };

    var isFoundByFilter = function (d) {
        var i;
        if (!filters.name && !filters.technos.length && !filters.hosts.length) {
            // nothing selected
            return true;
        }
        if (filters.name) {
            if (d.name.toLowerCase().indexOf(filters.name) === -1) return false;
        }
        var technosCount = filters.technos.length;
        if (technosCount) {
            if (d.index.technos.length === 0) return false;
            for (i = 0; i < technosCount; i++) {
                if (d.index.technos.indexOf(filters.technos[i]) === -1) return false;
            }
        }
        var hostCount = filters.hosts.length;
        if (hostCount) {
            if (d.index.hosts.length === 0) return false;
            for (i = 0; i < hostCount; i++) {
                if (d.index.hosts.indexOf(filters.hosts[i]) === -1) return false;
            }
        }
        return true;
    };

    var refreshFilters = function () {
        d3.selectAll('.node').classed('notFound', function (d) {
            return !isFoundByFilter(d);
        });
    };

    var select = function (name) {
        if (activeNode && activeNode.name == name) {
            unselect();
            return;
        }
        unselect();
        svg.selectAll(".node")
            .filter(function (d) {
                if (d.name === name) return true;
            })
            .each(function (d) {
                document.querySelector('#panel').dispatchEvent(
                    new CustomEvent("selectNode", { "detail": d.name })
                );
                d3.select(this).attr("id", "node-active");
                activeNode = d;
                fade(0.1)(d);
            });
    };

    var unselect = function () {
        if (activeNode == null) return;
        fade(1)(activeNode);
        d3.select('#node-active').attr("id", null);
        activeNode = null;
        document.querySelector('#panel').dispatchEvent(
            new CustomEvent("unSelectNode")
        );
    };

    chart.select = select;
    chart.unselect = unselect;

    chart.data = function (value) {
        if (!arguments.length) return treeData;
        treeData = value;
        return chart;
    };

    chart.diameter = function (value) {
        if (!arguments.length) return diameter;
        diameter = value;
        return chart;
    };

    chart.nameFilter = function (nameFilter) {
        filters.name = nameFilter;
        refreshFilters();
    };

    chart.technosFilter = function (technosFilter) {
        filters.technos = technosFilter;
        refreshFilters();
    };

    chart.hostsFilter = function (hostsFilter) {
        filters.hosts = hostsFilter;
        refreshFilters();
    };


    function toggleAll(d) {
        if (d.children) {
            d.children.forEach(toggleAll);
            toggle(d);
        }
    };
    console.log(root)

    // initialize the display to show a few nodes.
    root.children.forEach(toggleAll);
    //toggle(root.children[1]);
    //toggle(root.children[9]);

    //update(root);

    function update(source) {
        var duration = d3.event && d3.event.altKey ? 5000 : 500;

        // Compute the new tree layout.
        var nodes = tree.nodes(root).reverse();

        // Normalize for fixed-depth.
        nodes.forEach(function (d) { d.y = d.depth * 180; });

        // Update the nodes...
        var node = vis.selectAll("g.node")
            .data(nodes, function (d) { return d.id || (d.id = ++i); });

        // Enter any new nodes at the parent's previous position.
        var nodeEnter = node.enter().append("svg:g")
            .attr("class", "node")
            .attr("transform", function (d) { return "translate(" + source.y0 + "," + source.x0 + ")"; })
            .on("click", function (d) {
                toggle(d);
                update(d);
                if (d['info']) {
                    playvid(d['info']);
                }
            });

        nodeEnter.append("svg:circle")
            .attr("r", 1e-6)
            .style("fill", function (d) { return d._children ? "lightsteelblue" : "#fff"; });

        nodeEnter.append("svg:text")
            //.attr("y", function(d) { return d.children || d._children ? -10 : 10; })
            //.attr("dx", ".35em")
            .attr("x", function (d) { return d.children || d._children ? -10 : 10; })
            .attr("dy", ".35em")
            .attr("text-anchor", function (d) { return d.children || d._children ? "end" : "start"; })
            .text(function (d) { return d.name; })
            .style("fill-opacity", 1e-6);

        // Transition nodes to their new position.
        var nodeUpdate = node.transition()
            .duration(duration)
            .attr("transform", function (d) { return "translate(" + d.y + "," + d.x + ")"; });

        nodeUpdate.select("circle")
            .attr("r", 4.5)
            .style("fill", function (d) { return d._children ? "lightsteelblue" : "#fff"; });

        nodeUpdate.select("text")
            .style("fill-opacity", 1);

        // Transition exiting ndoes to the parent's new position.
        var nodeExit = node.exit().transition()
            .duration(duration)
            .attr("transform", function (d) { return "translate(" + source.y + "," + source.x + ")"; })
            .remove();

        nodeExit.select("circle")
            .attr("r", 1e-6);
        nodeExit.select("text")
            .style("fill-opacity", 1e-6);

        // Update the links...
        var link = vis.selectAll("path.link")
            .data(tree.links(nodes), function (d) { return d.target.id; });

        // Enter any new links at hte parent's previous position
        link.enter().insert("svg:path", "g")
            .attr("class", "link")
            .attr("d", function (d) {
                var o = { x: source.x0, y: source.y0 };
                return diagonal({ source: o, target: o });
            })
            .transition()
              .duration(duration)
              .attr("d", diagonal);

        // Transition links to their new position.
        link.transition()
            .duration(duration)
            .attr("d", diagonal);

        // Transition exiting nodes to the parent's new position.
        link.exit().transition()
            .duration(duration)
            .attr("d", function (d) {
                var o = { x: source.x, y: source.y };
                return diagonal({ source: o, target: o });
            })
            .remove();

        // Stash the old positions for transition.
        nodes.forEach(function (d) {
            d.x0 = d.x;
            d.y0 = d.y;
        });
    }
    // Toggle children
    function toggle(d) {
        if (d.children) {
            d._children = d.children;
            d.children = null;
        }
        else {
            d.children = d._children;
            d._children = null;
        }
    }

    // zoom in / out
    function zoom(d) {
        //vis.attr("transform", "translate(" + d3.event.translate + ")scale(" + d3.event.scale + ")");
        var nodes = vis.selectAll("g.node");
        nodes.attr("transform", transform);

        // Update the links...
        var link = vis.selectAll("path.link");
        link.attr("d", translate);

        // Enter any new links at hte parent's previous position
        //link.attr("d", function(d) {
        //      var o = {x: d.x0, y: d.y0};
        //      return diagonal({source: o, target: o});
        //    });
    }

    function transform(d) {
        return "translate(" + x(d.y) + "," + y(d.x) + ")";
    }

    function translate(d) {
        var sourceX = x(d.target.parent.y);
        var sourceY = y(d.target.parent.x);
        var targetX = x(d.target.y);
        var targetY = (sourceX + targetX) / 2;
        var linkTargetY = y(d.target.x0);
        var result = "M" + sourceX + "," + sourceY + " C" + targetX + "," + sourceY + " " + targetY + "," + y(d.target.x0) + " " + targetX + "," + linkTargetY + "";
        //console.log(result);

        return result;
    }
    return chart;
};