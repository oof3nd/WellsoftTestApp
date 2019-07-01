$(document).ready(function() {
    var url;
    var target;

    //Pagination
    GetPageData(1, 3);


    //AddCompany
    $("#btnAddCompany").click(function() {
        $("#addModal").modal();
    });

    //EditCompany
    $("body").on("click",
        "#btnEditCompany",
        function(data) {

            data.preventDefault();
            target = data.target;

            const id = $(target).data('id');
            const controller = $(target).data('controller');
            const action = $(target).data('action');
            url = `/${controller}/${action}/${id}`;

            $("#editModal").modal('show');

            $.get(url)
                .done((result) => {
                    $("#Id").val(result.id);
                    $("#NameEdit").val(result.name);
                })
                .fail(() => {
                }).always(() => {
                });

            $("#confirmEdit").on('click',
                () => {
                    $("#editModal").modal('hide');
                });
        });

    //AddEmployee
    $("#btnAddEmployee").click(function() {

        $("#addModal").modal("show");
        $.get("/Stocktaking/GetAllCompanies")
            .done((result) => {
                $.each(result,
                    function(key, value) {
                        $("#CompanyId").append(`<option value="${value.id}">${value.name}</option>`);
                    });
            })
            .fail(() => {
            }).always(() => {
            });
    });

    //EditEmployee 
    $("body").on("click",
        "#btnEditEmployee",
        function(data) {
            data.preventDefault();
            target = data.target;

            var id = $(target).data('id');
            var controller = $(target).data('controller');
            var action = $(target).data('action');


            const url = `/${controller}/${action}?id=${id}`;

            $("#editModal").modal('show');

            $("#CompanyIdEdit").find("option").remove();
            $.get("/Stocktaking/GetAllCompanies")
                .done((result) => {
                    $.each(result,
                        function(key, value) {
                            $("#CompanyIdEdit").append(`<option value="${value.id}">${value.name}</option>`);
                        });
                })
                .fail(() => {
                }).always(() => {
                });

            $.get(url)
                .done((result) => {
                    $("#Id").val(result.id);
                    $("#NameEdit").val(result.name);
                    $("#DescriptionEdit").val(result.description);
                    $("#AgeEdit").val(result.age);
                    $("#CompanyIdEdit").val(result.company.id);
                })
                .fail(() => {
                }).always(() => {
                });

        });

    //Delete controller
    $("body").on("click", "#btnDelete", function(data) {
            data.preventDefault();
            target = data.target;

            var id = $(target).data('id');
            var controller = $(target).data('controller');
            var action = $(target).data('action');
            var bodyMessage = $(target).data('body-message');

            $("body").append(
            `<div class="modal fade" id="deleteModal" tabindex="-1" role="dialog" aria-labelledby="myModalLabel">
            <div class="modal-dialog" role="document">
            <div class="modal-content">
            <div class="modal-header">
            </div>
            <div class="modal-body" id="deleteModalBody">

            </div>
            <div class="modal-footer">
            <button type="button" class="btn btn-secondary" data-dismiss="modal" id="bntCancelDelete">Закрыть</button>
            <button type="button" class="btn btn-danger" id="confirmDelete">Удалить</button>
            </div>
            </div>
            </div>
            </div>`);

            url = `/${controller}/${action}?id=${id}`;
            $("#deleteModalBody").text(bodyMessage);
            $("#deleteModal").modal("show");
            $.get(url)
                .done(() => {
                    url = `/${controller}/${action}/${id}`;
                    $("#confirmDelete").on("click",
                        () => {

                            var data = JSON.stringify({
                                "id": id
                            });

                            $.post(url, data)
                                .done(() => {
                                    $("#deleteModal").remove();
                                    $(".modal-backdrop").remove();
                                    GetPageData(1, $("#selectedId").val());
                                });
                        });
                });


            $("body").on("click",
                "#bntCancelDelete",
                function() {
                    $("#deleteModal").remove();
                    $(".modal-backdrop").remove();
                });


        });
});

