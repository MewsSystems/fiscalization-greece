using FuncSharp;
using Mews.Fiscalization.Core.Model;

namespace Mews.Fiscalization.Greece.Model
{
    public sealed class LocalInvoiceParty
    {
        private LocalInvoiceParty(InvoicePartyInfo info, Country country)
        {
            Info = info;
            Country = country;
        }

        public InvoicePartyInfo Info { get; }

        public Country Country { get; }

        public static ITry<LocalInvoiceParty, INonEmptyEnumerable<Error>> Create(InvoicePartyInfo info, Country country)
        {
            return ObjectValidations.NotNull(info).FlatMap(i =>
            {
                var validCountry = country.ToTry(c => c.Alpha2Code == "GR", _ => Error.Create($"Country must be Greece for this invoice type: {nameof(LocalInvoiceParty)}"));
                return validCountry.Map(c => new LocalInvoiceParty(i, c));
            });
        }
    }
}
