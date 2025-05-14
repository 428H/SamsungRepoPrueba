
//CODIGO CATEGORIA JS


var dataTable;

$(document).ready(function () {
    cargarDatatable();
});


function cargarDatatable() {
    dataTable = $("#tblTipoMovimientoIs").DataTable({
        "ajax": {
            "url": "/almacen/tipoMovimientoIs/GetAll",
            "type": "GET",
            "datatype": "json"
        },

        "columns": [
            { "data": "id_tipomovimiento", "width": "5%" },
            { "data": "nombre", "width": "15%" },
            {
                "data": "afecta_stock", "width": "15%",
                "render": function (data) {
                    if (data === "+") return '<span class="badge bg-success">Incrementa (+)</span>';
                    if (data === "-") return '<span class="badge bg-danger">Reduce (-)</span>';
                    return '<span>-</span>';
                }
            },
            { "data": "descripcion", "width": "30%" },
            { "data": "estado", "width": "15%" },
            {
                "data": "id_tipomovimiento",
                "render": function (data) {
                    return `<div class="dropdown text-center">
                              <button class="btn bg-primary-subtle dropdown-toggle" type="button" id="dropdownActions${data}" data-bs-toggle="dropdown" aria-expanded="false">
                                <i class="fas fa-cog"></i> Acciones
                              </button>
                              <ul class="dropdown-menu" aria-labelledby="dropdownActions${data}">
                                <li>
                                  <a class="dropdown-item" href="/Almacen/TipoMovimientoIs/Edit/${data}">
                                    <i class="far fa-edit text-primary"></i> Editar
                                  </a>
                                </li>
                                <li>
                                  <a class="dropdown-item" href="#" onclick="Delete('/Almacen/TipoMovimientoIs/Delete/${data}'); return false;">
                                    <i class="far fa-trash-alt text-danger"></i> Borrar
                                  </a>
                                </li>
                              </ul>
                            </div>`;
                }, "width": "40%"
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

