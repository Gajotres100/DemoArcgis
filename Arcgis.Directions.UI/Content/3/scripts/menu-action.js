
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

    var i = 0;
    $("#colName").click(function () {
        
        if(i == 0){
            $('#ascName').removeClass('mdi mdi-16px mdi-sort-ascending');
            $("#ascName").toggleClass('mdi mdi-16px mdi-sort-descending');
             i++;
        }
        else {
            $('#ascName').removeClass('mdi mdi-16px mdi-sort-descending');
            $("#ascName").toggleClass('mdi mdi-16px mdi-sort-ascending');
            i = 0;
        }

    });

    var z = 0;
    $("#colDate").click(function () {

        if (z == 0) {
            $('#ascDate').removeClass('mdi mdi-16px mdi-sort-ascending');
            $("#ascDate").toggleClass('mdi mdi-16px mdi-sort-descending');
            z++;
        }
        else {
            $('#ascDate').removeClass('mdi mdi-16px mdi-sort-descending');
            $("#ascDate").toggleClass('mdi mdi-16px mdi-sort-ascending');
            z = 0;
        }

    });

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