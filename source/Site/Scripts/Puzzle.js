var items = [];
var startDate;
var t;

Date.prototype.addHours= function(h){
    this.setHours(this.getHours()+h);
    return this;
};

function startTime()
{
    var tm=new Date(new Date() - startDate);
    var h=tm.getHours();
    var m=tm.getMinutes();
    var s=tm.getSeconds();
    m=checkTime(m);
    s=checkTime(s);
    document.getElementById("txt").innerHTML=h+":"+m+":"+s;
    t=setTimeout("startTime()",500);
}
function checkTime(i)
{
    if (i<10)
    {
        i="0" + i;
    }
    return i;
}

function stopTime() {
    var tm = new Date(new Date() - startDate);
    clearTimeout(t);
    return tm;
}

function convertDateToInt(date) {
    var hours = date.getHours() * 3600000;
    var minutes = date.getMinutes() * 60000;
    var seconds = date.getSeconds() * 1000;
    return parseInt(hours + minutes + seconds);
}

function showPopup() {
    $("#popup1").show();
}

function hidePopup() {
    $("#popup1").hide();
}

$(document).ready(function() {
    var complexity = parseInt($("#model-data").attr("complexity"));
    var pictureId = $("#model-data").attr("pictureId");
    hidePopup();
    var canvas = new fabric.Canvas("editor", {
        backgroundColor: "#ffffff"
    });


    var rowCount = 0;
    var cellCount = 0;
    switch (complexity) {
        case 1:
            rowCount = 4;
            cellCount = 3;
            break;
        case 2:
            rowCount = 6;
            cellCount = 4;
            break;
        case 3:
            rowCount = 8;
            cellCount = 6;
            break;
    }

    var checkPuzzle = function() {
        for (var i = 0; i < rowCount * cellCount; i++) {
            if (i != items[i]) return false;
        }
        showPopup();
        var time = stopTime();
        $(".editor-form").submit(function() {
            $(".Complexity").each(function() {
                $(this).val(complexity);
            });
            $(".Time").each(function() {
                $(this).val(convertDateToInt(time));
            });
            $(".PictureId").each(function() {
                $(this).val(pictureId);
            });
            return true;
        });
        return true;
    };
    for (var i = 0; i < rowCount * cellCount; i++) {
        items.push(-1);
    }

    for (var i = 1; i < rowCount; i++) {
        canvas.add(new fabric.Line([i * canvas.width / rowCount, 0, i * canvas.width / rowCount, canvas.height], {
            left: i * canvas.width / rowCount,
            top: 0,
            stroke: "red"
        }));
    }

    for (var i = 1; i < cellCount; i++) {
        canvas.add(new fabric.Line([0, i * canvas.height / cellCount, canvas.width, i * canvas.height / cellCount], {
            left: 0,
            top: i * canvas.height / cellCount,
            stroke: "red"
        }));
    }

    jQuery("div[id=image_part]").each(function() {
        var pic = this.getElementsByClassName("pic_for_item");
        $(pic[0]).draggable({ helper: "clone" });
    });

    startDate = new Date().addHours(3);
    startTime();

    $("#editor").droppable({
        drop: function(event, ui) {
            var leftMiddle = (ui.offset.left - $(this).offset().left) + (canvas.width / (2 * rowCount));
            var topMiddle = (ui.offset.top - $(this).offset().top) + (canvas.height / (2 * cellCount));
            var left = leftMiddle - leftMiddle % (canvas.width / rowCount);
            var top = topMiddle - topMiddle % (canvas.height / cellCount);
            var picture = ui.draggable;
            fabric.util.loadImage(picture.context.src, function(properties, img) {
                var object = new fabric.Image(img);
                object.set({
                    left: left,
                    top: top
                });
                object.hasRotatingPoint = false;
                object.lockMovementX = true;
                object.lockMovementY = true;
                object.hasControls = false;
                object.hasBorders = false;
                canvas.add(object);
                canvas.renderAll();
            }, null, { crossOrigin: "Anonymous" });
            var rows = (top / (canvas.height / cellCount));
            var cells = (left / (canvas.width / rowCount));
            var place = rows * rowCount + cells;
            var curIndex = picture.attr('index');
            $("img[index='" + curIndex + "']").parent().hide();
            var prevIndex = items[place];
            if (prevIndex > -1) {
                var objects = canvas.getObjects();
                for (var ind = 5; ind < objects.length; ++ind) {
                    var cur = objects[ind];
                    if (cur.get('top') == top && cur.get('left') == left) {
                        canvas.remove(cur);
                        break;
                    }
                }
                $("img[index='" + prevIndex + "']").parent().show();
            }
            items[place] = curIndex;
            checkPuzzle();
        }
    });

    addEventListener("dblclick", function(event) {
        var x = event.pageX;
        var y = event.pageY;
        var leftC = (x - $("#editor").offset().left);
        var topC = (y - $("#editor").offset().top);
        var left = leftC - leftC % (canvas.width / rowCount);
        var top = topC - topC % (canvas.height / cellCount);
        var rows = (top / (canvas.height / cellCount));
        var cells = (left / (canvas.width / rowCount));
        var place = rows * rowCount + cells;
        if (place < items.length) {
            var index = items[place];
            if (index > -1) {
                $("img[index='" + index + "']").parent().show();
            }
            items[place] = -1;
            var objects = canvas.getObjects();
            for (var ind = 5; ind < objects.length; ++ind) {
                var cur = objects[ind];
                if (cur.get('top') == top && cur.get('left') == left) {
                    canvas.remove(cur);
                    break;
                }
            }
        }
    });
});