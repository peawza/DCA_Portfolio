let d_password_ip_user_id = $("#d-password-user-id");
let old_password;
//Input
let d_password_ip_username, d_password_ip_name, d_password_ip_password, d_password_changepassword;

window.addEventListener("load", async (event) => {
    //console.log("load !");
    $("#d-password-ip-username").kendoTextBox({
        enable:false
    });
    d_password_ip_username = $("#d-password-ip-username").data("kendoTextBox");

    $("#d-password-ip-name").kendoTextBox({
        enable: false
    });
    d_password_ip_name = $("#d-password-ip-name").data("kendoTextBox");

    $("#d-password-ip-password").kendoTextBox({
    });
    d_password_ip_password = $("#d-password-ip-password").data("kendoTextBox");
    //$("#d-password-ip-password").on("blur", function () {
    //    changepasswordvalidatadialog.validateInput($("#d-password-ip-changepassword"));
    //});

    $("#d-password-ip-changepassword").kendoTextBox({
    });
    d_password_changepassword = $("#d-password-ip-changepassword").data("kendoTextBox");
});

function togglePassword(inputId, icon) {
    console.log("toggle=>", inputId);
    const ip = document.getElementById(inputId);
    if (!ip) return;
    ip.type = ip.type === 'password' ? 'text' : 'password';
    icon.classList.toggle('fa-eye');
    icon.classList.toggle('fa-eye-slash');
}
let changepassword_dialog_windows = {
    edit: async (data) => {
        changepasswordvalidatadialog.reset();
        await ui.Input.Clear("changepassword-window-dialog");
        d_password_ip_user_id.val(data.id);
        changepassword_dialog_windows.setValue(data);
        window_changepassword_dialog.open();
    },
    setValue: async (data) => {
        d_password_ip_username.value(data.userName);
        d_password_ip_name.value(data.firstName + ' ' + data.lastName);
    },
    getValue: () => {
        return {
            UserName: d_password_ip_username.value(),
            NewPassword: d_password_ip_password.value()
        }
    },
    onSave: async () => {
        if (!changepasswordvalidatadialog.validate()) {
            return; // ไม่ผ่านก็หยุดไว้
        }
        let result;
        try {
            let criteriaSave = changepassword_dialog_windows.getValue();
            result = await APIPost(_url_callapi + "/api/auth/ums010/admin/change/password", criteriaSave);
            result = result.data
        } catch (e) {
            console.error("Error Save =>", e.message);
        } finally {
            if (result.messageCode == "CHANGE_PASSWORD_SUCCESS") {
                await ui.Input.Clear("changepassword-window-dialog");
                await SearchData();

                showSuccess(Message("Information", "SaveSuccess"));
                changepasswordvalidatadialog.reset()
                $("#changepassword-window-dialog").data("kendoWindow").close();

            } else {

            }
        }
    }

};
let changepasswordvalidatadialog = $("#changepassword-window-dialog").kendoValidator(
    {
        rules: {
            confirmMatch: function (input) {
                if (input.is("#d-password-ip-changepassword")) {
                    const confirmVal = input.val();
                    const passVal = $("#d-password-ip-password").val();
                    // ปล่อยให้ rule "required" จัดการเองถ้ายังว่าง
                    if (!confirmVal || !passVal) return true;
                    return confirmVal === passVal;
                }
                return true;
            },
            securePassword: function (input) {
                if (input.is("[name='d-password-ip-password']")) {
                    const value = input.val();

                    // ความยาว >= 8
                    if (value.length < 8) return false;

                    // ต้องมี a-z, A-Z, 0-9, special character
                    const hasUpper = /[A-Z]/.test(value);
                    const hasLower = /[a-z]/.test(value);
                    const hasNumber = /[0-9]/.test(value);
                    const hasSpecial = /[^A-Za-z0-9]/.test(value);

                    return hasUpper && hasLower && hasNumber && hasSpecial;
                }
                return true
            }
        },
        messages: {
            required: function (input) {
                //return "Please input Start Test ."
            },
            confirmMatch: function () { },
            securePassword: function () { }
        }
        , validateOnBlur: false
    }
).data('kendoValidator');
var kendoWindow = $("#changepassword-window-dialog").kendoWindow({
    width: "50%",
    minWidth: 500,
    title: Resources("UMS012", "H001"),
    visible: false,
    modal: true,
    draggable: false,
    resizable: false,
    open: function () {
        this.center();


    }
});
setTimeout(function () {
    var titleElement = $("#changepassword-window-dialog_wnd_title"); // Select the title element
    titleElement.prepend(' <i class="fas fa-database" style="padding: 5px;"></i>');
}, 100);
$("#changepassword-window-dialog").data("kendoWindow").wrapper.append(`
            <div class="dropdown-divider m-0"></div>
            <div class="k-dialog-actions k-actions k-actions-horizontal k-actions-center ">
                <button type="button" id="windowsChangePasswordSaveButton" class="mes-button-save k-button k-button-md k-rounded-md k-button-solid k-button-solid-base" onclick="onSaveChangePasswordDialog()">
                    <span class="k-icon k-i-save k-button-icon"></span><span class="k-button-text" id="text-export">${Resources("COMMON", "SAVE")}</span>
                </button>
                <button type="button" id="windowsChangePasswordCancalButton" class="mes-button-cancel  k-button k-button-md k-rounded-md k-button-solid k-button-solid-base" onclick="onCancelChangePasswordDialog()">
                     <span class="k-icon k-i-arrow-left k-button-icon"></span><span class="k-button-text">${Resources("COMMON", "CANCEL")}</span>
                </button>
                
            </div>
        `);
let window_changepassword_dialog = $("#changepassword-window-dialog").data("kendoWindow");
$("#changepassword-window-dialog").parent().find(".k-window-action").css("visibility", "hidden");