﻿(function () {
    const columns = [
        { "data": "id", "name": "id", "autoWidth": true, "width": "100px" },
        { "data": "username", "name": "username", "autoWidth": true, "width": "150px" },
        { "data": "roleName", "name": "roleName", "autoWidth": true, "width": "180px" },
        { "data": "fullname", "name": "fullname", "autoWidth": true, "width": "180px" },
        { "data": "email", "name": "email", "autoWidth": true },
        //{ "data": "address", "name": "address", "autoWidth": true },
        //{ "data": "phoneNumber", "name": "phoneNumber", "autoWidth": true },
        { "data": "isSystem", "name": "isSystem", "autoWidth": true, "width": "80px", className: 'text-center' },
        //{ "data": "isActive", "name": "isActive", "autoWidth": true, "width": "80px", className: 'text-center' }
    ];

    const config = {
        elementName: '#tbl-account',
        url: '/admin/account/getDataAccount',
        //url: '/admin/role/getDataRole',
        columns: columns
    };

    initialDatatable(config);

    $(document).on('click', '.btn-delete', function (event) {

        event.preventDefault();

        const key = $(this).closest('span').data('key');

        confirmJQ('Confirm', 'Are you sure to delete?', function () {

            $.ajax({
                url: `/admin/account/delete?key=${key}`,
                method: 'DELETE',
                dataType: 'json',
                success: function (response) {
                    var toaster = new toasterNaranja('Confirm', 'Delete successful');
                    toaster.log('Confirm', 'Delete successful');
                    $('#tbl-account').DataTable().ajax.reload();
                },
                error: function (error) {

                }

            });


        }, function () {

        })
    });

    $(document).on('click', '.btn-show', function (e) {

        e.preventDefault();

        $('#accountModal').modal('show');

        //$.ajax({
        //    url: `/admin/account/get-infomation-user/${key}`,
        //    method: 'GET',
        //    success: function (response) {




        //    }




        //});
    });
})()