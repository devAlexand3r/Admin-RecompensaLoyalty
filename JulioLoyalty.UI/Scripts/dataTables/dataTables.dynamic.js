
(function ($) {
    $.fn.DataTableDynamic = function (options) {
        // Default options
        var settings = $.extend({
            aaData: [],
            aoColumns: [],
            aoColumnDefs: [],
            oLanguage: {
                sUrl: '//cdn.datatables.net/plug-ins/1.10.15/i18n/Spanish.json'
            },
            name: 'TableDynamic',
            rowId: 'id',
            destroy: true,
            paging: true,
            lengthChange: false,
            searching: true,
            ordering: true,
            info: true,
            autoWidth: false,
            showTotal: false,
            buttons: [],
            dom: 'Bfrtip',
            footerCallback: function (row, data, start, end, display) {
                this.api().columns('.sum').every(function () {
                    var column = this;
                    var sum = column.data().reduce(function (a, b) {
                        a = parseInt(a, 10);
                        if (isNaN(a)) { a = 0; }

                        b = parseInt(b, 10);
                        if (isNaN(b)) { b = 0; }
                        return a + b;
                    });
                    $(column.footer()).html(sum);
                });
            },
            drawCallback: function () {
            },
        }, options);

        // destroy if exist datatables
        if ($.fn.DataTable.isDataTable(this)) {
            this.DataTable().clear().destroy();
            this.empty();
        }

        // set columns
        if (settings.aoColumns.length === 0) {
            $.each(settings.aaData[0], function (key, value) {
                settings.aoColumns.push({
                    sTitle: key,
                    mData: key,
                    sClass: (isNaN(value) === true || settings.showTotal === false) ? "" : "sum"
                });
            });
        }

        if (settings.showTotal === true) {
            var hfooter = "<tfoot><tr style='font-weight: bold;'>";
            $.each(settings.aoColumns, function (index, row) {
                hfooter += "<td>TOTALES</td>"
            });
            hfooter += "</tr></tfoot>";
            this.empty().append(hfooter);
        }

        return this.DataTable(settings);
    }
}(jQuery));
