using System.Configuration;
using System.Data.SQLite;

namespace DataLayer.Repository
{
    public class SqLiteBaseRepository
    {
        public static string DbFile
        {
            get { return ConfigurationManager.AppSettings["SqlLitePath"]; }
        }

        public static SQLiteConnection SimpleDbConnection()
        {
            return new SQLiteConnection("Data Source=" + DbFile);
        }
    }
}