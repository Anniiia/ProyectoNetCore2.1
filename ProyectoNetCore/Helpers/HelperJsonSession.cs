using Newtonsoft.Json;
namespace ProyectoNetCore.Helpers
{
    public class HelperJsonSession
    {
        //internamente existe un metodo en session para trabajar con string, no con bytes
        //HttpContext.Session.GetString,HttpContext.Session.SetString, almacenaremos objetos: {Nombre='Mascotas', Raza: 'Perro', Edad 15}
        //necesitamos un metodo para almacenar el objeto

        public static string SerializeObject<T>(T data)
        {
            //convertimos el objeto a string utilizando newton

            string json = JsonConvert.SerializeObject(data);

            return json;
        }

        //recibiremos un string y lo convertiremos a cualquier objeto (T)

        public static T DeserializeObject<T>(string data)
        {
            //mediante newton deserializamos el string
            T objeto = JsonConvert.DeserializeObject<T>(data);

            return objeto;
        }
    }
}
