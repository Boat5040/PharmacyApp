(function ($, document) {
    $('#myData').dataTable({
        "bProcessing": true,
        "bServerSide": true,
        "sAjaxSource": document.URL,
        "sServerMethod": "POST",
        "aoColumns": [
            { "mDataProp": "Name" },
            { "mDataProp": "Percentage" },
            { "mDataProp": "Status" },
            { "mDataProp": "TaxId", "bSearchable": false, "bSortable": false }
        ],
        "fnRowCallback": function (nRow, aData, iDisplayIndex, iDisplayIndexFull) {
            $('td:eq(3)', nRow).html('<a href="/update-tax/?taxId=' + aData.TaxId + '"><i class="glyphicon glyphicon-edit"> </i></a> | ' +
                '<a href="/delete-tax/?taxId=' + aData.TaxId + '"><i class="glyphicon glyphicon-trash"> </i></a>');
        }
    });
})(jQuery, document)