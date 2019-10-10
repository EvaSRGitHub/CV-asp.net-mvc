// Write your JavaScript code.

(function AddDateTimePicker() {

    $('#datetimepickerDate').datetimepicker({
        format: 'L'
    });

    $('#datetimepickerMonthYears1').datetimepicker({
        viewMode: 'years',
        format: 'MM/YYYY'
    });
    $('#datetimepickerMonthYears2').datetimepicker({
        viewMode: 'years',
        format: 'MM/YYYY',
        useCurrent: false
    });
})();

(function InitTinymce() {
    tinymce.init({
        selector: '.form-control.tinymiceTextarea',
        plugins: ["lists advlist"],
        toolbar: "undo redo | styleselect | bold italic | alignleft aligncenter alignright alignjustify | bullist numlist outdent indent ",
    });
})();

function SetLanguageLevelDropdown() {
    let levels = ['Beginner/A1', 'Elementary/A2', 'Intermediate/B1', 'Upper Intermediate/B2', 'Advanced/C1', 'Proficient/C2'];

    for (i = 0; i < levels.length; i++) {
        $('#levelId').append($("<option>")
            .val(levels[i])
            .html(levels[i])
        );
    }
}

