function call() {
    $.ajax({
        type: "POST",
        url: "/WebServices/test.aspx/sayHello",
        data: "{}",
        contentType: "application/json",
        dataType: "json",
        success: function (msg) {
            // Replace the div's content with the page method's return.
            $("#Result").text(msg.d);
        },
        error: function (result) {
            alert( '[' + result.status + ']:' + result.statustext);
        }
    });
}

