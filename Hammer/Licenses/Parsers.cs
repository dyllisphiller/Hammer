namespace Hammer.Parsers
{
    interface ICallsignParser
    {
        string GetCountryOfOrigin();
        char[] ToCharArray();
    }
}
