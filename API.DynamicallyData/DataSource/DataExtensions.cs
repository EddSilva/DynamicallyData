using API.DynamicallyData.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;

namespace API.DynamicallyData.DataSource
{
    public static class DataExtensions
    {
        public static IEnumerable<IEnumerable<Element>> ToElements(this IEnumerable<dynamic> dynamicList)
        {
            var jarray = JArray.Parse(JsonConvert.SerializeObject(dynamicList));
            var list = jarray.Select(obj => ((JObject)obj).Properties().Select(p => new Element(p.Name, p.Value.ToString())));

            return list;
        }
    }
}
