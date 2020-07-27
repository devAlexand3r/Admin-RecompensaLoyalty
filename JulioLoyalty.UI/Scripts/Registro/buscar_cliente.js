
var cliente = {};

var principal = {
    url_busqueda_cliente: appBaseUrl + '/api/Customer/Find',
    url_consulta_seguimiento: appBaseUrl + '/api/Llamada/Seguimiento',
    table_resultado: $('#table_resultados'),

    iniciar: function () {
        var txt = localStorage.getItem('txtBusqueda');
        var parametro = txt === null ? "?" : txt;
        if (parametro === "?") {
            swal({
                title: 'Bienvenido a Recompensas Loyalty',
                text: '',
                icon: "info",
                button: "Aceptar",
                closeOnClickOutside: false
            });
        }
        this.table_resultado.DataTable();
        this.obtenerResultados(parametro);
        this.table_resultado.on('click', 'tr', function (event) {
            var objectRow = principal.table_resultado.DataTable().row(this).data();

            var nombre = objectRow.nombre + " " + objectRow.apellido_paterno + " " + objectRow.apellido_materno;

            localStorage.setItem('nombre', nombre);
            localStorage.setItem('objUser', JSON.stringify(objectRow));
            localStorage.setItem('name', 'menu_3');
            localStorage.setItem('menu', 'submenu_7');
            localStorage.setItem('href', appBaseUrl + '/Operation/Transaction');

            if ($(this).hasClass('row-selected')) {
                $(this).removeClass('row-selected');
            }
            else {
                principal.table_resultado.DataTable().$('tr.row-selected').removeClass('row-selected');
                $(this).addClass('row-selected');
            }
            if (objectRow.nombre === null) {
                localStorage.setItem('nombre', " ");
            } else {
                $('#spanCliente').text(nombre);
            }
            //swal({
            //    toast: true,
            //    position: 'top-end',
            //    title: '¡Cliente seleccionado!',
            //    type: 'success',
            //    //html: "Nombre completo: " + nombre + "<br /> Número de tarjeta: " + objectRow.clave + "",
            //    confirmButtonColor: '#11ACA6',
            //    confirmButtonText: 'Aceptar',
            //    showConfirmButton: false,
            //    timer: 2000
            //});

            // Checa si el participante tiene llamadas pendientes solo si tiene Rol Administrador ó CallCenter
            if ($('#hidAccesoLlamadas').val() === "1") {
                principal.consultaSeguimiento(objectRow.id);
            }
            else { // Se va directamente a transacciones
                window.location.href = appBaseUrl + "Operation/Transaction";
            }
        });
    },
    obtenerResultados: function (dato) {
        principal.table_resultado.DataTable().destroy();
        principal.table_resultado.DataTable({
            language: {
                url: '//cdn.datatables.net/plug-ins/1.10.15/i18n/Spanish.json'
            },
            ajax: {
                url: principal.url_busqueda_cliente,
                data: function (d) {
                    d.parameter = dato;
                },
                dataSrc: function (data) {
                    return JSON.parse(data);;
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
                { data: 'nombre' },
                { data: 'apellido_paterno' },
                { data: 'apellido_materno' },
                {
                    data: 'correo_electronico',
                    render: function (data, type, row) {
                        var rol = $('#rolName').val();
                        return rol === "-" ? "" : data === null ? "" : data;
                    }
                },
                {
                    data: 'status_participante_id',
                    render: function (data, type, row) {
                        var rol = $('#rolName').val();
                        var result = '';
                        if (rol !== "-") {
                            if (data === 1)
                                result = '<i class="fas fa-clock text-warning"></i>';
                            if (data === 2)
                                result = '<i class="fas fa-check text-success"></i>';
                            if (data === 3)
                                result = '<i class="fas fa-times text-danger"></i>';
                        }
                        return result;
                    }
                }
            ]
        });
    },

    consultaSeguimiento: function (participante_id) {
        $.getJSON(principal.url_consulta_seguimiento, { participante_id: participante_id }, function (json) {
            var array = JSON.parse(json);
            // Si tiene llamadas pendientes se redirecciona a Seguimiento de Llamadas
            if (array.length > 0) {
                swal({
                    title: '¡Atención!',
                    text: 'La socia tiene casos de llamadas pendientes',
                    icon: "info",
                    button: "Aceptar",
                    closeOnClickOutside: false,
                    type: "Success"
                }).then(function () {
                    window.location.href = appBaseUrl + "Llamada/Seguimiento";
                });
            } // Se redirecciona a Transacciones
            else {
                window.location.href = appBaseUrl + "Operation/Transaction";
            }
        });
    }
};
cliente.principal = principal;
principal.iniciar();
