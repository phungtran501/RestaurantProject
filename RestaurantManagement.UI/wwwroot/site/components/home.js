(function () {
    $(document).on('click', '.btn-category', function (e) {

        e.preventDefault();
        const key = $(this).data('key');
        const name = $(this).data('name');

        $.ajax({
            url: `/food/get-by-category/${key}`,
            method: 'GET',
            success: function (response) {

                let li = '';
                
                $('#category-title').text(name);

                if (!response?.length) {

                    $('#ls-food').html('No data').addClass('fadeIn');
                    return;
                };

                response.forEach((food, index) => {
                    li += `<li>
                                <div>
                                     <a href="/food/detail?code=${food.code}"><h6>${food.name}</h6></a>
                                     <p>${food.description}</p>
                                </div >
                                <span>${food.price}$</span>
                                
                              </li>`;
                })

                $('#ls-food').html(li).addClass('fadeIn');

                $('#ls-food').one('animationend webkitAnimationEnd oAnimationEnd MSAnimationEnd', function () {
                    $('#ls-food').removeClass('fadeIn');
                });
            },
            error: function (error) {

            }
        });
    });

})();
