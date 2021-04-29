var lottery={
	index:-1,	//��ǰת�����ĸ�λ�ã����λ��
	count:0,	//�ܹ��ж��ٸ�λ��
	timer:0,	//setTimeout��ID����clearTimeout���
	speed:20,	//��ʼת���ٶ�
	times:0,	//ת������
	cycle:50,	//ת��������������������Ҫת�����ٴ��ٽ���齱����
	prize:-1,	//�н�λ��
	init:function(id){
		if ($("#"+id).find(".lottery-unit").length>0) {
			$lottery = $("#"+id);
			$units = $lottery.find(".lottery-unit");
			this.obj = $lottery;
			this.count = $units.length;
			$lottery.find(".lottery-unit-"+this.index).addClass("active");
		};
	},
	roll:function(){
		var index = this.index;
		var count = this.count;
		var lottery = this.obj;
		$(lottery).find(".lottery-unit-"+index).removeClass("active");
		index += 1;
		if (index>count-1) {
			index = 0;
		};
		$(lottery).find(".lottery-unit-"+index).addClass("active");
		this.index=index;
		return false;
	},
	stop:function(index){
		this.prize=index;
		return false;
	}
};

function roll(){
	lottery.times += 1;
	lottery.roll();
	if (lottery.times > lottery.cycle+10 && lottery.prize==lottery.index) {
		clearTimeout(lottery.timer);
		
		ToMe(lottery.prize);//�齱��ɣ����Կ�ʼ��һ����
		
		lottery.prize=-1;
		lottery.times=0;

		click=false;
		
	}else{
		if (lottery.times<lottery.cycle) {
			lottery.speed -= 10;
		}else if(lottery.times==lottery.cycle) {
			var index = Math.random()*(lottery.count)|0;
			//alert(index);�ó��˳齱������˴����Ƴ齱���
			lottery.prize = index;		
		}else{
			if (lottery.times > lottery.cycle+10 && ((lottery.prize==0 && lottery.index==7) || lottery.prize==lottery.index+1)) {
				lottery.speed += 110;
			}else{
				lottery.speed += 20;
			}
		}
		if (lottery.speed<40) {
			lottery.speed=40;
		};
		//console.log(lottery.times+'^^^^^^'+lottery.speed+'^^^^^^^'+lottery.prize);
		lottery.timer = setTimeout(roll,lottery.speed);
	}
	return false;
}
function ToMe(_prize)
{
	document.getElementById("zhezhaoceng").style.visibility="visible";
	document.getElementById("tanchuangceng").style.visibility="visible";
	document.getElementById("jiangpinxianshi").innerHTML=_prize;
}

function closeTC()
{
	document.getElementById("zhezhaoceng").style.visibility="collapse";
	document.getElementById("tanchuangceng").style.visibility="collapse";
	document.getElementById("lablexianshi").innerHTML=document.getElementById("guanbiinput").value;
	document.getElementById("guanbiinput").value="";
}
var click=false;

window.onload=function(){
	lottery.init('lottery');
	$("#lottery a").click(function(){
		if (click) {
			return false;
		}else{
			lottery.speed=100;
			roll();
			click=true;
			return false;
		}
	});
	
	$(".lucky-btn-ok").click(function(){
		closeTC();
	});
	$(".lucky-btn-more").click(function(){
		closeTC();
	});
};