var objectRow;
var historico = {
    url_participante_buscar: appBaseUrl + '/api/Customer/FindByUser',
    url_consultar_historico: appBaseUrl + '/api/Llamada/Historico',
    url_consultar_llamada: appBaseUrl + '/api/Llamada/ConsultaLlamada',
    url_consultar_historico_detalle: appBaseUrl + '/api/Llamada/HistoricoDetalle',

    table_historico: $('#tabla_historico'),

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
            url: historico.url_participante_buscar,
            type: 'GET',
            data: { id: user.id },
            async: true,
            beforeSend: function () {
                $('.loading').removeClass('hidden');
            },
            success: function (data, status, xhr) {
                historico.plasmarDatos(data[0]);
                $('.card').removeClass('hidden');
                $('.loading').addClass('hidden');
                $('.bootstrap-select').removeClass('is-valid is-invalid');
            },
            error: function (data, status, xhr) {
                console.log(data);
            }
        });
        this.consultaHistorico(user.id);
        // Obtiene los Datos de la Consulta de la Llamada
        this.table_historico.on('click', 'tr', function (event) {
            objectRow = historico.table_historico.DataTable().row(this).data();
            $.ajax({
                url: historico.url_consultar_llamada,
                type: 'GET',
                data: { llamada_id: objectRow.id },
                async: true,
                success: function (data, status, xhr) {
                    var array = JSON.parse(data);
                    $('#tdParticipante').text($('#participante').val());
                    $('#tdPersona').text(array[0].nombre_llama);
                    $('#tdTelefono').text(array[0].telefono);
                    $('#tdFecha').text(array[0].fecha);
                    $('#txtDescripcion').text(array[0].descripcion);
                    $('#txtCierre').text(array[0].Cierre);
                    $('#tdUsuario').text(array[0].usuario);
                },
                error: function (data, status, xhr) {
                    console.log(data);
                }
            })
            // Obtiene el Detalle de la Llamada
            historico.consultaHistoricoDetalle();
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
    },

    formatoFecha: function (fecha) {
        var date = new Date(fecha);
        var formatted = ("0" + date.getDate()).slice(-2) + "/" + ("0" + (date.getMonth() + 1)).slice(-2) + "/" + date.getFullYear();
        return formatted;
    },

    maysPrimera: function (cadena) {
        return cadena.charAt(0).toUpperCase() + cadena.slice(1);
    },

    consultaHistorico: function (participante_id) {
        var dataResult = [];
        var rcolumns = [];
        $.getJSON(historico.url_consultar_historico, { participante_id: participante_id }, function (json) {
            var array = JSON.parse(json);
            if (array.length > 0) {
                const types = Object.keys(array[0]);
                var columnDefault = [];
                $.each(types, function (index, name) {
                    if (name.toLowerCase() !== "participante") {
                        const add = { sTitle: historico.maysPrimera(name).replace('_', ' '), mData: name }
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

            if ($.fn.DataTable.isDataTable(historico.table_historico)) {
                historico.table_historico.DataTable().clear().destroy();
                historico.table_historico.empty();
            }
            if (dataResult[0].aaData.length < 1) {
                dataResult[0].aoColumns = [
                    { title: 'Fecha', data: 'Fecha' },
                    { title: 'Caso', data: 'Caso' },
                    { title: 'Descripción', data: 'Descripcion' },
                    { title: 'Usuario', data: 'Usuario' },
                    { title: '', data: 'id' }
                ]
            }
            historico.table_historico.DataTable(dataResult[0]);
        });
    },
    table: null,
    consultaHistoricoDetalle: function () {
        console.log("Peticion");
        $('#tabla_historico_detalle p').empty();
        $('#tabla_historico_detalle li').empty();
        $.getJSON(historico.url_consultar_historico_detalle, { llamada_id: objectRow.id }, function (json) {
            var array = JSON.parse(json);
            if (array.length > 0) {
                const types = Object.keys(array[0]);
                var categoria = array[0].categoria;
                $wkslist = $('#wkslist');
                var cadenaCategoria = "";
                var li = "";
                for (i = 0; i <= array.length - 1; i++) {
                    if (i <= array.length - 1) {
                        if (i === 0 && categoria === array[i].categoria) {
                            cadenaCategoria = "<p style='margin-top:10px;'><b>" + categoria + "</b></p>";
                        }
                        else if (categoria !== array[i].categoria) {
                            cadenaCategoria = "<p style='margin-top:10px;'><b>" + array[i].categoria + "</b></p>";
                            categoria = array[i].categoria;
                        }
                        else {
                            cadenaCategoria = "";
                        }
                        li = li + cadenaCategoria + '<li><input type="checkbox" name="' + i + '" id="' + i + '" checked disabled/>' + '<label for="' + i + '">' + array[i].tipo_llamada + '</label></li>';
                    }
                }
                $(li).appendTo($wkslist);
            }
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
historico.iniciar();