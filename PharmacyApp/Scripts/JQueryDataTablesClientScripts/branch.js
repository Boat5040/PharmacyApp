(function ($, document) {
    $('#myData').dataTable({
        "bProcessing": true,
        "bServerSide": true,
        "sAjaxSource": document.URL,
        "sServerMethod": "POST",
        "aoColumns": [
            { "mDataProp": "Name" },
            { "mDataProp": "Phone" },
            { "mDataProp": "BranchId", "bSearchable": false, "bSortable": false }
        ],
        "fnRowCallback": function (nRow, aData, iDisplayIndex, iDisplayIndexFull) {
            $('td:eq(2)', nRow).html('<a href="/update-branch/?branchId=' + aData.BranchId + '"><i class="glyphicon glyphicon-edit"> </i></a> | ' +
                '<a href="/delete-branch/?branchId=' + aData.BranchId + '"><i class="glyphicon glyphicon-trash"> </i></a>');
        }
    });
})(jQuery, document)