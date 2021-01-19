using FuncSharp;
using Mews.Fiscalization.Core.Model;

namespace Mews.Fiscalization.Greece.Model
{
    public sealed class InvoicePartyInfo
    {
        private InvoicePartyInfo(TaxpayerIdentificationNumber taxpayerNumber, NonNegativeInt? branch = null, string name = null, Address address = null)
        {
            TaxpayerNumber = taxpayerNumber;
            Branch = branch ?? NonNegativeInt.Zero();
            Name = name.ToOption();
            Address = address.ToOption();
        }

        public NonNegativeInt Branch { get; }

        public TaxpayerIdentificationNumber TaxpayerNumber { get; }

        public IOption<string> Name { get; }

        public IOption<Address> Address { get; }

        public static ITry<InvoicePartyInfo, INonEmptyEnumerable<Error>> Create(TaxpayerIdentificationNumber taxpayerNumber, NonNegativeInt ? branch = null, string name = null, Address address = null)
        {
            return ObjectValidations.NotNull(taxpayerNumber).FlatMap(taxNumber =>
            {
                if (taxNumber.Country == Countries.Greece)
                {
                    return name.IsNull().Match(
                        t => Try.Success<InvoicePartyInfo, INonEmptyEnumerable<Error>>(new InvoicePartyInfo(taxNumber, branch ?? NonNegativeInt.Zero(), name, address)),
                        f => Try.Error<InvoicePartyInfo, INonEmptyEnumerable<Error>>(Error.Create("Assignee name should not be provided for local counterpart."))
                    );
                }
                return ObjectValidations.NotNull(name).Map(n => new InvoicePartyInfo(taxNumber, branch ?? NonNegativeInt.Zero(), n, address));
            });
        }
    }
}
