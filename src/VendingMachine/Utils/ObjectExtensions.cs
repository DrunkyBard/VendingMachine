using System.IO;
using Newtonsoft.Json;

namespace VendingMachineApp.Utils
{
    public static class ObjectExtensions
    {
        public static string ToJson(this object obj)
        {
            var js = JsonSerializer.Create(new JsonSerializerSettings());
            var jw = new StringWriter();
            js.Serialize(jw, obj);

            return jw.ToString();
        }

    }
}