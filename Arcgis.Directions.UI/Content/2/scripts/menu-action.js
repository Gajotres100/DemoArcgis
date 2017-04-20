
$(document).ready(function () {
    $(".menu-toggle").click(function () {
        $('#sidebar li').removeClass();
        $("ul.main-menu li").toggleClass('');
        $('.menu-toggle').hide();

    });
    $("ul.main-menu li").click(function (event) {
        $('#sidebar li').removeClass('selected');
        $(this).toggleClass('selected');
        $('.menu-toggle').show();
    });

    $('li.dropdown').click(function () {
        $('li.dropdown').not(this).find('ul').hide();
        $(this).find('ul').toggle();
    });

    $("#myTable").tablesorter();

});
