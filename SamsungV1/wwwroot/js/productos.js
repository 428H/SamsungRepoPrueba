
var dataTable;

$(document).ready(function () {
    cargarDatatable();
});

function cargarDatatable() {
    dataTable = $("#tblProductos").DataTable({
        "ajax": {
            "url": "/almacen/productos/GetAll",
            "type": "GET",
            "datatype": "json"
        },
        "columns": [
            { "data": "id_producto", "width": "3%" },
            { "data": "nombre", "width": "12%" },
            {
                "data": "precio",
                "width": "5%",
                "render": function (data) {
                    return '$' + parseFloat(data).toFixed(2);
                }
            },
            {
                "data": "stock_disponible",
                "width": "8%",
                "render": function (data, type, row) {
                    if (!row.tiene_stock) {
                        return "<span class='badge bg-secondary'>Sin inventario</span>";
                    }

                    let stockClass = 'bg-success';
                    let stockText = 'Disponible';

                    if (row.stock_disponible <= 0) {
                        stockClass = 'bg-danger';
                        stockText = 'Agotado';
                    } else if (row.stock_disponible <= row.stock_minimo) {
                        stockClass = 'bg-warning text-dark';
                        stockText = 'Stock bajo';
                    }

                    return `
                        <div class="text-center">
                            <span class="badge ${stockClass}">${stockText}</span>
                            <div class="mt-1 small fw-bold">${row.stock_disponible} unidades</div>
                        </div>`;
                }
            },
            {
                "data": "especificaciones",
                "width": "12%",
                "render": function (data) {
                    if (!data) {
                        return "<span class='text-muted'>Sin especificaciones</span>";
                    }

                    try {
                        const specs = JSON.parse(data);
                        let details = '';

                        if (specs.Memoria) details += `<span class="text-black badge bg-secondary-subtle me-1">Memoria: ${specs.Memoria}</span>`;
                        if (specs.Pantalla) details += `<span class="text-black badge bg-secondary-subtle me-1">Pantalla: ${specs.Pantalla}</span>`;
                        if (specs.Camara) details += `<span class="text-black badge bg-secondary-subtle me-1">Cámara: ${specs.Camara}</span>`;
                        if (specs.Color) details += `<span class="text-black badge bg-secondary-subtle me-1">Color: ${specs.Color}</span>`;

                        if (details === '') {
                            return "<span class='text-muted'>Especificaciones básicas</span>";
                        }

                        return details;
                    } catch {
                        return "<span class='text-danger'>Error en formato</span>";
                    }
                }
            },
            {
                "data": "ruta_imagen",
                "width": "10%",
                "render": function (imagen, type, row) {
                    const uniqueId = 'img_' + row.id_producto;

                    return `
                        <img class="product-img" id="${uniqueId}" src="../${imagen}" width="120px" height="120px" style="object-fit: cover;" alt="Producto">
                        <div id="modal_${uniqueId}" class="modal product-modal">
                            <span class="close">&times;</span>
                            <img class="modal-content" id="content_${uniqueId}">
                            <div id="caption_${uniqueId}" class="caption"></div>
                        </div>`;
                }
            },
            {
                "data": "id_producto",
                "render": function (data) {
                    return `
                        <div class="dropdown text-center">
                            <button class="btn bg-primary-subtle dropdown-toggle" type="button" id="dropdownActions${data}" data-bs-toggle="dropdown" aria-expanded="false">
                                <i class="fas fa-cog"></i> Acciones
                            </button>
                            <ul class="dropdown-menu" aria-labelledby="dropdownActions${data}">
                                <li>
                                    <a class="dropdown-item" href="/Almacen/Productos/Edit/${data}">
                                        <i class="far fa-edit text-primary"></i> Editar
                                    </a>
                                </li>
                                <li>
                                    <a class="dropdown-item" href="#" onclick="Delete('/Almacen/Productos/Delete/${data}'); return false;">
                                        <i class="far fa-trash-alt text-danger"></i> Borrar
                                    </a>
                                </li>
                                <li>
                                    <a class="dropdown-item" href="/Almacen/Productos/AgregarProducto/${data}">
                                        <i class="fa-solid fa-plus text-info"></i> Stock
                                    </a>
                                </li>
                                <li>
                                    <a class="dropdown-item" href="/Almacen/Productos/Details/${data}">
                                        <i class="fa-solid fa-list-ul text-secondary"></i> Detalles
                                    </a>
                                </li>
                            </ul>
                        </div>`;
                },
                "width": "10%"
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

    // Mostrar imagen en modal
    $('#tblProductos').on('click', '.product-img', function () {
        const id = this.id;
        const modalId = 'modal_' + id;
        const contentId = 'content_' + id;
        const captionId = 'caption_' + id;

        document.getElementById(modalId).style.display = "block";
        document.getElementById(contentId).src = this.src;
        document.getElementById(captionId).innerHTML = this.alt;
    });

    // "X"
    $(document).on('click', '.close', function () {
        $(this).closest('.product-modal').css('display', 'none');
    });

    // 
    $(document).on('click', '.product-modal', function (e) {
        if (e.target === this) {
            $(this).css('display', 'none');
        }
    });
}

// Función para eliminar producto con confirmación
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
