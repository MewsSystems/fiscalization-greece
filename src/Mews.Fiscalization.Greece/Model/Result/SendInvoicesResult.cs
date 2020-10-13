using Mews.Fiscalization.Greece.Dto.Xsd;
using System.Linq;
using System.Collections.Generic;
using Mews.Fiscalization.Core.Model;
using Mews.Fiscalization.Greece.Model.Collections;

namespace Mews.Fiscalization.Greece.Model.Result
{
    public class SendInvoicesResult
    {
        internal SendInvoicesResult(ResponseDoc responseDoc)
        {
            var indexedResults = responseDoc.Responses.Select(r => new IndexedItem<SendInvoiceResult>(
                index: r.Index,
                value: new SendInvoiceResult(
                    invoiceIdentifier: r.InvoiceUid,
                    invoiceRegistrationNumber: r.InvoiceMark,
                    invoiceRegistrationNumberSpecified: r.InvoiceMarkSpecified,
                    errors: r.Errors?.Select(error => new SendInvoiceError(MapErrorCode(error.Code, r.StatusCode), error.Message))
                )
            ));
            SendInvoiceResults = new SequentialEnumerable<SendInvoiceResult>(indexedResults);
        }

        public SequentialEnumerable<SendInvoiceResult> SendInvoiceResults { get; }

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
