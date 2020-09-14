$(function () {
    var pages = $('#pagination');
    var tableHead = $('#tableHead');
    var tableBody = $('#tableBody');
    var select = $('#formControlSelect');
    var pageSize = 5;
    select.on('change', function (e) {
        var tableName = select.val();
        getData(tableName, 1);
    });
    pages.on('click', 'li', function (e) {
        var tableName = select.val();
        if (tableName) {
            getData(tableName, e.currentTarget.dataset.page);
        }
    });
    function getData(tableName, pageNumber) {
        console.log(tableName, pageNumber);
        $.get('/GetTable', { tableName: tableName, pageSize: pageSize, pageNumber: pageNumber })
            .done(function (data) {
            console.log(data);
            buildTable(data);
            buildPagination(data);
        });
    }
    function buildTable(data) {
        var head = '';
        var body = '';
        var tr = '';
        var containsHead = false;
        for (var i = 0; i < data.items.length; i++) {
            var obj = data.items[i];
            tr = '<tr>';
            for (var key in obj) {
                if (!containsHead) {
                    head += "'<th scope=\"col\">" + data.keys[key] + "</th>";
                }
                tr += "'<td>" + obj[key] + "</td>";
            }
            containsHead = true;
            body += tr + "</tr>";
        }
        tableHead.html(head);
        tableBody.html(body);
    }
    function buildPagination(data) {
        var lis = '';
        if (data.totalCount > 0) {
            var from = 1;
            var to = 3;
            if (!data.hasNext) {
                from = data.totalPages - 3;
                to = data.totalPages;
            }
            else if (data.currentPage > 2) {
                from = data.currentPage - 1;
                to = data.currentPage + 1;
            }
            for (var i = from; i <= to; i++) {
                var active = data.currentPage == i ? 'active' : '';
                lis += "<li class=\"page-item " + active + "\" data-page=\"" + i + "\"><a class=\"page-link\" href=\"#\">" + i + "</a></li>";
            }
            lis = "<li class=\"page-item " + (data.hasPrevious ? '' : 'disabled') + "\" data-page=\"1\"><a class=\"page-link\" href=\"#\">First</a></li>" +
                lis +
                ("<li class=\"page-item " + (data.hasNext ? '' : 'disabled') + "\" data-page=\"" + data.totalPages + "\"><a class=\"page-link\" href=\"#\">Last</a></li>");
        }
        pages.html(lis);
    }
});
