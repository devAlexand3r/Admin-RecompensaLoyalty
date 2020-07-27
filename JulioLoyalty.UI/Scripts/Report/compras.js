$(function (e) {
    var reporte = new Object();
    reporte.urlAPI = {
        url_reporte_compras: appBaseUrl + '/api/Report/operativo',
        url_consultar_compras_detalle: appBaseUrl + '/api/Report/operativo_detalle',
    };

    reporte.webControls = {
        fec_ini: $('#fec_ini'),
        fec_fin: $('#fec_fin'),
        btnBuscar: $('#btnBuscar'),
        btnExportar: $('#btnExportar'),
        table_reporte_dinamico: $('#tabla_reporte_dinamico'),
        table_compras_detalle: $('#table_compras_detalle'),
        hidUserName: $('#hidUserName'),
    };

    reporte.selValues = {
        username: $('#hidUserName').val()
    },

    reporte.valido = {
        fecha: false
    }

    reporte.iniciar = function () {
        this.agregarEventos();
        $('.date').datepicker({
            format: "dd/mm/yyyy",
            language: 'es',
            autoclose: true,
            todayHighlight: true
        });
        var hoy = new Date();
        var primerDiaMes = new Date(hoy.getFullYear(), hoy.getMonth(), 1);
        $("#fec_ini").datepicker().datepicker("setDate", primerDiaMes);
        $("#fec_ini").datepicker().datepicker("setEndDate", hoy);
        $("#fec_fin").datepicker().datepicker("setDate", hoy);
        $("#fec_fin").datepicker().datepicker("setEndDate", hoy);
        $('.selectpicker').selectpicker({
            language: 'es',
            title: 'MOSTRAR TODOS'
        }).selectpicker('refresh');
    };

    reporte.agregarEventos = function () {
        reporte.webControls.fec_ini.change(function () {
            reporte.valido.fecha = reporte.Requerido(this, 'date');
        });
        reporte.webControls.fec_fin.change(function () {
            reporte.valido.fecha = reporte.Requerido(this, 'date');
        });
        reporte.webControls.btnBuscar.click(function () {
            reporte.obtenerReporteDinamico();
        });
        reporte.webControls.table_reporte_dinamico.on('click', 'td', function (event) {
            objectRow = reporte.webControls.table_reporte_dinamico.DataTable().row(this).data();
            var event_tarjet = $(event.target).attr('class');
            const selValue = $(event.target).text(); // Obtiene el valor de la celda
            if (selValue > 0) {
                // mostrar detalle de registro
                if (event_tarjet === 'label label-default') {
                    var trid = $(this).closest('tr').attr('id');
                    var colIndex = $(this).parent().children().index($(this));
                    reporte.consultaComprasDetalle(colIndex, trid);
                    $('td').removeClass('td-seleted');
                    if (!$(this).hasClass('td-seleted')) {
                        $(this).addClass('td-seleted');
                    }
                    $('#details').modal('show');
                }
            }
        });
    };

    reporte.Requerido = function (thisObj, type) {
        var valido = true;
        $("div[data-valmsg-for=" + thisObj.name + "]").addClass('hidden');
        if (type === 'date') {
            var vregexNaix = /^(0[1-9]|[1-2]\d|3[01])(\/)(0[1-9]|1[012])\2(\d{4})$/;
            if (!vregexNaix.test(thisObj.value) || thisObj.value.length > 10) {
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

    reporte.maysPrimera = function (texto) {
        return texto.charAt(0).toUpperCase() + texto.slice(1).toLowerCase()
    };

    reporte.formatoMembresia = function (number) {
        return '\u200C' + number; // Corrige el problema al exportar a excel con números de 16 digitos
    }

    reporte.formatoCantidad = function (val) {
        while (/(\d+)(\d{3})/.test(val.toString())) {
            val = val.toString().replace(/(\d+)(\d{3})/, '$1' + ',' + '$2');
        }
        return val;
    }

    reporte.obtenerReporteDinamico = function () {
        if (reporte.valido.fecha === true) {
            // Bloquea página para procesar  
            $.blockUI({
                css: {
                    border: 'none',
                    padding: '15px',
                    backgroundColor: '#000',
                    '-webkit-border-radius': '10px',
                    '-moz-border-radius': '10px',
                    opacity: .5,
                    color: '#fff'
                },
                message: "Procesando..."
            });
            var dataResult = [];
            var rcolumns = [];
            $.getJSON(this.urlAPI.url_reporte_compras,
                {
                    tipo_reporte_id: 2,
                    fecha_inicial: $('#fec_ini').val(),
                    fecha_final: $('#fec_fin').val(),
                },
                function (json) {
                    var array = JSON.parse(json);
                    if (array.length > 0) {
                        $('#pNotFound').attr('style', 'display:none');
                        const types = Object.keys(array[0]);
                        var columnDefault = [];
                        $.each(types, function (index, name) {
                            if (index === 2) // Oculta la columna 0
                                columnDefault.push({ visible: false, targets: 0 });
                            if (name.toUpperCase() === "CLASSIC" || name.toUpperCase() === "PREMIUM" || name.toUpperCase() === "ELITE" || name.toUpperCase() === "TOTAL") {
                                var controles = {
                                    sTitle: name,
                                    mData: name,
                                    mRender: function (data, type, row) {
                                        var campo_id = reporte.formatoCantidad(data);
                                        if (reporte.selValues.username !== "vianey_hernandez")
                                            return '<div class="label label-default">' + campo_id + '</div>';
                                        else
                                            return '<div>' + campo_id + '</div>';
                                    }
                                };
                                rcolumns.push(controles);
                            }
                            else {
                                const add = { sTitle: name.replace('_', ' '), mData: name }
                                rcolumns.push(add);
                            }
                        });
                    } else {
                        $("#pNotFound").removeAttr('style');
                        if ($.fn.DataTable.isDataTable(reporte.webControls.table_reporte_dinamico)) {
                            reporte.webControls.table_reporte_dinamico.DataTable().clear().destroy();
                            reporte.webControls.table_reporte_dinamico.empty();
                        }
                        setTimeout($.unblockUI, 2000); // Después desbloquea página
                        return;
                    }

                    var buttonExport = [];
                    var conf = {
                        extend: 'excel',
                        footer: true,
                        text: 'Exportar a Excel',
                        className: 'btn btn-primary',
                        title: null,
                        exportOptions: {
                            columns: ':visible'
                        },
                        customize: function (xlsx, row) {
                            var sheet = xlsx.xl.worksheets['sheet1.xml'];
                            $('row c[r^="B"],row c[r^="C"],row c[r^="D"],row c[r^="E"]', sheet).attr('s', 63);
                            // estilos personalizados                            
                            $('row:first c', sheet).attr('s', '2');
                        }
                    };
                    buttonExport.push(conf);
                    var footer = "<tfoot><tr style='font-weight: bold;'>";
                    $.each(rcolumns, function (index, row) {
                        if (index === 1) {
                            footer += "<td>TOTALES</td>"
                        } else {
                            footer += "<td></td>"
                        }
                    });
                    footer += "</tr></tfoot>";
                    reporte.webControls.table_reporte_dinamico.empty().append(footer);

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
                        ordering: true,
                        info: true,
                        dom: 'Bfrtip',
                        buttons: buttonExport,
                        footerCallback: function (row, data, start, end, display) {
                            var api = this.api();
                            var index = 0;
                            $.each(data[0], function (value) {
                                if (index > 1) {
                                    var total_column = api.column(index, {}).data().sum();
                                    total_column = reporte.formatoCantidad(total_column);
                                    $(api.column(index).footer()).html(total_column);
                                }
                                index += 1;
                            });
                        },
                        drawCallback: function () {
                        }
                    });
                    reporte.webControls.table_reporte_dinamico.DataTable(dataResult[0]);
                    setTimeout($.unblockUI, 2000); // Después desbloquea página                    
                });
        }
    };
    reporte.table_detalle = null;

    reporte.consultaComprasDetalle = function (colIndex, id) {
        console.log("Peticion Detalle");
        var dataResult = [];
        var rcolumns = [];
        $.getJSON(reporte.urlAPI.url_consultar_compras_detalle, { tipo_reporte_id: 2, fecha_inicial: $('#fec_ini').val(), fecha_final: $('#fec_fin').val(), campo_id: colIndex, distribuidor_id: id }, function (json) {
            var array = JSON.parse(json);
            if (array.length > 0) {
                const types = Object.keys(array[0]);
                var columnDefault = [];
                $.each(types, function (index, name) {
                    const add = { sTitle: reporte.maysPrimera(name).replace('_', ' '), mData: name }
                    rcolumns.push(add);
                })
            } else {
                if ($.fn.DataTable.isDataTable(reporte.webControls.table_compras_detalle)) {
                    reporte.table_detalle.clear().destroy();
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

            if ($.fn.DataTable.isDataTable(reporte.webControls.table_compras_detalle)) {
            }
            reporte.table_detalle = reporte.webControls.table_compras_detalle.DataTable(dataResult[0]);
        })
    };

    reporte.iniciar();
});