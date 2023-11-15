(function () {
    const columns = [
        { "data": "id", "name": "id", "width": "100px" },
        { "data": "name", "name": "name", "autoWidth": true },
        { "data": "isDisplay", "name": "isDisplay", "width": "130px" },
    ];

    const config = {
        elementName: '#tbl-menu',
        url: '/admin/menu/getDataMenu',
        columns: columns
    };

    initialDatatable(config);

    //DeleteMenu
    $(document).on('click', '.btn-delete', function (event) {

        event.preventDefault();

        const spanParent = $(this).closest('span');

        const key = spanParent.data('key');



        confirmJQ('Confirm', 'Are you sure to delete?', function () {

            $.ajax({
                url: `/admin/menu/delete?key=${key}`,
                method: 'DELETE',
                dataType: 'json',
                success: function (response) {
                    var toaster = new toasterNaranja('Success', 'Delete successful');
                    toaster.log('Success', 'Delete successful');
                    $('#tbl-menu').DataTable().ajax.reload();
                },
                error: function (error) {

                }

            });


        }, function () {

        })
    });

    //ShowModelMenuDetail
    $(document).on('click', '.btn-add', function (e) {

        e.preventDefault();

        const spanParent = $(this).closest('span');

        const key = spanParent.data('key');

        $('#menu-id').val(key);

        $.ajax({
            url: `/admin/menu/GetFoodByMenu?id=${key}`,
            method: 'GET',
            success: function (response) {

                response.forEach((item, index) => {
                    addFood(item.id, item.price, item.foodId);
                });
                $('#menuModal').modal('show');
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
        $('#menuModal').modal('hide');
        $('#body-food').html("");
    }

    //AddRowFood
    $(document).on('click', '#btn-add-row', function () {
        addFood();
    });

    function addFood(id = 0, price = 0, foodId = 0) {
        const optionFood = $('#ls-food').html();

        const row = ` <tr class="add-row-food" data-key="${id}">
                            <td><select class="form-control ddl-food ddd_${id}">${optionFood}</select></td>
                            <td class="d-none"><input type="number" class="form-control txt-price" value="0" /></td>
                            <td class="text-center padding-row-add"><a href="#" class="btn-delete-row"><span class="ti-close"></span></a></td>
                       </tr>`;
        $('#body-food').append(row);

        if (foodId) {
            $(`.ddd_${id}`).val(foodId);
        }

        

    }

    //DeleteRowFood
    $(document).on('click', '.btn-delete-row', function (e) {
        e.preventDefault();
        const self = $(this);

        const key = self.closest('tr').data('key');

        if (key === 0) {
            self.closest('tr').remove();
        }
        else {
            confirmJQ('Confirm', 'Are you sure to delete?', function () {

                $.ajax({
                    url: `/admin/menu/DeleteMenuDetail?key=${key}`,
                    method: 'delete',
                    datatype: 'json',
                    success: function (response) {
                        var toaster = new toasterNaranja('Success', 'Delete successful');
                        toaster.log('Success', 'Delete successful');
                        self.closest('tr').remove();
                    }
                });
            })
        }
    });

    //InsertDataMenuDetail
    $(document).on('click', '#btn-submit-food', function () {
        const ls = $('#body-food').find('tr');

        if (ls.length === 0) {
            var toaster = new toasterNaranja('Warning', 'Please add data to save!');
            toaster.warn();
            return;
        }

        const foods = [];
        const tempFoods = [];

       for (const tr of ls) {
            const foodId = $(tr).find('.ddl-food').val();
            const food = {
                menuId: parseInt($('#menu-id').val()),
                id: parseInt($(tr).data('key')),
                foodId: parseInt(foodId),
                price: parseInt($(tr).find('.txt-price').val()),
            }

           //
           if (tempFoods.includes(foodId))
           {
               var toaster = new toasterNaranja('Warning', 'Menu has duplicated food, please check again!');
               toaster.warn();
               return;
           }
           foods.push(food);
           tempFoods.push(foodId);
        }

        $.ajax({
            url: `/admin/menu/InsertMenuDetail`,
            method: 'POST',
            contentType: 'application/json',
            data: JSON.stringify(foods),
            success: function (response) {
                //if ok --> update display success
                closeModal();
                var toaster = new toasterNaranja('Insert', 'Insert successful');
                toaster.success('Insert', 'Insert successful');
            }
        })
    })

    //UpdateDisplay
    $(document).on('click', '.ck-display', function (event) {


        event.preventDefault();
        const key = $(this).closest('.even').find('span').data('key');

        const isDisplay = $(this).is(":checked");


        confirmJQ('Confirm', 'Are you sure to update display?', function () {

            $.ajax({
                url: `/admin/menu/UpdateDisplay`,
                method: 'PUT',
                data: {
                    key,
                    isDisplay
                },
                success: function (response) {
                    //if ok --> update display success
                    var toaster = new toasterNaranja('Update Display', 'Update successful');
                    toaster.log('Update Display', 'Update successful');
                    
                },
                error: function (error) {

                }
                
            })

        }, function () {

        })

    });
})()


