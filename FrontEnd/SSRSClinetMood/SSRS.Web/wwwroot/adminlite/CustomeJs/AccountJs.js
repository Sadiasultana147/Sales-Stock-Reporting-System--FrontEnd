$(document).ready(function () {

    // Login Function
    $('#loginForm').on('submit', function (e) {
        e.preventDefault();

        const username = $('#Username').val();
        const password = $('#Password').val();

        $.ajax({
            url: '/Admin/Account/Login',
            method: 'POST',
            contentType: 'application/json',
            data: JSON.stringify({ Username: username, Password: password }),
            success: function (result) {
                console.log('Login Response:', result);
                if (result.success) {
                    Swal.fire({
                        icon: 'success',
                        title: 'Login Successful',
                        text: 'You have logged in successfully!',
                        timer: 3000,
                        showConfirmButton: false
                    });

                    setTimeout(function () {
                        window.location.href = '/';
                    }, 3000);
                } else {
                    Swal.fire({
                        icon: 'error',
                        title: 'Login Failed',
                        text: result.message
                    });
                }
            },
            error: function () {
                Swal.fire({
                    icon: 'error',
                    title: 'Error',
                    text: 'An error occurred while logging in.'
                });
            }
        });
    });

    // Logout Function
    $('#logoutLink').on('click', function (e) {
        e.preventDefault();

        $.ajax({
            type: 'POST',
            url: '/Admin/Account/Logout',
            success: function () {
                Swal.fire({
                    icon: 'success',
                    title: 'Logged Out',
                    text: 'You have logged out successfully!',
                    timer: 2000,
                    showConfirmButton: false
                });

                setTimeout(function () {
                    window.location.href = '/Admin/Account/Login';
                }, 2000);
            },
            error: function () {
                Swal.fire({
                    icon: 'error',
                    title: 'Logout Error',
                    text: 'An error occurred while logging out.'
                });
            }
        });
    });

});
