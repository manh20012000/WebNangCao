$("#imageFile").change(function () {
    if (this.files && this.files[0]) {
        var reader = new FileReader();
        reader.onload = function (e) {
            $("#previewImage").attr("src", e.target.result); // Use "$" to access jQuery functions
        };
        reader.readAsDataURL(this.files[0]);
    }
});

$(document).ready(function () { // Use "$" to access jQuery functions
    // Khởi tạo datetime picker
    $('.picker').datetimepicker({ // Use "$" to access jQuery functions
        autoclose: true,
        timepicker: false,
        datepicker: true,
        format: "d/m/y",
        weeks: true,
    });

    // Cập nhật đồng hồ khi người dùng thay đổi ngày
    $.datetimepicker.setLocale('vi'); // Use "$." to access jQuery functions

    // Đặt sự kiện onchange cho trường ngày
    $("input[name='NGAYSANXUAT']").on('change', function () {
        var ngaySanXuat = $(this).val(); // Use "$" to access jQuery functions
        // Ở đây, bạn có thể sử dụng giá trị ngaySanXuat khi người dùng thay đổi ngày
    });
});