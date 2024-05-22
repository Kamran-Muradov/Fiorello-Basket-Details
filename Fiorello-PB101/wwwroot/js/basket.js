$(function () {
    $(document).on("click", "#products .add-basket", function () {
        let id = parseInt($(this).attr("data-id"));

        $.ajax({
            type: "POST",
            url: `home/addproducttobasket?id=${id}`,
            success: function (response) {
                $(".rounded-circle").text(response.count);
                $(".rounded-circle").next().text(`CART ($${response.totalPrice})`);
            }
        });
    })

    $(document).on("click", "#basket-area .delete-basket", function () {
        let id = parseInt($(this).attr("data-id"));
        var basket = $(this).closest(".basket-item");
        $.ajax({
            type: "POST",
            url: `cart/delete?id=${id}`,
            success: function (response) {
                basket.remove();
                $(".rounded-circle").text(response.count);
                $(".card-header h5").text(`Cart - ${response.count} items`);
                $(".rounded-circle").next().text(`CART ($${response.totalPrice})`);
                $("#basket-area .total-price span").text(`$${response.totalPrice}`);
                $("#basket-area .total-amount strong").text(`$${response.totalPrice}`);
            }
        })
    })
})

