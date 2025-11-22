let ip_hiden_Mode = $("#status-mode");
let ip_usergroup_id = $("#usergroup-id");

//Input
let ip_usergroupname, ip_description, ip_status;
let arryDisableInputNoWorking = [];
window.addEventListener("load", async (event) => {
    //Text Box
    $("#ip-usergroupname").kendoTextBox({

    });
    ip_usergroupname = $("#ip-usergroupname").data("kendoTextBox");

    //Text Area
    $("#ip-description").kendoTextArea({
        rows: 2,
    });
    ip_description = $("#ip-description").data("kendoTextArea");

    //Switch
    $("#ip-status").kendoSwitch({
    });
    ip_status = $("#ip-status").data("kendoSwitch");

    
});
let validatadialog = $("#usergroup-window-dialog").kendoValidator(
    {
        messages: {
            required: function (input) {
                //return "Please input Start Test ."
            }
        }
        , validateOnBlur: false
    }
).data('kendoValidator');

let usergroup_dialog = {
    new: async (e, data) => {
        ui.Input.DisableCondition("usergroup-window-dialog", true, arryDisableInputNoWorking)
        ui.display("windowsSaveButton");
        validatadialog.reset();
        await ui.Input.Clear("usergroup-window-dialog");
        ip_hiden_Mode.val("new");
        ip_usergroup_id.val('');
        await usergroup_dialog.setValue({});
        window_usergroup_dialog.open();

    },
    edit: async (data) => {
        ui.Input.DisableCondition("usergroup-window-dialog", true, arryDisableInputNoWorking)
        ui.display("windowsSaveButton");
        validatadialog.reset();
        await ui.Input.Clear("usergroup-window-dialog");
        ip_hiden_Mode.val("edit");
        ip_usergroup_id.val(data.id);
        ip_usergroupname.enable(false);
        await usergroup_dialog.setValue(data);
        window_usergroup_dialog.open();
    },
    view: async (data) => {
        ui.Input.DisableCondition("usergroup-window-dialog", false, arryDisableInputNoWorking)
        ui.hiden("windowsSaveButton");
        await ui.Input.Clear("usergroup-window-dialog");
        ip_hiden_Mode.val("view");
        ip_usergroup_id.val(data.id);
        await usergroup_dialog.setValue(data);

        window_usergroup_dialog.open();
    },
    getValue: () => {
        return {
            Name: ip_usergroupname.value(),
            Description: ip_description.value(),
            IsActive: ip_status.value()
        }
    },
    setValue: async (data) => {
        if (ip_hiden_Mode.val() == "edit" || ip_hiden_Mode.val() == "view") {
            ip_usergroupname.value(data.name);
            ip_description.value(data.description);
            ip_status.value(data.isActive);
        } else {
            ip_status.value(true)
        }
    },
    onSave: async () => {
        //console.log("Check validate=>", validatadialog);
        if (!validatadialog.validate()) {
            return;
        }
        let result
        try {
            let StatusMode = $("#status-mode").val();
            let criteriaSave = usergroup_dialog.getValue()
            if (StatusMode == "new") {
                console.log("Criteria Save=>", criteriaSave);
                result = await APIPost(_url_callapi + "/api/auth/ums020/roles/add", criteriaSave);

            } else if (StatusMode == "edit") {
                criteriaSave.Id = ip_usergroup_id.val();
                console.log("Criteria Save Update=>", criteriaSave);
                result = await APIPost(_url_callapi + "/api/auth/ums020/roles/update", criteriaSave);

            }
            result = result.data
        } catch (e) {
            console.error("Error Save =>", e.message);
        } finally {
            if (result.messageCode == "ROLE000" || result.messageCode == "ROLE005") {
                await ui.Input.Clear("usergroup-window-dialog");
                await SearchData();

                showSuccess(Message("Information", "SaveSuccess"));
                validatadialog.reset()
                $("#usergroup-window-dialog").data("kendoWindow").close();

            } else if (result.messageCode == "ROLE002") {
                //await ui.Input.Clear("usergroup-window-dialog");
                //await SearchData();

                messageDialog.error(Message("Warning", "UserGroupDuplicate"), () => { });
                validatadialog.reset()
            }
        }
    }
}
var kendoWindow = $("#usergroup-window-dialog").kendoWindow({
    width: "30%",
    minWidth: 500,
    title: Resources("UMS021", "H001"),
    visible: false,
    modal: true,
    draggable: false,
    resizable: false,
    open: function () {
        this.center();


    }
});
let window_usergroup_dialog = $("#usergroup-window-dialog").data("kendoWindow");
setTimeout(function () {
    var titleElement = $("#usergroup-window-dialog_wnd_title"); // Select the title element
    titleElement.prepend(' <i class="fas fa-database" style="padding: 5px;"></i>');
}, 100);
$("#usergroup-window-dialog").data("kendoWindow").wrapper.append(`
    <div class="dropdown-divider m-0"></div>
    <div class="k-dialog-actions k-actions k-actions-horizontal k-actions-center ">
        <button type="button" id="windowsSaveButton" class="mes-button-save k-button k-button-md k-rounded-md k-button-solid k-button-solid-base" onclick="onSaveDialog()">
            <span class="k-icon k-i-save k-button-icon"></span><span class="k-button-text" id="text-export">${Resources("COMMON", "SAVE")}</span>
        </button>
        <button type="button" id="windowsCancalButton" class="mes-button-cancel  k-button k-button-md k-rounded-md k-button-solid k-button-solid-base" onclick="onCancelDialog()">
                <span class="k-icon k-i-arrow-left k-button-icon"></span><span class="k-button-text">${Resources("COMMON", "CANCEL")}</span>
        </button>
                
    </div>
`);
$("#usergroup-window-dialog").parent().find(".k-window-action").css("visibility", "hidden");

