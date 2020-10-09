using System;
using Mews.Fiscalization.Greece.Model.Types;

namespace Mews.Fiscalization.Greece.Model
{
    public class Counterpart
    {
        public Counterpart(Country country, NonEmptyString taxIdentifier = null, NonNegativeInt branch = null, string name = null, Address address = null)
        {
            TaxIdentifier = taxIdentifier ?? new NonEmptyString("0");
            Country = country ?? throw new ArgumentNullException(nameof(country));
            Branch = branch ?? new NonNegativeInt(0);
            Name = name;
            Address = address;
        }

        public NonEmptyString TaxIdentifier { get; }

        public NonNegativeInt Branch { get; }

        public string Name { get; }

        public Country Country { get; }

        public Address Address { get; }
    }
}
