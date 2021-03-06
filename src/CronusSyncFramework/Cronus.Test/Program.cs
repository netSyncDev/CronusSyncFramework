﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cronus.Data;
using Cronus.Data.Sql;

namespace Cronus.Test
{
    [Table("TestTable")]
    class TestEntity : DataEntity
    {
        [EntityColumnName("Id")]
        [PK(PKType.Normal)]
        public int Id { get; set; }

        [EntityColumnName("Id2")]
        [PK(PKType.Normal)]
        public int Id2 { get; set; }

        private string _name;

        [EntityColumnName("Name")]
        public string Name
        {
            get { return this._name; }
            set { base.SetEntityValue<string>(ref this._name, value); }
        }
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
