var global = {};
(function (root, factory) {
    if (typeof define === 'function' && define.amd) { define(['jquery', 'query-builder'], factory); } else { factory(root.jQuery); }
}(this, function ($) {
    "use strict";
    var queryFilter = {
        appBaseUrl: undefined,
        init: function () {
            if (/^(undefined)$/.test(this.appBaseUrl)) {
                this.appBaseUrl = window.location.origin + "/";
            }

            this.setFilterTabTienda();
            this.setFilterTabSocia();
            this.setFilterTabContacto();
            this.setFilterTabMembresia();
            this.setFilterTabCampana();
            this.setFilterTabProducto();
            this.setFilterTabSegmento();

            this.loadCampaing();
            this.loadCampaingLlamada();


            var btnApply = $("#btn-sql");
            btnApply.click(function () {
                console.clear();
                var valueSQL = queryFilter.getFilter();
                var ele = {
                    url: queryFilter.appBaseUrl + "campaign/data",
                    data: { par: valueSQL },
                    container: $('#table-loading')
                };

                if (valueSQL.length > 0) {
                    queryFilter.getData(ele);
                    $('#hidQuery').val(valueSQL);
                }
            });

            /* Botón para obtener el número de registro de la consulta */
            var btn_cnt_sql = $("#btn-cnt-sql");
            btn_cnt_sql.click(function () {
                console.clear();
                var valueSQL = queryFilter.getFilter();
                $.ajax({
                    url: queryFilter.appBaseUrl + "campaign/datacnt",
                    type: "GET",
                    data: {
                        par: valueSQL
                    },
                    beforeSend: function () {

                    }
                }).done(function (result) {
                    result = formatoCantidad(result);
                    console.log(result);
                    $("#span-num_datos_sql").html(result);
                }).fail(function (data, textStatus, xhr) {

                });
            });

            var btnSegm = $("#btn-seg");
            btnSegm.click(function () {

                var controls = [
                    { input: $('#txtName') },
                    { input: $('#txtDescription') }
                ];
                var isValid = true;

                controls.forEach(function (e) {
                    if (e.input.val().length <= 5) {
                        const name = e.input.attr('name');
                        $("div[data-valmsg-for=" + name + "]").removeClass('hidden');
                        $("[name=" + name + "]").addClass('is-invalid');
                        isValid = false;
                    }
                });

                if (isValid) {
                    controls.forEach(function (e) {
                        const name = e.input.attr('name');
                        $("[name=" + name + "]").removeClass('is-invalid').addClass('is-valid');
                        $("div[data-valmsg-for=" + name + "]").addClass('hidden');
                    });

                    var valueSQL = queryFilter.getFilter();
                    $.ajax({
                        url: queryFilter.appBaseUrl + "campaign/savesegment",
                        type: "POST",
                        async: true,
                        data: {
                            prDescription: $('#txtName').val(),
                            prLongDescription: $('#txtDescription').val(),
                            prSQL: valueSQL
                        },
                        beforeSend: function () {
                            $('body').loading({ message: 'Working...', zIndex: 9999 });
                        }
                    }).done(function (result) {
                        console.log(result);
                        if (result.Success) {
                            swal({
                                title: "Atención",
                                text: "Segmento agregado exitosamente",
                                icon: "success",
                                button: "Aceptar",
                                allowOutsideClick: false
                            });
                            $('#modSeg').modal('hide');
                        } else {
                            swal({
                                title: "Atención",
                                text: result.Message,
                                icon: "error",
                                button: "Aceptar",
                                allowOutsideClick: false
                            });
                        }
                        $('body').loading('stop');
                    }).fail(function (data, textStatus, xhr) {
                        console.log(data, textStatus, xhr);
                        $('body').loading('stop');
                    });
                }

            });

        },

        queryOptionBuilder: {
            icons: {
                add_group: 'fas fa-plus-square',
                add_rule: 'fas fa-plus-circle',
                remove_group: 'fas fa-minus-square',
                remove_rule: 'fas fa-minus-circle',
                error: 'fas fa-exclamation-triangle'
            },
            plugins: [
                'unique-filter',
                'bt-checkbox',
                'invert'
            ],
            allow_empty: true,
            display_empty_filter: false,
            filters: null
        },
        setFilterTabTienda: function () {
            var jFilters = [];
            // 1.- Nombre de la tienda
            // 2.- Estado de la tienda
            // 3.- Ciudad de la tienda 
            // 4.- Tipo de tienda 
            var arrayFilter = [
                {
                    filter: {
                        id: "[Nombre Tienda]",
                        label: 'Nombre de la tienda',
                        type: 'string',
                        input: 'select',
                        values: [],
                        operators: ['equal', 'not_equal'],
                        plugin: 'selectpicker',
                        multiple: true,
                        plugin_config: {
                            title: "No hay seleccion",
                            liveSearch: true,
                            width: 'auto',
                            selectedTextFormat: 'count',
                            liveSearchStyle: 'contains',
                            style: null,
                            size: 'auto'
                        }
                    },
                    ajaxData: {
                        id: 1
                    },
                    data: null
                },
                {
                    filter: {
                        id: "[Estado Tienda]",
                        label: 'Estado de la tienda',
                        type: 'string',
                        input: 'select',
                        values: [],
                        operators: ['equal', 'not_equal'],
                        multiple: true,
                        plugin: 'selectpicker',
                        plugin_config: {
                            title: "No hay seleccion",
                            actionsBox: true,
                            liveSearch: true,
                            width: 'auto',
                            selectedTextFormat: 'count',
                            liveSearchStyle: 'contains',
                            style: null,
                            size: 'auto'
                        }
                    },
                    ajaxData: {
                        id: 2
                    },
                    data: null
                },
                {
                    filter: {
                        id: "[Ciudad Tienda]",
                        label: 'Ciudad de la tienda ',
                        type: 'string',
                        input: 'select',
                        values: [],
                        operators: ['equal', 'not_equal'],
                        plugin: 'selectpicker',
                        multiple: true,
                        plugin_config: {
                            title: "No hay seleccion",
                            liveSearch: true,
                            width: 'auto',
                            selectedTextFormat: 'count',
                            liveSearchStyle: 'contains',
                            style: null,
                            size: 'auto'
                        }
                    },
                    ajaxData: {
                        id: 3
                    },
                    data: null
                },
                {
                    filter: {
                        id: "[Tipo Tienda]",
                        label: 'Tipo de tienda',
                        type: 'string',
                        input: 'select',
                        values: [],
                        operators: ['equal', 'not_equal'],
                        plugin: 'selectpicker',
                        multiple: true,
                        plugin_config: {
                            title: "No hay seleccion",
                            liveSearch: true,
                            width: '200px',
                            selectedTextFormat: 'count',
                            liveSearchStyle: 'contains',
                            style: null,
                            size: 'auto'
                        }
                    },
                    ajaxData: {
                        id: 4
                    },
                    data: null
                }
            ];

            arrayFilter.forEach(function (element) {
                queryFilter.ajaxRequestCallBack(element.ajaxData, function (result) {
                    var data = JSON.parse(result);
                    if (data[0].hasOwnProperty("descripcion")) {
                        data.forEach(function (row) {
                            if (row.descripcion.toUpperCase() === "CDMX")
                                row.descripcion = row.descripcion.toUpperCase();
                            element.filter.values.push(row.descripcion);
                        });
                        jFilters.push(element.filter);
                    }
                });
            });

            this.queryOptionBuilder.filters = jFilters;
            var builder = $('#builderTienda');
            builder.queryBuilder(this.queryOptionBuilder);

            $('body').loading('stop');
        },
        setFilterTabSocia: function () {
            // 5.- Género de la Socia
            // 6.- Edad Socia
            // 7.- Estado Socia
            // 8.- Ciudad Socia
            // 9.- Ocupación
            // 10 .- Mes de nacimiento
            // 11 .- Fecha de nacimiento            
            var jFilters = [];
            var arrayFilter = [
                {
                    filter: {
                        id: "[Genero]",
                        label: 'Genero de la Socia',
                        type: 'string',
                        input: 'select',
                        values: [],
                        operators: ['equal', 'not_equal'],
                        plugin: 'selectpicker',
                        multiple: true,
                        plugin_config: {
                            title: "No hay seleccion",
                            liveSearch: true,
                            width: 'auto',
                            selectedTextFormat: 'count',
                            liveSearchStyle: 'contains',
                            style: null,
                            size: 'auto'
                        }
                    },
                    ajaxData: {
                        id: 5
                    },
                    data: null
                },
                {
                    filter: {
                        id: "[Estado]",
                        label: 'Estado Socia',
                        type: 'string',
                        input: 'select',
                        values: [],
                        operators: ['equal', 'not_equal'],
                        plugin: 'selectpicker',
                        multiple: true,
                        plugin_config: {
                            title: "No hay seleccion",
                            liveSearch: true,
                            width: 'auto',
                            selectedTextFormat: 'count',
                            liveSearchStyle: 'contains',
                            style: null,
                            size: 'auto'
                        }
                    },
                    ajaxData: {
                        id: 7
                    },
                    data: null
                },
                {
                    filter: {
                        id: "[Ciudad]",
                        label: 'Ciudad Socia',
                        type: 'string',
                        input: 'select',
                        values: [],
                        operators: ['equal', 'not_equal'],
                        plugin: 'selectpicker',
                        multiple: true,
                        plugin_config: {
                            title: "No hay seleccion",
                            liveSearch: true,
                            width: 'auto',
                            selectedTextFormat: 'count',
                            liveSearchStyle: 'contains',
                            style: null,
                            size: 'auto'
                        }
                    },
                    ajaxData: {
                        id: 8
                    },
                    data: null
                },
                {
                    filter: {
                        id: "[Ocupacion]",
                        label: 'Ocupacion',
                        type: 'string',
                        input: 'select',
                        values: [],
                        operators: ['equal', 'not_equal'],
                        plugin: 'selectpicker',
                        multiple: true,
                        plugin_config: {
                            title: "No hay seleccion",
                            liveSearch: true,
                            width: 'auto',
                            selectedTextFormat: 'count',
                            liveSearchStyle: 'contains',
                            style: null,
                            size: 'auto'
                        }
                    },
                    ajaxData: {
                        id: 9
                    },
                    data: null
                }
            ];

            arrayFilter.forEach(function (element) {
                queryFilter.ajaxRequestCallBack(element.ajaxData, function (result) {
                    var data = JSON.parse(result);
                    try {
                        if (data[0].hasOwnProperty("descripcion")) {
                            data.forEach(function (row) {
                                if (row.descripcion.toUpperCase() === "CDMX")
                                    row.descripcion = row.descripcion.toUpperCase();
                                if (row.descripcion.toUpperCase() === "TIENDA CDMX CLASSIC")
                                    row.descripcion = row.descripcion.toUpperCase();
                                element.filter.values.push(row.descripcion);
                            });
                            jFilters.push(element.filter);
                        }
                    }
                    catch (err) {
                        console.log(err);
                    }
                });

                if (element.ajaxData.id === 5) {
                    // Edad Socia 
                    jFilters.push({
                        id: '[Edad]',
                        label: 'Edad Socia',
                        type: 'integer',
                        operators: ['equal', 'not_equal'],
                        validation: {
                            min: 1,
                            step: 1,
                            max: 100
                        }
                    });
                }

                if (element.ajaxData.id === 9) {
                    // Mes de nacimiento 
                    jFilters.push({
                        id: '[Mes Nacimiento]',
                        label: 'Mes de nacimiento',
                        type: 'integer',
                        operators: ['equal', 'not_equal'],
                        validation: {
                            min: 1,
                            step: 1,
                            max: 12
                        }
                    });

                    // Fecha de nacimiento 
                    jFilters.push({
                        id: "FORMAT([Fecha Nacimiento], 'dd/MM/yyyy')",
                        label: 'Fecha de nacimiento',
                        type: 'date',
                        validation: {
                            format: 'DD/MM/YYYY'
                        },                        
                        plugin: 'datepicker',
                        plugin_config: {
                            language: 'es',
                            format: 'dd/mm/yyyy',
                            todayHighlight: true,
                            autoclose: true
                        }
                    });
                }

            });

            this.queryOptionBuilder.filters = jFilters;
            var builder = $('#builderSocia');
            builder.queryBuilder(this.queryOptionBuilder);

            $('body').loading('stop');
        },
        setFilterTabContacto: function () {
            // 13.- Con Celular
            // 14.- Con Correo
            var jFilters = [];
            var arrayFilter = [
                {
                    filter: {
                        id: "[Con Celular]",
                        label: 'Con Celular',
                        type: 'string',
                        input: 'select',
                        values: [],
                        operators: ['equal', 'not_equal'],
                        plugin: 'selectpicker',
                        multiple: true,
                        plugin_config: {
                            title: "No hay seleccion",
                            liveSearch: true,
                            width: '200px',
                            selectedTextFormat: 'count',
                            liveSearchStyle: 'contains',
                            style: null,
                            size: 'auto'
                        }
                    },
                    ajaxData: {
                        id: 13
                    },
                    data: null
                },
                {
                    filter: {
                        id: "[Con correo]",
                        label: 'Con correo',
                        type: 'string',
                        input: 'select',
                        values: [],
                        operators: ['equal', 'not_equal'],
                        plugin: 'selectpicker',
                        multiple: true,
                        plugin_config: {
                            title: "No hay seleccion",
                            liveSearch: true,
                            width: '200px',
                            selectedTextFormat: 'count',
                            liveSearchStyle: 'contains',
                            style: null,
                            size: 'auto'
                        }
                    },
                    ajaxData: {
                        id: 14
                    },
                    data: null
                },
                {
                    filter: {
                        id: "[Telefono Valido]",
                        label: 'Telefono valido',
                        type: 'string',
                        input: 'select',
                        values: [],
                        operators: ['equal', 'not_equal'],
                        plugin: 'selectpicker',
                        multiple: true,
                        plugin_config: {
                            title: "No hay seleccion",
                            liveSearch: true,
                            width: '200px',
                            selectedTextFormat: 'count',
                            liveSearchStyle: 'contains',
                            style: null,
                            size: 'auto'
                        }
                    },
                    ajaxData: {
                        id: 35
                    },
                    data: null
                },
                {
                    filter: {
                        id: "[Correo Baja]",
                        label: 'Correo baja',
                        type: 'string',
                        input: 'select',
                        values: [],
                        operators: ['equal', 'not_equal'],
                        plugin: 'selectpicker',
                        multiple: true,
                        plugin_config: {
                            title: "No hay seleccion",
                            liveSearch: true,
                            width: '200px',
                            selectedTextFormat: 'count',
                            liveSearchStyle: 'contains',
                            style: null,
                            size: 'auto'
                        }
                    },
                    ajaxData: {
                        id: 36
                    },
                    data: null
                }
            ];

            arrayFilter.forEach(function (element) {
                queryFilter.ajaxRequestCallBack(element.ajaxData, function (result) {
                    var data = JSON.parse(result);
                    if (data[0].hasOwnProperty("descripcion")) {
                        data.forEach(function (row) {
                            element.filter.values.push(row.descripcion);
                        });
                        jFilters.push(element.filter);
                    }
                });
            });

            this.queryOptionBuilder.filters = jFilters;
            var builder = $('#builderContacto');
            builder.queryBuilder(this.queryOptionBuilder);
            $('body').loading('stop');
        },
        setFilterTabMembresia: function () {
            // 15.- Nivel
            // 16.- Status
            // 17.- Fecha de inicio de ciclo
            // 18.- Fecha de fin de ciclo
            // 19.- Número de visitas 365
            // 20.- Número de visitas año vigente
            // 21.- Número de visitas histórico
            // 22.- Fecha primera compra
            // 23.- Fecha última compra
            // 24.- Fecha de activación
            // 25.- Monto acumulado en el ciclo. 
            // 26.- Monto año actual
            // 27.- Monto histórico. 
            // 28.- Monto acumulado 365
            var jFilters = [];
            var arrayFilter = [
                {
                    filter: {
                        id: "[Nivel]",
                        label: 'Nivel',
                        type: 'string',
                        input: 'select',
                        values: [],
                        operators: ['equal', 'not_equal'],
                        plugin: 'selectpicker',
                        multiple: true,
                        plugin_config: {
                            title: "No hay seleccion",
                            liveSearch: true,
                            width: '200px',
                            selectedTextFormat: 'count',
                            liveSearchStyle: 'contains',
                            style: null,
                            size: 'auto'
                        }
                    },
                    ajaxData: {
                        id: 15
                    },
                    data: null
                },
                {
                    filter: {
                        id: "[Status]",
                        label: 'Status',
                        type: 'string',
                        input: 'select',
                        values: [],
                        operators: ['equal', 'not_equal'],
                        plugin: 'selectpicker',
                        multiple: true,
                        plugin_config: {
                            title: "No hay seleccion",
                            liveSearch: true,
                            width: 'auto',
                            selectedTextFormat: 'count',
                            liveSearchStyle: 'contains',
                            style: null,
                            size: 'auto'
                        }
                    },
                    ajaxData: {
                        id: 16
                    },
                    data: null
                }
            ];

            arrayFilter.forEach(function (element) {
                queryFilter.ajaxRequestCallBack(element.ajaxData, function (result) {
                    var data = JSON.parse(result);
                    try {
                        if (data[0].hasOwnProperty("descripcion")) {
                            data.forEach(function (row) {
                                element.filter.values.push(row.descripcion);
                            });
                            jFilters.push(element.filter);
                        }
                    }
                    catch (err) {
                        console.log(err);
                    }
                });

                if (element.ajaxData.id === 16) {
                    // Fecha de inicio de ciclo 
                    jFilters.push({
                        id: "FORMAT([Fecha Inicio Ciclo], 'dd/MM/yyyy')",
                        label: 'Fecha de inicio de ciclo ',
                        type: 'date',
                        validation: {
                            format: 'DD/MM/YYYY'
                        },
                        plugin: 'datepicker',
                        plugin_config: {
                            language: 'es',
                            format: 'dd/mm/yyyy',
                            todayHighlight: true,
                            autoclose: true
                        }
                    });

                    // Fecha de fin de ciclo
                    jFilters.push({
                        id: "FORMAT([Fecha Fin Ciclo], 'dd/MM/yyyy')",
                        label: 'Fecha de fin de ciclo',
                        type: 'date',
                        validation: {
                            format: 'DD/MM/YYYY'
                        },
                        plugin: 'datepicker',
                        plugin_config: {
                            language: 'es',
                            format: 'dd/mm/yyyy',
                            todayHighlight: true,
                            autoclose: true
                        }
                    });

                    // Número de visitas 365 
                    jFilters.push({
                        id: '[Visitas 365]',
                        label: 'Numero de visitas 365',
                        type: 'integer',
                        validation: {
                            min: 1,
                            step: 1
                        }
                    });

                    // Número de visitas año vigente
                    jFilters.push({
                        id: '[Visitas Año Actual]',
                        label: 'Numero de visitas año vigente ',
                        type: 'integer',
                        validation: {
                            min: 1,
                            step: 1
                        }
                    });

                    // Número de visitas histórico
                    jFilters.push({
                        id: '[Visitas Historico]',
                        label: 'Numero de visitas historico',
                        type: 'integer',
                        validation: {
                            min: 1,
                            step: 1
                        }
                    });

                    // Fecha primera compra
                    jFilters.push({
                        id: "[Fecha Primera Compra]",
                        label: 'Fecha primera compra',
                        type: 'date',
                        plugin: 'datepicker',
                        plugin_config: {
                            language: 'es',
                            format: 'dd/mm/yyyy',
                            todayHighlight: true,
                            autoclose: true,
                        }
                    });

                    // Fecha última compra
                    jFilters.push({
                        id: "[Fecha Ultima Compra]",
                        label: 'Fecha ultima compra',
                        type: 'date',
                        plugin: 'datepicker',
                        plugin_config: {
                            language: 'es',
                            format: 'dd/mm/yyyy',
                            todayHighlight: true,
                            autoclose: true,
                        }
                    });

                    // Fecha de activación
                    jFilters.push({
                        //id: "[Fecha Activacion]",
                        id: "FORMAT([Fecha Activacion], 'dd/MM/yyyy')",
                        label: 'Fecha de activacion',
                        type: 'date',
                        plugin: 'datepicker',
                        plugin_config: {
                            language: 'es',
                            format: 'dd/mm/yyyy',
                            todayHighlight: true,
                            autoclose: true,
                        }
                    });

                    // Monto acumulado en el ciclo.
                    jFilters.push({
                        id: '[Monto Ciclo]',
                        label: 'Monto acumulado en el ciclo.',
                        type: 'double',
                        validation: {
                            min: 0,
                            step: 1
                        }
                    });

                    //Monto año actual
                    jFilters.push({
                        id: '[Monto Anual]',
                        label: 'Monto año actual',
                        type: 'double',
                        validation: {
                            min: 0,
                            step: 1
                        }
                    });

                    //Monto histórico.
                    jFilters.push({
                        id: '[Monto Historico]',
                        label: 'Monto historico',
                        type: 'double',
                        validation: {
                            min: 0,
                            step: 1
                        }
                    });

                    //Monto acumulado 365
                    jFilters.push({
                        id: '[Monto 365]',
                        label: 'Monto acumulado 365',
                        type: 'double',
                        validation: {
                            min: 0,
                            step: 1
                        }
                    });
                }
            });

            this.queryOptionBuilder.filters = jFilters;
            var builder = $('#builderMembresia');
            builder.queryBuilder(this.queryOptionBuilder);

            $('body').loading('stop');
        },
        setFilterTabCampana: function () {
            // 29.-	Campaña
            var jFilters = [];
            var arrayFilter = [
                {
                    filter: {
                        id: "ISNULL(camp.[descripcion],'')",
                        label: 'Campaña',
                        type: 'string',
                        input: 'select',
                        values: [],
                        operators: ['equal', 'not_equal', 'in', 'not_in'],
                        plugin: 'selectpicker',
                        multiple: true,
                        plugin_config: {
                            title: "No hay seleccion",
                            liveSearch: true,
                            width: 'auto',
                            selectedTextFormat: 'count',
                            liveSearchStyle: 'contains',
                            style: null,
                            size: 'auto'
                        }
                    },
                    ajaxData: {
                        id: 29
                    },
                    data: null
                }
            ];

            arrayFilter.forEach(function (element) {
                queryFilter.ajaxRequestCallBack(element.ajaxData, function (result) {
                    var data = JSON.parse(result);
                    try {
                        if (data[0].hasOwnProperty("descripcion")) {
                            data.forEach(function (row) {
                                element.filter.values.push(row.descripcion);
                            });
                            jFilters.push(element.filter);
                        }
                    }
                    catch (err) {
                        console.log(err);
                    }
                });
            });

            this.queryOptionBuilder.filters = jFilters;
            var builder = $('#builderCampana');
            builder.queryBuilder(this.queryOptionBuilder);
            $('body').loading('stop');
        },
        setFilterTabProducto: function () {
            //  30.- Temporada
            //  31.- Categoria
            //  32.- Familia
            //  33.- Linea
            //  34.- Talla
            //  35.- Producto
            //  36.- SKU
            //  37.- EAN
            var jFilters = [];
            var arrayFilter = [
                {
                    filter: {
                        id: "prod.[temporada]",
                        label: 'Temporada',
                        type: 'string',
                        input: 'select',
                        values: [],
                        operators: ['equal', 'not_equal'],
                        plugin: 'selectpicker',
                        multiple: false,
                        plugin_config: {
                            title: "No hay seleccion",
                            liveSearch: true,
                            width: 'auto',
                            selectedTextFormat: 'count',
                            liveSearchStyle: 'contains',
                            style: null,
                            size: 'auto'
                        }
                    },
                    ajaxData: {
                        id: 30
                    },
                    data: null
                },
                {
                    filter: {
                        id: "prod.[categoria]",
                        label: 'Categoria',
                        type: 'string',
                        input: 'select',
                        values: [],
                        operators: ['equal', 'not_equal', 'in', 'not_in'],
                        plugin: 'selectpicker',
                        multiple: true,
                        plugin_config: {
                            title: "No hay seleccion",
                            liveSearch: true,
                            width: 'auto',
                            selectedTextFormat: 'count',
                            liveSearchStyle: 'contains',
                            style: null,
                            size: 'auto'
                        }
                    },
                    ajaxData: {
                        id: 31
                    },
                    data: null
                },
                {
                    filter: {
                        id: "prod.[familia]",
                        label: 'Familia',
                        type: 'string',
                        input: 'select',
                        values: [],
                        operators: ['equal', 'not_equal', 'in', 'not_in'],
                        plugin: 'selectpicker',
                        multiple: true,
                        plugin_config: {
                            title: "No hay seleccion",
                            liveSearch: true,
                            width: 'auto',
                            selectedTextFormat: 'count',
                            liveSearchStyle: 'contains',
                            style: null,
                            size: 'auto'
                        }
                    },
                    ajaxData: {
                        id: 32
                    },
                    data: null
                },
                {
                    filter: {
                        id: "prod.[linea]",
                        label: 'Linea',
                        type: 'string',
                        input: 'select',
                        values: [],
                        operators: ['equal', 'not_equal', 'in', 'not_in'],
                        plugin: 'selectpicker',
                        multiple: true,
                        plugin_config: {
                            title: "No hay seleccion",
                            liveSearch: true,
                            width: 'auto',
                            selectedTextFormat: 'count',
                            liveSearchStyle: 'contains',
                            style: null,
                            size: 'auto'
                        }
                    },
                    ajaxData: {
                        id: 33
                    },
                    data: null
                },
                {
                    filter: {
                        id: "prod.[talla]",
                        label: 'Talla',
                        type: 'string',
                        input: 'select',
                        values: [],
                        operators: ['equal', 'not_equal', 'in', 'not_in'],
                        plugin: 'selectpicker',
                        multiple: true,
                        plugin_config: {
                            title: "No hay seleccion",
                            liveSearch: true,
                            width: 'auto',
                            selectedTextFormat: 'count',
                            liveSearchStyle: 'contains',
                            style: null,
                            size: 'auto'
                        }
                    },
                    ajaxData: {
                        id: 34
                    },
                    data: null
                }
            ];

            arrayFilter.forEach(function (element) {
                queryFilter.ajaxRequestCallBack(element.ajaxData, function (result) {
                    var data = JSON.parse(result);
                    try {
                        if (data[0].hasOwnProperty("descripcion")) {
                            data.forEach(function (row) {
                                element.filter.values.push(row.descripcion);
                            });
                            jFilters.push(element.filter);
                        }
                    }
                    catch (err) {
                        console.log(err);
                    }

                    if (element.ajaxData.id === 34) {
                        // Producto
                        jFilters.push({
                            id: 'prod.[producto]',
                            label: 'Producto',
                            type: 'string'
                        });

                        // SKU
                        jFilters.push({
                            id: 'prod.[sku]',
                            label: 'SKU',
                            type: 'string'
                        });

                        // EAN
                        jFilters.push({
                            id: 'prod.[ean]',
                            label: 'EAN',
                            type: 'string'
                        });

                        // Fecha compra
                        jFilters.push({
                            id: "prod.[Fecha Compra]",
                            label: 'Fecha compra',
                            type: 'date',
                            plugin: 'datepicker',
                            plugin_config: {
                                language: 'es',
                                format: 'dd/mm/yyyy',
                                todayHighlight: true,
                                autoclose: true
                            }
                        });

                        // Cantidad
                        jFilters.push({
                            id: 'prod.[Cantidad]',
                            label: 'Cantidad',
                            type: 'double'
                        });

                        // Importe
                        jFilters.push({
                            id: 'prod.[Importe]',
                            label: 'Importe',
                            type: 'double'
                        });

                    }
                });
            });

            this.queryOptionBuilder.filters = jFilters;
            var builder = $('#builderProducto');
            builder.queryBuilder(this.queryOptionBuilder).on("change.queryBuilder", function () {
                var raw = $(this).queryBuilder('getSQL', false, true);            
            });

            builder.queryBuilder(this.queryOptionBuilder).on('afterUpdateRuleValue.queryBuilder', function (e, rule) {
                const filter = rule.__.filter;
                const temporada = rule.__.value;
                console.log(filter); 
                const filters = e.builder.filters;
                if (filter.id === "prod.[temporada]" && temporada.length > 0) { 
                    $.each(filters, function (index, element) {
                        
                        if (element.id === "prod.[categoria]") {
                            element.values = [];
                            queryFilter.ajaxRequestCallBack({
                                id: 31,
                                temporada: temporada
                            }, function (result) {
                                if (result.length > 0) {                                    
                                    var data = JSON.parse(result);
                                    try {
                                        if (data[0].hasOwnProperty("descripcion")) {
                                            data.forEach(function (row) {
                                                element.values.push(row.descripcion);
                                            });
                                        }
                                    }
                                    catch (err) {
                                        console.log(err);
                                    }
                                }
                                $('body').loading('stop');
                            });
                        }

                        if (element.id === "prod.[familia]") {
                            element.values = [];
                            queryFilter.ajaxRequestCallBack({
                                id: 32,
                                temporada: temporada
                            }, function (result) {
                                if (result.length > 0) {  
                                    var data = JSON.parse(result);
                                    try {
                                        if (data[0].hasOwnProperty("descripcion")) {
                                            data.forEach(function (row) {
                                                element.values.push(row.descripcion);
                                            });
                                        }
                                    }
                                    catch (err) {
                                        console.log(err);
                                    }
                                }
                                $('body').loading('stop');
                            });
                        }

                        if (element.id === "prod.[linea]") {
                            element.values = [];
                            queryFilter.ajaxRequestCallBack({
                                id: 33,
                                temporada: temporada
                            }, function (result) {
                                if (result.length > 0) { 
                                    var data = JSON.parse(result);
                                    try {
                                        if (data[0].hasOwnProperty("descripcion")) {
                                            data.forEach(function (row) {
                                                element.values.push(row.descripcion);
                                            });
                                        }
                                    }
                                    catch (err) {
                                        console.log(err);
                                    }
                                }
                                $('body').loading('stop');
                            });
                        }

                        if (element.id === "prod.[talla]") {
                            element.values = [];
                            queryFilter.ajaxRequestCallBack({
                                id: 34,
                                temporada: temporada
                            }, function (result) {
                                if (result.length > 0) { 
                                    var data = JSON.parse(result);
                                    try {
                                        if (data[0].hasOwnProperty("descripcion")) {
                                            data.forEach(function (row) {
                                                element.values.push(row.descripcion);
                                            });
                                        }
                                    }
                                    catch (err) {
                                        console.log(err);
                                    }
                                }
                                $('body').loading('stop');
                            });
                        } 

                    });

                }
            });


            $('body').loading('stop');
        },
        setFilterTabSegmento: function () {
            // 12 .- Segmento
            var jFilters = [];
            var arrayFilter = [
                {
                    filter: {
                        id: "ISNULL(seg.[descripcion],'')",
                        label: 'Segmento',
                        type: 'string',
                        input: 'select',
                        values: [],
                        operators: ['equal', 'not_equal'],
                        plugin: 'selectpicker',
                        multiple: true,
                        plugin_config: {
                            title: "No hay seleccion",
                            liveSearch: true,
                            width: 'auto',
                            selectedTextFormat: 'count',
                            liveSearchStyle: 'contains',
                            style: null,
                            size: 'auto'
                        }
                    },
                    ajaxData: {
                        id: 12
                    },
                    data: null
                }
            ];

            arrayFilter.forEach(function (element) {
                queryFilter.ajaxRequestCallBack(element.ajaxData, function (result) {
                    var data = JSON.parse(result);
                    try {
                        if (data[0].hasOwnProperty("descripcion")) {
                            data.forEach(function (row) {
                                if (row.descripcion.toUpperCase() === "CDMX")
                                    row.descripcion = row.descripcion.toUpperCase();
                                if (row.descripcion.toUpperCase() === "TIENDA CDMX CLASSIC")
                                    row.descripcion = row.descripcion.toUpperCase();
                                element.filter.values.push(row.descripcion);
                            });
                            jFilters.push(element.filter);
                        }
                    }
                    catch (err) {
                        console.log(err);
                    }
                });
            });

            this.queryOptionBuilder.filters = jFilters;
            var builder = $('#builderSegmento');
            builder.queryBuilder(this.queryOptionBuilder);

            $('body').loading('stop');
        },
        getData: function (e) {
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
                var table = $("#tabla_reporte_dinamico").DataTableDynamic({
                    aaData: data,
                    aoColumnDefs: [
                        { visible: false, targets: 0 }
                    ],
                    info: true,
                    rowId: 'id',
                    showTotal: false,
                    scrollY: "400px",
                    scrollX: true,
                    scrollCollapse: true,
                    paging: false,
                    dom: 'Bfrtip',

                    buttons: [{
                        extend: 'excel',
                        footer: true,
                        text: '<i class="far fa-file-excel"></i> Exportar',
                        className: 'btn btn-success',
                        title: "Generador de campaña",
                        filename: 'Generador de campaña',
                        exportOptions: {
                            columns: ':visible'
                        }
                    },
                    {
                        extend: 'colvis',
                        text: 'Columnas',
                        className: 'btn btn-info'
                    },
                    {
                        text: '<i class="far fa-plus-square"></i> Generar campaña',
                        className: 'btn btn-warning',
                        action: function (e, dt, node, config) {
                            $("#crear").modal('show');
                            config.counter++;
                        },
                        counter: 1
                    },
                    {
                        text: '<i class="fas fa-phone"></i> Generar llamadas',
                        className: 'btn btn-danger',
                        action: function (e, dt, node, config) {
                            var counter = config.counter++;
                            $("#crear_llamada").modal('show').off('shown.bs.modal').on('shown.bs.modal', function (e) {
                                if (counter === 1) {
                                    global.loadGridUsuariosLlamadas();
                                }
                            })
                        },
                        counter: 1
                    },
                    {
                        text: '<i class="fa fa-plus-circle"></i> Generar segmento',
                        className: 'btn btn-success',
                        action: function (e, dt, node, config) {
                            var counter = config.counter++;
                            $("#modSeg").modal('show').off('shown.bs.modal').on('shown.bs.modal', function (e) { });
                        },
                        counter: 1
                    }
                    ],
                    fixedColumns: {
                        leftColumns: 2
                        //rightColumns: 1
                    }
                });
                table.buttons().container().appendTo('#tabla_reporte_dinamico_wrapper .col-sm-6:eq(0)');
                $('#lblNumRegistros').text($('#tabla_reporte_dinamico').DataTable().rows().count());
            }).always(function (data, status, xhr) {
                e.container.loading('stop');
            }).fail(function (data, status, xhr) {
                console.log(data, status, xhr);
            });
        },
        ajaxRequestCallBack: function (data, callBack) {
            $.ajax({
                url: this.appBaseUrl + "campaign/filter",
                type: "GET",
                async: false,
                data: data,
                beforeSend: function () {
                    $('body').loading({ theme: 'light' });
                }
            }).done(callBack).fail(function (data, textStatus, xhr) {
                console.log(data, textStatus, xhr);
                $('body').loading('stop');
            });
        },

        loadCampaing: function () {
            $.ajax({
                url: window.location.origin + '/campaign/ConsultaCampaniasPendientes',
                type: 'GET',
                data: {},
                async: true
            }).done(function (data, textStatus, xhr) {
                var array = [];
                $.each(JSON.parse(data), function (index, data) {
                    var add = {
                        id: data.campaign_id,
                        descripcion: data.name
                    };
                    array.push(add);
                });
                $('#campanias_pendientes').empty();
                $.each(array, function (index, row) {
                    var html = ' <option value=\"' + row.id + '">' + row.descripcion + '</option>'
                    $('#campanias_pendientes').append(html);
                });
                $('#campanias_pendientes').selectpicker({
                    language: 'es',
                    title: 'No hay selección'
                }).selectpicker('refresh');
            }).fail(function (data, textStatus, xhr) {
                console.log(data, textStatus, xhr);
            });
        },
        loadCampaingLlamada: function () {
            $.ajax({
                url: window.location.origin + '/campaign/ConsultaCampaniasLlamadas',
                type: 'GET',
                data: {},
                async: true
            }).done(function (data, textStatus, xhr) {
                var array = [];
                $.each(JSON.parse(data), function (index, data) {
                    var add = {
                        id: data.id,
                        descripcion: data.descripcion
                    };
                    array.push(add);
                });
                $('#campanias_llamada').empty();
                $.each(array, function (index, row) {
                    var html = ' <option value=\"' + row.id + '">' + row.descripcion + '</option>'
                    $('#campanias_llamada').append(html);
                });
            }).fail(function (data, textStatus, xhr) {
                console.log(data, textStatus, xhr);
            });
        },

        getFilter: function () {
            var sqlRaw_Tiend = $('#builderTienda').queryBuilder('getSQL', false, true);
            var sqlRaw_Socia = $('#builderSocia').queryBuilder('getSQL', false, true);
            var sqlRaw_Conta = $('#builderContacto').queryBuilder('getSQL', false, true);
            var sqlRaw_Membr = $('#builderMembresia').queryBuilder('getSQL', false, true);
            var sqlRaw_Compa = $('#builderCampana').queryBuilder('getSQL', false, true);
            var sqlRaw_Produ = $('#builderProducto').queryBuilder('getSQL', false, true);
            var sqlRaw_Segme = $('#builderSegmento').queryBuilder('getSQL', false, true);

            var rawSQL = ' 1 = 1 ';
            var valueSQL = rawSQL.concat(
                sqlRaw_Tiend.sql.length > 0 ? ' AND ' : ' ',
                sqlRaw_Tiend.sql, sqlRaw_Socia.sql.length > 0 ? ' AND ' : ' ',
                sqlRaw_Socia.sql, sqlRaw_Conta.sql.length > 0 ? ' AND ' : ' ',
                sqlRaw_Conta.sql, sqlRaw_Membr.sql.length > 0 ? ' AND ' : ' ',
                sqlRaw_Membr.sql, sqlRaw_Compa.sql.length > 0 ? ' AND ' : ' ',
                sqlRaw_Compa.sql, sqlRaw_Produ.sql.length > 0 ? ' AND ' : ' ',
                sqlRaw_Produ.sql, sqlRaw_Segme.sql.length > 0 ? ' AND ' : ' ',
                sqlRaw_Segme.sql
            );

            return valueSQL;
        }

    };
    queryFilter.init();
}));

