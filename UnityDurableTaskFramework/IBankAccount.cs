namespace UnityDurableTaskFramework
{
    /// <summary>
    /// Interface defining the behavior of a bank account.
    /// </summary>
    public interface IBankAccount
    {
        /// <summary>
        /// Debits the given <paramref name="accountNumber"/> with the given <paramref name="amount"/>.
        /// </summary>
        /// <param name="amount">The amount to debit.</param>
        /// <param name="accountNumber">The account number to debit.</param>
        /// <returns>True if the account number exists, the account has sufficient funds and the operation was successful, otherwise false.</returns>
        bool DebitAccount(decimal amount, string accountNumber);

        /// <summary>
        /// Credits the given <paramref name="accountNumber"/> with the given <paramref name="amount"/>.
        /// </summary>
        /// <param name="amount">The amount to credit.</param>
        /// <param name="accountNumber">The account number to credit.</param>
        /// <returns>True if the account number exists and the operation was successful, otherwise false.</returns>
        bool CreditAccount(decimal amount, string accountNumber);

        /// <summary>
        /// Returns the balance of .
        /// </summary>
        /// <param name="amount">The amount to debit.</param>
        /// <param name="accountNumber">The account number to debit.</param>
        /// <returns>True if the account exists and the operation was successful, otherwise false.</returns>
        decimal? GetBalance(string accountNumber);
    }
}
