var objectRow;
var seguimiento = {
    url_participante_buscar: appBaseUrl + '/api/Customer/FindByUser',
    url_consultar_seguimiento: appBaseUrl + '/api/Llamada/Seguimiento',
    url_consultar_seguimiento_detalle: appBaseUrl + '/api/Llamada/SeguimientoDetalle',
    url_buscar_escalamiento: appBaseUrl + '/api/Llamada/Escalamiento',
    url_buscar_status: appBaseUrl + '/api/Llamada/Status',
    url_guarda_seguimiento: appBaseUrl + '/api/Llamada/GuardarSeguimiento',

    table_seguimiento: $('#tabla_seguimiento'),
    table_seguimiento_detalle: $('#tabla_seguimiento_detalle'),

    hidLlamada_id: $('#hidLlamada_id'),
    txtComentarios: $('#comentarios'),
    hidParticipante_id: $('#participante_id'),
    txtParticipante: $('#participante'),
    hidClave: $('#clave'),
    hidDistribuidor: $('#distribuidor_id'),
    selectEscalamiento: $('#escalamiento'),
    selectStatus: $('#status'),
    btnGuardar: $('#btnGuardar'),

    iniciar: function () {
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
        //this.ajaxRequest('', 'GET', {}, this.crearTablas)
        $.ajax({
            url: seguimiento.url_participante_buscar,
            type: 'GET',
            data: { id: user.id },
            async: true,
            beforeSend: function () {
                $('.loading').removeClass('hidden');
            },
            success: function (data, status, xhr) {
                seguimiento.plasmarDatos(data[0]);
                $('.card').removeClass('hidden');
                $('.loading').addClass('hidden');
                $('.bootstrap-select').removeClass('is-valid is-invalid');
            },
            error: function (data, status, xhr) {
                console.log(data);
            }
        });
        this.consultaSeguimiento(user.id);
        // Obtiene los Datos de la Consulta de Seguimiento
        this.table_seguimiento.on('click', 'tr', function (event) {
            objectRow = seguimiento.table_seguimiento.DataTable().row(this).data();
            $('#hidLlamada_id').val(objectRow.id);
            $('#tdParticipante').text($('#participante').val());
            $('#tdDescripcion').text(objectRow.Descripción);
            $('#tdCaso').text(objectRow.id);
            //Obtiene el Detalle de Seguimiento
            seguimiento.consultaSeguimientoDetalle(objectRow.id);
        });
        // Iniciar la configuración de escalamiento y status        
        this.obtenerEscalamiento();
        this.obtenerStatus();
        this.agregarEventos();
    },

    Datos: {
        llamada_id: true,
        participante_id: false,
        participante: false,
        clave: false,
        distribuidor_id: false,
        comentarios: false,
        escalamiento: false,
        escalamiento_id: false,
        status: false,
        status_id: false,
    },

    agregarEventos: function () {
        this.txtComentarios.change(function () {
            seguimiento.Datos.comentarios = seguimiento.Requerido(this, 'text');
        });
        this.selectEscalamiento.change(function () {
            seguimiento.Datos.escalamiento_id = seguimiento.Requerido(this, 'select');
        });
        this.selectStatus.change(function () {
            seguimiento.Datos.status_id = seguimiento.Requerido(this, 'select');
        });

        this.btnGuardar.click(function () {
            seguimiento.txtComentarios.trigger('change');
            seguimiento.selectEscalamiento.trigger('change');
            seguimiento.selectStatus.trigger('change');
            var _valido = seguimiento.Datos;
            if (_valido.llamada_id === false ||
                _valido.comentarios === false ||
                _valido.escalamiento_id === false ||
                _valido.status_id === false
               ) {
                return;
            }
            // Proceso de Guardar Seguimiento            
            var _data = seguimiento.obtenerDatos();
            seguimiento.Guardar(_data, seguimiento.url_guarda_seguimiento);
            return;
        });
    },

    obtenerEscalamiento: function () {
        $.ajax({
            url: this.url_buscar_escalamiento,
            type: 'GET',
            async: false,
        }).done(function (data, textStatus, xhr) {
            seguimiento.selectEscalamiento.empty();
            $.each(data, function (index, row) {
                var html = ' <option value=\"' + row.Id + '">' + row.Descripcion + '</option>'
                seguimiento.selectEscalamiento.append(html);
            });
            seguimiento.selectEscalamiento.selectpicker("refresh");
        }).fail(function (data, textStatus, xhr) {
            console.log(data, textStatus, xhr);
        });
    },

    obtenerStatus: function () {
        $.ajax({
            url: this.url_buscar_status,
            type: 'GET',
            async: false,
        }).done(function (data, textStatus, xhr) {
            seguimiento.selectStatus.empty();
            $.each(data, function (index, row) {
                var html = ' <option value=\"' + row.Id + '">' + row.Descripcion + '</option>'
                seguimiento.selectStatus.append(html);
            });
            seguimiento.selectStatus.selectpicker("refresh");
        }).fail(function (data, textStatus, xhr) {
            console.log(data, textStatus, xhr);
        });
    },

    Requerido: function (thisObj, type) {
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
    },

    consultaSeguimientoDetalle: function (id) {
        var dataResult = [];
        var rcolumns = [];
        $.getJSON(seguimiento.url_consultar_seguimiento_detalle, { id: id }, function (json) {
            var array = JSON.parse(json);
            if (array.length > 0) {
                const types = Object.keys(array[0]);
                var columnDefault = [];
                $.each(types, function (index, name) {
                    const add = { sTitle: seguimiento.maysPrimera(name).replace('_', ' '), mData: name }
                    rcolumns.push(add);
                    //// Agregar boton de control
                    //if ((index + 1) === 1) {
                    //    var controles = {
                    //        mData: 'id',
                    //        sWidth: "20px",
                    //        mRender: function (data, type, row) {
                    //            return '<div style="text-align:center;"><button type="button" class="btn btn-info btn-sm" data-title="Ver detalles" data-toggle="modal" data-target="#details"><i class="fas fa-expand"></i></button>';
                    //        }
                    //    };
                    //    rcolumns.push(controles);
                    //}
                    if (index === 2)
                        columnDefault.push({ visible: false, targets: 0 });
                });
            }
            dataResult.push({
                aaData: array,
                aoColumns: rcolumns,
                aoColumnDefs: columnDefault,
                oLanguage: {
                    sUrl: '//cdn.datatables.net/plug-ins/1.10.15/i18n/Spanish.json'
                },
                //rowId: 'id',
                bDestroy: true,
                bLengthChange: false,
                bScrollCollapse: true,
                paging: true,
                searching: false,
                ordering: false,
                info: true
            });

            if ($.fn.DataTable.isDataTable(seguimiento.table_seguimiento_detalle)) {
                seguimiento.table_seguimiento_detalle.DataTable().clear().destroy();
                seguimiento.table_seguimiento_detalle.empty();
            }
            if (dataResult[0].aaData.length < 1) {
                dataResult[0].aoColumns = [
                    { title: 'Fecha', data: 'Fecha' },
                    { title: 'Comentarios', data: 'Comentarios' },
                    { title: 'Escalamiento', data: 'Escalamiento' },
                    { title: 'Status', data: 'Status' },
                    { title: '', data: 'id' }
                ]
            }
            seguimiento.table_seguimiento_detalle.DataTable(dataResult[0]);
        });
    },

    plasmarDatos: function (cliente) {
        var nombre_completo;
        if (cliente.nombre !== "")
            nombre_completo = cliente.nombre.trim();
        if (cliente.apellido_paterno !== "")
            nombre_completo = (nombre_completo + " " + cliente.apellido_paterno).trim();
        if (cliente.apellido_materno !== "")
            nombre_completo = (nombre_completo + " " + cliente.apellido_materno).trim();
        $('#participante_id').val(cliente.id);
        if (nombre_completo !== undefined)
            $('#participante').val(nombre_completo);
        $('#clave').val(cliente.clave);
        $('#distribuidor_id').val(cliente.distribuidor_id);
    },

    obtenerDatos: function () {
        var datos = {
            Llamada_id: this.hidLlamada_id.val(),
            Participante_id: this.hidParticipante_id.val(),
            Participante: this.txtParticipante.val().trim(),
            Clave: this.hidClave.val().trim(),
            Distribuidor_id: this.hidDistribuidor.val(),
            Comentarios: this.txtComentarios.val().trim(),
            Escalamiento_id: this.selectEscalamiento.val().trim(),
            Escalamiento: $('#escalamiento option:selected').text(),
            Status_id: this.selectStatus.val().trim(),
            Status: $('#status option:selected').text(),
        }
        return datos;
    },

    limpiar: function () {
        this.txtComentarios.val("");
        // Initialize the select picker.
        $('select[id=escalamiento]').selectpicker();
        // Extract the value of the first option.
        var sVal = $('select[id=escalamiento] option:first').val();
        // Set the "selected" value of the <select>.
        $('select[id=escalamiento]').val(sVal);
        // Force a refresh.
        $('select[id=escalamiento]').selectpicker('refresh');

        // Initialize the select picker.
        $('select[id=status]').selectpicker();
        // Extract the value of the first option.
        var sVal = $('select[id=status] option:first').val();
        // Set the "selected" value of the <select>.
        $('select[id=status]').val(sVal);
        // Force a refresh.
        $('select[id=status]').selectpicker('refresh');
    },

    formatoFecha: function (fecha) {
        var date = new Date(fecha);
        var formatted = ("0" + date.getDate()).slice(-2) + "/" + ("0" + (date.getMonth() + 1)).slice(-2) + "/" + date.getFullYear();
        return formatted;
    },

    maysPrimera: function (cadena) {
        return cadena.charAt(0).toUpperCase() + cadena.slice(1);
    },

    consultaSeguimiento: function (participante_id) {
        var dataResult = [];
        var rcolumns = [];
        $.getJSON(seguimiento.url_consultar_seguimiento, { participante_id: participante_id }, function (json) {
            var array = JSON.parse(json);
            if (array.length > 0) {
                const types = Object.keys(array[0]);
                var columnDefault = [];
                $.each(types, function (index, name) {
                    if (name.toLowerCase() !== "participante") {
                        const add = { sTitle: seguimiento.maysPrimera(name).replace('_', ' '), mData: name }
                        rcolumns.push(add);
                        // Agregar boton de control
                        if ((index + 1) === 1) {
                            var controles = {
                                mData: 'id',
                                sWidth: "20px",
                                mRender: function (data, type, row) {
                                    return '<div style="text-align:center;"><button type="button" class="btn btn-info btn-sm" data-title="Ver detalles" data-toggle="modal" data-target="#details"><i class="fas fa-expand"></i></button>';
                                }
                            };
                            rcolumns.push(controles);
                        }
                        if (index === 2)
                            columnDefault.push({ visible: false, targets: 0 });
                    }
                    else
                        $('#participante').val(array[0].Participante);
                });
            }
            dataResult.push({
                aaData: array,
                aoColumns: rcolumns,
                aoColumnDefs: columnDefault,
                oLanguage: {
                    sUrl: '//cdn.datatables.net/plug-ins/1.10.15/i18n/Spanish.json'
                },
                rowId: 'id',
                bDestroy: true,
                bLengthChange: false,
                bScrollCollapse: true,
                paging: true,
                searching: true,
                ordering: false,
                info: true
            });

            if ($.fn.DataTable.isDataTable(seguimiento.table_seguimiento)) {
                seguimiento.table_seguimiento.DataTable().clear().destroy();
                seguimiento.table_seguimiento.empty();
            }
            if (dataResult[0].aaData.length < 1) {
                dataResult[0].aoColumns = [
                    { title: 'Caso', data: 'id' },
                    { title: 'Fecha', data: 'fecha_llamada' },
                    { title: 'Nombre del que llama', data: 'nombre_llama' },
                    { title: 'Descripción', data: 'descripcion' },
                    { title: 'Status', data: 'status_seguimiento' },
                    { title: '', data: 'id' }
                ]
            }
            seguimiento.table_seguimiento.DataTable(dataResult[0]);
        });
    },
    table: null,

    Guardar: function (datos, urlDestino) {
        // Proceso de Guardar Llamada
        $.ajax({
            url: urlDestino,
            type: 'POST',
            async: false,
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify(datos),
            beforeSend: function () {
                seguimiento.btnGuardar.attr('disabled', true);
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
            seguimiento.btnGuardar.attr('disabled', false);
            var selectStatus = $('#status option:selected').text();
            if (selectStatus.indexOf('CERRADO') > -1) {
                $("#tabla_seguimiento_guardar").removeAttr("style").hide();
                // Remover fila de la tabla seguimiento    
                row = $('#hidLlamada_id').val();
                $('#tabla_seguimiento').DataTable().row('#' + row).remove().draw();
            }
            // Agregar nueva fila
            var d = new Date,
                   dformat = [d.getDate(),
                              (d.getMonth() + 1),
                               d.getFullYear()].join('/') +
                               ' ' +
                             [d.getHours(),
                               d.getMinutes(),
                               d.getSeconds()].join(':');

            escalamiento = $('#escalamiento option:selected').text();
            status = $('#status option:selected').text();
            var t = $('#tabla_seguimiento_detalle').DataTable();
            var row = {
                id: 0,
                Fecha: dformat,
                Comentarios: seguimiento.txtComentarios.val(),
                Escalamiento: escalamiento,
                Status: status
            };
            t.row.add(row).draw(false);
            seguimiento.limpiar();
        }).fail(function (data, textStatus, xhr) {
            console.log(data, textStatus, xhr);
            seguimiento.btnGuardar.attr('disabled', false);
        });
    },

    //crearTablas :  function(result){

    //},
    //ajaxRequest: function (_url, _type, _data, callback) {
    //    $.ajax({
    //        url: _url,
    //        type: _type,
    //        data: _data,
    //        async: true,
    //        beforeSend: function () {

    //        },
    //        success: callback,
    //        error: function (data, status, xhr) {
    //            console.log(data);
    //        }
    //    });
    //}
};
seguimiento.iniciar();