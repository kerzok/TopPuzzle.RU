var lastCurrentPage = 1;
var pictureId = 0;
function appendPage(i, currentPage) {
    if (i === currentPage) {
        lastCurrentPage = i;
        return "<li> <span class=\"pagerNumber active\">" + i + "</span></li>\n";
    } else {
        return "<li> <span class=\"pagerNumber\">" + i + "</span></li>\n";
    }
}

function showPopup() {
    $("#popup1").show();
}

function hidePopup() {
    $("#popup1").hide();
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

var getDataToCatalog = function(page) {
    $.ajax({
        url: "/home/getdata",
        type: "POST",
        datatype: "JSON",
        data: { CurrentPage: page},
        success: function (data) {
            $("#catalog").empty();
            $("#catalog").append(data.view);
            fillPager(data.PageCount, data.CurrentPage);
            jQuery("div[id=media]").each(function () {
                $(this).click(function() {
                    pictureId = $(this).attr("picture_id");
                    showPopup();
                });
            });
            $(".pagerNumber").click(function (e) {
                getDataToCatalog(e.target.textContent.replace(/[^0-9]+/, "").trim());
            });
        }
    });
}

$(document).ready(function () {
    hidePopup();
    $("#scores").load("/scores");
    $("#c-easy").addClass("chosen-complexity");
    getDataToCatalog(1);

    $("#random").click(function () {
        $.ajax("/home/random", {
            type: "POST",
            beforeSend: function() {
                $("#random").attr("disabled", "disabled");
            },
            success: function(data) {
                $("#random").removeAttr("disabled");
                getDataToCatalog(lastCurrentPage);
                pictureId = data.Id;
                showPopup();               
            }
        });
    });

    $(".complexity").click(function () {
        var complexity = $(this).attr("complexity");
        $("#scores").empty();
        $("#scores").load("/scores", { "complexity": complexity }, null);
        switch (complexity) {
            case "1":
                $("#c-easy").addClass("chosen-complexity");
                $("#c-medium").removeClass("chosen-complexity");
                $("#c-hard").removeClass("chosen-complexity");
            break;
            case "2":
                $("#c-easy").removeClass("chosen-complexity");
                $("#c-medium").addClass("chosen-complexity");
                $("#c-hard").removeClass("chosen-complexity");
            break;
            case "3":
                $("#c-easy").removeClass("chosen-complexity");
                $("#c-medium").removeClass("chosen-complexity");
                $("#c-hard").addClass("chosen-complexity");
            break;
        }
    });

    $("#easy, #medium, #hard").click(function() {
        var complexity = $(this).attr("complexity");
        window.location.href = "puzzle?Complexity=" + complexity + "&Id=" + pictureId;
    });

    $("#close-btn").click(function() {
        hidePopup();
    });
});

