let ScreenCode = "UMS020"
var date = new Date(), y = date.getFullYear(), m = date.getMonth();
var firstDay = new Date(y, m, 1);
var lastDay = new Date(date.getFullYear(), date.getMonth() + 1, 0);

//Input
let sc_usergroupname, sc_username, sc_status;

//DataSource
let dataSource_status;

document.addEventListener("DOMContentLoaded", async (event) => {

    await CreateUI();
    //await grid_inquire.using([]);

});
async function CreateUI() {
    //Text Box
    $("#sc-usergroupname").kendoTextBox({

    });
    sc_usergroupname = $("#sc-usergroupname").data("kendoTextBox");

    $("#sc-username").kendoTextBox({

    });
    sc_username = $("#sc-username").data("kendoTextBox");

    //DropdownList
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
}
// 2
$(document).ready(async function () {

    await LoadDefault();
});

// 3
window.addEventListener("load", (event) => {

});
async function SearchData() {
    let flagactive;
    if (sc_status.value() != "") {
        flagactive = sc_status.value() == "0" ? false : true;
    } else {
        flagactive = null
    }
    let criteria = {
        GroupName: sc_usergroupname.value(),
        UserName: sc_username.value(),
        Status: flagactive
    }
    try {
        let DataCallApi = await APIPost(_url_callapi + "/api/auth/ums020/roles/search", criteria);
        await app.ui.clearAlert("#message-container");
        if (DataCallApi.data.length == 0) {
            Event.showWarning(Message("Warning", "DataNotFound"));
            //grid_inquire.using([]);
            grid_inquiry1.using([]);
            app.ui.uiEnable(["#export-button"], false);
            return;
        }
        //grid_inquire.using(DataCallApi.data);
        console.log(DataCallApi.data);
        grid_inquiry1.using(DataCallApi.data);
        app.ui.uiEnable(["#export-button"], true);
    } catch (e) {

        // 
    } finally {

    }
}
async function LoadDefault() {

    $("#message-container").css("display", "none");
    await ui.Input.Clear("search-container", () => {


    });
    await app.ui.clearAlert("#message-container");
    app.ui.uiEnable(["#export-button"], false);
    await grid_inquiry1.using([]);
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