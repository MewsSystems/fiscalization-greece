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
            var numberWithValidName = ObjectValidations.NotNull(taxpayerNumber).Where(
                evaluator: taxNumber => taxNumber.Country.Equals(Countries.Greece).Implies(() => name.IsNull()),
                error: _ => new Error("Assignee name should not be provided for local counterpart.")
            );
            return numberWithValidName.Map(taxNumber => new InvoicePartyInfo(taxNumber, branch ?? NonNegativeInt.Zero(), name, address));
        }
    }
}
