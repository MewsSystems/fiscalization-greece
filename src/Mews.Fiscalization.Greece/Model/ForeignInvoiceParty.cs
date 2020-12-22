using FuncSharp;
using Mews.Fiscalization.Core.Model;

namespace Mews.Fiscalization.Greece.Model
{
    public sealed class ForeignInvoiceParty
    {
        private ForeignInvoiceParty(Country country, NonNegativeInt branch, TaxpayerIdentificationNumber taxIdentifier = null, string name = null, Address address = null)
        {
            Country = country;
            TaxIdentifier = taxIdentifier;
            Branch = branch;
            Name = name;
            Address = address;
        }

        public Country Country { get; }

        public NonNegativeInt Branch { get; }

        public TaxpayerIdentificationNumber TaxIdentifier { get; }

        public string Name { get; }

        public Address Address { get; }

        public static ITry<ForeignInvoiceParty, Error> Create(
            Country country,
            TaxpayerIdentificationNumber taxpayerNumber = null,
            NonNegativeInt? branch = null,
            string name = null,
            Address address = null)
        {
            return ObjectValidations.NotNull(country).FlatMap(c =>
            {
                var validCountry = country.ToTry(cc => cc.Alpha2Code != "GR", _ => new Error($"{nameof(ForeignInvoiceParty)} cannot use greece as a country.")) ;
                return validCountry.Map(cc => new ForeignInvoiceParty(cc, branch ?? NonNegativeInt.CreateUnsafe(0), taxpayerNumber, name, address));
            });
        }
    }
}
