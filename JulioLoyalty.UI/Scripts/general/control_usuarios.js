
var Ctrl = new Object();
Ctrl.httpUrl = {
    change_password: appBaseUrl + '/Account/ChangePassword'
};
Ctrl.webControls = {
    txtUserName: $('#username'),
    txtNewPassword: $('#password'),
    txtConfPassword: $('#cpassword'),
    hiddenToken: $('#token'),
    btnChangePassword: $('#change_password')
};
Ctrl.ejecutar = function () { };
Ctrl.iniciarControles = function () {
    this.webControls.btnChangePassword.click(function () {
        Ctrl.cambiarContrasena();
    });
};
Ctrl.cambiarContrasena = function () {
    const contrasena = this.webControls.txtNewPassword.val().trim();
    const confirmarContrasena = this.webControls.txtConfPassword.val().trim();
    if (contrasena.length === 0 && confirmarContrasena.length === 0)
        return;

    if (contrasena === confirmarContrasena) {
        var data = {
            Key: this.webControls.hiddenToken.val(),
            Username: this.webControls.txtUserName.val(),
            Password: this.webControls.txtNewPassword.val()
        };
        Ctrl.ejecutar.prototype.metodo = function () {
            Ctrl.AjaxRequest(Ctrl.httpUrl.change_password, 'POST', data, Ctrl.callBack);
        };
        $swal.confirmation('Alerta', 'warning', '¿Esta seguro de cambiar su contraseña?', Ctrl);
        this.webControls.txtNewPassword.val('');
        this.webControls.txtConfPassword.val('');
    } else {
        $swal.warning('Alerta', 'La nueva contraseña no conincide');
    }
};


Ctrl.AjaxRequest = function (ajaxUrl, ajaxType, ajaxData, callback) {
    $.ajax({
        url: ajaxUrl,
        type: ajaxType,
        data: JSON.stringify(ajaxData),
        dataType: "json",
        contentType: "application/json; charset=utf-8"
    }).done(callback).fail(function (data, textStatus, xhr) {
        console.log(data, textStatus, xhr);
    });
};
Ctrl.callBack = function (data) {
    if (data.Success === true) {
        $swal.success('Atención', data.Message);
    }
    if (data.Success === false) {
        if (data.InnerException !== null) {
            $swal.error(data.Message, data.InnerException);
        } else {
            $swal.warning('Alerta', data.Message);
        }
    }
}
Ctrl.iniciarControles();
























































//function successCallback() {
//    // Do stuff before send
//}

//function successCallback() {
//    // Do stuff if success message received
//}

//function completeCallback() {
//    // Do stuff upon completion
//}

//function errorCallback() {
//    // Do stuff if error received
//}

//$.ajax({
//    url: "http://fiddle.jshell.net/favicon.png",
//    success: successCallback,
//    complete: completeCallback,
//    error: errorCallback
//});





