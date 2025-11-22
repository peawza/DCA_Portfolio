var record_details = 0;

let grid_inquire = {
    grid_ID: "#grid-inquiry",
    using: async (DataApi) => {
        DataApi = DataApi.map(data => ({
            ...data
            //activeFlagDisplay: kendo_grid.template.Active_Inactive_Display(data.activeFlag)
        }));
        if ($(grid_inquire.grid_ID).data("kendoGrid") == undefined) {
            await grid_inquire.create(DataApi)
        } else {
            await grid_inquire.update(DataApi);
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
            pageSize: GridPageSizeDefault(),

        });

        // var record = 0;
        let grid = $(grid_inquire.grid_ID).kendoGrid({
            dataSource: dataSource,
            pageable: {
                pageSizes: GridPageSizes(),

            },
            filterable: GridFillterable(),
            sortable: true,
            reorderable: true,
            excel: {
                allPages: true
            }, excelExport: function (e) {
                GridExcelExport(e, "User")
            },
            toolbar: [
                setPagerInfoToToolbar(grid_inquire.grid_ID),
                { name: "search", text: Resources("COMMON", "ToolbarSearch") },
                { template: kendo_grid.Toolbar.btn_export_excel("export-button", "exportButtonClicked()") },
                { template: kendo_grid.Toolbar.btn_new("new-button", "newdocumentButtonClicked()") },
            ],
            columns: [
                {

                    title: Resources("UMS010", "GD001"),
                    width: "60px", attributes: { class: "k-text-center " },
                    headerAttributes: { "data-no-reorder": "true" },
                    template: dataItem => grid.dataSource.indexOf(dataItem) + 1

                },
                {
                    title: Resources("UMS010", "GD002"),
                    width: "160px", attributes: { class: "k-text-center " },
                    command: [
                        {
                            name: "viewmode",
                            text: `<span class='k-icon k-i-search'></span>`,
                            className: "k-button k-button-icontext",
                            click: async function (e) {
                                e.preventDefault();
                                var tr = $(e.target).closest("tr"); // get the current table row (tr)
                                var data = this.dataItem(tr);
                                let dataAPI = await APIPost(_url_callapi + "/api/auth/ums010/getby/id", { UserId: data.id });
                                user_dialog_windows.view(dataAPI.data);
                            },
                        },
                        {
                            name: "editmode",
                            text: `<span class='k-icon k-i-edit'></span>`,
                            className: "k-button k-button-icontext btn-editmode ",
                            visible: function (dataItem) { return permissions.AllowEdit },
                            click: async function (e) {
                                e.preventDefault();
                                var tr = $(e.target).closest("tr"); // get the current table row (tr)
                                var data = this.dataItem(tr);
                                //console.log(data);
                                let dataAPI = await APIPost(_url_callapi + "/api/auth/ums010/getby/id", { UserId: data.id });
                                user_dialog_windows.edit(dataAPI.data);
                            },
                        },
                        {
                            className: "btn-delete-grid btn-remove k-danger "

                            , name: "remove", text: "", iconClass: "k-icon k-i-trash"
                            , visible: function (dataItem) { return permissions.AllowDelete }
                            , click: async function (e) {
                                // prevent page scroll position change
                                e.preventDefault();
                                var tr = $(e.target).closest("tr"); // get the current table row (tr)
                                var data = await this.dataItem(tr);

                                //let dataTransportID = data.TransportID;
                                //const confirmationDialog = new ConfirmationDialog("dialogdelete");
                                //confirmationDialog.open({
                                //    yes: async function () {


                                //        try {


                                //            let ApiDelete = await APIPost(_url_callapi + "/api/production/master/pms010/delete", {
                                //                "PlantCode": data.plantCode,
                                //                "deleteby": _user_update_by
                                //            })
                                //            //console.log(ApiDelete);
                                //            ApiDelete = ApiDelete.data;

                                //            if (ApiDelete.statusCode == "Ok") {
                                //                //console.log("AAAA");
                                //                showSuccess(Message("Information", "DeleteSuccess"));
                                //                var grid = $(grid_inquire.grid_ID).data("kendoGrid");
                                //                grid.removeRow(tr);
                                //                await grid.data('kendoGrid').refresh();


                                //            }

                                //            // confirmationDialogDeleteOpen = 0;
                                //        } catch (e) {
                                //            //confirmationDialogDeleteOpen = 0;
                                //        } finally {

                                //            //confirmationDialogDeleteOpen = 0;
                                //        }

                                //    }, no: async function () {
                                //        // confirmationDialogDeleteOpen = 0;


                                //    }





                                //}, common.format(Message("Confirm", "ConfirmDelete")));
                                //$(document).ready(function () {
                                //    try {
                                //        var Selector = document.querySelectorAll("#dialogdelete");
                                //        for (let loop = 0; loop < Selector.length; loop++) {
                                //            if (loop < (Selector.length - 1)) {
                                //                Selector[loop].parentElement.remove()
                                //            }
                                //        }
                                //    } catch (e) {

                                //    }
                                //});
                            }

                        },
                        {
                            name: "changepassword",
                            iconClass: "k-icon usergroup-permission",
                            text: ``,
                            className: "k-button k-button-icontext ",


                            visible: function (dataItem) { return permissions.AllowEdit },
                            click: async function (e) {
                                e.preventDefault();
                                var tr = $(e.target).closest("tr"); // get the current table row (tr)
                                var data = this.dataItem(tr);
                                //console.log(data);
                                let dataAPI = await APIPost(_url_callapi + "/api/auth/ums010/getby/id", { UserId: data.id });
                                changepassword_dialog_windows.edit(dataAPI.data);
                            },
                        },
                    ]
                },
                {

                    field: "userName",
                    title: Resources("UMS010", "GD003"),
                    attributes: { class: "k-text-right" },
                    width: "150px"
                },
                {
                    field: "displayName",
                    title: Resources("UMS010", "GD004"),
                    attributes: { class: "k-text-left" },
                    width: "200px"
                },
                {
                    field: "displayDepartment",
                    title: Resources("UMS010", "GD005"),
                    attributes: { class: "k-text-left" },
                    width: "180px"

                },
                {
                    field: "displayPosition",
                    title: Resources("UMS010", "GD006"),
                    attributes: { class: "k-text-left" },
                    width: "150px"
                },
                {
                    field: "email",
                    title: Resources("UMS010", "GD007"),
                    attributes: { class: "k-text-left" },
                    width: "180px"
                },
                {
                    field: "phoneNumber",
                    title: Resources("UMS010", "GD008"),
                    attributes: { class: "k-text-left" },
                    width: "180px"
                },
                {
                    field: "isActive",
                    title: Resources("UMS010", "GD009"),
                    attributes: { class: "k-text-left" },
                    width: "120px",
                    filterable: kendo_grid.filter.filter_Active,
                    template: (data) => {

                        return kendo_grid.template.Active_Inactive(data.isActive)

                    }
                },
                {
                    field: "displayPermissionGroup",
                    title: Resources("UMS010", "GD010"),
                    attributes: { class: "k-text-left" },
                    width: "180px"
                },
                {
                    field: "lastLoginDate",
                    title: Resources("UMS010", "GD011"),
                    attributes: { class: "k-text-left" },
                    width: "180px",
                    template: (data) => {
                        if (data.lastLoginDate != null) {
                            return kendo.toString(new Date(data.lastLoginDate), formatDateTimePicker)
                        }
                        return "";
                    },
                    filterable: false
                },
                {
                    field: "lastChangedPassword",
                    title: Resources("UMS010", "GD012"),
                    attributes: { class: "k-text-left" },
                    width: "230px",
                    template: (data) => {
                        if (data.lastChangedPassword != null) {
                            return kendo.toString(new Date(data.lastChangedPassword), formatDateTimePicker)
                        }
                        return "";
                    },
                },
                {
                    field: "createBy",
                    title: Resources("UMS010", "GD013"),
                    attributes: { class: "text-left " },
                    width: "200px"
                },
                {
                    field: "createDate",
                    title: Resources("UMS010", "GD014"),
                    attributes: { class: "text-center " },
                    width: "160px",
                    template: (data) => {
                        if (data.createDate != null) {
                            return kendo.toString(new Date(data.createDate), formatDateTimePicker)
                        }
                        return "";
                    },
                    filterable: false
                },

                {

                    field: "updateBy",
                    title: Resources("UMS010", "GD015"),
                    attributes: { class: "text-left " },
                    width: "200px"
                },
                {
                    field: "updateDate",
                    title: Resources("UMS010", "GD016"),
                    attributes: { class: "text-center " },
                    width: "160px",
                    template: (data) => {
                        if (data.updateDate != null) {
                            return kendo.toString(new Date(data.updateDate), formatDateTimePicker)
                        }
                        return "";
                    },
                    format: `{0:${formatDateTimePicker}}`,
                    filterable: false
                },

            ],
            dataBound: function (e) {
                var preferedHeight = Math.round($(window).height() - $(grid_inquire.grid_ID).position().top) - 200;
                let grid = $(grid_inquire.grid_ID).data('kendoGrid');
                if (preferedHeight < 240) {
                    preferedHeight = 540;
                }
                app.ui.toggleVScrollable(grid, { height: preferedHeight });
                //movePagerInfoToToolbar(grid_inquire.grid_ID)
            },
            noRecords: kendo_grid.noRecords

        }).data("kendoGrid");
    },
    update: (DataApi) => {
        let dataSource = new kendo.data.DataSource({
            data: DataApi,
            pageSize: GridPageSizeDefault()
        });
        $(grid_inquire.grid_ID).data("kendoGrid").setDataSource(dataSource);

    },
}