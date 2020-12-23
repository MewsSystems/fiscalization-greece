using FuncSharp;
using Mews.Fiscalization.Core.Model;

namespace Mews.Fiscalization.Greece.Model
{
    public sealed class InvoiceParty :  Coproduct2<LocalInvoiceParty, ForeignInvoiceParty>
    {
        public InvoiceParty(LocalInvoiceParty localParty)
            : base(localParty)
        {
        }

        public InvoiceParty(ForeignInvoiceParty foreeignParty)
            : base(foreeignParty)
        {
        }

        public InvoicePartyInfo Info
        {
            get
            {
                return Match(
                    localInvoiceParty => localInvoiceParty.Info,
                    foreignInvoiceParty => foreignInvoiceParty.Info
                );
            }
        }

        public Country Country
        {
            get
            {
                return Match(
                    localInvoiceParty => localInvoiceParty.Country,
                    foreignInvoiceParty => foreignInvoiceParty.Country
                );
            }
        }

        public static ITry<InvoiceParty, Error> Create(InvoicePartyInfo info, Country country)
        {
            if (info.IsNull())
            {
                Try.Error<InvoiceParty, Error>(new Error($"{nameof(info)} cannot be null."));
            }

            return ObjectValidations.NotNull(country).FlatMap(c => c.Match(
                europeanUnionCountry =>
                {
                    if (europeanUnionCountry.Alpha2Code == "GR")
                    {
                        return LocalInvoiceParty.Create(info, Countries.GetByCode(europeanUnionCountry.Alpha2Code).Get()).Map(p => new InvoiceParty(p));
                    }
                    return ForeignInvoiceParty.Create(info, Countries.GetByCode(europeanUnionCountry.Alpha2Code).Get()).Map(p => new InvoiceParty(p));
                },
                nonEuropeanUnionCountry => ForeignInvoiceParty.Create(info, Countries.GetByCode(nonEuropeanUnionCountry.Alpha2Code).Get()).Map(p => new InvoiceParty(p))
            ));
        }
    }
}
