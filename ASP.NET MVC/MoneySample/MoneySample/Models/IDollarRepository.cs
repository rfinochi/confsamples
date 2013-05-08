namespace MoneySample.Models
{
    public interface IDollarRepository
    {
        Dollar GetSavings( string userId );
    }
}