var record_details = 0;

//let grid_inquire = {
//    grid_ID: "#grid-inquiry",
//    using: async (DataApi) => {
//        DataApi = DataApi.map(data => ({
//            ...data
//            //activeFlagDisplay: kendo_grid.template.Active_Inactive_Display(data.activeFlag)
//        }));
//        if ($(grid_inquire.grid_ID).data("kendoGrid") == undefined) {
//            await grid_inquire.create(DataApi)
//        } else {
//            await grid_inquire.update(DataApi);
//        }
//    },
//    create: (DataApi) => {

//        let dataSource = new kendo.data.DataSource({
//            transport: {
//                read: function (e) {
//                    e.success(DataApi);
//                },
//                destroy: function (e) {
//                    e.success();
//                },
//                create: function (e) {
//                    // On success.
//                    e.success();
//                },
//                parameterMap: function (options, operation) {
//                    if (operation !== "read" && options.models) {
//                        return {
//                            models: kendo.stringify(options.models)
//                        };
//                    }
//                }
//            },
//            page: 1,
//            pageSize: GridPageSizeDefault(),

//        });

//        // var record = 0;
//        let grid = $(grid_inquire.grid_ID).kendoGrid({
//            dataSource: dataSource,
//            pageable: {
//                pageSizes: GridPageSizes(),

//            },
//            filterable: GridFillterable(),
//            sortable: true,
//            reorderable: true,
//            excel: {
//                allPages: true
//            }, excelExport: function (e) {
//                GridExcelExport(e, "UserGroup")
//            },
//            toolbar: [
//                setPagerInfoToToolbar(grid_inquire.grid_ID),
//                { name: "search", text: Resources("COMMON", "ToolbarSearch") },
//                { template: kendo_grid.Toolbar.btn_export_excel("export-button", "exportButtonClicked()") },
//                { template: kendo_grid.Toolbar.btn_new("new-button", "newdocumentButtonClicked()") },
//            ],
//            columns: [
//                {

//                    title: Resources("UMS020", "GD001"),
//                    width: "60px", attributes: { class: "k-text-center " },
//                    headerAttributes: { "data-no-reorder": "true" },
//                    template: dataItem => grid.dataSource.indexOf(dataItem) + 1

//                },
//                {
//                    title: Resources("UMS020", "GD002"),
//                    width: "200px", attributes: { class: "k-text-center " },
//                    command: [
//                        {
//                            name: "viewmode",
//                            text: `<span class='k-icon k-i-search'></span>`,
//                            className: "k-button k-button-icontext",
//                            click: async function (e) {
//                                e.preventDefault();
//                                var tr = $(e.target).closest("tr"); // get the current table row (tr)
//                                var data = this.dataItem(tr);

//                                usergroup_dialog.view(data);
//                            },
//                        },
//                        {
//                            name: "editmode",
//                            text: `<span class='k-icon k-i-edit'></span>`,
//                            className: "k-button k-button-icontext btn-editmode ",
//                            visible: function (dataItem) { return permissions.AllowEdit },
//                            click: async function (e) {
//                                e.preventDefault();
//                                var tr = $(e.target).closest("tr"); // get the current table row (tr)
//                                var data = this.dataItem(tr);
//                                //console.log(data);
//                                //let dataAPI = await APIPost(_url_callapi + "/api/production/master/pms010/getplantid", { plantCode: data.plantCode });
//                                usergroup_dialog.edit(data);
//                            },
//                        },
//                        {
//                            className: "btn-delete-grid btn-remove k-danger "

//                            , name: "remove", text: "", iconClass: "k-icon k-i-trash"
//                            , visible: function (dataItem) { return permissions.AllowDelete }
//                            , click: async function (e) {
//                                // prevent page scroll position change
//                                e.preventDefault();
//                                var tr = $(e.target).closest("tr"); // get the current table row (tr)
//                                var data = await this.dataItem(tr);

//                                let usergroupid = data.id;
//                                const confirmationDialog = new ConfirmationDialog("dialogdelete");
//                                confirmationDialog.open({
//                                    yes: async function () {
//                                        try {
//                                            let ApiDelete = await APIPost(_url_callapi + "/api/auth/ums020/roles/delete", {
//                                                Id: usergroupid
//                                            })
//                                            //console.log(ApiDelete);
//                                            ApiDelete = ApiDelete.data;

//                                            if (ApiDelete.messageCode == "ROLE000") {
//                                                //console.log("AAAA");
//                                                showSuccess(Message("Information", "DeleteSuccess"));
//                                                var grid = $(grid_inquire.grid_ID).data("kendoGrid");
//                                                grid.removeRow(tr);
//                                                await grid.data('kendoGrid').refresh();

//                                            }

//                                            // confirmationDialogDeleteOpen = 0;
//                                        } catch (e) {
//                                            //confirmationDialogDeleteOpen = 0;
//                                        } finally {

