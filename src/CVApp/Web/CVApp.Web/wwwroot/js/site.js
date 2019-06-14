
// Write your JavaScript code.

function AddDateTimePicker() {
    $('#datetimepicker7').datetimepicker({
        viewMode: 'years',
        format: 'MM/YYYY'
    });
    $('#datetimepicker8').datetimepicker({
        viewMode: 'years',
        format: 'MM/YYYY',
        useCurrent: false
    });
    $("#datetimepicker7").on("change.datetimepicker", function (e) {
        $('#datetimepicker8').datetimepicker('minDate', e.date);
    });
    $("#datetimepicker8").on("change.datetimepicker", function (e) {
        $('#datetimepicker7').datetimepicker('maxDate', e.date);
    });
}

function InitTinymce() {
    tinymce.init({
        selector: '.form-control.myTextarea',
        plugins: ["lists advlist"],
        toolbar: "undo redo | styleselect | bold italic | alignleft aligncenter alignright alignjustify | bullist numlist outdent indent "
    });
}


