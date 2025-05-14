var dataTable;
$(document).ready(function () {
    cargarDatatable();
});

function cargarDatatable() {
    dataTable = $("#tblMovimientos").DataTable({
        "ajax": {
            "url": "/almacen/movimientos/GetAll",
            "type": "GET",
            "datatype": "json"
        },
        "columns": [
            { "data": "id_movimiento", "width": "5%" },
            {
                "data": "producto",
                "width": "15%",
                "render": function (data) {
                    return data ? data.nombre : "No disponible";
                }
            },
            {
                "data": "cantidad",
                "width": "10%",
                "render": function (data) {
                    return data ? parseFloat(data).toFixed(2) : "0.00";
                }
            },
            {
                "data": "tipoMovimiento",
                "width": "10%",
                "render": function (data) {
                    return data ? data.nombre : "No disponible";

                } 
            },
            {
                "data": "fecha_movimiento",
                "width": "15%",
                "render": function (data) {
                    if (!data) return "No disponible";
                    let fecha;
                    try {
                        fecha = new Date(data);
                        return fecha.toLocaleString();
                    } catch (e) {
                        return data; 
                    }
                }
            },
            {
                "data": "motivo",
                "width": "25%",
                "render": function (data) {
                    return data || "Sin motivo especificado";
                }
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