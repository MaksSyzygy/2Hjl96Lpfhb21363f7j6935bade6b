$(function () {
    $("a.delete").click(function () {
        if (!confirm("Удалить страницу?")) {
            return false;
        }
    });
});