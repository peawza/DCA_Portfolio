
let grid_userlist = {
    grid_ID: "#optionalGrid",
    using: async (DataApi) => {
        DataApi = DataApi.map(data => ({
            ...data
            //activeFlagDisplay: kendo_grid.template.Active_Inactive_Display(data.activeFlag)
        }));
        if ($(grid_userlist.grid_ID).data("kendoGrid") == undefined) {
            await grid_userlist.create(DataApi)
        } else {
            await grid_userlist.update(DataApi);
        }
    },
    create: (DataApi) => {

        let dataSource = new kendo.data.DataSource({
            transport: {
                read: function (e) {
                    e.success(DataApi);
                },
                destroy: function (e) {
                    e.success();
                },
                create: function (e) {
                    // On success.
                    e.success();
                },
                parameterMap: function (options, operation) {
                    if (operation !== "read" && options.models) {
                        return {
                            models: kendo.stringify(options.models)
                        };
                    }
                }
            },
            page: 1,
            //pageSize: GridPageSizeDefault(),
        });
        let grid = $(grid_userlist.grid_ID).kendoGrid({
            dataSource: dataSource,
            filterable: {
                extra: false,
                operators: {
                    string: {
                        //startswith: "Starts with",
                        contains: "Contains",
                        eq: "Equal to",
                        neq: "Not equal to"
                    }
                }
            },
            selectable: "multiple, row",
            columns: [
                {
                    field: "userName", title: Resources("UMS022", "GD001"),
                    width: 100,
                    attributes: { style: "text-align: left !important;" },
                },
                {
                    field: "displayFullName", title: Resources("UMS022", "GD002"),
                    width: 100,
                    attributes: { style: "text-align: left;" },
                },
                {
                    field: "displayDepartment", title: Resources("UMS022", "GD003"),
                    width: 100,
                    attributes: { style: "text-align: left;" },
                }
            ],
            height: 300,
            width: 600,
            scrollable: true
        });
    },
    update: (DataApi) => {
        let dataSource = new kendo.data.DataSource({
            data: DataApi,
            //pageSize: GridPageSizeDefault()
        });
        $(grid_userlist.grid_ID).data("kendoGrid").setDataSource(dataSource);

    },
}
let grid_userlistforsection = {
    grid_ID: "#selectedGrid",
    using: async (DataApi) => {
        DataApi = DataApi.map(data => ({
            ...data
            //activeFlagDisplay: kendo_grid.template.Active_Inactive_Display(data.activeFlag)
        }));
        if ($(grid_userlistforsection.grid_ID).data("kendoGrid") == undefined) {
            await grid_userlistforsection.create(DataApi)
        } else {
            await grid_userlistforsection.update(DataApi);
        }
    },
    create: (DataApi) => {

        let dataSource = new kendo.data.DataSource({
            transport: {
                read: function (e) {
                    e.success(DataApi);
                },
                destroy: function (e) {
                    e.success();
                },
                create: function (e) {
                    // On success.
                    e.success();
                },
                parameterMap: function (options, operation) {
                    if (operation !== "read" && options.models) {
                        return {
                            models: kendo.stringify(options.models)
                        };
                    }
                }
            },
            page: 1,
            //pageSize: GridPageSizeDefault(),
        });
        let grid = $(grid_userlistforsection.grid_ID).kendoGrid({
            dataSource: dataSource,
            filterable: {
                extra: false,
                operators: {
                    string: {
                        //startswith: "Starts with",
                        contains: "Contains",
                        eq: "Equal to",
                        neq: "Not equal to"
                    }
                }
            },
            selectable: "multiple, row",
            columns: [
                {
                    field: "userName", title: Resources("UMS022", "GD001"),
                    width: 100,
                    attributes: { style: "text-align: left !important;" },
                },
                {
                    field: "displayFullName", title: Resources("UMS022", "GD002"),
                    width: 100,
                    attributes: { style: "text-align: left;" },
                },
                {
                    field: "displayDepartment", title: Resources("UMS022", "GD003"),
                    width: 100,
                    attributes: { style: "text-align: left;" },
                }
            ],
            height: 300,
            width: 600,
            scrollable: true,
            dataBound: function (e) {
                //var preferedHeight = Math.round($(window).height() - $(grid_userlistforsection.grid_ID).position().top) - 200;
                //let grid = $(grid_userlistforsection.grid_ID).data('kendoGrid');
                //if (preferedHeight < 240) {
                //    preferedHeight = 300;
                //}
                //app.ui.toggleVScrollable(grid, { height: preferedHeight });
            },
            noRecords: kendo_grid.noRecords

        }).data("kendoGrid");
    },
    update: (DataApi) => {
        let dataSource = new kendo.data.DataSource({
            data: DataApi,
            //pageSize: GridPageSizeDefault()
        });
        $(grid_userlistforsection.grid_ID).data("kendoGrid").setDataSource(dataSource);

    },
}