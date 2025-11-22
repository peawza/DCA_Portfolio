function checkGridRowEdit(IdGrid) {
    const ElementGridComponent = document.getElementById(IdGrid);
    var nodes = ElementGridComponent.childNodes[2].childNodes[0];
    nodes = ElementGridComponent.childNodes[2].childNodes[0].getElementsByTagName('tr')
    var indexInsert = null;
    var checkEditData = false;
    for (var indexNode = 0; indexNode < nodes.length; indexNode++) {
        if (nodes[indexNode].className.includes("k-grid-edit-row")) {
            checkEditData = true;
            break;
        }
    }
    return checkEditData;
}

function GridPageSizes() {
    return [20, 40, 60, 80, 100, "All"];
}

function GridPageSizeDefault_Dialog() {
    return [5,10, 20, "All"];
}
function GridPageSizes_Dialog() {
    return 5;
}

function GridPageSizeDefault() {
    return 20;
}
function GridCommonPageSizeDefault() {
    return 40;
}

function GridFillterable() {
    return {
        mode: "menu",
        operators: {
            string: {
                contains: "Contains",
                startswith: "Starts with",
                eq: "Is equal to",
                neq: "Is not equal to",
                endswith: "Ends with",
                isnull: "Is null",
                isnotnull: "Is not null",
                isempty: "Is empty"
            },

            date: {
                eq: "Is equal to",
                neq: "Is not equal to",
                gt: "After",
                lt: "Before",
                isnull: "Is null",
                isnotnull: "Is not null"
            }

        },
        //extra: false,
    }
}
function GridExcelExport(e, filename, displayColumeNo) {
    let check_DisplayColume_No = true;
    if (displayColumeNo == undefined || displayColumeNo == null) {
        check_DisplayColume_No = true
    } else {
        check_DisplayColume_No = displayColumeNo
    }

    let file = setFileName(filename);
    e.workbook.fileName = file + ".xlsx";

    var sheet = e.workbook.sheets[0];
    var columns = sheet.columns; // Ensure columns are fetched properly

    if (check_DisplayColume_No == true) {
        // Add a new column for "Column No" at the beginning
        columns.unshift({
            width: 50, // Set a suitable width for the column
            title: "No", // Title for the column
        });

        // Add "Column No" values for each row
        sheet.rows.forEach((row, rowIndex) => {
            if (row.type !== "header") { // Skip header row
                row.cells.unshift({
                    value: String(rowIndex), // Running number starts from 1
                    textAlign: "center", // Optional alignment
                });
            } else {
                // Add header cell for "Column No" with background color
                row.cells.unshift({
                    value: "No",
                    textAlign: "center",
                    background: "#7A7A7A", // Set the desired background color
                    color: "#FFFFFF", // Optional: Set font color (white)
                });
            }
        });

        columns.forEach(function (column) {
            // Also delete the width if it is set
            delete column.width;
            column.autoWidth = true;
        });
    }

    // Process the rows and cells for formatting
    for (var rowIndex = 0; rowIndex < sheet.rows.length; rowIndex++) {
        var row = sheet.rows[rowIndex];
        for (var cellIndex = 0; cellIndex < row.cells.length; cellIndex++) {
            var cell = row.cells[cellIndex];
            if (cell.value && cell.value.toString().indexOf("<br>") >= 0) {
                cell.value = cell.value.replace(/<br>/g, ' ');
                cell.wrap = true;
            }
        }
    }

    for (var rowIndex = 1; rowIndex < sheet.rows.length; rowIndex++) {
        var row = sheet.rows[rowIndex];
        for (var cellIndex = 0; cellIndex < row.cells.length; cellIndex++) {
            var cell = row.cells[cellIndex];

            if (cell.value && typeof cell.value === "number") {
                if (Number.isInteger(cell.value)) {
                    cell.format = "#,##0"; // ✅ จำนวนเต็ม
                } else {
                    cell.format = "#,##0.00"; // ✅ มีทศนิยม
                }
            } else if (cell.value && typeof cell.value === "string" && /^\d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}(\.\d{1,3})?$/.test(cell.value)) {
                var date = kendo.parseDate(cell.value, "yyyy-MM-ddTHH:mm:ss");
                if (date) {
                    cell.value = date;
                    cell.format = "dd/MM/yyyy HH:mm:ss";
                }
            } else if (cell.value && typeof cell.value === "string" && /^\d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}$/.test(cell.value)) {
                var date = kendo.parseDate(cell.value, "yyyy-MM-ddTHH:mm:ss");
                if (date) {
                    cell.value = date;
                    cell.format = "dd/MM/yyyy";
                }
            } else if (cell.value && typeof cell.value === "string" && /^\d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}(\.\d+)?$/.test(cell.value)) {
                var date = kendo.parseDate(cell.value, "yyyy-MM-ddTHH:mm:ss.fff");
                if (date) {
                    cell.value = date;
                    cell.format = "dd/MM/yyyy";
                }
            }
            else if (cell.value && typeof cell.value === "object") {
                var date = kendo.parseDate(cell.value, "yyyy-MM-ddTHH:mm:ss");
                if (date) {
                    cell.value = date;
                    cell.format = "dd/MM/yyyy HH:mm:ss";
                }
            }
            //console.log("typeof cell.value =>", typeof cell.value);
        }
    }
}


