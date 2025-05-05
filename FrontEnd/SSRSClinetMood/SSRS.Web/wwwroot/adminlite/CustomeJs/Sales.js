$(function () {
    $('.select2').select2()
});

$(document).ready(function () {
    GetSalesListOnPageLoad();
    $(document).on("click", "#btnSave", function () {
        var isValid = true;
        isValid = RequiredSubmitFunction();

        if (isValid) {
            var Id = $("#Id").val();
            var ProductId = $("#ProductId").val();
            var QuantitySold = $("#QuantitySold").val();
            
            var postData = {
                Id: Id,
                ProductId: ProductId,
                QuantitySold: QuantitySold,
               
            };

            $.ajax({
                url: "/Sales/Sales/CreateSale",
                type: "POST",
                contentType: "application/json",          
                data: JSON.stringify(postData),    
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
                        GetSalesListOnPageLoad();
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
    $("#ProductId").change(function () {
        var productId = $(this).val();

        if (productId && productId !== "0") {
            $.ajax({
                url: "/Sales/Sales/GetProductById",
                type: "GET",
                data: { id: productId },
                success: function (response) {
                    console.log(response)
                    if (response.success) {
                        var stockQty = response.product.stockQty;
                        $("#StockQty").val(stockQty);
                    } else {
                        Swal.fire({
                            icon: 'error',
                            title: 'Error',
                            text: response.Message
                        });
                    }
                },
                error: function () {
                    Swal.fire({
                        icon: 'error',
                        title: 'Error',
                        text: 'Unable to fetch stock quantity. Please try again.'
                    });
                }
            });
        }
    });

});

function ClearUI() {

    $("#Id").val(0);
    $("#ProductId").val('');
    $("#QuantitySold").val(0);
    $("#StockQty").val(0);
   
}
function GetSalesListOnPageLoad() {
    $("#SalesList").html("");

    $.ajax({
        type: "POST",
        url: "/Sales/Sales/SalesList",
        success: function (response) {
            $("#SalesList").html(response);
            $('#tblSales').DataTable({
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


