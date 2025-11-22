(function ($, kendo) {
    $.extend(true, kendo.ui.validator, {
        rules: {
            required: function (input) {
                if (input.is("[data-val-required]") || input.is("[required]")) {
                    return $.trim(input.val()) !== "";
                }
                return true;
            },
            mvcdate: function (input) {
                if (input.is("[data-val-date]") && input.val() !== "") {
                    var datePicker = input.data("kendoDatePicker");
                    var date = kendo.parseDate(input.val(), datePicker.options.format);
                    return date !== null;
                }
                return true;
            }
        },
        messages: {
            required: function (input) {
                if (input.is("[data-val-required]")) {
                    return input.data("val-required");
                }
                var field = input.data("field");
                if (field)
                    return kendo.format(app.messages.validators.required, field);
                return app.messages.validators.required;
            }
        }
    });
})(jQuery, kendo);

$(function () {

    'use strict';

    $('.js-menu-toggle').click(function (e) {
        var $this = $(this);
        if ($('body').hasClass('show-sidebar2')) {
            $('body').removeClass('show-sidebar2');
            $this.removeClass('active');
        } else {
            $('body').addClass('show-sidebar2');
            $this.addClass('active');
        }
        e.preventDefault();
    });

    $(document).mouseup(function (e) {
        var container = $(".sidebar-2");
        if (!container.is(e.target) && container.has(e.target).length === 0) {
            if ($('body').hasClass('show-sidebar2')) {
                $('body').removeClass('show-sidebar2');
                $('body').find('.js-menu-toggle').removeClass('active');
                setTimeout(function () {

                    const openDropdowns = document.querySelectorAll('.sidebar-2 .collapse.show');
                    openDropdowns.forEach(function (dropdown) {
                        dropdown.classList.remove('show');
                        dropdown.previousElementSibling.setAttribute('aria-expanded', 'false');
                    });


                    this.scrollTop = 0;
                }, 300);

                $('.sidebar-2').removeClass("expanded");


            }
        }
    });

    document.querySelector('.sidebar-2').addEventListener('click', function (e) {
        // ถ้า element ที่คลิกอยู่ใน .sidebar-home หรือเป็นมันเอง => ไม่ทำอะไร
        if (e.target.closest('.sidebar-home')) return;

        $('body').addClass('show-sidebar2');
        $('.sidebar-2').addClass("expanded");
    });

    if (typeof ($.scrollUp) !== "undefined") {
        $.scrollUp({
            scrollName: 'scroll-up',
            animationSpeed: '600',
            scrollText: '<i class="fas fa-4x fa-chevron-circle-up"></i>',
            scrollTitle: "Scroll to top."
        });
    }

    

});