var dataTable;

$(document).ready(function () {
    cargarDatatable();
});

function cargarDatatable() {
    dataTable = $("#tblUsuarios").DataTable({
        "ajax": {
            "url": "/admin/usuarios/GetAll",
            "type": "GET",
            "datatype": "json"
        },
        "columns": [
            { "data": "userName", "width": "15%" },
            { "data": "email", "width": "20%" },
            {
                "data": "emailConfirmed",
                "width": "10%",
                "render": function (data) {
                    return data ? '<span class="">Sí</span>' : '<span class="">No</span>';
                }
            },
            { "data": "phoneNumber", "width": "15%" },
            {
                "data": "twoFactorEnabled",
                "width": "10%",
                "render": function (data) {
                    return data ? '<span class="">Activado</span>' : '<span class="">Desactivado</span>';
                }
            },
            {
                "data": "lockoutEnd",
                "width": "10%",
                "render": function (data) {
                    if (data) {
                        var date = new Date(data);
                        if (date > new Date()) {
                            return '<span class="badge bg-danger">Sí</span>';
                        }
                    }
                    return '<span class="badge bg-success">No</span>';
                }
            },
            {
                "data": "id",
                "render": function (data) {
                    return `<div class="text-center">
                                <a href="/Admin/Usuarios/Details/${data}" class="btn bg-secondary-subtle text-black btn-sm" style="cursor:pointer; width:100px;">
                                    <i class="fas fa-info-circle"></i> Detalles
                                </a>
                            </div>
                            `;
                }, "width": "10%"
            }
        ],
        "language": {
            "decimal": "",
            "emptyTable": "No hay registros",
            "info": "Mostrando _START_ a _END_ de _TOTAL_ Entradas",
            "infoEmpty": "Mostrando 0 to 0 of 0 Entradas",
            "infoFiltered": "(Filtrado de _MAX_ total entradas)",
            "infoPostFix": "",
            "thousands": ",",
            "lengthMenu": "Mostrar _MENU_ Entradas",
            "loadingRecords": "Cargando...",
            "processing": "Procesando...",
            "search": "Buscar:",
            "zeroRecords": "Sin resultados encontrados",
            "paginate": {
                "first": "Primero",
                "last": "Ultimo",
                "next": "Siguiente",
                "previous": "Anterior"
            }
        },
        "width": "100%"
    });
}