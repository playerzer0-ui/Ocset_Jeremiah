
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
    alert("item added");
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
                '<td><button class="btn btn-danger" onclick="removeItem(this.parentElement.parentElement)">Remove Item</button></td>' +
                "</tr>";
        }
    } else {
        // Handle case where cart data is not found in local storage
        console.log("Cart is empty");
    }
}

function removeItem(row) {
    // Remove the row from the webpage
    row.remove();

    // Remove the corresponding item from local storage
    var cartData = localStorage.getItem('cart');
    if (cartData) {
        var cart = JSON.parse(cartData);
        var productId = row.cells[0].textContent;
        delete cart[productId];
        localStorage.setItem('cart', JSON.stringify(cart));
    }
}

function placeOrder() {
    var orders = [];
    var userId = $("#userId").val();

    $("#tableBody tr").each(function () {
        var productId = $(this).find("td:eq(0)").text();
        var quantity = $(this).find("td:eq(2)").text();

        var order = {
            OrderDate: new Date().toISOString(), // Set to current date
            Quantity: parseInt(quantity),
            CustomerId: parseInt(userId),
            ProductId: parseInt(productId)
        };
        orders.push(order);
    });

    console.log(orders);

    // Use jQuery AJAX to submit the form data
    $.ajax({
        type: "POST",
        url: "/Orders/CreateMultiple",
        contentType: "application/json",
        data: JSON.stringify(orders),
        success: function (response) {
            // Handle success response from the server
            localStorage.clear();
            window.location.href = '/Orders/Index';
            console.log(response);
        },
        error: function (xhr, status, error) {
            // Handle errors or other status codes
            console.log("Error:", error);
        }
    });
}





