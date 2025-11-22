let ScreenCode = "CMS040"
var date = new Date(), y = date.getFullYear(), m = date.getMonth();
var firstDay = new Date(y, m, 1);
var lastDay = new Date(date.getFullYear(), date.getMonth() + 1, 0);

// Input  Search
let sc_module, sc_status, sc_misctype, sc_miscvalue;

//Data Source
let dataSource_module, dataSource_status, dataSource_misctype;

document.addEventListener("DOMContentLoaded", async (event) => {

    await CreateUI();
    //await grid_inquire.using([]);

});

async function CreateUI() {
    //DropdownList
    dataSource_module = await APIPost(_url_callapi + "/api/common/cms040/miscellaneous/modules", {});
    $("#sc-module").kendoDropDownList({
        dataSource: dataSource_module.data,
        filter: "contains",
        //minLength: 1,
        dataTextField: "modulesNameDisplay",
        dataValueField: "modulesCode",
        optionLabel: Resources("COMMON", "DropDownAll"),
        change: async function (e) {
            let misctype = this.value();
            await ModuleToMiscType(misctype)
            //console.log("value module select =>", this,this.text(), this.value());
        }
    });
    sc_module = $("#sc-module").data("kendoDropDownList");

    dataSource_status = await APIPost(_url_callapi + "/api/common/miscellaneous", { "MiscTypeCode": "Status", "Status": 1 })
    $("#sc-status").kendoDropDownList({
        dataSource: dataSource_status.data,
        filter: "contains",
        //minLength: 1,
        dataTextField: "displayName",
        dataValueField: "miscCode"
        , optionLabel: Resources("COMMON", "DropDownAll"),
    });
    sc_status = $("#sc-status").data("kendoDropDownList");

    dataSource_misctype = await APIPost(_url_callapi + "/api/common/cms040/miscellaneous/type", {});
    $("#sc-miscellaneoustype").kendoDropDownList({
        dataSource: dataSource_misctype.data,
        filter: "contains",
        //minLength: 1,
        dataTextField: "miscTypeName",
        dataValueField: "miscTypeCode"
        , optionLabel: Resources("COMMON", "DropDownAll"),
    });
    sc_misctype = $("#sc-miscellaneoustype").data("kendoDropDownList");

    //Text Box
    $("#sc-miscellaneousvalue").kendoTextBox({

    });
    sc_miscvalue = $("#sc-miscellaneousvalue").data("kendoTextBox");
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
        ModulesCode: sc_module.value(),
        Status: flagactive,
        MiscType: sc_misctype.value(),
        MiscValue: sc_miscvalue.value()
    }

    try {
        let DataCallApi = await APIPost(_url_callapi + "/api/common/cms040/search", criteria);
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
async function ModuleToMiscType(modulecode) {
    console.log("Misc Type =>", dataSource_misctype.data, modulecode);
    let list_misctype = dataSource_misctype.data;
    if (modulecode != '') {
        list_misctype = list_misctype.filter(item =>
            item.modules.split(",").map(m => m.trim()).includes(modulecode));
    }
    sc_misctype.setDataSource(list_misctype);
}

async function defaultData() {
    var listobjData = [];
    var dataobj = {
        data: listobjData,
        resultCode: "SUCCESS",
        resultMessage: "The operation was successful.",
        resultStatus: true
    }
    return dataobj;
}