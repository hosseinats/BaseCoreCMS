//--------------------Post Create----------------------
jQuery(document).ready(function () {
    $('#Body').summernote({
        height: 300,
        minHeight: 100,
        maxHeight: null,
        focus: false,
        lang:'fa-IR'
    });
});


$.validator.setDefaults({ ignore: ":hidden:not(.form-control-chosen)" });

$(".form-control-chosen").chosen({ rtl: true, no_results_text: ' چیزی یافت نشد !' });

$('textarea').maxlength({
    alwaysShow: true,
    placement: 'bottom-left-inside',
    warningClass: "badge badge-success",
    limitReachedClass: "badge badge-danger"
});


$(document).on("click", "#moreTag", function () {
    $("#contentModal").load('PartialAddTag', function () {
        $.validator.unobtrusive.parse($("#contentModal"));
    });
})


//--------------------Category Create---------------------
$(document).on("click", "#PopUpCreate", function () {
    var title = $(this).text();
    $(".custom-modal-title").html(title);
    var url = window.location.pathname.split("/");
    //var controllerName = url[2];
    var urlForLoad = "/"+url[1] + "/" + url[2] + "/CreatePartial" 
    $("#contentModal").load(urlForLoad, function () {
        $.validator.unobtrusive.parse($("#contentModal"));
    });
})
$(document).on("click", ".PopUpUpdate", function () {
    var title = $(this).attr('dataTitle');
    $(".custom-modal-title").html(" ویرایش " + title);
    idForUpdate = $(this).attr('dataId');
    var url = window.location.pathname.split("/");
    var urlForLoad = "/" + url[1] + "/" + url[2] + "/UpdatePartial/" 
    //var controllerName = url[2];
    $("#contentModal").load(urlForLoad + idForUpdate, function () {
        $.validator.unobtrusive.parse($("#contentModal"));
    });
})

!function ($) {
    var idForDelete;
    var tableRow;
    var spanObject;
    $('.DeleteButton').click(function (e) {
        spanObject = $(this);
        var titleDelete = spanObject.attr('dataTitle');
        nconfirm("آیا مطمئن هستید می خواهید " + titleDelete + " را پاک نمایید ؟ ");
        idForDelete = spanObject.attr('dataId');
        tableRow = spanObject.parent().parent();
        $('.DeleteButton').attr('disabled', true);
        spanObject.hide();
        return false;
    });
    $(document).on('click', '.no', function () {
        $(this).trigger('notify-hide');
        spanObject.show();
        $('.DeleteButton').removeAttr('disabled');
    });
    $(document).on('click', '.yes', function () {
        $(this).trigger('notify-hide');
        var url = window.location.pathname.split("/");
        //var controllerName = url[2];
        var urlForDel = "/"+ url[1] + "/" + url[2] + "/delete" 
        $.ajax({
            type: "POST",
            //url:  controllerName + '/Delete',
            url: urlForDel,
            data: { delId: idForDelete },
            //contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                if (data.result === "ok") {
                    $(tableRow).fadeTo(600, 0, function () {
                        $(tableRow).remove();
                        $('.DeleteButton').removeAttr('disabled');
                        autohidenotify('success', 'top center', 'با موفقیت پاک شد');

                    });
                }
                else if (data.result === "nok") {
                    spanObject.show();
                    $('.DeleteButton').removeAttr('disabled');
                    autohidenotify('error', 'top center', 'خطا ! احتمالا گزینه انتخابی دارای رکوردهای وابسته میباشد.');
                }
                else {
                    autohidenotify('error', 'top center', 'خطا');
                    spanObject.show();
                    $('.DeleteButton').removeAttr('disabled');
                }
            }
        });
    });
}(window.jQuery)