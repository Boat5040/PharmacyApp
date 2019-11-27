(function ($, document) {
    $('#myData').dataTable({
        "bProcessing": true,
        "bServerSide": true,
        "sAjaxSource": document.URL,
        "sServerMethod": "POST",
        "aoColumns": [
            { "mDataProp": "Name" },
            { "mDataProp": "CategoryId" },
            { "mDataProp": "PurchasedPrice" },
            { "mDataProp": "SellingPrice" },
            { "mDataProp": "Quantity" },
            { "mDataProp": "GenericName" },
            { "mDataProp": "CompanyName" },
            { "mDataProp": "Effect" },
            { "mDataProp": "MufDate" },
            { "mDataProp": "ExpiredDate" },
            { "mDataProp": "ProductId", "bSearchable": false, "bSortable": false }
        ],
        "fnRowCallback": function (nRow, aData, iDisplayIndex, iDisplayIndexFull) {
            $('td:eq(10)', nRow).html('<a href="/update-product/?productId=' + aData.ProductId + '">Edit</a> | ' +
                '<a href="/delete-product/?productId=' + aData.ProductId + '">Delete</a>');
        }
    });
})(jQuery, document)