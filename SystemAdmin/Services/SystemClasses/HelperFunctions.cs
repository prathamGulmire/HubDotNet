using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SystemAdmin.Models;

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

        public static ResponseModel<T> Success<T>(T data, string message = "Success")
        {
            return new ResponseModel<T>
            {
                IsSuccess = true,
                Message = message,
                Data = data 
            };
        }

        public static ResponseModel<T> Failure<T>(T data = default, string message = "Failure")
        {
            return new ResponseModel<T>
            {
                IsSuccess = false,
                Message = message,
                Data = data
            };
        }
    }
}
