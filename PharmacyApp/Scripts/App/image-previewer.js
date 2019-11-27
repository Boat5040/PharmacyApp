(function ($) {  
    var url = $('#url').val();
    function ProfileIndex() {  
        var $this = this;  
  
        function intialize() {  
            $("#ScannedImage").change(function () {  
                //readURL(this);
                uploadFile(this);
            });  
        }  
  
        function readURL(input) {  
            if (input.files && input.files[0]) {  
                var reader = new FileReader();  
  
                reader.onload = function (e) {  
                    var imgSrc = e.target.result;
                    $('#preview').attr('src', imgSrc);  
                }  
                reader.readAsDataURL(input.files[0]);  
            }  
        } 

        function uploadFile(input) {
            var blobFile = input.files[0];
            var token = $('[name=__RequestVerificationToken]').val();
            var formData = new FormData();
            formData.append("imgFile", blobFile);
            formData.append("__RequestVerificationToken", token);

            $('#preview').html('Loading...');

            $.ajax({
                url: url,
                type: "POST",
                data: formData,
                processData: false,
                contentType: false,
                success: function (response) {
                    $('#preview').attr('src', response);
                },
                error: function (jqXHR, textStatus, errorMessage) {
                    console.log(errorMessage); // Optional
                }
            });
        }
  
        $this.init = function () {  
            intialize();  
        }  
    }  
  
    $(function () {  
        var self = new ProfileIndex();  
        self.init();  
    })  
  
})(jQuery)  