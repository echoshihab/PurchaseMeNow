
$(document).ready(function () {
    loadNotifications();
})



function loadNotifications() {
    

    $.ajax({
        type: "GET",
        url: "/Admin/Order/GetOrderCount",
        success: function (data) {
            Object.keys(data).forEach(k =>
                document.getElementById(k).innerText = data[k]
            );

        }
    });
}
