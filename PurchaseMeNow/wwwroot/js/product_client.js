﻿var dataTable;

$(document).ready(function () {
    loadDataTable();
})

function loadDataTable() {
    dataTable = $('#tblData').DataTable({
        "ajax": {
            "url": "/Client/Product/GetAll"
        },
        "language": {
            "infoEmpty": "Nothing found!",
        },
        "columns": [
            { "data": "name", "width": "25%" },
            { "data": "description", "width": "25%" },
            { "data": "category.name", "width": "20%" },
            { "data": "department.name", "width": "20%" },
            {
                "data": "id", "render": function (data) {
                    return ` <div class="text-center">
                                <a href="/Client/Product/AddToOrder/${data}" class="btn btn-sm btn-success text-white" style="cursor:pointer">
                                    <i class="fas fa-shopping-cart"></i>
                                </a>
                          
                        </div>
`
                }, "width": "40%"
            }
        ]

    });
}

function Delete(url) {
    swal({
        title: "Are you sure you want to Delete?",
        text: "All deletions are permanent",
        icon: "warning",
        buttons: true,
        dangerMode: true

    }).then((willDelete) => {
        if (willDelete) {
            $.ajax({
                type: "DELETE",
                url: url,
                success: function (data) {
                    if (data.success) {
                        toastr.success(data.message);
                        dataTable.ajax.reload();
                    }
                    else {
                        toastr.error(data.message);
                    }
                }
            });
        }
    });
}