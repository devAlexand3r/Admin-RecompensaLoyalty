$(function (e) {
    var reporte = new Object();
    reporte.urlAPI = {
        url_reporte_ventas: appBaseUrl + '/api/Report/operativo'
    };

    reporte.webControls = {
        fec_ini: $('#fec_ini'),
        fec_fin: $('#fec_fin'),
        btnBuscar: $('#btnBuscar'),
        btnExportar: $('#btnExportar'),
        table_reporte_dinamico: $('#tabla_reporte_dinamico')
    };

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
        if (val !== undefined) {
            while (/(\d+)(\d{3})/.test(val.toString())) {
                val = val.toString().replace(/(\d+)(\d{3})/, '$1' + ',' + '$2');
            }
        }
        return val;
    }

    reporte.formatCurrency = function (num) {
        if (num !== undefined) {
            num = num.toString().replace(/\$|\,/g, '');
            if (isNaN(num))
                num = "0";
            sign = (num == (num = Math.abs(num)));
            num = Math.floor(num * 100 + 0.50000000001);
            cents = num % 100;
            num = Math.floor(num / 100).toString();
            if (cents < 10)
                cents = "0" + cents;
            for (var i = 0; i < Math.floor((num.length - (1 + i)) / 3) ; i++)
                num = num.substring(0, num.length - (4 * i + 3)) + ',' + num.substring(num.length - (4 * i + 3));
            return ((sign) ? '' : '-') + '$' + num + '.' + cents;
        }
        else
            return num;
    }

    reporte.formatPorcentaje = function (num) {
        if (num !== undefined) {
            num = parseFloat(num).toFixed(2) + '%';
        }
        return num;
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
            $.getJSON(this.urlAPI.url_reporte_ventas,
                {
                    tipo_reporte_id: 5,
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
                            if (name.toUpperCase() === "TOTAL VENTAS" || name.toUpperCase() === "VENTAS LOYALTY" || name.toUpperCase() === "TICKET PROMEDIO" || name.toUpperCase() === "COMPRA PROMEDIO") {
                                const add = {
                                    sTitle: name,
                                    mData: name,
                                    mRender: function (data, type, row) {
                                        return reporte.formatCurrency(data);
                                    }
                                }
                                rcolumns.push(add);
                            }
                            else if (name.toUpperCase() === "ACTIVACIONES" || name.toUpperCase() === "SOCIAS COMPRA") {
                                const add = {
                                    sTitle: name,
                                    mData: name,
                                    mRender: function (data, type, row) {
                                        return reporte.formatoCantidad(data);
                                    }
                                }
                                rcolumns.push(add);
                            }
                            else if (name.toUpperCase() === "PORC PART") {
                                const add = {
                                    sTitle: name,
                                    mData: name,
                                    mRender: function (data, type, row) {
                                        return reporte.formatPorcentaje(data);
                                    }
                                }
                                rcolumns.push(add);
                            }
                            else {
                                const add = {
                                    sTitle: name.replace('_', ' '),
                                    mData: name
                                }
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
                            $('row c[r^="D"]', sheet).attr('s', 60);
                            $('row c[r^="E"],row c[r^="F"]', sheet).attr('s', 63);
                            $('row c[r^="B"],row c[r^="C"],row c[r^="G"], row c[r^="H"]', sheet).attr('s', 57);
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
                            var totalVtas = 0;
                            var totalVtasLoy = 0;
                            var totalSociasCompra = 0
                            var totalCompraPromedio = 0;
                            $.each(data[0], function (value) {
                                if (index > 1) {
                                    if (value.toUpperCase() == "TOTAL VENTAS")
                                        totalVtas = api.column(index, { }).data().sum();
                                    if (value.toUpperCase() == "VENTAS LOYALTY")
                                        totalVtasLoy = api.column(index, { }).data().sum();
                                    if (value.toUpperCase() == "SOCIAS COMPRA")
                                        totalSociasCompra = api.column(index, { }).data().sum();
                                    if (value.toUpperCase() == "TOTAL VENTAS" || value.toUpperCase() == "VENTAS LOYALTY") {
                                        var total_column = api.column(index, { }).data().sum();
                                        total_column = reporte.formatCurrency(total_column);
                                        $(api.column(index).footer()).html(total_column);
                                    }
                                    else if (value.toUpperCase() == "ACTIVACIONES" || value.toUpperCase() == "SOCIAS COMPRA") {
                                        var total_column = api.column(index, { }).data().sum();
                                        total_column = reporte.formatoCantidad(total_column);
                                        $(api.column(index).footer()).html(total_column);
                                    }
                                    else if (value.toUpperCase() == "PORC PART") {
                                        var totalPorcPart = (totalVtasLoy / totalVtas) * 100;
                                        totalPorcPart = reporte.formatPorcentaje(totalPorcPart)
                                        $(api.column(index).footer()).html(totalPorcPart);
                                    }
                                    else if (value.toUpperCase() == "TICKET PROMEDIO") {
                                        var total_column = api.column(index, { }).data().average();
                                        total_column = reporte.formatCurrency(total_column);
                                        $(api.column(index).footer()).html(total_column);
                                    }
                                    else if (value.toUpperCase() == "COMPRA PROMEDIO") {
                                        totalCompraPromedio = totalVtasLoy / totalSociasCompra;
                                        totalCompraPromedio = reporte.formatCurrency(totalCompraPromedio);
                                        $(api.column(index).footer()).html(totalCompraPromedio);
                                    }
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
    reporte.iniciar();
});