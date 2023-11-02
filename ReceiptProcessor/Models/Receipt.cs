namespace ReceiptProcessor.Models
{
    public class Receipt
    {
        /// <summary>
        /// The Id of the receipt.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// The name of the retailer or store the receipt is from.
        /// </summary>
        public string Retailer { get; set; }
        /// <summary>
        /// The date of the purchase printed on the receipt.
        /// </summary>
        public string PurchaseDate { get; set; }
        /// <summary>
        /// The time of the purchase printed on the receipt. 24-hour time expected.
        /// </summary>
        public string PurchaseTime { get; set; }
        /// <summary>
        /// The total amount paid on the receipt.
        /// </summary>
        public string Total { get; set; }
        public List<Item> Items { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Receipt"/> class.
        /// </summary>
        public Receipt()
        {
            Id = Guid.NewGuid();
        }

        public int CalculateReceiptPoints()
        {
            int totalPoints = 0;

            totalPoints += ProcessPointsForRetailer();
            totalPoints += ProcessPointsForTotal();
            totalPoints += ProcessPointsForItems();
            totalPoints += ProcessPointsForDate();
            totalPoints += ProcessPointsForTime();

            return totalPoints;
        }

        /// <summary>
        /// Processes the amount of points for the name of the retailer.
        /// </summary>
        /// <returns>The points awarded from the retailer name.</returns>
        private int ProcessPointsForRetailer()
        {
            int points = 0;

            foreach (var character in Retailer)
            {

                if (char.IsLetterOrDigit(character))
                {
                    points++;
                }
            }

            return points;
        }

        /// <summary>
        /// Processes the amount of points for the receipt total.
        /// </summary>
        /// <returns>The points awarded from the receipt total.</returns>
        /// <exception cref="Exception"></exception>
        private int ProcessPointsForTotal()
        {
            int points = 0;


            var result = double.Parse(Total);

            if (result <= 0) return 0;

            if (result == Math.Ceiling(result))
            {
                points += 50;
            }

            if (result % .25 == 0)
            {
                points += 25;
            }

            return points;

        }

        /// <summary>
        /// Processes the amount of points for the items.
        /// </summary>
        /// <returns>The points awarded from the items.</returns>
        /// <exception cref="Exception"></exception>
        private int ProcessPointsForItems()
        {
            int points = 0;


            // Calculate points for every 2 items.
            var result = Items.Count / 2;

            points += result * 5;

            // Calculate points for item names.
            foreach (var item in Items)
            {
                if (item.ShortDescription.Trim().Length % 3 == 0)
                {
                    points += (int)Math.Ceiling(double.Parse(item.Price) * .2);
                }
            }

            return points;

        }

        /// <summary>
        /// Processes the amount of points for the receipt date.
        /// </summary>
        /// <returns>The points awarded from the date.</returns>
        /// <exception cref="Exception"></exception>
        private int ProcessPointsForDate()
        {
            int points = 0;


            var result = DateOnly.Parse(PurchaseDate);

            if (result.Day <= 0) return 0;

            if (result.Day % 2 != 0)
            {
                points += 6;
            }

            return points;

        }

        /// <summary>
        /// Processes the amount of points for the receipt time.
        /// </summary>
        /// <returns>The points awarded from the time.</returns>
        /// <exception cref="Exception"></exception>
        private int ProcessPointsForTime()
        {
            int points = 0;


            var result = TimeOnly.Parse(PurchaseTime);

            if (result.Hour <= 0) return 0;

            if (result.Hour >= DateTime.Today.AddHours(14).Hour && result.Hour <= DateTime.Today.AddHours(16).Hour)
            {
                points += 10;
            }

            return points;

        }
    }
}