let kendo_grid = {
    filter: {
        string: {
            operators: {
                string: {
                    contains: "Contains",
                    startswith: "Starts with",
                    eq: "Is equal to",
                    neq: "Is not equal to",
                    endswith: "Ends with",
                    isnull: "Is null",
                    isnotnull: "Is not null",
                    isempty: "Is empty"
                }
            }
        },
        filter_Active_Inactive: {
            ui: function (element) {
                //console.log(element);
                // 
                element.kendoDropDownList({
                    dataSource: [
                        { text: "Active", value: "Active" },
                        { text: "Inactive", value: "Inactive" },

                    ],
                    dataTextField: "text",
                    dataValueField: "value",
                    optionLabel: Resources("COMMON", "DropDownAll")
                });

                $(document).ready(function () {
                    // Add 'd-none' class to the dropdown with the title "Operator"
                    $($('.k-filter-menu-container .k-picker.k-dropdownlist.k-picker-solid.k-picker-md.k-rounded-md')[0]).addClass('d-none');
                });


                //$('.k-filter-menu-container').find('label:contains("Operator")')
                //    .closest('.k-dropdownlist') // Get the closest dropdown
                //    .addClass('d-none');

            }
            , operators: {
                string: {
                    eq: "Contains",
                }
            },
            extra: false,

        },

        filter_0_1: {
            ui: function (element) {
                //console.log(element);
                // 
                element.kendoDropDownList({
                    dataSource: [
                        { text: "Yes", value: true },
                        { text: "No", value: false }
                    ],
                    dataTextField: "text",
                    dataValueField: "value",
                    optionLabel: Resources("COMMON", "DropDownAll")
                });

                $(document).ready(function () {
                    // Add 'd-none' class to the dropdown with the title "Operator"
                    $($('.k-filter-menu-container .k-picker.k-dropdownlist.k-picker-solid.k-picker-md.k-rounded-md')[0]).addClass('d-none');
                });
            }
            , operators: {
                string: {
                    contains: "Contains",
                }
            },
            extra: false,

        },

        filter_true_false: {
            ui: function (element) {
                //console.log(element);
                // 
                element.kendoDropDownList({
                    dataSource: [
                        { text: "Yes", value: true },
                        { text: "No", value: false }
                    ],
                    dataTextField: "text",
                    dataValueField: "value",
                    optionLabel: Resources("COMMON", "DropDownAll"),
                });

                $(document).ready(function () {
                    // Add 'd-none' class to the dropdown with the title "Operator"
                    $($('.k-filter-menu-container .k-picker.k-dropdownlist.k-picker-solid.k-picker-md.k-rounded-md')[0]).addClass('d-none');
                });
            }
            , operators: {
                string: {
                    contains: "Contains",
                }
            },
            extra: false,

        },
        filter_Active: {
            ui: function (element) {
                //console.log(element);
                // 
                element.kendoDropDownList({
                    dataSource: [
                        { text: "Active", value: true },
                        { text: "Inactive", value: false }
                    ],
                    dataTextField: "text",
                    dataValueField: "value",
                    optionLabel: Resources("COMMON", "DropDownAll"),
                });

                $(document).ready(function () {
                    // Add 'd-none' class to the dropdown with the title "Operator"
                    $($('.k-filter-menu-container .k-picker.k-dropdownlist.k-picker-solid.k-picker-md.k-rounded-md')[0]).addClass('d-none');
                });
            }
            , operators: {
                string: {
                    contains: "Contains",
                }
            },
            extra: false,

        },

    },
    template: {
        Active_Inactive: (data) => {
            textDisplat = ``
            if (data == true) {
                textDisplat = `<i class="fa-solid fa-circle" style="color: green;"></i>  ${Resources("COMMON", "Active")}`

            } else {
                textDisplat = `<i class="fa-solid fa-circle" style="color: red;"></i>  ${Resources("COMMON", "Inactive")}`
            }
            return textDisplat
        },
        Active_Inactive_Display: (data) => {
            textDisplat = ``
            if (data == true) {
                textDisplat = `${Resources("COMMON", "Active")}`

            } else {
                textDisplat = `${Resources("COMMON", "Inactive")}`
            }
            return textDisplat
        }
    },

    noRecords:
    {
        template: "<div class='empty-grid'></div>"
    },
    Toolbar: {
        btn_new: (id, onclick) => {
            if (permissions.AllowNew == true) {
                return `<button class="k-success view-mode-check-new k-button k-button-md k-rounded-md k-button-solid k-button-solid-base" onclick="${onclick}" id="${id}" name="${id}" style="min-width:80px" data-role="button" type="button" role="button" aria-disabled="false" tabindex="0"><span class="k-button-text">${Resources("COMMON", "NEW")}</span> <span class="k-icon k-i-plus-circle k-button-icon"></span></button>`
            } else {
                return `<div ></div>`
            }

        },
        btn_export_excel: (id, onclick) => {
            if (permissions.AllowExport == true) {
                return ` <button class="k-success k-button k-button-md k-rounded-md k-button-solid k-button-solid-base k-state-disabled"  onclick="${onclick}" id="${id}" name="${id}" data-role="button" type="button" role="button" aria-disabled="false" tabindex="0" disabled="disabled"><span class="k-button-text">${Resources("COMMON", "EXPORT")}</span> <span class="k-icon k-i-download k-button-icon"></span></button>`
             } else {
                return `<div ></div>`
             }
        },
        btn_new_area: (id, onclick) => {
            if (permissions.AllowNew == true) {
                return `<button class="k-success view-mode-check-new k-button k-button-md k-rounded-md k-button-solid k-button-solid-base" onclick="${onclick}" id="${id}" name="${id}" style="min-width:80px" data-role="button" type="button" role="button" aria-disabled="false" tabindex="0"><span class="k-button-text">${Resources("COMMON", "NewArea")}</span><span class="k-icon k-i-plus-circle k-button-icon"></span></button>`
            } else {
                return `<div ></div>`
            }
        },
        btn_print: (id, onclick, text) => {
            if (permissions.AllowPrint == true) {
                return `<button class="k-danger k-button k-button-md k-rounded-md k-button-solid k-button-solid-base" onclick="${onclick}" id="${id}"  name="${id}" data-role="button" type="button" role="button" aria-disabled="false" tabindex="0"><span class="k-button-text">${text}</span><span class="k-icon k-i-print k-button-icon"></span></button>`
            } else {
                return `<div ></div>`
            }
        },
        btn_print_pdf: (id, onclick) => {
            if (permissions.AllowPrint == true) {
                return `<button class="k-danger k-button k-button-md k-rounded-md k-button-solid k-button-solid-bas k-state-disablede" onclick="${onclick}" id="${id}" name="${id}" data-role="button" type="button" role="button" aria-disabled="false" tabindex="0"><span class="k-button-text">Print QR</span> <span class="k-icon k-i-print k-button-icon"></span></button>`
            } else {
                return `<div ></div>`
            }
        },
        btn_inactive: (id, onclick, text) => {
            if (permissions.AllowPrint == true) {
                return `<button class="k-danger k-button k-button-md k-rounded-md k-button-solid k-button-solid-base k-state-disabled" onclick="${onclick}" id="${id}" name="${id}" data-role="button" type="button" role="button" aria-disabled="false" tabindex="0" disabled="disabled"><span class="k-button-text">${text}</span> <span class="k-icon k-i-minus-outline k-button-icon"></span></button>`
            } else {
                return `<div ></div>`
            }
        },
        btn_gen_file: (id, onclick, text) => {
            if (permissions.AllowExport == true) {
                return ` <button class="k-success k-button k-button-md k-rounded-md k-button-solid k-button-solid-base k-state-disabled"  onclick="${onclick}" id="${id}" name="${id}" data-role="button" type="button" role="button" aria-disabled="false" tabindex="0" disabled="disabled"><span class="k-button-text">${text}</span> <span class="k-icon k-i-download k-button-icon"></span></button>`
            } else {
                return `<div ></div>`
            }
        },
        btn_re_status: (id, onclick, text) => {
            if (permissions.AllowPrint == true) {
                return `<button class="k-danger k-button k-button-md k-rounded-md k-button-solid k-button-solid-base k-state-disabled" onclick="${onclick}" id="${id}" name="${id}" data-role="button" type="button" role="button" aria-disabled="false" tabindex="0" disabled="disabled"><span class="k-button-text">${text}</span> <span class="k-icon k-i-arrow-rotate-ccw k-button-icon"></span></button>`
            } else {
                return `<div ></div>`
            }
        },
        btn_confirm: (id, onclick, text) => {
            if (permissions.AllowPrint == true) {
                return `<button class="k-primary k-button k-button-md k-rounded-md k-button-solid k-button-solid-base k-state-disabled" onclick="${onclick}" id="${id}" name="${id}" data-role="button" type="button" role="button" aria-disabled="false" tabindex="0" disabled="disabled"><span class="k-button-text">${text}</span> <span class="k-icon k-i-check-outline k-button-icon"></span></button>`
            } else {
                return `<div ></div>`
            }
        },
        btn_add: (id, onclick, text) => {
            if (permissions.AllowPrint == true) {
                return `<button class="k-dark k-button k-button-md k-rounded-md k-button-solid k-button-solid-base" onclick="${onclick}" id="${id}" name="${id}" data-role="button" type="button" role="button" aria-disabled="false" tabindex="0"><span class="k-button-text">${text}</span> <span class="k-icon k-i-plus-circle k-button-icon"></span></button>`
            } else {
                return `<div ></div>`
            }
        },
        btn_remove: (id, onclick, text) => {
            if (permissions.AllowPrint == true) {
                return `<button class="k-danger k-button k-button-md k-rounded-md k-button-solid k-button-solid-base" onclick="${onclick}" id="${id}" name="${id}" data-role="button" type="button" role="button" aria-disabled="false" tabindex="0"><span class="k-button-text">${text}</span> <span class="k-icon k-i-trash k-button-icon"></span></button>`
            } else {
                return `<div ></div>`
            }
        },
        btn_download_sample_file: (id, onclick) => {
            if (permissions.AllowPrint == true) {
                return `<button class="k-primary k-button k-button-md k-rounded-md k-button-solid k-button-solid-base" onclick="${onclick}" id="${id}" name="${id}" data-role="button" type="button" role="button" aria-disabled="false" tabindex="0"><span class="k-button-text">${Resources("COMMON", "DownloadSampleFile")}</span> <span class="k-icon k-i-download  k-button-icon"></span></button>`
            } else {
                return `<div ></div>`
            }
        },
        btn_import_file: (id, onclick) => {
            if (permissions.AllowPrint == true) {
                return `<button class="k-primary k-button k-button-md k-rounded-md k-button-solid k-button-solid-base" onclick="${onclick}" id="${id}" name="${id}" data-role="button" type="button" role="button" aria-disabled="false" tabindex="0"><span class="k-button-text">${Resources("COMMON", "IMPORT")}</span> <span class="k-icon k-i-download  k-button-icon"></span></button>`
            } else {
                return `<div ></div>`
            }
        },
        btn_upload_file: (id, onclick) => {
            if (permissions.AllowPrint == true) {
                return `<button class="k-dark k-button k-button-md k-rounded-md k-button-solid k-button-solid-base" onclick="${onclick}" id="${id}" name="${id}" data-role="button" type="button" role="button" aria-disabled="false" tabindex="0"><span class="k-button-text">${Resources("COMMON", "UPLOAD")}</span> <span class="k-icon k-i-upload  k-button-icon"></span></button>`
            } else {
                return `<div ></div>`
            }
        },
        btn_clear: (id, onclick) => {
            return `<button class="mes-clear k-button k-button-md k-rounded-md k-button-solid k-button-solid-base" onclick="${onclick}" id="${id}" name="${id}" data-role="button" type="button" role="button" aria-disabled="false" tabindex="0"><span class="k-button-text">${Resources("COMMON", "CLEAR")}</span> <span class="k-icon k-i-clear k-button-icon"></span></button>`
            
        },
    }
}


