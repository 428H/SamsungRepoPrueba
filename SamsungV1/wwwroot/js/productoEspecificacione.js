
document.addEventListener('DOMContentLoaded', function () {
    // Referencias a los campos
    const inputMemoria = document.getElementById('especMemoria');
    const inputCamara = document.getElementById('especCamara');
    const inputPantalla = document.getElementById('especPantalla');
    const inputColor = document.getElementById('especColor');
    const inputEspecificacionesJson = document.getElementById('especificacionesJson');

    if (inputEspecificacionesJson.value) {
        try {
            const specs = JSON.parse(inputEspecificacionesJson.value);

            if (specs.Memoria) inputMemoria.value = specs.Memoria;
            if (specs.Camara) inputCamara.value = specs.Camara;
            if (specs.Pantalla) inputPantalla.value = specs.Pantalla;
            if (specs.Color) inputColor.value = specs.Color;
        } catch (e) {
            console.error('Error al parsear las especificaciones:', e);
        }
    }

   
    function actualizarEspecificaciones() {
        const specs = {
            Memoria: inputMemoria.value.trim(),
            Camara: inputCamara.value.trim(),
            Pantalla: inputPantalla.value.trim(),
            Color: inputColor.value.trim()
        };

        inputEspecificacionesJson.value = JSON.stringify(specs);
    }

    
    [inputMemoria, inputCamara, inputPantalla, inputColor].forEach(input => {
        input.addEventListener('input', actualizarEspecificaciones);
    });

    
    document.querySelector('form').addEventListener('submit', function (e) {
        actualizarEspecificaciones();
    });
});