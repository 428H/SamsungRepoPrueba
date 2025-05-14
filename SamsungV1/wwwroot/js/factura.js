$(document).ready(function () {
    cargarDatatable();
});

function cargarDatatable() {
    dataTable = $("#tblFacturas").DataTable({
        "ajax": {
            "url": "/venta/facturas/GetAll",
            "type": "GET",
            "datatype": "json"
        },
        "columns": [
            { "data": "numero", "width": "8%" },
            { "data": "cliente", "width": "10%" },
            { "data": "fecha_emision", "width": "15%" },
            { "data": "usuario", "width": "12%" },
            { "data": "productos", "width": "15%" },
            { "data": "cantidades", "width": "10%" },
            {
                "data": "total",
                "width": "10%",
                "render": function (data) {
                    return '$' + parseFloat(data).toFixed(2);
                }
            },
            {
                "data": "id_factura",
                "render": function (data, type, row) {
                    return `<div class="text-center">
                              <div class="btn-group">
                                <button class="btn btn-sm btn-primary generar-pdf" data-id="${data}">
                                  <i class="fa-solid fa-file-pdf"></i> PDF
                                </button>
                                
                              </div>
                            </div>`;
                },
                "width": "15%"
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

    // Evento boton
    $('#tblFacturas').on('click', '.generar-pdf', function () {
        var id = $(this).data('id');
        
        window.open(`/venta/facturas/GenerarPDF?id=${id}`, '_blank');
    });


}
