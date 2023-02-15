
function carouselinit() {
    let radius = 600;
    let autoRotate = true;
    let rotateSpeed = -60;
    let imgWidth = 140;
    let imgHeight = 205;
    setTimeout(init, 300);
    let odrag = document.getElementById("drag-container");
    let ospin = document.getElementById("spin-container");
    let carousel = document.getElementById("carousel");
    let aImg = ospin.getElementsByTagName("a");
    ospin.style.width = imgWidth + "px";
    ospin.style.height = imgHeight + "px";
    let ground = document.getElementById("ground");
    ground.style.width = radius + "px";
    ground.style.height = radius + "px";
    function init(delayTime) {
        for (let i = 0; i < aImg.length; i++) {
            aImg[i].style.transform =
                "rotateY(" +
                i * (360 / aImg.length) +
                "deg) translateZ(" +
                radius +
                "px)";
            aImg[i].style.transition = "transform 1s";
            aImg[i].style.transitionDelay =
                delayTime || (aImg.length - i) / 4 + "s";
        }
    }
    function applyTranform(obj) {
        if (tY > 180) tY = 180;
        if (tY < 0) tY = 0;
        obj.style.transform = " rotateY(" + tX + "deg)";
        // "rotateX(" + -tY + "deg) rotateY(" + tX + "deg)"
    }
    function playSpin(yes) {
        ospin.style.animationPlayState = yes ? "running" : "paused";
    }
    let sX,
        sY,
        nX,
        nY,
        desX = 0,
        desY = 0,
        tX = 0,
        tY = 0;
    if (autoRotate) {
        let animationName = rotateSpeed > 0 ? "spin" : "spinRevert";
        ospin.style.animation = `${animationName} ${Math.abs(
            rotateSpeed
        )}s infinite linear`;
    }
    carousel.onpointerdown = function (e) {
        clearInterval(odrag.timer);
        e = e || window.event;
        let sX = e.clientX,
            sY = e.clientY;
        this.onpointermove = function (e) {
            e = e || window.event;
            let nX = e.clientX,
                nY = e.clientY;
            desX = nX - sX;
            desY = nY - sY;
            tX += desX * 0.1;
            tY += desY * 0.1;
            applyTranform(odrag);
            sX = nX;
            sY = nY;
        };
        this.onpointerup = function (e) {
            odrag.timer = setInterval(function () {
                desX *= 0.95;
                desY *= 0.95;
                tX += desX * 0.1;
                tY += desY * 0.1;
                applyTranform(odrag);
                playSpin(false);
                if (Math.abs(desX) < 0.5 && Math.abs(desY) < 0.5) {
                    clearInterval(odrag.timer);
                    playSpin(true);
                }
            }, 17);
            this.onpointermove = this.onpointerup = null;
        };
        return false;
    };
};
if ($(window).innerWidth() > 1060) {
    carouselinit();

    ;



}
else {
    remove('carousel');
    $('.slider').slick();
    $('.slider').height = "600";


}
if ($(window).innerWidth() < 500) {
    // document.getElementById('slick-prev').parentNode.removeChild(elem);

}
function remove(deleted) {
    var elem = document.getElementById(deleted);
    elem.parentNode.removeChild(elem);
    return false;
}

//-------------Functions for zapisi-------------------//
var buttonforspecclicked=false;
var nextclicked=false;
function zapisiopen() {
    var service;
    var spec;
    document.getElementsByClassName('zapisi')[0].style.display = "block";
    document.getElementsByClassName('allcontainerforchoise')[0].style.display = "none";
    document.getElementsByClassName('masterscontainer')[0].style.display = "none";
    document.getElementsByClassName('timeanddate')[0].style.display = "none";
    // if ($(document).height() > $(window).height()) {
    //     var scrollTop = ($('html').scrollTop()) ? $('html').scrollTop() : $('body').scrollTop(); // Works for Chrome, Firefox, IE...
    //     $('html').addClass('noscroll').css('top', -scrollTop);
    // }
    //    for( i=0;i<9;i++){
    //     document.getElementsByClassName('uslugi')[i].style.display="none";
    //    }
    document.body.style.position = 'fixed';
    document.body.style.top = `-${window.scrollY}px`;
  
    
}
function zapisiclose() {
    document.getElementsByClassName('zapisi')[0].style.display = "none";
    
    // var scrollTop = parseInt($('html').css('top'));
    // $('html').removeClass('noscroll');
    // $('html,body').scrollTop(-scrollTop);
    document.getElementsByClassName('masters')[0].style.display = "block";
    document.getElementsByClassName('services')[0].style.display = "block";
    document.getElementsByClassName('allcontainerforchoise')[0].style.display = "none";
    document.getElementsByClassName('masterscontainer')[0].style.display = "none";
    document.getElementsByClassName('timeanddate')[0].style.display = "none";
    const scrollY = document.body.style.top;
    document.body.style.position = '';
    document.body.style.top = '';
    // window.scrollTo(0, parseInt(scrollY || '0') * -1);
    document.getElementsByClassName('main-block')[0].style.overflow = "hidden";
    nextclicked=false;
    buttonforspecclicked=false;

}
function mastersclick() {
    document.getElementsByClassName('masters')[0].style.display = "none";
    document.getElementsByClassName('services')[0].style.display = "none";
    document.getElementsByClassName('masterscontainer')[0].style.display = "block";
    document.getElementsByClassName('main-block')[0].style.overflow = "scroll";
    //buttonforspecclicked=false;
}
function servicesclick() {
    document.getElementsByClassName('masters')[0].style.display = "none";
    document.getElementsByClassName('services')[0].style.display = "none";
    document.getElementsByClassName('allcontainerforchoise')[0].style.display = "block";
    document.getElementsByClassName('main-block')[0].style.overflow = "scroll";

    
}
function backforzapisi(){
    zapisiclose();
    zapisiopen();
}

