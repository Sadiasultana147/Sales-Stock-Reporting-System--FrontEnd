
$(document).ready(function () {
    GetProductListOnPageLoad();
    $(document).on("click", "#btnSave", function () {
        var isValid = true;
        isValid = RequiredSubmitFunction();

        if (isValid) {
            var Id = $("#Id").val();
            var ProductName = $("#ProductName").val();
            var SKUCode = $("#SKUCode").val();
            var StockQuantity = $("#StockQuantity").val();
            var Price = $("#Price").val();
            var Description = $("#Description").val();

            var postData = {
                Id: Id,
                ProductName: ProductName,
                SKUCode: SKUCode,
                StockQuantity: StockQuantity,
                Price: Price,
                Description: Description
            };

            $.ajax({
                url: "/Products/Product/CreateOrUpdate",
                type: "POST",
                data: postData,
                success: function (response) {
                    if (response.success) {
                        Swal.fire({
                            icon: 'success',
                            title: 'Saved',
                            text: response.message,
                            timer: 3000,
                            showConfirmButton: false
                        });

                        ClearUI();
                        GetProductListOnPageLoad();
                    }
                    else {
                        Swal.fire({
                            icon: 'error',
                            title: 'Error',
                            text: response.message
                        });
                    }
                },
                error: function () {
                    Swal.fire({
                        icon: 'error',
                        title: 'Error',
                        text: 'Something went wrong. Please try again.'
                    });
                }
            });
        }
    });


});



function ClearUI() {

    $("#Id").val('0');
    $("#ProductName").val('');
    $("#StockQuantity").val(0);
    $("#SKUCode").val('');
    $("#Price").val(0);
    $("#Description").val('');
    $("#btnSave").html('Submit');
    $("#btnSave").removeClass("btn btn-md btn-success").addClass('btn btn-md btn-primary');
}
function GetProductListOnPageLoad() {
    $("#ProductList").html("");

    $.ajax({
        type: "POST",
        url: "/Products/Product/ProductList",
        success: function (response) {
            $("#ProductList").html(response);
            $('#tblProduct').DataTable({
                "paging": true,
                "pageLength": 10,
                "lengthChange": true,
                "searching": true,
                "ordering": true,
                "info": true,
                "autoWidth": false,
                "responsive": true,
            });

        }
    })
}
function GetProductFunction(Id) {
    var param = {};
    param.Id = Id;

    $.get("/Products/Product/GetProduct", param, function (res) {
        if (!res.success) {
            
        }
        else {
            $("#Id").val(res.record.id);
            $("#ProductName").val(res.record.name);
            $("#SKUCode").val(res.record.sku);
            $("#Price").val(res.record.price);
            $("#StockQuantity").val(res.record.stockQty);
            $("#Description").val(res.record.description);
            $("#btnSave").html('Update');
            $("#btnSave").removeClass("btn btn-md btn-primary").addClass('btn btn-md btn-success');
        }
    });
}

function deleteProduct(productId) {
    Swal.fire({
        title: 'Are you sure?',
        text: "This will mark the product as deleted.",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes, delete it!'
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: '/Products/Product/Delete',
                type: 'POST',
                data: { id: productId },
                success: function (response) {
                    if (response.success) {
                        Swal.fire({
                            icon: 'success',
                            title: 'Deleted',
                            text: response.message,
                            timer: 3000,
                            showConfirmButton: false
                        });

                        ClearUI();
                        GetProductListOnPageLoad();
                    } else {
                        Swal.fire({
                            icon: 'error',
                            title: 'Error',
                            text: response.message
                        });
                    }
                },
                error: function () {
                    Swal.fire({
                        icon: 'error',
                        title: 'Error',
                        text: 'Something went wrong. Please try again.'
                    });
                }
            });
        }
    });
}

