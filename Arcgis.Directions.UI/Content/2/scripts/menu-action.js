
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
    $("#search").on("keyup", function () {
        var value = $(this).val().toUpperCase();
        $("table > tbody  > tr").each(function (index) {
            if (index !== 0) {
               
                $row = $(this);

                var name = $row.find("td:first").text().toString().toUpperCase();
               
                //if (name.indexOf(value) !== -1) {
                for(var i = 0; i< name.length; i++){
                if (name.includes(value,0)) {
                    
                    $row.show();
                }
                else {
                    
                    $row.hide();
                }
                }
            }
            
        });
    });

});