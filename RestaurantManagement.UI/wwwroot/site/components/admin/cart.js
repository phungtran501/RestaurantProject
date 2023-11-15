(function () {
    const columns = [
        { "data": "id", "name": "id", "autoWidth": true, "width": "100px" },
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
            url: `/admin/cart/GetDetail?id=${key}`,
            method: 'GET',
            success: function (response) {
                let tr = '';
                response.forEach((item, index) => {

                    tr += ` <tr>
                            <td></td>
                            <td></td>
                            <td></td>
                            <td></td>
                       </tr>`;
                });
                $('#body-cart').append(row);
                $('#menuModal').modal('show');
            
            },
            error: function (error) {

            }

        });


    });


})()
