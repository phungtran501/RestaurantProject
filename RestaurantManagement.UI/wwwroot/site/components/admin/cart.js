(function () {
    const columns = [
        {
            data: 'id', name: 'id', width: '100px',
            render: function (id) {
                return `<span data-key='${id}'><a href=\"#\" title='delete' class='btn-delete'><span class=\"ti-trash\"></span></a>&nbsp
                                    <a href=\"/admin/cart/update?id=${id}\" title='edit'><span class=\"ti-pencil\"></span></a>&nbsp
                                    <a href=\"#\" title='show' class='btn-show'><span class=\"ti-receipt\"></span></span></a>`;
            }
        },
        
        { "data": "username", "name": "username", "autoWidth": true, "width": "150px" },
        { "data": "note", "name": "note", "autoWidth": true, "width": "180px" },
        { "data": "createDate", "name": "createDate", "autoWidth": true, "width": "180px" },
        { "data": "status", "name": "status", "autoWidth": true, "width": "80px", className: 'text-center' },
    ];

    const config = {
        elementName: '#tbl-cart',
        url: '/admin/cart/GetDataCart',
        columns: columns
    };

    initialDatatable(config);

    //show cart detail
    $(document).on('click', '.btn-show', function (e) {

        e.preventDefault();

        const key = $(this).closest('span').data('key');

        $('#cart-id').val(key);
        

        $.ajax({
            url: `/admin/cart/GetDetail?key=${key}`,
            method: 'GET',
            success: function (response) {
                let tr = '';
                let total = 0;
                response.forEach((item, index) => {

                    tr += ` <tr>
                            <td>${item.foodName}</td>
                            <td>${item.foodPrice}</td>
                            <td>${item.quantity}</td>
                            <td>${item.quantity * item.foodPrice}</td>
                       </tr>
                       `;
                    total += item.quantity * item.foodPrice;
                });
                tr += ` <tr>
                            <td colspan="3">Total Price</td>
                            <td>${total}</td>
                       </tr>
                       `;
                $('#body-cart').append(tr);
                $('#cartModal').modal('show');
            
            },
            error: function (error) {

            }

        });


    });

    //CloseModalMenuDetail
    $(document).on('click', '#close-modal', function () {
        closeModal();
    });

    function closeModal() {
        $('#cartModal').modal('hide');
        $('#body-cart').html("");
    }

    //AddRowFood
    //$(document).on('click', '#btn-add-row', function () {
    //    addFood();
    //});

    //function addFood(id = 0, price = 0, foodId = 0, quantity = 0) {
    //    const optionFood = $('#ls-food').html();

    //    const row = ` <tr class="add-row-food" data-key="${id}">
    //                        <td><select class="form-control ddl-food ddd_${id}">${optionFood}</select></td>
    //                        <td><input type="number" class="form-control txt-price" value="0" /></td>
    //                        <td class="text-center padding-row-add"><a href="#" class="btn-delete-row"><span class="ti-close"></span></a></td>
    //                   </tr>`;
    //    $('#body-food').append(row);

    //    if (foodId) {
    //        $(`.ddd_${id}`).val(foodId);
    //    }

    //}


})()
