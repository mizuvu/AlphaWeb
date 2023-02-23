/* Reload current page with no post data */
async function reload() {
    setTimeout(function () {
        location.reload();
    }, 100);
}

/* Redirect to url */
async function redirect(url, message) {
    if (message == undefined || message == null || message == '') {
        setTimeout(function () {
            window.location = url;
        }, 100);
        return;
    }
    if (confirm(message)) {
        setTimeout(function () {
            window.location = url;
        }, 100);
        return;
    }
}

/* SweetAlert2 alert handler */
async function errorInSweetAlert2(title, message) {
    Swal.fire({
        icon: 'error',
        title: '' + title,
        html: '' + message,
        buttonsStyling: false,
        customClass: {
            confirmButton: 'btn btn-danger',
        }
    });
}

async function warningInSweetAlert2(title, message) {
    Swal.fire({
        icon: 'warning',
        title: '' + title,
        html: '' + message,
        buttonsStyling: false,
        customClass: {
            confirmButton: 'btn btn-warning',
        }
    });
}