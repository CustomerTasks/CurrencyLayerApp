using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using CurrencyLayerApp.Infrastructure.Global;
using CurrencyLayerApp.Models;
using Newtonsoft.Json.Linq;

namespace CurrencyLayerApp.Infrastructure
{
    class CurrencyLayerProvider
    {
        private HttpClient _client;
        private readonly string _hostUrl;

        public CurrencyLayerProvider(HttpClient client)
        {
            _client = client;
            _hostUrl = CommonData.CurrentLayerApi;
        }

        public ApiCurrencyModel GetLiveCurrencyModel(params CurrencyModel[]currencies)
        {
            //todo:need access key
            var url =
                $"{CommonData.CurrentLayerApiLiveData}?access_key={Settings.Instance.ApiKey}" +
                $"&currencies={string.Join(",", currencies.Select(x => x.Code))}";
            var request = new HttpRequestMessage(HttpMethod.Get, GetFormattedString(url));
            var response = _client.SendAsync(request).Result;
            return ApiCurrencyModel.JsonParse(response.Content.ReadAsStringAsync().Result);
        }

        private string GetFormattedString(string url) => $"{_hostUrl}{url}";

        public Dictionary<string,ApiCurrencyModel> GetHistoricalCurrencyModel(CurrencyModel[] sources , DateTime dateTime,int days)
        {
            var result = new Dictionary<string, ApiCurrencyModel>(days);
            for (int i = 0; i < days; i++)
            {
                string date = dateTime.AddDays(-i).ToString("yyyy-MM-dd");
                var url =
                    $"{CommonData.CurrentLayerApiHistoricalData}?access_key={Settings.Instance.ApiKey}" +
                    $"&currencies={string.Join(",", sources.Select(x => x.Code))}" +
                    $"& date={date}";
                var request = new HttpRequestMessage(HttpMethod.Get, GetFormattedString(url));
                var response = _client.SendAsync(request).Result;
                result.Add(date, ApiCurrencyModel.JsonParse(response.Content.ReadAsStringAsync().Result));
            }
            return result;
        }
    }
}

