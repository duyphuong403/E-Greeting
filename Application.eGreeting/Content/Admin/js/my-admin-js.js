$(document).ready(function () {
    checkValidate();
    CKEDITOR.replace('myCkeditor');
})

function checkValidate() {
    var _formId = $('#loginForm');
    if (_formId.length > 0) {
        _formId.formValidation({
            framework: 'bootstrap',
            err: {
                container: ''
            },
            icon: {
                valid: 'fa fa-check',
                invalid: 'fa fa-times',
                validating: 'fa fa-refresh'

            },
            fields: {
                UserName: {
                    message: 'Nhập không đúng định dạng',
                    validators: {
                        notEmpty: {
                            message: 'Tên tài khoản không được để trống'
                        },
                       
                        regexp: {
                            regexp: /^[a-zA-Z0-9_]+$/,
                            message: 'The username can only consist of alphabetical, number and underscore',
                        },
                    }
                },
                Password: {
                    message: 'Nhập không đúng định dạng',
                    validators: {
                        notEmpty: {
                            message: 'Mật khẩu không được để trống'
                        }, stringLength: {
                            min: 6,
                            max: 30,
                            message: 'Mat khau phai nhieu hon 6 ki tu',
                        },
                    }
                }
            }
        });
    }
}

