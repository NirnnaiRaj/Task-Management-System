var user = (function () {

function redirectToRegister() {
    // You can replace '/Account/Register' with the actual URL of your register page
    window.location.href = '/User/Register';
}
    return {
        redirectToRegister
    }

})();