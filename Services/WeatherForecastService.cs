using System.Net.Http.Headers;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace WeatherApi.Services
{
    public class WeatherForecastService
    {
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;

        public WeatherForecastService(IConfiguration configuration, HttpClient httpClient)
        {
            _configuration = configuration;
            _httpClient = httpClient;
        }

        public async Task<WeatherForecast> GetForecastByCoordinatesAsync(double latitude, double longitude)
        {
            longitude = -97.0892; latitude = 39.7456;

            if (latitude == 0 || longitude == 0)
                throw new ApplicationException("Invalid coordinates");

            var uri = _configuration["WeatherGovApi:ForecastByCoordinatesUri"];
            if (string.IsNullOrWhiteSpace(uri))
                throw new ApplicationException("ApiUri not configured");

            var apiUri = string.Format(uri, latitude, longitude);
            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/geo+json"));
            _httpClient.DefaultRequestHeaders.UserAgent.TryParseAdd("(peterfuerte@hotmail.com)");

            var httpResponseMessage = await _httpClient.GetAsync(apiUri);

            if (httpResponseMessage.IsSuccessStatusCode)
            {
                var json = httpResponseMessage.Content.ReadAsStringAsync().Result;
                dynamic? results = JsonConvert.DeserializeObject<dynamic>(json);
                var forecastApiUri = results?.properties?.forecast;
                var city = results?.properties?.relativeLocation?.properties?.city;
                var state = results?.properties?.relativeLocation?.properties?.state;

                if (forecastApiUri != null)
                {

                    httpResponseMessage = await _httpClient.GetAsync(forecastApiUri?.ToString());
                    if (httpResponseMessage.IsSuccessStatusCode)
                    {
                        json = httpResponseMessage.Content.ReadAsStringAsync().Result;

                        JObject o = JObject.Parse(json);
                        JArray a = (JArray)o["properties"]["periods"];
                        IList<dynamic> forecasts = a.ToObject<IList<dynamic>>();
                        var forecastNow = from f in forecasts
                                          where f.number == 1
                                          select f;

                        var r = new WeatherForecast
                        {
                            City = city,
                            State = state,
                            ShortForecast = forecastNow.First().shortForecast,
                            Temperature = forecastNow.First().temperature
                        };
                        return r;
                    }
                }
            }

            throw new ApplicationException("An error occurred.");
        }
    }
}
