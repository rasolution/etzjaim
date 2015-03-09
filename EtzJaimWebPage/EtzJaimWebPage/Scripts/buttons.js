
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
                alert(data);
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
                alert(data);
            },
            error: function () {
                alert("An error has occured!!!");
            }
        });
    });
});