//                                            //confirmationDialogDeleteOpen = 0;
//                                        }

//                                    }, no: async function () {
//                                        // confirmationDialogDeleteOpen = 0;


//                                    }





//                                }, common.format(Message("Confirm", "ConfirmDelete")));
//                                $(document).ready(function () {
//                                    try {
//                                        var Selector = document.querySelectorAll("#dialogdelete");
//                                        for (let loop = 0; loop < Selector.length; loop++) {
//                                            if (loop < (Selector.length - 1)) {
//                                                Selector[loop].parentElement.remove()
//                                            }
//                                        }
//                                    } catch (e) {

//                                    }
//                                });
//                            }
//                        },
//                        {
//                            name: "usermapper",
//                            //iconClass: "k-icon k-i-trash",
//                            text: ``,
//                            className: "k-button k-button-icontext mes-user-mapper ",
//                            visible: function (dataItem) { return permissions.AllowEdit },
//                            click: async function (e) {
//                                e.preventDefault();
//                                var tr = $(e.target).closest("tr"); // get the current table row (tr)
//                                var data = this.dataItem(tr);
//                                //console.log(data);
//                                //let dataAPI = await APIPost(_url_callapi + "/api/production/master/pms010/getplantid", { plantCode: data.plantCode });
//                                usergroupmapping_dialog.edit(data);
//                            },
//                        },
//                        {
//                            name: "usergrouppermission",
//                            //iconClass: "k-icon k-i-trash",
//                            text: ``,
//                            className: "k-button k-button-icontext mes-usergroup-permission",
//                            visible: function (dataItem) { return permissions.AllowEdit },
//                            click: async function (e) {
//                                e.preventDefault();
//                                var tr = $(e.target).closest("tr"); // get the current table row (tr)
//                                var data = this.dataItem(tr);
//                                //console.log(data);
//                                //let dataAPI = await APIPost(_url_callapi + "/api/production/master/pms010/getplantid", { plantCode: data.plantCode });
//                                window.location.href = "/UserManages/GroupPermission/Index?id="+data.id;
//                            },
//                        },
//                    ]
//                },
//                {

//                    field: "name",
//                    title: Resources("UMS020", "GD003"),
//                    attributes: { class: "k-text-right" },
//                    width: "150px"
//                },
//                {
//                    field: "description",
//                    title: Resources("UMS020", "GD004"),
//                    attributes: { class: "k-text-left" },
//                    width: "250px"
//                },
//                {
//                    field: "isActive",
//                    title: Resources("UMS020", "GD005"),
//                    attributes: { class: "k-text-left" },
//                    width: "120px",
//                    filterable: kendo_grid.filter.filter_Active,
//                    template: (data) => {

//                        return kendo_grid.template.Active_Inactive(data.isActive)

//                    }

//                },
//                {
//                    field: "createBy",
//                    title: Resources("UMS020", "GD006"),
//                    attributes: { class: "text-left " },
//                    width: "200px"
//                },
//                {
//                    field: "createDate",
//                    title: Resources("UMS020", "GD007"),
//                    attributes: { class: "text-center " },
//                    width: "160px",
//                    template: (data) => {
//                        if (data.createDate != null) {
//                            return kendo.toString(new Date(data.createDate), formatDateTimePicker)
//                        }
//                        return "";
//                    },
//                    filterable: false
//                },
//                {
//                    field: "updateBy",
//                    title: Resources("UMS020", "GD008"),
//                    attributes: { class: "text-left " },
//                    width: "200px"
//                },
//                {
//                    field: "updateDate",
//                    title: Resources("UMS020", "GD009"),
//                    attributes: { class: "text-center " },
//                    width: "160px",
//                    template: (data) => {
//                        if (data.updateDate != null) {
//                            return kendo.toString(new Date(data.updateDate), formatDateTimePicker)
//                        }
//                        return "";
//                    },
//                    format: `{0:${formatDateTimePicker}}`,
//                    filterable: false
//                },

//            ],
//            dataBound: function (e) {
//                var preferedHeight = Math.round($(window).height() - $(grid_inquire.grid_ID).position().top) - 200;
//                let grid = $(grid_inquire.grid_ID).data('kendoGrid');
//                if (preferedHeight < 240) {
//                    preferedHeight = 540;
//                }
//                app.ui.toggleVScrollable(grid, { height: preferedHeight });
//                movePagerInfoToToolbar(grid_inquire.grid_ID)
//            },
//            noRecords: kendo_grid.noRecords

//        }).data("kendoGrid");
//    },
//    update: (DataApi) => {
//        let dataSource = new kendo.data.DataSource({
//            data: DataApi,
//            pageSize: GridPageSizeDefault()
//        });
//        $(grid_inquire.grid_ID).data("kendoGrid").setDataSource(dataSource);