/// funciones adicionales
function solonumeros() {
    if (event.keyCode < 45 || event.keyCode > 57 || event.keyCode === 45 || event.keyCode === 46 || event.keyCode === 47)
        event.returnValue = false;
};

function validaMayorCero(input) {
    if (parseInt(input.value.trim()) <= 0)
        input.value = "";
    else { // Hace la suma de llamadas
        var table = $('#tabla_usuarios_llamadas').DataTable();
        var sum = 0;
        var numReg = $('#tabla_reporte_dinamico').DataTable().rows().count();
        table
            .column(5)
            .data()
            .each(function (value, index) {
                var valor = $('#txtLlamada' + value.id).val().trim();
                if (valor !== "") {
                    sum = sum + parseInt(valor);
                }
            });
        // Debe verificar que la suma no exceda el total de registros
        if (sum <= numReg) {
            $('#lblTotaLlamadas').text(sum);
            if (sum > 0) {
                $("div[data-valmsg-for=tabla_usuarios_llamadas").addClass('hidden');
                $("[name=tabla_usuarios_llamadas]").removeClass('is-invalid');
            }
            else {
                $("div[data-valmsg-for=tabla_usuarios_llamadas").removeClass('hidden');
                $("[name=tabla_usuarios_llamadas]").addClass('is-invalid');
            }
        }
        else
            input.value = "";
    }
};

