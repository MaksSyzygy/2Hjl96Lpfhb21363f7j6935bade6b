﻿$(function () {
    $("a.delete").click(function () {
        if (!confirm("Удалить категорию?")) {
            return false;
        }
    });
});