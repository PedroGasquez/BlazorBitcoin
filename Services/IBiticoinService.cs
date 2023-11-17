using BlazorBitCoinApp.DTOs;

namespace BlazorBitCoinApp.Services
{
    public interface IBiticoinService
    {
        Task<List<BitcoinDataDTO>> FindBy(DateTime startDate);
    }
}
