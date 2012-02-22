namespace DbTransmogrifier.Dialects
{
    public interface ISqlDialect
    {
        string DatabaseExists { get; }
        string CreateDatabase { get; }
        string DropDatabase { get; }
    }
}