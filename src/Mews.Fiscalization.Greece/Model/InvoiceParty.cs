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

        public NonNegativeInt? Branch
        {
            get
            {
                return Match(
                    localInvoiceParty => localInvoiceParty.Branch,
                    foreignInvoiceParty => foreignInvoiceParty.Branch
                );
            }
        }

        public TaxpayerIdentificationNumber TaxpayerNumber
        {
            get
            {
                return Match(
                    localInvoiceParty => localInvoiceParty.TaxIdentifier,
                    foreignInvoiceParty => foreignInvoiceParty.TaxIdentifier
                );
            }
        }

        public string Name
        {
            get
            {
                return Match(
                    localInvoiceParty => localInvoiceParty.Name,
                    foreignInvoiceParty => foreignInvoiceParty.Name
                );
            }
        }

        public Address Address
        {
            get
            {
                return Match(
                    localInvoiceParty => localInvoiceParty.Address,
                    foreignInvoiceParty => foreignInvoiceParty.Address
                );
            }
        }

        public static ITry<InvoiceParty, Error> Create(
            Country country,
            TaxpayerIdentificationNumber taxpayerNumber = null,
            NonNegativeInt? branch = null,
            string name = null,
            Address address = null)
        {
            return ObjectValidations.NotNull(country).FlatMap(c => c.Match(
                europeanUnionCountry =>
                {
                    if (europeanUnionCountry.Alpha2Code == "GR")
                    {
                        return LocalInvoiceParty.Create(Countries.GetByCode(europeanUnionCountry.Alpha2Code).Get(), taxpayerNumber, branch, name, address).Map(p => new InvoiceParty(p));
                    }
                    return ForeignInvoiceParty.Create(Countries.GetByCode(europeanUnionCountry.Alpha2Code).Get(), taxpayerNumber, branch, name, address).Map(p => new InvoiceParty(p));
                },
                nonEuropeanUnionCountry => ForeignInvoiceParty.Create(Countries.GetByCode(nonEuropeanUnionCountry.Alpha2Code).Get(), taxpayerNumber, branch, name, address).Map(p => new InvoiceParty(p))
            ));
        }
    }
}
