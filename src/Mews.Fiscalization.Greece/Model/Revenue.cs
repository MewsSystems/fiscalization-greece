using FuncSharp;
using Mews.Fiscalization.Core.Model;

namespace Mews.Fiscalization.Greece.Model
{
    public sealed class Revenue : Coproduct2<NonNegativeRevenue, NegativeRevenue>
    {
        public Revenue(NonNegativeRevenue firstValue)
            : base(firstValue)
        {
        }

        public Revenue(NegativeRevenue firstValue)
            : base(firstValue)
        {
        }

        public decimal NetValue
        {
            get
            {
                return Match(
                    nonNegativeRevenue => nonNegativeRevenue.NetValue.Value,
                    negativeRevenue => negativeRevenue.NetValue.Value
                );
            }
        }

        public decimal VatValue
        {
            get
            {
                return Match(
                    nonNegativeRevenue => nonNegativeRevenue.VatValue.Value,
                    negativeRevenue => negativeRevenue.VatValue.Value
                );
            }
        }

        public TaxType TaxType
        {
            get
            {
                return Match(
                    nonNegativeRevenue => nonNegativeRevenue.TaxType,
                    negativeRevenue => negativeRevenue.TaxType
                );
            }
        }

        public VatExemptionType? VatExemption
        {
            get
            {
                return Match(
                    nonNegativeRevenue => nonNegativeRevenue.VatExemption,
                    negativeRevenue => negativeRevenue.VatExemption
                );
            }
        }

        public RevenueType RevenueType
        {
            get
            {
                return Match(
                    nonNegativeRevenue => nonNegativeRevenue.RevenueType,
                    negativeRevenue => negativeRevenue.RevenueType
                );
            }
        }

        public static ITry<Revenue, Error> Create(
            Amount netValue,
            Amount vatValue,
            TaxType taxType,
            RevenueType revenueType,
            VatExemptionType? vatExemption = null)
        {
            return ObjectValidations.NotNull(netValue).FlatMap(n =>
            {
                var validVat = ObjectValidations.NotNull(vatValue);
                return validVat.FlatMap(v =>
                {
                    if (n.Value >= 0 && v.Value >= 0)
                    {
                        var nonNegativeRevenue = Try.Aggregate(
                            NonNegativeAmount.Create(n.Value).MapError(e => e.ToEnumerable()),
                            NonNegativeAmount.Create(v.Value).MapError(e => e.ToEnumerable()),
                            (a, b) => NonNegativeRevenue.Create(a, b, taxType, revenueType, vatExemption)
                        ).Get(_ => new System.Exception());
                        return nonNegativeRevenue.Map(r => new Revenue(r));
                    }

                    var negativeRevenue = Try.Aggregate(
                        NegativeAmount.Create(n.Value).MapError(e => e.ToEnumerable()),
                        NonPositiveAmount.Create(v.Value).MapError(e => e.ToEnumerable()),
                        (a, b) => NegativeRevenue.Create(a, b, taxType, revenueType, vatExemption)
                    ).Get(_ => new System.Exception());
                    return negativeRevenue.Map(r => new Revenue(r));
                });
            });
        }
    }
}
