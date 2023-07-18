
using System.Text.Json.Serialization;

namespace Farm.Weather.Contracts.Models
{
    public class WeatherGeneralDto
    {
        [JsonPropertyName("location")]
        public LocationDto Location { get; set; } = null!;

        [JsonPropertyName("current")]
        public WeatherDto Weather { get; set; } = null!;
    }

    public class LocationDto
    {
        [JsonPropertyName("name")]
        public string Name { get; set; } = null!;

        [JsonPropertyName("region")]
        public string? Region { get; set; }

        [JsonPropertyName("country")]
        public string Country { get; set; } = null!;

        [JsonPropertyName("lat")]
        public float Lat { get; set; }

        [JsonPropertyName("lon")]
        public float Lon { get; set; }

        [JsonPropertyName("tz_id")]
        public string TzId { get; set; } = null!;

        [JsonPropertyName("localtime_epoch")]
        public int LocaltimeEpoch { get; set; }

        [JsonPropertyName("localtime")]
        public string Localtime { get; set; } = null!;
    }

    public class WeatherDto
    {
        [JsonPropertyName("last_updated_epoch")]
        public int LastUpdatedEpoch { get; set; }

        [JsonPropertyName("last_updated")]
        public string CreatedDate { get; set; } = null!;

        [JsonPropertyName("temp_c")]
        public float TempC { get; set; }

        [JsonPropertyName("temp_f")]
        public float TempF { get; set; }

        [JsonPropertyName("is_day")]
        public int IsDay { get; set; }

        [JsonPropertyName("condition")]
        public ConditionDto Condition { get; set; } = null!;

        [JsonPropertyName("wind_mph")]
        public float WindMph { get; set; }

        [JsonPropertyName("wind_kph")]
        public float WindKph { get; set; }

        [JsonPropertyName("wind_degree")]
        public int WindDegree { get; set; }

        [JsonPropertyName("wind_dir")]
        public string WindDir { get; set; } = null!;

        [JsonPropertyName("pressure_mb")]
        public float PressureMb { get; set; }

        [JsonPropertyName("pressure_in")]
        public float PressureIn { get; set; }

        [JsonPropertyName("precip_mm")]
        public float PrecipMm { get; set; }

        [JsonPropertyName("precip_in")]
        public float PrecipIn { get; set; }

        [JsonPropertyName("humidity")]
        public int Humidity { get; set; }

        [JsonPropertyName("cloud")]
        public int Cloud { get; set; }

        [JsonPropertyName("feelslike_c")]
        public float FeelsLikeC { get; set; }

        [JsonPropertyName("feelslike_f")]
        public float FeelsLikeF { get; set; }

        [JsonPropertyName("vis_km")]
        public float VisKm { get; set; }

        [JsonPropertyName("vis_miles")]
        public float VisMiles { get; set; }

        [JsonPropertyName("uv")]
        public float Uv { get; set; }

        [JsonPropertyName("gust_mph")]
        public float GustMph { get; set; }

        [JsonPropertyName("gust_kph")]
        public float GustKph { get; set; }

        [JsonPropertyName("air_quality")]
        public AirQualityDto AirQuality { get; set; } = null!;
    }

    public class ConditionDto
    {
        [JsonPropertyName("text")]
        public string Text { get; set; } = null!;

        [JsonPropertyName("icon")]
        public string Icon { get; set; } = null!;

        [JsonPropertyName("code")]
        public int Code { get; set; }
    }

    public class AirQualityDto
    {
        /// <summary>
        /// Carbon Monoxide (μg/m3)
        /// </summary>
        [JsonPropertyName("co")]
        public float Co { get; set; }

        /// <summary>
        /// Nitrogen dioxide (μg/m3)
        /// </summary>
        [JsonPropertyName("no2")]
        public float No2 { get; set; }

        /// <summary>
        /// Ozone (μg/m3)
        /// </summary>
        [JsonPropertyName("o3")]
        public float O3 { get; set; }

        /// <summary>
        /// Sulphur dioxide (μg/m3)
        /// </summary>
        [JsonPropertyName("so2")]
        public float So2 { get; set; }

        /// <summary>
        /// PM2.5 (μg/m3)
        /// </summary>
        [JsonPropertyName("pm2_5")]
        public float Pm25 { get; set; }

        /// <summary>
        /// PM10 (μg/m3)
        /// </summary>
        [JsonPropertyName("pm10")]
        public float Pm10 { get; set; }

        /// <summary>
        /// US - EPA standard.
        /// 1 means Good
        /// 2 means Moderate
        /// 3 means Unhealthy for sensitive group
        /// 4 means Unhealthy
        /// 5 means Very Unhealthy
        /// 6 means Hazardous
        /// </summary>
        [JsonPropertyName("us-epa-index")]
        public int UseEpaIndex { get; set; }

        /// <summary>
        /// UK Defra Index
        /// </summary>
        [JsonPropertyName("gb-defra-index")]
        public int GbDefraIndex { get; set; }
    }
}