
$(function (e) {
    "use strict";
    var ticket = {
        urlWepAPIs: {
            consultar_ticket: appBaseUrl + "api/Service/consultaTicket",
            cancelar_ticket: appBaseUrl + "api/Service/cancelTicket"
        },
        webControls: {
            txtTicket: $('#txtTicket')
        },
        agregarEventos: function () {
            $("#btnBuscar").click(function () {
                var selValue = ticket.webControls.txtTicket.val().trim();
                if (selValue !== "") {
                    ticket.buscarTickets(selValue);
                    localStorage.setItem('num_ticket', selValue);
                }
            });

            // Número de tickect, cancelTickect
            $(".btn-acept").click(function () {
                ticket.ajaxRequest(ticket.urlWepAPIs.cancelar_ticket, 'GET', { numTicket: localStorage.getItem('num_ticket') }, ticket.callbackCancelTicket);
            });

        },
        buscarTickets: function (txtTicket) {
            this.ajaxRequest(this.urlWepAPIs.consultar_ticket, 'GET', { numTicket: txtTicket }, this.resultTickect);
        },
        resultTickect: function (result) {
            var array = JSON.parse(result);
            $('#divContent').empty(); //Eliminar contenido del div
            $.each(array, function (index, array) {
                var html = '<div class="table-responsive" id="div' + index + '"><table id="table_' + index + '" class="table table - bordered table - sm table - hover w - 100"></table></div><br />';
                $('#divContent').append(html);
                if (array.length === 0) {
                    $('#div' + index).empty().text("No se encontraron resultados...");
                }
                ticket.crearTablaDinamicaUnica("#table_" + index, array);
                //ticket.crearTablaDinamica("#table_" + index, array);
            });
        },
        crearTablaDinamica: function (table_id, array) {
            var dataResult = [];
            var rcolumns = [];
            var columnDefault = [];
            if (array.length > 0) {
                const types = Object.keys(array[0]);
                $.each(types, function (index, name) {
                    const add = { sTitle: name.replace('_', ' '), mData: name }
                    rcolumns.push(add);
                });
                columnDefault.push({ visible: false, targets: 0 }); // Ocultamos la primera columna
            } else {
                if ($.fn.DataTable.isDataTable(table_id)) {
                    $(table_id).DataTable().clear().destroy();
                    $(table_id).empty();
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
                paging: false,
                searching: false,
                ordering: true,
                info: false,
            });
            $(table_id).DataTable(dataResult[0]);

            $(table_id).on('click', 'tr', function (event) { });
        },
        ajaxRequest: function (url, type, data, callBack) {
            $.ajax({
                url: url,
                type: type,
                async: false,
                data: data,
                beforeSend: function () {
                },
            }).done(callBack).fail(function (data, textStatus, xhr) {
                console.log(data, textStatus, xhr);
            });
        },
        iniciar: function () {
            this.agregarEventos();
        },


        callbackCancelTicket: function (callBack) {
            var result = JSON.parse(callBack);
            console.log(result);
            if (result.length > 0) {
                if (result[0][0].errorId === 0) {
                    swal({
                        title: 'Atención',
                        text: result[0][0].mensaje,
                        icon: "success",
                        button: "Aceptar",
                        closeOnClickOutside: false
                    });
                } else {
                    swal({
                        title: 'Advertencia',
                        text: result[0][0].mensaje,
                        icon: "warning",
                        button: "Aceptar",
                        closeOnClickOutside: false
                    });
                }
            }
        },
        crearTablaDinamicaUnica: function (table, array) {
            var dataResult = [];
            var rcolumns = [];
            var columnDefault = [];
            if (array.length > 0) {
                const types = Object.keys(array[0]);
                $.each(types, function (index, name) {
                    const add = { sTitle: name.replace('_', ' '), mData: name }
                    // Agregar boton de control
                    if (index === 0) {
                        var controles = {
                            mData: 'id',
                            sWidth: "20px",
                            mRender: function (data, type, row) {
                                return '<div style="text-align:center;"><button class="btn btn-danger btn-sm" title="Cancelar ticket!" data-toggle="modal" data-target="#modCancel"><i class="fas fa-trash"></i></button>';
                            }
                        };
                        rcolumns.push(controles);
                    } else {
                        rcolumns.push(add);
                    }
                });
                //columnDefault.push({ visible: false, targets: 0 }); // Ocultamos la primera columna
            } else {
                if ($.fn.DataTable.isDataTable(table)) {
                    $(table).DataTable().clear().destroy();
                    $(table).empty();
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
                paging: false,
                searching: false,
                ordering: true,
                info: false,
            });
            $(table).DataTable(dataResult[0]);

            $(table).on('click', 'tr', function (event) { });
        },



    };

    ticket.iniciar();

});