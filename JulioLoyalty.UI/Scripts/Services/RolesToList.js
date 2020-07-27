
var global = {};
var objectRow = null;
$(function (e) {
    var main = {
        url_rol: appBaseUrl + 'api/Service/GetRoles',
        URL_UPDATE_ROL: appBaseUrl + 'api/update/EditRoles',
        URL_CREATE_ROL: appBaseUrl + 'api/Update/AddRol',

        tableRoles: $('#tableRoles'),
        txtPageURL: $('#txtPageURL'),
        txtDescription: $('#txtDescription'),
        btnUpdate: $('#btnUpdate'),

        txtaddnombre: $('#txtNombre_nuevo'),
        txtaddPageURL: $('#txtPageURL_nuevo'),
        txtaddDescription: $('#txtDescription_nuevo'),
        btnAdd: $('#btnAdd'),


        init: function () {
            this.LoadRoles();
            main.tableRoles.on('click', 'tr', function () {
                objectRow = main.tableRoles.DataTable().row(this).data();
                main.txtDescription.val(objectRow.Description);
                main.txtPageURL.val(objectRow.InitialPageUrl);
            });

            main.btnUpdate.click(function () {
                main.Edit();
            });
            main.btnAdd.click(function () {
                main.Add();
            });
        },
        LoadRoles: function () {
            main.tableRoles.DataTable().destroy();
            main.tableRoles.DataTable({
                ajax: {
                    url: this.url_rol
                },
                language: {
                    url: '//cdn.datatables.net/plug-ins/1.10.15/i18n/Spanish.json'
                },
                lengthChange: false,
                processing: true,
                rowId: 'Id',
                scrollY: '35vh',
                scrollCollapse: true,
                //paging: false,
                //searching: false,
                ordering: false,
                columns: [
                    { data: "Name", },
                    { data: "Description" },
                    { data: "InitialPageUrl" },
                    {
                        data: "Id",
                        render: function (data, type, row) {
                            var btn = '<div style="text-align:center;"> <button class="btn btn-info btn-sm" data-title="Edit" data-toggle="modal" data-target="#edit" ><i class="far fa-edit"></i></button></div>';
                            return btn;
                        }
                    }
                ]

            });
        },
        Edit: function () {

            var obj = objectRow;
            obj.Description = main.txtDescription.val();
            obj.InitialPageUrl = main.txtPageURL.val();

            $.ajax({
                url: this.URL_UPDATE_ROL,
                type: 'PUT',
                async: false,
                data: obj,
            }).done(function (result, textStatus, xhr) {
                if (result.Success === true) {
                    swal({
                        title: "Atención",
                        text: 'Registro actualizado correctamente',
                        icon: "success",
                        button: "Aceptar",
                        closeOnClickOutside: false
                    });
                } else {
                    swal({
                        title: "Atención",
                        text: result.Message,
                        icon: "warning",
                        button: "Aceptar",
                        closeOnClickOutside: false
                    });
                }
            }).fail(function (data, textStatus, xhr) {
                console.log(data, textStatus, xhr);
            });

        },
        Add: function () {

            var rol = {
                Name: main.txtaddnombre.val(),
                Description: main.txtaddDescription.val(),
                InitialPageUrl: main.txtaddPageURL.val(),
            }

            $.ajax({
                url: main.URL_CREATE_ROL,
                type: 'PUT',
                async: false,
                data: rol,
            }).done(function (result, textStatus, xhr) {
                console.log(result);
                if (result.Success == true) {
                    main.LoadRoles();
                    main.txtaddnombre.val('');
                    main.txtaddDescription.val('');
                    main.txtaddPageURL.val('');
                    swal({
                        title: "Atención",
                        text: 'Registro agregado correctamente',
                        icon: "success",
                        button: "Aceptar",
                        closeOnClickOutside: false
                    });
                }
                else {
                    swal({
                        title: "Atención",
                        text: result.Message,
                        icon: "warning",
                        button: "Aceptar",
                        closeOnClickOutside: false
                    });
                }

            }).fail(function (data, textStatus, xhr) {
                console.log(data, textStatus, xhr);
            });

        },
        printMenssage(data) {
            //if (data.Success === true) {
            //    $(".close").click();
            //    $.notify({
            //        icon: 'glyphicon glyphicon-ok',
            //        message: data.Message,
            //    });
            //} else {
            //    $.notify({
            //        icon: 'glyphicon glyphicon-remove',
            //        message: data.Message,
            //    });
            //}
        },

    };

    global.main = main;
    main.init();

});