using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using Server.DBdata;
using Dapper;
using MySql.Data.MySqlClient;
using DBServer;
using DBServer.DBdata;
using System.Data.Common;

namespace Server
{

    class DBConnecter
    {
        //private static DBConnecter instance = null;

        private string ConnString = "Server=127.0.0.1;PORT=3306;Database=localhost;Uid=root;PASSWORD= ";
        private IDbConnection connection;

        //IDbConnection connection=new MySqlConnection();
        //string a = ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
        //connectionString


        public DBConnecter()
        {
            connection = new MySqlConnection(ConnString);
            connection.Open();
        }

        /* 싱글톤으로 하면 여러군대 에서 접근할때 에러발생
        /// <summary>
        /// 싱글톤 클래스
        /// </summary>
        /// <returns></returns>
        public static DBConnecter GetInstance()
        {
            if(instance == null)
            {
                instance = new DBConnecter();
            }

            return instance;
        }
        */

        public List<Cracker> GetCrackers()
        {

            var sql = "Select UniqueNumber,Name,Size From Cracker";
            IEnumerable<Cracker> a = connection.Query<Cracker>(sql);
            //IEnumerable<Cracker> a = Dapper.SqlMapper.Query<Cracker>(connection, sql);
            return a.ToList();
        }
        
        /// <summary>
        /// 템플릿타입에 맞는 데이터의 모든 테이블 값을 가져옵니다.
        /// 타입의 이름과 테이블 이름이 일치하고 테이블에 필드 이름과 클래스 내부 요소 이름이 같아야 한다.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public List<T> GetList<T>()
        {
            var sql = "Select * from " + typeof(T).Name;
            List<T> tmp;

            try
            {
                tmp = connection.Query<T>(sql).ToList();
            }
            catch(Exception e)
            {
                ExceptionMessage.PrintSQLException(e);

                return null;
            }
            return tmp;
        }

        /// <summary>
        /// sql을 사용직접 입력하여 값을 가져옵니다.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <returns></returns>
        public List<T> GetList<T>(string sql)
        {
            List<T> a;

            try
            {
                a = connection.Query<T>(sql).ToList();
            }
            catch(Exception e)
            {
                ExceptionMessage.PrintSQLException(e);
                return null;
            }

            return a;

        }


        public void InsertCraker(Cracker cracker)
        {
            connection.Execute(@"INSERT Cracker(UniqueNumber, Name, Size) values (@UniqueNumber, @Name, @Size)",
                new { UniqueNumber = cracker.UniqueNumber, Name = cracker.Name, Size = cracker.Size });
        }


        /* 밑에 있는 Insert<T>(T) 코드로 대체 
        /// <summary>
        /// {get; set;} 을 이용했으면 사용할수 없습니다.
        /// get_이름 set_이름 이렇게 형성됨.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="args"></param>
        public void InsertNew<T>(T args)
        {
            var objectType = typeof(object);

            var sql = "INSERT ";

            var type = typeof(T);


            var members = type.GetMembers();

            var objectMembers = objectType.GetMembers();

            var sqlVariable = "";

            sql += (type.Name + "(");

            int counter = 0;
            //이름
            foreach (var item in members)
            {
                bool isNotInObject = true;
                
                foreach(var a in objectMembers)
                {
                    if(a.Name == item.Name)
                    {
                        isNotInObject = false;
                    }
                }

                if(isNotInObject)
                {
                    if(counter == 0)
                    {
                        sql += item.Name;
                        sqlVariable += ("@" + item.Name);
                    }
                    else
                    {
                        sql += (", " + item.Name);
                        sqlVariable += (", @" + item.Name);
                    }

                    counter++;
                }
            }

            sql += ") VALUES (";
            sql += sqlVariable;
            sql += ")";

            //Execute sql
            connection.Execute(@sql,args);
        }
        */







        /*
             원본 출처
             https://www.codeproject.com/Tips/844691/Generic-Insert-for-Dapper
             
             제너릭을 이용하여넣는 코드 입니다.  
             필요에 맞춰 적당히 변형하였습니다.
        */

        [AttributeUsage(AttributeTargets.Property)]
        public class DapperKey : Attribute
        {
        }
        [AttributeUsage(AttributeTargets.Property)]
        public class DapperIgnore : Attribute
        {
        }
        
        /// <summary>
        /// objs를 db에 넣습니다.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="objs"></param>
        public void Insert<T>(T[] objs)
        {
            foreach(var item in objs)
            {
                Insert(item);
            }
        }

