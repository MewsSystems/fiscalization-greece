using FuncSharp;
using Mews.Fiscalization.Core.Model;

namespace Mews.Fiscalization.Greece.Model
{
    public sealed class Payment : Coproduct2<NonNegativePayment, NegativePayment>
    {
        public Payment(NonNegativePayment nonNegativePayment)
            : base(nonNegativePayment)
        {
        }

        public Payment(NegativePayment negativePayment)
            : base(negativePayment)
        {
        }

        public decimal Amount
        {
            get
            {
                return Match(
                    nonNegativePayment => nonNegativePayment.Amount.Value,
                    negativePayment => negativePayment.Amount.Value
                );
            }
        }

        public PaymentType PaymentType
        {
            get
            {
                return Match(
                    nonNegativePayment => nonNegativePayment.PaymentType,
                    negativePayment => negativePayment.PaymentType
                );
            }
        }

        public static ITry<Payment, Error> Create(Amount amount, PaymentType paymentType)
        {
            return ObjectValidations.NotNull(amount).FlatMap(c =>
            {
                return c.Match(
                    nonPositiveAmount => Try.Error<Payment, Error>(new Error("Payment amount can be only either non negative or negative.")),
                    nonNegativeAmount => NonNegativePayment.Create(nonNegativeAmount, paymentType).Map(p => new Payment(p)),
                    negativeAmount => NegativePayment.Create(negativeAmount, paymentType).Map(p => new Payment(p))
                );
            });
        }
    }
}
