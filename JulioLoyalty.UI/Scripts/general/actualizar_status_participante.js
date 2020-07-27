
var row = null;
$(function (e) {

    var control = new Object();
    control.urlAPI = {
        url_obtener_catalogo: appBaseUrl + '/api/MCatalogo/FKCatalogos',
        url_consultar_participantes: appBaseUrl + '/api/Customer/FindByUser',
        url_actualizar_stutus_participante: appBaseUrl + '/api/Customer/Actualizar/Status'
    };
    control.webControls = {
        table_participantes: $('#table_participantes'),
        selStatus: $('#selPartStatus'),
        txtComentarios: $('#comentarios'),
        btnActualizar: $('#btnActualizar')
    };
    control.obtenerParticipantes = function () {
        var lUser = localStorage.getItem('objUser');
        if (lUser === null) {
            swal({
                title: 'Socia no encontrada',
                text: 'Le recomendamos que seleccione el registro correspondiente',
                icon: "info",
                button: "Aceptar",
                closeOnClickOutside: false
            });
            return;
        }

        var user = JSON.parse(lUser);

        this.webControls.table_participantes.DataTable().destroy();
        this.webControls.table_participantes.DataTable({
            language: {
                url: '//cdn.datatables.net/plug-ins/1.10.15/i18n/Spanish.json'
            },
            ajax: {
                url: this.urlAPI.url_consultar_participantes,
                data: function (d) {
                    d.id = user.id;
                },
                dataSrc: function (json) {
                    return json;
                }
            },
            lengthChange: false,
            rowId: 'id',
            scrollCollapse: true,
            paging: false,
            searching: false,
            //ordering: false,
            info: false,
            columns: [
                { data: 'clave', width: '60px' },
                {
                    data: 'nombre',
                    render: function (data, type, row) {
                        return row.nombre + ' ' + row.apellido_paterno + ' ' + row.apellido_materno;
                    }
                },
                {
                    data: 'fecha_nacimiento',
                    render: function (data, type, row) {
                        return control.formatoFecha(data);
                    }
                },
                { data: 'correo_electronico' },
                { data: 'status' },
                {
                    mData: 'id',
                    width: '20px',
                    orderable: false,
                    mRender: function (data, type, row) {
                        return '<div style="text-align:center;"> <button class="btn btn-info btn-sm" title="Editar usuario" data-toggle="modal" data-target="#edit" ><i class="far fa-edit"></i></button>';
                    }
                },
            ]
        });
    };
    control.obtenerStatusParticipante = function () {
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
                    descripcion: control.maysPrimera(data.descripcion)
                };
                array.push(add);
            });

            control.webControls.selStatus.empty();
            $.each(array, function (index, row) {
                var html = ' <option value=\"' + row.id + '">' + row.descripcion + '</option>'
                control.webControls.selStatus.append(html);
            });

            //$Utils.addSelectOptions(array, control.webControls.selStatus, 'id', 'descripcion');
        }).fail(function (data, textStatus, xhr) {
            console.log(data, textStatus, xhr);
        });
    };
    control.actualizarParticipante = function () {
        var _data = {
            participante_id: row.id,
            status_id: this.webControls.selStatus.val(),
            comentarios: this.webControls.txtComentarios.val().trim()
        }
        control.webControls.txtComentarios.removeClass('is-invalid').addClass('is-valid');
        if (_data.comentarios === "") {
            control.webControls.txtComentarios.addClass('is-invalid');
            return;
        }
        $.ajax({
            url: this.urlAPI.url_actualizar_stutus_participante,
            type: 'POST',
            async: false,
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify(_data),
            beforeSend: function () {
                control.webControls.btnActualizar.attr('disabled', true);
            },
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
            control.webControls.btnActualizar.attr('disabled', false);
            // Actualizar status_participante_id del lado del cliente
            if (data.Success === true) {
                var lUser = localStorage.getItem('objUser');
                var user = JSON.parse(lUser);             
                user.status_participante_id = control.webControls.selStatus.val();
                user.acumula = data.acumula;
                user.status = data.status;
                user.acumula_mensaje = data.acumula_mensaje;
                localStorage.setItem('objUser', JSON.stringify(user));
            }

        }).fail(function (data, textStatus, xhr) {
            console.log(data, textStatus, xhr);
            control.webControls.btnActualizar.attr('disabled', false);
        });
    };

    control.maysPrimera = function (cadena) {
        return cadena.charAt(0).toUpperCase() + cadena.slice(1).toLowerCase();
    };
    control.formatoFecha = function (fecha) {
        var date = new Date(fecha);
        var formatted = ("0" + date.getDate()).slice(-2) + "/" + ("0" + (date.getMonth() + 1)).slice(-2) + "/" + date.getFullYear();
        return formatted;
    };
    control.iniciarControles = function () {
        this.obtenerStatusParticipante();
        this.obtenerParticipantes();

        this.webControls.selStatus.selectpicker({
            language: 'es',
            title: 'No hay selección'
        }).selectpicker('refresh');

        this.webControls.table_participantes.on('click', 'tr', function (event) {
            row = control.webControls.table_participantes.DataTable().row(this).data();
            var event_tarjet = $(event.target).attr('class');
            if (event_tarjet === 'far fa-edit' || event_tarjet === 'btn btn-info btn-sm') {
                control.webControls.selStatus.val(row.status_participante_id).selectpicker("refresh");
                //$('.modal').on('shown.bs.modal', function () {})
            }
        });

        this.webControls.btnActualizar.click(function () {
            control.actualizarParticipante();
        });
    };
    control.iniciarControles();

});