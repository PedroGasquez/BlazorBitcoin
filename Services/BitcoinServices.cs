using BlazorBitCoinApp.DTOs;
using Newtonsoft.Json.Linq;

namespace BlazorBitCoinApp.Services
{
    public class BitcoinServices : IBiticoinService
    {
        private readonly HttpClient _httpClient;
        

        public BitcoinServices(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<BitcoinDataDTO>> FindBy (DateTime startDate)
        {
            var response = await _httpClient.GetAsync("https://data.messari.io/api/v1/markets/binance-btc-usdt/metrics/price/time-series?start=" + startDate.ToString("yyyy-MM-dd") + "&interval=1d");
            response.EnsureSuccessStatusCode();
            var jsonResult = await response.Content.ReadAsStringAsync();
            JObject JObject = JObject.Parse(jsonResult);
            var values = JObject.SelectToken("data.values").ToString();
            if(string.IsNullOrWhiteSpace(values))
                return new List<BitcoinDataDTO>();

            var data = Newtonsoft.Json.JsonConvert.DeserializeObject<List<decimal[]>>(values);

            return data.Select(x => new BitcoinDataDTO(new DateTime(1970, 1, 1).AddMilliseconds((long)x[0]), x[3])).ToList();
        }
    }
}
