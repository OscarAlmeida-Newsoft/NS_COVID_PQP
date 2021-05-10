$(document).ready(function () {
    $.ajax({
        url: '/Login/ValidateUser',
        type: 'POST',
        contentType: 'application/json; charset=utf-8'
    }).done(function (data) {
        $("#divMenu").children().empty();
        $("#divMenu").append(data.Mensaje);
        if (data.Error) {
            $("#lblSession").text("Cerrar sesión");
            } else {
                $("#lblSession").text("Iniciar sesión");
            }
        })
        .fail(function (pRequest, pException) {
          
        });
});

