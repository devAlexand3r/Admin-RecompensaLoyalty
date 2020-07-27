
var global = {};
var objectRow = null;
$(function (e) {
    var main = {
        URL_CREATE_USER: appBaseUrl + 'api/Update/CreateUser',

        firstName: $('#first_name'),
        middleName: $('#middle_name'),
        lastName: $('#last_name'),
        age: $('#age'),
        email: $('#email'),
        btnCreate: $('#btnNewUser'),

        init: function () {

            //Create user, registro
            main.btnCreate.click(function () {
                var obj = {
                    FistName: main.firstName.val(),
                    MiddleName: main.middleName.val(),
                    LastName: main.lastName.val(),
                    Age: main.age.val(),
                    Email: main.email.val(),
                };

                main.createUser(obj);
            });
        },
        createUser: function (objdata) {
            $.ajax({
                url: main.URL_CREATE_USER,
                type: 'POST',
                data: objdata
            }).done(function (result, textStatus, xhr) {
                main.printMenssage(result);
                if (result.Success === true) {
                    $(':input', '#formRegistro')
                        .not(':button, :submit, :reset, :hidden')
                        .val('')
                        .removeAttr('checked')
                        .removeAttr('selected');
                }
            }).fail(function (data, textStatus, xhr) {
                console.log(data, textStatus, xhr);
            });
        },
        printMenssage(data) {
            if (data.Success === true) {
                $(".close").click();

                $.notify({
                    icon: 'glyphicon glyphicon-ok',
                    message: data.Message,
                });
            } else {
                $.notify({
                    icon: 'glyphicon glyphicon-remove',
                    message: data.Message,
                });
            }
        }
    };

    global.main = main;
    main.init();

});