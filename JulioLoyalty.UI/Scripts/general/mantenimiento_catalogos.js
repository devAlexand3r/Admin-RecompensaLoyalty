
var objectRow;
$(function (e) {
    var catalogo = new Object();
    catalogo.urlAPI = {
        url_busqueda_lealtad: appBaseUrl + '/api/MCatalogo/Lealtad',
        url_busqueda_resultado: appBaseUrl + '/api/MCatalogo/Relacion',
        url_busqueda_relacionCatalogo: appBaseUrl + '/api/MCatalogo/RCatalogos',
    };
    catalogo.webControls = {
        selectFiltro: $('#filtro'),
        table_catalogo: $('#table_catalogo'),
        btnActualizar: $('#btnActualizar'),
        btnAgregar: $('#btnAgregar')
    };
    catalogo.ejecutar = function () { };
    catalogo.obtenerLealtad = function () {
        $.ajax({
            url: this.urlAPI.url_busqueda_lealtad,
            type: 'GET',
            async: true,
            success: function (data, status, xhr) {
                catalogo.webControls.selectFiltro.empty();
                $.each(data, function (index, row) {
                    var html = ' <option value=\"' + row.descripcion + '">' + row.descripcion_larga + '</option>'
                    catalogo.webControls.selectFiltro.append(html);
                });
                //$Utils.addSelectOptions(data, catalogo.webControls.selectFiltro, 'descripcion', 'descripcion_larga');
                catalogo.webControls.selectFiltro.selectpicker("refresh");
            },
            error: function (data, status, xhr) {
                console.log(data);
            }
        });
    };
    catalogo.obtenerEsquema = function (value) {
        $.ajax({
            url: appBaseUrl + '/api/MCatalogo/CEsquema',
            type: 'GET',
            data: { catalogo: value },
        }).done(function (result, textStatus, xhr) {
            localStorage.setItem('cEsquema', result); // Guardar en el local storage el esquema del catalogo consultado
            var data = JSON.parse(result);

            $('#contenido').html('');
            $('#contenido_editar').html('');
            $.each(JSON.parse(result), function (data, row) {

                if (row.Tipo === 'varchar') {
                    var addHTML = '<div class="col-lg-6 required"><label class="control-label">' + row.Columna.replace('_', ' ') + ' </label><div class="dataFieldError hidden" data-valmsg-for="' + row.Columna + '"></div><input type="text" id="' + row.Columna + '" class="form-control" placeholder="' + row.Columna + '" /></div >';
                    $('#contenido').append(addHTML);

                    //Modal Editar:
                    var addHTML_Editar = '<div class="col-lg-6 required"><label class="control-label">' + row.Columna.replace('_', ' ') + ' </label><div class="dataFieldError hidden" data-valmsg-for="E' + row.Columna + '"></div><input type="text" id="E' + row.Columna + '" class="form-control" placeholder="' + row.Columna + '" /></div >';
                    $('#contenido_editar').append(addHTML_Editar);
                }
                // drowndown list
                if (row.FK === 1) {

                    var catalogoFK = row.Columna.substring(0, (row.Columna.length - 3));
                    var addDrown = '<div class="col-lg-6 required"><label class="control-label"> ' + catalogoFK.replace('_', ' ') + '</label ><div class="dataFieldError hidden" data-valmsg-for="' + row.Columna + '"></div> <select class="form-control" id="' + row.Columna + '" data-live-search="true" data-size="7"></select></div >';
                    $('#contenido').append(addDrown);

                    //Modal Editar:
                    var addDrown_Editar = '<div class="col-lg-6 required"><label class="control-label"> ' + catalogoFK.replace('_', ' ') + '</label ><div class="dataFieldError hidden" data-valmsg-for="E' + row.Columna + '"></div> <select class="form-control" id="E' + row.Columna + '" data-live-search="true" data-size="7"></select></div >';
                    $('#contenido_editar').append(addDrown_Editar);

                    // Obtener catalogo
                    catalogo.obtenerFKCatalogo(catalogoFK, row.Columna);
                    catalogo.obtenerFKCatalogo(catalogoFK, row.Columna);
                }
            });
        }).fail(function (data, textStatus, xhr) {
            console.log(data, textStatus, xhr);
        });

    };
    catalogo.obtenerFKCatalogo = function (descripcion, control_id) {
        $.ajax({
            url: appBaseUrl + '/api/MCatalogo/FKCatalogos',
            type: 'GET',
            data: { descripcion: descripcion }
        }).done(function (data, textStatus, xhr) {
            $('#' + control_id).empty();
            $('#E' + control_id).empty();
            $.each(JSON.parse(data), function (index, row) {
                var html = ' <option value=\"' + row.id + '">' + row.descripcion + '</option>'
                $('#' + control_id).append(html);
                $('#E' + control_id).append(html);
                //catalogo.webControls.selectFiltro.append(html);
            });

            //$Utils.addSelectOptions(JSON.parse(data), $('#' + control_id), 'id', 'descripcion'); // controles, Agregar           
            //$Utils.addSelectOptions(JSON.parse(data), $('#E' + control_id), 'id', 'descripcion'); // controles, Editar
        }).fail(function (data, textStatus, xhr) {
            console.log(data, textStatus, xhr);
        });
    };
    
    catalogo.obtenerRelacionCatalogo = function (value) {
        catalogo.webControls.table_catalogo.DataTable().clear().destroy();
        catalogo.webControls.table_catalogo.DataTable({
            language: {
                url: '//cdn.datatables.net/plug-ins/1.10.15/i18n/Spanish.json'
            },
            ajax: {
                url: this.urlAPI.url_busqueda_relacionCatalogo,
                data: function (d) {
                    d.catalogo = value
                },
                dataSrc: function (data) {
                    return JSON.parse(data);
                },
                error: function (data, status, xhr) {
                    //$swal.error(data.statusText, data.responseJSON.ExceptionMessage);
                }
            },
            lengthChange: false,
            rowId: 'id',
            scrollCollapse: true,
            paging: true,
            searching: true,
            ordering: false,
            columns: [
                { data: 'clave' },
                { data: 'descripcion' },
                { data: 'descripcion_larga' },
                {
                    data: 'fecha_alta',
                    render: function (data, type, row) {
                        return catalogo.formatoFecha(data);
                    }
                },
                {
                    data: 'id',
                    width: "70px",
                    render: function (data, type, row) {
                        var btn = '<div style="text-align:center;"> <button class="btn btn-info btn-sm" data-title="Editar" data-toggle="modal" data-target="#edit" ><i class="far fa-edit"></i></button></div>';
                            //+ '  <button title="Eliminar" class="btn btn-danger btn-sm"><i class="far fa-trash-alt"></i> </button></div>';
                        return btn;
                    }
                }
            ]
        });
    };
    catalogo.formatoFecha = function (fecha) {
        var date = new Date(fecha);
        var formatted = ("0" + date.getDate()).slice(-2) + "/" + ("0" + (date.getMonth() + 1)).slice(-2) + "/" + date.getFullYear(); // + " " + date.getHours() + ":" + date.getMinutes();
        return formatted;
    };

    catalogo.agregarRegistro = function () {
        // Armar objeto con las columnas requeridas por cada catalogo
        var obj = {
            tableName: catalogo.webControls.selectFiltro.val(),
            Columns: []
        };
        var data = JSON.parse(localStorage.getItem('cEsquema'));
        $.each(data, function (data, row) {
            var add = {
                name: row.Columna,
                value: $('#' + row.Columna).val(),
                type: row.Tipo
            }
            obj.Columns.push(add);
        });
        // Realizar la petición para el nuevo registro 
        $.ajax({
            url: appBaseUrl + '/api/MCatalogo/AgregarMCatalogo',
            type: 'POST',
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify(obj),
            beforeSend: function () {
                catalogo.webControls.btnAgregar.attr('disabled', true);
            }
        }).done(function (data, textStatus, xhr) {
            if (data.Success === true) {
                swal({
                    title: "Atención",
                    text: data.Message,
                    icon: "success",
                    button: "Aceptar",
                    closeOnClickOutside: false
                });
            }
            if (data.Success === false) {
                if (data.InnerException !== null) {
                    swal({
                        title: "Error",
                        text: data.InnerException,
                        icon: "error",
                        button: "Aceptar",
                        closeOnClickOutside: false
                    });
                } else {
                    swal({
                        title: "Atención",
                        text: data.Message,
                        icon: "warning",
                        button: "Aceptar",
                        closeOnClickOutside: false
                    });
                }
            }
            catalogo.webControls.btnAgregar.attr('disabled', false);
        }).fail(function (data, textStatus, xhr) {
            console.log(data, textStatus, xhr);
            catalogo.webControls.btnAgregar.attr('disabled', false);
        });
    };
    catalogo.actualizarRegistro = function () {
        // Armar objeto con las columnas requeridas por cada catalogo
        var obj = {
            tableName: catalogo.webControls.selectFiltro.val(),
            Columns: []
        };
        var data = JSON.parse(localStorage.getItem('cEsquema'));
        $.each(data, function (data, row) {
            if (row.Columna !== "usuario_alta_id" && row.Columna !== "fecha_alta") {
                var add = {
                    name: row.Columna,
                    value: $('#E' + row.Columna).val(),
                    type: row.Tipo
                }
                obj.Columns.push(add);
            }
        });
        obj.Columns.push({ name: 'id', value: objectRow.id, type: 'decimal' });
        obj.Columns.push({ name: 'fecha_cambio', value: null, type: 'datetime' });
        obj.Columns.push({ name: 'usuario_cambio_id', value: null, type: 'uniqueidentifier' });

        // Realizar proceso de actualizacion 
        $.ajax({
            url: appBaseUrl + '/api/MCatalogo/ActualizarMCatalogo',
            type: 'POST',
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify(obj),
            beforeSend: function () {
                catalogo.webControls.btnActualizar.attr('disabled', true);
            }
        }).done(function (data, textStatus, xhr) {
            if (data.Success === true) {
                swal({
                    title: "Atención",
                    text: data.Message,
                    icon: "success",
                    button: "Aceptar",
                    closeOnClickOutside: false
                });
            }
            if (data.Success === false) {
                if (data.InnerException !== null) {
                    swal({
                        title: "Error",
                        text: data.InnerException,
                        icon: "error",
                        button: "Aceptar",
                        closeOnClickOutside: false
                    });
                } else {
                    swal({
                        title: "Atención",
                        text: data.Message,
                        icon: "warning",
                        button: "Aceptar",
                        closeOnClickOutside: false
                    });
                }
            }
            catalogo.webControls.btnActualizar.attr('disabled', false);

        }).fail(function (data, textStatus, xhr) {
            console.log(data, textStatus, xhr);
            catalogo.webControls.btnActualizar.attr('disabled', false);
        });
    };
    catalogo.eliminarRegistro = function () {
        var obj = {
            tableName: catalogo.webControls.selectFiltro.val(),
            Columns: [
                { name: 'id', value: objectRow.id, type: 'decimal' }
            ]
        };
        //$swal.info('Atención', 'Por el momento no podemos procesar su solicitud');
        return;
        // Enviar instrucción de eliminar de manera definitivamente
        $.ajax({
            url: appBaseUrl + '/api/MCatalogo/EliminarMCatalogo',
            type: 'POST',
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify(obj),
        }).done(function (data, textStatus, xhr) {
            if (data.Success === true) {
                swal({
                    title: "Atención",
                    text: data.Message,
                    icon: "success",
                    button: "Aceptar",
                    closeOnClickOutside: false
                });
                catalogo.webControls.table_catalogo.DataTable().row('#' + objectRow.id).remove().draw(false);
            }
            if (data.Success === false) {
                if (data.InnerException !== null) {
                    swal({
                        title: "Error",
                        text: data.InnerException,
                        icon: "error",
                        button: "Aceptar",
                        closeOnClickOutside: false
                    });
                } else {
                    swal({
                        title: "Atención",
                        text: data.Message,
                        icon: "warning",
                        button: "Aceptar",
                        closeOnClickOutside: false
                    });
                }
            }


        }).fail(function (data, textStatus, xhr) {
            console.log(data, textStatus, xhr);
        });
    };

    catalogo.iniciar = function () {
        this.obtenerLealtad();
        this.webControls.table_catalogo.DataTable(
            {
                language: {
                    url: '//cdn.datatables.net/plug-ins/1.10.15/i18n/Spanish.json'
                },
                lengthChange: false,
                scrollCollapse: true,
                paging: true,
                searching: true,
                ordering: false,
            });

        this.webControls.selectFiltro.on('change', function () {
            catalogo.obtenerRelacionCatalogo(this.value);
            catalogo.obtenerEsquema(this.value);
        });

        this.webControls.table_catalogo.on('click', 'tr', function (event) {
            objectRow = catalogo.webControls.table_catalogo.DataTable().row(this).data();
            var event_tarjet = $(event.target).attr('class');

            // Boton editar
            if (event_tarjet === 'far fa-edit' || event_tarjet === 'btn btn-info btn-sm') {
                var data = JSON.parse(localStorage.getItem('cEsquema'));
                $.each(data, function (data, row) {
                    var value = objectRow[row.Columna];
                    if (row.Tipo === 'varchar') {
                        $('#E' + row.Columna).val(value);
                    }
                    // drown down catalogos
                    if (row.FK === 1) {
                        $('#E' + row.Columna).val(value);
                    }
                });

            }

            // Boton eliminar
            if (event_tarjet === 'far fa-trash-alt' || event_tarjet === 'btn btn-danger btn-sm') {
                catalogo.ejecutar.prototype.metodo = function () {
                    catalogo.eliminarRegistro();
                };
                //$swal.confirmation('Alerta', 'warning', '¿Esta seguro de eliminar ' + objectRow.clave + '?', catalogo);
            }
        });
        this.webControls.btnAgregar.click(function () {
            catalogo.agregarRegistro();
        });

        this.webControls.btnActualizar.click(function () {
            catalogo.actualizarRegistro();
        });
    };
    catalogo.iniciar();

});

