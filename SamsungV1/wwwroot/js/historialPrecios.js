var dataTable;

$(document).ready(function () {
    cargarDatatable();
});

function cargarDatatable() {
    dataTable = $("#tblHistorialPrecios").DataTable({
        "ajax": {
            "url": "/almacen/historialprecios/GetAll",
            "type": "GET",
            "datatype": "json"
        },
        "columns": [
            {
                "data": "producto", 
                "width": "15%",
                "render": function (data, type, row) {
                    return (data && data.nombre) ? data.nombre : "N/A";
                }
            },
            {
                "data": "precio_Anterior",
                "width": "15%",
                "render": function (data) {
                    return data !== null && data !== undefined ? '$' + parseFloat(data).toFixed(2) : 'N/A';
                }
            },
            {
                "data": "precio_Nuevo",
                "width": "15%",
                "render": function (data) {
                    return data !== null && data !== undefined ? '$' + parseFloat(data).toFixed(2) : 'N/A';
                }
            },
            {
                "data": "fecha_Cambio",
                "width": "15%",
                "render": function (data) {
                    return data || 'N/A';
                }
            },
            {
                "data": "motivo",
                "width": "15%",
                "render": function (data) {
                    return data || 'N/A';
                }
            },
            {
                "data": "usuario", 
                "width": "15%",
                "render": function (data, type, row) {
                    return (data && data.userName) ? data.userName : "Sistema";
                }
            }
        ],
        "language": {
            "decimal": "",
            "emptyTable": "No hay registros de cambios de precio",
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
                "last": "Último",
                "next": "Siguiente",
                "previous": "Anterior"
            }
        },
        "order": [[3, "desc"]],
        "width": "100%"
    });
}