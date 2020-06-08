$(function () {
    $("a.delete").click(function () {
        if (!confirm("Удалить клиента?")) {
            return false;
        }
    });
});