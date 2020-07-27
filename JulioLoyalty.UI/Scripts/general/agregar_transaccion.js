

$(function (e) {
    var control = new Object();
    control.urlAPI = {
        url_obtener_catalogo: appBaseUrl + '/api/MCatalogo/FKCatalogos',
        url_obtener_participante_direccion: appBaseUrl + '/api/Customer/Participante/Direccion',
        url_agregar_transaccion: appBaseUrl + '/api/Customer/Agregar/Transaccion'
    };
    control.webControls = {
        selTipoTransaccion: $('#selTipoTransaccion'),
        setPais: $('#selPais'),
        txtNombre: $('#fullname'),
        hidParticipanteId: $('#participante_id'),
        txtPais: $('#pais'),
        txtDistribuidora: $('#distribuidora'),
        txtPuntos: $('#txtPuntos'),
        txtFechaTransaccion: $('#txtFechaTransaccion'),
        txtComentarios: $('#comentarios'),
        btnAgregar: $('#btnAgregar')
    };
    control.valido = {
        distribuidora: false,
        puntos: false,
        fecha: false,
        comentarios: false
    }
    control.obtenerPais = function () {
        $.ajax({
            url: this.urlAPI.url_obtener_catalogo,
            type: 'GET',
            data: { descripcion: 'pais' },
            async: false
        }).done(function (data, textStatus, xhr) {
            var array = [];
            $.each(JSON.parse(data), function (index, data) {
                var add = {
                    id: data.id,
                    descripcion: control.maysPrimera(data.descripcion)
                };
                array.push(add);
            });
            control.webControls.setPais.empty();
            $.each(array, function (index, row) {
                var html = ' <option value=\"' + row.id + '">' + row.descripcion + '</option>'
                control.webControls.setPais.append(html);
            });
            //$Utils.addSelectOptions(array, control.webControls.setPais, 'id', 'descripcion');
        }).fail(function (data, textStatus, xhr) {
            console.log(data, textStatus, xhr);
        });
    };
    control.obtenerTipoTransaccion = function () {
        $.ajax({
            url: this.urlAPI.url_obtener_catalogo,
            type: 'GET',
            data: { descripcion: 'tipo_transaccion' },
            async: false
        }).done(function (data, textStatus, xhr) {
            var array = [];
            $.each(JSON.parse(data), function (index, data) {
                var add = {
                    id: data.id,
                    descripcion: control.maysPrimera(data.descripcion)
                };
                array.push(add);
            });

            control.webControls.selTipoTransaccion.empty();
            $.each(array, function (index, row) {
                var html = ' <option value=\"' + row.id + '">' + row.descripcion + '</option>'
                control.webControls.selTipoTransaccion.append(html);
            });
            //$Utils.addSelectOptions(array, control.webControls.selTipoTransaccion, 'id', 'descripcion');
        }).fail(function (data, textStatus, xhr) {
            console.log(data, textStatus, xhr);
        });
    };
    control.obtenerParticipanteTemporal = function () {
        var lUser = localStorage.getItem('objUser');
        if (lUser === null) {
            //$swal.warning('Socia no encontrado', 'Le recomendamos que seleccione el registro correspondiente');
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
            url: this.urlAPI.url_obtener_participante_direccion,
            type: 'GET',
            data: { id: user.id },
            async: false
        }).done(function (data, textStatus, xhr) {
            var user = JSON.parse(data);
            if (user.length > 0) {
                const nombrecompleto = user[0].nombre + " " + user[0].apellido_paterno + " " + user[0].apellido_materno;
                control.webControls.hidParticipanteId.val(user[0].id);
                control.webControls.txtNombre.val(nombrecompleto);
                control.webControls.txtPais.val(user[0].clave_pais);
                control.webControls.txtDistribuidora.val(user[0].distribuidor);
            }

        }).fail(function (data, textStatus, xhr) {
            console.log(data, textStatus, xhr);
        });

        //const nombrecompleto = user.nombre + " " + user.apellido_paterno + " " + user.apellido_materno;
        //control.webControls.hidParticipanteId.val(user.id);
        //control.webControls.txtNombre.val(nombrecompleto);
    };
    control.maysPrimera = function (texto) {
        return texto.charAt(0).toUpperCase() + texto.slice(1).toLowerCase()
    };
    control.agregarTransaccion = function () {
        var _data = {
            Participante_id: this.webControls.hidParticipanteId.val(),
            Tipo_transaccion_id: this.webControls.selTipoTransaccion.val(),
            Puntos: this.webControls.txtPuntos.val(),
            Fecha_transaccion: this.webControls.txtFechaTransaccion.val(),
            Comentarios: this.webControls.txtComentarios.val()
        };
        
        var _ouser = localStorage.getItem('objUser');
        var _user = JSON.parse(_ouser);
        if (_user.acumula === 0) {
            swal({
                title: 'Atención',
                text: _user.acumula_mensaje,
                icon: "info",
                button: "Aceptar",
                closeOnClickOutside: false
            });
            return;
        }

        this.webControls.selTipoTransaccion.trigger('change');
        this.webControls.txtPuntos.trigger('change');
        this.webControls.txtFechaTransaccion.trigger('change');
        this.webControls.txtComentarios.trigger('change');

        //Recorremos el arreglo de validación, si existe un campo no es valido no seguimos con el proceso proximo
        var array = [];
        $.each(this.valido, function (index, isvalid) {
            if (isvalid === false) {
                array.push(1);
            }
        });
        if (array.length > 0) {
            swal({
                title: 'Atención',
                text: 'Por favor ingrese la información faltante',
                icon: "info",
                button: "Aceptar",
                closeOnClickOutside: false
            });
            return;
        }

        $.ajax({
            url: this.urlAPI.url_agregar_transaccion,
            type: 'POST',
            async: false,
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify(_data),
            beforeSend: function () {
                control.webControls.btnAgregar.attr('disabled', true);
            },
        }).done(function (data, textStatus, xhr) {
            if (data.Success === true) {
                swal({
                    title: "Atención",
                    text: data.Message,
                    icon: "success",
                    button: "Aceptar",
                    closeOnClickOutside: false
                });

                $(':input', '#addtransaccion-form')
                    .not(':button, :submit, :reset, :hidden')
                    .val('')
                    .removeAttr('checked')
                    .removeAttr('selected');
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
            control.webControls.btnAgregar.attr('disabled', false);
        }).fail(function (data, textStatus, xhr) {
            console.log(data, textStatus, xhr);
            control.webControls.btnAgregar.attr('disabled', false);
        });

    };
    control.registrarEventos = function () {
        this.webControls.selTipoTransaccion.change(function () {
            control.valido.distribuidora = control.Requerido(this, 'select');
        });
        this.webControls.txtPuntos.change(function () {
            control.valido.puntos = control.Requerido(this, 'text');
        });
        this.webControls.txtFechaTransaccion.change(function () {
            control.valido.fecha = control.Requerido(this, 'date');
        });
        this.webControls.txtComentarios.change(function () {
            control.valido.comentarios = control.Requerido(this, 'text');
        });
    };

    control.Requerido = function (thisObj, type) {
        var valido = true;
        $("div[data-valmsg-for=" + thisObj.name + "]").addClass('hidden');
        if (type === 'text') {
            if (thisObj.value.length < 3) {
                $("div[data-valmsg-for=" + thisObj.name + "]").removeClass('hidden');
                $("[name=" + thisObj.name + "]").addClass('is-invalid');
                valido = false;
            }
        }
        if (type === 'select') {
            if (thisObj.value.length < 1) {
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
        if (type === 'tel') {
            if (thisObj.value.length < 8) {
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
            if (thisObj.value.length < 5) {
                $("div[data-valmsg-for=" + thisObj.name + "]").removeClass('hidden');
                valido = false;
            }
            return true;
        }

        if (valido === true) {
            $("[name=" + thisObj.name + "]").removeClass('is-invalid');
            $("[name=" + thisObj.name + "]").addClass('is-valid');
        }
        return valido;

    };
    control.iniciarCotroles = function () {

        this.obtenerTipoTransaccion();
        this.obtenerParticipanteTemporal();

        $('.selectpicker').selectpicker({
            language: 'es',
            title: 'No hay selección'
        }).selectpicker('refresh');

        this.webControls.txtFechaTransaccion.datepicker({
            format: "dd/mm/yyyy",
            language: 'es',
            autoclose: true,
            todayHighlight: true
        });
        this.webControls.txtFechaTransaccion.datepicker('setEndDate', new Date());


        $(".numeric").on("keypress keyup blur", function (event) {
            $(this).val($(this).val().replace(/[^\d].+/, ""));
            if ((event.which < 48 || event.which > 57)) {
                event.preventDefault();
            }
        });

        this.webControls.btnAgregar.click(function () {
            control.agregarTransaccion();
        });
        this.registrarEventos();
    };
    control.iniciarCotroles();
});

