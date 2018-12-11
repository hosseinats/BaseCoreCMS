function notify(style, position, text, title) {
	if(style == "error"){
		icon = "fa fa-exclamation";
	}else if(style == "warning"){
		icon = "fa fa-warning";
	}else if(style == "success"){
		icon = "fa fa-check";
	}else if(style == "info"){
		icon = "fa fa-question";
	}else{
		icon = "fa fa-circle-o";
	}
    $.notify({
        title: title,
        text: text,
        image: "<i class='" + icon + "'></i>",
		
    }, {
        style: 'metro',
        className: style,
        globalPosition:position,
        showAnimation: "show",
        showDuration: 0,
        hideDuration: 0,
        autoHide: false,
        clickToHide: true
    });
}

function notify2(style,position) {
    $(".autohidebut").notify({
        text: '<i class="fa fa-comment-o"></i> سلام رفیق. من اینجا هستم!'
    }, {
        style: 'metro',
        className: 'nonspaced',
        elementPosition:position,
        showAnimation: "show",
        showDuration: 0,
        hideDuration: 0,
        autoHide: false,
        clickToHide: true
    });
}

function autohidenotify(style, position, text, title ="", autoHideDelay=3000 ) {
 	if(style == "error"){
		icon = "fa fa-exclamation";
	}else if(style == "warning"){
		icon = "fa fa-warning";
	}else if(style == "success"){
		icon = "fa fa-check";
	}else if(style == "info"){
		icon = "fa fa-question";
	}else{
		icon = "fa fa-circle-o";
	}   
    $.notify({
        title: title,
        text: text,
        image: "<i class='" + icon + "'></i>",
    }, {
        style: 'metro',
        className: style,
        globalPosition:position,
        showAnimation: "show",
        showDuration: 0,
        hideDuration: 0,
        autoHideDelay: autoHideDelay,
        autoHide: true,
        clickToHide: true
    });
}

function nconfirm(text) {
    $.notify({
        title: '',
        text:   text + '<div class="clearfix"></div><br><a class="btn btn-sm btn-dark yes">بله</a> <a class="btn btn-sm btn-danger no">خیر</a>',
        //image: "<i class='fa fa-warning'></i>"
    }, {
        style: 'metro',
        className: "cool",
        showAnimation: "show",
        showDuration: 0,
        hideDuration: 0,
        autoHide: false,
        globalPosition : "top center",
        clickToHide: false
    });
}
//function nconfirmAccept(text) {
//    $.notify({
//        title: '',
//        text: text + '<div class="clearfix"></div><br><a class="btn btn-sm btn-default yesAcc">بله</a> <a class="btn btn-sm btn-danger no">خیر</a>',
//        //image: "<i class='fa fa-warning'></i>"
//    }, {
//            style: 'metro',
//            className: "cool",
//            showAnimation: "show",
//            showDuration: 0,
//            hideDuration: 0,
//            autoHide: false,
//            globalPosition: "top",
//            clickToHide: false
//        });
//}

//$(function(){
//	//listen for click events from this style
//	$(document).on('click', '.notifyjs-metro-base .no', function() {
//	  //programmatically trigger propogating hide event
//	  $(this).trigger('notify-hide');
//	});
//	$(document).on('click', '.notifyjs-metro-base .yes', function() {
//	  //show button text
//	  alert($(this).text() + " clicked!");
//	  //hide notification
//	  $(this).trigger('notify-hide');
//	});
//})