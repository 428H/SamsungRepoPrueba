var dataTableProductos;
var dataTableClientes;
var productosSeleccionados = [];

$(document).ready(function () {
    cargarDataTableProductos();
    cargarDataTableClientes();
    inicializarEventos();
});

function inicializarEventos() {
    // Limpiar cliente seleccionado
    $('#limpiarCliente').on('click', function () {
        $('#clienteSeleccionado').val('');
        $('#clienteID').val('');
    });

   
    $('form').on('submit', function () {
        
        actualizarCamposFormulario();
        return true;
    });
}

function cargarDataTableProductos() {
    dataTableProductos = $("#tblProductos").DataTable({
        "ajax": {
            "url": "/Venta/Ventas/GetAll",
            "type": "GET",
            "datatype": "json"
        },
        "pageLength": 5,
        "lengthMenu": [[5, 10, 25, 50, -1], [5, 10, 25, 50, "Todos"]],
        "columns": [
            { "data": "id_producto", "width": "5%" },
            { "data": "nombre", "width": "20%" },
            {
                "data": "precio",
                "width": "10%",
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
                "data": "ruta_imagen",
                "width": "15%",
                "render": function (imagen) {
                    return `<img src="/${imagen}" width="80px" height="80px" style="object-fit: cover;" alt="Producto">`;
                }
            },
            {
                "data": "id_producto",
                "width": "15%",
                "render": function (data, type, row) {
                    let disabled = '';
                    let title = '';

                    if (row.stock_disponible <= 0 || !row.tiene_stock) {
                        disabled = 'disabled';
                        title = 'No hay stock disponible';
                    } else if (isProductoSeleccionado(data)) {
                        disabled = 'disabled';
                        title = 'Producto ya seleccionado';
                    }

                    return `
                        <button class="btn btn-sm bg-primary-subtle agregarProducto"
                                data-id="${data}"
                                data-nombre="${row.nombre}"
                                data-precio="${row.precio}"
                                data-stock="${row.stock_disponible}"
                                ${disabled}
                                title="${title}">
                            <i class="fas fa-plus"></i> Agregar
                        </button>`;
                }
            }
        ],
        "language": {
            "decimal": "",
            "emptyTable": "No hay productos disponibles",
            "info": "Mostrando _START_ a _END_ de _TOTAL_ productos",
            "infoEmpty": "Mostrando 0 a 0 de 0 productos",
            "infoFiltered": "(Filtrado de _MAX_ productos totales)",
            "lengthMenu": "Mostrar _MENU_ productos",
            "loadingRecords": "Cargando...",
            "processing": "Procesando...",
            "search": "Buscar:",
            "zeroRecords": "No se encontraron productos",
            "paginate": {
                "first": "Primero",
                "last": "Último",
                "next": "Siguiente",
                "previous": "Anterior"
            }
        },
        "width": "100%"
    });

    
    $('#tblProductos').on('click', '.agregarProducto:not([disabled])', function () {
        var id = $(this).data('id');
        var nombre = $(this).data('nombre');
        var precio = parseFloat($(this).data('precio'));
        var stock = parseInt($(this).data('stock'));

        
        var producto = {
            id: id,
            nombre: nombre,
            precio: precio,
            cantidad: 1,
            stock: stock
        };

        productosSeleccionados.push(producto);
        actualizarVistaProductosSeleccionados();
        actualizarTotales();
        actualizarCamposFormulario();

        
        $(this).prop('disabled', true).attr('title', 'Producto ya seleccionado');

        toastr.success(`Producto "${nombre}" agregado a la venta`);
    });
}

function isProductoSeleccionado(id) {
    return productosSeleccionados.some(p => p.id == id);
}

function actualizarVistaProductosSeleccionados() {
    var $contenedor = $('#productosSeleccionados');
    $contenedor.empty();

    if (productosSeleccionados.length === 0) {
        $contenedor.html(`
            <div class="text-center text-muted py-3" id="mensajeProductosVacios">
                No hay productos seleccionados
            </div>
        `);
        return;
    }

    productosSeleccionados.forEach(function (producto, index) {
        $contenedor.append(`
            <div class="producto-item border-bottom py-2" data-index="${index}">
                <div class="d-flex justify-content-between align-items-center">
                    <div>
                        <span class="fw-bold">${producto.nombre}</span>
                        <span class="text-muted ms-2">$${producto.precio.toFixed(2)}</span>
                    </div>
                    <div class="d-flex align-items-center">
                        <div class="input-group input-group-sm" style="width: 120px;">
                            <button class="btn btn-outline-secondary decrementar-cantidad" type="button">-</button>
                            <input type="number" class="form-control text-center cantidad-producto" value="${producto.cantidad}" min="1" max="${producto.stock}">
                            <button class="btn btn-outline-secondary incrementar-cantidad" type="button">+</button>
                        </div>
                        <button class="btn btn-sm btn-outline-danger ms-2 eliminar-producto">
                            <i class="fas fa-trash"></i>
                        </button>
                    </div>
                </div>
            </div>
        `);
    });

    // Inicializar eventos para los elementos agregados
    inicializarEventosProductos();
}

function actualizarCamposFormulario() {
    // Buscar o crear el contenedor para campos ocultos
    if ($('#productosFormulario').length === 0) {
        $('form').append('<div id="productosFormulario" style="display:none;"></div>');
    }

    // Limpiar contenedor
    $('#productosFormulario').empty();

    // Crear campos ocultos para cada producto
    productosSeleccionados.forEach(function (producto, index) {
        $('#productosFormulario').append(`
            <input type="hidden" name="ProductosSeleccionados[${index}].Id" value="${producto.id}" />
            <input type="hidden" name="ProductosSeleccionados[${index}].Cantidad" value="${producto.cantidad}" />
            <input type="hidden" name="ProductosSeleccionados[${index}].Precio" value="${producto.precio}" />
            <input type="hidden" name="ProductosSeleccionados[${index}].Nombre" value="${producto.nombre}" />
        `);
    });
}

