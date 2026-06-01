using Microsoft.Data.SqlClient;
using QuanLyCaPheApp.Helpers;

namespace QuanLyCaPheApp.Repositories
{
    public abstract class BaseRepository
    {
        protected SqlConnection GetConnection() => DatabaseHelper.GetConnection();
        protected static string S(SqlDataReader r, string col) => DatabaseHelper.SafeString(r, col);
        protected static int    I(SqlDataReader r, string col) => DatabaseHelper.SafeInt(r, col);
        protected static decimal D(SqlDataReader r, string col) => DatabaseHelper.SafeDecimal(r, col);
        protected static bool   B(SqlDataReader r, string col) => DatabaseHelper.SafeBool(r, col);
        protected static DateTime   DT(SqlDataReader r, string col) => DatabaseHelper.SafeDateTime(r, col);
        protected static DateTime?  DTN(SqlDataReader r, string col) => DatabaseHelper.SafeDateTimeNull(r, col);
        protected static int?       IN(SqlDataReader r, string col) => DatabaseHelper.SafeIntNull(r, col);
        protected static decimal?   DN(SqlDataReader r, string col) => DatabaseHelper.SafeDecimalNull(r, col);
    }
}
