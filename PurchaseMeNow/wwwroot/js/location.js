var dataTable;

$(document).ready(function () {
    loadDataTable();
})

function loadDataTable() {
    dataTable = $('#tblData').DataTable({
        "ajax": {
            "url": "/Admin/Location/GetAll"
        },
        "language": {
            "infoEmpty": "Nothing found!",
        },
        "columns": [
           
            { "data": "name", "width": "80%" },
            {
                "data": "id", "render": function (data) {
                    return ` <div class="text-center">
                                <a href="/Admin/Location/Upsert/${data}" class="btn btn-sm btn-info text-white" style="cursor:pointer">
                                    <i class="fas fa-edit"></i>
                                </a>
                            <a onclick=Delete("/Admin/Location/Delete/${data}") class="btn btn-sm btn-danger text-white" style="cursor:pointer">
                                <i class="fas fa-trash-alt"></i>
                            </a>
                        </div>
`
                }, "width": "20%"
            },

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