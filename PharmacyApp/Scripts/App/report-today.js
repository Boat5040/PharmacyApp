(function ($, framework) {
    var today = new Date();
    var month = (today.getMonth() + 1).toString();
    var day = today.getDate().toString();
    var year = today.getFullYear().toString();

    month = month.length < 2 ? '0' + month : month;
    day = day.length < 2 ? '0' + day : day;

    var date = month + '/' + day + '/' + year

    $(':text').val(date + ' - ' + date);
    framework.oTable.fnFilter($(':text').val());
})(jQuery, framework)