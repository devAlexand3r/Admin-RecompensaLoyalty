$(function () {
    var registro = new Object();
    registro.urlAPI = {
        url_participante_agregarCliente: appBaseUrl + '/api/Customer/AddCustomer',
        url_participante_verificarCorreoTelCel: appBaseUrl + '/api/Customer/CheckEmailTelcel',
        url_participante_unificarCliente: appBaseUrl + '/api/Customer/UnifyCustomer',
    };
    registro.webControls = {
        hidParticipante_Id: $('#hidParticipante_Id'),
        txtNumTarjeta: $('#num_tarjeta'),
        txtNombre: $('#nombre'),
        txtApePaterno: $('#ape_paterno'),
        txtApeMaterno: $('#ape_materno'),
        txtFecNacimiento: $('#fecha_nacimiento'),
        selectEstado: $('#direccion_estado'),
        selectOcupacion: $('#selOcupacion'),
        txtCodPostal: $('#codigo_postal'),
        txtTelCelular: $('#tel_celular'),
        txtCorreo: $('#correo'),
        hidUserName: $('#hidUserName'),
        btnRegistrar: $('#btnRegistrar'),
        btnNuevo: $('#btnreset')
    };
    registro.iniciar = function () {
        $('.loading').removeClass('hidden');
        direccion.iniciar(); // Iniciar la configuación de direcciones
        var dynamicEndDate = '01/01/' + (new Date().getFullYear() - 18).toString();
        var dynamicStarDate = '01/01/' + (new Date().getFullYear() - 70).toString();
        var firtsOpen = true;
        $('.form-control.date').datepicker({
            format: "dd/mm/yyyy",
            language: 'es',
            autoclose: true,
            todayHighlight: true,
            startDate: '01/01/' + (new Date().getFullYear() - 70).toString(),
            defaultViewDate: { year: (new Date().getFullYear() - 70), month: 0, day: 1 },
        }).on("show", function (event) {
            if (event.date === undefined) {
            }
        }).on("hide", function (event) { });
        $('.form-control.date').datepicker('setEndDate', dynamicEndDate);
        // Validación Teléfono Celular No Permite Empiece con 0
        $("#tel_celular").mask('ZA', {
            translation: {
                Z: { pattern: /[1-9]/ },
                A: {
                    pattern: /[0-9]/,
                    recursive: true
                }
            }
        });
        this.agregarEventos();
        $('.card').removeClass('hidden');
        $('.loading').addClass('hidden');
    };
    registro.Datos = {
        num_tarjeta: false,
        nombre: false,
        ape_paterno: false,
        ape_materno: false,
        fec_nacimiento: false,
        estado: false,
        tel_celular: false,
        correo: false,
        ocupacion: false,
        codigo_postal: false,
    };
    registro.agregarEventos = function () {
        this.webControls.btnRegistrar.click(function () {
            registro.validarCorreo();
            registro.validarFechaNacimiento();
        });
        this.webControls.btnNuevo.click(function () {
            registro.webControls.selectEstado.val('').selectpicker("refresh");
            registro.webControls.selectOcupacion.val('').selectpicker("refresh");
            $(".dataFieldError").addClass('hidden');
            $(':input, :checked, :selected').removeClass('is-valid is-invalid');
            $('.bootstrap-select').removeClass('is-valid is-invalid');
        });
        this.webControls.txtNumTarjeta.change(function () {
            registro.Datos.num_tarjeta = registro.Requerido(this, 'card');
        });
        this.webControls.txtNombre.change(function () {
            registro.Datos.nombre = registro.Requerido(this, 'text');
        });
        this.webControls.txtApePaterno.change(function () {
            registro.Datos.ape_paterno = registro.Requerido(this, 'text');
        });
        this.webControls.txtFecNacimiento.change(function () {
            if (this.value.trim().length > 0) {
                registro.Datos.fec_nacimiento = registro.Requerido(this, 'date');
                if (registro.Datos.fec_nacimiento === true) {
                    var array = this.value.split("/");
                    var currentDate = new Date(array[2], array[1] - 1, array[0]);
                    var maxDate = new Date((new Date().getFullYear() - 18), 11, 31);
                    if (currentDate > maxDate) {
                        $('.form-control.date').val('').datepicker('update');
                    }
                }
            } else {
                registro.Datos.fec_nacimiento = true;
                $(this).removeClass('is-valid is-invalid');
                $("div[data-valmsg-for=" + this.name + "]").addClass('hidden');
            }
        });
        this.webControls.txtTelCelular.change(function () {
            var user = registro.buscaUserName('username', registro.webControls.hidUserName.val());
            if (user !== null && this.value.trim().length == 0) {
                $(this).removeClass('is-valid is-invalid');
                $("div[data-valmsg-for=" + this.name + "]").addClass('hidden');
                return;
            }
            else {
                if (this.value.trim().length > 0) {
                    registro.Datos.tel_celular = registro.Requerido(this, 'telcel');
                } else {
                    if (registro.webControls.txtCorreo.val().length < 1) {
                        registro.Datos.tel_celular = registro.Requerido(this, 'telcel');
                        return;
                    }
                    registro.Datos.tel_celular = true;
                    $(this).removeClass('is-valid is-invalid');
                    $("div[data-valmsg-for=" + this.name + "]").addClass('hidden');
                }
            }
        });
        this.webControls.txtCorreo.change(function () {
            var user = registro.buscaUserName('username', registro.webControls.hidUserName.val());
            if (user !== null && this.value.trim().length == 0) {
                $(this).removeClass('is-valid is-invalid');
                $("div[data-valmsg-for=" + this.name + "]").addClass('hidden');
                return;
            }
            else {
                if (this.value.trim().length > 0) {
                    registro.Datos.correo = registro.Requerido(this, 'email');
                } else {
                    if (registro.webControls.txtTelCelular.val().length < 1) {
                        registro.Datos.correo = registro.Requerido(this, 'email');
                        return;
                    }
                    registro.Datos.correo = true;
                    $(this).removeClass('is-valid is-invalid');
                    $("div[data-valmsg-for=" + this.name + "]").addClass('hidden');
                }
            }
        });
        this.webControls.selectOcupacion.change(function () {
            registro.Datos.ocupacion = registro.Requerido(this, 'select');
        });
        this.webControls.txtCodPostal.change(function () {
            var user = registro.buscaUserName('username', registro.webControls.hidUserName.val());
            if (user !== null && this.value.trim().length == 0) {
                $(this).removeClass('is-valid is-invalid');
                $("div[data-valmsg-for=" + this.name + "]").addClass('hidden');
                return;
            }
            else {
                if (this.value.trim().length > 0) {
                    registro.Datos.codigo_postal = registro.Requerido(this, 'CP');
                } else {
                    if (registro.webControls.selectEstado.val().length < 1) {
                        registro.Datos.codigo_postal = registro.Requerido(this, 'CP');
                        return;
                    }
                    registro.Datos.codigo_postal = true;
                    $(this).removeClass('is-valid is-invalid');
                    $("div[data-valmsg-for=" + this.name + "]").addClass('hidden');
                }
            }
        });
        this.webControls.selectEstado.change(function () {
            var user = registro.buscaUserName('username', registro.webControls.hidUserName.val());
            if (user !== null && this.value.trim().length == 0) {
                $(this).removeClass('is-valid is-invalid');
                $("div[data-valmsg-for=" + this.name + "]").addClass('hidden');
                return;
            }
            else {
                if (this.value.trim().length > 0) {
                    registro.Datos.estado = registro.Requerido(this, 'select');
                } else {
                    if (registro.webControls.txtCodPostal.val().length < 1) {
                        registro.Datos.estado = registro.Requerido(this, 'select');
                        return;
                    }
                    registro.Datos.estado = true;
                    $(this).removeClass('is-valid is-invalid');
                    $("div[data-valmsg-for=" + this.name + "]").addClass('hidden');
                }
            }
        });
        var modalConfirm = function (callback) {
            $("#modal-btn-si").on("click", function () {
                callback(true);
                $("#mi-modal").modal('hide');
            });
            $("#modal-btn-no").on("click", function () {
                callback(false);
                $("#mi-modal").modal('hide');
            });
        };
        $("#modal-btn-si-com").on("click", function () {
            window.location.href = appBaseUrl;
        });
        modalConfirm(function (confirm) {
            if (confirm) {
                //Acciones si el usuario confirma
                var _data = registro.obtenerdatosParticipante();
                registro.unificarParticipante(_data, registro.urlAPI.url_participante_unificarCliente);
            } else {
                //Acciones si el usuario no confirma
                $("#mi-modal-com").modal('show');
            }
        });
    };
    registro.Requerido = function (thisObj, type) {
        var valido = true;
        $("div[data-valmsg-for=" + thisObj.name + "]").addClass('hidden');
        if (type === 'text') {
            if (thisObj.value.length < 3) {
                $("div[data-valmsg-for=" + thisObj.name + "]").removeClass('hidden');
                $("[name=" + thisObj.name + "]").addClass('is-invalid');
                valido = false;
            }
        }
        if (type === 'date') {
            var vregexNaix = /^(0[1-9]|[1-2]\d|3[01])(\/)(0[1-9]|1[012])\2(\d{4})$/;
            if (!vregexNaix.test(thisObj.value) || thisObj.value.length > 10) {
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
        if (type === 'telcel') {
            if (thisObj.value.length !== 10) {
                $("div[data-valmsg-for=" + thisObj.name + "]").removeClass('hidden');
                $("[name=" + thisObj.name + "]").addClass('is-invalid');
                $('#divValidaTelCelSpecial').addClass('hidden');
                $('#lblMensajeTelCelSpecial').text('');
                valido = false;
            }
            else if (thisObj.value.length === 10) {
                if (thisObj.value.trim().substring(0, 3) === "123" || thisObj.value.trim().substring(0, 3) === "234") {
                    $("div[data-valmsg-for=" + thisObj.name + "]").addClass('hidden');
                    $('#tel_celular').addClass('is-valid is-invalid');
                    $('#divValidaTelCelSpecial').removeClass('hidden');
                    $('#lblMensajeTelCelSpecial').text('Teléfono Celular Invalido');
                    valido = false;
                }
            }
        }
        if (type === 'card') {
            if (thisObj.value.length !== 16) {
                $("div[data-valmsg-for=" + thisObj.name + "]").removeClass('hidden');
                $("[name=" + thisObj.name + "]").addClass('is-invalid');
                valido = false;
            }
        }
        if (type === 'select') {
            if (thisObj.value.length < 1) {
                $("div[data-valmsg-for=" + thisObj.name + "]").removeClass('hidden');
                valido = false;
            }
        }
        if (type === 'CP') {
            if (thisObj.value.length !== 5) {
                if (thisObj.value.length < 5 && thisObj.value.length !== 0) {
                    $("div[data-valmsg-for=" + thisObj.name + "]").removeClass('hidden');
                    $('#lblErrorCP').text('Código Postal Invalido');
                    valido = false;
                }
                else {
                    $("div[data-valmsg-for=" + thisObj.name + "]").removeClass('hidden');
                    $('#lblErrorCP').text('Código postal es requerido');
                    valido = false;
                }
            }            
        }
        if (valido === true) {
            $("[name=" + thisObj.name + "]").removeClass('is-invalid');
            $("[name=" + thisObj.name + "]").addClass('is-valid');
        }
        return valido;
    };
    registro.obtenerdatosParticipante = function () {
        if (this.webControls.hidParticipante_Id.val() === "")
        {         
            this.webControls.hidParticipante_Id.val(0);
        }        
        var datos = {            
            Id: parseInt(this.webControls.hidParticipante_Id.val()),
            Num_tarjeta: this.webControls.txtNumTarjeta.val().trim(),
            Nombre: this.webControls.txtNombre.val().trim(),
            Ape_paterno: this.webControls.txtApePaterno.val().trim(),
            Ape_materno: this.webControls.txtApeMaterno.val().trim(),
            Fecha_nacimiento: this.webControls.txtFecNacimiento.val().trim(),
            Estado: this.webControls.selectEstado.val(),
            Codigo_postal: this.webControls.txtCodPostal.val().trim(),
            Tel_celular: this.webControls.txtTelCelular.val().trim(),
            Correo: this.webControls.txtCorreo.val().trim(),
            Ocupacion_id: this.webControls.selectOcupacion.val()
        }
        var from = datos.Fecha_nacimiento.split("/");
        datos.Fecha_nacimiento = new Date(from[2], from[1] - 1, from[0]);
        return datos;
    };
    registro.validarCorreo = function () {
        $(".dataFieldError").addClass('hidden');
        var _data = registro.obtenerdatosParticipante();
        registro.webControls.txtNumTarjeta.trigger('change');
        registro.webControls.txtNombre.trigger('change');
        registro.webControls.txtApePaterno.trigger('change');
        registro.webControls.selectOcupacion.trigger('change');
        var user = registro.buscaUserName('username', registro.webControls.hidUserName.val());
        if (user !== null) {
            registro.Datos.correo = false;
            registro.Datos.tel_celular = false;
            registro.Datos.codigo_postal = false;
            registro.Datos.estado = false;
            var correo = $('#correo').val();
            var tel_celular = $('#tel_celular').val();
            var codigo_postal = $('#codigo_postal').val();
            var estado = $('#direccion_estado').val();
            if (correo.length > 0) {
                registro.webControls.txtCorreo.trigger('change');
            }
            else {
                registro.Datos.correo = true;
                $("#divValidaCorreo").addClass('hidden');
                $("#correo").removeClass('is-valid is-invalid');
            }
            if (tel_celular.length > 0) {
                registro.webControls.txtTelCelular.trigger('change');
            }
            else {
                registro.Datos.tel_celular = true;
                $("#divValidaTelCel").addClass('hidden');
                $("#tel_celular").removeClass('is-valid is-invalid');
            }
            // Para Código postal y Estado
            if (codigo_postal.length > 0) {
                registro.webControls.txtCodPostal.trigger('change');
            }
            else {
                registro.Datos.codigo_postal = true;
                $("#divValidaCP").addClass('hidden');
                $("#CodigoPostal").removeClass('is-valid is-invalid');
            }
            if (estado.length > 0) {
                registro.webControls.selectEstado.trigger('change');
            }
            else {
                registro.Datos.estado = true;
                $("#divValidaEstado").addClass('hidden');
                $("#direccion_estado").removeClass('is-valid is-invalid');
            }
        } else {
            registro.webControls.txtCorreo.trigger('change');
            registro.webControls.txtTelCelular.trigger('change');
            registro.webControls.selectEstado.trigger('change');
            registro.webControls.txtCodPostal.trigger('change');            
        }
        registro.webControls.txtCodPostal.trigger('change');
        registro.webControls.txtFecNacimiento.trigger('change');
        var _valido = registro.Datos;
        if (_valido.num_tarjeta === false ||
            _valido.nombre === false ||
            _valido.ape_paterno === false ||
            _valido.fec_nacimiento === false ||
            _valido.estado === false ||            
            _valido.tel_celular === false ||
            _valido.correo === false ||
            _valido.ocupacion === false ||
            _valido.codigo_postal === false
        ) {
            return;
        }
        _data.Correo = _data.Correo === "" ? "" : _data.Correo;
        _data.Tel_celular = _data.Tel_celular === "" ? "0" : _data.Tel_celular;
        var resultado_validacion = null;
        var mensaje = "";
        var existeCorreo = false;
        var existeTelCel = false;
        $('#tbody').empty();
        // Realizar una solicitud para verificar la existencia del correo electronico y validarlo
        $.ajax({
            url: this.urlAPI.url_participante_verificarCorreoTelCel,
            type: 'GET',
            data: { email: _data.Correo, clave: _data.Num_tarjeta, telcel: _data.Tel_celular, participante_id: _data.Id },
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            async: false
        }).done(function (data, status, xhr) {
            resultado_validacion = data;
            console.log('id ', data.id);
            console.log('correo ', data.correo_electronico);
            console.log('celular ', data.telefono_celular);
            if (data.id !== 0) {
                if (data.correo_electronico !== null) {
                    mensaje += '<tr><th style="text-align: right; border: none;">Nombre de Socia:</th><th style="border: none;">' + data.nombre + ' ' + data.apellido_paterno + ' ' + data.apellido_materno + '</th></tr><tr><th style="text-align: right; border: none;">Correo electrónico:</th><th style="border: none;">' + data.correo_electronico + '</th></tr>';
                    $('#tbody').append(mensaje);
                    $('#lblCuidado').text('¡Cuidado! Esta dirección de correo ya existe.');
                    existeCorreo = true;
                    _data.Id = data.id;
                    $('#hidParticipante_Id').val(_data.Id);
                }
                else {
                    mensaje += '<tr><th style="text-align: right; border: none;">Nombre de Socia:</th><th style="border: none;">' + data.nombre + ' ' + data.apellido_paterno + ' ' + data.apellido_materno + '</th></tr><tr><th style="text-align: right; border: none;">Teléfono Celular:</th><th style="border: none;">' + data.telefono_celular + '</th></tr>';
                    $('#lblCuidado').text('¡Cuidado! Este teléfono celular ya existe.');
                    $('#tbody').append(mensaje);
                    existeTelCel = true;
                    _data.Id = data.id;
                    $('#hidParticipante_Id').val(_data.Id);
                }
            }
        }).fail(function (data, textStatus, xhr) {
            console.log(data, textStatus, xhr);
        });
        // verificar si hubo errores en la validacion de correo electrónico
        if (resultado_validacion.result !== "ok" && resultado_validacion.result !== "success" && resultado_validacion.result !== null) {
            swal({
                title: 'Error',
                text: resultado_validacion.result,
                icon: "error",
                button: "Aceptar",
                closeOnClickOutside: false
            });
            return;
        }
        if (existeCorreo === false && existeTelCel === false) {
            if (resultado_validacion.status == 0) {
                registro.agregarParticipante(_data, this.urlAPI.url_participante_agregarCliente);
            }
            return;
        } else {
            if (resultado_validacion.status == 0) {
                $("#mi-modal").modal('show');
            }
        }
    };
    registro.validarFechaNacimiento = function () {
        if (registro.webControls.txtFecNacimiento.val().trim() != "") {
            var maxDate = new Date((new Date().getFullYear() - 18), 0, 01);
            var minDate = new Date((new Date().getFullYear() - 70), 0, 01);
            var fecha_nac = registro.webControls.txtFecNacimiento.val().trim();
            console.log('fecha nacimiento ' + fecha_nac);
            var anio = fecha_nac.substring(6);
            var mes = fecha_nac.substring(3, 5);
            var dia = fecha_nac.substring(0, 2);
            var dfecha_nac = new Date(anio + "/" + mes + "/" + dia);
            console.log('maxDate ' + maxDate);
            console.log('minDate ' + minDate);
            console.log('anio ' + anio);
            console.log('mes ' + mes);
            console.log('dia ' + dia);
            console.log('fecha nacimiento ' + dfecha_nac);
            if (!(dfecha_nac >= minDate && dfecha_nac <= maxDate)) {
                swal({
                    title: 'Error',
                    text: "La fecha de nacimiento es invalida",
                    icon: "error",
                    button: "Aceptar",
                    closeOnClickOutside: false
                });
                return;
            }
        }
    };
    registro.agregarParticipante = function (datos, urlDestino) {
        $.ajax({
            url: urlDestino,
            type: 'POST',
            async: false,
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify(datos),
            beforeSend: function () {
                registro.webControls.btnRegistrar.attr('disabled', true);
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
                $("#btnreset").click();
                registro.webControls.selectEstado.val('').selectpicker("refresh");
                registro.webControls.selectOcupacion.val('').selectpicker("refresh");
            }
            registro.webControls.btnRegistrar.attr('disabled', false);
        }).fail(function (data, textStatus, xhr) {
            console.log(data, textStatus, xhr);
            registro.webControls.btnRegistrar.attr('disabled', false);
        });
    };
    registro.unificarParticipante = function (datos, urlDestino) {
        $.ajax({
            url: urlDestino,
            type: 'POST',
            async: false,
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify(datos),
            beforeSend: function () {
                registro.webControls.btnRegistrar.attr('disabled', true);
            },
        }).done(function (data, textStatus, xhr) {
            var resultado = JSON.parse(data);
            if (resultado.Success === true) {
                if (resultado.jsonObject.length > 0) {
                    var result = JSON.parse(resultado.jsonObject);
                    if (result[0].errorId === 0) {
                        swal({
                            title: "Unificación",
                            text: result[0].mensaje,
                            icon: "success",
                            button: "Aceptar",
                            allowClickOutside: false
                        });
                        $("#btnreset").click();
                        registro.webControls.selectEstado.val('').selectpicker("refresh");
                        registro.webControls.selectOcupacion.val('').selectpicker("refresh");
                    } else {
                        swal({
                            title: "Alerta",
                            text: result[0].mensaje,
                            icon: "warning",
                            button: "Aceptar",
                            allowOutsideClick: false
                        });
                    }
                }
            }
            if (resultado.Success === false) {
                if (resultado.InnerException !== null) {
                    swal({
                        title: resultado.Message,
                        text: resultado.InnerException,
                        icon: "error",
                        button: "Aceptar",
                        allowOutsideClick: false
                    });
                } else {
                    swal({
                        title: "Alerta",
                        text: resultado.Message,
                        icon: "warning",
                        button: "Aceptar",
                        allowOutsideClick: false
                    });
                }
            }
            registro.webControls.btnRegistrar.attr('disabled', false);
        }).fail(function (data, textStatus, xhr) {
            console.log(data, textStatus, xhr);
            registro.webControls.btnRegistrar.attr('disabled', false);
        });
    };
    registro.buscaUserName = function (key, value) {
        var objArray = [
                        { id: 1, username: 'vianey_hernandez' },
                        { id: 2, username: 'tania_guadalquivir' },
                        { id: 3, username: 'rocio_lucio' },
                        { id: 4, username: 'desarrollo' }
        ];
        for (var i = 0; i < objArray.length; i++) {
            if (objArray[i][key] === value) {
                return objArray[i];
            }
        }
        return null;
    }    
    registro.iniciar();
});
function isNumber(evt) {
    evt = (evt) ? evt : window.event;
    var charCode = (evt.which) ? evt.which : evt.keyCode;
    if (charCode > 31 && (charCode < 48 || charCode > 57)) {
        return false;
    }
    return true;
}