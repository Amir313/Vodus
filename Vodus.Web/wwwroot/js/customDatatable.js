$(document).ready(function () {
    $("#customDatatable").DataTable({
        "processing": true,
        "serverSide": true,
        "filter": true,
        "ajax": {
            "url": "/api/Common/GetData",
            "type": "POST",
            "datatype": "json"
        },
        "columnDefs": [{
            "targets": [0],
            "visible": true,
            "searchable": true
        }],
        "columns": [
            { "data": "page", "name": "page", "autoWidth": false },
            { "data": "promoTitle", "name": "promoTitle", "autoWidth": false },
            { "data": "promoDescription", "name": "promoDescription", "autoWidth": false },
            { "data": "startDate", "name": "startDate", "autoWidth": false },
            { "data": "endDate", "name": "endDate", "autoWidth": false },
            { "data": "imageUrl", "name": "imageUrl", "autoWidth": false },
        ]
    });

    var table = $('#customDatatable').DataTable();

    $('a.toggle-vis').on('click', function (e) {
        e.preventDefault();

        // Get the column API object
        var column = table.column($(this).attr('data-column'));

        // Toggle the visibility
        column.visible(!column.visible());
    });
});