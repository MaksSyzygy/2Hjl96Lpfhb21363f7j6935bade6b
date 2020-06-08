$(function () {
    $("a.delete").click(function () {
        if (!confirm("Удалить сотрудника?")) {
            return false;
        }
    });
});