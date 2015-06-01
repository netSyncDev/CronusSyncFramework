using Cronus.Data.Sql;

namespace Cronus.Data.Sync
{
    public interface IChangesExecuter
    {
        bool ExecuteSqlStatement(string statement, SqlBuildOperations sqlOperation);
    }
}