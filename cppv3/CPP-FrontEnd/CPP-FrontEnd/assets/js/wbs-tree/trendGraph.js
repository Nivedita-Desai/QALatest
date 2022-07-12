/**
 * Created by nvasa on 4/23/2015.
 */
// Create a new directed graph
graphJSON = d3.json("http://192.168.0.19:1832/Request/WBS", function(error, treeData) {
    var g = new dagreD3.graphlib.Graph().setGraph({});

// States and transitions from RFC 793
    var states = ["Trend 0", "Trend 1", "Trend 2", "Trend 3",
        "Trend 4", "Trend 5", "Current Project - Reporting Server", "Trend 7",
        "Trend 8", "Trend 9", "Forecast Project - Reporting Server"];

// Automatically label each of the nodes
    states.forEach(function (state) {
        g.setNode(state, {label: state});
    });

// Set up the edges
    g.setEdge("Trend 0", "Current Project - Reporting Server", {label: "Approved", lineInterpolate: 'basis', arrrowhead: 'vee'});
    g.setEdge("Trend 1", "Current Project - Reporting Server", {label: "Approved", lineInterpolate: 'basis', arrrowhead: 'vee'});
    g.setEdge("Trend 2", "Current Project - Reporting Server", {label: "Approved", lineInterpolate: 'basis', arrrowhead: 'vee'});
    g.setEdge("Trend 3", "Current Project - Reporting Server", {label: "Rejected", lineInterpolate: 'basis', arrrowhead: 'vee'});
    g.setEdge("Trend 4", "Current Project - Reporting Server", {label: "Rejected", lineInterpolate: 'basis', arrrowhead: 'vee'});
    g.setEdge("Trend 5", "Current Project - Reporting Server", {label: "Approved", lineInterpolate: 'basis', arrrowhead: 'vee'});
    g.setEdge("Current Project - Reporting Server", "Trend 7", {label: "$1000, 20 days", lineInterpolate: 'basis', arrrowhead: 'vee'});
    g.setEdge("Current Project - Reporting Server", "Trend 8", {label: "-$200, -5 days", lineInterpolate: 'basis', arrrowhead: 'vee'});
    g.setEdge("Current Project - Reporting Server", "Trend 9", {label: "$20500, 20 days", lineInterpolate: 'basis', arrrowhead: 'vee'});
    g.setEdge("Trend 7", "Forecast Project - Reporting Server", {label: "Pending: Adam", lineInterpolate: 'basis', arrrowhead: 'vee'});
    g.setEdge("Trend 8", "Forecast Project - Reporting Server", {label: "Pending: Betty", lineInterpolate: 'basis', arrrowhead: 'vee'});
    g.setEdge("Trend 9", "Forecast Project - Reporting Server", { lineInterpolate: 'basis', arrrowhead: 'vee' });

    g.setNode("Trend 0", {shape: "iconA"});
    g.setNode("Trend 1", {shape: "iconA"});
    g.setNode("Trend 2", {shape: "iconA"});
    g.setNode("Trend 3", {shape: "iconR"});
    g.setNode("Trend 4", {shape: "iconR"});
    g.setNode("Trend 5", {shape: "iconA"});
    g.setNode("Trend 7", {shape: "iconP"});
    g.setNode("Trend 8", {shape: "iconP"});
    g.setNode("Trend 9", {shape: "iconP"});
    g.setNode("Current Project - Reporting Server", {shape: "rect"});
    g.setNode("Forecast Project - Reporting Server", {shape: "diamond"});




// Set some general styles
    g.nodes().forEach(function (v) {
        var node = g.node(v);
        node.rx = node.ry = 5;
    });

// Add some custom colors based on state
    g.node('Current Project - Reporting Server').style = "fill: #A7E8E8";
    g.node('Forecast Project - Reporting Server').style = "fill: #E8A7A7";

    //var svg = d3.select("svg");



var svg = d3.select("#trend-tree-container").select("svg");
    var inner = svg.selectAll("g");


// Set up zoom support
    var zoom = d3.behavior.zoom().on("zoom", function () {
        inner.attr("transform", "translate(" + d3.event.translate + ")" +
        "scale(" + d3.event.scale + ")");
    });
    svg.call(zoom);

// Create the renderer
    var render = new dagreD3.render();


    // Add our custom shape IconA
    render.shapes().iconA = function(parent, bbox, node) {
        var w = bbox.width,
            h = bbox.height,
            points = [
                { x:   15, y:        0 },
                { x:   30, y:        0 },
                { x:   30, y:       30 },
                //{ x: w/2, y: -h * 3/2 },
                { x:   0, y:       30 }
            ];


        shapeSvg = parent.append("image")
            .attr("xlink:href", function(d)
            {
               return "assets/js/wbs-tree/images/nodeB.png"
            })
            .attr("x", "-7px")
            .attr("y", "15px")
            .attr("width", "15px")
            .attr("height", "15px");


        node.intersect = function(point) {
            return dagreD3.intersect.polygon(node, points, point);
        };

        return shapeSvg;
    };

    // Add our custom shape IconP
    render.shapes().iconP = function(parent, bbox, node) {
        var w = bbox.width,
            h = bbox.height,
            points = [
                { x:   15, y:        0 },
                { x:   30, y:        0 },
                { x:   30, y:       30 },
                //{ x: w/2, y: -h * 3/2 },
                { x:   0, y:       30 }
            ];


        shapeSvg = parent.append("image")
            .attr("xlink:href", function(d)
            {
                return "assets/js/wbs-tree/images/nodeC.png"
            })
            .attr("x", "-7px")
            .attr("y", "15px")
            .attr("width", "15px")
            .attr("height", "15px");


        node.intersect = function(point) {
            return dagreD3.intersect.polygon(node, points, point);
        };

        return shapeSvg;
    };


    // Add our custom shape IconR
    render.shapes().iconR = function(parent, bbox, node) {
        var w = bbox.width,
            h = bbox.height,
            points = [
                { x:   15, y:        0 },
                { x:   30, y:        0 },
                { x:   30, y:       30 },
                //{ x: w/2, y: -h * 3/2 },
                { x:   0, y:       30 }
            ];


        shapeSvg = parent.append("image")
            .attr("xlink:href", function(d)
            {
                return "assets/js/wbs-tree/images/nodeD.png"
            })
            .attr("x", "-7px")
            .attr("y", "15px")
            .attr("width", "15px")
            .attr("height", "15px");


        node.intersect = function(point) {
            return dagreD3.intersect.polygon(node, points, point);
        };

        return shapeSvg;
    };



// Run the renderer. This is what draws the final graph.
    render(inner, g);

// Center the graph
    var initialScale = 1;
    zoom
        .translate([(svg.attr("width") - g.graph().width * initialScale)/3, 20])
        .scale(initialScale)
        .event(svg);
    svg.attr('height', g.graph().height * initialScale + 240);

});