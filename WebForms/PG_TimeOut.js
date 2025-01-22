<Script>
	var intSeconds = 600;
    function countDown()
    {
		if(intSeconds <= 0) 
        {
			alert("Your session has been terminated. Please Sign In again.");
            document.location.href = "PG_Logout.aspx?Timed=True";
		}
		intSeconds = intSeconds - 1;
		window.setTimeout("countDown()",6000);    
    }
    function resetCounter()
    {
        intSeconds = 600;
    }
		
		var message = "Sorry! We have disabled this function for your Security";
		///////////////////////////////////
		function clickIE() {if (document.all) {(message);return false;}}
		function clickNS(e) {if 
		(document.layers||(document.getElementById&&!document.all)) {
		if (e.which==2||e.which==3) {(message);return false;}}}
		if (document.layers) 
		{document.captureEvents(Event.MOUSEDOWN);document.onmousedown=clickNS;}
		else{document.onmouseup=clickNS;document.oncontextmenu=clickIE;}

		document.oncontextmenu=new Function("return false")
		// --> 
</Script>