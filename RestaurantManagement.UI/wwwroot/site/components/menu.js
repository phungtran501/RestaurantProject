(function (e) {

    e.preventDefault();

    const config = {
        url: `/food/GetFood/${key}`,
        method: 'GET',
        success: function (response) {

            let li = '';


            response.forEach((food, index) => {
                li += `<li>
                            <div class="delicious">
                                <h6>${food}</h6>
                                <span>$12.00</span>
                            </div>
                            <p>Sausage, three rashers of streaky bacon, two fried eggs</p>
                        </li>`;
            })


        },
        error: function (error) {

        }
    };

})();

