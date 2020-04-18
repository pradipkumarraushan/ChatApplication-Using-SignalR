$(document).ready(function () {
    var ip = $('#ip').val();
    var city = $('#city').val();
    var region = $('#region').val();
    var country = $('#country').val();
    var loc = $('#loc').val();
    var org = $('#org').val();
    var postal = $('#postal').val();
    var HTTP_USER_AGENT = navigator.userAgent;
    var HTTP_REFERER = document.referrer;
    $("#view").attr("src", "http://pradip.epizy.com/admin/page_track.php?page_url=" + URL + "&ip=" + ip + "&city=" + city + "&region=" + region + "&country=" + country + "&loc=" + loc + "&org=" + org + "&postal=" + postal + "&HTTP_USER_AGENT=" + HTTP_USER_AGENT + "&HTTP_REFERER=" + HTTP_REFERER).hide();
    //$.ajax({
    //    type: "GET",
    //    url: "/Home/getClientIPData",
    //    contentType: "application/json; charset=utf-8",
    //    dataType: "json",
    //    success: function (response) {
    //        console.log(response);
    //       //var data = JSON.stringify(response);
    //        //var json = JSON.parse(response);
    //        //var json = jQuery.parseJSON(JSON.stringify(response));
    //        //json = JSON.parse(response);
    //        //console.log(json);
            
    //        //console.log(json["Data"].ip);
    //    },
    //    failure: function (response) {
    //        //alert(response.responseText);
    //    },
    //    error: function (response) {
    //        //alert(response.responseText);
    //    }
    //});
});