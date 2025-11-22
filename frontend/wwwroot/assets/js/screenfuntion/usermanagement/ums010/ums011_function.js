let ip_hiden_Mode = $("#status-mode");
let ip_user_id = $("#user-id");

//Input
let ip_username, ip_firstname, ip_lastname, ip_department, ip_position, ip_email, ip_phonenumber, ip_status;

//DataSource
let d_dataSource_department, d_dataSource_position;

let arryDisableInputNoWorking = [];
window.addEventListener("load", async (event) => {
    //console.log("load !");
    $("#ip-username").kendoTextBox({

    });
    ip_username = $("#ip-username").data("kendoTextBox");

    $("#ip-firstname").kendoTextBox({

    });
    ip_firstname = $("#ip-firstname").data("kendoTextBox");

    $("#ip-lastname").kendoTextBox({

    });
    ip_lastname = $("#ip-lastname").data("kendoTextBox");

    d_dataSource_department = await defaultData();
    d_dataSource_department = await APIPost(_url_callapi + "/api/auth/common/departments", {})
    $("#ip-department").kendoDropDownList({
        dataSource: d_dataSource_department.data,
        filter: "contains",
        //minLength: 1,
        dataTextField: "departmentName",
        dataValueField: "departmentCode"
        , optionLabel: Resources("COMMON", "DropDownSelect"),
    });
    ip_department = $("#ip-department").data("kendoDropDownList");

    d_dataSource_position = await defaultData();
    d_dataSource_position = await APIPost(_url_callapi + "/api/auth/common/positions", {})
    $("#ip-position").kendoDropDownList({
        dataSource: d_dataSource_position.data,
        filter: "contains",
        //minLength: 1,
        dataTextField: "positionName",
        dataValueField: "positionCode"
        , optionLabel: Resources("COMMON", "DropDownSelect"),
    });
    ip_position = $("#ip-position").data("kendoDropDownList");

    $("#ip-email").kendoTextBox({

    });
    ip_email = $("#ip-email").data("kendoTextBox");

    $("#ip-phonenumber").kendoTextBox({

    });
    ip_phonenumber = $("#ip-phonenumber").data("kendoTextBox");

    $("#ip-status").kendoSwitch({
    });

    ip_status = $("#ip-status").data("kendoSwitch");
});

let user_dialog_windows = {
    new: async (e, data) => {
        ui.Input.DisableCondition("user-window-dialog", true, arryDisableInputNoWorking)
        ui.display("windowsSaveButton");
        validatadialog.reset();
        await ui.Input.Clear("user-window-dialog");
        ip_hiden_Mode.val("new");
        ip_user_id.val('');
        await user_dialog_windows.setValue({});
        window_user_dialog.open();
    },
    edit: async (data) => {
        ui.Input.DisableCondition("user-window-dialog", true, arryDisableInputNoWorking)
        ui.display("windowsSaveButton");
        validatadialog.reset();
        await ui.Input.Clear("user-window-dialog");
        ip_hiden_Mode.val("edit");
        ip_user_id.val(data.id);
        user_dialog_windows.setValue(data);
        window_user_dialog.open();
    },
    view: async (data) => {
        ui.Input.DisableCondition("user-window-dialog", false, arryDisableInputNoWorking)
        ui.hiden("windowsSaveButton");
        validatadialog.reset();
        await ui.Input.Clear("user-window-dialog");
        ip_hiden_Mode.val("view");
        ip_user_id.val(data.id);
        user_dialog_windows.setValue(data);
        window_user_dialog.open();
    },
    setValue: async(data) => {
        if (ip_hiden_Mode.val() == "edit" || ip_hiden_Mode.val() == "view") {
            //console.log("data for edit=>", data);
            ip_username.enable(false);
            ip_username.value(data.userName);
            ip_firstname.value(data.firstName);
            ip_lastname.value(data.lastName);
            ip_department.value(data.departmentCode);
            ip_position.value(data.positionCode);
            ip_email.value(data.email);
            ip_phonenumber.value(data.phoneNumber);
            ip_status.value(data.activeFlag);
        } else {
            ip_status.value(true)
        }
    },
    getValue: () => {
        return {
            UserName: ip_username.value(),
            FirstName: ip_firstname.value(),
            LastName: ip_lastname.value(),
            DepartmentCode: ip_department.value(),
            PositionCode: ip_position.value(),
            Email: ip_email.value(),
            PhoneNumer: ip_phonenumber.value(),
            ActiveFlag: ip_status.value(),
            FirstLoginFlag: false,
            SystemAdminFlag: false,
            LanguageCode:null
        }
    },
    onSave: async () =>{
        if (!validatadialog.validate()) {
            return;
        }
        let result
        try {
            let StatusMode = $("#status-mode").val();

            let criteriaSave = user_dialog_windows.getValue()
            if (StatusMode == "new") {
                criteriaSave.LanguageCode = "en";
                console.log("Criteria Save=>", criteriaSave);
                result = await APIPost(_url_callapi + "/api/auth/ums010/create", criteriaSave);

            } else if (StatusMode == "edit") {
                criteriaSave.Id = ip_user_id.val();
                //console.log("Criteria Save Update=>", criteriaSave);
                result = await APIPost(_url_callapi + "/api/auth/ums010/update", criteriaSave);

                //add Update Index Data
            }
            result = result.data
        } catch (e) {
            console.error("Error Save =>", e.message);
        } finally {
            if (result.messageCode == "UMS010_CREATE_SUCCESS" || result.messageCode == "UMS010_UPDATE_SUCCESS") {
                await ui.Input.Clear("user-window-dialog");
                await SearchData();

                showSuccess(Message("Information", "SaveSuccess"));
                validatadialog.reset()
                $("#user-window-dialog").data("kendoWindow").close();

            } else {

            }
        }
    }
}
let validatadialog = $("#user-window-dialog").kendoValidator(
    {
        messages: {
            required: function (input) {
                //return "Please input Start Test ."
            }
        }
        , validateOnBlur: false
    }
).data('kendoValidator');
var kendoWindow = $("#user-window-dialog").kendoWindow({
    width: "50%",
    minWidth: 500,
    title: Resources("UMS011", "H001"),
    visible: false,
    modal: true,
    draggable: false,
    resizable: false,
    open: function () {
        this.center();


    }
});
setTimeout(function () {
    var titleElement = $("#user-window-dialog_wnd_title"); // Select the title element
    titleElement.prepend(' <i class="fas fa-database" style="padding: 5px;"></i>');
}, 100);
$("#user-window-dialog").data("kendoWindow").wrapper.append(`
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
let window_user_dialog = $("#user-window-dialog").data("kendoWindow");
$("#user-window-dialog").parent().find(".k-window-action").css("visibility", "hidden");