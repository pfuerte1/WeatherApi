using System;

namespace WeatherApi
{
    public class WeatherForecast
    {
        public string? City { get; set; }
        public string? State { get; set; }

        public int Temperature;

        public string? ShortForecast { get; set; }
        public string FeelsLike
        {
            get
            {
                string feelsLike = "";
                switch (Temperature)
                {
                    case <= 32:
                        feelsLike = "Freezing";
                        break;
                    case > 32 and <= 55:
                        feelsLike = "Cold";
                        break;
                    case > 55 and <= 65:
                        feelsLike = "Chilly";
                        break;
                    case > 65 and <= 75:
                        feelsLike = "Pleasant";
                        break;
                    case > 75 and <= 85:
                        feelsLike = "Warm";
                        break;
                    case > 85 and <= 95:
                        feelsLike = "Hot";
                        break;
                    case > 95:
                        feelsLike = "Hell";
                        break;
                }
                return feelsLike;
            }


        }


    }
}
