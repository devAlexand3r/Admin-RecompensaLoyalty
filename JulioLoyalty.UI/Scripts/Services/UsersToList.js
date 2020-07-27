

var global = {};
var objectRow = null;
$(function (e) {
    var main = {
        URL_GET_USERS: appBaseUrl + 'api/Service/GetUsers',
        URL_GET_MENUS: appBaseUrl + 'api/Service/GetMenu',
        URL_UPDATE_ACCESS: appBaseUrl + 'api/update/EditUserAccess',

        URL_GET_PERMITS: appBaseUrl + 'api/Service/GetSubMenusByUser',
        URL_UPDATE_USER: appBaseUrl + 'api/Customer/UpdateUser',

        url_obtener_distribuidores_usuario: appBaseUrl + 'api/Service/DistribuidorPorUsuario',
        url_actualizar_distribuidores_usuario: appBaseUrl + 'api/Update/Actualizar/Lista/Distribuidor',

        table_users: $('#tableUsers'),
        btnUpdate: $('#btnUpdateAllow'),
        selectList: $('#selectAccess'),

        firstName: $('#FirstName'),
        middleName: $('#MiddleName'),
        lastName: $('#LastName'),
        email: $('#Email'),
        btnUpdateUser: $('#btnUpdateUser'),
        
        btnUpdateDistribuidor: $('#btnUpdatedistribuidores'),

        init: function () {
            this.table_users.DataTable();
            this.LoadUsers();
            this.loadMenus();
            main.table_users.on('click', 'tr', function (event) {
                objectRow = main.table_users.DataTable().row(this).data();
                var event_tarjet = $(event.target).attr('class');

                if (event_tarjet === 'far fa-edit' || event_tarjet === 'btn btn-info btn-sm') {
                    main.firstName.val(objectRow.FirstName);
                    main.middleName.val(objectRow.MiddleName);
                    main.lastName.val(objectRow.LastName);
                    main.email.val(objectRow.Email);
                }

                if (event_tarjet === 'fas fa-cogs' || event_tarjet === 'btn btn-warning btn-sm') {
                    main.setByUser();
                    main.obtenerDistribuidores();
                }

            });

            //Update user, permisos
            main.btnUpdate.click(function () {
                main.EditAllow();
            });

            //Update user
            main.btnUpdateUser.click(function () {
                main.EditUser();
            });

            main.btnUpdateDistribuidor.click(function () {
                main.actualizarDistribuidor();
            });

            $.ajax({
                url: appBaseUrl + 'api/Service/ObtenerDistribuidores',
                type: 'GET',
                async: false
            }).done(function (result, textStatus, xhr) {
                $('#listadistribuidor').empty();
                $.each(result, function (index, row) {
                    var html = ' <option value=\"' + row.id + '">' + row.descripcion_larga + '</option>'
                    $('#listadistribuidor').append(html);
                });
                //$Utils.addSelectOptions(result, $('#listadistribuidor'), 'id', 'descripcion_larga');
                $('#listadistribuidor').selectpicker('refresh');
            }).fail(function (data, textStatus, xhr) {
                console.log(data, textStatus, xhr);
            });

        },
        LoadUsers: function () {
            main.table_users.DataTable().destroy();
            main.table_users.DataTable({
                language: {
                    url: '//cdn.datatables.net/plug-ins/1.10.15/i18n/Spanish.json'
                },
                ajax: {
                    url: main.URL_GET_USERS
                },
                lengthChange: false,
                rowId: 'Id',
                //scrollY: '35vh',
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
                            var btn = '<div style="text-align:center;"> <button class="btn btn-info btn-sm" title="Editar usuario" data-toggle="modal" data-target="#edit" ><i class="far fa-edit"></i></button>'
                                + '  <button class="btn btn-warning btn-sm" title="Configurar usuario" data-toggle="modal" data-target="#config" data-backdrop="static" data-keyboard="false" ><i class="fas fa-cogs"></i></button></div>';
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

        EditAllow: function () {
            $.ajax({
                url: main.URL_UPDATE_ACCESS,
                type: 'GET',
                data: {
                    key: objectRow.Id,
                    arrayKey: main.selectList.val().toString()
                },
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                beforeSend: function () {
                    main.btnUpdate.attr('disabled', true);
                }
            }).done(function (data, textStatus, xhr) {
                if (data.Success === true) {
                    swal({
                        title: "Actualización",
                        text: 'Lista de permisos, actualizado correctamente',
                        icon: "success",
                        button: "Aceptar",
                        closeOnClickOutside: false
                    });                       
                }
                if (data.Success === false) {
                    swal({
                        title: "Alerta",
                        text: data.Message,
                        icon: "warning",
                        button: "Aceptar",
                        closeOnClickOutside: false
                    }); 
                }       
                main.btnUpdate.attr('disabled', false);
            }).fail(function (data, textStatus, xhr) {
                console.log(data, textStatus, xhr);
                main.btnUpdate.attr('disabled', false);
            });

        },
        loadMenus: function () {
            $.ajax({
                url: main.URL_GET_MENUS,
                type: 'GET',
                async: false
            }).done(function (result, textStatus, xhr) {
                $.each(result.data, function (key, row) {
                    var group = "<optgroup label=\"" + row.Name + "\">";
                    $.each(row.SubMenu, function (key, row) {
                        group += "<option value=" + row.Id + ">" + row.Name + "</option>";
                    });
                    group += "</optgroup>";
                    main.selectList.append(group);
                });
                main.selectList.selectpicker({ title: 'Seleccione los permisos' }).selectpicker("refresh");
            }).fail(function (data, textStatus, xhr) {
                console.log(data, textStatus, xhr);
            });
        },        

        setByUser: function () {
            $.ajax({
                url: main.URL_GET_PERMITS,
                type: 'GET',
                data: {
                    key: objectRow.Id
                }
            }).done(function (result, textStatus, xhr) {
                var array = [];
                $.each(result.data, function (index, row) {
                    array.push(row.Id);
                });
                //$('.selectpicker.myselect').selectpicker('val', array);
                main.selectList.selectpicker('val', array).selectpicker("refresh");
            }).fail(function (data, textStatus, xhr) {
                console.log(data, textStatus, xhr);
            });

        },
        EditUser: function () {
            var obj = {
                Key: objectRow.Id,
                Nombre: main.firstName.val(),
                Ape_paterno: main.middleName.val(),
                Ape_materno: main.lastName.val(),
                Email: main.email.val()
            };
            $.ajax({
                url: main.URL_UPDATE_USER,
                type: 'POST',
                data: obj,
                beforeSend: function () {
                    main.btnUpdateUser.attr('disabled', true);
                }
            }).done(function (result, textStatus, xhr) {              
                if (result.Success === true) {
                    swal({
                        title: "Actualización",
                        text: 'Registro actualizado con exito',
                        icon: "success",
                        button: "Aceptar",
                        closeOnClickOutside: false
                    });     
                    main.LoadUsers();
                }
                if (result.Success === false) {
                    swal({
                        title: "Alerta",
                        text: result.Message,
                        icon: "warning",
                        button: "Aceptar",
                        closeOnClickOutside: false
                    });     
                }
                main.btnUpdateUser.attr('disabled', false);
            }).fail(function (data, textStatus, xhr) {
                console.log(data, textStatus, xhr);
                main.btnUpdateUser.attr('disabled', false);
            });
        },
        obtenerDistribuidores: function () {
            $.ajax({
                url: main.url_obtener_distribuidores_usuario,
                type: 'GET',
                data: {
                    key: objectRow.Id
                }
            }).done(function (result, textStatus, xhr) {
                var array = [];
                $.each(result, function (index, row) {
                    array.push(row.IdDistribuidor);
                });
                $('#listadistribuidor').selectpicker('val', array).selectpicker("refresh");               
            }).fail(function (data, textStatus, xhr) {
                console.log(data, textStatus, xhr);
            });
        },
        actualizarDistribuidor: function () {
            $.ajax({
                url: main.url_actualizar_distribuidores_usuario,
                type: 'GET',
                data: {
                    key: objectRow.Id,
                    arrayKey: $('#listadistribuidor').val().toString()
                },
                beforeSend: function () {
                    main.btnUpdateDistribuidor.attr('disabled', true);
                }
            }).done(function (result, textStatus, xhr) {                
                if (result.Success === true) {
                    swal({
                        title: "Actualización",
                        text: 'Lista de distribuidores, se actualizo correctamente',
                        icon: "success",
                        button: "Aceptar",
                        closeOnClickOutside: false
                    });     
                }
                if (result.Success === false) {
                    swal({
                        title: "Alerta",
                        text: result.Message,
                        icon: "warning",
                        button: "Aceptar",
                        closeOnClickOutside: false
                    });     
                }

                main.btnUpdateDistribuidor.attr('disabled', false);
            }).fail(function (data, textStatus, xhr) {
                console.log(data, textStatus, xhr);
                main.btnUpdateDistribuidor.attr('disabled', false);
            });

        }
    };

    global.main = main;
    main.init();

});


var principal = {
    url_obtener_roles: appBaseUrl + 'api/Service/GetRoles',
    url_obtener_distribuidor: appBaseUrl + 'api/Service/ObtenerDistribuidores',
    url_registrar_usuario: appBaseUrl + 'api/Customer/AddUser',

    txtNombre: $('#nombre'),
    txtApe_paterno: $('#ape_paterno'),
    txtApe_materno: $('#ape_materno'),
    txtUsername: $('#username'),
    txtPassword: $('#password'),
    txtConfPassword: $('#Conf_password'),
    txtEmail: $('#email'),
    selectRoles: $('#roles'),
    selectDistribuidor: $('#distribuidor'),
    btnRegistrar: $('#btnAddUser'),

    valido: {
        nombre: false,
        ape_paterno: true,
        ape_materno: true,
        username: false,
        password: false,
        passwordC: false,
        email: false,
        roles: false
    },
    iniciar: function () {
        principal.obtenerDistribuidor();

        this.txtNombre.on('change', function () {
            $("div[data-valmsg-for=" + this.name + "]").addClass('hidden');
            if (this.value === "") {
                principal.validarCampos(this.name, "Nombre es obligatorio");
                return;
            }
            principal.valido.nombre = true;
        });
        this.txtUsername.on('change', function () {
            $("div[data-valmsg-for=" + this.name + "]").addClass('hidden');
            if (this.value === "") {
                principal.validarCampos(this.name, "Usuario es obligatorio");
                return;
            }
            principal.valido.username = true;
        });
        this.txtPassword.on('change', function () {
            $("div[data-valmsg-for=" + this.name + "]").addClass('hidden');
            if (this.value === "") {
                principal.validarCampos(this.name, "Contraseña es obligatorio");
                return;
            }
            principal.valido.password = true;
        });
        this.txtConfPassword.on('change', function () {
            $("div[data-valmsg-for=" + this.name + "]").addClass('hidden');
            if (this.value === "") {
                principal.validarCampos(this.name, "La confirmación es obligatorio");
                return;
            }
            if (this.value !== principal.txtPassword.val()) {
                principal.validarCampos(this.name, "La contraseña no coincide");
                return;
            }
            principal.valido.passwordC = true;
        });
        this.txtEmail.on('change', function () {
            $("div[data-valmsg-for=" + this.name + "]").addClass('hidden');
            if (this.value === "") {
                principal.validarCampos(this.name, "Correo es obligatorio");
                return;
            }
            var expr = /^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$/;
            if (!expr.test(this.value)) {
                principal.validarCampos(this.name, "Correo invalido");
                return;
            }
            principal.valido.email = true;
        });
        this.selectRoles.on('change', function () {           
            $("div[data-valmsg-for=" + this.name + "]").addClass('hidden');
            if (this.value === "") {
                principal.validarCampos(this.name, "Rol es obligatoria");
                return;
            }
            principal.valido.roles = true;
        });
        
        this.btnRegistrar.click(function () {
            principal.valido.nombre = false;
            principal.valido.username = false;
            principal.valido.password = false;
            principal.valido.passwordC = false;
            principal.valido.email = false;
            principal.valido.roles = false;

            principal.txtNombre.trigger('change');
            principal.txtUsername.trigger('change');
            principal.txtPassword.trigger('change');
            principal.txtConfPassword.trigger('change');
            principal.txtEmail.trigger('change');
            principal.selectRoles.trigger('change');
            
            var usuario = {
                Nombre: principal.txtNombre.val(),
                Ape_paterno: principal.txtApe_paterno.val(),
                Ape_materno: principal.txtApe_materno.val(),
                Username: principal.txtUsername.val(),
                Password: principal.txtPassword.val(),
                Email: principal.txtEmail.val(),
                Roles: principal.selectRoles.val().toString(),
                Distribuidor: principal.selectDistribuidor.val().toString()
            };
            var val = principal.valido;
            if (val.nombre === false || val.username === false || val.password === false || val.passwordC === false || val.email === false || val.roles === false) {
                swal({
                    title: "Atención",
                    text: 'Por favor ingrese la información restante',
                    icon: "info",
                    button: "Aceptar",
                    closeOnClickOutside: false
                });     
                return;
            }
            principal.registrarUsuario(usuario);
        });

        $('#btnreset').click(function () {
            $(".dataFieldError").addClass('hidden');
            principal.selectRoles.val('').selectpicker("refresh");
            principal.selectDistribuidor.val('').selectpicker('refresh');
        });
    },
    validarCampos: function (nombre, mensaje) {
        $("div[data-valmsg-for=" + nombre + "]").text(mensaje);
        $("div[data-valmsg-for=" + nombre + "]").removeClass('hidden');
    },
    obtenerRoles: function () {
        $.ajax({
            url: principal.url_obtener_roles,
            type: 'GET',
            async: false
        }).done(function (result, textStatus, xhr) {
            principal.selectRoles.empty();
            $.each(result.data, function (index, row) {
                var html = ' <option value=\"' + row.Name + '">' + row.Name + '</option>'
                principal.selectRoles.append(html);
            });
            //$Utils.addSelectOptions(result.data, principal.selectRoles, 'Name', 'Name');
            principal.selectRoles.selectpicker({ title: 'Roles disponibles' }).selectpicker('refresh');
        }).fail(function (data, textStatus, xhr) {
            console.log(data, textStatus, xhr);
        });
    },
    obtenerDistribuidor: function () {
        $.ajax({
            url: principal.url_obtener_distribuidor,
            type: 'GET',
            async: false
        }).done(function (result, textStatus, xhr) {
            principal.selectDistribuidor.empty();
            $.each(result, function (index, row) {
                var html = ' <option value=\"' + row.id + '">' + row.descripcion_larga + '</option>'
                principal.selectDistribuidor.append(html);
            });
            //$Utils.addSelectOptions(result, principal.selectDistribuidor, 'id', 'descripcion_larga');
            principal.selectDistribuidor.selectpicker({ title: 'Lista de distribuidores' }).selectpicker('refresh');
        }).fail(function (data, textStatus, xhr) {
            console.log(data, textStatus, xhr);
        });
    },
    registrarUsuario: function (usuario) {
          
        $.ajax({
            url: principal.url_registrar_usuario,
            type: 'POST',
            async: false,
            data: usuario,
            beforeSend: function () {
                principal.btnRegistrar.attr('disabled', true);
            }
        }).done(function (result, textStatus, xhr) {
            if (result.Success === false) {
                swal({
                    title: "Alerta",
                    text: result.Message,
                    icon: "warning",
                    button: "Aceptar",
                    closeOnClickOutside: false
                });     
            }
            if (result.Success === true) {
                $(".close").click();
                swal({
                    title: "Atención",
                    text: 'Nuevo registro, agregado correctamente',
                    icon: "success",
                    button: "Aceptar",
                    closeOnClickOutside: false
                });     
                // Cargar de nuevo todos los usuarios, (buscar mejor opción para no recargas todos los usuarios)
                global.main.LoadUsers();
                principal.btnRegistrar.attr('disabled', false);
            }
            principal.btnRegistrar.attr('disabled', false);
        }).fail(function (data, textStatus, xhr) {
            console.log(data, textStatus, xhr);
            principal.btnRegistrar.attr('disabled', false);
            //swal({
            //    title: "Error",
            //    text: textStatus,
            //    icon: "error",
            //    button: "Aceptar",
            //    closeOnClickOutside: false
            //});     
        });
    },
    
};
principal.obtenerRoles();
principal.iniciar();