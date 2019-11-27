(function ($, document) {
    $('#myData').dataTable({
        "bProcessing": true,
        "bServerSide": true,
        "sAjaxSource": document.URL,
        "sServerMethod": "POST",
        "aoColumns": [
            { "mDataProp": "Name" },
            { "mDataProp": "Description" },
            { "mDataProp": "CategoryId", "bSearchable": false, "bSortable": false }
        ],
        "fnRowCallback": function (nRow, aData, iDisplayIndex, iDisplayIndexFull) {
            $('td:eq(2)', nRow).html('<a href="/update-category/?categoryId=' + aData.CategoryId + '"><i class="glyphicon glyphicon-edit"> </i></a> | ' +
                '<a href="/delete-category/?categoryId=' + aData.CategoryId + '"><i class="glyphicon glyphicon-trash"> </i></a>');
        }
    });
})(jQuery, document)