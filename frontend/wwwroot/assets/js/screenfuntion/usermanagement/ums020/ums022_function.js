let ip_usgmapping_usergroup_id = $("#usgmapping_usergroup-id");

//Input
let ip_usgmapping_usergroupname, ip_usgmapping_description;

//DataSource
let dataSource_useroutrole, dataSource_userinrole;

window.addEventListener("load", async (event) => {
    //Text Box
    $("#ip-usgmapping-usergroupname").kendoTextBox({

    });
    ip_usgmapping_usergroupname = $("#ip-usgmapping-usergroupname").data("kendoTextBox");

    //Text Area
    $("#ip-usgmapping-description").kendoTextArea({
        rows: 2,
    });
    ip_usgmapping_description = $("#ip-usgmapping-description").data("kendoTextArea");

    //Button
    $("#transferButtonLR").kendoButton({
        icon: "caret-alt-right",
        themeColor: "primary",
        text: false,
        click: async function (e) {
            TransferLeftToRight();
        }
    });

    $("#transferButtonRL").kendoButton({
        icon: "caret-alt-left",
        themeColor: "primary",
        text: false,
        click: async function (e) {
            TransferRightToLeft();
        }
    });

    $("#transferButtonLRAll").kendoButton({
        icon: "caret-double-alt-right",
        themeColor: "primary",
        text: false,
        click: async function (e) {
            TransferAllLeftToRight();
        }
    });

    $("#transferButtonRLAll").kendoButton({
        icon: "caret-double-alt-left",
        themeColor: "primary",
        text: false,
        click: async function (e) {
            TransferAllRightToLeft();
        }
    });
});
async function TransferLeftToRight() {
    var optionalGrid = $("#optionalGrid").data("kendoGrid");
    var selectedGrid = $("#selectedGrid").data("kendoGrid");
    var selectedItems = optionalGrid.select();
    console.log(selectedItems);
    var itemsToRemove = [];

    selectedItems.each(function (index, element) {
        console.log(index, element);
        var dataItem = optionalGrid.dataItem(element);
        if (dataItem) {
            itemsToRemove.push(dataItem);
        }
    });

    for (var i = 0; i < itemsToRemove.length; i++) {
        optionalGrid.dataSource.remove(itemsToRemove[i]);
        selectedGrid.dataSource.add(itemsToRemove[i]);
    }
}
async function TransferRightToLeft() {
    var optionalGrid = $("#optionalGrid").data("kendoGrid");
    var selectedGrid = $("#selectedGrid").data("kendoGrid");
    var selectedItems = selectedGrid.select();
    console.log(selectedItems);
    var itemsToRemove = [];

    selectedItems.each(function (index, element) {
        console.log(index, element);
        var dataItem = selectedGrid.dataItem(element);
        if (dataItem) {
            itemsToRemove.push(dataItem);
        }
    });

    for (var i = 0; i < itemsToRemove.length; i++) {
        selectedGrid.dataSource.remove(itemsToRemove[i]);
        optionalGrid.dataSource.add(itemsToRemove[i]);
    }
}
async function TransferAllLeftToRight() {
    var optionalGrid = $("#optionalGrid").data("kendoGrid");
    var selectedGrid = $("#selectedGrid").data("kendoGrid");
    var optionalGridData = optionalGrid.dataSource.data().toJSON();
    if (optionalGridData.length > 0) {
        for (var i = 0; i < optionalGridData.length; i++) {
            selectedGrid.dataSource.add(optionalGridData[i]);
        }
        optionalGrid.dataSource.data([]);
        selectedGrid.dataSource.sync();
    }
}
async function TransferAllRightToLeft() {
    var optionalGrid = $("#optionalGrid").data("kendoGrid");
    var selectedGrid = $("#selectedGrid").data("kendoGrid");
    var selectedGridData = selectedGrid.dataSource.data().toJSON();
    if (selectedGridData.length > 0) {
        for (var i = 0; i < selectedGridData.length; i++) {
            optionalGrid.dataSource.add(selectedGridData[i]);
        }
        selectedGrid.dataSource.data([]);
        optionalGrid.dataSource.sync();
    }
}

let usergroupmapping_dialog = {
    edit: async (data) => {
        await ui.Input.Clear("usergroupmapping-window-dialog");
        //ip_hiden_Mode.val("edit");
        ip_usgmapping_usergroup_id.val(data.id);
        await usergroupmapping_dialog.setValue(data);
        window_usergroupmapping_dialog.open();
    },
    setValue: async (data) => {
        let criteria = {
            RoleId : data.id
        }
        let resultApi = await APIPost(_url_callapi + "/api/auth/ums020/roles/usermapping/search", criteria);
        ip_usgmapping_usergroup_id.val(data.id);
        ip_usgmapping_usergroupname.value(data.name);
        ip_usgmapping_description.value(data.description);
        grid_userlist.using(resultApi.data.userListOutRole);
        grid_userlistforsection.using(resultApi.data.userListInRole);
    },
    onSave: async () => {
        var data = $("#selectedGrid").data("kendoGrid").dataSource.data();
        var jsonData = JSON.stringify(data);
        var userIds = data.map(x => x.userId);
        let result;
        let criteriaSave = {
            RoleId: ip_usgmapping_usergroup_id.val(),
            Users: userIds,
            User : _user_create_by
        }
        console.log("On Save =>", criteriaSave);
        try {
            result = await APIPost(_url_callapi + "/api/auth/ums020/roles/users/add", criteriaSave);
            result = result.data
        } catch (e) {

        } finally {
            if (result.messageCode == "USR000") {
                await ui.Input.Clear("usergroupmapping-window-dialog");
                await SearchData();

                showSuccess(Message("Information", "SaveSuccess"));
                validatadialog.reset()
                $("#usergroupmapping-window-dialog").data("kendoWindow").close();

            }
        }
    }
}
var kendoWindowums022 = $("#usergroupmapping-window-dialog").kendoWindow({
    width: "70%",
    minWidth: 500,
    title: Resources("UMS022", "H001"),
    visible: false,
    modal: true,
    draggable: false,
    resizable: false,
    open: function () {
        this.center();


    }
});
let window_usergroupmapping_dialog = $("#usergroupmapping-window-dialog").data("kendoWindow");
setTimeout(function () {
    var titleElement = $("#usergroupmapping-window-dialog_wnd_title"); // Select the title element
    titleElement.prepend(' <i class="fas fa-database" style="padding: 5px;"></i>');
}, 100);
$("#usergroupmapping-window-dialog").data("kendoWindow").wrapper.append(`
    <div class="dropdown-divider m-0"></div>
    <div class="k-dialog-actions k-actions k-actions-horizontal k-actions-center ">
        <button type="button" id="windowsSaveButton" class="mes-button-save k-button k-button-md k-rounded-md k-button-solid k-button-solid-base" onclick="onSaveUserGroupMappingDialog()">
            <span class="k-icon k-i-save k-button-icon"></span><span class="k-button-text" id="text-export">${Resources("COMMON", "SAVE")}</span>
        </button>
        <button type="button" id="windowsCancalButton" class="mes-button-cancel  k-button k-button-md k-rounded-md k-button-solid k-button-solid-base" onclick="onCancelUserGroupMappingDialog()">
                <span class="k-icon k-i-arrow-left k-button-icon"></span><span class="k-button-text">${Resources("COMMON", "CANCEL")}</span>
        </button>
                
    </div>
`);
$("#usergroupmapping-window-dialog").parent().find(".k-window-action").css("visibility", "hidden");