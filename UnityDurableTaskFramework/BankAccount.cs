namespace UnityDurableTaskFramework
{
    using System.Collections.Generic;

    /// <summary>
    /// Bank account implementation.
    /// </summary>
    public class BankAccount : IBankAccount
    {
        #region [ Constructors ]
        
        /// <summary>
        /// Initializes a new instance of the <see cref="BankAccount"/> class.
        /// </summary>
        public BankAccount()
        {
            this.Accounts = new Dictionary<string, decimal> 
            { 
                { "123456", 100000M }, 
                { "654321", 0M } 
            };
        }

        #endregion

        #region [ Private Properties ]
        
        private Dictionary<string, decimal> Accounts
        {
            get;
            set;
        }

        #endregion

        #region [ Public Members ]
        
        /// <summary>
        /// Debits the given <paramref name="accountNumber"/> with the given <paramref name="amount"/>.
        /// </summary>
        /// <param name="amount">The amount to debit.</param>
        /// <param name="accountNumber">The account number to debit.</param>
        /// <returns>True if the account number exists, the account has sufficient funds and the operation was successful, otherwise false.</returns>
        public bool DebitAccount(decimal amount, string accountNumber)
        {
            if (!this.Accounts.ContainsKey(accountNumber))
            {
                return false;
            }

            if (amount > this.Accounts[accountNumber])
            {
                return false;
            }

            this.Accounts[accountNumber] -= amount;

            return true;
        }

        /// <summary>
        /// Credits the given <paramref name="accountNumber"/> with the given <paramref name="amount"/>.
        /// </summary>
        /// <param name="amount">The amount to credit.</param>
        /// <param name="accountNumber">The account number to credit.</param>
        /// <returns>True if the account number exists and the operation was successful, otherwise false.</returns>
        public bool CreditAccount(decimal amount, string accountNumber)
        {
            if (!this.Accounts.ContainsKey(accountNumber))
            {
                return false;
            }

            this.Accounts[accountNumber] += amount;

            return true;
        }

        /// <summary>
        /// Returns the balance of .
        /// </summary>
        /// <param name="amount">The amount to debit.</param>
        /// <param name="accountNumber">The account number to debit.</param>
        /// <returns>True if the account exists and the operation was successful, otherwise false.</returns>
        public decimal? GetBalance(string accountNumber)
        {
            if (!this.Accounts.ContainsKey(accountNumber))
            {
                return null;
            }

            return this.Accounts[accountNumber];
        }

        #endregion
    }
}
