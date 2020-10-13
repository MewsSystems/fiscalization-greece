using Mews.Fiscalization.Greece.Dto.Xsd;
using System.Linq;
using Mews.Fiscalization.Core.Model;
using Mews.Fiscalization.Greece.Model.Collections;

namespace Mews.Fiscalization.Greece.Model.Result
{
    public class SendInvoicesResult
    {
        internal SendInvoicesResult(ResponseDoc responseDoc)
        {
            SendInvoiceResults = SequentialEnumerable.Create(responseDoc.Responses.Select(response => new IndexedItem<SendInvoiceResult>(response.Index, new SendInvoiceResult(
                invoiceIdentifier: response.InvoiceUid,
                invoiceRegistrationNumber: response.InvoiceMark,
                invoiceRegistrationNumberSpecified: response.InvoiceMarkSpecified,
                errors: response.Errors?.Select(error => new ResultError(MapErrorCode(error.Code, response.StatusCode), error.Message))))));
        }

        public ISequentialEnumerable<SendInvoiceResult> SendInvoiceResults { get; }

        private string MapErrorCode(string errorCode, StatusCode statusCode)
        {
            // Error codes which are returned from API have some integer value that describes particular error. But we need only category of the error
            // so we return value of the status code.
            if (int.TryParse(errorCode, out _))
            {
                return statusCode.ToString();
            }

            return errorCode;
        }
    }
}