function cargaResumen() {
    $('#span-num_datos_sql').text('');
    console.clear();
    var sqlRaw_Tiend = $('#builderTienda').queryBuilder('getSQL', false, true);
    var sqlRaw_Socia = $('#builderSocia').queryBuilder('getSQL', false, true);
    var sqlRaw_Conta = $('#builderContacto').queryBuilder('getSQL', false, true);
    var sqlRaw_Membr = $('#builderMembresia').queryBuilder('getSQL', false, true);
    var sqlRaw_Compa = $('#builderCampana').queryBuilder('getSQL', false, true);
    var sqlRaw_Produ = $('#builderProducto').queryBuilder('getSQL', false, true);
    var sqlRaw_Segme = $('#builderSegmento').queryBuilder('getSQL', false, true);

    var rawSQL = '  1 = 1 ';
    var value = rawSQL.concat(
        sqlRaw_Tiend.sql.length > 0 ? ' AND ' : ' ',
        sqlRaw_Tiend.sql, sqlRaw_Socia.sql.length > 0 ? ' AND ' : ' ',
        sqlRaw_Socia.sql, sqlRaw_Conta.sql.length > 0 ? ' AND ' : ' ',
        sqlRaw_Conta.sql, sqlRaw_Membr.sql.length > 0 ? ' AND ' : ' ',
        sqlRaw_Membr.sql, sqlRaw_Compa.sql.length > 0 ? ' AND ' : ' ',
        sqlRaw_Compa.sql, sqlRaw_Produ.sql.length > 0 ? ' AND ' : ' ',
        sqlRaw_Produ.sql, sqlRaw_Segme.sql.length > 0 ? ' AND ' : ' ',
        sqlRaw_Segme.sql
    );

    $('#txtResumen').val(value);
    $('.btn-apply').removeClass('hidden');
}

