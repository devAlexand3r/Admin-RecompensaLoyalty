
$(function (e) {
    var main = {
        urls: {
            report_dynamic: appBaseUrl + "/api/data/report/dynamic"
        },
        webControl: {
            btnSearch: $("#btn-search"),
            btnExport: $("#btnExportar"),
            inpDateStart: $("#dateStart"),
            inpDateEnd: $("#dateEnd"),
            selReport: $("#selectReport"),
            tableDynamic: $("#tabla_reporte_dinamico")
        },
        init: function () {
            var date = new Date();
            var dateSta = "01/" + (date.getMonth() + 1) + "/" + date.getFullYear();
            var dateEnd = date.getDate() + "/" + (date.getMonth() + 1) + "/" + date.getFullYear();

            var options = {
                url: this.urls.report_dynamic,
                data: {
                    id: 2,
                    dStart: dateSta,
                    dEnd: dateEnd
                },
                container: $("#divTable")
            }
            this.ajaxRequest(options);

            this.addEvents();
        },
        addEvents: function () {
            this.webControl.btnSearch.click(function () {
                var options = {
                    url: main.urls.report_dynamic,
                    data: {
                        id: main.webControl.selReport.val(),
                        dStart: main.webControl.inpDateStart.val(),
                        dEnd: main.webControl.inpDateEnd.val()
                    },
                    container: $("#divTable")
                }
                main.ajaxRequest(options);
            });
        },
        reportCallBack: function (data, status, xhr) {
            main.webControl.tableDynamic.DataTableDynamic({
                aaData: data,
                aoColumnDefs: [
                    { visible: false, targets: 0 }
                ],
                info: false,
                rowId: 'id',
                buttons: [{
                    extend: 'excel',
                    footer: true,
                    text: '<li class="fa fa-file-excel-o"></li> Exportar a Excel',
                    className: 'btn btn-success',
                    title: "Reporte",
                    exportOptions: {
                        columns: ':visible'
                    }
                }],
                showTotal: true
            });
        },
        ajaxRequest: function (opt) {
            $.ajax({
                url: opt.url,
                type: "GET",
                async: true,
                data: opt.data,
                beforeSend: function () {
                    opt.container.LoadingOverlay("show", {
                        zIndex: 100,
                        image: "",
                        text: "Loading...",
                        maxSize: 30,
                        minSize: 30,
                        textColor: "#6DB093"
                    });
                },
            }).done(main.reportCallBack).always(function (data, status, xhr) {
                opt.container.LoadingOverlay("hide");
            }).fail(main.reportError);
        },
        reportError: function (data, status, xhr) {
            console.log(data);
        },
        toUpperCaseFirst: function (text) {
            return text.charAt(0).toUpperCase() + text.slice(1).toLowerCase()
        },
        formatCard: function (number) {
            return '\u200C' + number;
        }
    }
    main.init();
});
