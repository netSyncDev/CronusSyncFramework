using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cronus.Data;
using Cronus.Data.Sync;
using SQLite;

namespace Cronus.Windows.Test
{
    class TestEntity : SyncEntity
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string Name { get; set; }

        public DateTime GebDat { get; set; }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var db = new SQLiteConnection(@"G:\Test\CronusSync\Db\TestDb.sqlite");
            db.CreateTable<TestEntity>();
        }
    }
}
