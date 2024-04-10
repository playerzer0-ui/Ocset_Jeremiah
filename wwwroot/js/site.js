// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
function addAllToCart() {
    var selectedItems = [];

    // Loop through each product row
    $("input[type='checkbox']:checked").each(function () {
        var productId = $(this).attr("id").replace("chk_", "");
        var productName = $(this).closest('tr').find('td:eq(0)').text(); // Get product name from the first column of the current row
        var quantity = parseInt($("#quantity_" + productId).val());

        if (!isNaN(quantity) && quantity > 0 && Number.isInteger(quantity)) {
            selectedItems.push({
                productId: productId,
                productName: productName,
                quantity: quantity
            });
        }
    });

    if (selectedItems.length > 0) {
        var cart = {};
        selectedItems.forEach(function (item) {
            cart[item.productId] = {
                productId: item.productId,
                productName: item.productName,
                quantity: item.quantity
            };
        });
        localStorage.setItem('cart', JSON.stringify(cart));
        alert('Selected items added to cart!');
    } else {
        alert('Please select at least one item with a valid integer quantity!');
    }
}
