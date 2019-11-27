(function ($, document) {
    $('#myData').dataTable({
        "bProcessing": true,
        "bServerSide": true,
        "sAjaxSource": document.URL,
        "sServerMethod": "POST",
        "aoColumns": [
            { "mDataProp": "FirstName" },
            { "mDataProp": "LastName" },
            { "mDataProp": "UserName" },
            { "mDataProp": "Email" },
            { "mDataProp": "Institution" },
            { "mDataProp": "Status", "bSearchable": false, "bSortable": false },
            { "mDataProp": "UserId", "bSearchable": false, "bSortable": false }
        ],
        "fnRowCallback": function (nRow, aData, iDisplayIndex, iDisplayIndexFull) {
            var labelType = aData.Status === 'Active' ? 'success' : 'danger';

            $('td:eq(5)', nRow).html('<span class="label label-' + labelType + '">' + aData.Status + '</span>');

            $('td:eq(6)', nRow).html('<a href="/update-admin/?userId=' + aData.UserId + '"><i class="glyphicon glyphicon-edit"> </i></a> | ' +
                '<a href="/reset-admin/?userId=' + aData.UserId + '"><i class="glyphicon glyphicon-wrench"> </i></a> | ' +
                '<a href="/delete-admin/?userId=' + aData.UserId + '"><i class="glyphicon glyphicon-trash"> </i></a>');
        }
    });
})(jQuery, document)