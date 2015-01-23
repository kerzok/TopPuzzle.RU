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
        data: { page: page},
        success: function (data) {
            $("#catalog").empty();
            $("#catalog").append(data.view);
            fillPager(data.PageCount, data.CurrentPage);
            jQuery("div[id=media]").each(function () {
                $(this).click(function() {
                    showPopup();
                    pictureId = $(this).attr("picture_id");
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
    getDataToCatalog(1);

    $("#random").click(function () {
        $.post("/home/random", null, function() {
            $("#random").removeAttr("disabled");
            getDataToCatalog(lastCurrentPage);
        });
    });

    $(".complexity").click(function () {
        var complexity = $(this).attr("complexity");
        $("#scores").empty();
        $("#scores").load("/scores", { "complexity": complexity }, null);
    });

    $("#easy, #medium, #hard").click(function() {
        var complexity = $(this).attr("complexity");
        $.post("/puzzle/get", { "complexity": complexity, "pictureId": pictureId });
    });
});

