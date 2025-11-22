let ScreenCode = "UMS010"
var date = new Date(), y = date.getFullYear(), m = date.getMonth();
var firstDay = new Date(y, m, 1);
var lastDay = new Date(date.getFullYear(), date.getMonth() + 1, 0);

// Input  Search
let sc_username1, sc_name, sc_status, sc_permissiongroup, sc_department;
// DataSource
let dataSource_status, dataSource_permissiongroup,dataSource_department;

document.addEventListener("DOMContentLoaded", async (event) => {

    await CreateUI();
    //await grid_inquire.using([]);

});
async function CreateUI() {
    //Text Box
    $("#sc-username1").kendoTextBox({

    });
    sc_username1 = $("#sc-username1").data("kendoTextBox");

    $("#sc-name").kendoTextBox({

    });
    sc_name = $("#sc-name").data("kendoTextBox");

    //DropdownList
    //dataSource_status = await defaultData();
    dataSource_status = await APIPost(_url_callapi + "/api/common/miscellaneous", { "MiscTypeCode": "Status", "Status": 1 })
    //console.log("Test DropdownList =>", dataSource_status);
    $("#sc-status").kendoDropDownList({
        dataSource: dataSource_status.data,
        filter: "contains",
        //minLength: 1,
        dataTextField: "displayName",
        dataValueField: "miscCode"
        , optionLabel: Resources("COMMON", "DropDownAll"),
    });
    sc_status = $("#sc-status").data("kendoDropDownList");

    //dataSource_permissiongroup = await defaultData();
    dataSource_permissiongroup = await APIPost(_url_callapi + "/api/auth/common/roles", {})
    $("#sc-permissiongroup").kendoDropDownList({
        dataSource: dataSource_permissiongroup.data,
        filter: "contains",
        //minLength: 1,
        dataTextField: "name",
        dataValueField: "name"
        , optionLabel: Resources("COMMON", "DropDownAll"),
    });
    sc_permissiongroup = $("#sc-permissiongroup").data("kendoDropDownList");

    //dataSource_department = await defaultData();
    dataSource_department = await APIPost(_url_callapi + "/api/auth/common/departments", { })
    $("#sc-department").kendoDropDownList({
        dataSource: dataSource_department.data,
        filter: "contains",
        //minLength: 1,
        dataTextField: "departmentName",
        dataValueField: "departmentCode"
        , optionLabel: Resources("COMMON", "DropDownAll"),
    });
    sc_department = $("#sc-department").data("kendoDropDownList");
}
// 2
$(document).ready(async function () {

    await LoadDefault();
});

// 3
window.addEventListener("load", (event) => {

});
async function LoadDefault() {

    $("#message-container").css("display", "none");
    await ui.Input.Clear("search-container", () => {

    });
    await app.ui.clearAlert("#message-container");
    app.ui.uiEnable(["#export-button"], false);
    await grid_inquire.using([]);
}
async function SearchData() {
    let flagactive;
    if (sc_status.value() != "") {
        flagactive = sc_status.value() == "0" ? false : true;
    } else {
        flagactive = null
    }
    let criteria = {
        UserName: sc_username1.value(),
        Name: sc_name.value(),
        PermissionGroup: sc_permissiongroup.value(),
        Department: sc_department.value(),
        ActiveFlag: flagactive,
    }
    try {
        let DataCallApi = await APIPost(_url_callapi + "/api/auth/ums010/search", criteria);
        await app.ui.clearAlert("#message-container");
        if (DataCallApi.data.length == 0) {
            Event.showWarning(Message("Warning", "DataNotFound"));
            grid_inquire.using([]);
            app.ui.uiEnable(["#export-button"], false);
            return;
        }
        grid_inquire.using(DataCallApi.data);
        app.ui.uiEnable(["#export-button"], true);
    } catch (e) {

        // 
    } finally {

    }
}
async function defaultData() {
    var listobjData = [];
    var dataobj = {
        data: listobjData,
        resultCode: "SUCCESS",
        resultMessage: "The operation was successful.",
        resultStatus:true
    }
    return dataobj;
}



(function () {
    const input = document.getElementById('ip-username');

    // กำหนด regex ไว้ใช้งานร่วมกัน
    const rxSingle = /^[A-Za-z0-9_.-]$/;      // ตัวเดียว (ใช้กับ keydown)
    const rxChunk = /^[A-Za-z0-9_.-]+$/;     // หลายตัว (ใช้กับ beforeinput)
    const rxClean = /[^A-Za-z0-9_.-]/g;      // ใช้ลบสิ่งที่ไม่อนุญาต

    input.addEventListener('keydown', (e) => {
        const controlKeys = [
            'Backspace', 'Delete', 'ArrowLeft', 'ArrowRight', 'ArrowUp', 'ArrowDown',
            'Home', 'End', 'Tab', 'Escape', 'Enter'
        ];
        if (controlKeys.includes(e.key) || e.ctrlKey || e.metaKey || e.altKey) return;

        // อนุญาตเฉพาะ A–Z a–z 0–9 _ - .
        if (!rxSingle.test(e.key)) e.preventDefault();
    });

    input.addEventListener('beforeinput', (e) => {
        // กันเคสพิมพ์/วางที่เป็นหลายตัว
        if (e.data && !rxChunk.test(e.data)) e.preventDefault();
    });

    input.addEventListener('input', () => {
        // ลบอักขระที่ไม่อนุญาต (ครอบคลุมพิมพ์/วาง/IME)
        const cleaned = input.value.replace(rxClean, '');
        if (input.value !== cleaned) input.value = cleaned;

        // ปรับข้อความ validation หากมีการใช้ pattern บน <input>
        if (input.validity && input.validity.patternMismatch) {
            input.setCustomValidity('ใช้ได้เฉพาะ A–Z, a–z, 0–9, _, -, . เท่านั้น');
        } else if (input.setCustomValidity) {
            input.setCustomValidity('');
        }
    });
})();