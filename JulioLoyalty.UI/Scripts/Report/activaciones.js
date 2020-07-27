$(function (e) {
    var reporte = new Object();
    reporte.urlAPI = {
        url_consulta_usuario_distribuidor: appBaseUrl + '/api/Customer/Usuario/Distribuidor',
        url_obtener_catalogo: appBaseUrl + '/api/MCatalogo/FKCatalogos',
        url_reporte_participante: appBaseUrl + '/api/Report/participantes'
    };

    reporte.webControls = {
        fec_ini: $('#fec_ini'),
        fec_fin: $('#fec_fin'),
        selNoTienda: $('#selNoTienda'),
        selStatus: $('#selStatus'),
        btnBuscar: $('#btnBuscar'),
        btnExportar: $('#btnExportar'),
        table_reporte_dinamico: $('#tabla_reporte_dinamico')
    };

    reporte.selValues = {
        rolTienda: $('#hidRole').val(),
        username: $('#hidUserName').val()
    },

        reporte.valido = {
            fecha: false
        }

    reporte.iniciar = function () {
        this.agregarEventos();
        this.consultarUDistribuidor();
        this.obtenerStatusParticipante();
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

        if (reporte.selValues.rolTienda === "9699e07c-4546-46eb-8456-4a5777c1fb77") { // Para el Rol Tienda solo permite consultar el mes actual
            $("#fec_ini").datepicker().datepicker("setStartDate", primerDiaMes);
            $("#fec_fin").datepicker().datepicker("setStartDate", primerDiaMes);
        }

        var selRolGerente = $('#hidRoleGerente').val();
        if (selRolGerente === "rol-gerente") {
            primerDiaMes.setMonth(primerDiaMes.getMonth() - parseInt(1));
            $("#fec_ini").datepicker().datepicker("setStartDate", primerDiaMes);
            $("#fec_fin").datepicker().datepicker("setStartDate", primerDiaMes);
        }

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
    }

    reporte.obtenerStatusParticipante = function () {
        $.ajax({
            url: this.urlAPI.url_obtener_catalogo,
            type: 'GET',
            data: { descripcion: 'status_participante' },
            async: false
        }).done(function (data, textStatus, xhr) {
            var array = [];
            $.each(JSON.parse(data), function (index, data) {
                var add = {
                    id: data.id,
                    descripcion: data.descripcion
                };
                if (reporte.selValues.rolTienda === "9699e07c-4546-46eb-8456-4a5777c1fb77") {
                    if (data.id === 3) { // Si el rol es Tienda, solo va cargar el Status Bloqueada por Falta de Datos
                        array.push(add);
                        return false;
                    }
                } else {
                    var selRolGerente = $('#hidRoleGerente').val();
                    if (selRolGerente === "rol-gerente") {
                        // RolId de Gerente
                        if (data.id === 3 || data.id === 5 || data.id === 7) { // Va a cargar los Status BLOQUEADA POR FALTA DE DATOS, BAJA POR FALTA DE DATOS y BAJA  PORQUE TIENDA PERDIÓ FORMULARIO
                            array.push(add);
                        }
                    } else {
                        if (reporte.selValues.username === "beatriz_caballero") {
                            if (data.id === 3 || data.id === 5) { // Va a cargar los Status BLOQUEADA POR FALTA DE DATOS, BAJA POR FALTA DE DATOS
                                array.push(add);
                            }
                        }
                        else
                            array.push(add);
                    }
                }
            });
            reporte.webControls.selStatus.empty();
            var html = ' <option value=\"0">MOSTRAR TODOS</option>'
            reporte.webControls.selStatus.append(html);
            $.each(array, function (index, row) {
                var html = ' <option value=\"' + row.id + '">' + row.descripcion + '</option>'
                reporte.webControls.selStatus.append(html);
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
            var sta_id = $('#selStatus').val();
            $.getJSON(this.urlAPI.url_reporte_participante,
                {
                    fecha_inicial: $('#fec_ini').val(),
                    fecha_final: $('#fec_fin').val(),
                    distribuidor_id: dis_id === "" ? 0 : dis_id,
                    status_participante_id: sta_id === "" ? 0 : sta_id,
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
                        title: "Reporte_Activaciones",
                        exportOptions: {
                            columns: ':visible'
                        }
                    };
                    if (reporte.selValues.rolTienda !== "9699e07c-4546-46eb-8456-4a5777c1fb77") { // Sino es Rol Tienda Muestra el Botón Exportar Excel
                        buttonExport.push(conf);
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
                        buttons: buttonExport
                    });
                    reporte.webControls.table_reporte_dinamico.DataTable(dataResult[0]);
                    setTimeout($.unblockUI, 2000); // Después desbloquea página                    
                });
        }
    };
    reporte.iniciar();
});