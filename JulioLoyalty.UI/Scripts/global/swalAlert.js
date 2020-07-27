
var $swal = {
    success: function (title, message) {
        swal({
            title: title,
            type: 'success',
            html: message,
            confirmButtonColor: '#11ACA6',
            confirmButtonText: 'Aceptar',
            allowOutsideClick: false,
            customClass: 'sweetalert-sm'
        });
    },
    info: function (title, message) {
        swal({
            title: title,
            type: 'info',
            html: message,
            confirmButtonColor: '#11ACA6',
            confirmButtonText: 'Aceptar',
            allowOutsideClick: false
        });
    },
    warning: function (title, message) {
        swal({
            title: title,
            type: 'warning',
            html: message,
            confirmButtonColor: '#11ACA6',
            confirmButtonText: 'Aceptar',
            allowOutsideClick: false
        });
    },
    error: function (message, error) {
        swal({
            title: 'Error',
            type: 'error',
            html: message + ' <br /> >_ ' + error,
            confirmButtonColor: '#11ACA6',
            confirmButtonText: 'Aceptar',
            allowOutsideClick: false
        });
    },

    confirmation: function (title, type, html, objeto) {
        swal({
            title: title,
            type: type,
            html: html,
            showCancelButton: true,
            confirmButtonColor: '#11ACA6',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Aceptar',
            cancelButtonText: "Cancelar",
        }).then((result) => {
            if (result.value) {
                var obj = new objeto.ejecutar().metodo();
            }
        });
    }

}