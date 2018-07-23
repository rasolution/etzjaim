$(document).ready(function fnButtons() {
    $("#aprobarCita").bind("click", function () {
        var cita_id = $("#cita_id").text();
        $.ajax({
            url: "/WebPage/Citas/Aprobar",
            type: "POST",
            data: JSON.stringify({ 'cita_id': cita_id }),
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                data = JSON.parse(data);
                alert(data.response);
            },
            error: function () {
                alert("An error has occured!!!");
            }
        });
    });

    $("#rechazarCita").bind("click", function () {
        var cita_id = $("#cita_id").text();
        $.ajax({
            url: "/WebPage/Citas/Rechazar",
            type: "POST",
            data: JSON.stringify({ 'cita_id': cita_id }),
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                data = JSON.parse(data);
                alert(data.response);
            },
            error: function () {
                alert("An error has occured!!!");
            }
        });
    });
    $("#spam").bind("click", function () {
        var conv_id = $("#conv_id").text();
        var user = $("#user").text();
        $.ajax({
            url: "/WebPage/Conversaciones/moveSpam",
            type: "POST",
            data: JSON.stringify({ 'username': user, 'conv_id': conv_id, }),
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                data = JSON.parse(data);
                alert(data.response);
            },
            error: function () {
                alert("An error has occured!!!");
            }
        });
    });
    $("#quitspam").bind("click", function () {
        var conv_id = $("#conv_id").text();
        var user = $("#user").text();
        $.ajax({
            url: "/WebPage/Conversaciones/NoSpam",
            type: "POST",
            data: JSON.stringify({ 'username': user, 'conv_id': conv_id, }),
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                data = JSON.parse(data);
                alert(data.response);
            },
            error: function () {
                alert("An error has occured!!!");
            }
        });
    });
    $("#enviar").bind("click", function () {
        var user = $("#user").text();
        var conv_id = $("#conv_id").text();
        var message = document.getElementById("message").value;
        $.ajax({
            url: "/WebPage/Conversaciones/Send",
            type: "POST",
            data: JSON.stringify({ 'conv_id': conv_id, 'message': message, 'username': user }),
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                pullmessajes();
            },
            error: function () {
                alert("An error has occured!!!");
            }
        });
        var message = document.getElementById("message").value = "";
    });
    $("#btneditProduct").bind("click", function () {
        var pro_id = document.getElementById("pro_id").value;
        var pro_nombre;
        var pro_precio;
        if (document.getElementById("pro_nombre").value == "") {
            pro_nombre = document.getElementById("pro_nombre").placeholder;
        }
        else {
            pro_nombre = document.getElementById("pro_nombre").value;
        }
        if (document.getElementById("pro_precio").value == "") {
            pro_precio = document.getElementById("pro_precio").placeholder;
        }
        else {
            pro_precio = document.getElementById("pro_precio").value;
        }
        var pro_estado = document.getElementById("pro_estado").value;
        $.ajax({
            url: "/WebPage/Productos/EditProduct",
            type: "POST",
            data: JSON.stringify({ 'pro_id': pro_id, 'pro_nombre': pro_nombre, 'pro_precio': pro_precio, 'pro_estado': pro_estado }),
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                data = JSON.parse(data);
                alert(data.response);
            },
            error: function () {
                alert("An error has occured!!!");
            }
        });

    });

});

function pullmessajes() {
    var conv_id = $("#conv_id").text();
    $.ajax({
        url: "/WebPage/Conversaciones/Pull",
        type: "POST",
        data: JSON.stringify({ 'conv_id': conv_id }),
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            var messages = JSON.parse(data);
            var div = "<div class='pre-scrollable' id='messages'>";
            for (var i = 0; i < messages.length; i++) {
                div = div + "<h4>" + messages[i].username + "</h4>";
                div = div + "<h6>" + messages[i].mes_fecha + "</h6>";
                div = div + "<p>" + messages[i].message + "</p><br/>";
            }
            div = div + "</div>";
            document.getElementById("messages").innerHTML = div;
        },
        error: function () {
            alert("An error has occured!!!");
        }
    });
}

function Search(busqueda) {
    $.ajax({
        url: "/WebPage/Usuarios/Ajax",
        type: "POST",
        data: JSON.stringify({ 'user': busqueda }),
        dataType: "json",
        contentType: "application/json; charset=utf-8",
    }).done(function (response) {
        var data = [];
        response = JSON.parse(response);
        for (var i = 0; i < response.length; i++) {
            data.push(response[i].username);
        }
        $("#search").autocomplete({
            source: data
        });
    });

}

