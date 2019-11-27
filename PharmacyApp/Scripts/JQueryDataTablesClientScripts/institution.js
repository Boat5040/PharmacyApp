(function ($, document) {
    $('#myData').dataTable({
        "bProcessing": true,
        "bServerSide": true,
        "sAjaxSource": document.URL,
        "sServerMethod": "POST",
        "aoColumns": [
            { "mDataProp": "Name" },
            { "mDataProp": "Title" },
            { "mDataProp": "Phone" },
            { "mDataProp": "Email" },
            { "mDataProp": "InstitutionId", "bSearchable": false, "bSortable": false }
        ],
        "fnRowCallback": function (nRow, aData, iDisplayIndex, iDisplayIndexFull) {
            $('td:eq(4)', nRow).html('<a href="/update-institution/?institutionId=' + aData.InstitutionId + '"><i class="glyphicon glyphicon-edit"></i></a> | ' +
                '<a href="/delete-institution/?institutionId=' + aData.InstitutionId + '"><i class="glyphicon glyphicon-trash"></i></a>');
        }
    });
})(jQuery, document)