using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystemAdmin.Services.SystemClasses
{
    public static class HelperFunctions
    {
        public static List<T> DataTableToList<T>(this DataTable dt) where T : class, new()
        {
            List<T> list = new List<T>();
            string temp = JsonConvert.SerializeObject(dt);
            var JsonConvertSettings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                MissingMemberHandling = MissingMemberHandling.Ignore
            };

            list = JsonConvert.DeserializeObject<List<T>>(temp, JsonConvertSettings);

            return list;
        }
    }
}
