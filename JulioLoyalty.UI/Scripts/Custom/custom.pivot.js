/**
 * @author Alejandro Jiménez <devAlexander@gmail.com>
 */
$(function (e) {
    var cube = {
        appBaseUrl: undefined,
        // Extract "GET" parameters from a JS include querystring
        getParams: function (a) {
            var b = document.getElementsByTagName("script");
            for (var i = 0; i < b.length; i++) {
                if (b[i].src.indexOf("/" + a) > -1) {
                    var c = b[i].src.split("?").pop().split("&");
                    var p = {};
                    for (var j = 0; j < c.length; j++) {
                        var d = c[j].split("=");
                        p[d[0]] = d[1];
                    }
                    return p;
                }
            }
            return {};
        },
        // initialize
        init: function () {
            if (/^(undefined)$/.test(this.appBaseUrl)) {
                this.appBaseUrl = window.location.origin;
            }

            this.loadCubes(this.getParams("custom.pivot.js"));
        },
        // Load Cubes
        loadCubes: function (params) {
            var tpl = $.pivotUtilities.aggregatorTemplates;
            var numberFormat = $.pivotUtilities.numberFormat;
            var intFormat = numberFormat({ digitsAfterDecimal: 0 });
            var usdFormat = numberFormat({ prefix: "$", digitsAfterDecimal: 2 });

            if (params.hasOwnProperty("cube")) {
                const type = params.cube;

                // Generar cubo de participantes
                if (/^(part|participante)$/.test(type)) {
                    const _part = {
                        fileName: "Activación semanal",
                        container: $('#divCube'),
                        mps: null,
                        url: this.appBaseUrl + "/report/everyweek",
                        data: {},
                        cols: ["Semana Activación", "Año Activación"],
                        rows: ["Sucursal"],
                        aggregators: {
                            "Socias": function () { return tpl.sum(intFormat)(["Socias"]); }
                        },
                        hiddenAttributes: ["Socias"]
                    };
                    this.ajaxRequest(_part);
                }

                // Generar cubo socias compra
                if (/^(buy|compras)$/.test(type)) {
                    var date = new Date();
                    var dateSta = "01/" + (date.getMonth() + 1) + "/" + date.getFullYear();
                    var dateEnd = date.getDate() + "/" + (date.getMonth() + 1) + "/" + date.getFullYear();

                    const _buy = {
                        fileName: "Cubo socias compra",
                        container: $('#divCube'),
                        mps: null,
                        url: this.appBaseUrl + "/report/buydata",
                        data: {
                            dateStart: dateSta,
                            dateEnd: dateEnd
                        },
                        cols: ["Nivel"],
                        rows: ["Sucursal"],
                        aggregators: {
                            "Socias": function () { return tpl.sum(intFormat)(["Socias"]); },
                            "Visitas": function () { return tpl.sum(intFormat)(["Visitas"]); },
                            "Tickets": function () { return tpl.sum(intFormat)(["Tickets"]); },
                            "Monto Tickets": function () { return tpl.sum(usdFormat)(["Monto Ticket"]); },
                            "Ticket Promedio": function () { return tpl.average(usdFormat)(["Ticket Promedio"]); },
                            "Visitas Promedio": function () { return tpl.average(intFormat)(["Visitas Promedio"]); }
                        },
                        hiddenAttributes: ["Socias", "Visitas", "Tickets", "Monto Ticket", "Ticket Promedio", "Visitas Promedio", "Socias Nuevas", "Socias Recurrentes"]
                    };
                    this.ajaxRequest(_buy);

                    var btnBuscar = $("#btnBuscar");
                    btnBuscar.click(function () {
                        _buy.data.dateStart = $('#dateStart').val().toString();
                        _buy.data.dateEnd = $('#dateEnd').val().toString();
                        if (_buy.data.dateStart.length === 10 && _buy.data.dateEnd.length === 10) {
                            cube.ajaxRequest(_buy);
                        }
                    });
                }

                // Generar cubo de participantes (Activaciones)
                if (/^(act|activation)$/.test(type)) {
                    const _part = {
                        fileName: "Cubo socias",
                        container: $('#divCube'),
                        mps: null,
                        url: this.appBaseUrl + "/report/activationData",
                        data: {},
                        cols: ["Semana Activación", "Año Activación"],
                        rows: ["Sucursal"],
                        aggregators: {
                            "Socias": function () { return tpl.sum(intFormat)(["Socias"]); }
                        },
                        hiddenAttributes: ["Socias"]
                    };
                    this.ajaxRequest(_part);
                }

            }
        },
        // All request
        ajaxRequest: function (e) {
            $.ajax({
                url: e.url,
                type: "GET",
                async: true,
                data: e.data,
                beforeSend: function () {
                    e.container.loading({
                        theme: 'light'
                    });
                }
            }).done(function (data, status, xhr) {
                e.mps = data;
                cube.setPivotTable(e);
            }).always(function (data, status, xhr) {
                e.container.loading('stop');
            }).fail(function (data, status, xhr) {
                console.log(data, status, xhr);
            });
        },
        // Set Pivottable Cube
        setPivotTable: function (e) {
            if (e.mps.length === 0)
                return;

            const dataClass = $.pivotUtilities.SubtotalPivotData;
            const renderers = $.pivotUtilities.subtotal_renderers;

            e.container.pivotUI(e.mps, {
                rowOrder: "key_a_to_z",
                colOrder: "key_a_to_z",
                exclusions: {
                    //Status: ["BAJA POR UNIFICACIÓN"]
                },
                inclusions: {
                    //Status: ["BAJA POR UNIFICACIÓN"]
                },
                cols: e.cols,
                rows: e.rows,
                rendererName: "Table With Subtotal",
                dataClass: dataClass,
                aggregators: e.aggregators,
                //renderers: renderers,
                derivedAttributes: {
                },
                rendererOptions: {
                    table: {
                        clickCallback: function (e, value, filters, pivotData) {
                            var names = [];
                            pivotData.forEachMatchingRecord(filters, function (record) {
                                names.push(record.Name);
                            });
                        }
                    },
                    c3: {
                        size: { height: 200, width: 200 }
                    },
                    localeStrings: {
                        renderError: "Ocurri&oacute; un error durante la interpretaci&oacute;n de la tabla din&acute;mica.",
                        computeError: "Ocurri&oacute; un error durante el c&acute;lculo de la tabla din&acute;mica.",
                        uiRenderError: "Ocurri&oacute; un error durante el dibujado de la tabla din&acute;mica.",
                        selectAll: "Seleccionar todo",
                        selectNone: "Deseleccionar todo",
                        tooMany: "(demasiados valores)",
                        filterResults: "Filtrar resultados",
                        totals: "TOTALES",
                        vs: "vs",
                        by: "por"
                    },
                    collapseRowsAt: 1,
                    collapseColsAt: 1,
                    rowSubtotalDisplay: {
                        disableAfter: 0
                    },
                    colSubtotalDisplay: {
                        disableAfter: 0
                    }
                },
                onRefresh: function (config) {
                    $('.pvtAggregator').addClass('form-control select2 selectWidth');

                    //style="caption-side: bottom;
                    $('table.pvtTable').first().prepend('<caption style="caption-side: top;"> <input type="button" class="ExportToExcel btn btn-primary" value="Exportar a Excel" /></caption>');

                    var today = ' ' + cube.getToday();
                    var fileName = e.fileName;
                    $('.ExportToExcel').off('click').click(function (e) {

                        if (fileName === undefined)
                            fileName = "Cubo dinámico";

                        fileName += today;
                        tableToExcel('.pvtTable', fileName);
                    });
                },
                hiddenAttributes: e.hiddenAttributes
            }, true);

        },

        getToday: function () {
            var today = new Date();
            var dd = today.getDate();
            var mm = today.getMonth() + 1; //January is 0!
            var yyyy = today.getFullYear();

            if (dd < 10) {
                dd = '0' + dd;
            }

            if (mm < 10) {
                mm = '0' + mm;
            }

            today = dd + '' + mm + '' + yyyy;
            return today;
        }
    };
    cube.init();

    var tableToExcel = (function () {
        var uri = 'data:application/vnd.ms-excel;base64,'
            , template = '<html xmlns:o="urn:schemas-microsoft-com:office:office" xmlns:x="urn:schemas-microsoft-com:office:excel" xmlns="http://www.w3.org/TR/REC-html40"><head><!--[if gte mso 9]><xml><x:ExcelWorkbook><x:ExcelWorksheets><x:ExcelWorksheet><x:Name>{worksheet}</x:Name><x:WorksheetOptions><x:DisplayGridlines/></x:WorksheetOptions></x:ExcelWorksheet></x:ExcelWorksheets></x:ExcelWorkbook></xml><![endif]--><meta http-equiv="content-type" content="text/plain; charset=UTF-8"/></head><body><table>{table}</table></body></html>'
            , base64 = function (s) { return window.btoa(unescape(encodeURIComponent(s))); }
            , format = function (s, c) { return s.replace(/{(\w+)}/g, function (m, p) { return c[p]; }); };

        return function (table, name) {
            if (!table.nodeType)
                table = $(table);

            var ctx = {
                worksheet: name || 'Worksheet',
                table: table.html()
            };

            var browser = window.navigator.appVersion;
            //Workaround to enable the users to download the report in IE.
            if ((browser.indexOf('Trident') !== -1 && browser.indexOf('rv:11') !== -1) || (browser.indexOf('MSIE 10') !== -1)) {
                var builder = new window.MSBlobBuilder();
                builder.append(uri + format(template, ctx));
                var blobie = builder.getBlob('data:application/vnd.ms-excel');
                window.navigator.msSaveOrOpenBlob(blobie, name + '.xls');
            } else {
                //var link = document.createElement("a");
                //link.download = name + ".xls";
                //link.href = uri + base64(format(template, ctx));
                //link.click();
                var blob = new Blob([format(template, ctx)], { type: 'application/vnd.ms-excel', endings: 'native' });
                var elem = window.document.createElement('a');
                elem.href = window.URL.createObjectURL(blob);
                elem.download = name + '.xls';
                document.body.appendChild(elem);
                elem.click();
                document.body.removeChild(elem);
            }
        };
    })();

}(jQuery));