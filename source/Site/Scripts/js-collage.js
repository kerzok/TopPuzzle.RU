var removedObjects = new Array();
var canvasScale = 1;
var SCALE_FACTOR = 1.2;
var total_count = 0;
var lookId = -1;

function getRandomInt(min, max) {
    return Math.floor(Math.random() * (max - min + 1)) + min;
}

function OfferInfo(id, deleted, topX, topY, scaleX, scaleY, angle, isFlipped) {
    this.OfferId = id;
    this.TopX = topX;
    this.TopY = topY;
    this.ScaleX = scaleX;
    this.ScaleY = scaleY;
    this.Angle = angle;
    this.IsFlipped = isFlipped;
    this.IsDeleted = deleted;
}

function addPrice(price) {
    total_count += parseFloat(price);
    $(".total-count").text("Общая стоимость: " + total_count + " руб.");
}

$().ready(function () {
    // canvas
    var canvas = new fabric.Canvas('collage', {
        backgroundColor: "#ffffff"
    });

    $(".total-count").text("Общая стоимость: " + total_count + " руб.");

    $.ajax('/look/get_data_to_edit', {
        type: 'POST',
        success: function (result) {
            if (result.message == 'ok') {
                $('#save_as_new_text').css('display', 'inline-block');
                $('#save_as_new').css('display', 'inline-block');
                var data = JSON.parse(result.data);
                lookId = result.id;
                for (var i = 0; i < data.length; i++) {
                    if (data[i].IsDeleted) {
                        removedObjects.push(data[i].OfferId);
                        addPrice(data[i].OfferPrice);
                    } else {
                        fabric.util.loadImage("../Content/localImage/" + data[i].OfferPicture, function(properties, img) {
                            var object = new fabric.Image(img);
                            object.set({
                                left: properties.TopX,
                                top: properties.TopY,
                                scaleX: properties.ScaleX,
                                scaleY: properties.ScaleY,
                                angle: properties.Angle,
                                flipX: properties.IsFlipped,
                                price: properties.OfferPrice,
                                offer_id: properties.OfferId
                            });
                            object.hasRotatingPoint = true;
                            canvas.add(object);
                            canvas.renderAll();
                            addPrice(properties.OfferPrice);
                        }, null, { crossOrigin: 'Anonymous' }, data[i]);
                    }
                }

                var lookInfo = JSON.parse(result.info);
                $('#title').val(lookInfo.Title);
                $('#sex').val(lookInfo.Sex);
                $('#tags').val(result.tags);
                $('#addition').val(lookInfo.Addition);
                if (lookInfo.Type != lookInfo.Id) {
                    $('#is_personal').prop('checked', true);
                    $("#type").removeAttr("disabled");
                    $('#type').val(lookInfo.Type);
                }
            } else {
                $('#save_as_new_text').css('display', 'none');
                $('#save_as_new').css('display', 'none');
                $('#save_as_new').prop('checked', true);
            }
        }
    });

    
    $('.look-saved').hide();
    
    $("#align").click(function () {
        canvas._activeObject.set("angle", 0);
        canvas.renderAll();
    });

    $("#mirror").click(function () {
        canvas._activeObject.set('flipX', !canvas._activeObject.get('flipX'));
        canvas.renderAll();
    });

    $("#upper").click(function () {
        canvas._activeObject.bringForward();
        canvas.renderAll();
    });

    $("#lower").click(function () {
        canvas._activeObject.sendBackwards();
        canvas.renderAll();
    });

    $("#erase").click(function () {
        var object = canvas.getActiveObject();
        total_count -= parseFloat(object.get('price'));
        canvas.remove(object);
        canvas.renderAll();
        $(".total-count").text("Общая стоимость: " + total_count + " руб.");
    });

    $("#remove_from_look").click(function () {
        var object = canvas.getActiveObject();
        removedObjects.push(object.get('offer_id'));
        canvas.remove(object);
        canvas.renderAll();
        $(".total-count").text("Общая стоимость: " + total_count + " руб.");
    });


    $('#collage').droppable({
        drop: function (event, ui) {
            var left = ui.offset.left - $(this).offset().left;
            var top = ui.offset.top - $(this).offset().top;
            var picture = ui.draggable;
            var price = picture.attr('price');
            var id = picture.attr('offer-id');
            var alreadyAddedIndex = -1;
            canvas.getObjects().forEach(function(element, index) {
                if (element.get('offer_id') == id) {
                    alreadyAddedIndex = index;
                }
            });
            if (alreadyAddedIndex == -1) {
                total_count += parseFloat(price);
                fabric.util.loadImage(picture.context.src, function(properties, img) {
                    var object = new fabric.Image(img);
                    object.set({
                        left: left,
                        top: top,
                        price: price,
                        offer_id: id
                    });
                    object.hasRotatingPoint = true;
                    object.scaleX = object.scaleY = .30;
                    canvas.add(object);
                    removedObjects = removedObjects.filter(function(el) {
                        return el != id;
                    });
                    canvas.renderAll();
                    $(".total-count").text("Общая стоимость: " + total_count + " руб.");
                }, null, { crossOrigin: 'Anonymous' });
            }
        }
    });

    $(".pic_for_item").live('click', function () {
        
    });

    $("#is_personal").click(function() {
        if ($("#is_personal").prop("checked")) {
            $("#type").removeAttr("disabled");
        } else {
            $("#type").attr("disabled", "disabled");
            $("#type").val('');
        }
    });

    $("#save-canvas").click(function () {
        $("#save-canvas").attr("disabled", "disabled");
        if (canvas._activeObject != null) {
            canvas._activeObject.set('active', false);
        }
        var offersInfo = new Array();
        canvas.getObjects().forEach(function(element) {
            offersInfo.push(new OfferInfo(element.get('offer_id'),
                false,
                element.get('left'),
                element.get('top'),
                element.get('scaleX'),
                element.get('scaleY'),
                element.get('angle'),
                element.get('flipX')));
        });
        removedObjects.forEach(function(element) {
            offersInfo.push(new OfferInfo(element, true));
        });
        var dataUrl = canvas.toDataURL("image/png");
        var type = document.getElementById('type').value;
        var addition = document.getElementById('addition').value;
        var tags = document.getElementById('tags').value;
        var title = document.getElementById('title').value;
        var e = document.getElementById('sex');
        var sex = e.options[e.selectedIndex].value;
        var saveAsNew = $('#save_as_new:checked').val();
        $.ajax('/look/save_look', {
            type: 'POST',
            data: {
                "imageData": dataUrl,
                'type': type,
                'title': title,
                'sex': sex,
                'tags': tags,
                'offersInfo': JSON.stringify(offersInfo),
                'saveAsNew': saveAsNew,
                'updatedLookId': lookId,
                'addition': addition
            },
            dataType: 'JSON',
            success: function (data) {
                if (data.message == 'ok') {
                    if (!saveAsNew) {
                        $('.look-saved').text('Лук успешно обновлен!');
                    } else {
                        $('.look-saved').text('Лук успешно сохранен!');
                    }
                } else {
                    $('.look-saved').text('Упс, во время сохранения лука произошла ошибка:(');
                    console.log(data.message);
                }
                $('.look-saved').show();
                $('body, html').animate({
                    scrollTop: 0
                }, 800);
                $("#save-canvas").removeAttr("disabled");
                $(document).click(function() {
                    $('.look-saved').hide();
                });
            }
        });
    });
});
