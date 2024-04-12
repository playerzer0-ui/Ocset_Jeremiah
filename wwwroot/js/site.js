// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
function addToCart(productId) {
    var quantity = document.getElementById("quantityInput" + productId).value;
    var itemName = document.getElementById("itemName" + productId).innerHTML;

    // Retrieve existing cart data from local storage
    var cartData = localStorage.getItem('cart');
    var cart = cartData ? JSON.parse(cartData) : {};

    // Update cart with new item
    cart[productId] = {
        productId: productId,
        productName: itemName,
        quantity: quantity
    };

    // Save updated cart data back to local storage
    localStorage.setItem('cart', JSON.stringify(cart));
    console.log("cart add");
}

function displayCart() {
    var cartData = localStorage.getItem('cart');
    var tBody = document.getElementById("tableBody");

    if (cartData) {
        // Parse the cart data as JSON
        var cart = JSON.parse(cartData);
        tBody.innerHTML = "";

        for (var productId in cart) {
            var item = cart[productId];
            var id = item.productId;
            var productName = item.productName;
            var quantity = item.quantity;

            // Do something with productName and quantity
            tBody.innerHTML += "<tr>" +
                "<td>" + id + "</td>" +
                "<td>" + productName + "</td>" +
                "<td>" + quantity + "</td>" +
                '<td><button class="btn btn-danger">remove button</button></td>' +
                "</tr>";
        }
    } else {
        // Handle case where cart data is not found in local storage
        console.log("Cart is empty");
    }
}


function removeFromCart() {

}