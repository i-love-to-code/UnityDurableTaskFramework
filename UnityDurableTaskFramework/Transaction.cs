namespace UnityDurableTaskFramework
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Describes a bank transaction.
    /// </summary>
    [DataContract]
    public class Transaction
    {
        /// <summary>
        /// Gets or sets the transaction amount.
        /// </summary>
        [DataMember]
        public decimal Amount { get; set; }

        /// <summary>
        /// Gets or sets the source bank account number.
        /// </summary>
        [DataMember]
        public string SourceBankAccount { get; set; }

        /// <summary>
        /// Gets or sets the target bank account number.
        /// </summary>
        [DataMember]
        public string TargetBankAccount { get; set; }
    }
}
