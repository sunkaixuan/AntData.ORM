using System;
using System.Collections.Generic;
using System.Linq;

using AntData.ORM;
using AntData.ORM.Linq;
using AntData.ORM.Mapping;

namespace DbModels.Mysql
{
	/// <summary>
	/// Database       : testorm
	/// Data Source    : 127.0.0.1
	/// Server Version : 5.6.26-log
	/// </summary>
	public partial class Entitys : IEntity
	{
		public ITable<Person> People  { get { return this.Get<Person>(); } }
		public ITable<School> Schools { get { return this.Get<School>(); } }

		private readonly IDataContext con;

		public ITable<T> Get<T>()
			 where T : class
		{
			return this.con.GetTable<T>();
		}

		public Entitys(IDataContext con)
		{
			this.con = con;
		}
	}

	[Table("person")]
	public partial class Person : BaseEntity
	{
		#region Column

		/// <summary>
		/// 主键
		/// </summary>
		[Column("Id",                  DataType=DataType.Int64)   , PrimaryKey, Identity]
		public long Id { get; set; } // bigint(20)

		/// <summary>
		/// 最后更新时间
		/// </summary>
		[Column("DataChange_LastTime", DataType=DataType.DateTime), NotNull]
		public DateTime DataChangeLastTime // datetime
		{
			get { return _DataChangeLastTime; }
			set { _DataChangeLastTime = value; }
		}

		/// <summary>
		/// 姓名
		/// </summary>
		[Column("Name",                DataType=DataType.VarChar,  Length=50), NotNull]
		public string Name { get; set; } // varchar(50)

		/// <summary>
		/// 年纪
		/// </summary>
		[Column("Age",                 DataType=DataType.Int32)   ,    Nullable]
		public int? Age { get; set; } // int(11)

		/// <summary>
		/// 学校主键
		/// </summary>
		[Column("SchoolId",            DataType=DataType.Int64)   ,    Nullable]
		public long? SchoolId { get; set; } // bigint(20)

		#endregion

		#region Field

		private DateTime _DataChangeLastTime = System.Data.SqlTypes.SqlDateTime.MinValue.Value;

		#endregion

		#region Associations

		/// <summary>
		/// persons_school
		/// </summary>
		[Association(ThisKey="SchoolId", OtherKey="Id", CanBeNull=true, KeyName="persons_school", BackReferenceName="persons")]
		public School Personsschool { get; set; }

		#endregion
	}

	[Table("school")]
	public partial class School : BaseEntity
	{
		#region Column

		/// <summary>
		/// 主键
		/// </summary>
		[Column("Id",                  DataType=DataType.Int64)   , PrimaryKey, Identity]
		public long Id { get; set; } // bigint(20)

		/// <summary>
		/// 学校名称
		/// </summary>
		[Column("Name",                DataType=DataType.VarChar,  Length=50),    Nullable]
		public string Name { get; set; } // varchar(50)

		/// <summary>
		/// 学校地址
		/// </summary>
		[Column("Address",             DataType=DataType.VarChar,  Length=100),    Nullable]
		public string Address { get; set; } // varchar(100)

		/// <summary>
		/// 最后更新时间
		/// </summary>
		[Column("DataChange_LastTime", DataType=DataType.DateTime), NotNull]
		public DateTime DataChangeLastTime // datetime
		{
			get { return _DataChangeLastTime; }
			set { _DataChangeLastTime = value; }
		}

		#endregion

		#region Field

		private DateTime _DataChangeLastTime = System.Data.SqlTypes.SqlDateTime.MinValue.Value;

		#endregion

		#region Associations

		/// <summary>
		/// persons_school_BackReference
		/// </summary>
		[Association(ThisKey="Id", OtherKey="SchoolId", CanBeNull=true, IsBackReference=true)]
		public IEnumerable<Person> Persons { get; set; }

		#endregion
	}

	public static partial class TableExtensions
	{
		public static Person FindByBk(this ITable<Person> table, long Id)
		{
			return table.FirstOrDefault(t =>
				t.Id == Id);
		}

		public static School FindByBk(this ITable<School> table, long Id)
		{
			return table.FirstOrDefault(t =>
				t.Id == Id);
		}
	}
}
