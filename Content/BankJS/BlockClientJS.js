$('a.blockLink').click(function (event) {
    event.preventDefault();
    $('#myOverlay').fadeIn(297, function () {
        $('#blockClient')
            .css('display', 'block')
            .animate({ opacity: 1 }, 198);
    });
});

$('#blockClient_Close, #myOverlay').click(function () {
    $('#blockClient').animate({ opacity: 0 }, 198, function () {
        $(this).css('display', 'none');
        $('#myOverlay').fadeOut(297);
    });
});