/* ajax call error handler */
function AjaxErrorHandler(jqXHR, exception) {
    var title = '[' + jqXHR.status + ']';
    var msg = '';

    if (jqXHR.status === 0) {
        msg = 'Not connect.\n Verify Network.';
    } else if (jqXHR.status === 400) {
        msg = 'Bad Request.';
    } else if (jqXHR.status === 401) {
        ErrorInSweetAlert2(title, 'Unauthorized');
        Reload();
    } else if (jqXHR.status === 403) {
        msg = 'Forbidden.';
    } else if (jqXHR.status === 404) {
        msg = 'Requested page not found.';
    } else if (jqXHR.status === 409) {
        msg = 'Conflict exception.';
    } else if (jqXHR.status === 500) {
        msg = 'Internal Server Error.';
    } else if (exception === 'parsererror') {
        msg = 'Requested JSON parse failed.';
    } else if (exception === 'timeout') {
        msg = 'Time out error.';
    } else if (exception === 'abort') {
        msg = 'Ajax request aborted.';
    } else {
        msg = 'Uncaught Error.';
    }

    if (jqXHR.responseText !== null) {
        try {
            msg = jQuery.parseJSON(jqXHR.responseText);
        }
        catch (err) {
            console.log(err.message);
        }
    }

    ErrorInSweetAlert2(title, msg); //in Common.js
}

/* Process data in Modal */
showInModal = (modalId, url) => {
    try {
        $.ajax({
            type: 'GET',
            url: url,
            success: function (res) {
                $("" + modalId + " .modal-content").html(res);
                $.validator.unobtrusive.parse(modalId);
                $(modalId).modal({ backdrop: 'static', keyboard: false })
                $(modalId).modal('show');
            },
            error: function (jqXHR, exception) {
                AjaxErrorHandler(jqXHR, exception);
            }
        });
    }
    catch (err) {
        ErrorInSweetAlert2(err.message); //in Common.js
    }
}

