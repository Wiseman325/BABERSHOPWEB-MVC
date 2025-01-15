var dataTable;

$(Document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#tblTable').DataTable({
        "ajax": { url: '/admin/product/getall' },
        "columns": [
            { data: 'name', "width": "20%" },
            { data: 'description', "width": "25%" },
            { data: 'listPrice', "width": "10%" },
            { data: 'quantity', "width": "10%" },
            { data: 'productType.productType_name', "width": "20%" },
            {
                data: 'id',
                "render": function (data) {
                    return ` <div class="w-75 btn-group" role="group">
                                    <a href="/admin/product/upsert?id=${data}" class="btn btn-primary mx-2 ">
                                        <i class="bi bi-pencil-square me-1"></i> Edit
                                    </a>
                                    <a onClick=Delete('/admin/product/upsert/${data}') class="btn btn-danger mx-2 ">
                                        <i class="bi bi-trash me-1"></i> Delete
                                    </a>
                             </div>`
                }
                "width": "20%"
            }
        ]
    });
}

function Delete(url) {
    //sweet alert code for confirmation
    if (result.isConfirmed) {
        $.ajax({
            url: url,
            type: 'DELETE',
            success: function (data) {
                dataTable.ajax.reload();
                toastr.success(data.message);
            }
        })
    }
}