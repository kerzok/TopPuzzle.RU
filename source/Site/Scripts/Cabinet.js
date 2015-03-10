var settingsForm = -1;
var lastCurrentPage = 1;
function appendPage(i, currentPage) {
    if (i === currentPage) {
        lastCurrentPage = i;
        return "<li> <span class=\"pagerNumber active\">" + i + "</span></li>\n";
    } else {
        return "<li> <span class=\"pagerNumber\">" + i + "</span></li>\n";
    }
}

function fillPager(pageCount, currentPage) {
    $("#pager").empty();
    var result = "";
    if (pageCount <= 5) {
        for (var i = 0; i < pageCount; i++) {
            result += appendPage(i + 1, currentPage);
        }
    } else if (currentPage < 4) {
        for (i = 0; i < 4; i++) {
            result += appendPage(i + 1, currentPage);
        }
        result += "<li><span>...</span></li>\n";
        result += appendPage(pageCount, currentPage);
    } else if (pageCount - currentPage < 3) {
        result += appendPage(1, currentPage);
        result += "<li><span>...</span></li>\n";
        for (i = pageCount - 4; i < pageCount; i++) {
            result += appendPage(i + 1, currentPage);
        }
    } else {
        result += appendPage(1, currentPage);
        result += "<li><span>...</span></li>\n";
        for (i = currentPage - 2; i < currentPage + 1; i++) {
            result += appendPage(i + 1, currentPage);
        }
        result += "<li><span>...</span></li>\n";
        result += appendPage(pageCount, currentPage);
    }
    $("#pager").append("<ul class=\"horizontal-navlist\" id=\"pager-navlist\">" + result + "</ul>");
}

var getDataToCatalog = function (page) {
    var userId = $("#user-id").val();
    $.ajax({
        url: "/cabinet/default",
        type: "POST",
        datatype: "JSON",
        data: { CurrentPage: page, UserId: userId },
        beforeSend: function () {
            $("#placeholder").show();
        },
        success: function (data) {
            $("#placeholder").hide();
            $("#partial").empty();
            $("#partial").append(data.view);
            fillPager(data.PageCount, data.CurrentPage);
            $(".pagerNumber").click(function (e) {
                getDataToCatalog(e.target.textContent.replace(/[^0-9]+/, "").trim());
            });
        }
    });
}

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
        var ok = true;
        switch (id) {
            case "old-password-text-field":
                if (val.length >= 6) {
                    $(this).addClass("not_error");
                    $(this).css("border-color", "green");
                } else {
                    $("#message-box").empty();
                    $("#message-box").append("Старый пароль слишком короткий!");
                    $(this).removeClass("not_error").addClass("error");
                    $(this).css("border-color", "red");
                    ok = false;
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
                    $("#message-box").empty();
                    $("#message-box").append("Пароли не совпадают!");
                    $("#new-password-text-field").removeClass("not_error").addClass("error");
                    $("#new-password-text-field").css("border-color", "red");
                    $("#confirm-password-text-field").removeClass("not_error").addClass("error");
                    $("#confirm-password-text-field").css("border-color", "red");
                    ok = false;
                }
        }
        if (ok) {
            $("#message-box").empty();
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
                dataType: "HTML",
                beforeSend: function (xhr, textStatus) {
                    $("#change-password-form :input").attr("disabled", "disabled");
                },

                success: function (data) {
                    $("#settings-partial").empty();
                    $("#settings-partial").append(data);
                    changePassword();
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
        var ok = true;
        switch (id) {
        case "change-email-input":
            var regex = /^([a-zA-Z0-9_.-])+@([a-zA-Z0-9_.-])+\.([a-zA-Z])+([a-zA-Z])+/;
            if (val.length > 5 && regex.test(val)) {
                $(this).addClass("not_error");
                $(this).css("border-color", "green");
            } else {
                $("#message-box").empty();
                $("#message-box").append("неверный e-mail!");
                $(this).removeClass("not_error").addClass("error");
                $(this).css("border-color", "red");
                ok = false;
            }
            break;
        }
        if (ok) {
            $("#message-box").empty();
        }
    });
    $("#change-email-form").submit(function(e) {
        e.preventDefault();
        if ($(".not_error").length === 1) {
            $.ajax({
                url: "/cabinet/settings/default",
                type: "post",
                data: $(this).serialize(),
                dataType: "HTML",
                beforeSend: function(xhr, textStatus) {
                    $("#change-email-form :input").attr("disabled", "disabled");
                },
                success: function (data) {
                    $("#settings-partial").empty();
                    $("#settings-partial").append(data);
                    changeEmail();
                }
            });
        } else {
            return false;
        }
        return false;
    });
};

$(document).ready(function() {
    getDataToCatalog(1);
    $("#my-puzzle").addClass("active");

    $("#my-puzzle").click(function () {
        getDataToCatalog(lastCurrentPage);
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