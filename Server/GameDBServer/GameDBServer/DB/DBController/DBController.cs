using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using MySQLDriverCS;
using System.Reflection;
using GameDBServer.Logic;
using Server.Tools;
using System.Data;

namespace GameDBServer.DB.DBController
{
    /// <summary>
    /// DB映射缓存
    /// </summary>
    internal class DBMapper
    {
        //<columnName, FieldInfo or PropertyInfo>
        private Dictionary<string, MemberInfo> memberMappings = new Dictionary<string, MemberInfo>();

        public DBMapper(Type type)
        {
            MemberInfo[] members = type.GetMembers();

            foreach (MemberInfo member in members)
            {
                if (member.MemberType != MemberTypes.Field && member.MemberType != MemberTypes.Property)
                    continue;

                Object[] attributes = member.GetCustomAttributes(typeof(DBMappingAttribute), false);

                if (null == attributes)
                    continue;

                DBMappingAttribute[] mappingAttrs = (DBMappingAttribute[])attributes;

                foreach (DBMappingAttribute mappingAttr in mappingAttrs)
                {
                    if (null == mappingAttr.ColumnName || "".Equals(mappingAttr.ColumnName))
                        continue;

                    memberMappings.Add(mappingAttr.ColumnName, member);
                }

            }
        }

        public MemberInfo getMemberInfo(string columnName)
        {
            MemberInfo member = null;
            memberMappings.TryGetValue(columnName, out member);
            return member;
        }
    }

    /// <summary>
    /// Data Access Object数据访问对象
    /// </summary>
    public abstract class DBController<T>
    {
        protected DBManager dbMgr = DBManager.getInstance();

        private DBMapper mapper = null;

        protected DBController()
        {
            mapper = new DBMapper(typeof(T));
        }

        /// <summary>
        /// 查询返回单一实例
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <returns></returns>
        protected T queryForObject(string sql)
        {
            MySQLConnection conn = null;
            T obj = default(T);// Activator.CreateInstance<T>();
            try
            {
                conn = dbMgr.DBConns.PopDBConnection();

                MySQLCommand cmd = new MySQLCommand(sql, conn);
                MySQLDataReader reader = cmd.ExecuteReaderEx();

                int columnNum = reader.FieldCount;

                while (reader.Read())
                {
                    //索引下标
                    int index = 0;

                    for (int i = 0; i < columnNum; i++)
                    {
                        int _index = index++;
                        string columnName = reader.GetName(_index);
                        Object columnValue = reader.GetValue(_index);

                        if (null == obj)
                            obj = Activator.CreateInstance<T>();

                        setValue(obj, columnName, columnValue);

                    }

                    break;

                }

                GameDBManager.SystemServerSQLEvents.AddEvent(string.Format("+SQL: {0}", sql), EventLevels.Important);

                cmd.Dispose();
                cmd = null;
            }
            catch (Exception)
            {
                LogManager.WriteLog(LogTypes.Error, string.Format("查询数据库失败: {0}", sql));
                return default(T);
            }
            finally
            {
                if (null != conn)
                {
                    dbMgr.DBConns.PushDBConnection(conn);
                }
            }

            return obj;
        }

        /// <summary>
        /// 查询返回多个实例
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <returns>List<T>形式查询结果集s</returns>
        protected List<T> queryForList(string sql)
        {
            MySQLConnection conn = null;
            List<T> list = null;
            try
            {
                conn = dbMgr.DBConns.PopDBConnection();

                MySQLCommand cmd = new MySQLCommand(sql, conn);
                MySQLDataReader reader = cmd.ExecuteReaderEx();

                int columnNum = reader.FieldCount;

                //list = new List<T>();
                string[] nameArray = new string[columnNum];
                while (reader.Read())
                {
                    //索引下标
                    int index = 0;

                    T obj = Activator.CreateInstance<T>();
                    for (int i = 0; i < columnNum; i++)
                    {
                        int _index = index++;
                        if (null == nameArray[_index])
                        {
                            nameArray[_index] = reader.GetName(_index);
                        }

                        string columnName = nameArray[_index];
                        Object columnValue = reader.GetValue(_index);

                        if (null == list)
                            list = new List<T>();

                        setValue(obj, columnName, columnValue);

                    }

                    list.Add(obj);

                }

                GameDBManager.SystemServerSQLEvents.AddEvent(string.Format("+SQL: {0}", sql), EventLevels.Important);

                cmd.Dispose();
                cmd = null;
            }
            catch (Exception e)
            {
                LogManager.WriteLog(LogTypes.Error, string.Format("查询数据库失败: {0},exception:{1}", sql, e));
                return null;
            }
            finally
            {
                if (null != conn)
                {
                    dbMgr.DBConns.PushDBConnection(conn);
                }
            }

            return list;
        }


