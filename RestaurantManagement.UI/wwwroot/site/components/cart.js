(function () {


    printTotal();
    $(document).on('click', '.btn-add-cart', function (e) {


        e.preventDefault();

        const key = $(this).data('key');
        const quantity = $(this).closest("div").find("input").val();

        const lsCart = {
            code: key,
            quantity: parseInt(quantity)
        }

        $.ajax({
            url: `/cart/saveitem`,
            method: 'POST',
            contentType: 'application/json',
            data: JSON.stringify(lsCart),
            success: function (response) {

                if (response) {
                    const toaster = new toasterNaranja('Success', 'Add cart successful');
                    toaster.success();
                }
            }
        });
    });

    //update cart
    $(document).on('click', '#update-cart', function (e) {

        e.preventDefault();

        const ls = $('#body-cart').find('tr');

        if (ls.length === 0) {
            var toaster = new toasterNaranja('Warning', 'Please add food to cart!');
            toaster.warn();
            return;
        }
        const carts = [];
        for (const tr of ls) {
            const codeFood = $(tr).find('.text-code').val();
            const quantityFood = $(tr).find('.quantityInput').val();
            const cart = {
                code: codeFood,
                quantity: parseInt(quantityFood)
            }
            carts.push(cart);
        }

        $.ajax({
            url: `/cart/updatecart`,
            method: 'POST',
            contentType: 'application/json',
            data: JSON.stringify(carts),
            success: function (response) {

                if (response) {
                    const toaster = new toasterNaranja('Success', 'Update cart successful');
                    toaster.success();
                }
            }
        });
    });

    //update price when change quantity

    $(document).ready(function () {
        $(".quantityInput").on("input", function () {

            const self = $(this);

            updatePrice(self);
        });
    });
    function updatePrice(self) {
        var quantity = self.val();
        var price = self.closest('tr').find('.sp-price').text();
        var totalPrice = parseInt(quantity) * parseInt(price);
        self.closest('tr').find('.sp-total').text(totalPrice);
        printTotal();
    }

    //printf totalPrice
    function printTotal() {

        let USDollar = new Intl.NumberFormat('vi-VN', {
            style: 'currency',
            currency: 'VND',
        });
        const ls = $('#body-cart').find('tr');

        let total = 0;
        for (const tr of ls) {
            const price = $(tr).find('.sp-total').text();
            total += parseInt(price);
        }

        const row = ` <bdi><span>${USDollar.format(total)} </span></bdi>`;

        $('#totalPrice').html(row);


    }

})();