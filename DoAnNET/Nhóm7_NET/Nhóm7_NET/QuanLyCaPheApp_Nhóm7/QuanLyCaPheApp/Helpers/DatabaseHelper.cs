using Microsoft.Data.SqlClient;

namespace QuanLyCaPheApp.Helpers
{
    public static class DatabaseHelper
    {
        //data SQL
        private const string ConnectionString =
    "Server=DESKTOP-NLH6NL3\\MSSQLSERVER01;Database=QuanLyQuanCaPhe;" +
    "User Id=sa;Password=123;TrustServerCertificate=True;Encrypt=False;";

        public static SqlConnection GetConnection()
            => new SqlConnection(ConnectionString);

        public static bool TestConnection()
        {
            try { using var c = GetConnection(); c.Open(); return true; }
            catch { return false; }
        }

        public static int ExecuteNonQuery(string sql, SqlParameter[]? parameters = null)
        {
            try
            {
                using var conn = GetConnection();
                conn.Open();
                using var cmd = new SqlCommand(sql, conn);
                if (parameters != null) cmd.Parameters.AddRange(parameters);
                return cmd.ExecuteNonQuery();
            }
            catch { return 0; }
        }

        public static object? ExecuteScalar(string sql, SqlParameter[]? parameters = null)
        {
            try
            {
                using var conn = GetConnection();
                conn.Open();
                using var cmd = new SqlCommand(sql, conn);
                if (parameters != null) cmd.Parameters.AddRange(parameters);
                return cmd.ExecuteScalar();
            }
            catch { return null; }
        }

        public static List<T> ExecuteQuery<T>(string sql, Func<SqlDataReader, T> map,
            SqlParameter[]? parameters = null)
        {
            var list = new List<T>();
            try
            {
                using var conn = GetConnection();
                conn.Open();
                using var cmd = new SqlCommand(sql, conn);
                if (parameters != null) cmd.Parameters.AddRange(parameters);
                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    try { list.Add(map(reader)); }
                    catch { /* bỏ qua dòng lỗi, tiếp tục */ }
                }
            }
            catch { /* bỏ qua lỗi kết nối, trả về list rỗng */ }
            return list;
        }

        public static T? ExecuteQuerySingle<T>(string sql, Func<SqlDataReader, T> map,
            SqlParameter[]? parameters = null) where T : class
        {
            try
            {
                using var conn = GetConnection();
                conn.Open();
                using var cmd = new SqlCommand(sql, conn);
                if (parameters != null) cmd.Parameters.AddRange(parameters);
                using var reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    try { return map(reader); }
                    catch { return null; }
                }
                return null;
            }
            catch { return null; }
        }

        public static string SafeString(SqlDataReader r, string col)
        {
            try
            {
                int i = r.GetOrdinal(col);
                return r.IsDBNull(i) ? "" : r.GetString(i);
            }
            catch { return ""; }
        }

        public static int SafeInt(SqlDataReader r, string col)
        {
            try
            {
                int i = r.GetOrdinal(col);
                return r.IsDBNull(i) ? 0 : r.GetInt32(i);
            }
            catch { return 0; }
        }

        public static decimal SafeDecimal(SqlDataReader r, string col)
        {
            try
            {
                int i = r.GetOrdinal(col);
                return r.IsDBNull(i) ? 0m : r.GetDecimal(i);
            }
            catch { return 0m; }
        }

        public static bool SafeBool(SqlDataReader r, string col)
        {
            try
            {
                int i = r.GetOrdinal(col);
                return !r.IsDBNull(i) && r.GetBoolean(i);
            }
            catch { return false; }
        }

        public static DateTime SafeDateTime(SqlDataReader r, string col)
        {
            try
            {
                int i = r.GetOrdinal(col);
                return r.IsDBNull(i) ? DateTime.MinValue : r.GetDateTime(i);
            }
            catch { return DateTime.MinValue; }
        }

        public static DateTime? SafeDateTimeNull(SqlDataReader r, string col)
        {
            try
            {
                int i = r.GetOrdinal(col);
                return r.IsDBNull(i) ? null : r.GetDateTime(i);
            }
            catch { return null; }
        }

        public static int? SafeIntNull(SqlDataReader r, string col)
        {
            try
            {
                int i = r.GetOrdinal(col);
                return r.IsDBNull(i) ? null : r.GetInt32(i);
            }
            catch { return null; }
        }

        public static decimal? SafeDecimalNull(SqlDataReader r, string col)
        {
            try
            {
                int i = r.GetOrdinal(col);
                return r.IsDBNull(i) ? null : r.GetDecimal(i);
            }
            catch { return null; }
        }
        public static bool HasColumn(SqlDataReader r, string col)
        {
            try { r.GetOrdinal(col); return true; }
            catch { return false; }
        }
    }
}
