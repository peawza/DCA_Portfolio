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
                GridExcelExport(e, "Miscellaneous")
            },
            toolbar: [
                setPagerInfoToToolbar(grid_inquire.grid_ID),
                { name: "search", text: Resources("COMMON", "ToolbarSearch") },
                { template: kendo_grid.Toolbar.btn_export_excel("export-button", "exportButtonClicked()") },
                { template: kendo_grid.Toolbar.btn_new("new-button", "newdocumentButtonClicked()") },
            ],
            columns: [
                {

                    title: Resources("CMS040", "GD001"),
                    width: "60px", attributes: { class: "k-text-center " },
                    headerAttributes: { "data-no-reorder": "true" },
                    template: dataItem => grid.dataSource.indexOf(dataItem) + 1

                },
                {
                    title: Resources("CMS040", "GD002"),
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
                                let criteria = {
                                    ModulesCode: data.modulesCode,
                                    MiscTypeCode: data.miscTypeCode,
                                    MiscCode: data.miscCode
                                }
                                let dataAPI = await APIPost(_url_callapi + "/api/common/cms040/get/miscellaneous/id", criteria);
                                dataAPI.data.moduleCode = data.modulesCode;
                                misc_dialog_windows.view(dataAPI.data);
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
                                let criteria = {
                                    ModulesCode: data.modulesCode,
                                    MiscTypeCode: data.miscTypeCode,
                                    MiscCode: data.miscCode
                                }
                                let dataAPI = await APIPost(_url_callapi + "/api/common/cms040/get/miscellaneous/id", criteria);
                                dataAPI.data.moduleCode = data.modulesCode;
                                misc_dialog_windows.edit(dataAPI.data);
                            },
                        },
                        {
                            className: "btn-delete-grid btn-remove k-danger "

                            , name: "remove", text: "", iconClass: "k-icon k-i-trash"
                            , visible: function (dataItem) { return false }
                            , click: async function (e) {
                                // prevent page scroll position change
                                e.preventDefault();
                                var tr = $(e.target).closest("tr"); // get the current table row (tr)
                                var data = await this.dataItem(tr);

                                let criteriaDel = {
                                    ModulesCode: data.modulesCode,
                                    MiscTypeCode: data.miscTypeCode,
                                    MiscCode: data.miscCode,
                                    DeletedBy: _user_update_by
                                }
                                const confirmationDialog = new ConfirmationDialog("dialogdelete");
                                confirmationDialog.open({
                                    yes: async function () {
                                        try {
                                            let ApiDelete = await APIPost(_url_callapi + "/api/common/cms040/delete", criteriaDel)
                                            //console.log(ApiDelete);
                                            ApiDelete = ApiDelete.data;

                                            if (ApiDelete.messageCode == "S001") {
                                                //console.log("AAAA");
                                                showSuccess(Message("Information", "DeleteSuccess"));
                                                var grid = $(grid_inquire.grid_ID).data("kendoGrid");
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
                        }]
                },
                {

                    field: "modulesName",
                    title: Resources("CMS040", "GD003"),
                    attributes: { class: "k-text-right" },
                    width: "150px"
                },
                {
                    field: "miscTypeName",
                    title: Resources("CMS040", "GD004"),
                    attributes: { class: "k-text-left" },
                    width: "200px"
                },
                {
                    field: "miscCode",
                    title: Resources("CMS040", "GD005"),
                    attributes: { class: "k-text-left" },
                    width: "180px",
                    //filterable: kendo_grid.filter.filter_Active,
                    //template: (data) => {

                    //    return kendo_grid.template.Active_Inactive(data.activeFlag)

                    //}

                },
                {
                    field: "value1",
                    title: Resources("CMS040", "GD006"),
                    attributes: { class: "k-text-left" },
                    width: "150px"
                },
                {
                    field: "value2",
                    title: Resources("CMS040", "GD007"),
                    attributes: { class: "k-text-left" },
                    width: "150px"
                },
                {
                    field: "seqNo",
                    title: Resources("CMS040", "GD008"),
                    attributes: { class: "k-text-left" },
                    width: "150px"
                },
                {
                    field: "description",
                    title: Resources("CMS040", "GD009"),
                    attributes: { class: "k-text-left" },
                    width: "150px"
                },
                {
                    field: "status",
                    title: Resources("CMS040", "GD010"),
                    attributes: { class: "k-text-left" },
                    width: "120px",
                    filterable: kendo_grid.filter.filter_Active,
                    template: (data) => {

                        return kendo_grid.template.Active_Inactive(data.status)

                    }
                },
                {
                    field: "createBy",
                    title: Resources("CMS040", "GD011"),
                    attributes: { class: "text-left " },
                    width: "200px"
                },
                {
                    field: "createDate",
                    title: Resources("CMS040", "GD012"),
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
                    title: Resources("CMS040", "GD013"),
                    attributes: { class: "text-left " },
                    width: "200px"
                },
                {
                    field: "updateDate",
                    title: Resources("CMS040", "GD014"),
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