        /// <summary>
        /// 反射机制为对象属性赋值
        /// </summary>
        /// <param name="obj">对象实例</param>
        /// <param name="columnName">字段名</param>
        /// <param name="columnValue">字段值</param>
        private void setValue(Object obj, string columnName, Object columnValue)
        {
            MemberInfo member = mapper.getMemberInfo(columnName);

            if (null == member)
                return;

            switch (member.MemberType)
            {
                case MemberTypes.Field:
                    FieldInfo field = (FieldInfo)member;

                    if (field.FieldType.Equals(typeof(System.Int64)) && columnValue.GetType().Equals(typeof(System.String)))
                        columnValue = Convert.ToInt64(columnValue);
                    if (field.FieldType.Equals(typeof(System.Byte)) && (columnValue.GetType().Equals(typeof(System.String)) || columnValue.GetType().Equals(typeof(System.Int16)) || columnValue.GetType().Equals(typeof(System.Int32)) || columnValue.GetType().Equals(typeof(System.Int64))))
                        columnValue = Convert.ToByte(columnValue);
                    if (field.FieldType.Equals(typeof(System.String)) && (columnValue.GetType().Equals(typeof(byte[])) || columnValue.GetType().Equals(typeof(System.Byte[]))))
                    {
                        byte[] _columnValue = (byte[])columnValue;
                        columnValue = Convert.ToString(new UTF8Encoding().GetString(_columnValue, 0, _columnValue.Length));
                    }

                    field.SetValue(obj, columnValue);
                    break;
                case MemberTypes.Property:
                    PropertyInfo property = (PropertyInfo)member;
                    if (property.PropertyType.Equals(typeof(System.Int64)) && columnValue.GetType().Equals(typeof(System.String)))
                        columnValue = Convert.ToInt64(columnValue);
                    if (property.PropertyType.Equals(typeof(System.Byte)) && (columnValue.GetType().Equals(typeof(System.String)) || columnValue.GetType().Equals(typeof(System.Int16)) || columnValue.GetType().Equals(typeof(System.Int32)) || columnValue.GetType().Equals(typeof(System.Int64))))
                        columnValue = Convert.ToByte(columnValue);
                    if (property.PropertyType.Equals(typeof(System.String)) && columnValue.GetType().Equals(typeof(System.Byte[])))
                        columnValue = Convert.ToString(columnValue);
                    property.SetValue(obj, columnValue, null);
                    break;
            }
        }

        protected int insert(string sql)
        {
            MySQLConnection conn = null;
            int resultCount = -1;
            try
            {
                conn = dbMgr.DBConns.PopDBConnection();

                GameDBManager.SystemServerSQLEvents.AddEvent(string.Format("+SQL: {0}", sql), EventLevels.Important);

                MySQLCommand cmd = new MySQLCommand(sql, conn);

                try
                {
                    resultCount = cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("向数据库写入数据失败: {0},{1}", sql, ex));
                }

                cmd.Dispose();
                cmd = null;

            }
            finally
            {
                if (null != conn)
                {
                    dbMgr.DBConns.PushDBConnection(conn);
                }
            }

            return resultCount;
        }

        protected int update(string sql)
        {
            MySQLConnection conn = null;
            int resultCount = -1;
            try
            {
                conn = dbMgr.DBConns.PopDBConnection();

                GameDBManager.SystemServerSQLEvents.AddEvent(string.Format("+SQL: {0}", sql), EventLevels.Important);

                MySQLCommand cmd = new MySQLCommand(sql, conn);

                try
                {
                    resultCount = cmd.ExecuteNonQuery();
                }
                catch (Exception)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("向数据库更新数据失败: {0}", sql));
                }

                cmd.Dispose();
                cmd = null;

            }
            finally
            {
                if (null != conn)
                {
                    dbMgr.DBConns.PushDBConnection(conn);
                }
            }

            return resultCount;
        }

        protected int delete(string sql)
        {
            MySQLConnection conn = null;
            int resultCount = -1;
            try
            {
                conn = dbMgr.DBConns.PopDBConnection();

                GameDBManager.SystemServerSQLEvents.AddEvent(string.Format("+SQL: {0}", sql), EventLevels.Important);

                MySQLCommand cmd = new MySQLCommand(sql, conn);

                try
                {
                    resultCount = cmd.ExecuteNonQuery();
                }
                catch (Exception)
                {
                    LogManager.WriteLog(LogTypes.Error, string.Format("向数据库删除数据失败: {0}", sql));
                }

                cmd.Dispose();
                cmd = null;

            }
            finally
            {
                if (null != conn)
                {
                    dbMgr.DBConns.PushDBConnection(conn);
                }
            }

            return resultCount;
        }

    }

}
