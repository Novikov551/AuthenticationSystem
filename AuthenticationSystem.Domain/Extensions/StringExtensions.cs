namespace AuthenticationSystem.Domain.Extensions
{
    public static class StringExtensions
    {
        public static bool IsNullOrEmpty(this string str)
        {
            return string.IsNullOrEmpty(str);
        }

        public static bool IsNullOrWhiteSpace(this string str)
        {
            return string.IsNullOrWhiteSpace(str);
        }

        public static string ToSnakeCase(this string str)
        {
            return string.Concat(str.Select((char x, int i) => (i <= 0 || !char.IsUpper(x)) ? x.ToString() : ("_" + x))).ToLower();
        }
    }
}
