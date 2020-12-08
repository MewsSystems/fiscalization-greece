using System;
using Mews.Fiscalization.Greece.Model.Types;

namespace Mews.Fiscalization.Greece.Model
{
    public abstract class InvoiceParty
    {
        public InvoiceParty(Country country, Core.Model.NonEmptyString taxIdentifier = null, Core.Model.NonNegativeInt branch = null, string name = null, Address address = null)
        {
            TaxIdentifier = taxIdentifier ?? new Core.Model.NonEmptyString("0");
            Country = country ?? throw new ArgumentNullException(nameof(country));
            Branch = branch ?? new Core.Model.NonNegativeInt(0);
            Name = name;
            Address = address;
        }

        public Core.Model.NonEmptyString TaxIdentifier { get; }

        public Core.Model.NonNegativeInt Branch { get; }

        public string Name { get; }

        public Country Country { get; }

        public Address Address { get; }
    }
}
