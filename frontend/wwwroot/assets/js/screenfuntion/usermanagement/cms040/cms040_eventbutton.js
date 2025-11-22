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
    misc_dialog_windows.new();
}
async function onSaveDialog() {
    misc_dialog_windows.onSave();
}
async function onCancelDialog() {
    var confirmationDialog = new ConfirmationDialog();
    confirmationDialog.open({
        yes: function () {
            validatadialog.reset();
            $("#misc-window-dialog").data("kendoWindow").close();
        }
    }, Message("Confirmation", "ConfirmExitDialog"), "Confirmation");
}