function fncBack() {

    window.history.back();
}
function fncNew() {

    window.location.href = "PG_UploadFile.aspx?SERVTYPE=";
}
function fncProgressBar() {

    Page_InvalidControlToBeFocused = null;

    if (typeof (Page_Validators) == "undefined") {
        return true;
    }

    var i;
    for (i = 0; i < Page_Validators.length; i++) {
        ValidatorValidate(Page_Validators[i], '', null);
    }
    ValidatorUpdateIsValid();
   // if (Page_IsValid == true)
      //   window.showModelessDialog('progress.aspx', '', 'dialogHeight: 100px; dialogWidth: 350px; edge: Raised; center: Yes; help: No; resizable: yes; status: No;scroll:yes;');
}