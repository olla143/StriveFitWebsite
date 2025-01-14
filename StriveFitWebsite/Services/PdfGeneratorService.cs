using DinkToPdf;
using DinkToPdf.Contracts;
using System.IO;
namespace StriveFitWebsite.Services
{
    public class PdfGeneratorService
    {
        public byte[] GenerateInvoice(string memberName, string memberEmail, string subscriptionName, decimal subscriptionPrice)
        {
            // Initialize the PDF converter
            var converter = new SynchronizedConverter(new PdfTools());

            // Create the PDF document
            var doc = new HtmlToPdfDocument
            {
                GlobalSettings = new GlobalSettings
                {
                    ColorMode = ColorMode.Color,
                    Orientation = Orientation.Portrait, // Portrait or Landscape
                    PaperSize = PaperKind.A4 // Standard A4 paper size
                }
            };

            // Add the object settings (content) to the document
            doc.Objects.Add(new ObjectSettings
            {
                HtmlContent = $@"
                <h1>Invoice</h1>
                <p><strong>Member Name:</strong> {memberName}</p>
                <p><strong>Email:</strong> {memberEmail}</p>
                <p><strong>Subscription Name:</strong> {subscriptionName}</p>
                <p><strong>Amount:</strong> {subscriptionPrice:C}</p>
                <p><strong>Date:</strong> {DateTime.Now:yyyy-MM-dd}</p>",
                WebSettings = new WebSettings { DefaultEncoding = "utf-8" }
            });

            // Convert the document to a byte array
            return converter.Convert(doc);
        }
    }
}
