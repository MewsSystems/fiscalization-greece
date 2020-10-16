using Mews.Fiscalization.Core.Model;
using Mews.Fiscalization.Greece.Model.Types;

namespace Mews.Fiscalization.Greece.Model
{
    public class LocalInvoiceParty : InvoiceParty
    {
        public LocalInvoiceParty(GreekTaxIdentifier taxIdentifier, NonNegativeInt branch = null, string name = null, Address address = null)
            : base(Country.Greece, taxIdentifier: taxIdentifier, branch, name ,address)
        {
        }
    }
}
