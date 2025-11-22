async function searchButtonClicked(e) {
    e.preventDefault();
    await SearchData();
}
async function clearButtonClicked(e) {
    e.preventDefault();
    LoadDefault();
}
function newdocumentButtonClicked() {
    user_dialog_windows.new();
}
async function exportButtonClicked() {
    var gridexport = $(grid_inquire.grid_ID).data("kendoGrid");
    gridexport.saveAsExcel();

}
async function onSaveDialog() {
    await user_dialog_windows.onSave();
}
async function onCancelDialog() {
    var confirmationDialog = new ConfirmationDialog();
    confirmationDialog.open({
        yes: function () {
            validatadialog.reset();
            $("#user-window-dialog").data("kendoWindow").close();
        }
    }, Message("Confirmation", "ConfirmExitDialog"), "Confirmation");
}
async function onSaveChangePasswordDialog() {
    await changepassword_dialog_windows.onSave();
}
async function onCancelChangePasswordDialog() {
    var confirmationDialog = new ConfirmationDialog();
    confirmationDialog.open({
        yes: function () {
            validatadialog.reset();
            $("#changepassword-window-dialog").data("kendoWindow").close();
        }
    }, Message("Confirmation", "ConfirmExitDialog"), "Confirmation");
}