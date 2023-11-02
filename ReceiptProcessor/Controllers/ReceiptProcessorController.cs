using Microsoft.AspNetCore.Mvc;
using ReceiptProcessor.Models;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace ReceiptProcessor.Controllers
{
    public class ReceiptProcessorController : ControllerBase
    {
        /// <summary>
        /// Gets all receipts.
        /// </summary>
        /// <returns>The list of receipts.</returns>
        [HttpGet("receipts", Name = "GetAllReceipts")]
        public ActionResult<List<Receipt>> GetAllReceipts()
        {
            var listReceipts = GetFromHttpContext<Receipt>("Receipts");

            if (listReceipts.Any())
            {
                return Ok(listReceipts);
            }

            return NotFound("No receipts found");
        }

        /// <summary>
        /// Gets all receipt points.
        /// </summary>
        /// <returns>The list of peceipt points.</returns>
        [HttpGet("receiptPoints", Name = "GetAllReceiptPoints")]
        public ActionResult<List<ReceiptPoints>> GetAllReceiptPoints()
        {
            var listReceiptPoints = GetFromHttpContext<ReceiptPoints>("ReceiptPoints");

            if (listReceiptPoints.Any())
            {
                return Ok(listReceiptPoints);
            }

            return NotFound("No receiptPoints found");
        }

        /// <summary>
        /// Gets the receipt points by id.
        /// </summary>
        /// <param name="id">The id of the receipt.</param>
        /// <returns>The receiptPoints by Id.</returns>
        [HttpGet("receipts/{id}/points", Name = "GetReceiptPoints")]
        public ActionResult<ReceiptPoints> GetReceiptPoints(string id)
        {
            var receiptPoints = GetFromHttpContext<ReceiptPoints>("ReceiptPoints");

            var result = receiptPoints.FirstOrDefault(r => r.Id == Guid.Parse(id));

            if (result != null)
            {
                return Ok(result);
            }
            return NotFound("No receipt found for that id");
        }

        /// <summary>
        /// Processes the Receipt and calculates the Receipt Points then saves both to memory.
        /// </summary>
        /// <param name="receipt">The receipt to process and save.</param>
        /// <returns>The receiptPoints object with the id and amount of points awarded.</returns>
        [HttpPost("receipts/process", Name = "ProcessReceipt")]
        public ActionResult<JObject> ProcessReceipt([FromBody] Receipt receipt)
        {
            try
            {
                if (receipt != null)
                {
                    var points = receipt.CalculateReceiptPoints();
                    var receiptPoint = new ReceiptPoints
                    {
                        Id = receipt.Id,
                        Points = points
                    };

                    SaveReceipt(receipt);
                    SaveReceiptPoints(receiptPoint);

                    return Ok(receiptPoint);
                }
                else
                {
                    return NotFound("The receipt is invalid");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Appends to existing or else saves to a new Receipts List.
        /// </summary>
        /// <param name="receipt">The receipt to save to the Receipt List.</param>
        private void SaveReceipt(Receipt receipt)
        {
            List<Receipt> listReceipts = GetFromHttpContext<Receipt>("Receipts");
            listReceipts.Add(receipt);

            HttpContext.Session.SetString("Receipts", JsonConvert.SerializeObject(listReceipts));
        }

        /// <summary>
        /// Appends to existing or else saves to a new ReceiptPoints List.
        /// </summary>
        /// <param name="receiptPoints">The ReceiptPoints to save to the ReceiptPoints List.</param>
        private void SaveReceiptPoints(ReceiptPoints receiptPoints)
        {
            List<ReceiptPoints> listReceiptPoints = GetFromHttpContext<ReceiptPoints>("ReceiptPoints");
            listReceiptPoints.Add(receiptPoints);

            HttpContext.Session.SetString("ReceiptPoints", JsonConvert.SerializeObject(listReceiptPoints));
        }

        /// <summary>
        /// Gets the specified key from the httpcontext.
        /// </summary>
        /// <typeparam name="T">The type of object.</typeparam>
        /// <param name="key">The key name of the object.</param>
        /// <returns>A List of new or existing objects of the specified type.</returns>
        private List<T> GetFromHttpContext<T>(string key)
        {
            var stringData = HttpContext.Session.GetString(key);

            if (string.IsNullOrEmpty(stringData)) return new List<T>();

            var jsonData = JsonConvert.DeserializeObject<List<T>>(stringData);

            return jsonData ?? new List<T>();
        }
    }
}
