using Mews.Fiscalization.Greece.Model.Types;

namespace Mews.Fiscalization.Greece.Model
{
    public class ForeignInvoiceParty : InvoiceParty
    {
        public ForeignInvoiceParty(Country country, Core.Model.NonEmptyString taxIdentifier = null, Core.Model.NonNegativeInt branch = null, string name = null, Address address = null)
            : base(country, taxIdentifier, branch, name, address)
        {
            Core.Model.Check.Condition(country.Code != Country.Greece.Code, "Foreign counterpart cannot use greece as a country.");
        }
    }
}
