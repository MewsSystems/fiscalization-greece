using Mews.Fiscalization.Core.Model;
using Mews.Fiscalization.Greece.Model.Types;

namespace Mews.Fiscalization.Greece.Model
{
    public class ForeignCounterpart : Counterpart
    {
        public ForeignCounterpart(Country country, NonEmptyString taxIdentifier = null, NonNegativeInt branch = null, string name = null, Address address = null)
            : base(country, taxIdentifier, branch, name, address)
        {
            Check.Condition(country.Code != Country.Greece().Code, "Foreign counterpart cannot use greece as country.");
        }
    }
}
