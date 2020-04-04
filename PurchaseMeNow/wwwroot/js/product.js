var dataTable;

$(document).ready(function () {
    loadDataTable();
})

function loadDataTable() {
    dataTable = $('#tblData').DataTable({
        "ajax": {
            "url": "/Admin/Product/GetAll"
        },
        "language": {
            "infoEmpty": "Nothing found!",
        },
        "columns": [
            { "data": "name", "width": "15%" },
            { "data": "description", "width": "15%" },
            { "data": "category.Name", "width": "15%" },
            { "data": "department.Name", "width": "15%" },
            {
                "data": "id", "render": function (data) {
                    return ` <div class="text-center">
                                <a href="/Admin/Product/Upsert/${data}" class="btn btn-success text-white" style="cursor:pointer">
                                    <i class="fas fa-edit"></i>
                                </a>
                            <a onclick=Delete("/Admin/Product/Delete/${data}") class="btn btn-sm btn-danger text-white" style="cursor:pointer">
                                <i class="fas fa-trash-alt"></i>
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