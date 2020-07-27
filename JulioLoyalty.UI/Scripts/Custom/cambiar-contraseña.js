var row = null;
$(function (e) {
    var main = {
        url_getusers: appBaseUrl + 'api/service/getsocias',
        table_users: $('#tableUsers'),
        init: function () {
            this.table_users.DataTable();
            this.LoadUsers("");

            this.table_users.on('search.dt', function () {
                var value = $('.dataTables_filter input').val();
                if (value.length > 14 && value.length < 17) {
                    console.log(value);
                }   
                $('.dataTables_filter input').unbind().change(function (e) {
                    var value = $(this).val();
                    if (value !== "" || value !== undefined) {
                        main.LoadUsers(value);
                    }
                });
            });

            this.table_users.on('click', 'tr', function (event) {
                row = main.table_users.DataTable().row(this).data();
                var event_tarjet = $(event.target).attr('class');

                if (event_tarjet === 'far fa-edit' || event_tarjet === 'btn btn-info btn-sm') {
                    $('#UserName').val(row.UserName);
                    $('#Password').val("");
                    $('#ConfirmPassword').val("");
                    $('span[data-valmsg-for="OK"]').text("");
                }
            });
        },
        LoadUsers: function (card) {
            main.table_users.DataTable().destroy();
            main.table_users.DataTable({
                language: {
                    url: '//cdn.datatables.net/plug-ins/1.10.15/i18n/Spanish.json'
                },
                ajax: {
                    url: main.url_getusers,
                    data: function (d) {
                        d.tarjeta = card;
                    }
                },
                lengthChange: false,
                rowId: 'Id',
                scrollY: '45vh',
                scrollCollapse: true,
                paging: true,
                searching: true,
                ordering: false,
                columns: [
                    { data: "FirstName" },
                    { data: "MiddleName" },
                    { data: "LastName" },
                    { data: "Email" },
                    {
                        data: "Roles",
                        render: function (data, type, row) {
                            return main.Formatter(row.Roles);
                        }
                    },
                    { data: "UserName" },
                    {
                        data: "Id",
                        width: "70px",
                        render: function (data, type, row) {
                            var btn = '<div style="text-align:center;"> <button class="btn btn-info btn-sm" title="Editar usuario" data-toggle="modal" data-target="#edit" ><i class="far fa-edit"></i></button>';
                            return btn;
                        }
                    }
                ]

            });
        },
        Formatter: function (data) {
            if (data.length === 0)
                return '';

            var result = '<ul>';
            $.each(data, function (index, row) {
                result += '<li>' + row.Name + '</li>';
            });
            result += '</ul>';
            return result;
        },
    };
    main.init();
});
