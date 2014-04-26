using System.ComponentModel.DataAnnotations;

namespace BankingSystem.Models
{
    public class TransferMoneyViewModel
    {
        [Display( Name = "Cuenta de origen" )]
        public int OriginAccountId
        {
            get;
            set;
        }

        [Display( Name = "Cuenta de destino" )]
        public int DestinationAccountId
        {
            get;
            set;
        }

        [Display( Name = "Monto a tranferir" )]
        public decimal AmmountToTransfer
        {
            get;
            set;
        }
    }
}