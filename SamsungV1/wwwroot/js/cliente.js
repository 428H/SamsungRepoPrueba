
//CODIGO CATEGORIA JS


var dataTable;

$(document).ready(function () {
    cargarDatatable();
});


function cargarDatatable() {
    dataTable = $("#tblClientes").DataTable({
        "ajax": {
            "url": "/venta/clientes/GetAll",
            "type": "GET",
            "datatype": "json"
        },

        "columns": [
            { "data": "dui", "width": "10%" },
            { "data": "nombre", "width": "15%" },
            { "data": "apellido", "width": "15%" },
            { "data": "telefono", "width": "15%" },
            { "data": "email", "width": "20%" },
            {
                "data": "id_cliente",
                "render": function (data) {

                    return `<div class="dropdown text-center">
                              <button class="btn bg-primary-subtle dropdown-toggle" type="button" id="dropdownActions${data}" data-bs-toggle="dropdown" aria-expanded="false">
                                <i class="fas fa-cog"></i> Acciones
                              </button>
                              <ul class="dropdown-menu" aria-labelledby="dropdownActions${data}">
                                <li>
                                  <a class="dropdown-item" href="/Venta/clientes/Edit/${data}">
                                    <i class="far fa-edit text-primary"></i> Editar
                                  </a>
                                </li>
                                <li>
                                  <a class="dropdown-item" href="#" onclick="Delete('/venta/clientes/Delete/${data}'); return false;">
                                    <i class="far fa-trash-alt text-danger"></i> Borrar
                                  </a>
                                </li>
                              </ul>
                            </div>`;
                }, "width": "30%"
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



function Delete(url) {

    Swal.fire({
        title: "¿Está seguro de borrar?",
        text: "¡Este contenido no se puede recuperar!",
        icon: "warning",
        showCancelButton: true,
        confirmButtonColor: "#DD6B55",
        confirmButtonText: "Sí, borrar!",
        cancelButtonText: "Cancelar"
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                type: 'DELETE',
                url: url,
                success: function (data) {
                    if (data.success) {
                        toastr.success(data.message);
                        dataTable.ajax.reload();
                    } else {
                        toastr.error(data.message);
                    }
                },
                error: function (error) {
                    toastr.error("Error en la operación: " + error.statusText);
                    console.log(error);
                }
            });
        }
    });
}

