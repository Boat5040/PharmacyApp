(function ($, document) {
    $('#myData').dataTable({
        "bProcessing": true,
        "bServerSide": true,
        "sAjaxSource": document.URL,
        "sServerMethod": "POST",
        "aoColumns": [
            { "mDataProp": "FirstName" },
            { "mDataProp": "LastName" },
            { "mDataProp": "UserName", },
            { "mDataProp": "Status", "bSearchable": false, "bSortable": false },
            { "mDataProp": "UserId", "bSearchable": false, "bSortable": false }
        ],
        "fnRowCallback": function (nRow, aData, iDisplayIndex, iDisplayIndexFull) {
            var labelType = aData.Status === 'Active' ? 'success' : 'danger';

            $('td:eq(3)', nRow).html('<span class="label label-' + labelType + '">' + aData.Status + '</span>');

            $('td:eq(4)', nRow).html('<a href="/update-super-user/?userId=' + aData.UserId + '"><i class="glyphicon glyphicon-edit"> </i></a> | ' +
                '<a href="/super-user-profile/?userId=' + aData.UserId + '"><i class="glyphicon glyphicon-eye-open"> </i></a> | ' +
                '<a href="/reset-super-user/?userId=' + aData.UserId + '"><i class="glyphicon glyphicon-wrench"> </i></a> | ' +
                '<a href="/delete-super-user/?userId=' + aData.UserId + '"><i class="glyphicon glyphicon-trash"></i></a>');
        }
    });
})(jQuery, document)

