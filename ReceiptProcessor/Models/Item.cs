namespace ReceiptProcessor.Models
{
    public class Item
    {
        /// <summary>
        /// The Short Product Description for the item.
        /// </summary>
        public string ShortDescription { get; set; }
        /// <summary>
        /// The total price payed for this item.
        /// </summary>
        public string Price { get; set; }
    }
}
