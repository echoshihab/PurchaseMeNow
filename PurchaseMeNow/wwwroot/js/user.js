var dataTable;

$(document).ready(function () {
    loadDataTable();
})

function loadDataTable() {
    dataTable = $('#tblData').DataTable({
        "ajax": {
            "url": "/Admin/User/GetAll"
        },
        "language": {
            "infoEmpty": "Nothing found!",
        },
        "columns": [
            { "data": "name", "width": "25%" },
            { "data": "email", "width": "25%" },
            { "data": "department.name", "width": "15%" },
            { "data": "role", "width": "15%" },
            {
                "data": {
                    id: "id", lockoutEnd: "lockoutEnd"
                },
                "render": function (data) {
                    var today = new Date().getTime();
                    var lockout = new Date(data.lockoutEnd).getTime();
                    if (lockout > today) {
                        return ` <div class="text-center">
                            <a onclick=ToggleLock('${data.id}') class="btn btn-sm btn-danger text-white" style="cursor:pointer">
                                <i class="fas fa-lock-open"></i> Unlock 
                            </a>
                        </div>
                            `;
                    }
                    else {
                        return ` <div class="text-center">
                            <a onclick=ToggleLock('${data.id}') class="btn btn-sm btn-success text-white" style="cursor:pointer">
                                <i class="fas fa-lock"></i> Lock 
                            </a>
                        </div>
                            `;
                    }

                },"width": "20%"

            }
        ]

    });
}

function ToggleLock(id){

            $.ajax({
                type: "POST",
                url: '/Admin/User/ToggleLock',
                data: JSON.stringify(id),
                contentType: "application/json", 
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
  