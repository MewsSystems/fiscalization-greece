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
            SendInvoiceResults = SequentialEnumerable.Create(responseDoc.Responses.Select(r => new IndexedItem<SendInvoiceResult>(r.Index, new SendInvoiceResult(
                invoiceIdentifier: r.InvoiceUid,
                invoiceRegistrationNumber: r.InvoiceMark,
                invoiceRegistrationNumberSpecified: r.InvoiceMarkSpecified,
                errors: r.Errors?.Select(error => new ResultError(MapErrorCode(error.Code, r.StatusCode), error.Message))
            ))));
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
