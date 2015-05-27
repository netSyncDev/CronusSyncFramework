using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cronus.Core.Data;
using Cronus.Core.Data.Sql;

namespace Cronus.Test
{
    [Table("TestTable")]
    class TestEntity : DataEntity
    {
        [EntityColumnName("Id")]
        [Pk(PkAttributeType.Normal)]
        public int Id { get; set; }

        [EntityColumnName("Id2")]
        [Pk(PkAttributeType.Normal)]
        public int Id2 { get; set; }

        [EntityColumnName("Name")]
        public string Name { get; set; }
    }

    class Program
    {
        static void Main(string[] args)
        {
            DataEntity.SetTargetDatabaseType(DatabaseType.MsSql);

            TestEntity test = new TestEntity();
            test.Id = 1;
            test.Name = "Test";

            string cmd = test.GetInsertCommand();
        }
    }
}
