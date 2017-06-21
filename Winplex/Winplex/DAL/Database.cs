using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite.Net;
using SQLite.Net.Platform.WinRT;
using Winplex.models;

namespace Winplex.DAL
{
    public class Database
    {
        private static Database Instance { get; set; }

        private SQLiteConnection Conn { get; set; }

        private Database()
        {
            var path = Path.Combine(Windows.Storage.ApplicationData.Current.LocalFolder.Path, "db.sqlite");
            Conn = new SQLiteConnection(new SQLitePlatformWinRT(), path);

            CreateTable();
        }

        public static Database GetDatabase()
        {
            return Instance ?? (Instance = new Database());
        }

        public void CreateTable()
        {
            Conn.CreateTable<Settings>();
        }

        public void AddOrUpdateSetting(string key, string value)
        {
            if (ShowSetting(key) == null)
            {
                AddSetting(key, value);
            }
            else
            {
                UpdateSetting(key, value);
            }
        }

        private void AddSetting(string key, string value)
        {
            try
            {
                var add = Conn.Insert(new Settings {Key = key, Value = value});
            }
            catch (SQLiteException e)
            {
                Debug.Fail(e.Message);
            }
        }

        private void UpdateSetting(string key, string value)
        {
            try
            {
                var add = Conn.Update(new Settings {Key = key, Value = value});
            }
            catch (SQLiteException e)
            {
                Debug.Fail(e.Message);
            }
        }

        public string ShowSetting(string key)
        {
            var query = Conn.Table<Settings>();

            var result = query.First(q => q.Key == key);

            return result?.Value;
        }
    }
}