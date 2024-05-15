using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SnakeQuest.model;

namespace SnakeQuest
{
    public class Database()
    {

        private string sqlpath = AppDomain.CurrentDomain.BaseDirectory + "GameData.sqlite3";

        // 连接数据库
        public SQLiteConnection ConnectSQL()
        {
            // 判断是否存在该数据库
            if (!System.IO.File.Exists(sqlpath))
            {
                SQLiteConnection.CreateFile(sqlpath);
            }
            SQLiteConnection connection = new SQLiteConnection($"Data Source={sqlpath}");
            return connection;
        }

        // 建立表
        public void CreatTable(SQLiteConnection connection)
        {
            connection.Open();
            SQLiteCommand command = connection.CreateCommand();
            command.Connection = connection;
            command.CommandText = "CREATE TABLE IF NOT EXISTS Users(" +
                "Id UUID PRIMARY KEY," +
                "Name STRING," +
                "Score INT," +
                "Time STRING); ";
            command.ExecuteNonQuery();
            connection.Close();
        }

        // 添加数据
        public void AddData(SQLiteConnection connection, string name, int score, string time)
        {
            connection.Open();
            SQLiteCommand command = connection.CreateCommand();
            command.CommandText = "INSERT INTO Users(Name, Score, Time) VALUES(@Name, @Score, @Time)";
            command.Parameters.Add("Name", DbType.String).Value = name;
            command.Parameters.Add("Score", DbType.Int32).Value = score;
            command.Parameters.Add("Time", DbType.String).Value = time;
            command.ExecuteNonQuery();
            connection.Close();
        }
        
        // 查询数据
        public List<DataItem> ReadData(SQLiteConnection connection)
        {
            connection.Open();
            SQLiteCommand command = connection.CreateCommand();
            command.CommandText = "SELECT * FROM Users";
            SQLiteDataReader reader = command.ExecuteReader();

            // 将从数据库中取得内容进行转换
            List<DataItem> dataList = new List<DataItem>();
            while (reader.Read())
            {
                DataItem item = new DataItem(); 
                item.Name = reader.GetString("Name");
                item.Score = reader.GetInt32("Score");
                item.Time = reader.GetDateTime("Time");
                dataList.Add(item);
                
            }
            connection.Close();
            return dataList;
        }
    }
    
}