function setPagerInfoToToolbar(gridID) {
    //let gridID = "#aaa";
    gridID = gridID.replace("#", "");  // Removes the #
    let templateGrid = `<span id="${gridID}customPagerInfo" class="k-label"></span>` 
    return { template: templateGrid }
}



function movePagerInfoToToolbar(gridID) {
    gridID = gridID.replace("#", "");
    setTimeout(function () {
        let pagerInfo = $(`#${gridID} .k-pager-info`).text();
        $(`#${gridID}customPagerInfo`).text(pagerInfo);
    }, 100);
}

function setPagerInfoAndFileNameImportToToolbar(gridID) {
    //let gridID = "#aaa";
    gridID = gridID.replace("#", "");  // Removes the #
    let templateGrid = `<div  style='width:25%' class="row"><div  class="col-lg-12 col-md-24 col-sm-36"> <span id="fileNameImport" ></span></span></div> <div  class="col-lg-12 col-md-24 col-sm-36"><span id="${gridID}customPagerInfo" class="k-label"> </div></div>`
    return { template: templateGrid }
}

function movePagerInfoAndFileNameImportToToolbar(gridID, fileName) {
    gridID = gridID.replace("#", "");
    //console.log('fileName: ', fileName)
    setTimeout(function () {
        let pagerInfo = $(`#${gridID} .k-pager-info`).text();
        $(`#${gridID}customPagerInfo`).text(pagerInfo);
        if (fileName) {
            $(`#fileNameImport`).text("File Name : " + fileName);
        } else {
            $(`#fileNameImport`).text('');
        }
    }, 100);
}