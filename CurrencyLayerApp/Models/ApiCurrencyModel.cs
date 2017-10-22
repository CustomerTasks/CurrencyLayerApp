using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CurrencyLayerApp.Abstractions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CurrencyLayerApp.Models
{
    public class ApiCurrencyModel : ICurrency
    {
        /// <summary>
        /// Code
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// "quotes": {
        ///         "USDAED": 3.67266,
        ///         "USDALL": 96.848753,
        ///         "USDAMD": 475.798297,
        ///         "USDANG": 1.790403,
        ///         "USDARS": 2.918969,
        ///         "USDAUD": 1.293878,
        ///         [...]
        ///     }
        /// </summary>
        public Dictionary<string, double> Currencies { get; set; } = new Dictionary<string, double>();
        
        /// <summary>
        /// Parses json response to model. 
        /// Example of json response:
        /// {
        ///     "success": true,
        ///     "historical": true,
        ///     "date": "2005-02-01",
        ///     "timestamp": 1107302399,
        ///     "source": "USD",
        ///     "quotes": {
        ///         "USDAED": 3.67266,
        ///         "USDALL": 96.848753,
        ///         "USDAMD": 475.798297,
        ///         "USDANG": 1.790403,
        ///         "USDARS": 2.918969,
        ///         "USDAUD": 1.293878,
        ///         [...]
        ///     }
        /// } 
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public static ApiCurrencyModel JsonParse(string json)
        {
            JObject jsonObject = JObject.Parse(json);
            ApiCurrencyModel res = new ApiCurrencyModel {Code = (string) jsonObject["source"]};
            res.Currencies = JsonConvert.DeserializeObject<Dictionary<string, double>>(
                jsonObject["quotes"]
                .ToString()
                .Replace($"{res.Code}{res.Code}", "#")
                .Replace(res.Code, "")  //Early it was a bug with replacing in 'USDUSD'
                .Replace("#", res.Code));
            return res;
        }
    }
}
