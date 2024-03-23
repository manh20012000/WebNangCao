document.addEventListener("DOMContentLoaded", function () {
    const dropdownButtons = document.querySelectorAll(".dropdown button");

    dropdownButtons.forEach(function (button) {
        button.addEventListener("click", function () {
            const dropdownMenu = this.nextElementSibling;
            if (dropdownMenu.style.display === "block") {
                dropdownMenu.style.display = "none";
            } else {
                dropdownMenu.style.display = "block";
            }
        });
    });
});
document.addEventListener("DOMContentLoaded", function () {
    // Khởi tạo datetime picker
    var picker = document.querySelectorAll('.picker');
    picker.forEach(function (el) {
        flatpickr(el, {
            enableTime: false,
            dateFormat: "d/m/y",
            weekNumbers: true
        });
    });

    // Cập nhật đồng hồ khi người dùng thay đổi ngày
    flatpickr.localize(flatpickr.l10ns.vi);

    // Đặt sự kiện onchange cho trường ngày
    var ngaySanXuatInput = document.querySelector("input[name='NGAYSANXUAT']");
    ngaySanXuatInput.addEventListener('change', function () {
        var ngaySanXuat = ngaySanXuatInput.value;
        // Ở đây, bạn có thể sử dụng giá trị ngaySanXuat khi người dùng thay đổi ngày
    });
});

var imageFileInput = document.getElementById("imageFile");
var previewImage = document.getElementById("previewImage");

imageFileInput.addEventListener("change", function () {
    if (this.files && this.files[0]) {
        var reader = new FileReader();
        reader.onload = function (e) {
            previewImage.setAttribute("src", e.target.result);
        };
        reader.readAsDataURL(this.files[0]);
    }
});