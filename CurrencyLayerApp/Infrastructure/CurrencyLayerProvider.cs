using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using CurrencyLayerApp.Infrastructure.Global;
using CurrencyLayerApp.Models;
using CurrencyLayerApp.Resources.Strings;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CurrencyLayerApp.Infrastructure
{
    /// <summary>
    /// Provides connection to CurrentLayer APIs with sending and downloading.
    /// See in README.md (5)
    /// </summary>
    class CurrencyLayerProvider
    {
        /// <summary>
        /// Creates HttpClient for connecting to CurrentLayer App
        /// </summary>
        /// <param name="client"></param>
        public CurrencyLayerProvider(HttpClient client)
        {
            _client = client;
            _hostUrl = CommonData.CurrentLayerApi;
        }

        #region <Fields>

        private readonly HttpClient _client;

        /// <summary>
        /// Main URL which starts with "https" till ".com" or etc.
        /// </summary>
        private readonly string _hostUrl;

        #endregion

        #region <Properties which reads messages from server>

        public string ErrorLog
        {
            set { Logger.SetLogMessage(value, Logger.Color.Red); }
        }

        public string SuccessLog
        {
            set { Logger.SetLogMessage(value, Logger.Color.Green); }
        }

        #endregion

        #region <Methods>

        /// <summary>
        /// Gets RT currencies rating which corelated to USD.
        /// </summary>
        /// <param name="currencies">currencies</param>
        /// <returns></returns>
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
                var res = IsSuccessful(responseMessage) ? ApiCurrencyModel.JsonParse(responseMessage) : null;
                if (res != null)
                    SuccessLog = MainLogMessages.ConnectedMessage;
                return res;
            }
            catch
            {
                //Notifies us about subscription plans or troubles on the server.
                ErrorLog = MainLogMessages.NotAvailableInternetMessage;
                return null;
            }
        }

        /// <summary>
        /// Gets historic data for concrete currencies last days which corelated to USD.
        /// </summary>
        /// <param name="currencies">for currencies</param>
        /// <param name="dateTime">current time</param>
        /// <param name="days">last days</param>
        /// <returns></returns>
        public Dictionary<DateTime, ApiCurrencyModel> GetHistoricalCurrencyModel(CurrencyModel[] currencies,
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
                        $"&currencies={string.Join(",", currencies.Select(x => x.Code))}" +
                        $"& date={date.ToString("yyyy-MM-dd")}";
                    var request = new HttpRequestMessage(HttpMethod.Get, GetFormattedString(url));
                    var response = _client.SendAsync(request).Result;
                    var responseMessage = response.Content.ReadAsStringAsync().Result;
                    if (!IsSuccessful(responseMessage))
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
                ErrorLog = MainLogMessages.NotAvailableInternetMessage;
                return null;
            }
        }

        #endregion

        #region <Additional>

        /// <summary>
        /// Checks response from CurrencyLayer for errors.
        /// Default error message:
        /// {
        ///     "success": false,
        ///     "error": {
        ///         "code": 104,
        ///         "info": "Your monthly usage limit has been reached. Please upgrade your subscription plan."    
        ///     }
        /// }
        /// </summary>
        /// <param name="responseMessage">response in json string</param>
        /// <returns>true if successful, false - with errors on server</returns>
        private bool IsSuccessful(string responseMessage)
        {
            var message = JObject.Parse(responseMessage);
            var res = (bool) message["success"];
            if (!res)
            {
                var error = JsonConvert.DeserializeObject<Dictionary<string, string>>(message["error"].ToString());
                ErrorLog = error["info"];
            }
            return res;
        }

        /// <summary>
        /// Concats host and secong url
        /// Examole: 'http://www.google.com' + '/gmail'
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        private string GetFormattedString(string url) => $"{_hostUrl}{url}";

        #endregion
    }
}