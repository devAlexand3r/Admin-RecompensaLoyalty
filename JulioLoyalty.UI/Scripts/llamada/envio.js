$(function (e) {
    var envio = new Object();
    envio.urlAPI = {
        url_participante_buscar: appBaseUrl + '/api/Customer/FindByUser',
        url_participante_envio: appBaseUrl + '/api/Llamada/Envio'
    };

    envio.webControls = {
        hiddenId: $('#participante_id'),
        txtParticipante: $('#participante'),
        txtCorreo: $('#correo'),
        txtAsunto: $('#asunto'),
        txtMensaje: $('#mensaje'),
        btnEnviar: $('#btnEnviar')
    };

    envio.iniciar = function () {
        this.agregarEventos();
        this.obtenerParticipante();
    };

    envio.Datos = {
        participante: false,
        correo: false,
        asunto: false,
        mensaje: false
    };

    envio.agregarEventos = function () {
        this.webControls.txtParticipante.change(function () {
            envio.Datos.participante = envio.Requerido(this, 'text');
        });
        this.webControls.txtCorreo.change(function () {          
            envio.Datos.correo = envio.Requerido(this, 'email');          
        });
        this.webControls.txtAsunto.change(function () {
            envio.Datos.asunto = envio.Requerido(this, 'text');
        });
        this.webControls.txtMensaje.change(function () {
            envio.Datos.mensaje = envio.Requerido(this, 'text');
        });

        this.webControls.btnEnviar.click(function () {
            envio.webControls.txtParticipante.trigger('change');
            envio.webControls.txtCorreo.trigger('change');
            envio.webControls.txtAsunto.trigger('change');
            envio.webControls.txtMensaje.trigger('change');
            var _valido = envio.Datos;
            if (_valido.participante === false ||
                _valido.correo === false ||
                _valido.asunto === false ||
                _valido.mensaje === false
               ) {
                return;
            }
            // Proceso de enviar mensaje            
            var _data = envio.obtenerDatos();
            envio.envio(_data, envio.urlAPI.url_participante_envio);
            return;
        });
    }

    envio.Requerido = function (thisObj, type) {
        var valido = true;
        $("div[data-valmsg-for=" + thisObj.name + "]").addClass('hidden');
        if (type === 'text') {
            if (thisObj.value.length < 3) {
                $("div[data-valmsg-for=" + thisObj.name + "]").removeClass('hidden');
                $("[name=" + thisObj.name + "]").addClass('is-invalid');
                valido = false;
            }
        }
        if (type === 'email') {
            var vregexNaix = /^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$/;
            if (!vregexNaix.test(thisObj.value) || thisObj.value.length < 5) {
                $("div[data-valmsg-for=" + thisObj.name + "]").removeClass('hidden');
                $("[name=" + thisObj.name + "]").addClass('is-invalid');
                valido = false;
            }
        }
        if (valido === true) {
            $("[name=" + thisObj.name + "]").removeClass('is-invalid');
            $("[name=" + thisObj.name + "]").addClass('is-valid');
        }
        return valido;
    };

    envio.obtenerParticipante = function () {
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
        $.ajax({
            url: envio.urlAPI.url_participante_buscar,
            type: 'GET',
            data: { id: user.id },
            async: true,
            beforeSend: function () {
                $('.loading').removeClass('hidden');
            },
            success: function (data, status, xhr) {
                envio.plasmarDatos(data[0]);
                $('.card').removeClass('hidden');
                $('.loading').addClass('hidden');
                $('.bootstrap-select').removeClass('is-valid is-invalid');
            },
            error: function (data, status, xhr) {
                console.log(data);
            }
        });
    };

    envio.plasmarDatos = function (cliente) {
        var nombre_completo;
        if (cliente.nombre !== "")
            nombre_completo = cliente.nombre.trim();
        if (cliente.apellido_paterno !== "")
            nombre_completo = (nombre_completo + " " + cliente.apellido_paterno).trim();
        if (cliente.apellido_materno !== "")
            nombre_completo = (nombre_completo + " " + cliente.apellido_materno).trim();
        this.webControls.hiddenId.val(cliente.id);
        this.webControls.txtParticipante.val(nombre_completo);
        this.webControls.txtCorreo.val(cliente.correo_electronico);

        if (nombre_completo === undefined) // Si no viene el nombre que habilite el textbox del nombre el participante, para que pueda ser editado
            this.webControls.txtParticipante.prop("disabled", false);
        else
            this.webControls.txtParticipante.prop("disabled", true);

        if (cliente.correo_electronico === null || cliente.correo_electronico.trim() === "") // Si no viene el correo que habilite el textbox del correo del participante, para que pueda ser editado
            this.webControls.txtCorreo.prop("disabled", false);
        else
            this.webControls.txtCorreo.prop("disabled", true);

        this.webControls.txtAsunto.val("");
        this.webControls.txtMensaje.val("");
    };

    envio.obtenerDatos = function () {
        var datos = {
            Participante_Id: this.webControls.hiddenId.val().trim(),
            Participante: this.webControls.txtParticipante.val().trim(),
            Correo: this.webControls.txtCorreo.val().trim(),
            Asunto: this.webControls.txtAsunto.val().trim(),
            Mensaje: this.webControls.txtMensaje.val().trim()
        }
        return datos;
    };

    envio.limpiar = function () {
        this.webControls.txtAsunto.val("");
        this.webControls.txtMensaje.val("");
    }

    envio.envio = function (datos, urlDestino) {
        $.ajax({
            url: urlDestino,
            type: 'POST',
            async: false,
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify(datos),
            beforeSend: function () {
                envio.webControls.btnEnviar.attr('disabled', true);
            },
        }).done(function (data, textStatus, xhr) {
            if (data.Success === false) {
                swal({
                    title: "Alerta",
                    text: data.Message,
                    icon: "warning",
                    button: "Aceptar",
                    allowOutsideClick: false
                });
            }
            if (data.Success === true) {
                swal({
                    title: "Atención",
                    text: data.Message,
                    icon: "success",
                    button: "Aceptar",
                    allowOutsideClick: false
                });
            }
            envio.webControls.btnEnviar.attr('disabled', false);
            envio.limpiar();

        }).fail(function (data, textStatus, xhr) {
            console.log(data, textStatus, xhr);
            envio.webControls.btnEnviar.attr('disabled', false);
        });
    };

    envio.iniciar();
});