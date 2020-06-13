$(function () {

    $('#login-form-link').click(function (e) {
        // $("#login-form").delay(100).fadeIn(100);
        // $("#register-form").fadeOut(100);
        $("#login-form").show();
        $("#register-form").hide();
        $('#register-form-link').removeClass('active');
        $(this).addClass('active');
        e.preventDefault();
    });
    $('#register-form-link').click(function (e) {
        // $("#register-form").delay(100).fadeIn(100);
        // $("#login-form").fadeOut(100);
        $("#register-form").show();
        $("#login-form").hide();
        $('#login-form-link').removeClass('active');
        $(this).addClass('active');
        e.preventDefault();
    });

    $('#clock').countdown("2018/07/01", function(event) {
        var totalHours = event.offset.totalDays * 24 + event.offset.hours;
        $(this).html(event.strftime(totalHours + ' : %M : %S '));
    });
});