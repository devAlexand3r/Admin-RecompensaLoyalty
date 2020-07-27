$(function (e) {
    var reporte = new Object();
    reporte.urlAPI = {
        url_consulta_usuario_distribuidor: appBaseUrl + '/api/Customer/Usuario/Distribuidor',
        url_obtener_status_participante: appBaseUrl + '/api/Report/cargaStatusParticipante',
        url_obtener_status_seguimiento: appBaseUrl + '/api/Report/cargaStatusSeguimiento',
        url_reporte_llamadas: appBaseUrl + '/api/Report/llamadas',
        url_consultar_seguimiento: appBaseUrl + '/Llamada/Seguimiento',
        url_consultar_historico: appBaseUrl + '/Llamada/Historico'
    };

    reporte.webControls = {
        fec_ini: $('#fec_ini'),
        fec_fin: $('#fec_fin'),
        selNoTienda: $('#selNoTienda'),
        selStatusParticipante: $('#selStatusParticipante'),
        selStatusSeguimiento: $('#selStatusSeguimiento'),
        btnBuscar: $('#btnBuscar'),
        btnExportar: $('#btnExportar'),
        table_reporte_dinamico: $('#tabla_reporte_dinamico')
    };

    reporte.valido = {
        fecha: false
    };

    reporte.iniciar = function () {
        this.agregarEventos();
        this.consultarUDistribuidor();
        this.obtenerStatusParticipante();
        this.obtenerStatusSeguimiento();
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

    reporte.formatoMembresia = function (number) {
        return '\u200C' + number; // Corrige el problema al exportar a excel con números de 16 digitos
    };

    reporte.obtenerStatusParticipante = function () {
        $.ajax({
            url: this.urlAPI.url_obtener_status_participante,
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
            reporte.webControls.selStatusParticipante.empty();
            var html = ' <option value=\"0">MOSTRAR TODOS</option>'
            reporte.webControls.selStatusParticipante.append(html);
            $.each(array, function (index, row) {
                var html = ' <option value=\"' + row.id + '">' + row.descripcion + '</option>'
                reporte.webControls.selStatusParticipante.append(html);
            });
        }).fail(function (data, textStatus, xhr) {
            console.log(data, textStatus, xhr);
        });
    };

    reporte.obtenerStatusSeguimiento = function () {
        $.ajax({
            url: this.urlAPI.url_obtener_status_seguimiento,
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
            reporte.webControls.selStatusSeguimiento.empty();
            var html = ' <option value=\"0">MOSTRAR TODOS</option>'
            reporte.webControls.selStatusSeguimiento.append(html);
            $.each(array, function (index, row) {
                var html = ' <option value=\"' + row.id + '">' + row.descripcion + '</option>'
                reporte.webControls.selStatusSeguimiento.append(html);
            });
        }).fail(function (data, textStatus, xhr) {
            console.log(data, textStatus, xhr);
        });
    };

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
            var sta_part_id = $('#selStatusParticipante').val();
            var sta_segu_id = $('#selStatusSeguimiento').val();
            $.getJSON(this.urlAPI.url_reporte_llamadas,
                {
                    fecha_inicial: $('#fec_ini').val(),
                    fecha_final: $('#fec_fin').val(),
                    distribuidor_id: dis_id === "" ? 0 : dis_id,
                    status_participante_id: sta_part_id === "" ? 0 : sta_part_id,
                    status_seguimiento_id: sta_segu_id === "" ? 0 : sta_segu_id,
                },
                function (json) {
                    var array = JSON.parse(json);
                    if (array.length > 0) {
                        $('#pNotFound').attr('style', 'display:none');
                        const types = Object.keys(array[0]);
                        var columnDefault = [];
                        $.each(types, function (index, name) {
                            if (index === 2)
                                columnDefault.push({ visible: false, targets: 0 });
                            if (name.toLowerCase() === "membresía") {
                                const add = {
                                    sTitle: name,
                                    mData: name,
                                    mRender: function (data, type, row) {
                                        return reporte.formatoMembresia(data);
                                    }
                                }
                                rcolumns.push(add);
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
                        text: 'Exportar a Excel',
                        className: 'btn btn-primary',
                        title: "Reporte_Llamadas",
                        exportOptions: {
                            columns: ':visible'
                        }
                    };
                    buttonExport.push(conf);
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
                        buttons: buttonExport
                    });
                    reporte.webControls.table_reporte_dinamico.DataTable(dataResult[0]);
                    // Al hacer clic dependiendo del status manda hacer el redirect
                    reporte.webControls.table_reporte_dinamico.on('click', 'tr', function (event) {
                        var objectRow = reporte.webControls.table_reporte_dinamico.DataTable().row(this).data();
                        swal({
                            title: "¿Estás seguro de eso?",
                            text: "¡Una vez realizado el cambio de socia, deberás volver a realizar la búsqueda!",
                            icon: "warning",
                            buttons: [
                              'Cancelar',
                              'Aceptar'
                            ],
                            dangerMode: true,
                        }).then(function (isConfirm) {
                            if (isConfirm) {
                                reporte.cambiarCliente(objectRow);
                            }
                        });

                        if ($(this).hasClass('row-selected')) {
                            $(this).removeClass('row-selected');
                        }
                        else {
                            reporte.webControls.table_reporte_dinamico.DataTable().$('tr.row-selected').removeClass('row-selected');
                            $(this).addClass('row-selected');
                        }
                    });
                    setTimeout($.unblockUI, 2000); // Después desbloquea página                    
                });
        }
    };

    reporte.cambiarCliente = function (row) {
        var status_seguimiento = row.Status;
        var membresia = row.Membresía;
        var nombre = row.Nombre;

        if (row.nombre === null) {
            localStorage.setItem('nombre', " ");
        } else {
            $('#spanCliente').text(nombre);
        }

        $.ajax({
            url: appBaseUrl + '/api/Customer/Find',
            type: 'GET',
            data: { parameter: membresia.trim() },
            async: false,
            beforeSend: function () {

            },
            success: function (data, status, xhr) {
                var cliente = JSON.parse(data);
                if (cliente !== "" && cliente !== null) {
                    localStorage.setItem('nombre', nombre);
                    localStorage.setItem('objUser', JSON.stringify(cliente[0]));
                    if (status_seguimiento.trim().toUpperCase() === "ABIERTO" ||
                        status_seguimiento.trim().toUpperCase() === "EN PROCESO" ||
                        status_seguimiento.trim().toUpperCase() === "ABIERTO JULIO LOYALTY" ||
                        status_seguimiento.trim().toUpperCase() === "ABIERTO LMS" ||
                        status_seguimiento.trim().toUpperCase() === "EN PROCESO JULIO LOYALTY" ||
                        status_seguimiento.trim().toUpperCase() === "EN PROCESO LMS") {
                        localStorage.setItem('name', 'menu_4');
                        localStorage.setItem('menu', 'submenu_13');
                        localStorage.setItem('href', reporte.urlAPI.url_consultar_seguimiento);
                        window.location.href = reporte.urlAPI.url_consultar_seguimiento;
                    }
                    else if (status_seguimiento.trim().toUpperCase() === "CERRADO" ||
                             status_seguimiento.trim().toUpperCase() === "CERRADO SATISFACTORIO" ||
                             status_seguimiento.trim().toUpperCase() === "CERRADO NO SATISFACTORIO") {
                        localStorage.setItem('name', 'menu_4');
                        localStorage.setItem('menu', 'submenu_14');
                        localStorage.setItem('href', reporte.urlAPI.url_consultar_historico);
                        window.location.href = reporte.urlAPI.url_consultar_historico;
                    }
                }
            },
            error: function (data, status, xhr) {
                console.log(data);
            }
        });
    };
    reporte.iniciar();
});