var dataTable;

$(document).ready(function () {
    cargarDatatable();
});

function cargarDatatable() {
    dataTable = $("#tblMetodosPago").DataTable({
        "ajax": {
            "url": "/venta/metodospago/GetAll",
            "type": "GET",
            "datatype": "json"
        },
        "columns": [
            { "data": "id_metodoPago", "width": "10%" },
            { "data": "nombre", "width": "20%" },
            { "data": "descripcion", "width": "40%" },
            {
                "data": "estado",
                "width": "10%",
                "render": function (data) {
                    if (data === "Activo") {
                        return '<span class="badge bg-success">Activo</span>';
                    } else {
                        return '<span class="badge bg-danger">Inactivo</span>';
                    }
                }
            },
            {
                "data": "id_metodoPago",
                "render": function (data) {
                    return `<div class="text-center">
                                <a href="/Venta/MetodosPago/Edit/${data}" class="btn bg-primary-subtle btn-sm me-1" title="Editar">
                                    <i class="fas fa-edit"></i>
                                </a>
                                <button onclick="eliminar(${data})" class="btn btn-danger btn-sm" title="Eliminar">
                                    <i class="fas fa-trash"></i>
                                </button>
                            </div>`;
                }, "width": "20%"
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

function eliminar(id) {
    Swal.fire({
        title: '¿Estás seguro?',
        text: "¿Deseas eliminar este método de pago?",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#d33',
        cancelButtonColor: '#3085d6',
        confirmButtonText: 'Sí, eliminar',
        cancelButtonText: 'Cancelar'
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                type: "DELETE",
                url: "/Venta/MetodosPago/Delete/" + id,
                success: function (data) {
                    if (data.success) {
                        Swal.fire(
                            '¡Eliminado!',
                            data.message,
                            'success'
                        );
                        dataTable.ajax.reload();
                    } else {
                        Swal.fire(
                            'Error',
                            data.message,
                            'error'
                        );
                    }
                }
            });
        }
    });
}