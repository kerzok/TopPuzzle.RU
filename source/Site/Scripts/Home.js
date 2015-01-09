$(document).ready(function() {
    $("#scores").load("/scores", null, null);

    $(".complexity").click(function () {
        var complexity = $(this).attr("complexity");
        $("#scores").empty();
        $("#scores").load("/scores", { "complexity": complexity }, null);
    });
});

