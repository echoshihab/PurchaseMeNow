var dataTable;
const params =  ["inprocess", "pending", "rejected", "shipped", "delivered"];

$(document).ready(function () {
    let url = window.location.search;
    let result = params.find(param => url.includes(param));
    if (typeof (result) != "undefined") {
    loadDataTable(`GetOrderList?status=${result}`);
    }
    else {
        loadDataTable("GetOrderList?status=all");
    }
});

function loadDataTable(url) {
    dataTable = $('#tblData').DataTable({
        "ajax": {
            "url": "/Admin/Order/" + url
        },
        "language": {
            "infoEmpty": "Nothing found!",
        },
        "columns": [
            {
                "data": "id", "render": function (data) {
                    return ` <div class="text-center">
                                <a href="/Admin/Order/Details/${data}" class="btn btn-sm btn-info text-white" style="cursor:pointer">
                                    <i class="fas fa-edit"></i>
                                </a>
                            
                        </div>
`
                }, "width": "10%"
            },
            { "data": "applicationUser.name", "width": "40%" },
            { "data": "orderStatus", "width": "40%" },

            
            
        ]

    });
}

