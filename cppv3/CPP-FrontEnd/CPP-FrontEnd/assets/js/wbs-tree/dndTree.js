/**
 * Created by nvasa on 3/5/2015.
 */
// Get JSON data
var selectedNode = null;



treeJSON = d3.json("http://192.168.0.19:1832/Request/WBS", function(error, treeData) {
//treeJSON = d3.json("data.json", function(error, treeData) {

    // Calculate total nodes, max label length
    var totalNodes = 0;
    var maxLabelLength = 0;
    // variables for drag/drop

    var draggingNode = null;
    // panning variables
    var panSpeed = 200;
    var panBoundary = 20; // Within 20px from edges will pan when dragging.
    // Misc. variables
    var i = 0;
    var duration = 750;
    var root;
    //var selectedNode;
    // size of the diagram
    var viewerWidth = $("#program_navigation").width();
    var viewerHeight = $(document).height();

    var tree = d3.layout.tree()
        .size([viewerHeight, viewerWidth]);

    // define a d3 diagonal projection for use by the node paths later on.
    var diagonal = d3.svg.diagonal()
        .projection(function(d) {
            return [d.y, d.x];
        });

    // A recursive helper function for performing some setup by walking through all nodes

    function visit(parent, visitFn, childrenFn) {
        if (!parent) return;

        visitFn(parent);

        var children = childrenFn(parent);
        if (children) {
            var count = children.length;
            for (var i = 0; i < count; i++) {
                visit(children[i], visitFn, childrenFn);
            }
        }
    }



    // Call visit function to establish maxLabelLength
    visit(treeData, function(d) {
        totalNodes++;
        //maxLabelLength = Math.max(d.name.length, maxLabelLength);
        maxLabelLength = 25;

    }, function(d) {
        return d.children && d.children.length > 0 ? d.children : null;
    });


    // sort the tree according to the node names

    function sortTree() {
        tree.sort(function(a, b) {
            return b.name.toLowerCase() < a.name.toLowerCase() ? 1 : -1;
        });
    }
    // Sort the tree initially incase the JSON isn't in a sorted order.
    //sortTree();

    // TODO: Pan function, can be better implemented.

    function pan(domNode, direction) {
        d3.select("#toolTip").style("opacity", 0);
        var speed = panSpeed;
        if (panTimer) {
            clearTimeout(panTimer);
            translateCoords = d3.transform(svgGroup.attr("transform"));
            if (direction == 'left' || direction == 'right') {
                translateX = direction == 'left' ? translateCoords.translate[0] + speed : translateCoords.translate[0] - speed;
                translateY = translateCoords.translate[1];
            } else if (direction == 'up' || direction == 'down') {
                translateX = translateCoords.translate[0];
                translateY = direction == 'up' ? translateCoords.translate[1] + speed : translateCoords.translate[1] - speed;
            }
            scaleX = translateCoords.scale[0];
            scaleY = translateCoords.scale[1];
            scale = zoomListener.scale();
            svgGroup.transition().attr("transform", "translate(" + translateX + "," + translateY + ")scale(" + scale + ")");
            d3.select(domNode).select('g.node').attr("transform", "translate(" + translateX + "," + translateY + ")");
            zoomListener.scale(zoomListener.scale());
            zoomListener.translate([translateX, translateY]);
            panTimer = setTimeout(function() {
                pan(domNode, speed, direction);
            }, 50);
        }
    }





    // Define the zoom function for the zoomable tree

    function zoom() {
        svgGroup.attr("transform", "translate(" + d3.event.translate + ")scale(" + d3.event.scale + ")");
    }



    // define the zoomListener which calls the zoom function on the "zoom" event constrained within the scaleExtents
    var zoomListener = d3.behavior.zoom().scaleExtent([0.1, 3]).on("zoom", zoom);



    // define the baseSvg, attaching a class for styling and the zoomListener
    var baseSvg = d3.select("#tree-container").append("svg")
        .attr("width", viewerWidth)
        .attr("height", viewerHeight)
        .attr("class", "overlay")
        .call(zoomListener)
        .on("dblclick.zoom", null);




    // Helper functions for collapsing and expanding nodes.

    function collapse(d) {
        if (d.children) {
            d._children = d.children;
            d._children.forEach(collapse);
            d.children = null;
        }
    }

    function expand(d) {
        if (d._children) {
            d.children = d._children;
            d.children.forEach(expand);
            d._children = null;
        }
    }

    // Function to center node when clicked/dropped so node doesn't get lost when collapsing/moving with large amount of children.

    function centerNode(source) {
        scale = zoomListener.scale();
        x = -source.y0;
        y = -source.x0;
        x = x * scale + viewerWidth / 2;
        y = y * scale + viewerHeight / 2;
        d3.select('g').transition()
            .duration(duration)
            .attr("transform", "translate(" +        x + "," + y + ")scale(" + scale + ")");
        zoomListener.scale(scale);
        zoomListener.translate([x, y]);
    }

    // Toggle children function
    function toggleChildren(d) {

            if (d.children)
            {
                d._children = d.children;
                d.children = null;
            }
            else
            {
                d.children = d._children;
                d._children = null;
            }



        return d;
    }

    // Toggle children on click.


    function click(d) {
        selectedNode = d;
        d = toggleChildren(d);
        update(d);
    }

    //var panelToggle;

    function rightclick(d) {


           selectedNode = d;
        if (d.level == "Program")
            programPanel(d);

        if (d.level == "ProgramElement")
            programElementPanel(d);

        if (d.level == "Project")
            projectPanel(d);


    }
    //DELETE NODE FUNCTION

    $('#delete_button').on('click', function () {
        // business logic...
        var parentNode = selectedNode.parent;
         //Find  index of selected node
         var i = parentNode.children.indexOf(selectedNode);
         //Remove selected child index from parent
         parentNode.children.splice(i,1);

        /*API CALL FOR DELETE HERE */
        /* if selectedNode.level == XYZ, call delete API for XYZ */

        tooltip = d3.select("#toolTip").style("opacity", 0);
        $('#DeleteModal').modal('hide');
        update(parentNode);
    })


    $('#new_program').on('click', function () {
        // business logic...
        newNode.name = modal.find('.modal-body #program_name').val()
        newNode.ProgramManager = modal.find('.modal-body #program_manager').val()
        newNode.Programponsor = modal.find('.modal-body #program_sponsor').val()


        //API to Insert Program Element
        $('#ProgramModal').modal('hide');
        selectedNode = selectedNode.parent;
        if (!selectedNode._children && !selectedNode.children) {//Empty parent
            selectedNode._children = [newNode];
            selectedNode = toggleChildren(selectedNode);
        }

        else if (selectedNode._children) { //Parent is collapsed
            selectedNode._children.push(newNode);
            selectedNode = toggleChildren(selectedNode);
        }

        else if (selectedNode.children) { //Parent is expanded
            selectedNode.children.push(newNode);

        }




        update(selectedNode);
        tooltip = d3.select("#toolTip").style("opacity", 0);
        //update(parentNode)
    })


    $('#update_program').on('click', function () {
        // business logic...
        selectedNode.name = modal.find('.modal-body #program_name').val()
        selectedNode.ProgramManager = modal.find('.modal-body #program_manager').val()
        selectedNode.Programponsor = modal.find('.modal-body #program_sponsor').val()
        //API to Insert/Update

        $('#ProgramModal').modal('hide');
        update(selectedNode);
        tooltip = d3.select("#toolTip").style("opacity", 0);

    })


    $('#new_program_element').on('click', function () {
        // business logic...
        newNode.name = modal.find('.modal-body #program_element_name').val()
        newNode.ProgramElementManager = modal.find('.modal-body #program_element_manager').val()
        newNode.ProgramElementSponsor = modal.find('.modal-body #program_element_sponsor').val()


        //API to Insert Program Element
        $('#ProgramElementModal').modal('hide');
        selectedNode = selectedNode.parent;
        if (!selectedNode._children && !selectedNode.children) {//Empty parent
            selectedNode._children = [newNode];
            selectedNode = toggleChildren(selectedNode);
        }

        else if (selectedNode._children) { //Parent is collapsed
            selectedNode._children.push(newNode);
            selectedNode = toggleChildren(selectedNode);
        }

        else if (selectedNode.children) { //Parent is expanded
            selectedNode.children.push(newNode);

        }




        update(selectedNode);
        tooltip = d3.select("#toolTip").style("opacity", 0);
        //update(parentNode)
    })


    $('#update_program_element').on('click', function () {
        // business logic...
        selectedNode.name = modal.find('.modal-body #program_element_name').val()
        selectedNode.ProgramElementManager = modal.find('.modal-body #program_element_manager').val()
        selectedNode.ProgramElementSponsor =  modal.find('.modal-body #program_element_sponsor').val()
         //API to Insert/Update
         $('#ProgramElementModal').modal('hide');
        update(selectedNode);
        tooltip = d3.select("#toolTip").style("opacity", 0);

    })




    $('#new_project').on('click', function () {
        // business logic...
        newNode.name = modal.find('.modal-body #project_name').val()
        newNode.ProjectManager = modal.find('.modal-body #project_manager').val()
        newNode.ProjectManager = modal.find('.modal-body #project_sponsor').val()


        //API to Insert Program Element
        $('#ProjectModal').modal('hide');
        selectedNode = selectedNode.parent;
        if (!selectedNode._children && !selectedNode.children) {//Empty parent
            selectedNode._children = [newNode];
            selectedNode = toggleChildren(selectedNode);
        }

        else if (selectedNode._children) { //Parent is collapsed
            selectedNode._children.push(newNode);
            selectedNode = toggleChildren(selectedNode);
        }

        else if (selectedNode.children) { //Parent is expanded
            selectedNode.children.push(newNode);

        }




        update(selectedNode);
        tooltip = d3.select("#toolTip").style("opacity", 0);
        //update(parentNode)
    })


    $('#update_project').on('click', function () {
        // business logic...
        selectedNode.name = modal.find('.modal-body #project_name').val()
        selectedNode.ProjectManager = modal.find('.modal-body #project_manager').val()
        selectedNode.ProjectSponsor =  modal.find('.modal-body #project_sponsor').val()
        //API to Insert/Update
        $('#ProjectModal').modal('hide');
        update(selectedNode);
        tooltip = d3.select("#toolTip").style("opacity", 0);

    })




    $('#ProgramModal').on('show.bs.modal', function (event) {

        //button = $(event.relatedTarget.id) // Button that triggered the modal

        modal = $(this)
        modal.find('.modal-title').text('Program: ' + selectedNode.name)
        modal.find('.modal-body #program_name').val(selectedNode.name)
        modal.find('.modal-body #program_manager').val(selectedNode.ProgramManager)
        modal.find('.modal-body #program_sponsor').val(selectedNode.ProgramSponsor)
        /*if (button.selector == "button_add")
        {
            modal.find('.modal-footer #new_program').css('opacity', 1)
            modal.find('.modal-footer #update_program').css('opacity', 0)
        }

        else
        {
            modal.find('.modal-footer #new_program').css('opacity', 0)
            modal.find('.modal-footer #update_program').css('opacity', 1)
        }*/
    })



    $('#ProgramElementModal').on('show.bs.modal', function (event) {

        //var button = $(event.relatedTarget.id) // Button that triggered the modal
        //var recipient = button.data('whatever') // Extract info from data-* attributes
        // If necessary, you could initiate an AJAX request here (and then do the updating in a callback).
        // Update the modal's content. We'll use jQuery here, but you could use a data binding library or other methods instead.
        modal = $(this)
        modal.find('.modal-title').text('Project: ' + selectedNode.name) //Original: Program Element:
        modal.find('.modal-body #program_element_name').val(selectedNode.name)
        modal.find('.modal-body #program_element_manager').val(selectedNode.ProgramElementManager)
        modal.find('.modal-body #program_element_sponsor').val(selectedNode.ProgramElementSponsor)
        /*if (button.selector == "button_add")
        {
            modal.find('.modal-footer #new_program_element').css('opacity', 1)
            modal.find('.modal-footer #update_program_element').css('opacity', 0)
        }

        else
        {
            modal.find('.modal-footer #new_program_element').css('opacity', 0)
            modal.find('.modal-footer #update_program_element').css('opacity', 1)
        }*/


    })




    $('#ProjectModal').on('show.bs.modal', function (event) {
        tooltip = d3.select("#toolTip").style("opacity", 0);
        //var button = $(event.relatedTarget.id) // Button that triggered the modal
        //var recipient = button.data('whatever') // Extract info from data-* attributes
        // If necessary, you could initiate an AJAX request here (and then do the updating in a callback).
        // Update the modal's content. We'll use jQuery here, but you could use a data binding library or other methods instead.
        modal = $(this)
        modal.find('.modal-title').text('Project: ' + selectedNode.name)
        modal.find('.modal-body #project_name').val(selectedNode.name)
        modal.find('.modal-body #project_manager').val(selectedNode.ProjectManager)
        modal.find('.modal-body #project_sponsor').val(selectedNode.ProjectSponsor)
        /*if (button.selector == "button_add")
        {
            modal.find('.modal-footer #new_project').css('opacity', 1)
            modal.find('.modal-footer #update_project').css('opacity', 0)
        }

        else
        {
            modal.find('.modal-footer #new_project').css('opacity', 0)
            modal.find('.modal-footer #update_project').css('opacity', 1)
        }*/
    })






    function rootPanel(d) {
        popup = d3.select("#ontop");
        heading_label = popup.append("div")
            .attr("id","heading")
            .text(d.name);

        add_button = popup.append("input").attr("type","button").attr("id","add_button").attr("value", "Add Program ")
            .on("click", function(){ //Add node
                var newNode = { name : "New Program"};
                newNode.ProgramID = "";
                newNode.ProgramManager = "";
                newNode.ProgramSponsor = "";
                newNode.CurrentCost = "0";
                newNode.ForecastCost = "0";
                newNode.level = "Program";



                //if (selectedNode._children) selectedNode._children.push(newNode); else selectedNode._children = [newNode];
                //Push new node

                if (!selectedNode._children && !selectedNode.children) {//Empty parent
                    selectedNode._children = [newNode];
                    selectedNode = toggleChildren(selectedNode);
                }

                else if (selectedNode._children) { //Parent is collapsed
                    selectedNode._children.push(newNode);
                    selectedNode = toggleChildren(selectedNode);
                }

                else if (selectedNode.children) { //Parent is expanded
                    selectedNode.children.push(newNode);

            }
                d3.select("#ontop").selectAll("*").remove();
                update(selectedNode);
                programPanel(newNode);
                selectedNode = newNode;

            });
    }

    function programPanel(d) {


        /*tooltip = d3.select("#toolTip");
        tooltip.style("left", (d3.event.pageX) + "px")
                .style("top", (d3.event.pageY) + "px")
                .style("opacity", 1);



        tooltip.selectAll("#head")//.enter()
            .text("Program");
        tooltip.selectAll("#header1")
            .text(d.name);
        tooltip.selectAll("#header2")
            .text("Manager: " + d.ProgramManager);


        costpane1 = tooltip.selectAll("#costpane1");
        costpane1.selectAll("#cost1")
            .text("$" + d.CurrentCost);

        costpanediff = tooltip.selectAll("#costpanediff");
        costpanediff.selectAll("#costdiff")
            .text("$" + (Number(d.ForecastCost) - Number(d.CurrentCost)));

        costpane2 = tooltip.selectAll("#costpane2");
        costpane2.selectAll("#cost2")
            .text("$" + d.ForecastCost);

        edit_button = costpanediff.selectAll("#button_edit");
        edit_button.attr("data-target", "#ProgramModal");

        add_button = costpane1.selectAll("#button_add");
        add_button.on('click', function(){ //Add Node

                        newNode = { name : "New ProgramElement"};
                        newNode.ProgramID = d.ProgramID;
                        newNode.ProgramElementID = "";
                        newNode.ProgramElementManager = "Temp";
                        newNode.ProgramElementSponsor = "Temp";
                        newNode.CurrentCost = "0";
                        newNode.ForecastCost = "0";
                        newNode.level = "ProgramElement";
                        newNode.parent = d;

                        //programElementPanel(newNode);
                        selectedNode = newNode;


        })
        add_button.attr("data-target", "#ProgramElementModal");


        */
        $('#ProgramModal').modal('show');

        //
        //popup = d3.select("#ontop");
        //popup.style("left", (d3.event.pageX ) + "px")
        //     .style("top", (d3.event.pageY) + "px")
        //     .style("opacity", 0);
        //
        //ID_label = popup.append("div")
        //    .attr("id","heading")
        //    .text("Program ID: ");
        //ProgramID_input = popup.append("input").attr("type","text").attr("id","ProgramID_input").attr("value",d.ProgramID);
        //popup.append("div").html("<br>");
        //
        //heading_label = popup.append("div")
        //    .attr("id","heading")
        //    .text("Program: ");
        //ProgramName_input = popup.append("input").attr("type","text").attr("id","ProgramName_input").attr("value",d.name);
        //popup.append("div").html("<br>");
        //
        //manager_label = popup.append("div").attr("id","mgr").text("Program Manager");
        //manager_input = popup.append("input").attr("type","text").attr("id","manager_input").attr("value",d.ProgramManager);
        //popup.append("div").html("<br>");
        //
        //sponsor_label = popup.append("div").attr("id","sponsor").text("Program Sponsor:");
        //sponsor_input = popup.append("input").attr("type","text").attr("id","sponsor_input").attr("value",d.ProgramSponsor);
        //popup.append("div").html("<br>");
        //
        //cost_label = popup.append("div").attr("id","current_costs").text("Current Cost: $" + d.CurrentCost);
        //cost_label = popup.append("div").attr("id","forecast_costs").text(" Forecast Cost: $" + d.ForecastCost);
        //popup.append("div").html("<br>");
        //
        //current_date_label =  popup.append("div").attr("id","current_date").text("Current Timeline: " + d.CurrentStartDate + " to " + d.CurrentEndDate);
        //forecast_date_label =  popup.append("div").attr("id","forecast_date").text("Forecast Timeline: " + d.ForecastStartDate + " to " + d.ForecastEndDate);
        //popup.append("div").html("<br>");
        //
        //
        //save_button = popup.append("input").attr("type","button").attr("id","save_button").attr("value", "Save changes")
        //    .on("click", function(){ //Add node
        //        d.name = $("#ProgramName_input").val();
        //        d.ProgramID = $("#ProgramID_input").val();
        //        toSave_name = $("#ProgramName_input").val();
        //        toSave_manager = $("#manager_input").val();
        //        toSave_sponsor = $("#sponsor_input").val();
        //
        //        /* API CALL TO UPDATE HERE */
        //
        //
        //        //selectedNode = toggleChildren(selectedNode);
        //        update(selectedNode);
        //
        //    });
        //
        //clear_button = popup.append("input").attr("type","button").attr("id","clear_button").attr("value", "Clear changes")
        //    .on("click", function(){ //Add node
        //
        //        d3.select("#ontop").selectAll("*").remove();
        //        programPanel(d);
        //        //selectedNode = toggleChildren(selectedNode);
        //        update(selectedNode);
        //
        //    });
        //
        //popup.append("div").html("<br>");
        //
        //add_button = popup.append("input").attr("type","button").attr("id","add_button").attr("value", "Add Program Element")
        //    .on("click", function(){ //Add node
        //        var newNode = { name : "New ProgramElement"};
        //        newNode.ProgramID = d.ProgramID;
        //        newNode.ProgramElementID = "";
        //        newNode.ProgramElementManager = "";
        //        newNode.ProgramElementSponsor = "";
        //        newNode.CurrentCost = "0";
        //        newNode.ForecastCost = "0";
        //        newNode.level = "ProgramElement";
        //
        //        if (!selectedNode._children && !selectedNode.children) {//Empty parent
        //            selectedNode._children = [newNode];
        //            selectedNode = toggleChildren(selectedNode);
        //        }
        //
        //        else if (selectedNode._children) { //Parent is collapsed
        //            selectedNode._children.push(newNode);
        //            selectedNode = toggleChildren(selectedNode);
        //        }
        //
        //        else if (selectedNode.children) { //Parent is expanded
        //            selectedNode.children.push(newNode);
        //
        //        }
        //        d3.select("#ontop").selectAll("*").remove();
        //        update(selectedNode);
        //        programElementPanel(newNode);
        //        selectedNode = newNode;
        //
        //    });
        //
        //
        //
        //remove_button = popup.append("input").attr("type","button").attr("id","remove_button").attr("value", "Remove Program")
        //    .on("click", function(){ //Remove node
        //        var parentNode = selectedNode.parent;
        //        //Find  index of selected node
        //        var i = parentNode.children.indexOf(selectedNode);
        //        //Remove selected child index from parent
        //        parentNode.children.splice(i,1);
        //
        //        /*API CALL FOR DELETE HERE */
        //
        //        update(parentNode);
        //
        //    });


    }


    function programElementPanel(d) {


        //tooltip = d3.select("#toolTip");
        //tooltip.style("left", (d3.event.pageX) + "px")
        //    .style("top", (d3.event.pageY) + "px")
        //    .style("opacity", 1);
        //
        //
        //tooltip.selectAll("#head")//.enter()
        //    .text("Program Element");
        //tooltip.selectAll("#header1")
        //    .text(d.name);
        //tooltip.selectAll("#header2")
        //    .text("Manager: " + d.ProgramElementManager);
        //
        //
        //costpane1 = tooltip.selectAll("#costpane1");
        //costpane1.selectAll("#cost1")
        //    .text("$" + d.CurrentCost);
        //
        //costpanediff = tooltip.selectAll("#costpanediff");
        //costpanediff.selectAll("#costdiff")
        //    .text("$" + (Number(d.ForecastCost) - Number(d.CurrentCost)));
        //
        //costpane2 = tooltip.selectAll("#costpane2");
        //costpane2.selectAll("#cost2")
        //    .text("$" + d.ForecastCost);
        //
        //edit_button = costpanediff.selectAll("#button_edit");
        //edit_button.attr("data-target", "#ProgramElementModal");
        //
        //
        //add_button = costpane1.selectAll("#button_add");
        //add_button.on('click', function(){ //Add Node
        //
        //     newNode = { name : "New Project"};
        //            newNode.ProgramID = d.ProgramID;
        //            newNode.ProgramElement = d.ProgramElementID;
        //            newNode.ProjectID = "";
        //            newNode.ProjectManager = "";
        //            newNode.ProjectSponsor = "";
        //            newNode.CurrentCost = "0";
        //            newNode.ForecastCost = "0";
        //            newNode.ApprovedTrendNumber = "-";
        //            newNode.level = "Project";
        //            newNode.parent = d;
        //
        //            selectedNode = newNode;
        //
        //
        //
        //})
        //add_button.attr("data-target", "#ProjectModal");

        $('#ProgramElementModal').modal('show');

        //
        //popup = d3.select("#ontop");
        //
        //ID_label = popup.append("div")
        //    .attr("id","heading")
        //    .text("Program Element ID: ");
        //ProgramElementID_input = popup.append("input").attr("type","text").attr("id","ProgramElementID_input").attr("value",d.ProgramElementID);
        //popup.append("div").html("<br>");
        //
        //heading_label = popup.append("div")
        //    .append("h4")
        //    .attr("id","heading")
        //    .text("Program Element: ");
        //ProgramElementName_input = popup.append("input").attr("type","text").attr("id","ProgramElementName_input").attr("value",d.name);
        //
        //manager_label = popup.append("div").attr("id","mgr").text("Program Element Manager");
        //manager_input = popup.append("input").attr("type","text").attr("id","manager_input").attr("value",d.ProgramElementManager);
        //popup.append("div").html("<br>");
        //
        //sponsor_label = popup.append("div").attr("id","sponsor").text("Program Element Sponsor:");
        //sponsor_input = popup.append("input").attr("type","text").attr("id","sponsor_input").attr("value",d.ProgramElementSponsor);
        //popup.append("div").html("<br>");
        //
        //cost_label = popup.append("div").attr("id","current_costs").text("Current Cost: $" + d.CurrentCost);
        //cost_label = popup.append("div").attr("id","forecast_costs").text(" Forecast Cost: $" + d.ForecastCost);
        //popup.append("div").html("<br>");
        //
        //current_date_label =  popup.append("div").attr("id","current_date").text("Current Timeline: " + d.CurrentStartDate + " to " + d.CurrentEndDate);
        //forecast_date_label =  popup.append("div").attr("id","forecast_date").text("Forecast Timeline: " + d.ForecastStartDate + " to " + d.ForecastEndDate);
        //popup.append("div").html("<br>");
        //
        //
        //
        //save_button = popup.append("input").attr("type","button").attr("id","save_button").attr("value", "Save changes")
        //    .on("click", function(){ //save node
        //
        //        d.name = $("#ProgramElementName_input").val();
        //        d.ProgramElementID = $("#ProgramElementID_input").val();
        //        toSave_manager = $("#manager_input").val();
        //        toSave_sponsor = $("#sponsor_input").val();
        //
        //        /* API CALL TO UPDATE HERE */
        //
        //
        //        //selectedNode = toggleChildren(selectedNode);
        //        update(selectedNode);
        //
        //    });
        //
        //
        //
        //
        //
        //
        //
        //
        //clear_button = popup.append("input").attr("type","button").attr("id","clear_button").attr("value", "Clear changes")
        //    .on("click", function(){ //Add node
        //
        //        d3.select("#ontop").selectAll("*").remove();
        //        programElementPanel(d);
        //        //selectedNode = toggleChildren(selectedNode);
        //        update(selectedNode);
        //
        //    });
        //
        //popup.append("div").html("<br>");
        //
        //add_button = popup.append("input").attr("type","button").attr("id","add_button").attr("value", "Add Project")
        //    .on("click", function(){ //Add node
        //        var newNode = { name : "New Project"};
        //        newNode.ProgramID = d.ProgramID;
        //        newNode.ProgramElement = d.ProgramElementID;
        //        newNode.ProjectID = "";
        //        newNode.ProjectManager = "";
        //        newNode.ProjectSponsor = "";
        //        newNode.CurrentCost = "0";
        //        newNode.ForecastCost = "0";
        //        newNode.ApprovedTrendNumber = "-";
        //        newNode.level = "Project";
        //
        //        if (!selectedNode._children && !selectedNode.children) {//Empty parent
        //            selectedNode._children = [newNode];
        //            selectedNode = toggleChildren(selectedNode);
        //        }
        //
        //        else if (selectedNode._children) { //Parent is collapsed
        //            selectedNode._children.push(newNode);
        //            selectedNode = toggleChildren(selectedNode);
        //        }
        //
        //        else if (selectedNode.children) { //Parent is expanded
        //            selectedNode.children.push(newNode);
        //
        //        }
        //        d3.select("#ontop").selectAll("*").remove();
        //        update(selectedNode);
        //        projectPanel(newNode);
        //        selectedNode = newNode;
        //
        //    });
        //
        //
        //
        //remove_button = popup.append("input").attr("type","button").attr("id","remove_button").attr("value", "Remove Program Element")
        //    .on("click", function(){ //Remove node
        //        var parentNode = selectedNode.parent;
        //        //Find  index of selected node
        //        var i = parentNode.children.indexOf(selectedNode);
        //        //Remove selected child index from parent
        //        parentNode.children.splice(i,1);
        //
        //        /*API CALL FOR DELETE HERE */
        //
        //        update(parentNode);
        //
        //    });


    }



    function projectPanel(d) {



        //tooltip = d3.select("#toolTip");
        //tooltip.style("left", (d3.event.pageX) + "px")
        //    .style("top", (d3.event.pageY) + "px")
        //    .style("opacity", 1);
        //
        //
        //tooltip.selectAll("#head")//.enter()
        //    .text("Project");
        //tooltip.selectAll("#header1")
        //    .text(d.name);
        //tooltip.selectAll("#header2")
        //    .text("Manager: " + d.ProjectManager);
        //
        //
        //costpane1 = tooltip.selectAll("#costpane1");
        //costpane1.selectAll("#cost1")
        //    .text("$" + d.CurrentCost);
        //
        //costpanediff = tooltip.selectAll("#costpanediff");
        //costpanediff.selectAll("#costdiff")
        //    .text("$" + (Number(d.ForecastCost) - Number(d.CurrentCost)));
        //
        //costpane2 = tooltip.selectAll("#costpane2");
        //costpane2.selectAll("#cost2")
        //    .text("$" + d.ForecastCost);
        //
        //edit_button = costpanediff.selectAll("#button_edit");
        //edit_button.attr("data-target", "#ProjectModal");
        //
        //add_button = costpane1.selectAll("#button_add");
        //add_button.on('click', function(){ //Add Node
        //
        //    newNode = { name : "New Trend"};
        //    //newNode.ProgramID = d.ProgramID;
        //    //newNode.ProgramElement = d.ProgramElementID;
        //    //newNode.ProjectID = "";
        //    //newNode.ProjectManager = "";
        //    //newNode.ProjectSponsor = "";
        //    //newNode.CurrentCost = "0";
        //    //newNode.ForecastCost = "0";
        //    //newNode.ApprovedTrendNumber = "-";
        //    //newNode.level = "Project";
        //    //newNode.parent = d;
        //    //
        //    //selectedNode = newNode;
        //
        //
        //
        //})
        //add_button.attr("data-target", "#TrendModal");

        $('#ProjectModal').modal('show');

        //popup = d3.select("#ontop");
        //
        //ID_label = popup.append("div")
        //    .attr("id","heading")
        //    .text("Project ID: ");
        //ProjectID_input = popup.append("input").attr("type","text").attr("id","ProjectID_input").attr("value",d.ProjectID);
        //popup.append("div").html("<br>");
        //
        //heading_label = popup.append("div")
        //    .append("h4")
        //    .attr("id","heading")
        //    .text("Project: ");
        //ProjectName_input = popup.append("input").attr("type","text").attr("id","ProjectName_input").attr("value",d.name);
        //
        //manager_label = popup.append("div").attr("id","mgr").text("Project Manager");
        //manager_input = popup.append("input").attr("type","text").attr("id","manager_input").attr("value",d.ProjectManager);
        //popup.append("div").html("<br>");
        //
        //sponsor_label = popup.append("div").attr("id","sponsor").text("Project Sponsor:");
        //sponsor_input = popup.append("input").attr("type","text").attr("id","sponsor_input").attr("value",d.ProjectSponsor);
        //popup.append("div").html("<br>");
        //
        //trend_label = popup.append("div").attr("id","mgr").text("Approved Trend : Trend " + d.ApprovedTrendNumber);
        //popup.append("div").html("<br>");
        //
        //cost_label = popup.append("div").attr("id","current_costs").text("Current Cost: $" + d.CurrentCost);
        //cost_label = popup.append("div").attr("id","forecast_costs").text(" Forecast Cost: $" + d.ForecastCost);
        //popup.append("div").html("<br>");
        //
        //current_date_label =  popup.append("div").attr("id","current_date").text("Current Timeline: " + d.CurrentStartDate + " to " + d.CurrentEndDate);
        //forecast_date_label =  popup.append("div").attr("id","forecast_date").text("Forecast Timeline: " + d.ForecastStartDate + " to " + d.ForecastEndDate);
        //popup.append("div").html("<br>");
        //
        //
        //save_button = popup.append("input").attr("type","button").attr("id","save_button").attr("value", "Save changes")
        //    .on("click", function(){ //Add node
        //        d.name = $("#ProjectName_input").val();
        //        d.ProjectID = $("#ProjectID_input").val();
        //        toSave_manager = $("#manager_input").val();
        //        toSave_sponsor = $("#sponsor_input").val();
        //
        //        /* API CALL TO UPDATE HERE */
        //
        //
        //        //selectedNode = toggleChildren(selectedNode);
        //        update(selectedNode);
        //
        //    });
        //
        //clear_button = popup.append("input").attr("type","button").attr("id","clear_button").attr("value", "Clear changes")
        //    .on("click", function(){ //Add node
        //
        //        d3.select("#ontop").selectAll("*").remove();
        //        projectPanel(d);
        //        //selectedNode = toggleChildren(selectedNode);
        //        update(selectedNode);
        //
        //    });
        //
        //popup.append("div").html("<br>");
        //
        //add_button = popup.append("input").attr("type","button").attr("id","add_button").attr("value", "Add Trend")
        //    .on("click", function(){ //Add node
        //        var newNode = { name : "New Node"};
        //        newNode.ProgramID = d.ProgramID;
        //        newNode.ProgramElement = d.ProgramElementID;
        //        newNode.ProjectID = d.ProjectID;
        //        newNode.TrendDescription = "";
        //        newNode.TrendJustification = "";
        //        newNode.TrendImpact = "";
        //        newNode.TrendStatus = "Pending"
        //        newNode.PreTrendCost = "0";
        //        newNode.PostTrendCost = "0";
        //        newNode.level = "Trend";
        //
        //        if (!selectedNode._children && !selectedNode.children) {//Empty parent
        //            selectedNode._children = [newNode];
        //            selectedNode = toggleChildren(selectedNode);
        //        }
        //
        //        else if (selectedNode._children) { //Parent is collapsed
        //            selectedNode._children.push(newNode);
        //            selectedNode = toggleChildren(selectedNode);
        //        }
        //
        //        else if (selectedNode.children) { //Parent is expanded
        //            selectedNode.children.push(newNode);
        //
        //        }
        //        d3.select("#ontop").selectAll("*").remove();
        //        update(selectedNode);
        //        trendPanel(newNode);
        //        selectedNode = newNode;
        //
        //    });
        //
        //
        //
        //remove_button = popup.append("input").attr("type","button").attr("id","remove_button").attr("value", "Remove Project")
        //    .on("click", function(){ //Remove node
        //        var parentNode = selectedNode.parent;
        //        //Find  index of selected node
        //        var i = parentNode.children.indexOf(selectedNode);
        //        //Remove selected child index from parent
        //        parentNode.children.splice(i,1);
        //
        //        /*API CALL FOR DELETE HERE */
        //
        //        update(parentNode);
        //
        //    });
    }


    function trendPanel(d) {

        popup = d3.select("#ontop");
        heading_label = popup.append("div").attr("id","heading").text("Trend " + d.TrendDescription + " : ");
        TrendDescription_input = popup.append("input").attr("type","text").attr("id","TrendDescription_input").attr("value",d.name);
        popup.append("div").html("<br>");

        description_label = popup.append("div").attr("id","mgr").text("Description ");
        description_input = popup.append("input").attr("type","text").attr("id","description_input").attr("value",d.name);
        popup.append("div").html("<br>");

        justification_label = popup.append("div").attr("id","sponsor").text("Justification ");
        justification_input = popup.append("input").attr("type","text").attr("id","justification_input").attr("value",d.TrendJustification);
        popup.append("div").html("<br>");

        impact_label = popup.append("div").attr("id","sponsor").text("Impact ");
        impact_input = popup.append("input").attr("type","text").attr("id","impact_input").attr("value",d.TrendImpact);
        popup.append("div").html("<br>");

        status_label = popup.append("div").attr("id","sponsor").text("Status " + d.TrendStatus);
        popup.append("div").html("<br>");

        cost_label = popup.append("div").attr("id","costs").text("PreTrend Cost: $" + d.PreTrendCost);
        cost_label = popup.append("div").attr("id","costs").text(" PostTrend Cost: $" + d.PostTrendCost);
        popup.append("div").html("<br>");

        pretrend_date_label =  popup.append("div").attr("id","current_date").text("Current Timeline: " + d.PreTrendStartDate + " to " + d.PreTrendEndDate);
        posttrend_date_label =  popup.append("div").attr("id","forecast_date").text("Forecast Timeline: " + d.PostStartStartDate + " to " + d.PostTrendEndDate);
        popup.append("div").html("<br>");

        save_button = popup.append("input").attr("type","button").attr("id","save_button").attr("value", "Save changes")
            .on("click", function(){ //Add node
                d.name = $("#TrendDescription_input").val();
                toSave_description = $("#TrendDescription_input").val();
                toSave_justification = $("#justification_input").val();
                toSave_impact = $("#impact_input").val();
                /* API CALL TO UPDATE HERE */


                //selectedNode = toggleChildren(selectedNode);
                update(selectedNode);

            });

        clear_button = popup.append("input").attr("type","button").attr("id","clear_button").attr("value", "Clear changes")
            .on("click", function(){ //Add node

                d3.select("#ontop").selectAll("*").remove();
                TrendPanel(d);
                //selectedNode = toggleChildren(selectedNode);
                update(selectedNode);

            });

        popup.append("div").html("<br>");

        add_button = popup.append("input").attr("type","button").attr("id","add_button").attr("value", "Add Phase")
            .on("click", function(){ //Add node
                var newNode = { name : "New Node"};
                newNode.ProgramID = d.ProgramID;
                newNode.ProgramElement = d.ProgramElementID;
                newNode.ProjectID = d.ProjectID;
                newNode.PhaseCode = "";
                newNode.level = "Phase";

                if (!selectedNode._children && !selectedNode.children) {//Empty parent
                    selectedNode._children = [newNode];
                    selectedNode = toggleChildren(selectedNode);
                }

                else if (selectedNode._children) { //Parent is collapsed
                    selectedNode._children.push(newNode);
                    selectedNode = toggleChildren(selectedNode);
                }

                else if (selectedNode.children) { //Parent is expanded
                    selectedNode.children.push(newNode);

                }
                d3.select("#ontop").selectAll("*").remove();
                update(selectedNode);
                phasePanel(newNode);
                selectedNode = newNode;

            });

        remove_button = popup.append("input").attr("type","button").attr("id","remove_button").attr("value", "Remove Trend")
            .on("click", function(){ //Remove node
                var parentNode = selectedNode.parent;
                //Find  index of selected node
                var i = parentNode.children.indexOf(selectedNode);
                //Remove selected child index from parent
                parentNode.children.splice(i,1);

                /*API CALL FOR DELETE HERE */

                update(parentNode);

            });
    }

    function phasePanel(d) {

        popup = d3.select("#ontop");
        heading_label = popup.append("div").attr("id","heading").text("Phase ");
        heading_input = popup.append("input").attr("type","text").attr("id","heading_input").attr("value",d.name);
        popup.append("div").html("<br>");

        cost_label = popup.append("div").attr("id","costs").text("PreTrend Cost: $" + d.PreTrendPhaseCost);
        cost_label = popup.append("div").attr("id","costs").text(" PostTrend Cost: $" + d.PostTrendPhaseCost);
        popup.append("div").html("<br>");

        pretrend_date_label =  popup.append("div").attr("id","current_date").text("Current Timeline: " + d.PreTrendStartDate + " to " + d.PreTrendEndDate);
        posttrend_date_label =  popup.append("div").attr("id","forecast_date").text("Forecast Timeline: " + d.PostStartStartDate + " to " + d.PostTrendEndDate);
        popup.append("div").html("<br>");

        save_button = popup.append("input").attr("type","button").attr("id","save_button").attr("value", "Save changes")
            .on("click", function(){ //Add node
                d.name = $("#heading_input").val();
                toSave_heading = $("#heading_input").val();
                /* API CALL TO UPDATE HERE */


                //selectedNode = toggleChildren(selectedNode);
                update(selectedNode);

            });

        clear_button = popup.append("input").attr("type","button").attr("id","clear_button").attr("value", "Clear changes")
            .on("click", function(){ //Add node

                d3.select("#ontop").selectAll("*").remove();
                phasePanel(d);
                //selectedNode = toggleChildren(selectedNode);
                update(selectedNode);

            });

        popup.append("div").html("<br>");

        add_button = popup.append("input").attr("type","button").attr("id","add_button").attr("value", "Add Activity")
            .on("click", function(){ //Add node

                /* CALL TO GANNT CHART */

            });

        remove_button = popup.append("input").attr("type","button").attr("id","remove_button").attr("value", "Remove Phase")
            .on("click", function(){ //Remove node
                var parentNode = selectedNode.parent;
                //Find  index of selected node
                var i = parentNode.children.indexOf(selectedNode);
                //Remove selected child index from parent
                parentNode.children.splice(i,1);

                /*API CALL FOR DELETE HERE */

                update(parentNode);
                selectedNode = newNode;

            });
    }

    function activityPanel(d) {

        popup = d3.select("#ontop");
        heading_label = popup.append("div").attr("id","heading").text("Category :" + d.BudgetCategory + " : " + d.name);
        cost_label1 = popup.append("div").attr("id","costs").text("FTE Cost: $" + d.FTECost);
        cost_label = popup.append("div").attr("id","costs").text(" Lumpsum Cost: $" + d.LumpsumCost);
        cost_label2 = popup.append("div").attr("id","costs").text("Unit Cost: $" + d.UnitCost);
        cost_label = popup.append("div").attr("id","costs").text(" Lumpsum Cost: $" + d.PercentageBasisCost);

        //date_label = popup.append("div").attr("id","dates").text("Current Schedule: " + "100 days " + "Forecast Schedule: " + "120 days");

        remove_button = popup.append("input").attr("type","button").attr("id","remove_button").attr("value", "Remove Activity")
            .on("click", function(){ //Remove node
                var parentNode = selectedNode.parent;
                var i = parentNode.children.indexOf(selectedNode);
                parentNode.children.splice(i,1);
                //selectedNode = toggleChildren(parentNode);
                update(parentNode);

            });
    }


    function update(source) {
        // Compute the new height, function counts total children of root node and sets tree height accordingly.
        // This prevents the layout looking squashed when new nodes are made visible or looking sparse when nodes are removed
        // This makes the layout more consistent.
        var levelWidth = [1];
        var childCount = function(level, n) {

            if (n.children && n.children.length > 0) {
                if (levelWidth.length <= level + 1) levelWidth.push(0);

                levelWidth[level + 1] += n.children.length;
                n.children.forEach(function(d) {
                    childCount(level + 1, d);
                });
            }
        };
        childCount(0, root);
        var newHeight = d3.max(levelWidth) * 100; // 25 pixels per line
        tree = tree.size([newHeight, viewerWidth]);



        // Compute the new tree layout.
        var nodes = tree.nodes(root).reverse(),
            links = tree.links(nodes);

        // Set widths between levels based on maxLabelLength.
        nodes.forEach(function(d) {
            d.y = (d.depth * (maxLabelLength * 7)); //maxLabelLength * 10px
            // alternatively to keep a fixed scale one can set a fixed depth per level
            // Normalize for fixed-depth by commenting out below line
            // d.y = (d.depth * 500); //500px per level.
        });

        // Update the nodes…
        node = svgGroup.selectAll("g.node")
            .data(nodes, function(d) {
                return d.id || (d.id = ++i);
            });

        // Enter any new nodes at the parent's previous position.
        var nodeEnter = node.enter().append("g")
            //.call(dragListener)
            .attr("class", "node")
            .attr("transform", function(d) {
                return "translate(" + source.y0 + "," + source.x0 + ")";
            })
            .on('click', click)
            .on('contextmenu', function (d, i) {
                debugger
                d3.event.preventDefault();
                rightclick(d);
            });


        nodeEnter.append("image")
            .attr("xlink:href", function(d)
            {
                if (d.level == "Root")
                    return "assets/js/wbs-tree/images/nodeA.png";
                if (d.level == "Program")
                    return "assets/js/wbs-tree/images/nodeB.png";
                if (d.level == "ProgramElement")
                    return "assets/js/wbs-tree/images/nodeE.png";
                if (d.level == "Project")
                    return "assets/js/wbs-tree/images/nodeD.png";
            })
            .attr("x", "-5px")
            .attr("y", "-5px")
            .attr("width", "15px")
            .attr("height", "15px");


        nodeEnter.append("text")
            /*.attr("x", function(d) {
                return d.children || d._children ? -10 : 10;
            })*/
            .attr("x", 1)
            .attr("y", -15)
            .attr("dy", ".35em")
            .attr('class', 'nodeText')
            .attr("text-anchor", function(d) {
                return d.children || d._children ? "end" : "start";
            })
            .text(function(d) {
                return d.name;
            })
            .style("fill-opacity", 1);

        nodeEnter.append("text")
            .attr("x", function(d) {
                return d.children || d._children ? -10 : 10;
            })
            .attr("y", 15)
            .attr("dy", ".35em")
            .attr('class', 'nodeText')
            .attr("text-anchor", function(d) {
                return d.children || d._children ? "end" : "start";
            })
            .text(function(d) {
                if (d.level == "Root")
                {}
                else
                    return "( $" + d.CurrentCost + ", " + d.CurrentStartDate + ")";
            })
            .style("fill-opacity", 1);

        // phantom node to give us mouseover in a radius around it
        nodeEnter.append("circle")
            .attr('class', 'ghostCircle')
            .attr("r", 30)
            .attr("opacity", 0.2) // change this to zero to hide the target area
            .style("fill", "red")
            .attr('pointer-events', 'mouseover')
            .on("mouseover", function(node) {
                overCircle(node);
            })
            .on("mouseout", function(node) {
                outCircle(node);
            });

        // Update the text to reflect whether node has children or not.
        node.select('text')
            .attr("x", function(d) {
                return d.children || d._children ? -10 : 10;
            })
            .attr("text-anchor", function(d) {
                return d.children || d._children ? "end" : "start";
            })
            .text(function(d) {
                return d.name;
            });

        // Change the circle fill depending on whether it has children and is collapsed
        node.select("circle.nodeCircle")
            .attr("r", 4.5)
            .style("fill", function(d) {
                return d._children ? "lightsteelblue" : "#fff";
            });

        // Transition nodes to their new position.
        var nodeUpdate = node.transition()
            .duration(duration)
            .attr("transform", function(d) {
                return "translate(" + d.y + "," + d.x + ")";
            });

        // Fade the text in
        nodeUpdate.select("text")
            .style("fill-opacity", 1);

        // Transition exiting nodes to the parent's new position.
        var nodeExit = node.exit().transition()
            .duration(duration)
            .attr("transform", function(d) {
                return "translate(" + source.y + "," + source.x + ")";
            })
            .remove();

        nodeExit.select("circle")
            .attr("r", 0);

        nodeExit.select("text")
            .style("fill-opacity", 0);

        // Update the links…
        var link = svgGroup.selectAll("path.link")
            .data(links, function(d)
            {
                if (d.target.level == "PastTrend" || d.target.level == "CurrentTrend" || d.target.level == "FutureTrend")
                {
                }
                else
                    return d.target.id;
            });

        // Enter any new links at the parent's previous position.
        link.enter().insert("path", "g")
            .attr("class", "link")
            .attr("d", function(d) {
                var o = {
                    x: source.x0,
                    y: source.y0
                };
                return diagonal({
                    source: o,
                    target: o
                });
            });

        // Transition links to their new position.
        link.transition()
            .duration(duration)
            .attr("d", diagonal);

        // Transition exiting nodes to the parent's new position.
        link.exit().transition()
            .duration(duration)
            .attr("d", function(d) {
                var o = {
                    x: source.x,
                    y: source.y
                };
                return diagonal({
                    source: o,
                    target: o
                });
            })
            .remove();

        // Stash the old positions for transition.
        nodes.forEach(function(d) {
            d.x0 = d.x;
            d.y0 = d.y;
        });
    }

    // Append a group which holds all nodes and which the zoom Listener can act upon.
    var svgGroup = baseSvg.append("g")
        .on("click", function(){
            //d3.select("#toolTip").style("opacity", 0);
        });


    // Define the root
    root = treeData;
    root.x0 = viewerHeight / 2;
    root.y0 = 0;

    // Layout the tree initially and center on the root node.
    update(root);
    centerNode(root.children[0].children[0]);
    update(root);
    var svgHeight = svgGroup[0].parentNode.clientHeight;
    var svgWidthUnit = svgGroup[0].parentNode.clientWidth / 7;
    //Add text headings and lines
    /*svgGroup.append("text")
        .attr("x", svgWidthUnit*1.1)
        .attr("y", -10)
        .attr("text-anchor", "middle")
        .style("font-size", "12px")
        .style("text-decoration", "underline")
        .text("Program");

    svgGroup.append("text")
        .attr("x", svgWidthUnit*2.1)
        .attr("y", -10)
        .attr("text-anchor", "middle")
        .style("font-size", "12px")
        .style("text-decoration", "underline")
        .text("Program Element");

    svgGroup.append("text")
        .attr("x", svgWidthUnit*3.2)
        .attr("y", -10)
        .attr("text-anchor", "middle")
        .style("font-size", "12px")
        .style("text-decoration", "underline")
        .text("Project");

    svgGroup.append("text")
        .attr("x", svgWidthUnit*4.35)
        .attr("y", -10)
        .attr("text-anchor", "middle")
        .style("font-size", "12px")
        .style("text-decoration", "underline")
        .text("Past");

    svgGroup.append("text")
        .attr("x", svgWidthUnit*5.5)
        .attr("y", -10)
        .attr("text-anchor", "middle")
        .style("font-size", "12px")
        .style("text-decoration", "underline")
        .text("Current");


    svgGroup.append("text")
        .attr("x", svgWidthUnit*6.5)
        .attr("y", -10)
        .attr("text-anchor", "middle")
        .style("font-size", "12px")
        .style("text-decoration", "underline")
        .text("Future");


    var line1 = svgGroup.append("line")
        .attr("x1", svgWidthUnit*0.6)
        .attr("y1", -10)
        .attr("x2", svgWidthUnit*0.6)
        .attr("y2", svgHeight)
        .attr("stroke-width", 0.3)
        .attr("stroke", "grey");


    var line2 = svgGroup.append("line")
        .attr("x1", svgWidthUnit*1.6)
        .attr("y1", -10)
        .attr("x2", svgWidthUnit*1.6)
        .attr("y2", svgHeight)
        .attr("stroke-width", 0.3)
        .attr("stroke", "grey");

    var line3 = svgGroup.append("line")
        .attr("x1", svgWidthUnit*2.6)
        .attr("y1", -10)
        .attr("x2", svgWidthUnit*2.6)
        .attr("y2", svgHeight)
        .attr("stroke-width", 0.3)
        .attr("stroke", "grey");

    var line4 = svgGroup.append("line")
        .attr("x1", svgWidthUnit*3.8)
        .attr("y1", -10)
        .attr("x2", svgWidthUnit*3.8)
        .attr("y2", svgHeight)
        .attr("stroke-width", 0.3)
        .attr("stroke", "grey");

    var line5 = svgGroup.append("line")
        .attr("x1", svgWidthUnit*4.9)
        .attr("y1", -10)
        .attr("x2", svgWidthUnit*4.9)
        .attr("y2", svgHeight)
        .attr("stroke-width", 0.3)
        .attr("stroke", "grey");

    var line6 = svgGroup.append("line")
        .attr("x1", svgWidthUnit*6.1)
        .attr("y1", -10)
        .attr("x2", svgWidthUnit*6.1)
        .attr("y2", svgHeight)
        .attr("stroke-width", 0.3)
        .attr("stroke", "grey");

    var line7 = svgGroup.append("line")
        .attr("x1", svgWidthUnit*7.2)
        .attr("y1", -10)
        .attr("x2", svgWidthUnit*7.2)
        .attr("y2", svgHeight)
        .attr("stroke-width", 0.3)
        .attr("stroke", "grey");*/


});
