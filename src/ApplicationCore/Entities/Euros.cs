using CSharpFunctionalExtensions;
using System.Collections.Generic;

namespace ApplicationCore.Entities
{
    public class Euros : ValueObject
    {
        private const decimal MaxEuroAmount = 1_000_000;

        public decimal Value { get; private set; }

        public bool IsZero => Value == 0;

        private Euros(decimal value)
        {
            Value = value;
        }

        public static Result<Euros> Create(decimal euroAmount)
        {
            if (euroAmount < 0)
            {
                return Result.Fail<Euros>("Euro amount cannot be negative");
            }

            if (euroAmount > MaxEuroAmount)
            {
                return Result.Fail<Euros>("Euro amount cannot be greater than " + MaxEuroAmount);
            }

            if (euroAmount % 0.01m > 0)
            {
                return Result.Fail<Euros>("Euro amount cannot contain part of a cent");
            }

            return Result.Ok(new Euros(euroAmount));
        }

        public static Euros Of(decimal euroAmount)
        {
            return Create(euroAmount).Value;
        }

        public static Euros operator *(Euros euros, decimal multiplier)
        {
            return new Euros(euros.Value * multiplier);
        }

        public static Euros operator +(Euros euros1, Euros euros2)
        {
            return new Euros(euros1.Value + euros2.Value);
        }

        public static implicit operator decimal(Euros euros)
        {
            return euros.Value;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }
    }
}
