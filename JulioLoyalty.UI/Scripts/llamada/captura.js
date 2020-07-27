$(function (e) {
    var captura = new Object();
    captura.urlAPI = {
        url_participante_buscar: appBaseUrl + '/api/Customer/FindByUser',
        url_participante_origen: appBaseUrl + '/api/Llamada/CargaStatusSeguimientoAbierto',
        url_participante_telefono: appBaseUrl + '/api/Llamada/CargaTelefono',
        url_participante_usuario_distribuidor: appBaseUrl + '/api/Customer/Usuario/Distribuidor',
        url_participante_categoria_tipo_llamada: appBaseUrl + '/api/Llamada/CargaTipoLlamada',
        url_participante_captura: appBaseUrl + '/api/Llamada/Captura'
    };

    captura.webControls = {
        hiddenId: $('#participante_id'),
        hidDistribuidor_Id: $('#hidDistribuidor_Id'),
        txtParticipante: $('#participante'),
        txtPersona: $('#persona'),
        selStatusSeguimientoAbierto: $('#selStatusSeguimientoAbierto'),
        selNoTienda: $('#selNoTienda'),
        txtComentarios: $('#comentarios'),
        btnGuardar: $('#btnGuardar'),
        btnLimpiar: $('#btnLimpiar')
    };

    captura.iniciar = function () {
        this.agregarEventos();
        this.consultarDistribuidor();
        this.obtenerParticipante();
    };

    captura.Datos = {
        participante: false,
        persona: false,
        telefono: true,
        origen: false,
        Notienda: false,
        comentarios: false
    };

    captura.agregarEventos = function () {
        this.webControls.txtParticipante.change(function () {
            captura.Datos.participante = captura.Requerido(this, 'text');
        });
        this.webControls.txtPersona.change(function () {
            captura.Datos.persona = captura.Requerido(this, 'text');
        });
        this.webControls.selStatusSeguimientoAbierto.change(function () {
            captura.Datos.origen = captura.Requerido(this, 'select');
        });
        this.webControls.selNoTienda.change(function () {
            captura.Datos.Notienda = captura.Requerido(this, 'select');
        });
        this.webControls.txtComentarios.change(function () {
            captura.Datos.comentarios = captura.Requerido(this, 'text');
        });

        this.webControls.btnGuardar.click(function () {
            captura.webControls.txtParticipante.trigger('change');
            captura.webControls.txtPersona.trigger('change');
            captura.webControls.selStatusSeguimientoAbierto.trigger('change');
            captura.webControls.selNoTienda.trigger('change');
            captura.webControls.txtComentarios.trigger('change');
            var _valido = captura.Datos;
            if (_valido.participante === false ||
                _valido.persona === false ||
                _valido.origen === false ||
                _valido.Notienda === false ||
                _valido.comentarios === false
               ) {
                return;
            }
            // Proceso de Guardar Llamada
            var _data = captura.obtenerDatos();
            captura.Guardar(_data, captura.urlAPI.url_participante_captura);
            return;
        });

        this.webControls.btnLimpiar.click(function () {
            captura.limpiar();
        });
    }

    captura.Requerido = function (thisObj, type) {
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
                valido = false;
            }
        }
        if (valido === true) {
            $("[name=" + thisObj.name + "]").removeClass('is-invalid');
            $("[name=" + thisObj.name + "]").addClass('is-valid');
        }
        return valido;
    };

    captura.obtenerParticipante = function () {
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
            url: captura.urlAPI.url_participante_buscar,
            type: 'GET',
            data: { id: user.id },
            async: true,
            beforeSend: function () {
                $('.loading').removeClass('hidden');
            },
            success: function (data, status, xhr) {
                captura.plasmarDatos(data[0]);
                $('.card').removeClass('hidden');
                $('.loading').addClass('hidden');
                $('.bootstrap-select').removeClass('is-valid is-invalid');
            },
            error: function (data, status, xhr) {
                console.log(data);
            }
        });
        this.consultarStatusSeguimientoAbierto();
        // Selecciona por default el status seguimiento abierto LMS                        
        // Initialize the select picker.
        $('select[id=selStatusSeguimientoAbierto]').selectpicker();
        // Extract the value of the first option.
        var sVal = $('select[id=selStatusSeguimientoAbierto] option:first').val();
        // Set the "selected" value of the <select>.
        $('select[id=selStatusSeguimientoAbierto]').val(sVal);
        // Force a refresh.
        $('select[id=selStatusSeguimientoAbierto]').selectpicker('refresh');
        this.cargaTelefono(user.id);
        this.CargaTipoLlamada();
    };

    captura.plasmarDatos = function (cliente) {
        var nombre_completo;
        if (cliente.nombre !== "")
            nombre_completo = cliente.nombre.trim();
        if (cliente.apellido_paterno !== "")
            nombre_completo = (nombre_completo + " " + cliente.apellido_paterno).trim();
        if (cliente.apellido_materno !== "")
            nombre_completo = (nombre_completo + " " + cliente.apellido_materno).trim();
        this.webControls.hiddenId.val(cliente.id);
        this.webControls.txtParticipante.val(nombre_completo);
        if (nombre_completo === undefined) // Si no viene el nombre que habilite el textbox del nombre el participante, para que pueda ser editado
            this.webControls.txtParticipante.prop("disabled", false);
        else
            this.webControls.txtParticipante.prop("disabled", true);
        this.webControls.txtPersona.val("");
        this.webControls.txtComentarios.val("");
        // Selecciona por default la socia donde hizo su primera compra  
        this.webControls.hidDistribuidor_Id.val(cliente.distribuidor_id);
        $('#selNoTienda').val(cliente.distribuidor_id);
        $('#selNoTienda').selectpicker('refresh')
    };

    captura.obtenerDatos = function () {
        var telefono = "";
        if ($('#txtTelefono').val() === undefined)
            telefono = $('#hidTelefono').val().trim();
        else
            telefono = $('#txtTelefono').val().trim();
        var datos = {
            Participante_Id: this.webControls.hiddenId.val().trim(),
            Participante: this.webControls.txtParticipante.val().trim(),
            Persona: this.webControls.txtPersona.val().trim(),
            Telefono: telefono,
            Status_Seguimiento_Id: $('#selStatusSeguimientoAbierto').val(),
            Distribuidor_Id: $('#selNoTienda').val(),
            Comentarios: this.webControls.txtComentarios.val().trim()
        }
        return datos;
    };

    captura.limpiar = function () {
        if (!this.webControls.txtParticipante.attr("disabled")) // Si esta modo editar limpia los datos
            this.webControls.txtParticipante.val("");
        this.webControls.txtPersona.val("");
        $("#grpTelefono").remove();
        $("#lblTelefono").remove();
        $("#txtTelefono").remove();
        this.consultarStatusSeguimientoAbierto();
        // Selecciona por default el status seguimiento abierto LMS                        
        // Initialize the select picker.
        $('select[id=selStatusSeguimientoAbierto]').selectpicker();
        // Extract the value of the first option.
        var sVal = $('select[id=selStatusSeguimientoAbierto] option:first').val();
        // Set the "selected" value of the <select>.
        $('select[id=selStatusSeguimientoAbierto]').val(sVal);
        // Force a refresh.
        $('select[id=selStatusSeguimientoAbierto]').selectpicker('refresh');
        // Selecciona por default la socia donde hizo su primera compra                
        $('#selNoTienda').val(this.webControls.hidDistribuidor_Id.val());
        $('#selNoTienda').selectpicker('refresh')
        captura.cargaTelefono(this.webControls.hiddenId.val());
        this.webControls.txtComentarios.val("");
        $('#form_captura input[type=checkbox]').each(function () {
            if (this.checked) {
                this.checked = false;
            }
        });
    };

    captura.consultarStatusSeguimientoAbierto = function () {
        $.ajax({
            url: this.urlAPI.url_participante_origen,
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
            captura.webControls.selStatusSeguimientoAbierto.empty();
            $.each(array, function (index, row) {
                var html = ' <option value=\"' + row.id + '">' + row.descripcion + '</option>'
                captura.webControls.selStatusSeguimientoAbierto.append(html);
            });
        }).fail(function (data, textStatus, xhr) {
            console.log(data, textStatus, xhr);
        });
    };

    captura.cargaTelefono = function (participante_id) {
        $.ajax({
            url: captura.urlAPI.url_participante_telefono,
            type: 'GET',
            data: { participante_id: participante_id },
            success: function (data, status, xhr) {
                $container = $('#divTelefono');
                var cadenaTelefono = "";
                var result = JSON.parse(data);
                if (result.length > 0) { // Agrega los tipos de teléfono en radiobutton
                    cadenaTelefono = '<fieldset id="grpTelefono"><div class="row"><div class="col-lg-3"><label class="control-label">Tipo Teléfono</label></div><div class="col-lg-3"><label class="control-label">Teléfono</label></div></div>';
                    for (i = 0; i < result.length; i++) {
                        if (i === 0) {
                            cadenaTelefono = cadenaTelefono + '<div class="row"><div class="col-lg-3"><input type="radio" id="rbTel' + result[i].tipotelefono + '" name="grpTelefono" checked="checked" onchange="getTelefono(' + "'" + result[i].telefono.trim() + "'" + ');" />' + result[i].tipotelefono + '</div>';
                            $('#hidTelefono').val(result[i].telefono.trim());
                        }
                        else
                            cadenaTelefono = cadenaTelefono + '<div class="row"><div class="col-lg-3"><input type="radio" id="rbTel' + result[i].tipotelefono + '" name="grpTelefono" onchange="getTelefono(' + "'" + result[i].telefono.trim() + "'" + ');" />' + result[i].tipotelefono + '</div>';
                        cadenaTelefono = cadenaTelefono + '<div class="col-lg-3"><label class="control-label" id="lblTel' + result[i].tipotelefono + '" name="lblTel' + result[i].tipotelefono + '">' + result[i].telefono + '</label></div></div>';
                    }
                    cadenaTelefono = cadenaTelefono + '</fieldset>';
                    $(cadenaTelefono).appendTo($container);
                }
                else { // No tiene teléfono, se agrego el TextBox para agregarlo en la llamada
                    cadenaTelefono = '<label id="lblTelefono" class="control-label"> Teléfono</label><input type="text" id="txtTelefono" name="txtTelefono" class="form-control" />';
                    $(cadenaTelefono).appendTo($container);
                }
            },
            error: function (data, status, xhr) {
                console.log(data);
            }
        })
    };

    captura.consultarDistribuidor = function () {
        $.ajax({
            url: this.urlAPI.url_participante_usuario_distribuidor,
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
            captura.webControls.selNoTienda.empty();
            $.each(array, function (index, row) {
                var html = ' <option value=\"' + row.id + '">' + row.descripcion + '</option>'
                captura.webControls.selNoTienda.append(html);
            });
        }).fail(function (data, textStatus, xhr) {
            console.log(data, textStatus, xhr);
        });
    };

    captura.CargaTipoLlamada = function () {
        $.ajax({
            url: captura.urlAPI.url_participante_categoria_tipo_llamada,
            type: 'GET',
            success: function (data, status, xhr) {
                var result = JSON.parse(data);
                if (result.length > 0) {
                    var clave_categoria_tipo_llamada = result[0].Clave_Categoria_Tipo_Llamada;
                    var posicion = 0;
                    var j = 0;
                    var divCategoria = "";
                    $container = $('#containerTipoLLamada');
                    for (i = posicion; i <= result.length - 1; i++) {
                        if (posicion <= result.length - 1) {
                            $('<div class="header" data-toggle="collapse" data-target=' + "#CategoriaTipoLlamada" + j + '><h6 class="text-center">' + result[posicion].Descripcion_Categoria_Tipo_Llamada + '</h6></div>').appendTo($container);
                            if (j === 0) {
                                divCategoria = '<div id="CategoriaTipoLlamada' + j + '"class="collapse show">';
                            }
                            else {
                                divCategoria = '<div id="CategoriaTipoLlamada' + j + '"class="collapse">';
                            }
                            var cadenaCheck = "";
                            while (posicion <= result.length - 1 && clave_categoria_tipo_llamada === result[posicion].Clave_Categoria_Tipo_Llamada) {
                                cadenaCheck = cadenaCheck + '<input type="checkbox" id=chkTipo_llamada' + posicion + ' name=chkTipo_llamada' + posicion + ' value=' + clave_categoria_tipo_llamada + ':' + result[posicion].Tipo_Llamada_Id + ' />' + result[posicion].Descripcion_Tipo_Llamada + '</br>';
                                posicion++;
                            }
                            cadenaCheck = divCategoria + cadenaCheck + '</div>';
                            $(cadenaCheck).appendTo($container);
                            if (posicion <= result.length - 1) {
                                clave_categoria_tipo_llamada = result[posicion].Clave_Categoria_Tipo_Llamada;
                            }
                            j++;
                        }
                    }
                }
            },
            error: function (data, status, xhr) {
                console.log(data);
            }
        })
    };

    captura.ValidaTipoLlamada = function () {
        var selected = '';
        $("#form_captura div[data-valmsg-for=tipo_llamada]").addClass('hidden');
        $('#form_captura input[type=checkbox]').each(function () {
            if (this.checked) {
                selected += $(this).val() + ', ';
            }
        });
        if (selected !== '') {
            $("#form_captura div[data-valmsg-for=tipo_llamada]").removeClass('is-invalid');
            $("#form_captura div[data-valmsg-for=tipo_llamada]").addClass('is-valid');
            selected = selected.trim().substring(0, selected.trim().length - 1);
        }
        else {
            $("#form_captura div[data-valmsg-for=tipo_llamada]").removeClass('hidden');
            $("#form_captura div[data-valmsg-for=tipo_llamada]").addClass('is-invalid');
            return '';
        }
        return selected;
    };

    captura.Guardar = function (datos, urlDestino) {
        datos.CadenaTipoLlamada = captura.ValidaTipoLlamada();
        if (datos.CadenaTipoLlamada !== "") {
            // Proceso de Guardar Llamada
            $.ajax({
                url: urlDestino,
                type: 'POST',
                async: false,
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                data: JSON.stringify(datos),
                beforeSend: function () {
                    captura.webControls.btnGuardar.attr('disabled', true);
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
                captura.webControls.btnGuardar.attr('disabled', false);
                captura.limpiar();

            }).fail(function (data, textStatus, xhr) {
                console.log(data, textStatus, xhr);
                captura.webControls.btnGuardar.attr('disabled', false);
            });
        }
    };
    captura.iniciar();
});

function getTelefono(telefono) {
    $('#hidTelefono').val(telefono);
}

$(document).ready(function () {
    $("#txtTelefono").keydown(function (event) {
        if (event.shiftKey) {
            event.preventDefault();
        }
        if (event.keyCode === 46 || event.keyCode === 8) {
        }
        else {
            if (event.keyCode < 95) {
                if (event.keyCode < 48 || event.keyCode > 57) {
                    event.preventDefault();
                }
            }
            else {
                if (event.keyCode < 96 || event.keyCode > 105) {
                    event.preventDefault();
                }
            }
        }
    });
});