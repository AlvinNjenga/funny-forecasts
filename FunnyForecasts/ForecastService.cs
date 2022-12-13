using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;

namespace FunnyForecasts
{
    public class ForecastService
    {
        HttpClient _httpClient { get; set; }
        public ForecastService()
        {
            _httpClient = new HttpClient();
        }

        public void GetRandomApiCall()
        {

        }
    }
}
