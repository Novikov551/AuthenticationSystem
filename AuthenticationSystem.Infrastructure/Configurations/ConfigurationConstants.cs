namespace AuthenticationSystem.Infrastructure.Configurations;

public static class ConfigurationConstants
{
    public const string IntegerTypeName = "integer";
    public const string GuidTypeName = "uuid";
    public const string GuidAarrayTypeName = "uuid[]";
    public const string DateTimeTypeName = "timestamp with time zone";
    public const string DateOnlyTypeName = "date";
    public const string ContentTypeName = "text";
    public const string BinaryTypeName = "bytea";
    public const string BooleanTypeName = "boolean";
    public const string BigintTypeName = "bigint";
    public const string JsonbTypeName = "jsonb";

    public const string MoneyTypeName = "numeric(19, 2)";
    public const string DecimalTypeName = "numeric(19, 4)";
    public const string DecimalExtendTypeName = "numeric(17, 6)";
    public const string ShortTypeName = "smallint";

    public const int TickerMaxLength = 10;
    public const int ProductCodeMaxLength = 15;

    public const int GuidStringMaxLength = 36;

    public const int TokenMaxLenght = 2000;
    public const int EmailHashMaxLenght = 150;
}