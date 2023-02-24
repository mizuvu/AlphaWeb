$(document).ready(function () {
    //$('.number100').autoNumeric('init', { vMin: 0, mDec: 0 });
    // $('.number200').autoNumeric('init', { vMin: 0, mDec: 2 });

    /*
    var path = window.location.href; // because the 'href' property of the DOM element is the absolute path
    $('.nav-main-item ul a').each(function () {
        if (this.href === path) {
            $(this).parents('li').addClass('open');
            $(this).addClass('active');
        }
    });
    */

    // auto active menu in NAV
    $(".side-menu li ul li .active").parents("li").addClass('is-expanded');
    $(".side-menu li ul li .active").parents("li").parents("ul").parents("li").children("a").first().addClass('active');

    //var a = $(".side-menu li ul li .active").parents("li").parents("ul").parents("li").children("a").first().attr("class");
    //alert(a);

    // enable tooltip
    enableTooltip();
});

function enableTooltip() {
    // enable tooltip
    let tooltipTriggerList = [].slice.call(document.querySelectorAll('[data-bs-toggle="tooltip"]'))
    var tooltipList = tooltipTriggerList.map(function (tooltipTriggerEl) {
        return new bootstrap.Tooltip(tooltipTriggerEl, {
            container: 'body',
            trigger: 'hover'
        });
    })
}