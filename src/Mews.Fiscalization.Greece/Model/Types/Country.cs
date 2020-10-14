namespace Mews.Fiscalization.Greece.Model.Types
{
    public class Country
    {
        public static readonly Country Greece = new Country(new CountryCode("GR"), isWithinEU: true);

        public Country(CountryCode code, bool isWithinEU)
        {
            Code = code;
            IsWithinEU = isWithinEU;
        }

        public CountryCode Code { get; }
        public bool IsWithinEU { get; }
    }
}