        /// <summary>
        /// 퍼포먼스를 위해 따로 만듬
        /// </summary>
        /// <param name="obj"></param>
        public void InsertRawData(RawData obj)
        {
            var sql = "INSERT RawData VALUES ";

            sql += obj.DataType;
            sql += ", ";
            sql += obj.EatTime;
            sql += ", ";
            sql += obj.CrackerUN;
            sql += ", ";
            sql += obj.CustomerUN;

            try
            {
                connection.Execute(sql);
            }
            catch(Exception e)
            {
                ExceptionMessage.PrintSQLException(e);
            }
            

        }

        /// <summary>
        /// obj를 db의 class와 이름이 같은 테이블에 넣어줍니다. 
        /// 요소의 이름도 같아야 합니다.
        /// RawData는 퍼포먼스를 위해 InsertRawData 를 이용하세요.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        public void Insert<T>(T obj)
        {
            var propertyContainer = ParseProperties(obj);
            var sql = string.Format("INSERT {0}({1}) VALUES(@{2})", // SELECT CAST(scope_identity() AS int)",
                typeof(T).Name,
                string.Join(", ", propertyContainer.ValueNames),
                string.Join(", @", propertyContainer.ValueNames));
            try
            {
                connection.Execute(sql, propertyContainer.ValuePairs, commandType: CommandType.Text);

            }
            catch(Exception e)
            {
                ExceptionMessage.PrintSQLException(e);
            }
            
            /* 필요 없는 부분
            using (var connection = this.connection)
            {
                var id = connection.Query<T>
                (sql, propertyContainer.ValuePairs, commandType: CommandType.Text).First();
                SetId(obj, id, propertyContainer.IdPairs);
            }
            */
        }

        public void SendSQL(string sql)
        {
            try
            {
                connection.Execute(sql);
            }
            catch(Exception e)
            {
                ExceptionMessage.PrintSQLException(e);
            }
        }

        public void Close()
        {
            connection.Close();
        }















        /*
        private void SetId<T>(T obj, T id, IDictionary<string, object> propertyPairs)
        {
            if (propertyPairs.Count == 1)
            {
                var propertyName = propertyPairs.Keys.First();
                var propertyInfo = obj.GetType().GetProperty(propertyName);
                if (propertyInfo.PropertyType == typeof(int))
                {
                    propertyInfo.SetValue(obj, id, null);
                }
            }
        }
        */
        

        private static PropertyContainer ParseProperties<T>(T obj)
        {
            var propertyContainer = new PropertyContainer();

            var typeName = typeof(T).Name;
            var validKeyNames = new[] { "Id",
            string.Format("{0}Id", typeName), string.Format("{0}_Id", typeName) };

            var properties = typeof(T).GetProperties();
            foreach (var property in properties)
            {
                // Skip reference types (but still include string!)
                if (property.PropertyType.IsClass && property.PropertyType != typeof(string))
                    continue;

                // Skip methods without a public setter
                if (property.GetSetMethod() == null)
                    continue;

                // Skip methods specifically ignored
                if (property.IsDefined(typeof(DapperIgnore), false))
                    continue;

                var name = property.Name;
                var value = typeof(T).GetProperty(property.Name).GetValue(obj, null);

                if (property.IsDefined(typeof(DapperKey), false) || validKeyNames.Contains(name))
                {
                    propertyContainer.AddId(name, value);
                }
                else
                {
                    propertyContainer.AddValue(name, value);
                }
            }

            return propertyContainer;
        }

        private class PropertyContainer
        {
            private readonly Dictionary<string, object> _ids;
            private readonly Dictionary<string, object> _values;

            #region Properties

            internal IEnumerable<string> IdNames
            {
                get { return _ids.Keys; }
            }

            internal IEnumerable<string> ValueNames
            {
                get { return _values.Keys; }
            }

            internal IEnumerable<string> AllNames
            {
                get { return _ids.Keys.Union(_values.Keys); }
            }

            internal IDictionary<string, object> IdPairs
            {
                get { return _ids; }
            }

            internal IDictionary<string, object> ValuePairs
            {
                get { return _values; }
            }

            internal IEnumerable<KeyValuePair<string, object>> AllPairs
            {
                get { return _ids.Concat(_values); }
            }

            #endregion

            #region Constructor

            internal PropertyContainer()
            {
                _ids = new Dictionary<string, object>();
                _values = new Dictionary<string, object>();
            }

            #endregion

            #region Methods

            internal void AddId(string name, object value)
            {
                _ids.Add(name, value);
            }

            internal void AddValue(string name, object value)
            {
                _values.Add(name, value);
            }

            #endregion
        }
    }
}
