using ParkingApp.Data;

namespace ParkingApp.Models
{
    public class Account
    {
        int NextId = 1;
        public int Id { get; private set; }
        //public User User { get; private set; }
        public double Debt { get; private set; } = 0;
        public ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();


        public Account() 
        {
            Id = NextId++;
        }

        public void MakePayment(Transaction transaction)
        {
            if (transaction == null)
                throw new ArgumentNullException(nameof(transaction));

            if (transaction.Amount > Debt)
                throw new InvalidOperationException("Payment exceeds the current debt.");

            Transactions.Add(transaction);
            Debt -= transaction.Amount;
        }

        public void AddParkingDebt(Transaction transaction)
        {
            if (transaction == null)
                throw new ArgumentNullException(nameof(transaction));

            Transactions.Add(transaction);
            Debt += transaction.Amount;
        }


        public void ClearDebt()
        {
            Debt = 0;
        }

        public double CalculateDebt()
        {
            return Transactions.Sum(t => t.Amount);
        }
    }
}
