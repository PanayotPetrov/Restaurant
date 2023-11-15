// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.


function ChangeMenuItemQuantity(id) {

    var mealContainer = document.getElementById(id);
    var quantity = mealContainer.querySelector('input');

    var quantityValue = Number.parseInt(quantity.getAttribute('value'));

    var button = event.srcElement;

    if (button.classList.contains('btn-plus')) {
        quantityValue++;
    }
    else {
        if (quantityValue > 1) {
            quantityValue--;
        }
    }

    quantity.setAttribute('value', quantityValue);
}

function AddToCart(id) {

    var mealId = id.substring(13);
    var mealContainer = document.getElementById(id);
    var quantity = mealContainer.querySelector('input').value;

    const data =
    {
        mealId: mealId,
        quantity: quantity
    };

    $.ajax({
        url: '/Cart/AddToCart',
        type: 'POST',
        data: JSON.stringify(data),
        contentType: 'application/json',
        success: function (result, status, xhr) {
            if (xhr.status === 200) {
                $("#modalLabel").text('Success');
                $("#modalBody").text('You have successfully added the item to your cart!');
                $("#exampleModal").modal('show');

                var currentCartTotalPrice = JSON.parse(xhr.responseText);
                $("#cartSubTotalBtn").html('<i class="fa-solid fa-cart-shopping"></i> ' + currentCartTotalPrice + ' $');
            }
        },
        error: function (xhr) {
            $("#modalLabel").text('Oops!');
            if (xhr.responseText.length == 0) {

                $("#modalBody").append('<li class="text-danger"> You need to log in to add items to your cart! </li>')
            }
            else {
                var errors = JSON.parse(xhr.responseText);
                for (var i = 0; i < errors.length; i++) {
                    $("#modalBody").append('<li class="text-danger">' + errors[i] + '</li>')
                }
            }
            $("#exampleModal").modal('show')
        }
    });

    $("#btnCloseModal").click(function () {
        $("#exampleModal").modal("hide");
        $("#modalBody").text('');
    });
}

function GetAddress() {
    var addressName = $('#addressNames').val()
    var cartItemsCount = $('#cartItemsCount').val()


    $.ajax({
        url: '/Checkout/GetAddressDetails',
        type: 'GET',
        data: {
            addressName: addressName,
            cartItemsCount: cartItemsCount
        },
        success: function (result, status, xhr) {
            if (xhr.status === 200) {
                var jsonResult = JSON.parse(xhr.responseText);
                $('#street').val(jsonResult.street);
                $('#addressLine2').val(jsonResult.addressLineTwo);
                $('#district').val(jsonResult.district);
                $('#city').val(jsonResult.city);
                $('#country').val(jsonResult.country);
                $('#postCode').val(jsonResult.postCode);
                $('#addressId').val(jsonResult.id);

            }
        }
    });
}

function ChangeCartItemQuantity(id) {

    var mealId = id.substring(17);
    var cartItemContainer = document.getElementById(id);
    var quantity = cartItemContainer.querySelector('input').value;

    var button = event.srcElement;

    if (button.classList.contains('btn-plus')) {

        quantity++;

        if (quantity <= 100) {
        }
    }
    else {

        quantity--;

        if (quantity >= 1) {
        }
    }

    const data =
    {
        mealId: mealId,
        quantity: quantity
    };

    $.ajax({
        url: '/Cart/ChangeQuantity',
        type: 'POST',
        data: JSON.stringify(data),
        contentType: 'application/json',
        success: function (result, status, xhr) {
            if (xhr.status === 200) {
                var jsonResult = JSON.parse(xhr.responseText);

                cartItemContainer.querySelector('input').setAttribute('value', jsonResult.quantity);
                $("#cartSubTotalBtn").html('<i class="fa-solid fa-cart-shopping"></i> ' + jsonResult.cartSubTotal + ' $');
                $("#cartSubTotalCard").html(jsonResult.cartSubTotal + ' $');
                $("#cartItemTotalPrice" + mealId).html(jsonResult.itemTotalPrice + ' $');
                $("#cartTotalPrice").html(jsonResult.cartTotalPrice + ' $');
            }
        },
        error: function (xhr) {
            $("#modalLabel").text('Oops!');
            var errors = JSON.parse(xhr.responseText);
            for (var i = 0; i < errors.length; i++) {
                $("#modalBody").append('<li class="text-danger">' + errors[i] + '</li>')
            }

            $("#exampleModal").modal('show')
        }
    });

    $("#btnCloseModal").click(function () {
        $("#exampleModal").modal("hide");
        $("#modalBody").text('');
    });
}

function RemoveFromCart(id) {

    var mealId = id.substring(17);

    const data =
    {
        mealId: mealId,
    };

    $.ajax({
        url: '/Cart/RemoveFromCart',
        type: 'POST',
        data: JSON.stringify(data),
        contentType: 'application/json',
        success: function (result, status, xhr) {
            if (xhr.status === 200) {
                var itemPrice = Number.parseFloat($("#cartItemTotalPrice" + mealId).text());


                var cartSubTotalBtn = Number.parseFloat($("#cartSubTotalBtn").text());
                console.log(cartSubTotalBtn);
                var cartSubTotalCard = Number.parseFloat($("#cartSubTotalCard").text());
                var cartTotal = Number.parseFloat($("#cartTotalPrice").text());


                $("#cartSubTotalBtn").html('<i class="fa-solid fa-cart-shopping"></i> ' + (cartSubTotalBtn - itemPrice).toFixed(2) + ' <i class="fas fa-dollar-sign"></i>');
                $("#cartSubTotalCard").html((cartSubTotalCard - itemPrice).toFixed(2) + ' <i class="fas fa-dollar-sign"></i>');
                $("#cartTotalPrice").html((cartTotal - itemPrice).toFixed(2) + ' <i class="fas fa-dollar-sign"></i>');
                $("#" + id).remove();
            }
        }
    });
}