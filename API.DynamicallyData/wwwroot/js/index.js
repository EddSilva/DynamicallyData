$(function () {

    const pages = $('#pagination');
    const tableHead = $('#tableHead');
    const tableBody = $('#tableBody');
    const select = $('#formControlSelect');
    const pageSize = 5;


    select.on('change', (e) => {
        const tableName = select.val();
        getData(tableName, 1);
    });

    pages.on('click', 'li', (e) => {
        const tableName = select.val();
        if (tableName) {
            getData(tableName, e.currentTarget.dataset.page);
        }
    });

    function getData(tableName, pageNumber) {
        $.get('/GetTable', { tableName: tableName, pageSize: pageSize, pageNumber: pageNumber })
            .done(function (data) {
                buildTable(data);
                buildPagination(data);
            });
    }

    function buildTable(data) {
        let head = '';
        let body = '';
        let tr = '';
        let containsHead = false;

        for (var i = 0; i < data.items.length; i++) {

            var obj = data.items[i];
            tr = '<tr>';

            for (const key in obj) {
                if (!containsHead) {
                    head += `'<th scope="col">${data.keys[key]}</th>`;
                }

                tr += `'<td>${obj[key]}</td>`;
            }
            containsHead = true;
            body += `${tr}</tr>`;
        }

        tableHead.html(head);
        tableBody.html(body);
    }

    function buildPagination(data) {
        let lis = '';

        if (data.totalCount > 0) {

            let from = 1;
            let to = 3;

            if (!data.hasNext) {
                from = data.totalPages - 2;
                to = data.totalPages;
            } else if (data.currentPage > 2) {
                from = data.currentPage - 1;
                to = data.currentPage + 1;
            }

            if (from < 1) {
                from = 1;
            }

            if (to > data.totalPages) {
                to = data.totalPages;
            }

            for (let i = from; i <= to; i++) {
                const active = data.currentPage == i ? 'active disabled' : '';
                lis += `<li class="page-item ${active}" data-page="${i}"><a class="page-link" href="#">${i}</a></li>`;
            }

            lis = `<li class="page-item ${data.hasPrevious ? '' : 'disabled'}" data-page="1"><a class="page-link" href="#">First</a></li>` +
                  lis +
                 `<li class="page-item ${data.hasNext ? '' : 'disabled'}" data-page="${data.totalPages}"><a class="page-link" href="#">Last</a></li>`;
        }

        pages.html(lis);
    }
})
