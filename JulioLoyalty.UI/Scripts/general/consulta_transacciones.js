
var objectRow;
var consulta = {
    url_consultar_datos: appBaseUrl + '/api/Customer/FindByUser',
    url_consultar_transacciones: appBaseUrl + '/api/Customer/ConsultaTransaccion',
    url_consultar_transacciones_detalle: appBaseUrl + '/api/Customer/CTransaccionDetalle',
    url_consultar_transacciones_comentarios: appBaseUrl + '/api/Customer/CTransaccionComentarios',
    table_estado_cuenta: $('#table_estado_cuenta'),
    table_movimientos: $('#table_movimientos'),
    table_movimientos_detalle: $('#table_movimientos_detalle'),
    table_comentarios_detalle: $('#table_comentarios_detalle'),

    iniciar: function () {
        var lUser = localStorage.getItem('objUser');
        if (lUser === null) {
            //$swal.info('Socia no encontrado', 'Le recomendamos que seleccione el registro correspondiente');
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
        this.consultarEstadoCuenta(user.id);
        this.consultaTransacciones(user.id);

        this.table_movimientos.on('click', 'tr', function (event) {
            objectRow = consulta.table_movimientos.DataTable().row(this).data();
            var event_tarjet = $(event.target).attr('class');
            // mostrar detalle de transaccion
            if (event_tarjet === 'fas fa-expand' || event_tarjet === 'btn btn-info btn-sm') {
                consulta.consultaTransaccionesDetalle();
                consulta.consultaTransaccionesComentarios();
                //$('.modal').on('shown.bs.modal', function () {
                //    consulta.consultaTransaccionesDetalle();
                //})
            }
        });

        this.table_estado_cuenta.on('click', 'tr', function (event) {
            var row = consulta.table_estado_cuenta.DataTable().row(this).data();
            var event_tarjet = $(event.target).attr('class');

            // mostrar detalle de transaccion
            if (event_tarjet === 'fas fa-expand' || event_tarjet === 'btn btn-success btn-sm') {
                var saldo = row.saldo.toString().replace(/(\d)(?=(\d{3})+(?!\d))/g, "$1,");
                $('#tdPpuntos').text(saldo);

                $('#tdFechaActual').text(consulta.formatoFecha(row.participante_nivel[0].fecha_inicial));
                $('#tdFechaFinal').text(consulta.formatoFecha(row.participante_nivel[0].fecha_final));

                //.toFixed(2).replace(/\d(?=(\d{3})+\.)/g, '$&,');                
                $('#tdMonto').text(consulta.formatoCantidad(row.participante_nivel[0].importe, 2));

                var nivel = row.nivel === null ? '' : row.nivel === 1 ? 'CLASSIC' : row.nivel === 2 ? 'PREMIUM' : 'ELITE';
                $('#tdStatus').text(nivel);

                $('.modal').on('shown.bs.modal', function () { })
            }
        });

    },
    formatoFecha: function (fecha) {
        var date = new Date(fecha);
        var formatted = ("0" + date.getDate()).slice(-2) + "/" + ("0" + (date.getMonth() + 1)).slice(-2) + "/" + date.getFullYear();
        return formatted;
    },
    maysPrimera: function (cadena) {
        return cadena.charAt(0).toUpperCase() + cadena.slice(1);
    },

    consultarEstadoCuenta: function (participante_id) {
        this.table_estado_cuenta.DataTable().destroy();
        this.table_estado_cuenta.DataTable({
            language: {
                url: '//cdn.datatables.net/plug-ins/1.10.15/i18n/Spanish.json'
            },
            ajax: {
                url: consulta.url_consultar_datos,
                data: function (d) {
                    d.id = participante_id;
                },
                dataSrc: function (json) {
                    return json;
                }
            },
            //lengthChange: false,
            rowId: 'id',
            scrollCollapse: true,
            paging: false,
            searching: false,
            //ordering: false,
            info: false,
            columns: [
                {
                    mData: 'id',
                    width: '20px',
                    orderable: false,
                    mRender: function (data, type, row) {
                        return '<div style="text-align:center;"><button class="btn btn-success btn-sm" data-title="Resumen de saldo" data-toggle="modal" data-target="#summary"><i class="fas fa-expand"></i></button>';
                    }
                },
                { data: 'clave' },
                {
                    data: 'nombre',
                    width: '300px',
                    render: function (data, type, row) {
                        return row.nombre + ' ' + row.apellido_paterno + ' ' + row.apellido_materno;
                    }
                },
                {
                    data: 'fecha_nacimiento',
                    render: function (data, type, row) {
                        var rol = $('#rolName').val();
                        return rol === "-" ? "" : data === null ? "" : consulta.formatoFecha(data);
                    }
                },
                {
                    data: 'correo_electronico',
                    render: function (data, type, row) {
                        var rol = $('#rolName').val();
                        return rol === "-" ? "" : data === null ? "" : data;
                    }
                },
                {
                    data: 'status',
                    render: function (data, type, row) {
                        var rol = $('#rolName').val();
                        return rol === "-" ? "" : data === null ? "" : data;
                    }
                },
                {
                    data: 'saldo',
                    render: function (data, type, row) {
                        return data.toString().replace(/(\d)(?=(\d{3})+(?!\d))/g, "$1,")
                    }
                }
            ],

            fnRowCallback: function (nRow, aData, iDisplayIndex) {
                const value = parseInt(aData.saldo);
                if (value < 0) {
                    $(nRow).addClass('lightBlue');
                }
            }


        });
    },
    consultaTransacciones: function (participante_id) {
        var dataResult = [];
        var rcolumns = [];
        $.getJSON(consulta.url_consultar_transacciones, { id: participante_id }, function (json) {
            var array = JSON.parse(json);
            if (array.length > 0) {
                const types = Object.keys(array[0]);
                var columnDefault = [];
                $.each(types, function (index, name) {
                    if (name === "importe" || name === "puntos") {
                        if (name === "importe") {
                            const add = {
                                sTitle: consulta.maysPrimera(name).replace('_', ' '),
                                mData: name,
                                mRender: function (data, type, full, meta) {
                                    return consulta.formatoCantidad(data, 2);
                                }
                            }
                            rcolumns.push(add);
                        }
                        if (name === "puntos") {
                            const add = {
                                sTitle: consulta.maysPrimera(name).replace('_', ' '),
                                mData: name,
                                mRender: function (data, type, row) {
                                    return data.toString().replace(/(\d)(?=(\d{3})+(?!\d))/g, "$1,");
                                }
                            }
                            rcolumns.push(add);
                        }
                    } else {
                        const add = { sTitle: consulta.maysPrimera(name).replace('_', ' '), mData: name }
                        rcolumns.push(add);
                    }
                    // Agregar boton de control
                    if ((index + 1) === 1) {
                        var controles = {
                            mData: 'id',
                            sWidth: "20px",
                            mRender: function (data, type, row) {
                                return '<div style="text-align:center;"><button class="btn btn-info btn-sm" data-title="Ver detalles" data-toggle="modal" data-target="#details"><i class="fas fa-expand"></i></button>';
                            }
                        };
                        rcolumns.push(controles);
                    }
                    if (index === 2)
                        columnDefault.push({ visible: false, targets: 0 });
                    //if (index > 4)
                    //    columnDefault.push({ visible: false, targets: index });
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
                info: true,
                "fnRowCallback": function (nRow, aData, iDisplayIndex) {
                    const value = parseInt(aData.importe);
                    const puntos = parseInt(aData.puntos);
                    if (value < 0 || puntos < 0) {
                        $(nRow).addClass('lightBlue');
                    }
                }
            });

            if ($.fn.DataTable.isDataTable(consulta.table_movimientos)) {
                consulta.table_movimientos.DataTable().clear().destroy();
                consulta.table_movimientos.empty();
            }
            if (dataResult[0].aaData.length < 1) {
                dataResult[0].aoColumns = [
                    { title: 'Fecha', data: 'fecha' },
                    { title: 'Tipo de transacción', data: 'tipo_de_transaccion' },
                    { title: 'Puntos', data: 'puntos' },
                    { title: '', data: 'id' }
                ]
            }
            consulta.table_movimientos.DataTable(dataResult[0]);
        });
    },
    table_detalle: null,
    table_comentarios: null,
    consultaTransaccionesDetalle: function () {
        console.log("Peticion Detalle");
        var dataResult = [];
        var rcolumns = [];
        $.getJSON(consulta.url_consultar_transacciones_detalle, { id: objectRow.id }, function (json) {
            var array = JSON.parse(json);
            if (array.length > 0) {
                const types = Object.keys(array[0]);
                var columnDefault = [];
                $.each(types, function (index, name) {
                    if (name === "importe" || name === "puntos" || name === "pago" || name === "precio") {
                        if (name === "importe" || name === "precio") {
                            const add = {
                                sTitle: consulta.maysPrimera(name).replace('_', ' '),
                                mData: name,
                                mRender: function (data, type, row) {
                                    return consulta.formatoCantidad(data, 2);
                                }
                            }
                            rcolumns.push(add);
                        }
                        if (name === "puntos") {
                            const add = {
                                sTitle: consulta.maysPrimera(name).replace('_', ' '),
                                mData: name,
                                mRender: function (data, type, row) {
                                    return data.toString().replace(/(\d)(?=(\d{3})+(?!\d))/g, "$1,");
                                }
                            }
                            rcolumns.push(add);
                        }
                        if (name === "pago") {
                            const add = {
                                sTitle: consulta.maysPrimera(name).replace('_', ' '),
                                mData: name,
                                mRender: function (data, type, row) {
                                    return consulta.formatoCantidad(data, 2);
                                }
                            }
                            rcolumns.push(add);
                        }
                    } else {
                        const add = { sTitle: consulta.maysPrimera(name).replace('_', ' '), mData: name }
                        rcolumns.push(add);
                    }
                    // Agregar boton de control
                    //if ((index + 1) === 1) {
                    //    var controles = {
                    //        mData: 'id',
                    //        sWidth: "20px",
                    //        mRender: function (data, type, row) {
                    //            return '<div style="text-align:center;"><button class="btn btn-info btn-sm" data-title="Ver detalles" data-toggle="modal" data-target="#details"><i class="fas fa-expand"></i></button>';
                    //        }
                    //    };
                    //    rcolumns.push(controles);
                    //}
                    if (index === 2)
                        columnDefault.push({ visible: false, targets: 0 });
                    //if (index > 4)
                    //    columnDefault.push({ visible: false, targets: index });
                });
            } else {
                if ($.fn.DataTable.isDataTable(consulta.table_movimientos_detalle)) {
                    consulta.table_detalle.clear().destroy();
                    consulta.table_movimientos_detalle.empty();
                }
                return;
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

            if ($.fn.DataTable.isDataTable(consulta.table_movimientos_detalle)) {
                //consulta.table_movimientos_detalle.DataTable().clear().destroy();
                //consulta.table_movimientos_detalle.empty();

                //consulta.table.clear().destroy();
                //consulta.table_movimientos_detalle.empty();
            }
            //if (dataResult[0].aaData.length < 1) {
            //    dataResult[0].aoColumns = [
            //        { title: 'Fecha', data: 'fecha' },
            //        { title: 'Tipo de transacción', data: 'tipo_de_transaccion' },
            //        { title: 'Puntos', data: 'puntos' },
            //        { title: '', data: 'id' }
            //    ]
            //}
            consulta.table_detalle = consulta.table_movimientos_detalle.DataTable(dataResult[0]);
        });




        //if ($.fn.DataTable.isDataTable(consulta.table_movimientos_detalle)) {
        //    consulta.table.ajax.reload();
        //    return;
        //}
        ////this.table_movimientos_detalle.DataTable().destroy();
        //consulta.table = this.table_movimientos_detalle.DataTable({
        //    bDestroy: true,
        //    language: {
        //        url: '//cdn.datatables.net/plug-ins/1.10.15/i18n/Spanish.json'
        //    },
        //    ajax: {
        //        url: consulta.url_consultar_transacciones_detalle,
        //        data: function (d) {
        //            d.id = objectRow.id;
        //        },
        //        dataSrc: function (json) {
        //            return JSON.parse(json);
        //        }
        //    },
        //    lengthChange: false,
        //    rowId: 'id',
        //    scrollCollapse: true,
        //    paging: true,
        //    searching: true,
        //    ordering: false,
        //    info: false,
        //    columns: [
        //        { data: 'clave', width: '50px' },
        //        { data: 'descripcion' },
        //        { data: 'cantidad', width: '50px' },
        //        {
        //            data: 'importe',
        //            width: '50px',
        //            render: function (data, type, row) {
        //                return consulta.formatoCantidad(data, 2);
        //            }
        //        },
        //        {
        //            data: 'puntos',
        //            width: '50px',
        //            render: function (data, type, row) {
        //                return data.toString().replace(/(\d)(?=(\d{3})+(?!\d))/g, "$1,")
        //            }
        //        },
        //        { data: 'descuento', width: '50px' },
        //    ]
        //});
    },

    consultaTransaccionesComentarios: function () {
        console.log("Peticion Comentarios");
        var dataResult = [];
        var rcolumns = [];
        $.getJSON(consulta.url_consultar_transacciones_comentarios, { transaccion_id: objectRow.id }, function (json) {
            var array = JSON.parse(json);
            if (array.length > 0) {
                const types = Object.keys(array[0]);
                var columnDefault = [];
                $.each(types, function (index, name) {
                    const add = { sTitle: consulta.maysPrimera(name).replace('_', ' '), mData: name }
                    rcolumns.push(add);
                    if (index === 2)
                        columnDefault.push({ visible: false, targets: 0 });
                });
            } else {
                if ($.fn.DataTable.isDataTable(consulta.table_comentarios_detalle)) {
                    consulta.table_comentarios.clear().destroy();
                    consulta.table_comentarios_detalle.empty();
                }
                return;
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
            if ($.fn.DataTable.isDataTable(consulta.table_comentarios_detalle)) {
            }
            consulta.table_comentarios = consulta.table_comentarios_detalle.DataTable(dataResult[0]);
        });
    },

    formatoCantidad: function (number, fraction) {
        //const formatter = new Intl.NumberFormat('en-US', {
        //    style: 'currency',
        //    currency: 'USD',
        //    minimumFractionDigits: fraction
        //})
        //return formatter.format(number);
        return '$' + number.toFixed(2).replace(/\d(?=(\d{3})+\.)/g, '$&,');
    }
};
consulta.iniciar();