function GetPageData(pageNum, pageSize) {

    $("#tblData").empty();
    $("#paged").empty();

    const urlGet = window.location.href.split("/");

    var lastItemUrl = urlGet[urlGet.length - 1].toString();
    const testUrl = `/Stocktaking/Get${lastItemUrl}`;

    $.getJSON(testUrl,
        { pageNumber: pageNum, pageSize: pageSize },
        function(response) {

            var table;
            if (lastItemUrl.includes("Companies"))
                table = CreateTable(response, "Company", 1);
            else
                table = CreateTable(response, "Employee", 2);

            $("#tblData").append(table);

            PaggingTemplate(response.totalPages, response.currentPage, pageSize);

        });
}


function CreateTable(response, name, id) {
    var rowData = "";
    var columnData = "";
    for (let i = 0; i < response.data.length; i++) {
        var keysRow = Object.keys(response.data[i]);
        if (name === "Employee")
            columnData = columnData + '<td>' + response.data[i].company.name + '</td>';
        for (let j = id; j < keysRow.length; j++) {
            columnData = columnData + '<td>' + response.data[i][keysRow[j]] + '</td>';
        }
        rowData = rowData +
            '<tr>' +
            columnData +
            '<td>' +
            '<button  type="button" class="btn btn-primary btn-sm" id="btnEdit' +
            name +
            '" data-id="' +
            response.data[i].id +
            '" data-controller="Stocktaking" data-action="Edit' +
            name +
            '" >Редактировать</button>' +
            ' | <button  type="button" class="btn btn-danger btn-sm" id="btnDelete" data-id="' +
            response.data[i].id +
            '" data-controller="Stocktaking" data-action="Delete' +
            name +
            '" data-body-message="Удалить ' +
            response.data[i].name +
            '?"">Удалить</button>' +
            '</td>' +
            '</tr>';
        columnData = '';
    }
    return rowData;
}


function PaggingTemplate(_totalPages, _currentPage, pageSize) {
    var template = "";
    var totalPages = _totalPages;
    var currentPage = _currentPage;
    var pageNumberArray = Array.from(Array(totalPages), (x, index) => index + 1);

    var firstPage = 1;
    var lastPage = totalPages;

    if (totalPages !== currentPage) {
        var forwardOne = currentPage + 1;
    }

    var backwardOne = 1;
    if (currentPage > 1) {
        backwardOne = currentPage - 1;
    }

    if (totalPages <= 0) {
        template = "<p>Нет элементов</p>"; 
        $("#paged").append(template);
        return;
    }

    template = "<p>" + currentPage + " из " + totalPages + " стр.</p>";

    template = template +
        '<ul class="pagination">' +
        '<li class="page-item"><a class="page-link" href="#" onclick="GetPageData(' +
        firstPage +
        "," +
        pageSize +
        ')">Первая</a></li>' +
        '<li><a href="#" onclick="GetPageData(' +
        backwardOne +
        ')"></a>';

    var numberingLoop = "";
    for (let i = 0; i < pageNumberArray.length; i++) {
        numberingLoop = numberingLoop +
            '<li class="page-item" ><a class="page-link" onclick="GetPageData(' +
            pageNumberArray[i] +
            "," +
            pageSize +
            ')" href="#">' +
            pageNumberArray[i] +
            ' &nbsp;&nbsp;</a></li>';
    }
    template = template +
        numberingLoop +
        '<a href="#" onclick="GetPageData(' +
        forwardOne +
        ')" ></a></li>' +
        '<li class="page-item"><a class="page-link" href="#" onclick="GetPageData(' +
        lastPage +
        "," +
        pageSize +
        ')">Последняя</a></li></ul>' +
        'Показывать по: <select class="custom-select" id="selectedId"><option value="3">3</option><option value="10">10</option><option value="15">15</option></select>';
    $("#paged").append(template);

    $(`#selectedId option[value=${pageSize}]`).prop("selected", true);

    $("#selectedId").change(function() {
        GetPageData(1, $(this).val());
    });
}