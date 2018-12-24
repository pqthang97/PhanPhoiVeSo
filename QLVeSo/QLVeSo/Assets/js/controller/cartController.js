var cart = {
    init: function () {
        cart.regEvents();
        
    },
    
    regEvents: function () {
        
        
        $('#btnAdd').off('click').on('click', function () {
            window.location.href = "/";
        });
        $('#btnContinue').off('click').on('click', function () {
            window.location.href = "/";
        });
        $('#btnPayment').off('click').on('click', function () {
            window.location.href = "/Cart/Payment";
        });
        $('#btnUpdate').off('click').on('click', function () {
            var listProduct = $('.txtQuantity');
            var cartList = [];
            $.each(listProduct, function (i, item) {
                cartList.push({
                    soluong: $(item).val(),
                    SANPHAM: {
                        MaSP: $(item).data('id')
                    }
                });
            });

            $.ajax({
                url: '/Cart/Update',
                data: { cartModel: JSON.stringify(cartList) },
                dataType: 'json',
                type: 'POST',
                success: function (res) {
                    if (res.status == true) {
                        //alert("Đã update giỏ hàng");
                        window.location.href = "/Cart/IndexCart";
                    }
                    if (res.status == false) {

                        alert("Số lượng không đủ");
                        window.location.href = "/Cart/IndexCart";
                        
                    }


                }
            })
        });

        $('#btnDeleteAll').off('click').on('click', function () {


            $.ajax({
                url: '/Cart/DeleteAll',
                dataType: 'json',
                type: 'POST',
                success: function (res) {
                    if (res.status == true) {
                        window.location.href = "/Cart/IndexCart";
                    }
                }
            })
        });

        $('.btn-delete').off('click').on('click', function (e) {
            e.preventDefault();
            $.ajax({
                data: { id: $(this).data('id') },
                url: '/Cart/Delete',
                dataType: 'json',
                type: 'POST',
                success: function (res) {
                    if (res.status == true) {

                        window.location.href = "/Cart/IndexCart";
                    }
                }
            })
        });
    }
}
cart.init();