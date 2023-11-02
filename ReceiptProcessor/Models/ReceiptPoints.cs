namespace ReceiptProcessor.Models
{
    public class ReceiptPoints
    {
        /// <summary>
        /// The receipt Id.
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// The number of points the receipt was awarded.
        /// </summary>
        public int Points { get; set; }
    }
}
