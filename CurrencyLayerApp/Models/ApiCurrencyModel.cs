using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CurrencyLayerApp.Models
{
    public class ApiCurrencyModel : ICurrency
    {
        public string Code { get; set; }
        public Dictionary<string, double> Quotes { get; set; } = new Dictionary<string, double>();

        public static ApiCurrencyModel JsonParse(string json)
        {
            JObject jsonObject = JObject.Parse(json);
            ApiCurrencyModel res = new ApiCurrencyModel {Code = (string) jsonObject["source"]};
            res.Quotes = JsonConvert.DeserializeObject<Dictionary<string, double>>(jsonObject["quotes"].ToString()
                .Replace($"{res.Code}{res.Code}", "#").Replace(res.Code, "").Replace($"#", res.Code));
            return res;
        }
    }
}