function formatoCantidad(cantidad) {
    while (/(\d+)(\d{3})/.test(cantidad.toString())) {
        cantidad = cantidad.toString().replace(/(\d+)(\d{3})/, '$1' + ',' + '$2');
    }
    return cantidad;
};

$(function () {
    $('input[name="rbCampania"]:radio').change(function () {
        var opcion = ($("input[name='rbCampania']:checked").val());
        if (opcion === "nueva") {
            $('#divNuevaCampania').css("display", "");
            $('#divListCampania').css("display", "none");
        }
        else {
            $('#divListCampania').css("display", "");
            $('#divNuevaCampania').css("display", "none");
        }
    });

    $('input[name="rbCampaniaLlamada"]:radio').change(function () {
        var opcion = ($("input[name='rbCampaniaLlamada']:checked").val());
        if (opcion === "nueva") {
            $('#divNuevaCampaniaLlamada').css("display", "");
            $('#divListCampaniaLlamada').css("display", "none");
        }
        else {
            $('#divListCampaniaLlamada').css("display", "");
            $('#divNuevaCampaniaLlamada').css("display", "none");
        }
    });

    var table = null;
    var currentRow = null;
    var currentRowSelected = null;
    var currentRowData = null;

    //Eventos de Guardar Campaña y Lista
    var campain = new Object();
    campain.urlAPI = {
        url_campain_aceptar: 'Campaign/Aceptar',
        url_campain_aceptar_llamada: 'Campaign/AceptarLlamada'
    };

    campain.webControls = {
        hidQuery: $('#hidQuery'),
        txtNombre_lista: $('#txtNombre_lista'),
        txtPermiso_recordatorio: $('#txtPermiso_recordatorio'),
        txtNombre_campania: $('#txtNombre_campania'),
        txtNombre_campania_llamada: $('#txtNombre_campania_llamada'),
        campanias_pendientes: $('#campanias_pendientes'),
        txtAsunto: $('#txtAsunto'),
        txtCorreo_responder: $('#txtCorreo_responder'),
        txtNombre_responder: $('#txtNombre_responder'),
        btnAceptar: $('#btnAceptar'),
        table_usuarios_llamadas: $('#tabla_usuarios_llamadas'),
        lblTotaLlamadas: $('#lblTotaLlamadas'),
        txtScript: $('#txtScript'),
        campanias_llamada: $('#campanias_llamada'),
        btnAceptarLlamada: $('#btnAceptarLlamada')
    };

    campain.iniciar = function () {
        this.agregarEventos();
    };

    campain.Datos = {
        nombre_lista: false,
        permiso_recordatorio: false,
        nombre_campania: false,
        nombre_campania_llamada: false,
        campanias_pendientes: $('#campanias_pendientes'),
        campanias_llamada: $('#campanias_llamada'),
        asunto: false,
        correo_responder: false,
        nombre_responder: false,
        script: false
    };

    campain.agregarEventos = function () {
        this.webControls.txtNombre_lista.change(function () {
            campain.Datos.nombre_lista = campain.Requerido(this, 'text');
        });
        this.webControls.txtPermiso_recordatorio.change(function () {
            campain.Datos.permiso_recordatorio = campain.Requerido(this, 'text');
        });
        this.webControls.txtNombre_campania.change(function () {
            campain.Datos.nombre_campania = campain.Requerido(this, 'text');
        });
        this.webControls.txtNombre_campania_llamada.change(function () {
            campain.Datos.nombre_campania_llamada = campain.Requerido(this, 'text');
        });
        this.webControls.campanias_pendientes.change(function () {
            campain.Datos.campanias_pendientes = campain.Requerido(this, 'select');
        });
        this.webControls.campanias_llamada.change(function () {
            campain.Datos.campanias_llamada = campain.Requerido(this, 'select');
        });
        this.webControls.txtAsunto.change(function () {
            campain.Datos.asunto = campain.Requerido(this, 'text');
        });
        this.webControls.txtCorreo_responder.change(function () {
            campain.Datos.correo_responder = campain.Requerido(this, 'email');
        });
        this.webControls.txtNombre_responder.change(function () {
            campain.Datos.nombre_responder = campain.Requerido(this, 'text');
        });

        this.webControls.btnAceptar.click(function () {
            campain.webControls.txtNombre_lista.trigger('change');
            campain.webControls.txtPermiso_recordatorio.trigger('change');
            campain.webControls.txtNombre_campania.trigger('change');
            campain.webControls.campanias_pendientes.trigger('change');
            campain.webControls.txtAsunto.trigger('change');
            campain.webControls.txtCorreo_responder.trigger('change');
            campain.webControls.txtNombre_responder.trigger('change');
            var opcion = ($("input[name='rbCampania']:checked").val());
            var _valido = campain.Datos;
            if (opcion === "nueva") { // Proviene de Nombre de la Lista
                if (_valido.nombre_lista === false ||
                    _valido.permiso_recordatorio === false ||
                    _valido.nombre_campania === false ||
                    _valido.asunto === false ||
                    _valido.correo_responder === false ||
                    _valido.nombre_responder === false
                ) {
                    return;
                }
            }
            else {      // Proviene del Combo de Campañas Pendientes
                if (_valido.nombre_lista === false ||
                    _valido.permiso_recordatorio === false ||
                    _valido.campanias_pendientes === false ||
                    _valido.asunto === false ||
                    _valido.correo_responder === false ||
                    _valido.nombre_responder === false
                ) {
                    return;
                }
            }

            // Proceso de Guardar Campaña
            var _data = campain.obtenerDatos();
            campain.Guardar(_data, campain.urlAPI.url_campain_aceptar);
            return;
        });

        this.webControls.btnAceptarLlamada.click(function () {
            campain.webControls.txtNombre_campania_llamada.trigger('change');
            campain.webControls.campanias_llamada.trigger('change');
            campain.webControls.txtScript.trigger('change');
            var opcion = ($("input[name='rbCampaniaLlamada']:checked").val());
            var _valido = campain.Datos;
            if (opcion === "nueva") { // Proviene de Nombre de la Lista
                if (_valido.nombre_campania_llamada === false) {
                    return;
                }
            }
            else {      // Proviene del Combo de Campañas Llamadas
                if (_valido.campanias_llamada === false) {
                    return;
                }
            }
            // Valida que haya llamadas asignadas en el grid  
            var table = $('#tabla_usuarios_llamadas').DataTable();
            var suma = 0;
            table
                .column(5)
                .data()
                .each(function (value, index) {
                    var Llamada = $('#txtLlamada' + value.id).val().trim();
                    if (Llamada !== "") {
                        suma = suma + parseInt(Llamada);
                    }
                });
            if (suma === 0) {
                $("div[data-valmsg-for=tabla_usuarios_llamadas").removeClass('hidden');
                $("[name=tabla_usuarios_llamadas]").addClass('is-invalid');
                return;
            }
            else {
                $("div[data-valmsg-for=tabla_usuarios_llamadas").addClass('hidden');
                $("[name=tabla_usuarios_llamadas]").removeClass('is-invalid');
            }
            // Valida que tengo el texto Script
            if (campain.webControls.txtScript.val().trim() === "") {
                $("div[data-valmsg-for=txtScript").removeClass('hidden');
                $("[name=txtScript]").addClass('is-invalid');
                return;
            }
            else {
                $("div[data-valmsg-for=txtScript").addClass('hidden');
                $("[name=txtScript]").removeClass('is-invalid');
            }
            // Proceso de Guardar Llamada
            var _data = campain.obtenerDatosLlamada();
            campain.GuardaLlamada(_data, campain.urlAPI.url_campain_aceptar_llamada);
            return;
        });
    }

    campain.Requerido = function (thisObj, type) {
        var valido = true;
        $("div[data-valmsg-for=" + thisObj.name + "]").addClass('hidden');
        if (type === 'text') {
            if (thisObj.value.length < 3) {
                $("div[data-valmsg-for=" + thisObj.name + "]").removeClass('hidden');
                $("[name=" + thisObj.name + "]").addClass('is-invalid');
                valido = false;
            }
        }
        if (type === 'email') {
            var vregexNaix = /^([\w-\.]+)\u0040((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$/;
            if (!vregexNaix.test(thisObj.value) || thisObj.value.length < 5) {
                $("div[data-valmsg-for=" + thisObj.name + "]").removeClass('hidden');
                $("[name=" + thisObj.name + "]").addClass('is-invalid');
                valido = false;
            }
        }
        if (type === 'select') {
            if (thisObj.value.length < 1) {
                $("div[data-valmsg-for=" + thisObj.name + "]").removeClass('hidden');
                valido = false;
            }
        }
        if (valido === true) {
            $("[name=" + thisObj.name + "]").removeClass('is-invalid');
            $("[name=" + thisObj.name + "]").addClass('is-valid');
        }
        return valido;
    };

    campain.obtenerDatos = function () {
        var nombreCampania, campaniaPendiente;
        var opcion = ($("input[name='rbCampania']:checked").val());
        if (opcion === "nueva")  // Proviene de Nombre de la Campaña
            nombreCampania = this.webControls.txtNombre_campania.val().trim();
        else {
            campaniaPendiente = this.webControls.campanias_pendientes.val().trim(); // Proviene del combo de campañas pendientes
            nombreCampania = this.webControls.campanias_pendientes.text().trim(); // Proviene del combo de campañas pendientes
        }
        var datos = {
            nombre_lista: this.webControls.txtNombre_lista.val().trim(),
            permiso_recordatorio: this.webControls.txtPermiso_recordatorio.val().trim(),
            nombre_campania: nombreCampania,
            campania_pendiente: campaniaPendiente,
            asunto: this.webControls.txtAsunto.val().trim(),
            correo_responder: this.webControls.txtCorreo_responder.val().trim(),
            nombre_responder: this.webControls.txtNombre_responder.val().trim(),
            query: this.webControls.hidQuery.val()
        }
        return datos;
    };

    campain.Guardar = function (datos, urlDestino) {
        // Proceso de Guardar Campaña
        $.ajax({
            url: urlDestino,
            type: 'POST',
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify(datos),
            beforeSend: function () {
                $("#loading").show(); // Carga el loading
                campain.webControls.btnAceptar.attr('disabled', true); // Deshabilita el botón e impide hacer el doble click
            },
        }).done(function (data, textStatus, xhr) {
            if (data.Success === false) {
                $("#crear").show(); // Carga el popup
                $("#loading").hide(); // Cierra el loading
                swal({
                    title: "Alerta",
                    text: data.Message,
                    icon: "warning",
                    button: "Aceptar",
                    allowOutsideClick: false
                });
            }
            if (data.Success === true) {
                $("#loading").hide(); // Cierra el loading
                $("#crear").modal('toggle'); // Cierra el Popup
                swal({
                    title: "Atención",
                    text: data.Message,
                    icon: "success",
                    button: "Aceptar",
                    allowOutsideClick: false
                });
            }
            campain.webControls.btnAceptar.attr('disabled', false); // Habilita el botón
            campain.limpiar();

        }).fail(function (data, textStatus, xhr) {
            console.log(data, textStatus, xhr);
            $("#loading").hide(); // Cierra el loading
            $("#crear").show(); // Carga el popup
            campain.webControls.btnAceptar.attr('disabled', false); // Habilita el botón
        });
    };

    campain.limpiar = function () {
        this.webControls.txtNombre_lista.val("");
        this.webControls.txtPermiso_recordatorio.val("");
        this.webControls.txtNombre_campania.val("");
        this.webControls.txtAsunto.val("");
        this.webControls.txtCorreo_responder.val("");
        this.webControls.txtNombre_responder.val("");
        // Elementos de Generar Llamadas
        this.webControls.txtNombre_campania_llamada.val("");
        this.webControls.lblTotaLlamadas.text("");
        this.webControls.txtScript.val("");
        // Limpia los TextBox del DataTable de Usuarios Llamada
        var table = $('#tabla_usuarios_llamadas').DataTable();
        table
            .column(5)
            .data()
            .each(function (value, index) {
                $('#txtLlamada' + value.id).val("");
            });
    };

    // Carga Grid de Usuarios Llamadas
    campain.loadGridUsuariosLlamadas = function () {
        $.ajax({
            url: "campaign/ConsultaUsuariosLlamadas",
            type: "GET",
            data: {},
            async: false,
        }).done(function (data, status, xhr) {
            var rData = JSON.parse(data);
            table = $("#tabla_usuarios_llamadas").DataTableDynamic({
                aaData: rData,
                info: true,
                rowId: 'id',
                showTotal: false,
                scrollX: true,
                scrollCollapse: true,
                paging: false,
                dom: 'Bfrtip',
                aoColumnDefs: [
                    {
                        targets: [0, 4],
                        visible: false,
                    },
                    {
                        targets: 5,
                        mData: null,
                        mRender: function (data, type, full) {
                            return '<input type="text" id="txtLlamada' + data.id + '" size="10" maxlength="3" onKeypress="solonumeros();" onchange="validaMayorCero(this);" />';
                        }
                    }
                ]
            }).on('click', 'tbody tr', function () {

            });

        }).always(function (data, status, xhr) {

        }).fail(function (data, status, xhr) {
            console.log(data, status, xhr);
        });
    };

    $('#txtScript').change(function () {
        if (this.value.length < 1) {
            $("div[data-valmsg-for=txtScript").removeClass('hidden');
            $("[name=txtScript]").addClass('is-invalid');
        }
        else {
            $("div[data-valmsg-for=txtScript").addClass('hidden');
            $("[name=txtScript]").removeClass('is-invalid');
        }
    });

    campain.obtenerDatosLlamada = function () {
        var nombreCampania, campaniaLlamada, usuarios_llamada;
        var opcion = ($("input[name='rbCampaniaLlamada']:checked").val());
        if (opcion === "nueva")  // Proviene de Nombre de la Campaña
            nombreCampania = this.webControls.txtNombre_campania_llamada.val().trim();
        else {
            campaniaLlamada = this.webControls.campanias_llamada.val().trim(); // Proviene del combo de campañas llamadas
            nombreCampania = this.webControls.campanias_llamada.text().trim(); // Proviene del combo de campañas llamadas
        }
        var prData = [];

        var table = $('#tabla_usuarios_llamadas').DataTable();
        table
            .column(5)
            .data()
            .each(function (value, index) {
                var Llamada = $('#txtLlamada' + value.id).val().trim();
                console.log('Data in index: ' + index + ' is: ' + Llamada);
                if (Llamada !== "") {
                    value.Llamada = Llamada;
                }
            });
        currentRowData = table.data();

        $.each(currentRowData, function (index, value) {
            if (value.Llamada !== null && value.Llamada !== "") {
                prData.push(value); // Solo va agregar los que tienen Llamada
            }
        });

        var datos = {
            nombre_campania: nombreCampania,
            campania_llamada: campaniaLlamada,
            query: this.webControls.hidQuery.val(),
            script: this.webControls.txtScript.val().trim(),
            usuarios: prData,
        }
        return datos;
    };

    campain.GuardaLlamada = function (datos, urlDestino) {
        console.log(datos);
        // Proceso de Guardar Campaña
        $.ajax({
            url: urlDestino,
            type: 'POST',
            dataType: "json",
            //contentType: "application/json; charset=utf-8",
            data: datos,
            beforeSend: function () {
                //$('body').loading({ theme: 'light' });
                $("#loading").show(); // Carga el loading
                campain.webControls.btnAceptarLlamada.attr('disabled', true); // Deshabilita el botón e impide hacer el doble click
            },
        }).done(function (data, textStatus, xhr) {
            if (data.Success === false) {
                $("#crear_llamada").show(); // Carga el popup
                $("#loading").hide(); // Cierra el loading
                swal({
                    title: "Alerta",
                    text: data.Message,
                    icon: "warning",
                    button: "Aceptar",
                    allowOutsideClick: false
                });
            }
            if (data.Success === true) {
                $("#loading").hide(); // Cierra el loading
                $("#crear_llamada").modal('toggle'); // Cierra el Popup
                swal({
                    title: "Atención",
                    text: data.Message,
                    icon: "success",
                    button: "Aceptar",
                    allowOutsideClick: false
                });
            }
            campain.webControls.btnAceptarLlamada.attr('disabled', false); // Habilita el botón
            campain.limpiar();

            //$('body').loading('stop');

        }).fail(function (data, textStatus, xhr) {
            console.log(data, textStatus, xhr);
            $("#loading").hide(); // Cierra el loading
            $("#crear_llamada").show(); // Carga el popup
            campain.webControls.btnAceptarLlamada.attr('disabled', false); // Habilita el botón
        });
    };

    campain.iniciar();
    global = campain;
});