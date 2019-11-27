(function ($) {
    if ($('#toastr-info').val() !== null && $('#toastr-info').val() !== '') {
        toastr.info($('#toastr-info').val());
    }

    if ($('#toastr-success').val() !== null && $('#toastr-success').val() !== '') {
        toastr.success($('#toastr-success').val());
    }

    if ($('#toastr-warn').val() !== null && $('#toastr-warn').val() !== '') {
        toastr.warning($('#toastr-warn').val());
    }

    if ($('#toastr-error').val() !== null && $('#toastr-error').val() !== '') {
        toastr.error($('#toastr-error').val());
    }
})(jQuery)
