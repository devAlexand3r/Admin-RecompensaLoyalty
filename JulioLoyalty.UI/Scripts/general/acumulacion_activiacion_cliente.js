
$(function (e) {


    var control = new Object();
    control.urlAPI = {
        url_acumulacion_agregar: appBaseUrl + '/api/Customer/Agregar/Acumulacion',
        url_activacion_agregar: appBaseUrl + '/api/Customer/Activar/Tarjeta',
        url_consulta_usuario_distribuidor: appBaseUrl + '/api/Customer/Usuario/Distribuidor',
    };
    control.webControls = {
        txtNombre: $('#fullname'),
        txtNoTarjeta: $('#NoTarjeta'),
        txtNoTicket: $('#NoTicket'),
        selNoTienda: $('#selNoTienda'),

        hidParticipanteId: $('#participante_id'),
        btnAgregar: $('#btnAgregar')
    };
    control.acumulacion = {
        valido: {
            Notarjeta: true,
            Notienda: false,
            Noticket: false
        },
        obtenerParticipanteTemporal: function () {
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
            control.webControls.hidParticipanteId.val(user.id);
            control.webControls.txtNombre.val(nombrecompleto);
            control.webControls.txtNoTarjeta.val(user.clave);
        },
        registrarEventos: function () {
            control.webControls.btnAgregar.click(function () {
                control.acumulacion.agregarAcumulacion();
            });
            control.webControls.selNoTienda.change(function () {
                control.acumulacion.valido.Notienda = control.Requerido(this, 'select');
            });
            control.webControls.txtNoTicket.change(function () {
                control.acumulacion.valido.Noticket = control.Requerido(this, 'text');
            });
        },
        agregarAcumulacion: function () {
            var _data = {
                participante_id: control.webControls.hidParticipanteId.val(),
                NoTarjeta: control.webControls.txtNoTarjeta.val(),
                NoTienda: control.webControls.selNoTienda.val(),
                NoTicket: control.webControls.txtNoTicket.val()
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

            control.webControls.selNoTienda.trigger('change');
            control.webControls.txtNoTicket.trigger('change');
            var array = [];
            $.each(control.acumulacion.valido, function (index, isvalid) {
                if (isvalid === false)
                    array.push(1);
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
                url: control.urlAPI.url_acumulacion_agregar,
                type: 'POST',
                async: false,
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                data: JSON.stringify(_data),
                beforeSend: function () {
                    control.webControls.btnAgregar.attr('disabled', true);
                },
            }).done(function (data, textStatus, xhr) {
                var result = JSON.parse(data);
                if (result.length > 0) {
                    if (result[0].error_id === 0) {
                        swal({
                            title: 'Atención',
                            text: result[0].mensaje,
                            icon: "success",
                            button: "Aceptar",
                            closeOnClickOutside: false
                        });
                        control.webControls.selNoTienda.val('').selectpicker("refresh");
                        control.webControls.NoTicket.val('');
                    } else {
                        if (result[0].error_id === 45) {
                            var mensaje = result[0].mensaje;
                            $('#title').text(mensaje);
                            var dataResult = [];
                            var rcolumns = [];
                            if (result.length > 0) {
                                const types = Object.keys(result[0]);
                                var columnDefault = [];
                                $.each(types, function (index, name) {
                                    if (name === "importe" || name === "puntos" || name === "pago") {
                                        if (name === "importe") {
                                            const add = {
                                                sTitle: control.maysPrimera(name).replace('_', ' '),
                                                mData: name,
                                                mRender: function (data, type, row) {
                                                    return control.formatoCantidad(data, 2);
                                                }
                                            }
                                            rcolumns.push(add);
                                        }
                                        if (name === "puntos") {
                                            const add = {
                                                sTitle: control.maysPrimera(name).replace('_', ' '),
                                                mData: name,
                                                mRender: function (data, type, row) {
                                                    return data.toString().replace(/(\d)(?=(\d{3})+(?!\d))/g, "$1,");
                                                }
                                            }
                                            rcolumns.push(add);
                                        }
                                        if (name === "pago") {
                                            const add = {
                                                sTitle: control.maysPrimera(name).replace('_', ' '),
                                                mData: name,
                                                mRender: function (data, type, row) {
                                                    return control.formatoCantidad(data, 2);
                                                }
                                            }
                                            rcolumns.push(add);
                                        }
                                    } else {
                                        const add = { sTitle: control.maysPrimera(name).replace('_', ' '), mData: name }
                                        rcolumns.push(add);
                                    }
                                    // Agregar boton de control
                                    //if ((index + 1) === 1) {
                                    //    var controles = {
                                    //        mData: 'id',
                                    //        sWidth: "20px",
                                    //        mRender: function (data, type, row) {
                                    //            return '<div style="text-align:center;"> <button class="btn btn-info btn-sm" data-title="Ver detalles" data-toggle="modal" data-target="#details" ><i class="fas fa-expand"></i></button>';
                                    //        }
                                    //    };
                                    //    rcolumns.push(controles);
                                    //}
                                    if (index === 2) {
                                        columnDefault.push({ visible: false, targets: 0 });
                                        columnDefault.push({ visible: false, targets: 1 });
                                    }
                                });
                            }
                            dataResult.push({
                                aaData: result,
                                aoColumns: rcolumns,
                                aoColumnDefs: columnDefault,
                                oLanguage: {
                                    sUrl: '//cdn.datatables.net/plug-ins/1.10.15/i18n/Spanish.json'
                                },
                                rowId: 'id',
                                bDestroy: true,
                                bLengthChange: false,
                                bScrollCollapse: true,
                                paging: false,
                                searching: false,
                                ordering: false,
                                info: false
                            });
                            $('#table_movimientos').DataTable(dataResult[0]);
                            $("#details").modal('show');
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
                }
                control.webControls.btnAgregar.attr('disabled', false);

            }).fail(function (data, textStatus, xhr) {
                console.log(data, textStatus, xhr);
                control.webControls.btnAgregar.attr('disabled', false);
            });
        }
    }
    control.activacion = {
        valido: {
            Notarjeta: false,
            Notienda: false,
            Noticket: false
        },
        registrarEventos: function () {
            control.webControls.btnAgregar.click(function () {
                control.activacion.agregarActivacion();
            });
            control.webControls.txtNoTarjeta.change(function () {
                control.activacion.valido.Notarjeta = control.Requerido(this, 'text');
            });
            control.webControls.selNoTienda.change(function () {
                control.activacion.valido.Notienda = control.Requerido(this, 'select');
            });
            control.webControls.txtNoTicket.change(function () {
                control.activacion.valido.Noticket = control.Requerido(this, 'text');
            });
        },
        agregarActivacion: function () {
            var _data = {
                participante_id: control.webControls.hidParticipanteId.val(),
                NoTarjeta: control.webControls.txtNoTarjeta.val(),
                NoTienda: control.webControls.selNoTienda.val(),
                NoTicket: control.webControls.txtNoTicket.val()
            };
            control.webControls.txtNoTarjeta.trigger('change');
            control.webControls.selNoTienda.trigger('change');
            control.webControls.txtNoTicket.trigger('change');
            var array = [];
            $.each(control.activacion.valido, function (index, isvalid) {
                if (isvalid === false)
                    array.push(1);
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
                url: control.urlAPI.url_activacion_agregar,
                type: 'POST',
                async: false,
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                data: JSON.stringify(_data),
                beforeSend: function () {
                    control.webControls.btnAgregar.attr('disabled', true);
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
                        $(':input', '#form_activacion')
                            .not(':button, :submit, :reset, :hidden')
                            .val('')
                            .removeAttr('checked')
                            .removeAttr('selected');
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
                control.webControls.btnAgregar.attr('disabled', false);

            }).fail(function (data, textStatus, xhr) {
                console.log(data, textStatus, xhr);
                control.webControls.btnAgregar.attr('disabled', false);
            });
        }
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
    control.consultarUDistribuidor = function () {
        $.ajax({
            url: this.urlAPI.url_consulta_usuario_distribuidor,
            type: 'GET',
            data: {},
            async: false
        }).done(function (data, textStatus, xhr) {
            var array = [];
            $.each(JSON.parse(data), function (index, data) {
                var add = {
                    id: data.id,
                    descripcion: data.descripcion
                };
                array.push(add);
            });
            control.webControls.selNoTienda.empty();
            $.each(array, function (index, row) {
                var html = ' <option value=\"' + row.id + '">' + row.descripcion + '</option>'
                control.webControls.selNoTienda.append(html);
            });
            //$Utils.addSelectOptions(array, control.webControls.selNoTienda, 'id', 'descripcion');
        }).fail(function (data, textStatus, xhr) {
            console.log(data, textStatus, xhr);
        });
    };

    control.maysPrimera = function (texto) {
        return texto.charAt(0).toUpperCase() + texto.slice(1).toLowerCase()
    };
    control.formatoCantidad = function (number, fraction) {
        return '$' + number.toFixed(2).replace(/\d(?=(\d{3})+\.)/g, '$&,');
    };
    control.formatoFecha = function (fecha) {
        var date = new Date(fecha);
        var formatted = ("0" + date.getDate()).slice(-2) + "/" + ("0" + (date.getMonth() + 1)).slice(-2) + "/" + date.getFullYear();
        return formatted;
    };

    control.iniciarCotroles = function () {
        this.consultarUDistribuidor();
        $('.selectpicker').selectpicker({
            language: 'es',
            title: 'No hay selección'
        }).selectpicker('refresh');

        var pathname = window.location.pathname;
        var url = window.location.href;
        if (url === (appBaseUrl + 'Operation/Accumulation')) {
            control.acumulacion.obtenerParticipanteTemporal();
            control.acumulacion.registrarEventos();
            console.log('load script acumulación');
        } else {
            this.activacion.registrarEventos();
            console.log('load script activación');
        }
    };
    control.iniciarCotroles();

});


function isNumber(evt) {
    evt = (evt) ? evt : window.event;
    var charCode = (evt.which) ? evt.which : evt.keyCode;
    if (charCode > 31 && (charCode < 48 || charCode > 57)) {
        return false;
    }
    return true;
}

