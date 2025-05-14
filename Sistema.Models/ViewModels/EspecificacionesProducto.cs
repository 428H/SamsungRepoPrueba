using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
//////no se esta usando de momento
namespace Sistema.Models.ViewModels
{
    public class EspecificacionesProducto
    {
        public string Memoria { get; set; } = string.Empty;
        public string Camara { get; set; } = string.Empty;
        public string Pantalla { get; set; } = string.Empty;
        public string Color { get; set; } = string.Empty;

        
        public string ToJson() => JsonSerializer.Serialize(this);

        public static EspecificacionesProducto FromJson(string json)
        {
            if (string.IsNullOrWhiteSpace(json))
                return new EspecificacionesProducto();

            try
            {
                return JsonSerializer.Deserialize<EspecificacionesProducto>(json);
            }
            catch
            {
                return new EspecificacionesProducto();
            }
        }
    }
}
