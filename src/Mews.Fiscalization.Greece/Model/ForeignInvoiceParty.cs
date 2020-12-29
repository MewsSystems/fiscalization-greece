using FuncSharp;
using Mews.Fiscalization.Core.Model;

namespace Mews.Fiscalization.Greece.Model
{
    public sealed class ForeignInvoiceParty
    {
        private ForeignInvoiceParty(InvoicePartyInfo info, Country country)
        {
            Country = country;
            Info = info;
        }

        public InvoicePartyInfo Info { get; }

        public Country Country { get; }

        public static ITry<ForeignInvoiceParty, Error> Create(InvoicePartyInfo info, Country country)
        {
            return ObjectValidations.NotNull(info).FlatMap(i =>
            {
                var validCountry = country.ToTry(c => c.Alpha2Code != "GR", _ => new Error($"{nameof(ForeignInvoiceParty)} cannot use greece as a country.")) ;
                return validCountry.Map(c => new ForeignInvoiceParty(i, c));
            });
        }
    }
}
