using Mews.Fiscalization.Greece.Dto.Xsd;
using System;

namespace Mews.Fiscalization.Greece.Model.Types
{
    public class Country
    {
        public Country(CountryCode code, bool isWithinEU)
        {
            Code = code;
            IsWithinEU = isWithinEU;
        }

        public CountryCode Code { get; }
        public bool IsWithinEU { get; }
    }
}