function nextclick(){
    if($(".allcontainerforchoise").find(".check1:checked")){
        if (buttonforspecclicked == true) {
            var massiv = [];
            var mastername;
            var week = 1;
            if (spec == 1) {
                mastername = "саша";
            }
            massiv[0] = mastername;
            massiv[1] = week;
            
            json = JSON.stringify(massiv);
            console.log(json);
            jsonforfindtimeforzapisi(json);
            timeanddateopen();
        }
        else{
            document.getElementsByClassName('allcontainerforchoise')[0].style.display = "none";
            mastersclick();
            
        }
        nextclicked=true;
    }
    
}
$(document).mouseup(function (e){
    if ( 
        $(".zapisi").has(e.target).length === 0) { 
            zapisiclose();
    }
});
$('.containerforchoise input:checkbox').click(function(){
	if ($(this).is(':checked')) {
		 $('.containerforchoise input:checkbox').not(this).prop('checked', false);
	}
});
$('.containerforchoise input:checkbox .check1').click(function(){
	if ($(this).is(':checked')) {
		 $('.containerforchoise input:checkbox .check1').not(this).attr('checked', false);
	}
});
$('.containerforchoise input:checkbox').click(function(){
	if ($(this).is(':checked',false)){
        
		$('.next').removeAttr('disabled');
	} else {
        $('.next').attr('disabled', 'disabled');
	}
});

$('#buttonforspec1').click(function(){
    spec = 1;
    if(nextclicked==false){
        document.getElementsByClassName('masterscontainer')[0].style.display = "none";
        servicesclick();
    
    }
    else{
        timeanddateopen();
    }
    buttonforspecclicked=true;
});
$('#buttonforspec2').click(function () {
    spec = 2;
    if (nextclicked == false) {
        document.getElementsByClassName('masterscontainer')[0].style.display = "none";
        servicesclick();

    }
    else {
        timeanddateopen();
    }
    buttonforspecclicked = true;
});
$('#buttonforspec3').click(function () {
    spec = 3;
    if (nextclicked == false) {
        document.getElementsByClassName('masterscontainer')[0].style.display = "none";
        servicesclick();

    }
    else {
        timeanddateopen();
    }
    buttonforspecclicked = true;
});
$('#buttonforspec4').click(function () {
    spec = 4;
    if (nextclicked == false) {
        document.getElementsByClassName('masterscontainer')[0].style.display = "none";
        servicesclick();

    }
    else {
        timeanddateopen();
    }
    buttonforspecclicked = true;
});
function timeanddateopen(){
    document.getElementsByClassName('timeanddate')[0].style.display = "block";
    document.getElementsByClassName('allcontainerforchoise')[0].style.display = "none";
    document.getElementsByClassName('masterscontainer')[0].style.display = "none";
}
$('#block1').click(function(){
    
	document.getElementById('haircut1').scrollIntoView();
});
$('#block2').click(function(){
    opencontainer();
    document.getElementById('haircut2').scrollIntoView();
    // scrollTo(0,3450)
    
});
$('#block3').click(function(){
    opencontainer();
        document.getElementById('haircut3').scrollIntoView();
        // will scroll to 4th h3 element
      
});
$('#block4').click(function(){
    opencontainer();
    document.getElementById('haircut4').scrollIntoView();
});
$('#block5').click(function(){
    opencontainer();
    document.getElementById('haircut5').scrollIntoView();
});
function opencontainer(){
    document.getElementsByClassName('container4')[0].style.height = "max-content";
    document.getElementsByClassName('containerforprice')[0].style.height = "max-content";
    document.getElementById('haircut2').style.display="block";
    document.getElementById('haircut3').style.display="block";
    document.getElementById('haircut4').style.display="block";
    document.getElementById('haircut5').style.display="block";
    document.getElementById('haircut5').style.height="max-content";
    document.getElementById('haircut5').style.margin="0";
    document.getElementById('openfullcontainer').style.display = "none";
    document.getElementById('closefullcontainer').style.display="block";
}
function closecontainer(){
    document.getElementsByClassName('container4')[0].style.height = "max-conent";
    document.getElementsByClassName('containerforprice')[0].style.height = "max-content";
    document.getElementById('haircut1').style.margin="50px 0 0 0";
    document.getElementById('haircut2').style.display="none";
    document.getElementById('haircut3').style.display="none";
    document.getElementById('haircut4').style.display="none";
    document.getElementById('haircut5').style.display="none";
    document.getElementById('openfullcontainer').style.display = "block";
    document.getElementById('closefullcontainer').style.display="none";
    document.getElementById('openfullcontainer').style.margin = "80px  auto 0 auto";
    document.getElementsByClassName('containerforprice').style.padding = "0 auto 20px auto";
}
    $('#openfullcontainer').click(function (){
    opencontainer();
    });
    $('#closefullcontainer').click(function (){
        closecontainer();
        });


// document.onmousewheel = function(e) {
//     e = e || window.event;
//     let d = e.wheelDelta / 20 || -e.detail;
//     radius += d;
//     init(1);
// };  	
