
function subscribe() {
    //var emailpattern = /^\w+([\.-]?\w+)*@\w+([\.-]?\w+)*(\.\w{2,3})+$/;
    //var email = $('#txtemail').val();
    //if (email != "") {
    //    if (!emailpattern.test(email)) {
    //        $('.text-danger').remove();
    //        var str = '<span class="error">Invalid Email</span>';
    //        $('#txtemail').after('<div class="text-danger">Invalid Email</div>');

    //        return false;
    //    }
    //    else {
    //        $.ajax({
    //            url: 'index.php?route=extension/module/newsletters/news',
    //            type: 'post',
    //            data: 'email=' + $('#txtemail').val(),
    //            dataType: 'json',


    //            success: function (json) {

    //                $('.text-danger').remove();
    //                $('#txtemail').after('<div class="text-danger">' + json.message + '</div>');

    //            }

    //        });
    //        return false;
    //    }
    //}
    //else {
    //    $('.text-danger').remove();
    //    $('#txtemail').after('<div class="text-danger">Email Is Require</div>');
    //    $(email).focus();

    //    return false;
    //}


}

function quickbox() {
    if ($(window).width() > 767) {
        $('.quickview').magnificPopup({
            type: 'iframe',
            delegate: 'a',
            preloader: true,
            tLoading: 'Loading image #%curr%...'
        });
    }
}



$(document).ready(function () {
    /* Search */
    $('#searchbox input[name=\'search\']').parent().find('button').on('click', function () {
        //var url = $('base').attr('href') + 'index.php?route=product/search';

        //var value = $('#searchbox input[name=\'search\']').val();

        //if (value) {
        //    url += '&search=' + encodeURIComponent(value);
        //}

        //var category_id = $('#searchbox select[name=\'category_id\']').prop('value');

        //if (category_id > 0) {
        //    url += '&category_id=' + encodeURIComponent(category_id);
        //}

        //location = url;

        location.href = 'http://' + location.host;
    });

    $('#slideshow0').swiper({
        mode: 'horizontal',
        slidesPerView: 1,
        pagination: '.slideshow0',
        paginationClickable: true,
        nextButton: '.swiper-button-next',
        prevButton: '.swiper-button-prev',
        spaceBetween: 0,
        autoplay: 1500,
        autoplayDisableOnInteraction: true,
        loop: true,
        speed: 1500
    });

    quickbox();
    jQuery(window).resize(function () { quickbox(); });

    $("#spinner").fadeOut("slow");

    $('.blogcarousel').owlCarousel({
        items: 4,
        singleItem: false,
        navigation: true,
        navigationText: ['<i class="fa fa-angle-left fa-5x"></i>', '<i class="fa fa-angle-right fa-5x"></i>'],
        pagination: true,
        itemsDesktop: [1000, 3],
        itemsDesktopSmall: [979, 2],
        itemsTablet: [767, 2]
    });

    $('#tabs a').tabs();
});