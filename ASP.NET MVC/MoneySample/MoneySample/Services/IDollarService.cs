using MoneySample.Models;

namespace MoneySample.Services
{
    public interface IDollarService
    {
        Dollar GetSavings( string userId );
    }
}