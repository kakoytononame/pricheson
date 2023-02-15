window.onload = function () {
	inizialpage();
};
function inizialpage() {
	//var calendar;
	calendarEl = document.getElementById('calendar');
	//calendarEl.style.display = "block";
	//document.getElementsByClassName('blockforlogin')[0].style.display = "none";
	//document.getElementById('login').style.display = "none";
	//document.getElementById('password').style.display = "none";
	//document.getElementById('logintext').style.display = "none";
	//document.getElementById('passwordtext').style.display = "none";
	//document.getElementById('entry').style.display = "none";
	//document.getElementsByClassName('dbvisualline')[0].style.display = "block";
	//document.getElementsByClassName('dbvisuallineforadd')[0].style.display = "block";
	//document.getElementsByClassName('blockforaddtable')[0].style.display = "block";
	//document.getElementsByClassName('blockforbuttons')[0].style.display = "block";
	loadcalendar(calendarEl);
	
	
};
$('#entry').click(function (e) {
	
	var loginandpassword = [];
	loginandpassword[0] = document.getElementById('login').value;
	//console.log();
	loginandpassword[1] = document.getElementById('password').value;
	//console.log(loginandpassword);
	var json = JSON.stringify(loginandpassword);
	jsonforautorization(json);

})
var changeenable = 0;
$('.changetimetablestart').click(function (e) {
	changeenable = 1;
	chagestart();
});
var sender = [];
$('.changetimetableend').click(function (e) {
	changeenable = 0;
	chagestart();

	for (let j = 1; j < 9; j++) {

		sender[j - 1] = document.getElementById(changedstringid).childNodes[j].innerHTML;
	}
	var json = JSON.stringify(sender);
	jsonforupdate(json);



})
$('.changetimetableadd').click(function (e) {
	changeenable = 0;
	chagestart();

	for (let j = 1; j < 9; j++) {

		sender[j - 1] = document.getElementById(changedstringid).childNodes[j].innerHTML;
	}
	var json = JSON.stringify(sender);
	jsonforadd(json);



})
$('.changetimetabledelete').click(function (e) {
	changeenable = 0;
	chagestart();
	
	var iddelete = document.getElementsByClassName('idfordelete')[0].value;
	
	var json = JSON.stringify(iddelete);
	jsonfordelete(json);



})
$('.sortingtable').click(function (e) {
	var date = document.getElementsByClassName('dateforsorting')[0].value;
	document.location.href = "https://pricheson.tk/Home/Privacy?name=саша&date=" + date;
})
function loadcalendar(calendarEl) {
	
	//var calendar;
	//var calendarEl = document.getElementById('calendar');
	calendarview()
	 function calendarview() {
		calendar = new FullCalendar.Calendar(calendarEl, {
			initialView: 'dayGridMonth',
			initialDate: '2022-08-07',
			allDaySlot: false,
			headerToolbar: {
				left: 'prev,next today',
				center: 'title',
				right: 'dayGridMonth,timeGridWeek,timeGridDay'
			},
			locale: 'ru',
		});
		for (let i = 0; i < event.length; i++) {
			calendar.addEvent(event[i]);
		}
		//calendarEl.style.display = "none";
		calendar.render();
	};
}

var changeenable = 0;
var changedstringid;
function chagestart() {
	var event = $(document).click(function (e) {
		e.stopPropagation();
		e.preventDefault();
		e.stopImmediatePropagation();
		return false;
	});
	if (changeenable == 1) {
		var click = 0;

		$('td').click(function (e) {
			
			//ловим элемент, по которому кликнули
			var t = e.target || e.srcElement;
			if (click == 0) {
				click = 1;

				var bufer = e.target.id;
				if (e.target.id != 'edit' || e.target.parentElement.id == "tr99999999999999999") {
					changedstringid = e.target.parentElement.id;

				}
				if (e.target.id != 'edit' || e.target.parentElement.id != "tr99999999999999999") {
					var teg = e.target.parentElement.id;
					changedstringid = teg;
				}

				var width = $(this).width();

				//console.log(changedstringid);
			}

			//получаем название тега
			var elm_name = t.tagName.toLowerCase();
			//если это инпут - ничего не делаем
			if (elm_name == 'input') { return false; }
			var val = $(this).html();
			var code = '<input type="text" style="width:' + width + "px" + '" id="edit" value="' + val + '" />';
			$(this).empty().append(code);
			$('#edit').focus();
			$('#edit').blur(function () {
				click = 0;
				var val = $(this).val();
				$(this).parent().empty().html(val);

			});
		});
	}
	else {
		$('td').off();
	}

};
$(window).keydown(function (event) {
	//ловим событие нажатия клавиши
	if (event.keyCode == 13) {	//если это Enter
		$('#edit').blur();	//снимаем фокус с поля ввода
		var bufer = e.target.id;
		if (e.target.id != 'edit') {
			var teg = e.target.parentElement.id;
			changedstringid = teg;
		}
		changedstringid = teg;
	}
});



	
