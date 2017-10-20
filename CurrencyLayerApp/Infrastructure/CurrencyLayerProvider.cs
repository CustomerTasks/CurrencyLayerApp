using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using CurrencyLayerApp.Infrastructure.Global;
using CurrencyLayerApp.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CurrencyLayerApp.Infrastructure
{
    class CurrencyLayerProvider
    {
        public CurrencyLayerProvider(HttpClient client)
        {
            _client = client;
            _hostUrl = CommonData.CurrentLayerApi;
        }

        #region <Fields>

        private readonly HttpClient _client;
        private readonly string _hostUrl;

        #endregion

        #region <Properties>

        public string LastLog { get; set; }

        #endregion

        #region <Methods>

        public ApiCurrencyModel GetLiveCurrencyModel(params CurrencyModel[] currencies)
        {
            try
            {
                var url =
                    $"{CommonData.CurrentLayerApiLiveData}?access_key={Settings.Instance.ApiKey}" +
                    $"&currencies={string.Join(",", currencies.Select(x => x.Code))}";
                var request = new HttpRequestMessage(HttpMethod.Get, GetFormattedString(url));
                var response = _client.SendAsync(request).Result;
                var responseMessage = response.Content.ReadAsStringAsync().Result;
                return CheckStatus(responseMessage) ? ApiCurrencyModel.JsonParse(responseMessage) : null;
            }
            catch
            {
                LastLog = "Internet isn`t available. Please, check connection";
                return null;
            }
        }

        public Dictionary<DateTime, ApiCurrencyModel> GetHistoricalCurrencyModel(CurrencyModel[] sources,
            DateTime dateTime, int days)
        {
            try
            {
                var result = new Dictionary<DateTime, ApiCurrencyModel>(days);
                for (int i = 0; i < days; i++)
                {
                    DateTime date = dateTime.AddDays(-i);
                    var url =
                        $"{CommonData.CurrentLayerApiHistoricalData}?access_key={Settings.Instance.ApiKey}" +
                        $"&currencies={string.Join(",", sources.Select(x => x.Code))}" +
                        $"& date={date.ToString("yyyy-MM-dd")}";
                    var request = new HttpRequestMessage(HttpMethod.Get, GetFormattedString(url));
                    var response = _client.SendAsync(request).Result;
                    var responseMessage = response.Content.ReadAsStringAsync().Result;
                    if (!CheckStatus(responseMessage))
                    {
                        result = null;
                        break;
                    }
                    result.Add(date, ApiCurrencyModel.JsonParse(responseMessage));
                }
                return result;
            }
            catch
            {
                LastLog = "Internet isn`t available. Please, check connection";
                return null;
            }
        }

        #endregion

        #region <Additional>

        private bool CheckStatus(string responseMessage)
        {
            var message = JObject.Parse(responseMessage);
            var res = (bool) message["success"];
            if (!res)
            {
                var error = JsonConvert.DeserializeObject<Dictionary<string, string>>(message["error"].ToString());
                LastLog = error["info"];
            }
            return res;
        }

        private string GetFormattedString(string url) => $"{_hostUrl}{url}";

        #endregion
    }
}

