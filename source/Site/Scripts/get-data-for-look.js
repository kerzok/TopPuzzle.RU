jQuery.fn.extend({
    slideRightShow: function () {
        return this.each(function () {
            $(this).show('slide', { direction: 'right' }, 1000);
        });
    },
    slideLeftHide: function () {
        return this.each(function () {
            $(this).hide('slide', { direction: 'left' }, 1000);
        });
    },
    slideRightHide: function () {
        return this.each(function () {
            $(this).hide('slide', { direction: 'right' }, 1000);
        });
    },
    slideLeftShow: function () {
        return this.each(function () {
            $(this).show('slide', { direction: 'left' }, 1000);
        });
    }
});

$(function () {
    $(document).ready(function () {
        if ($('#inpSuggestion').val().length > 0) {
            $(".search-icon").hide();
            $(".search-sample").hide();

        }
        var sex = gup('sex');
        $(".brandSelector").chosen({
            disable_search_threshold: 10,
            no_results_text: "Ничего не найдено :(",
            placeholder_text_multiple: "Выбрать нужные бренды..."
        }).change(function () {
            searchAndFilter();
        });

        $('.categoryMenSelector').chosen({ disable_search: true }).change(function () {
            resetText();
            getBrands();
            getSizes();
            //searchAndFilter();
        });
        $('.categoryWomenSelector').chosen({ disable_search: true }).change(function () {
            resetText();
            getBrands();
            getSizes();
            //searchAndFilter();
        });
        if (sex == 1) {
            SetMen();

        } else {
            SetWomen();

        }
        var page = gup('page');
        if (page != null)
            searchAndFilter(page, true);
        else {

            searchAndFilter(page, true);
        }

        $(".brandSelector").chosen().change(function () {
            $("#brandFilterApplyBtn").show();
        });

    });
    $("#priceButton").click(function () {
        $("#discOrder").removeClass("asc");
        $("#discOrder").removeClass("desc");
        $("#discButton").removeClass("active");
        $("#priceButton").addClass("active");
        var elem = $("#priceOrder");
        if (elem.hasClass("asc")) {
            elem.removeClass("asc");
            elem.addClass("desc");
            searchAndFilter();
            return;
        }
        if (elem.hasClass("desc")) {
            elem.removeClass("desc");
            elem.addClass("asc");
            searchAndFilter();
            return;
        }

        elem.addClass("asc");
        searchAndFilter();
    });
    function gup(name) {
        name = name.replace(/[\[]/, "\\\[").replace(/[\]]/, "\\\]");
        var regexS = "[\\?&]" + name + "=([^&#]*)";
        var regex = new RegExp(regexS);
        var results = regex.exec(window.location.href);
        if (results == null)
            return null;
        else
            return results[1];
    }
    $("#discButton").click(function () {
        $("#priceOrder").removeClass("asc");
        $("#priceOrder").removeClass("desc");
        $("#discButton").addClass("active");
        $("#priceButton").removeClass("active");
        var elem = $("#discOrder");
        if (elem.hasClass("asc")) {
            elem.removeClass("asc");
            elem.addClass("desc");
            searchAndFilter();
            return;
        }
        if (elem.hasClass("desc")) {
            elem.removeClass("desc");
            elem.addClass("asc");
            searchAndFilter();
            return;
        }

        elem.addClass("desc");
        searchAndFilter();
    });
    function SetMen() {
        $('.filter-item.sex .men').addClass("active");
        $('.categoryMenSelector-wrapper').show();
        $('.filter-item.sex .women').removeClass("active");
        $('.categoryWomenSelector-wrapper').hide();
        $(".filter-item.sex .women").trigger("chosen:updated");
        $(".filter-item.sex .men").trigger("chosen:updated");
    }
    function SetWomen() {
        $('.filter-item.sex .women').addClass("active");
        $('.categoryWomenSelector-wrapper').show();
        $('.filter-item.sex .men').removeClass("active");
        $('.categoryMenSelector-wrapper').hide();

        $(".filter-item.sex .women").trigger("chosen:updated");
        $(".filter-item.sex .men").trigger("chosen:updated");
    }
    function SetTopBlockWidth(blocksCount) {
        var parent = $(".found-data-block-item").parent();
        var containerWidth = $(parent).width();
        var width = containerWidth / blocksCount - 40;
        $(".found-data-block-item").width(width);
        $(".found-data-block-item").css("margin-right: 20px;");
    }
    function sleep(millis, callback) {
        setTimeout(function ()
        { callback(); }
        , millis);
    }
    function SetTopBlocks() {
        if ($(window).width() < 1250) {
            $(".found-data-block-item:last").hide();
            SetTopBlockWidth(3);
        }
        if ($(window).width() >= 1250) {
            $(".found-data-block-item:last").show();
            SetTopBlockWidth(4);
        }
    }

    SetTopBlocks();

    $(window).resize(function () {
        SetTopBlocks();
    });

    $(window).scroll(function () {
        if ($(this).scrollTop() > 750)
            $('.scrollup').fadeIn();
        else
            $('.scrollup').fadeOut();
    });

    $('.scrollup').click(function () {
        $("html, body").animate({ scrollTop: 0 }, 600);
        return false;
    });

    $(".price-inputs input").bind("change paste keyup", function () {
        $("#priceFilterApplyBtn").show();
    });




    $(".input").click(function () {
        $("#inpSuggestion").focus();
        $(".search-icon").hide();
        $(".search-sample").hide();


    });

    $(".men").click(function () {
        if ($('.filter-item.sex .women').hasClass("active")) {
            SetMen();
            searchAndFilter();
            return;
        }
        SetMen();
    });
    $(".women").click(function () {
        if ($('.filter-item.sex .men').hasClass("active")) {
            SetWomen();
            searchAndFilter();
            return;
        }
        SetWomen();

    });

    $(".sizeSelector").chosen({
        disable_search_threshold: 10,
        no_results_text: "Ничего не найдено :(",
        placeholder_text_multiple: "Выбрать размер..."
    }).change(function () {
        searchAndFilter();
    });

    $('.sourceSelector').chosen({
        disable_search_threshold: 10,
        no_results_text: "Ничего не найдено :(",
        placeholder_text_multiple: "Выбрать магазин..."
    }).change(function () {
        resetText();
        getBrands();
        getSizes();
        searchAndFilter();
    });

    $(".color-filter").append(function () {
        var filter = $(".color-filter");
        $.get('/filter/colors/',
            function (data) {
                $(".color-filter").val = data.results.length;
                for (var i = 0; i < data.results.length; i++) {
                    filter.append("<div class=\"color-item\">" +
                                        "<input  type=\"checkbox\" id=\"color-check" + i + "\" value=\"" + data.results[i].Id + "\"></input>" +
                                        "<label style=\" display:inline-block; width:83%; height:100%\" for=\"color-check" + i + "\"><span class=\"boxColorPixel \" style=\"background:" + data.results[i].HexColor + ";\"></span>" +
                                        "<span class=\"labelForColorText\" >" + data.results[i].Name + "</span><\label>" +
                                    "</div>");

                    $("#color-check" + i).click(function () {
                        searchAndFilter();
                    });
                }
            });
    });

    var check = false;
    var checkMenu = false;

    $(window).scroll(function () {
        $('#collage')
            .stop()
            .animate({ "marginTop": (checkMenu || $(window).scrollTop() <= 100) ? ('0px') : ($(window).scrollTop() - 100) + "px" }, "fast");
    });

    $(".box").ready(function () {
        $(".color-filter").hide();
        $(".color-filter .color-item").css("background-color", "#efefef");
        $(".expand").click(function () {
            $(".color-filter").slideToggle();
            check = !check;
        });
    });

    $("#slideMenuButton").click(function () {
        if (!checkMenu) {
            $('.sorting-wrapper-look').slideRightHide();
            $('#catalogLookView').slideRightHide();
            $('.pager').slideRightHide();
            checkMenu = true;
        } else {
            $('.sorting-wrapper-look').slideRightShow();
            $('#catalogLookView').slideRightShow();
            $('.pager').slideRightShow();
            checkMenu = false;
        }
    });

    $("body").click(function (e) {

        if (e.target.className !== "expand" && check) {
            if ($(e.target).closest(".color-item").length) {
                return;
            }
            $(".color-filter").hide();
            check = false;
        }

        if (e.target.id !== "slideMenuButton" && !check) {
            if ($(e.target).closest("#catalogLookView").length) {
                return;
            }
            if ($(e.target).closest("#pager").length) {
                return;
            }
            $('.sorting-wrapper-look').slideRightHide();
            $('#catalogLookView').slideRightHide();
            $('.pager').slideRightHide();
            checkMenu = true;
        }
    });

    function getSelectedColors() {
        var colors = "";
        for (var i = 0; i < 19; i++) {
            var checker = $("#color-check" + i);
            if ($("#color-check" + i).prop("checked")) {
                colors += checker.val() + ",";
            }
        }
        return colors.substr(0, colors.length - 1);
    }
    $('.header-filter-button').click(function (e) {

        searchAndFilter();

    });
    function getOrder() {
        if ($("#priceOrder").hasClass("asc")) {
            return "price-asc";
        }
        if ($("#priceOrder").hasClass("desc")) {
            return "price-desc";
        }
        if ($("#discOrder").hasClass("asc")) {
            return "discount-asc";
        }
        if ($("#discOrder").hasClass("desc")) {
            return "discount-desc";
        }
        return null;
    }
    function resetCategory() {
        $('.categoryWomenSelector').val('').trigger('chosen:updated');
        $('.categoryMenSelector').val('').trigger('chosen:updated');
    }

    function resetBrand() {
        $(".brandSelector").val('');
    }
    function resetText() {
        $("#inpSuggestion").val('');
    }
    function getCategory() {
        var cat = $('.categoryWomenSelector').val();
        if ($(".men").hasClass("active")) {
            cat = $('.categoryMenSelector').val();
        }
        if (cat == "Выберите категорию")
            cat = null;
        return cat;
    }
    function getSource() {
        var source = $('.sourceSelector').val();
        if (source == "Выберите магазин")
            source = null;
        return source;
    }
    function getSex() {
        if ($(".men").hasClass("active")) {
            return 1;
        }
        return 0;
    }

    function searchAndFilter(i, load) {
        var page = null;
        if (i != null) {
            page = i;
        }

        $(".offers").empty();
        $(".preloader").show();
        //var selectedSizes = $('#sizeSelector').val();
        var url = getUrl(page);
        $.ajax({
            url: '/look/getdata/' + url.toString(),
            cache: false,
            type: "POST",
            dataType: "json",
            success: function (data) {
                $(".offers").empty();
                $(".offers").append(data.offersView);
                fillPager(data.totalPages, data.currentPage);
                if (load != true)
                    window.history.pushState('', '', url);
                $(".preloader").hide();
                $(".pagerNumber").click(function (e) {
                    searchAndFilter(e.target.textContent.replace(/[^0-9]+/, "").trim());
                });
                jQuery('div[id=media-look]').each(function () {
                    var pic = this.getElementsByClassName('pic_for_item');
                    $(pic[0]).draggable({helper: 'clone'});
                });
                $(".media").hover(function () {

                    $(".preview-button", this).show();
                    $(".like-button", this).show();
                    //$(".addToWishList", this).show();
                    $(".about-block", this).addClass("active");


                    //$(".arrow-up", this).show();
                }, function () {
                    $(".about-block").removeClass("active");
                    $(".preview-button").hide();
                    $(".like-button").hide();

                    //$(".addToWishList").hide();
                    //$(".arrow-up").hide();});
                });
                $('.like-button').click(function () {

                    var id = $(this).data().id;
                    var count = $(this).data().count;
                    var queryParam = {
                        'id': id,
                        'count': count
                    };
                    var _this = $(this);
                    var url = getUriWithQueryParams(queryParam);
                    $.ajax(
                    {
                        url: '/offer/like/' + url.toString(),
                        cache: false,
                        type: "POST",
                        dataType: "html",
                        success: function () {
                            _this.empty();
                            _this.data().count += 1;
                            _this.html(count + 1);
                        }
                    });
                });
            }
        });
    }

    function parseIds(arr) {
        var res = '';
        for (var i = 0; arr && (i < arr.length) ; i++) {
            res += arr[i] + ',';
        }
        res = res.length > 0 ? res.slice(0, res.length - 1) : null;
        return res;
    }

    function getUrl(page) {
        var searchText = $("#inpSuggestion").val();
        var minPrice = $('#minPrice').val();
        var maxPrice = $('#maxPrice').val();
        var brandIds = $(".brandSelector").val();
        var sizeIds = $(".sizeSelector").val();
        var source = getSource();
        var selectedColors = getSelectedColors();
        var orderBy = getOrder();
        var brands = parseIds(brandIds);
        var sizes = parseIds(sizeIds);

        //for (var i = 0; brandIds && (i < brandIds.length) ; i++) {
        //    brands += brandIds[i] + ',';
        //}

        //for (var i = 0; sizeIds && (i < sizeIds.length); i++) {
        //    sizes += sizeIds[i] + ',';
        //}

        //for (var i = 0; sourceIds && (i < sourceIds.length) ; i++) {
        //    sources += sourceIds[i] + ',';
        //}

        var sex = getSex();
        var cat = getCategory();
        var queryParams = {
            'minPrice': minPrice,
            'maxPrice': maxPrice,
            'colors': selectedColors,
            'brands': brands,
            'searchText': searchText,
            'category': cat,
            'sex': sex,
            'page': page,
            'orderBy': orderBy,
            'sizes': sizes,
            'sources': source
        };
        var url = getUriWithQueryParams(queryParams);
        return url;
    }
    $('.addToWishList').click(function (e) {
        var id = $(this).data().id;
        var wishList = $.cookie('wishList');
        if ($.cookie('wishList') == null || $.cookie('wishList') == "") {
            wishList = id;
        } else {
            wishList += "a" + id;
        }

        $.cookie('wishList', wishList, { expires: 30 });
    });

    $('.deleteFromWishList').click(function (e) {

        var id = $(this).data().id;
        var wishList = $.cookie('wishList');
        var regex1 = new RegExp(id + "a", 'g');
        var regex2 = new RegExp("a" + id + "$", 'g');
        var regex3 = new RegExp(id + "$", 'g');
        wishList = wishList.replace(regex1, '');
        wishList = wishList.replace(regex2, '');
        wishList = wishList.replace(regex3, '');

        if (wishList == "") {
            $.cookie('wishList', "");
        } else {
            $.cookie('wishList', wishList, { expires: 30 });
        }
        location.href = "/wishlist";
    });
    function getUriWithQueryParams(params) {
        var uri = new Uri("");

        $.each(params || {}, function (key, value) {
            if (value || (key == "sex" && value == 0))
                uri.replaceQueryParam(key, value);
            else
                uri.deleteQueryParam(key);
        });

        return uri;
    }

    $('#inpSuggestion').focus(function (e) {
        var inpSearch = $('#inpSuggestion');
        var inpCont = $('.input-wrapper.input-search');
        inpCont.addClass('active');
        if (inpSearch.val()) {
            $('.suggestion-cont').show();
            $('.suggestion-cont').width($('#inpSuggestion').width() + 10);
        }
    });
    $('#inpSuggestion').blur(function (e) {

        $('.suggestion-cont').hide();
        if ($('#inpSuggestion').val().length == 0) {
            $(".search-icon").show();
            $(".search-sample").show();
        }

    });
    $('#inpSuggestion').keyup(function (e) {
        var inpSearch = $('#inpSuggestion');
        var inpCont = $('.input-wrapper.input-search');
        var inpVal = inpSearch.val();
        if (!inpVal) {
            $('.suggestion-cont').hide();
            return;
        }
        $('.suggestion-cont').show();
        var code = (e.keyCode ? e.keyCode : e.which);
        if (code == 13) {
            resetCategory();
            searchAndFilter(e);
            $('.suggestion-cont').hide();
        }
        $.get('/json/suggestion?q=' + inpVal,
            function (data) {
                var cont = $('.suggestion-cont');
                $('.suggestion-cont').width($('#inpSuggestion').width() + 10);
                $('.suggestion-block').removeClass('active');
                cont.html("");
                $('.suggestion-block').addClass('active');
                for (var i = 0; i < data.results.length; i++) {
                    cont.append("<div class=\"sugLink\"> <a href=\"/look/create/?searchText=" + data.results[i] + "\">" + data.results[i] + " </a></div>");
                }

            });
    });
    $('#minPrice').keyup(function (e) {
        var code = (e.keyCode ? e.keyCode : e.which);
        if (code == 13) {
            searchAndFilter();

        }
    });
    $('#maxPrice').keyup(function (e) {
        var code = (e.keyCode ? e.keyCode : e.which);
        if (code == 13) {
            searchAndFilter();

        }
    });

    function checkContain(brandIds, brand) {
        var i = 0;
        for (i = 0; i < brandIds.length; i++) {
            if (brand == parseInt(brandIds[i])) {
                return 1;
            }
        }
        return 0;
    }
    function getBrands() {
        var cat = getCategory();
        var brandIds = $(".brandSelector").val();
        $.get('/json/brands?categoryId=' + cat,
            function (data) {
                $(".brandSelector").empty();
                for (var i = 0; i < data.brands.length; i++) {
                    if ((brandIds != null) && (checkContain(brandIds, data.brands[i].Id) == 1)) {
                        $(".brandSelector").append($("<option></option>")
                            .attr("value", data.brands[i].Id).attr("selected", "selected").text(data.brands[i].Title));
                    } else {
                        $(".brandSelector").append($("<option></option>")
                            .attr("value", data.brands[i].Id).text(data.brands[i].Title));
                    }
                }
                $(".brandSelector").trigger('chosen:updated');

                var url = getUrl();
                searchAndFilter();
                //window.history.pushState('', '', url.toString());
            });
    }

    function getSizes() {
        var cat = getCategory();
        var sizeIds = $(".sizeSelector").val();
        //var sourceIds = $('.sourceSelector').val();
        var sources = getSource();
        var sizes = parseIds(sizeIds);
        $.get('/json/sizes?categoryId=' + cat + '&sources=' + sources + "&sizes=" + sizes,
            function (data) {
                $(".sizeSelector").empty();
                for (var i = 0; i < data.sizes.length; i++) {
                    if (data.sizes[i].IsSelected) {
                        $(".sizeSelector").append($("<option></option>")
                            .attr("value", data.sizes[i].Id).attr("selected", "selected").text(data.sizes[i].Value + '(' + data.sizes[i].Unit + ')'));
                    } else {
                        $(".sizeSelector").append($("<option></option>")
                            .attr("value", data.sizes[i].Id).text(data.sizes[i].Value + '(' + data.sizes[i].Unit + ')'));
                    }
                }
                $(".sizeSelector").trigger('chosen:updated');

                var url = getUrl();
                searchAndFilter();
                //window.history.pushState('', '', url.toString());
            });
    }

    function appendPage(i, currentPage) {
        if (i == currentPage) {
            return '<li> <span class="pagerNumber active">' + i + '</span></li>\n';
        } else {
            return '<li> <span class="pagerNumber">' + i + '</span></li>\n';
        }
    }

    function fillPager(pageCount, currentPage) {
        $('#pager').empty();
        var result = '';
        if (pageCount <= 5) {
            for (var i = 0; i < pageCount; i++) {
                result += appendPage(i + 1, currentPage);
            }
        } else if (currentPage < 4) {
            for (i = 0; i < 4; i++) {
                result += appendPage(i + 1, currentPage);
            }
            result += '<li><span>...</span></li>\n';
            result += appendPage(pageCount, currentPage);
        } else if (pageCount - currentPage < 3) {
            result += appendPage(1, currentPage);
            result += '<li><span>...</span></li>\n';
            for (i = pageCount - 4; i < pageCount; i++) {
                result += appendPage(i + 1, currentPage);
            }
        } else {
            result += appendPage(1, currentPage);
            result += '<li><span>...</span></li>\n';
            for (i = currentPage - 2; i < currentPage + 1; i++) {
                result += appendPage(i + 1, currentPage);
            }
            result += '<li><span>...</span></li>\n';
            result += appendPage(pageCount, currentPage);
        }
        $('#pager').append('<ul class="horizontal">' + result + '</ul>');
    }

})