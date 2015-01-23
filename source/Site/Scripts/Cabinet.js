var settingsForm = -1;

function changeActive(num) {
    switch (num) {
        case 1:
            $("#navlist-email").addClass("active");
            $("#navlist-password").removeClass("active");
            $("#navlist-avatar").removeClass("active");
            break;
        case 2:
            $("#navlist-password").addClass("active");
            $("#navlist-email").removeClass("active");
            $("#navlist-avatar").removeClass("active");
            break;
        case 3:
            $("#navlist-avatar").addClass("active");
            $("#navlist-password").removeClass("active");
            $("#navlist-email").removeClass("active");
            break;
    }
}

function readURL(input) {
    if (input.files && input.files[0]) {
        var reader = new FileReader();
        reader.onload = function(e) {
            $("#previewHolder").attr("src", e.target.result);
        }
        $("#previewHolder").css("display", "inline-block");
        reader.readAsDataURL(input.files[0]);
    } else {
        $("#previewHolder").css("display", "none");
    }
}

var changeAvatar = function () {
    changeActive(3);
    $("#file").change(function() {
        readURL(this);
    });
    $("#change-avatar-form").submit(function (e) {
        e.preventDefault();
        var fd = new FormData();
        fd.append("file", $("#file")[0].files[0]);
        var val12 = $(this).serialize();
        $.ajax({
            url: "/cabinet/settings/avatar",
            type: "post",
            data: fd,
            processData: false,
            contentType: false,

            beforeSend: function (xhr, textStatus) {
                $("#change-email-form :input").attr("disabled", "disabled");
                $("#previewHolder").css("display", "none");
            },

            success: function (response) {
                $("#change-email-form :input").removeAttr("disabled");
                var timestamp = new Date().getTime();
                $("#avatar-img").attr("src", $("#avatar-img").attr("src") + "?" + timestamp);
                if (response.data === "ok") {
                    $("#message-box").text("Аватар был успешно изменен!");
                } else {
                    $("#message-box").text("Упс, произошла ошибка:(");
                }
            }
        });
    });
};

var changePassword = function () {
    changeActive(2);
    $("#old-password-text-field, #new-password-text-field, #confirm-password-text-field").unbind().blur(function () {
        var id = $(this).attr("id");
        var val = $(this).val();
        switch (id) {
            case "old-password-text-field":
                if (val.length >= 6) {
                    $(this).addClass("not_error");
                    $(this).css("border-color", "green");
                } else {
                    $(this).removeClass("not_error").addClass("error");
                    $(this).css("border-color", "red");
                }
                break;
            case "new-password-text-field":
            case "confirm-password-text-field":
                var newPas = $("#new-password-text-field").val();
                if (val.length >= 6 && val === newPas) {
                    $("#new-password-text-field").addClass("not_error");
                    $("#new-password-text-field").css("border-color", "green");
                    $("#confirm-password-text-field").addClass("not_error");
                    $("#confirm-password-text-field").css("border-color", "green");
                } else {
                    $("#new-password-text-field").removeClass("not_error").addClass("error");
                    $("#new-password-text-field").css("border-color", "red");
                    $("#confirm-password-text-field").removeClass("not_error").addClass("error");
                    $("#confirm-password-text-field").css("border-color", "red");
                }
        }
    });
    $("#change-password-form").submit(function (e) {
        e.preventDefault();
        var val3 = $(this).serialize();
        if ($(".not_error").length === 3) {
            $.ajax({
                url: "/cabinet/settings/password",
                type: "post",
                data: $(this).serialize(),

                beforeSend: function (xhr, textStatus) {
                    $("#change-password-form :input").attr("disabled", "disabled");
                },

                success: function (response) {
                    $("#change-password-form :input").removeAttr("disabled");
                    if (response.data === "ok") {
                        $("#message-box").text("Пароль был успешно изменен!");
                    } else {
                        $("#message-box").text(response.data);
                    }
                }
            });
        } else {
            return false;
        }
        return false;
    });
};

var changeEmail = function () {
    changeActive(1);
    $("#change-email-input").unbind().blur(function() {
        var id = $(this).attr("id");
        var val = $(this).val();
        switch (id) {
        case "change-email-input":
            var regex = /^([a-zA-Z0-9_.-])+@([a-zA-Z0-9_.-])+\.([a-zA-Z])+([a-zA-Z])+/;
            if (val.length > 5 && regex.test(val)) {
                $(this).addClass("not_error");
                $(this).css("border-color", "green");
            } else {
                $(this).removeClass("not_error").addClass("error");
                $(this).css("border-color", "red");
            }
            break;
        }
    });
    $("#change-email-form").submit(function(e) {
        e.preventDefault();
        if ($(".not_error").length === 1) {
            $.ajax({
                url: "/cabinet/settings/default",
                type: "post",
                data: $(this).serialize(),

                beforeSend: function(xhr, textStatus) {
                    $("#change-email-form :input").attr("disabled", "disabled");
                },

                success: function(response) {
                    $("#change-email-form :input").removeAttr("disabled");
                    if (response.data === "ok") {
                        $("#message-box").text("E-mail был успешно изменен!");
                    } else {
                        $("#message-box").text("Упс, произошла ошибка:(");
                    }
                }
            });
        } else {
            return false;
        }
        return false;
    });
};

$(document).ready(function() {
    $("#partial").load("/cabinet/default");
    $("#my-puzzle").addClass("active");

    $("#my-puzzle").click(function() {
        $("#partial").load("/cabinet/default");
        $("#my-puzzle").addClass("active");
        $("#settings").removeClass("active");
    });

    $("#settings").click(function () {
        $("#my-puzzle").removeClass("active");
        $("#settings").addClass("active");
        $.get("/cabinet/settings", function(result) {
            $("#partial").empty();
            $("#partial").append(result);
            $("#settings-partial").load("/cabinet/settings/default", null, changeEmail);

            $("#navlist-email").click(function() {
                $("#settings-partial").load("/cabinet/settings/default", null, changeEmail);
            });

            $("#navlist-password").click(function () {
                $("#settings-partial").load("/cabinet/settings/password", null, changePassword);
            });

            $("#navlist-avatar").click(function () {
                $("#settings-partial").load("/cabinet/settings/avatar", null, changeAvatar);
            });
        });
    });
});