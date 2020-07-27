$(function (e) {
    var reporte = new Object();
    reporte.urlAPI = {
        url_consulta_usuario_distribuidor: appBaseUrl + '/api/Customer/Usuario/Distribuidor',
        url_obtener_catalogo: appBaseUrl + '/api/MCatalogo/FKCatalogos',
        url_reporte_actividad: appBaseUrl + '/api/Report/actividad'
    };

    reporte.webControls = {
        fec_ini: $('#fec_ini'),
        fec_fin: $('#fec_fin'),
        selNoTienda: $('#selNoTienda'),        
        btnBuscar: $('#btnBuscar'),
        btnExportar: $('#btnExportar'),
        table_reporte_dinamico: $('#tabla_reporte_dinamico')
    };

    reporte.valido = {
        fecha: false
    }

    reporte.iniciar = function () {
        var rol = $('#hidRole').val();
        this.agregarEventos();
        this.consultarUDistribuidor();        
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

        if (rol === "9699e07c-4546-46eb-8456-4a5777c1fb77") { // Para el Rol Tienda solo permite consultar el mes actual
            $("#fec_ini").datepicker().datepicker("setStartDate", primerDiaMes);
            $("#fec_fin").datepicker().datepicker("setStartDate", primerDiaMes);
        }

        $('.selectpicker').selectpicker({
            language: 'es',
            title: 'MOSTRAR TODAS'
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

    reporte.consultarUDistribuidor = function () {
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
            reporte.webControls.selNoTienda.empty();
            var html = ' <option value=\"0">MOSTRAR TODAS</option>'
            reporte.webControls.selNoTienda.append(html);
            $.each(array, function (index, row) {
                var html = ' <option value=\"' + row.id + '">' + row.descripcion + '</option>'
                reporte.webControls.selNoTienda.append(html);
            });
        }).fail(function (data, textStatus, xhr) {
            console.log(data, textStatus, xhr);
        });
    };

    reporte.maysPrimera = function (texto) {
        return texto.charAt(0).toUpperCase() + texto.slice(1).toLowerCase()
    };

    reporte.formatoCantidad = function (number, fraction) {     
        return '$' + number.toFixed(2).replace(/\d(?=(\d{3})+\.)/g, '$&,');
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
            var dis_id = $('#selNoTienda').val();
            $.getJSON(this.urlAPI.url_reporte_actividad,
                {
                    fecha_inicial: $('#fec_ini').val(),
                    fecha_final: $('#fec_fin').val(),
                    distribuidor_id: dis_id === "" ? 0 : dis_id,                    
                },
                function (json) {
                    var array = JSON.parse(json);
                    if (array.length > 0) {
                        $('#pNotFound').attr('style', 'display:none');
                        const types = Object.keys(array[0]);
                        var columnDefault = [];
                        $.each(types, function (index, name) {                            
                            if (index === 2) {
                                columnDefault.push({ visible: false, targets: 0 });
                            }
                            if (name.toLowerCase() === "importe ventas total" || name.toLowerCase() === "importe ventas loyalty") {
                                const add = {
                                    sTitle: name,
                                    mData: name,                                    
                                    mRender: function (data, type, row) {
                                        return reporte.formatoCantidad(data, 2);
                                    }
                                }
                                rcolumns.push(add);
                            }
                            else if (name.toLowerCase() === "socias loyalty" || name.toLowerCase() === "socias nuevas" || name.toLowerCase() === "Socias Registradas" || name.toLowerCase() === "tickets ventas total" || name.toLowerCase() === "tickets ventas loyalty")
                            {
                                const add = {
                                    sTitle: name,
                                    mData: name,
                                    mRender: function (data, type, row) {
                                        return data.toString().replace(/(\d)(?=(\d{3})+(?!\d))/g, "$1,");
                                    }
                                }
                                rcolumns.push(add);
                            }
                            else
                            {
                                const add = { sTitle: name, mData: name }
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
                        buttons: [
                            {
                                extend: 'excel',
                                text: 'Exportar a Excel',
                                className: 'btn btn-primary',
                                title: null,
                                exportOptions: {
                                    columns: ':visible'
                                }
                            }
                        ]
                    });
                    reporte.webControls.table_reporte_dinamico.DataTable(dataResult[0]);
                    setTimeout($.unblockUI, 2000); // Después desbloquea página
                });
        }
    };
    reporte.iniciar();
});