//    },
//}

let grid_inquiry1 = {
    grid_ID: "#grid-inquiry",
    option_grid_detailInit: async (detail) => {
        console.log("Test=>", detail.data.detail);
        detail.detailRow.find(".detail-grid").kendoGrid({
            dataSource: {
                transport: {
                    read: async function (e) {// On success.

                        await e.success(detail.data.detail);
                    },
                },


                //filter: { field: "id", operator: "eq", value: detail.data.id }
            },
            scrollable: false,
            filterable: true,
            pageable: false,
            columns: [
                {
                    field: "username",
                    title: Resources("UMS020", "GD021"),
                    headerAttributes: {
                        "class": "text-center",
                    },
                    attributes: { class: "k-text-left " },
                    width: "150px",
                    filterable: true,

                },
                {
                    field: "name",
                    title: Resources("UMS020", "GD022"),
                    headerAttributes: {
                        "class": "text-center",
                    },
                    attributes: { class: "k-text-left " },
                    width: "150px",
                    filterable: true,

                },
                {
                    field: "displayDepartmentCode",
                    title: Resources("UMS020", "GD023"),
                    headerAttributes: {
                        "class": "text-center",
                    },
                    attributes: { class: "k-text-left " },
                    width: "150px",
                    filterable: true,

                },
                {
                    field: "displayPosition",
                    title: Resources("UMS020", "GD024"),
                    headerAttributes: {
                        "class": "text-center",
                    },
                    attributes: { class: "k-text-left " },
                    width: "150px",
                    filterable: true,

                },
            ],
            noRecords: kendo_grid.noRecords
        });
    },
    using: async (DataApi) => {
        //console.log(DataApi.length)
        DataApi = DataApi.map(obj => (
            {
                ...obj
            }))
        if ($(grid_inquiry1.grid_ID).data("kendoGrid") == undefined) {
            await grid_inquiry1.create(DataApi)
        } else {
            await grid_inquiry1.update(DataApi);
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

        let grid = $(grid_inquiry1.grid_ID).kendoGrid({
            dataSource: dataSource,
            detailTemplate: '<div class="detail-grid"></div>',
            detailInit: function (e) {
                console.log(e, Array.isArray(e.data.detail));
                //const items = Array.isArray(e.data.detail) ? e.data.detail : [];
                grid_inquiry1.option_grid_detailInit(e);
                //e.detailRow.find('.detail-grid').kendoGrid({
                //    dataSource: {
                //        transport: {
                //            read: ds => ds.success(e.data.detail)
                //        }
                //    },
                //    scrollable: false,
                //    filterable: true,
                //    pageable: false,
                //    columns: [
                //        { field: 'username', title: Resources('UMS020', 'GD021'), width: '150px', attributes: { class: 'k-text-left' } },
                //        { field: 'name', title: Resources('UMS020', 'GD022'), width: '150px', attributes: { class: 'k-text-left' } },
                //        { field: 'displayDepartmentCode', title: Resources('UMS020', 'GD023'), width: '150px', attributes: { class: 'k-text-left' } },
                //        { field: 'displayPosition', title: Resources('UMS020', 'GD023'), width: '150px', attributes: { class: 'k-text-left' } },
                //    ],
                //    noRecords: kendo_grid.noRecords
                //});
            },
            pageable: {
                pageSizes: GridPageSizes(),

            },
            filterable: GridFillterable(),
            sortable: true,
            reorderable: true,
            excel: {
                allPages: true
            }, excelExport: function (e) {
                GridExcelExport(e, "UserGroup")
            },
            toolbar: [
                setPagerInfoToToolbar(grid_inquiry1.grid_ID),
                { name: "search", text: Resources("COMMON", "ToolbarSearch") },
                { template: kendo_grid.Toolbar.btn_export_excel("export-button", "exportButtonClicked()") },
                { template: kendo_grid.Toolbar.btn_new("new-button", "newdocumentButtonClicked()") },
            ],
            columns: [
                {

                    title: Resources("UMS020", "GD001"),
                    width: "60px", attributes: { class: "k-text-center " },
                    headerAttributes: { "data-no-reorder": "true" },
                    template: dataItem => grid.dataSource.indexOf(dataItem) + 1

                },
                {
                    title: Resources("UMS020", "GD002"),
                    width: "200px", attributes: { class: "k-text-center " },
                    command: [
                        {
                            name: "viewmode",
                            text: `<span class='k-icon k-i-search'></span>`,
                            className: "k-button k-button-icontext",
                            click: async function (e) {
                                e.preventDefault();
                                var tr = $(e.target).closest("tr"); // get the current table row (tr)
                                var data = this.dataItem(tr);

                                usergroup_dialog.view(data);
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
                                //let dataAPI = await APIPost(_url_callapi + "/api/production/master/pms010/getplantid", { plantCode: data.plantCode });
                                usergroup_dialog.edit(data);
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

                                let usergroupid = data.id;
                                const confirmationDialog = new ConfirmationDialog("dialogdelete");
                                confirmationDialog.open({
                                    yes: async function () {
                                        try {
                                            let ApiDelete = await APIPost(_url_callapi + "/api/auth/ums020/roles/delete", {
                                                Id: usergroupid
                                            })
                                            //console.log(ApiDelete);
                                            ApiDelete = ApiDelete.data;

                                            if (ApiDelete.messageCode == "ROLE000") {
                                                //console.log("AAAA");
                                                showSuccess(Message("Information", "DeleteSuccess"));
                                                var grid = $(grid_inquiry1.grid_ID).data("kendoGrid");
                                                grid.removeRow(tr);
                                                await grid.data('kendoGrid').refresh();

                                            }

                                            // confirmationDialogDeleteOpen = 0;
                                        } catch (e) {
                                            //confirmationDialogDeleteOpen = 0;
                                        } finally {

                                            //confirmationDialogDeleteOpen = 0;
                                        }

                                    }, no: async function () {
                                        // confirmationDialogDeleteOpen = 0;


                                    }
                                }, common.format(Message("Confirm", "ConfirmDelete")));
                                $(document).ready(function () {
                                    try {
                                        var Selector = document.querySelectorAll("#dialogdelete");
                                        for (let loop = 0; loop < Selector.length; loop++) {
                                            if (loop < (Selector.length - 1)) {
                                                Selector[loop].parentElement.remove()
                                            }
                                        }
                                    } catch (e) {

                                    }
                                });
                            }
                        },
                        {
                            name: "usermapper",
                            iconClass: "k-icon user-mapper",
                            text: ``,
                            className: "k-button k-button-icontext ",
                            visible: function (dataItem) { return permissions.AllowEdit },
                            click: async function (e) {
                                e.preventDefault();
                                var tr = $(e.target).closest("tr"); // get the current table row (tr)
                                var data = this.dataItem(tr);
                                //console.log(data);
                                //let dataAPI = await APIPost(_url_callapi + "/api/production/master/pms010/getplantid", { plantCode: data.plantCode });
                                usergroupmapping_dialog.edit(data);
                            },
                        },
                        {
                            name: "usergrouppermission",
                            iconClass: "k-icon usergroup-permission",
                            text: ``,
                            className: "k-button k-button-icontext ",
                            visible: function (dataItem) { return permissions.AllowEdit },
                            click: async function (e) {
                                e.preventDefault();
                                var tr = $(e.target).closest("tr"); // get the current table row (tr)
                                var data = this.dataItem(tr);
                                //console.log(data);
                                //let dataAPI = await APIPost(_url_callapi + "/api/production/master/pms010/getplantid", { plantCode: data.plantCode });
                                window.location.href = "/UserManages/GroupPermission/Index?id=" + data.id;
                            },
                        },
                    ]
                },
                {

                    field: "name",
                    title: Resources("UMS020", "GD003"),
                    attributes: { class: "k-text-right" },
                    width: "150px"
                },
                {
                    field: "description",
                    title: Resources("UMS020", "GD004"),
                    attributes: { class: "k-text-left" },
                    width: "250px"
                },
                {
                    field: "isActive",
                    title: Resources("UMS020", "GD005"),
                    attributes: { class: "k-text-left" },
                    width: "120px",
                    filterable: kendo_grid.filter.filter_Active,
                    template: (data) => {

                        return kendo_grid.template.Active_Inactive(data.isActive)

                    }
                },
                {
                    field: "createBy",
                    title: Resources("UMS020", "GD006"),
                    attributes: { class: "text-left " },
                    width: "200px"
                },
                {
                    field: "createDate",
                    title: Resources("UMS020", "GD007"),
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
                    title: Resources("UMS020", "GD008"),
                    attributes: { class: "text-left " },
                    width: "200px"
                },
                {
                    field: "updateDate",
                    title: Resources("UMS020", "GD009"),
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
                var preferedHeight = Math.round($(window).height() - $(grid_inquiry1.grid_ID).position().top) - 200;
                let grid = $(grid_inquiry1.grid_ID).data('kendoGrid');
                if (preferedHeight < 240) {
                    preferedHeight = 540;
                }
                app.ui.toggleVScrollable(grid, { height: preferedHeight });
                //movePagerInfoToToolbar(grid_inquiry1.grid_ID)
            },
            noRecords: kendo_grid.noRecords,
        }).data("kendoGrid");
    },
    update: (DataApi) => {
        let dataSource = new kendo.data.DataSource({
            data: DataApi,
            pageSize: GridPageSizeDefault()
        });
        $(grid_inquiry1.grid_ID).data("kendoGrid").setDataSource(dataSource);

    },
}