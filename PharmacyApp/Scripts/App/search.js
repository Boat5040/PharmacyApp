(function ($) {
    var branch_url = $('#branch-url').val();
    var signatory_url = $('#signatory-url').val();
    var complaint_url = $('#complaint-url').val();

    var selInstitutionId = $('#selected-institution-id').val();
    var selBranchId = $('#selected-branch-id').val();
    var selSignatoryId = $('#selected-signatory-id').val();

    function getBranches(institutionId) {
        $.ajax({
            type: "GET",
            url: branch_url + '/?institutionId=' + institutionId
        }).done(function (data) {
            $('#BranchId').empty();
            $('#BranchId').append($('<option />').val("").text("Branch"));
            $(data).each(function (index, branch) {
                $('#BranchId').append($('<option />').val(branch.Id).text(branch.Name));
            });
            $('#BranchId').val(selBranchId);
        });
    }

    function getSignatories(branchId) {
        $.ajax({
            type: "GET",
            url: signatory_url + '/?branchId=' + branchId,
        }).done(function (data) {
            $('#SignatoryId').empty();
            $('#SignatoryId').append($('<option />').val("").text("Signatory"));
            $(data).each(function (index, signatory) {
                $('#SignatoryId').append($('<option />').val(signatory.Id).text(signatory.Name));
            });
            $('#SignatoryId').val(selSignatoryId);
        });
    }

    function postComplaint() {
        var myForm = new FormData();
        var instrumentId = $('#Complaint_InstrumentId').val();
        var comments = $('#Complaint_Comments').val();
        var token = $('#complaint-form').find('input[name=__RequestVerificationToken]').val();
        var signatoryId = $('#Complaint_SignatoryId').val();
        myForm.append("InstrumentId", instrumentId);
        myForm.append("Comments", comments);
        myForm.append("SignatoryId", signatoryId);
        myForm.append("__RequestVerificationToken", token);
        $.ajax({
            type: 'POST',
            url: complaint_url,
            data: myForm,
            processData: false,
            contentType: false,
            success: function (response) {
                $('#myModal').modal('toggle');
            },
            error: function (jqXHR, textStatus, errorMessage) {
                console.log(errorMessage); // Optional
            }

        });
    }

    $('#InstitutionId').change(function () {
        var institutionId = $(this).val();
        getBranches(institutionId);
    });

    $('#BranchId').change(function () {
        var branchId = $(this).val();
        getSignatories(branchId);
    });

    $('#send-comment').click(function () {
        var valid = $('#complaint-form').valid();
        if (valid) {
            postComplaint();
        }
    });

    getBranches(selInstitutionId);
    getSignatories(selBranchId);
           
})(jQuery)