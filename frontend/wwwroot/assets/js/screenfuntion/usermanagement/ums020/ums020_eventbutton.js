async function searchButtonClicked(e) {
    e.preventDefault();
    await SearchData();
}
async function clearButtonClicked(e) {
    e.preventDefault();
    LoadDefault();
}
async function exportButtonClicked() {
    var gridexport = $(grid_inquire.grid_ID).data("kendoGrid");
    gridexport.saveAsExcel();
}
async function newdocumentButtonClicked() {
    await usergroup_dialog.new();
}
async function onSaveDialog() {
    await usergroup_dialog.onSave();
}
async function onCancelDialog() {
    var confirmationDialog = new ConfirmationDialog();
    confirmationDialog.open({
        yes: function () {
            validatadialog.reset();
            $("#usergroup-window-dialog").data("kendoWindow").close();
        }
    }, Message("Confirmation", "ConfirmExitDialog"), "Confirmation");
}
async function onSaveUserGroupMappingDialog() {
    usergroupmapping_dialog.onSave();
}
async function onCancelUserGroupMappingDialog() {
    var confirmationDialog = new ConfirmationDialog();
    confirmationDialog.open({
        yes: function () {
            $("#usergroupmapping-window-dialog").data("kendoWindow").close();
        }
    }, Message("Confirmation", "ConfirmExitDialog"), "Confirmation");

}