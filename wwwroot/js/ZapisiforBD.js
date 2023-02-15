

function particalviewinit() {
    
    //$('.time .checkfordate').click(function () {
    //    if ($(this).is(':checked')) {
    //        $('.time .checkfordate').not(this).attr('checked', false);
    //    }
    //});
    //$('.timeanddate .stringofday .time .checkfordate').click(function () {
    //    if ($(this).is(':checked', false)) {

    //        $('.zapisbutton').removeAttr('disabled');
    //    } else {
    //        $('zapisbutton').attr('disabled', 'disabled');
    //    }
    //});
    document.getElementsByClassName('zapisbutton')[0].onclick = function () {

        console.log("123");
        $('.timeanddate .stringofday .time .checkfordate:checked').each(function () {


            dataforzapisbd[2] = document.getElementById($(this).attr('id')).parentElement.children[1].innerHTML + ":00";
            var data = document.getElementById($(this).attr('id')).parentElement.parentElement.children[0].innerHTML;
            dataforzapisbd[3] = data.substr(data.lastIndexOf('<span>') + 6).replace("</span>","");
            //console.log(dataforzapisbd);
            dataforzapisbd[4] = "fio";
            dataforzapisbd[5] = "asd";
            
            endofzapistobd();
            // Output: input_1, input_3
        });
    };
    function endofzapistobd(){
        $.ajax({
            type: "POST",
            url: "https://pricheson.tk/Home/ParticalViewForFIOAndPhone",
            //dataType: "json",
            //data: { id: json },
            success: function (result) {
                //var timeforgetting=JSON.parse(result);
                document.getElementsByClassName("main-block")[0].innerHTML = result;
                console.log();
                //jsonforfindtimeforzapisi();
                particalviewforfioandphone();

                //location.reload();
                
            },
            error: function (req, status, error) {

                //location.reload();
            }

        });
    };
    document.getElementsByClassName('main-block')[0].style.overflow = "scroll";
};
function particalviewforfioandphone() {
    document.getElementsByClassName('finishofzapis')[0].onclick = function () {

        
        
        dataforzapisbd[4] = document.getElementById("FIOTEXT").value;
        dataforzapisbd[5] = document.getElementById("phonenumbertext").value;
        json = JSON.stringify(dataforzapisbd);
        finishofzapistobd(json);

            //dataforzapisbd[5] = "asd";
            //endofzapistobd();
            // Output: input_1, input_3
        
    };
    function finishofzapistobd(json) {
        $.ajax({
            type: "POST",
            url: "https://pricheson.tk/Home/AddZapis",
            //dataType: "json",
            data: { id: json },
            success: function () {
                //var timeforgetting=JSON.parse(result);
                //document.getElementsByClassName("main-block")[0].innerHTML = result;
                console.log();
                //jsonforfindtimeforzapisi();
                
                //location.reload();

            },
            error: function (req, status, error) {

                //location.reload();
            }

        });
    };
};


    
