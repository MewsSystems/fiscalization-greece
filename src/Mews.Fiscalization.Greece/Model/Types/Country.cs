namespace Mews.Fiscalization.Greece.Model.Types
{
    public class Country
    {
        private const string GreeceCountryCode = "GR";

        public Country(CountryCode code, bool isWithinEU)
        {
            Code = code;
            IsWithinEU = isWithinEU;
        }

        public CountryCode Code { get; }
        public bool IsWithinEU { get; }

        public static Country Greece()
        {
            return new Country(new CountryCode(GreeceCountryCode), isWithinEU: true);
        }
    }
}
