let ip_hiden_Mode = $("#status-mode");

//Input
let ip_module, ip_misctypename, ip_valuecode, ip_seqno, ip_valueen, ip_valuelocal, ip_valuedescription, ip_status;

//Data Source
let d_dataSource_module, d_dataSource_misctypename;

let arryDisableInputNoWorking = []
window.addEventListener("load", async (event) => {
    //DropdownList
    d_dataSource_module = await APIPost(_url_callapi + "/api/common/cms040/miscellaneous/modules", {});
    $("#ip-module").kendoDropDownList({
        dataSource: d_dataSource_module.data,
        filter: "contains",
        //minLength: 1,
        dataTextField: "modulesNameDisplay",
        dataValueField: "modulesCode",
        optionLabel: Resources("COMMON", "DropDownSelect"),
        change: async function (e) {
            let misctype = this.value();
            await DialogModuleToMiscType(misctype)
            //console.log("value module select =>", this,this.text(), this.value());
        }
    });
    ip_module = $("#ip-module").data("kendoDropDownList");

    d_dataSource_misctypename = await APIPost(_url_callapi + "/api/common/cms040/miscellaneous/type", {});
    $("#ip-miscellaneoustype").kendoDropDownList({
        dataSource: d_dataSource_misctypename.data,
        filter: "contains",
        //minLength: 1,
        dataTextField: "miscTypeName",
        dataValueField: "miscTypeCode"
        , optionLabel: Resources("COMMON", "DropDownSelect"),
    });
    ip_misctypename = $("#ip-miscellaneoustype").data("kendoDropDownList");

    //TextBox
    $("#ip-valuecode").kendoTextBox({

    });
    ip_valuecode = $("#ip-valuecode").data("kendoTextBox");

    $("#ip-valueen").kendoTextBox({

    });
    ip_valueen = $("#ip-valueen").data("kendoTextBox");

    $("#ip-valuelocal").kendoTextBox({

    });
    ip_valuelocal = $("#ip-valuelocal").data("kendoTextBox");

    $("#ip-description").kendoTextArea({
        rows: 2,
    });
    ip_valuedescription = $("#ip-description").data("kendoTextArea");

    //NumericTextBox
    $("#ip-seqno").kendoNumericTextBox({
        decimals: 0,
        spinners: false,
        format: "n0"
    });
    ip_seqno = $("#ip-seqno").data("kendoNumericTextBox");

    //Switch
    $("#ip-status").kendoSwitch({
    });

    ip_status = $("#ip-status").data("kendoSwitch");

});

let misc_dialog_windows = {
    new: async (e, data) => {
        ui.Input.DisableCondition("misc-window-dialog", true, arryDisableInputNoWorking)
        ui.display("windowsSaveButton");
        validatadialog.reset();
        await ui.Input.Clear("misc-window-dialog");
        ip_hiden_Mode.val("new");
        await misc_dialog_windows.setValue({});
        window_misc_dialog.open();
    },
    edit: async (data) => {
        ui.Input.DisableCondition("misc-window-dialog", true, arryDisableInputNoWorking)
        ui.display("windowsSaveButton");
        validatadialog.reset();
        await ui.Input.Clear("misc-window-dialog");
        ip_hiden_Mode.val("edit");
        await misc_dialog_windows.setValue(data);
        ip_module.enable(false);
        ip_misctypename.enable(false);
        ip_valuecode.enable(false);
        window_misc_dialog.open();
    },
    view: async (data) => {
        ui.Input.DisableCondition("misc-window-dialog", false, arryDisableInputNoWorking)
        ui.hiden("windowsSaveButton");
        await ui.Input.Clear("misc-window-dialog");
        ip_hiden_Mode.val("view");
        await misc_dialog_windows.setValue(data);
        
        window_misc_dialog.open();
    },
    setValue: async (data) => {

        if (ip_hiden_Mode.val() == "edit" || ip_hiden_Mode.val() == "view") {
            //console.log("data for edit=>", data);
            ip_module.value(data.moduleCode);
            ip_misctypename.value(data.miscTypeCode);
            ip_valuecode.value(data.miscCode);
            ip_valueen.value(data.value1);
            ip_valuelocal.value(data.value2);
            ip_valuedescription.value(data.description);
            ip_seqno.value(data.seqNo);
            ip_status.value(data.activeFlag);
        } else {
            ip_status.value(true)
        }
    },
    getValue: () => {
        return {
            ModulesCode: ip_module.value(),
            MiscTypeCode: ip_misctypename.value(),
            MiscCode: ip_valuecode.value(),
            Value1: ip_valueen.value(),
            Value2: ip_valuelocal.value(),
            SeqNo: ip_seqno.value(),
            Description: ip_valuedescription.value(),
            Status: ip_status.value(),
            CreatedBy:_user_create_by
        }
    },
    onSave:async () => {
        if (!validatadialog.validate()) {
            return;
        }
        let result;
        try {
            let StatusMode = $("#status-mode").val();

            let criteriaSave = misc_dialog_windows.getValue()
            console.log("Save criteria=>", criteriaSave);
            if (StatusMode == "new") {
                result = await APIPost(_url_callapi + "/api/common/cms040/insert", criteriaSave);
            } else {
                criteriaSave.updatedBy = _user_update_by;
                result = await APIPost(_url_callapi + "/api/common/cms040/update", criteriaSave);
            }
            result = result.data
        } catch (e) {
            console.error("Error Save =>", e.message);
        } finally {
            if (result.messageCode == "S001" || result.messageCode == "UpdateSuccess") {
                await ui.Input.Clear("user-window-dialog");
                await SearchData();

                showSuccess(Message("Information", "SaveSuccess"));
                validatadialog.reset()
                $("#misc-window-dialog").data("kendoWindow").close();

            } else {
                if (result.messageCode == "E002") {
                    messageDialog.error(result.messageName, () => { });
                    validatadialog.reset()
                }
            }
        }
    }
}
async function DialogModuleToMiscType(modulecode) {
    let list_misctype = d_dataSource_misctypename.data;
    if (modulecode != '') {
        list_misctype = list_misctype.filter(item =>
            item.modules.split(",").map(m => m.trim()).includes(modulecode));
    }
    ip_misctypename.setDataSource(list_misctype);
}
let validatadialog = $("#misc-window-dialog").kendoValidator(
    {
        messages: {
            required: function (input) {
                //return "Please input Start Test ."
            }
        }
        , validateOnBlur: false
    }
).data('kendoValidator');
var kendoWindow = $("#misc-window-dialog").kendoWindow({
    width: "50%",
    minWidth: 500,
    title: Resources("CMS041", "H001"),
    visible: false,
    modal: true,
    draggable: false,
    resizable: false,
    open: function () {
        this.center();


    }
});
setTimeout(function () {
    var titleElement = $("#misc-window-dialog_wnd_title"); // Select the title element
    titleElement.prepend(' <i class="fas fa-database" style="padding: 5px;"></i>');
}, 100);
$("#misc-window-dialog").data("kendoWindow").wrapper.append(`
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
let window_misc_dialog = $("#misc-window-dialog").data("kendoWindow");
$("#misc-window-dialog").parent().find(".k-window-action").css("visibility", "hidden");