using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
namespace DalPro
{
    public static class DALPro
    {
        public static string ConnectionString = null!;

        private static readonly Dictionary<Type, PropertyInfo[]> _cacheProps =
        new Dictionary<Type, PropertyInfo[]>();

        public static SqlConnection GetConnection()
        {
            return new SqlConnection(ConnectionString);
        }

        // --------------------------------------------------
        // CREATE COMMAND
        // --------------------------------------------------
        private static SqlCommand CreateCommand(
        string sql,
            SqlTransaction? trans = null,
            Dictionary<string, object>? parameters = null)
        {
            SqlCommand cmd;

            if (trans != null)
                cmd = new SqlCommand(sql, trans.Connection, trans);
            else
            {
                SqlConnection cn = GetConnection();
                cn.Open();
                cmd = new SqlCommand(sql, cn);
            }

            if (parameters != null)
            {
                foreach (var p in parameters)
                    cmd.Parameters.AddWithValue(p.Key, p.Value ?? DBNull.Value);
            }

            return cmd;
        }

        // --------------------------------------------------
        // EXECUTE NON QUERY
        // --------------------------------------------------
        public static int Execute(
        string sql,
            Dictionary<string, object>? parameters = null,
            SqlTransaction? trans = null)
        {
            using SqlCommand cmd = CreateCommand(sql, trans, parameters);

            int result = cmd.ExecuteNonQuery();

            if (trans == null)
                cmd.Connection.Close();

            return result;
        }

        // --------------------------------------------------
        // EXECUTE SCALAR
        // --------------------------------------------------
        public static object ExecuteScalar(
        string sql,
            Dictionary<string, object>? parameters = null,
            SqlTransaction? trans = null)
        {
            using SqlCommand cmd = CreateCommand(sql, trans, parameters);

            object result = cmd.ExecuteScalar();

            if (trans == null)
                cmd.Connection.Close();

            return result;
        }

        // --------------------------------------------------
        // QUERY GENERIC
        // --------------------------------------------------
        public static List<T> Query<T>(
        string sql,
            Dictionary<string, object>? parameters = null,
            SqlTransaction? trans = null) where T : new()
        {
            List<T> list = new();


            using SqlCommand cmd = CreateCommand(sql, trans, parameters);
            using SqlDataReader dr = cmd.ExecuteReader();

            PropertyInfo[]? props;

            if (!_cacheProps.TryGetValue(typeof(T), out props))
            {
                props = typeof(T).GetProperties();
                _cacheProps[typeof(T)] = props;
            }

            while (dr.Read())
            {
                
                T obj = new T();

                foreach (var prop in props)
                {
                    try
                    {
                        int idx = dr.GetOrdinal(prop.Name);

                        if (!dr.IsDBNull(idx))
                            prop.SetValue(obj, dr[idx]);
                    }
                    catch { }
                }

                list.Add(obj);
            }

            if (trans == null)
                cmd.Connection.Close();

            return list;
        }

        // --------------------------------------------------
        // DATATABLE FOR UPDATE
        // --------------------------------------------------
        public static DataTable DataTableForUpdate(
        string sql,
        ref SqlDataAdapter da,
            SqlTransaction? trans = null)
        {
            SqlConnection cn;

            if (trans != null)
                cn = trans.Connection;
            else
            {
                cn = GetConnection();
                cn.Open();
            }

            da = new SqlDataAdapter(sql, cn);

            if (trans != null)
                da.SelectCommand.Transaction = trans;

            da.MissingSchemaAction = MissingSchemaAction.AddWithKey;

            SqlCommandBuilder cb = new SqlCommandBuilder(da);
            cb.QuotePrefix = "[";
            cb.QuoteSuffix = "]";

            DataTable dt = new DataTable();
            da.Fill(dt);

            if (trans == null)
                cn.Close();

            return dt;
        }

        // --------------------------------------------------
        // STORED PROCEDURE
        // --------------------------------------------------
        public static DataTable ExecuteSP(
        string spName,
            Dictionary<string, object>? parameters = null,
            SqlTransaction? trans = null)
        {
            SqlConnection cn;

            if (trans != null)
                cn = trans.Connection;
            else
            {
                cn = GetConnection();
                cn.Open();
            }

            SqlCommand cmd = new SqlCommand(spName, cn);
            cmd.CommandType = CommandType.StoredProcedure;

            if (trans != null)
                cmd.Transaction = trans;

            if (parameters != null)
            {
                foreach (var p in parameters)
                    cmd.Parameters.AddWithValue(p.Key, p.Value ?? DBNull.Value);
            }

            SqlDataAdapter da = new SqlDataAdapter(cmd);

            DataTable dt = new DataTable();
            da.Fill(dt);

            if (trans == null)
                cn.Close();

            return dt;
        }

        // --------------------------------------------------
        // TRANSACTIONS
        // --------------------------------------------------
        public static SqlTransaction BeginTransaction(string? connectionString = null)
        {

            SqlConnection cn = (connectionString == null) ? GetConnection() : new SqlConnection(connectionString);
            cn.Open();
            return cn.BeginTransaction();
        }

        public static void Commit(SqlTransaction trans)
        {
            SqlConnection cn = trans.Connection;

            trans.Commit();

            if (cn.State == ConnectionState.Open)
                cn.Close();
        }

        public static void Rollback(SqlTransaction trans)
        {
            SqlConnection cn = trans.Connection;

            trans.Rollback();

            if (cn.State == ConnectionState.Open)
                cn.Close();

        }

    }
}