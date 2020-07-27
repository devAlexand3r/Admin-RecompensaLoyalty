
var $Utils = {
    addSelectOptions: function (options, select, valField, textField) {
        $(select).find('option').remove();
        for (var i = 0; i < options.length; i++) {
            $('<option/>', {
                text: options[i][textField],
                value: options[i][valField],
                data: options[i]
            }).appendTo(select);
        }
    },

    fileDownLoad: function (downloadUrl, variables, method, callback) {
        switch (method.toUpperCase()) {
            case "POST":
                if ($('#_downloadForm').length < 1) {
                    $('<form method="POST" id="_downloadForm" target="_blank" action="">').appendTo('body');
                }
                var downLoadForm = $('#_downloadForm');
                $(downLoadForm).children().remove();

                for (var i = 0; i < variables.length; i++) {
                    var hiddenField = $('<input id="' + variables[i].id + '" name="' + variables[i].id + '" type="hidden" value="' + variables[i].value + '" />');
                    downLoadForm.append(hiddenField);
                }

                downLoadForm[0].action = downloadUrl;
                downLoadForm.submit();
                break;

            case "GET":
                var downloadUrlWithParams = downloadUrl;
                var iframe;

                for (var i = 0; i < variables.length; i++) {
                    if (i === 0) {
                        downloadUrlWithParams += '?' + variables[i].id + '=' + variables[i].value;
                    }
                    else {
                        downloadUrlWithParams += '&' + variables[i].id + '=' + variables[i].value;
                    }
                }

                if ($('#_downloadiFrame').length > 0) {
                    $('#_downloadiFrame').remove();
                }

                iframe = $('<iframe id="_downloadiFrame" name="_downloadiFrame" src="' + downloadUrlWithParams + '" style="display: none;" />');
                iframe.appendTo('body');
                document.getElementsByTagName('iframe')[0].onload = function () {
                    alert('Frame element loaded');
                };

                if (callback) {
                    $(iframe).load(callback);
                }
                break;
        }
    },
    getUrlVars: function () {
        var vars = [], hash;
        var hashes = window.location.href.slice(window.location.href.indexOf('?') + 1).split('&');
        for (var i = 0; i < hashes.length; i++) {
            hash = hashes[i].split('=');
            vars.push(hash[0]);
            vars[hash[0]] = hash[1];
        }
        return vars;
    },
    convertMayusFirst: function (text) {
        return text.charAt(0).toUpperCase() + text.slice(1).toLowerCase()
    }

};


var $validate = {
    init: function (obj) {
        $("div[data-valmsg-for=" + obj.name + "]").addClass('hidden');
    },
    isvalid: function (obj) {
        $("[name=" + obj.name + "]").removeClass('is-invalid').addClass('is-valid');
        $("div[data-valmsg-for=" + obj.name + "]").addClass('hidden');
    },
    isnotvalid: function (obj) {
        $("div[data-valmsg-for=" + obj.name + "]").removeClass('hidden');
        $("[name=" + obj.name + "]").addClass('is-invalid');
    },

    email: function (obj) {
        this.init(obj);
        var vregexNaix = /^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$/;
        if (!vregexNaix.test(obj.value) || obj.value.length < 5) {
            this.isnotvalid(obj);
            return false;
        }
        this.isvalid(obj);
        return true;
    },
    card: function (obj, max) {
        let cmin = 1;
        let cmax;
        if (isNaN(max)) { cmax = 5; }
        if (obj.value.length < cmin || obj.value.length > cmax) {
            this.isnotvalid(obj);
            return false;
        }
        this.isvalid(obj);
        return true;
    },
    date: function (obj) {
        this.init(obj);
        var vregexNaix = /^(0[1-9]|[1-2]\d|3[01])(\/)(0[1-9]|1[012])\2(\d{4})$/;
        if (!vregexNaix.test(obj.value) || obj.value.length > 10) {
            this.isnotvalid(obj);
            return false;
        }
        this.isvalid(obj);
        return true;
    },
    birthdate: function (obj) {
        this.init(obj);
        var vregexNaix = /^(0[1-9]|[1-2]\d|3[01])(\/)(0[1-9]|1[012])\2(\d{4})$/;
        if (!vregexNaix.test(obj.value) || obj.value.length > 10) {
            this.isnotvalid(obj);
            return false;
        }
        this.isvalid(obj);
        return true;
    }, 
    options: function (obj) {
        this.init(obj);
        if (obj.value.length < 1) {
            this.isnotvalid(obj);
            return false;
        }
        this.isvalid(obj);
        return true;
    },
    text: function (obj, min, max) {
        let cmin, cmax;
        if (isNaN(min)) { cmin = 1; }
        if (isNaN(max)) { cmax = 1000000; }
        if (obj.value.length < cmin || obj.value.length > cmax) {
            this.isnotvalid(obj);
            return false;
        }
        this.isvalid(obj);
        return true;
    },
    telephone: function (obj, min, max) {
        let cmin, cmax;
        if (isNaN(min)) { cmin = 7; }
        if (isNaN(max)) { cmax = 10; }
        if (obj.value.length < cmin || obj.value.length > cmax) {
            this.isnotvalid(obj);
            return false;
        }
        this.isvalid(obj);
        return true;
    },
    zipcode: function (obj, max) {
        let cmin = 1;
        let cmax;
        if (isNaN(max)) { cmax = 5; }
        if (obj.value.length < 1 || obj.value.length > cmax) {
            this.isnotvalid(obj);
            return false;
        }
        this.isvalid(obj);
        return true;
    },
}


function isNumber(evt) {
    evt = (evt) ? evt : window.event;
    var charCode = (evt.which) ? evt.which : evt.keyCode;
    if (charCode > 31 && (charCode < 48 || charCode > 57)) {
        return false;
    }
    return true;
}
