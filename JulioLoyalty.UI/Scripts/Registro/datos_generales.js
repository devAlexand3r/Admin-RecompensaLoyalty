$(function (e) {
    var actualizar = new Object();
    actualizar.urlAPI = {
        url_participante_buscar: appBaseUrl + '/api/Customer/FindByUser',
        url_participante_actualizar: appBaseUrl + '/api/Customer/UpdateCustomer',
        url_participante_verificarCorreoTelCel: appBaseUrl + '/api/Customer/CheckEmailTelcel',
        url_participante_unificarCliente: appBaseUrl + '/api/Customer/Update/UnifyCustomer'
    };
    actualizar.webControls = {
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
        hiddenId: $('#participanteId'),
        hidUserName: $('#hidUserName'),
        btnActualizar: $('#btnActualizar'),
        btnCancelar: $('#btnCancelar'),
        btnHabilitar: $('#btnHabilitar')
    };
    actualizar.iniciar = function () {
        direccion.iniciar(); // Iniciar la configuración de direcciones
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
        $('.form-control.date').datepicker('setEndDate', '01/01/' + (new Date().getFullYear() - 18).toString());
        $(".numeric").on("keypress keyup blur", function (event) {
            $(this).val($(this).val().replace(/[^\d].+/, ""));
            if (event.which < 48 || event.which > 57) {
                event.preventDefault();
            }
        });
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
        this.obtenerParticipante();
    };
    actualizar.Datos = {
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
    actualizar.agregarEventos = function () {
        this.webControls.btnHabilitar.click(function () {
            actualizar.webControls.btnHabilitar.addClass('hidden');
            actualizar.webControls.btnCancelar.removeClass('hidden');
            actualizar.webControls.btnActualizar.removeClass('hidden');
            $('#form_upadte :input, :selected').prop('disabled', false);
            actualizar.webControls.txtNumTarjeta.prop('disabled', true);
        });
        this.webControls.btnCancelar.click(function () {
            actualizar.webControls.btnHabilitar.removeClass('hidden');
            actualizar.webControls.btnCancelar.addClass('hidden');
            actualizar.webControls.btnActualizar.addClass('hidden');
            $('#form_upadte :input, :selected').prop('disabled', true);
            $(".dataFieldError").addClass('hidden');
            $('#form_upadte :input, :selected').removeClass('is-valid is-invalid');
            $('.bootstrap-select').removeClass('is-valid is-invalid');
        });
        this.webControls.btnActualizar.click(function () {
            actualizar.validarCorreo();
            actualizar.validarFechaNacimiento();
        });
        this.webControls.txtNumTarjeta.change(function () {
            actualizar.Datos.num_tarjeta = actualizar.Requerido(this, 'text');
        });
        this.webControls.txtNombre.change(function () {
            actualizar.Datos.nombre = actualizar.Requerido(this, 'text');
        });
        this.webControls.txtApePaterno.change(function () {
            actualizar.Datos.ape_paterno = actualizar.Requerido(this, 'text');
        });
        this.webControls.txtFecNacimiento.change(function () {
            if (this.value.trim().length > 0) {
                actualizar.Datos.fec_nacimiento = actualizar.Requerido(this, 'date');
                if (actualizar.Datos.fec_nacimiento === true) {
                    var array = this.value.split("/");
                    var currentDate = new Date(array[2], array[1] - 1, array[0]);
                    var maxDate = new Date((new Date().getFullYear() - 18), 11, 31);
                    if (currentDate > maxDate) {
                        $('.form-control.date').val('').datepicker('update');
                    }
                }
            } else {
                actualizar.Datos.fec_nacimiento = true;
                $(this).removeClass('is-valid is-invalid');
                $("div[data-valmsg-for=" + this.name + "]").addClass('hidden');
            }
        });
        this.webControls.txtTelCelular.change(function () {
            var user = actualizar.buscaUserName('username', actualizar.webControls.hidUserName.val());
            if (user !== null && this.value.trim().length == 0) {
                $(this).removeClass('is-valid is-invalid');
                $("div[data-valmsg-for=" + this.name + "]").addClass('hidden');
                return;
            }
            else {
                if (this.value.trim().length > 0) {
                    actualizar.Datos.tel_celular = actualizar.Requerido(this, 'telcel');
                } else {
                    if (actualizar.webControls.txtCorreo.val().length < 1) {
                        actualizar.Datos.tel_celular = actualizar.Requerido(this, 'telcel');
                        return;
                    }
                    actualizar.Datos.tel_celular = true;
                    $(this).removeClass('is-valid is-invalid');
                    $("div[data-valmsg-for=" + this.name + "]").addClass('hidden');
                }
            }
        });
        this.webControls.txtCorreo.change(function () {
            var user = actualizar.buscaUserName('username', actualizar.webControls.hidUserName.val());
            if (user !== null && this.value.trim().length == 0) {
                $(this).removeClass('is-valid is-invalid');
                $("div[data-valmsg-for=" + this.name + "]").addClass('hidden');
                return;
            }
            else {
                if (this.value.trim().length > 0) {
                    actualizar.Datos.correo = actualizar.Requerido(this, 'email');
                } else {
                    if (actualizar.webControls.txtTelCelular.val().length < 1) {
                        actualizar.Datos.correo = actualizar.Requerido(this, 'email');
                        return;
                    }
                    actualizar.Datos.correo = true;
                    $(this).removeClass('is-valid is-invalid');
                    $("div[data-valmsg-for=" + this.name + "]").addClass('hidden');
                }
            }
        });
        this.webControls.selectOcupacion.change(function () {
            actualizar.Datos.ocupacion = actualizar.Requerido(this, 'select');
        });
        this.webControls.txtCodPostal.change(function () {
            var user = actualizar.buscaUserName('username', actualizar.webControls.hidUserName.val());
            if (user !== null && this.value.trim().length == 0) {
                $(this).removeClass('is-valid is-invalid');
                $("div[data-valmsg-for=" + this.name + "]").addClass('hidden');
                return;
            }
            else {
                if (this.value.trim().length > 0) {
                    actualizar.Datos.codigo_postal = actualizar.Requerido(this, 'CP');
                } else {
                    if (actualizar.webControls.selectEstado.val().length < 1) {
                        actualizar.Datos.codigo_postal = actualizar.Requerido(this, 'CP');
                        return;
                    }
                    actualizar.Datos.codigo_postal = true;
                    $(this).removeClass('is-valid is-invalid');
                    $("div[data-valmsg-for=" + this.name + "]").addClass('hidden');
                }
            }
        });
        this.webControls.selectEstado.change(function () {
            var user = actualizar.buscaUserName('username', actualizar.webControls.hidUserName.val());
            if (user !== null && this.value.trim().length == 0) {
                $(this).removeClass('is-valid is-invalid');
                $("div[data-valmsg-for=" + this.name + "]").addClass('hidden');
                return;
            }
            else {
                if (this.value.trim().length > 0) {
                    actualizar.Datos.estado = actualizar.Requerido(this, 'select');
                } else {
                    if (actualizar.webControls.txtCodPostal.val().length < 1) {
                        actualizar.Datos.estado = actualizar.Requerido(this, 'select');
                        return;
                    }
                    actualizar.Datos.estado = true;
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
            //Redicciona a la busqueda de socia
            window.location.href = appBaseUrl;
        });
        modalConfirm(function (confirm) {
            if (confirm) {
                //Si, realizar unificacion de membresia
                var _data = actualizar.obtenerdatosParticipante();
                actualizar.unificarParticipante(_data);
            } else {
                //Mostral popup
                $("#mi-modal-com").modal('show');
            }
        });
    };
    actualizar.Requerido = function (thisObj, type) {
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
    actualizar.obtenerParticipante = function () {
        var lUser = localStorage.getItem('objUser');
        if (lUser === null) {
            swal({
                title: 'Socia no encontrada',
                text: 'Le recomendamos que seleccione el registro correspondiente',
                icon: "info",
                button: "Aceptar",
                closeOnClickOutside: false
            });
            actualizar.webControls.btnHabilitar.attr('disabled', true);
            return;
        }
        var user = JSON.parse(lUser);
        $.ajax({
            url: actualizar.urlAPI.url_participante_buscar,
            type: 'GET',
            data: { id: user.id },
            async: true,
            beforeSend: function () {
                $('.loading').removeClass('hidden');
            },
            success: function (data, status, xhr) {
                actualizar.plasmarDatos(data[0]);
                $('.card').removeClass('hidden');
                $('.loading').addClass('hidden');
                $("#form_upadte :input, :selected").prop("disabled", true);
                $('.bootstrap-select').removeClass('is-valid is-invalid');
            },
            error: function (data, status, xhr) {
                console.log(data);
            }
        });
    };
    actualizar.obtenerdatosParticipante = function () {
        var datos = {
            Id: this.webControls.hiddenId.val(),
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
    actualizar.validarCorreo = function () {
        $(".dataFieldError").addClass('hidden');
        var _data = actualizar.obtenerdatosParticipante();
        actualizar.webControls.txtNombre.trigger('change');
        actualizar.webControls.txtApePaterno.trigger('change');
        actualizar.webControls.selectOcupacion.trigger('change');
        var user = actualizar.buscaUserName('username', actualizar.webControls.hidUserName.val());
        if (user !== null) {
            actualizar.Datos.correo = false;
            actualizar.Datos.tel_celular = false;
            actualizar.Datos.codigo_postal = false;
            actualizar.Datos.estado = false;
            var correo = $('#correo').val();
            var tel_celular = $('#tel_celular').val();
            var codigo_postal = $('#codigo_postal').val();
            var estado = $('#direccion_estado').val();
            if (correo.length > 0) {
                actualizar.webControls.txtCorreo.trigger('change');
            }
            else {
                actualizar.Datos.correo = true;
                $("#divValidaCorreo").addClass('hidden');
                $("#correo").removeClass('is-valid is-invalid');
            }
            if (tel_celular.length > 0) {
                actualizar.webControls.txtTelCelular.trigger('change');
            }
            else {
                actualizar.Datos.tel_celular = true;
                $("#divValidaTelCel").addClass('hidden');
                $("#tel_celular").removeClass('is-valid is-invalid');
            }
            // Para Código postal y Estado
            if (codigo_postal.length > 0) {
                actualizar.webControls.txtCodPostal.trigger('change');
            }
            else {
                actualizar.Datos.codigo_postal = true;
                $("#divValidaCP").addClass('hidden');
                $("#codigo_postal").removeClass('is-valid is-invalid');
            }
            if (estado.length > 0) {
                actualizar.webControls.selectEstado.trigger('change');
            }
            else {
                actualizar.Datos.estado = true;
                $("#divValidaEstado").addClass('hidden');
                $("#direccion_estado").removeClass('is-valid is-invalid');
            }
        } else {
            actualizar.webControls.txtCorreo.trigger('change');
            actualizar.webControls.txtTelCelular.trigger('change');
            actualizar.webControls.txtCodPostal.trigger('change');
            actualizar.webControls.selectEstado.trigger('change');
        }
        actualizar.webControls.txtCodPostal.trigger('change');
        actualizar.webControls.txtFecNacimiento.trigger('change');
        var _valido = actualizar.Datos;
        if (_valido.nombre === false ||
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
        var participante_id = 0;
        $('#tbody').empty();
        // Realizar una solicitud para verificar la existencia del correo electronico
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
                    participante_id = data.id.toString();
                }
                else {
                    mensaje += '<tr><th style="text-align: right; border: none;">Nombre de Socia:</th><th style="border: none;">' + data.nombre + ' ' + data.apellido_paterno + ' ' + data.apellido_materno + '</th></tr><tr><th style="text-align: right; border: none;">Teléfono Celular:</th><th style="border: none;">' + data.telefono_celular + '</th></tr>';
                    $('#lblCuidado').text('¡Cuidado! Este teléfono celular ya existe.');
                    $('#tbody').append(mensaje);
                    existeTelCel = true;
                    participante_id = data.id.toString();
                }
            }
        }).fail(function (data, textStatus, xhr) {
            console.log(data, textStatus, xhr);
        });
        // verificar si hubo errores en la validacion de correo electrónico y teléfono celular
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
                actualizar.actualizarParticipante(_data);
            }
        } else {
            if (_data.Id === participante_id) {
                if (resultado_validacion.status == 0) {
                    actualizar.actualizarParticipante(_data);
                }
            } else {
                $("#mi-modal").modal('show');
            }
        }
    };
    actualizar.validarFechaNacimiento = function () {
        if (actualizar.webControls.txtFecNacimiento.val().trim() != "") {
            var maxDate = new Date((new Date().getFullYear() - 18), 0, 01);
            var minDate = new Date((new Date().getFullYear() - 70), 0, 01);
            var fecha_nac = actualizar.webControls.txtFecNacimiento.val().trim();
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
    actualizar.actualizarParticipante = function (_data) {
        $.ajax({
            url: this.urlAPI.url_participante_actualizar,
            type: 'POST',
            async: false,
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify(_data),
            beforeSend: function () {
                actualizar.webControls.btnActualizar.attr('disabled', true);
            }
        }).done(function (data, textStatus, xhr) {
            if (data.Success === true) {
                swal({
                    title: "Actualización",
                    text: data.Message,
                    icon: "success",
                    button: "Aceptar",
                    closeOnClickOutside: false
                });
                const nombre = _data.Nombre + " " + _data.Ape_paterno + " " + _data.Ape_materno;
                localStorage.setItem('nombre', nombre);
                $('#spanCliente').text(nombre);
            }
            if (data.Success === false) {
                if (data.InnerException !== null) {
                    swal({
                        title: "Error",
                        text: data.InnerException,
                        icon: "error",
                        button: "Aceptar",
                        closeOnClickOutside: false
                    });
                } else {
                    swal({
                        title: "Atención",
                        text: data.Message,
                        icon: "warning",
                        button: "Aceptar",
                        closeOnClickOutside: false
                    });
                }
            }
            actualizar.webControls.btnActualizar.attr('disabled', false);
        }).fail(function (data, textStatus, xhr) {
            console.log(data, textStatus, xhr);
            actualizar.webControls.btnActualizar.attr('disabled', false);
        });
    };
    actualizar.unificarParticipante = function (datos) {
        $.ajax({
            url: this.urlAPI.url_participante_unificarCliente,
            type: 'POST',
            async: false,
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify(datos),
            beforeSend: function () {
                actualizar.webControls.btnActualizar.attr('disabled', true);
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
            actualizar.webControls.btnActualizar.attr('disabled', false);
        }).fail(function (data, textStatus, xhr) {
            console.log(data, textStatus, xhr);
            actualizar.webControls.btnActualizar.attr('disabled', false);
        });
    };
    actualizar.plasmarDatos = function (cliente) {
        this.webControls.hiddenId.val(cliente.id);
        this.webControls.txtNumTarjeta.val(cliente.clave);
        this.webControls.txtNombre.val(cliente.nombre);
        this.webControls.txtApePaterno.val(cliente.apellido_paterno);
        this.webControls.txtApeMaterno.val(cliente.apellido_materno);
        if (cliente.fecha_nacimiento !== null) {
            this.webControls.txtFecNacimiento.val(this.formatoFecha(cliente.fecha_nacimiento)).datepicker('update');
        }
        if (cliente.estado !== null && cliente.estado !== "") {
            this.webControls.selectEstado.val(cliente.estado).trigger('change').selectpicker("refresh");
        }
        this.webControls.txtCodPostal.val(cliente.codigo_postal);
        $.each(cliente.telefonos, function (data, row) {
            // Telefono celular
            if (row.tipo_telefono_id === 3 && row.telefono.length > 5)
                actualizar.webControls.txtTelCelular.val(row.telefono);
        });
        this.webControls.txtCorreo.val(cliente.correo_electronico);
        if (cliente.ocupacion_id !== null) {
            this.webControls.selectOcupacion.selectpicker("refresh");
            this.webControls.selectOcupacion.val(cliente.ocupacion_id).selectpicker("refresh");;
        }
    };
    actualizar.formatoFecha = function (fecha) {
        var date = new Date(fecha);
        var formatted = ("0" + date.getDate()).slice(-2) + "/" + ("0" + (date.getMonth() + 1)).slice(-2) + "/" + date.getFullYear();
        return formatted;
    };
    actualizar.buscaUserName = function (key, value) {
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
    actualizar.iniciar();
});