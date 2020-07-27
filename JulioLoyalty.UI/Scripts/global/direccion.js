var direccion = {
    url_buscar_sexo: appBaseUrl + '/api/Direccion/Sexo',
    url_buscar_estados: appBaseUrl + '/api/Direccion/Estados',
    url_buscar_municipios: appBaseUrl + '/api/Direccion/Municipios',
    url_buscar_colonias: appBaseUrl + '/api/Direccion/Colonias',
    url_buscar_codigopostal: appBaseUrl + '/api/Direccion/CodigoPostal',
    url_buscar_listaOcupacion: appBaseUrl + '/api/Direccion/Lista/Ocupacion',
    selectGenero: $('#genero'),
    selectEstado: $('#direccion_estado'),
    selectMunicipio: $('#direccion_municipio'),
    selectColonia: $('#direccion_colonia'),
    selectOpcuacion: $('#selOcupacion'),
    inputCodPostal: $('#CodigoPostal'),
    btnBusquedaDir: $('#direccion_busqueda'),
    iniciar: function () {
        $(".numeric").on("keypress keyup blur", function (event) {
            $(this).val($(this).val().replace(/[^\d].+/, ""));
            if ((event.which < 48 || event.which > 57)) {
                event.preventDefault();
            }
        });
        this.obtenerSexo();
        this.obtenerListaOcupacion();
        this.obtenerEstados();
        this.selectEstado.on('change', function () {
            direccion.obtenerMunicipios(this.value);
        });
        this.selectMunicipio.on('change', function () {
            direccion.obtenerColonias(this.value);
        });
        this.selectColonia.on('change', function () {
            direccion.obtenerCodigoPostal(this.value);
        });
        this.btnBusquedaDir.click(function () {
            var codPostal = direccion.inputCodPostal.val();
            $('div[data-valmsg-for="CodigoPostal"]').removeClass('hidden');
            if (codPostal === "") {
                $('div[data-valmsg-for="CodigoPostal"]').text("Código postal invalido");
                return;
            }
            if (codPostal.length < 5) {
                $('div[data-valmsg-for="CodigoPostal"]').text("El código postal debe ser de 5 digitos");
                return;
            }
            $('div[data-valmsg-for="CodigoPostal"]').addClass('hidden');
            $.ajax({
                url: appBaseUrl + '/api/Direccion/BusquedaCP',
                type: 'GET',
                dataType: 'json',
                data: {
                    codigo_postal: codPostal,
                },
                success: function (data) {
                    if (data.length > 0) {
                        direccion.selectEstado.val(data[0].estado).trigger('change');
                        direccion.selectMunicipio.val(data[0].municipio).trigger('change');
                        direccion.selectColonia.val(data[0].colonia).selectpicker("refresh");
                    } else {
                    }
                }, error: function (xhr, status, error) {
                    var mensaje = "Status: " + xhr.status + " Error: " + error;
                }
            });
        });
        $('.selectpicker').selectpicker({
            language: 'es',
            title: 'No hay selección'
        }).selectpicker('refresh');
    },
    obtenerEstados: function () {
        $.ajax({
            url: this.url_buscar_estados,
            type: 'GET',
            async: false,
        }).done(function (data, textStatus, xhr) {
            direccion.selectEstado.empty();
            var html = ' <option value=\""></option>'
            direccion.selectEstado.append(html);
            $.each(data, function (index, row) {
                var html = ' <option value=\"' + row.Id + '">' + row.Descripcion + '</option>'
                direccion.selectEstado.append(html);
            });
            direccion.selectEstado.selectpicker("refresh");
        }).fail(function (data, textStatus, xhr) {
            console.log(data, textStatus, xhr);
        });
    },
    obtenerMunicipios: function (keyEstado) {
        if (keyEstado === "") {
            direccion.selectMunicipio.selectpicker("refresh");
            return;
        }
        $.ajax({
            url: this.url_buscar_municipios,
            type: 'GET',
            async: false,
            data: { estado: keyEstado }
        }).done(function (data, textStatus, xhr) {
            direccion.selectMunicipio.attr('disabled', false);
            direccion.selectMunicipio.empty();
            $.each(data, function (index, row) {
                var html = ' <option value=\"' + row.Id + '">' + row.Descripcion + '</option>'
                direccion.selectMunicipio.append(html);
            });
            direccion.selectMunicipio.selectpicker("refresh");
        }).fail(function (data, textStatus, xhr) {
            console.log(data, textStatus, xhr);
        });
    },
    obtenerColonias: function (keyMunicipio) {
        if (keyMunicipio === "") {
            direccion.selectColonia.selectpicker("refresh");
            return;
        }
        $.ajax({
            url: this.url_buscar_colonias,
            type: 'GET',
            async: false,
            data: {
                estado: this.selectEstado.val(),
                municipio: keyMunicipio,
                codigopostal: ""
            }
        }).done(function (data, textStatus, xhr) {
            direccion.selectColonia.attr('disabled', false);
            direccion.selectColonia.empty();
            $.each(data, function (index, row) {
                var html = ' <option value=\"' + row.Id + '">' + row.Descripcion + '</option>'
                direccion.selectColonia.append(html);
            });
            direccion.selectColonia.selectpicker("refresh");
        }).fail(function (data, textStatus, xhr) {
            console.log(data, textStatus, xhr);
        });
    },
    obtenerCodigoPostal: function (keyColonia) {
        $.ajax({
            url: this.url_buscar_codigopostal,
            type: 'GET',
            async: false,
            data: {
                estado: direccion.selectEstado.val(),
                municipio: direccion.selectMunicipio.val(),
                colonia: keyColonia
            }
        }).done(function (data, textStatus, xhr) {
            if (data !== null) {
                direccion.inputCodPostal.val(JSON.stringify(data.CodigoPostal).replace(/"/g, ''));
            }
        }).fail(function (data, textStatus, xhr) {
            console.log(data, textStatus, xhr);
        });
    },
    obtenerSexo: function () {
        $.ajax({
            url: this.url_buscar_sexo,
            type: 'GET',
            async: false,
        }).done(function (data, textStatus, xhr) {
            direccion.selectGenero.empty();
            $.each(data, function (index, row) {
                var html = ' <option value=\"' + row.id + '">' + row.descripcion + '</option>'
                direccion.selectGenero.append(html);
            });
            direccion.selectGenero.selectpicker("refresh");
        }).fail(function (data, textStatus, xhr) {
            console.log(data, textStatus, xhr);
        });
    },
    obtenerListaOcupacion: function () {
        $.ajax({
            url: this.url_buscar_listaOcupacion,
            type: 'GET',
            async: false,
        }).done(function (data, textStatus, xhr) {
            direccion.selectOpcuacion.empty();
            $.each(data, function (index, row) {
                var html = ' <option value=\"' + row.id + '">' + row.descripcion + '</option>'
                direccion.selectOpcuacion.append(html);
            });
            direccion.selectOpcuacion.selectpicker("refresh");
        }).fail(function (data, textStatus, xhr) {
            console.log(data, textStatus, xhr);
        });
    },
}