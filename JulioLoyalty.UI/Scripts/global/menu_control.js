
$(function (e) {

    var menu = new Object();
    menu.guardarDato = function (key, data) {
        localStorage.setItem(key, data);
    };
    menu.eliminarDato = function (key) {
        localStorage.removeItem(key);
    };
    menu.agregarEventos = function () {
        $('.logueado a').click(function () {
            menu.eliminarDato('name');
            menu.eliminarDato('menu');
            menu.eliminarDato('href');
        });

        $('#buscador input').change(function () {
            menu.eliminarDato('name');
            menu.eliminarDato('menu');
            menu.eliminarDato('href');
        });

        $('.dropdown-menu a').click(function () {
            menu.guardarDato('name', $(this).attr('name'));
            menu.guardarDato('menu', $(this).attr('menu'));
            menu.guardarDato('href', $(this).attr('href'));
        });
    };
    menu.obtenerDato = function (key) {
        return localStorage.getItem(key);
    };
    menu.agregarClase = function () {
        const name = this.obtenerDato('name');
        const menu = this.obtenerDato('menu');
        const href = this.obtenerDato('href');
        if (name !== null) {
            $('a[name=' + name + ']').addClass('active');
            $('a[name=' + menu + ']').addClass('active');
        }
        if (href != null) {
            var pathname = window.location.pathname;
            if (pathname !== href) {
                $('a[name=' + name + ']').removeClass('active');
                $('a[name=' + menu + ']').removeClass('active');
                //window.location.href = href;
            }
        }
    };
    menu.iniciar = function () {
        this.agregarEventos();
        this.agregarClase();
    };
    menu.iniciar();

});