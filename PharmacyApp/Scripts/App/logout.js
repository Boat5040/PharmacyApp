(function ($) {
    $('.cmd-logout').click(function (e) {
        e.preventDefault();
        $('#logout-form').submit();
    });
})(jQuery)