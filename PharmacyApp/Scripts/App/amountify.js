(function ($) {
    //$('.amount').val('0.00'); // initialize all amount fields to #.##
    $.each($('.amount'), function (index, element) {
        var amount = parseFloat(element.value).toFixed(2);
        if (amount == "NaN") {
            $('.amount')[index].value = "0.00";
        }
        else {
            $('.amount')[index].value = amount;
        }
    });
    $('.amount').blur(function () {
        var amount = parseFloat($(this).val()).toFixed(2);
        if (amount == "NaN") {
            $(this).val('0.00');
        }
        else {
            $(this).val(amount);
        }
        
    });
})(jQuery)