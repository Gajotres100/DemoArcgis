
$(document).ready(function () {
    $(".menu-toggle").click(function () {
        $('#sidebar li').removeClass();
        
        $('.menu-toggle').hide();

    });
    $("ul.main-menu li").click(function (event) {
        $('#sidebar li').removeClass('selected');
        $(this).toggleClass('selected');
        $('.menu-toggle').show();
    });

});
