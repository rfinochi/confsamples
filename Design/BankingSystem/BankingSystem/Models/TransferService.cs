using System;

namespace BankingSystem.Models
{
    public class TransferService
    {
        private IAccountRepository _repository;

        public TransferService( IAccountRepository repository )
        {
            _repository = repository;
        }

        public string TransferMoney( int originAccountId, int destinationAccountId, decimal amountToTransfer )
        {
            Account originAccount = _repository.Get( originAccountId );

            if ( originAccount == null )
                return "La cuenta de origen no existe";

            Account destinationAccount = _repository.Get( destinationAccountId ); ;

            if ( destinationAccount == null )
                return "La cuenta de destino no existe";

            if ( originAccount.Balance < amountToTransfer )
                return "La cuenta de origen no tiene fondos suficientes";

            _repository.Save( originAccount.Id, ( originAccount.Balance - amountToTransfer ) );
            _repository.Save( destinationAccount.Id, ( destinationAccount.Balance + amountToTransfer ) );

            return String.Empty;
        }
    }
}