function inicializarEventosProductos() {
    // Incrementar cantidad
    $('.incrementar-cantidad').off('click').on('click', function () {
        var $item = $(this).closest('.producto-item');
        var index = $item.data('index');
        var producto = productosSeleccionados[index];

        
        if (producto.cantidad < producto.stock) {
            producto.cantidad++;
            $item.find('.cantidad-producto').val(producto.cantidad);
            actualizarTotales();
            actualizarCamposFormulario();
        } else {
            toastr.warning(`No hay más stock disponible para "${producto.nombre}"`);
        }
    });

    // Decrementar cantidad
    $('.decrementar-cantidad').off('click').on('click', function () {
        var $item = $(this).closest('.producto-item');
        var index = $item.data('index');
        var producto = productosSeleccionados[index];

        if (producto.cantidad > 1) {
            producto.cantidad--;
            $item.find('.cantidad-producto').val(producto.cantidad);
            actualizarTotales();
            actualizarCamposFormulario();
        }
    });

    // Cambio directo en input
    $('.cantidad-producto').off('change').on('change', function () {
        var $item = $(this).closest('.producto-item');
        var index = $item.data('index');
        var producto = productosSeleccionados[index];

        var nuevaCantidad = parseInt($(this).val());
        if (isNaN(nuevaCantidad) || nuevaCantidad < 1) {
            $(this).val(1);
            nuevaCantidad = 1;
        } else if (nuevaCantidad > producto.stock) {
            $(this).val(producto.stock);
            nuevaCantidad = producto.stock;
            toastr.warning(`El stock disponible para "${producto.nombre}" es ${producto.stock}`);
        }

        producto.cantidad = nuevaCantidad;
        actualizarTotales();
        actualizarCamposFormulario();
    });

    
    $('.eliminar-producto').off('click').on('click', function () {
        var $item = $(this).closest('.producto-item');
        var index = $item.data('index');
        var producto = productosSeleccionados[index];
        var idProducto = producto.id;

        // Eliminar de la lista
        productosSeleccionados.splice(index, 1);

        // Actualizar vista
        actualizarVistaProductosSeleccionados();
        actualizarTotales();
        actualizarCamposFormulario();

       
        $(`.agregarProducto[data-id="${idProducto}"]`)
            .prop('disabled', false)
            .attr('title', '');

        toastr.info(`Producto "${producto.nombre}" eliminado de la venta`);
    });
}

function actualizarTotales() {
    // Calcular subtotal
    var subtotal = productosSeleccionados.reduce(function (sum, producto) {
        return sum + (producto.precio * producto.cantidad);
    }, 0);

   
    var porcentajeIVA = 0.13;
    var impuesto = subtotal * porcentajeIVA;

    // Calcular total 
    var total = subtotal + impuesto;

    
    $('#Venta_Subtotal').val(subtotal.toFixed(2));
    $('#Venta_Total').val(total.toFixed(2));
}

function cargarDataTableClientes() {
    dataTableClientes = $("#tblClientes").DataTable({
        "ajax": {
            "url": "/venta/clientes/GetAll",
            "type": "GET",
            "datatype": "json"
        },
        "pageLength": 5,
        "lengthMenu": [[5, 10, 25, 50, -1], [5, 10, 25, 50, "Todos"]],
        "columns": [
            { "data": "dui", "width": "15%" },
            { "data": "nombre", "width": "20%" },
            { "data": "apellido", "width": "20%" },
            { "data": "telefono", "width": "15%" },
            {
                "data": "email",
                "width": "10%",
                "render": function (data) {
                    return `<div class="text-truncate" style="max-width: 150px;" title="${data}">${data}</div>`;
                }
            },
            {
                "data": "id_cliente",
                "width": "15%",
                "render": function (data, type, row) {
                    return `
                        <button class="btn btn-sm btn-primary seleccionarCliente" 
                                data-id="${data}" 
                                data-nombre="${row.nombre} ${row.apellido}">
                            <i class="fas fa-check"></i> Seleccionar
                        </button>`;
                }
            }
        ],
        "language": {
            "decimal": "",
            "emptyTable": "No hay clientes disponibles",
            "info": "Mostrando _START_ a _END_ de _TOTAL_ clientes",
            "infoEmpty": "Mostrando 0 a 0 de 0 clientes",
            "infoFiltered": "(Filtrado de _MAX_ clientes totales)",
            "lengthMenu": "Mostrar _MENU_ clientes",
            "loadingRecords": "Cargando...",
            "processing": "Procesando...",
            "search": "Buscar:",
            "zeroRecords": "No se encontraron clientes",
            "paginate": {
                "first": "Primero",
                "last": "Último",
                "next": "Siguiente",
                "previous": "Anterior"
            }
        },
        "width": "100%"
    });

    // Seleccionar de clientes
    $('#tblClientes').on('click', '.seleccionarCliente', function () {
        var id = $(this).data('id');
        var nombre = $(this).data('nombre');

        
        $('#clienteSeleccionado').val(nombre);
        $('#clienteID').val(id);
        $('#Venta_Id_cliente').val(id);

        
        $('#productos-tab').tab('show');

        toastr.success(`Cliente "${nombre}" seleccionado`);
    });
}