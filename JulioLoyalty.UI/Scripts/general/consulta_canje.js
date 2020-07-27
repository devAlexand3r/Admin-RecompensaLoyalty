
$(function (e) {

    var consulta = new Object();
    consulta.urlAPI = {
        url_unificarORemplazar_tarjeta: appBaseUrl + '/api/Customer/Remplazar/Unificar/Menbresia',
    };

    consulta.webControls = {
        txtNombre: $('#nom_participante'),
        txtClave: $('#num_tarjeta'),
        txtNuevaClave: $('#nuevo_num_tarjeta'),
        selAccion: $('#selAccion'),
        btnRemplazar: $('#btnRemplazar')
    };
    consulta.realizarOperacion = function () {
        var lUser = localStorage.getItem('objUser');
        if (lUser === null) {
            swal({
                title: 'Socia no encontrada',
                text: 'Le recomendamos que seleccione el registro correspondiente',
                icon: "info",
                button: "Aceptar",
                closeOnClickOutside: false
            });
            return;
        }
        var user = JSON.parse(lUser);
        var _data = {
            participante_id: user.id,
            tranferencia: consulta.webControls.selAccion.val(),
            NoTarjeta: consulta.webControls.txtClave.val(),
            NuevaTarjeta: consulta.webControls.txtNuevaClave.val()
        }
        
        //if (user.acumula === 0) {
        //    swal({
        //        title: 'Atención',
        //        text: user.acumula_mensaje,
        //        icon: "info",
        //        button: "Aceptar",
        //        closeOnClickOutside: false
        //    });
        //    return;
        //}

        $('.dataFieldError').addClass('hidden');
        if (_data.tranferencia === "" || _data.NuevaTarjeta === "") {
            $('.dataFieldError').text('¡Es necesario registrar la información faltante!');
            $('.dataFieldError').removeClass('hidden');
            return;
        }
        
        $.ajax({
            url: this.urlAPI.url_unificarORemplazar_tarjeta,
            type: 'POST',
            async: false,
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify(_data),
            beforeSend: function () {
                consulta.webControls.btnRemplazar.attr('disabled', true);
            },
        }).done(function (data, textStatus, xhr) {
            var result = JSON.parse(data);
            if (result.length > 0) {
                if (result[0].errorId === 0) {
                    swal({
                        title: 'Atención',
                        text: result[0].mensaje,
                        icon: "success",
                        button: "Aceptar",
                        closeOnClickOutside: false
                    });
                } else {
                    swal({
                        title: 'Advertencia',
                        text: result[0].mensaje,
                        icon: "warning",
                        button: "Aceptar",
                        closeOnClickOutside: false
                    });
                }
            }
            consulta.webControls.btnRemplazar.attr('disabled', false);
        }).fail(function (data, textStatus, xhr) {
            console.log(data, textStatus, xhr);
            consulta.webControls.btnRemplazar.attr('disabled', false);
        });


    }
    consulta.iniciarComponentes = function () {

        $(".numeric").on("keypress keyup blur", function (event) {
            $(this).val($(this).val().replace(/[^\d].+/, ""));
            if ((event.which < 48 || event.which > 57)) {
                event.preventDefault();
            }
        });

        this.webControls.selAccion.selectpicker({
            language: 'es',
            title: 'No hay selección'
        }).selectpicker('refresh');

        var lUser = localStorage.getItem('objUser');
        if (lUser === null) {
            swal({
                title: 'Socia no encontrada',
                text: 'Le recomendamos que seleccione el registro correspondiente',
                icon: "info",
                button: "Aceptar",
                closeOnClickOutside: false
            });           
            return;
        }
        var user = JSON.parse(lUser);
        const nombrecompleto = user.nombre + " " + user.apellido_paterno + " " + user.apellido_materno;
        consulta.webControls.txtNombre.val(nombrecompleto);
        consulta.webControls.txtClave.val(user.clave);

        this.webControls.btnRemplazar.click(function () {
            consulta.realizarOperacion();
        });
    };
    consulta.iniciarComponentes();

});



