namespace Cronus.Data.Sql
{
    /// <summary>
    /// Describes the Methods a SqlStatmentExecution Implementation has to implement
    /// </summary>
    public interface ISqlStatementExecutionEngine
    {
        bool ExecuteSqlStatement(string statement, SqlBuildOperations sqlOperation);
    }
}