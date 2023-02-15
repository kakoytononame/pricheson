using Microsoft.Data.SqlClient;

namespace pricheson.Models
{
    public class Applicationdb
    {
        string connection = @"Server=(localdb)\mssqllocaldb;Database=prichesonbd;Trusted_Connection=True;";
        private SqlConnection sqlConnection;
        public Applicationdb()
        {
            sqlConnection = new SqlConnection(connection);
        }
        
        private void ConnectionOpen()
        {
            
            if (sqlConnection.State == System.Data.ConnectionState.Closed)
            {
                sqlConnection.Open();
            }
            
        }
        private void ConnectionClose()
        {
            if (sqlConnection.State == System.Data.ConnectionState.Open)
            {
                sqlConnection.Close();
            }
        }
        public List<Timetable> GetTable()
        {    
            string sqlcommand = "SELECT * FROM timetable";   
            
            return propertyinicialization(sqlcommand); 
        }
        public List<Timetable> MastersNaameFilter(string Name)
        {
            string sqlcommand = $"SELECT * FROM timetable WHERE mastername LIKE N'{Name}'";

            return propertyinicialization(sqlcommand); 
        }
        public List<Timetable> DateFilter(string Date)
        {
            string sqlcommand = $"SELECT * FROM timetable WHERE date = '{Date}'";

            return propertyinicialization(sqlcommand);
        }
        public List<Timetable> DateandMaasterFilter(string Name, string Date)
        {
            string sqlcommand = $"SELECT * FROM timetable WHERE mastername LIKE N'{Name}' AND date = '{Date}'";
            return propertyinicialization(sqlcommand);
        }
        public List<DateandTime> DateandTimeGetFromTable(string[] Week,string MasterName)
        {
            string beginweek = Convert.ToDateTime(Week[0]).ToString("yyyy - MM - dd");
            string endweek = Convert.ToDateTime(Week[1]).ToString("yyyy - MM - dd");
            string sqlcommand = $"SELECT * FROM timetable WHERE(date BETWEEN '{beginweek}' AND '{endweek}') AND mastername LIKE N'{MasterName}' ORDER BY date,time";
            ConnectionOpen();
            List<DateandTime> weekbase= new List<DateandTime>();
            SqlCommand sqlCommand = new SqlCommand(sqlcommand, sqlConnection);
            SqlDataReader reader = sqlCommand.ExecuteReader();
            while (reader.Read())
            {
                weekbase.Add(new DateandTime()
                {
                    Date = Convert.ToDateTime(reader.GetValue(2)).ToString("yyyy-MM-dd"),
                    Time = Convert.ToString(reader.GetValue(3)),
                    Servicedelation=Convert.ToString(reader.GetValue(5)),
                });

            }
            reader.Close();
            ConnectionClose();
            return weekbase;
        }

        private List<Timetable> propertyinicialization(string sqlcommand)
        {
            ConnectionOpen();
            List<Timetable> list = new List<Timetable>();
            SqlCommand sqlCommand = new SqlCommand(sqlcommand, sqlConnection);
            SqlDataReader reader = sqlCommand.ExecuteReader();
            while (reader.Read())
            {
                list.Add(new Timetable()
                {
                    Id = Convert.ToInt32(reader.GetValue(0)),
                    Mastername = Convert.ToString(reader.GetValue(1)),
                    Date = Convert.ToDateTime(reader.GetValue(2)).ToString("yyyy-MM-dd"),
                    Time = Convert.ToString(reader.GetValue(3)),
                    Service = Convert.ToString(reader.GetValue(4)),
                    Servicedelation = Convert.ToString(reader.GetValue(5)),
                    Username = Convert.ToString(reader.GetValue(6)),
                    Userphone = Convert.ToString(reader.GetValue(7)),

                });

            }
            reader.Close();
            ConnectionClose();
            return list;
        }
        
        public void dbUpdate(string[]massiv)
        {
            Timetable timetable=new Timetable()
            {
                Id= Convert.ToInt32(massiv[0]),
                Mastername = massiv[1],
                Date = massiv[2],
                Time = massiv[3],
                Service = massiv[4],
                Servicedelation= massiv[5],
                Username = massiv[6],
                Userphone = massiv[7],

            };

            ConnectionOpen();
            string sqlcommand = $"UPDATE timetable SET mastername=N'{timetable.Mastername}'," +
                $"date='{timetable.Date}',time='{timetable.Time}',service=N'{timetable.Service}',servicedelation='{timetable.Servicedelation}'," +
                $"username=N'{timetable.Username}',userphone=N'{timetable.Userphone}' WHERE ID={timetable.Id}";
            SqlCommand sqlCommand = new SqlCommand(sqlcommand, sqlConnection);
            sqlCommand.ExecuteNonQuery();
            ConnectionClose();
        }
        public void dbADD(string[] massiv)
        {
            Timetable timetable = new Timetable()
            {
                Id = Convert.ToInt32(massiv[0]),
                Mastername = massiv[1],
                Date = massiv[2],
                Time = massiv[3],
                Service = massiv[4],
                Servicedelation = massiv[5],
                Username = massiv[6],
                Userphone = massiv[7],

            };

            ConnectionOpen();
            string sqlcommand = $"INSERT INTO timetable (id,mastername,date,time,service,servicedelation,username,userphone) VALUES ('{timetable.Id}',N'{timetable.Mastername}','{timetable.Date}'" +
                $",'{timetable.Time}',N'{timetable.Service}','{timetable.Servicedelation}',N'{timetable.Username}',N'{timetable.Userphone}')";
            SqlCommand sqlCommand = new SqlCommand(sqlcommand, sqlConnection);
            sqlCommand.ExecuteNonQuery();
            ConnectionClose();
        }
        public void dbzapisADD(string[] massiv)
        {   
            ConnectionOpen();
            string sqlcommand1 = "SELECT MAX(id) FROM timetable";
            SqlCommand sqlCommand1 = new SqlCommand(sqlcommand1, sqlConnection);
            int buferid = 0;
            SqlDataReader reader = sqlCommand1.ExecuteReader();
            while (reader.Read())
            {
                buferid = Convert.ToInt32(reader.GetValue(0)) + 1;
            }
            reader.Close();
            ConnectionClose();
            Timetable timetable = new Timetable()
            {
                Id=buferid,
                Mastername = massiv[0],
                Date = massiv[1],
                Time = massiv[2],
                Service = massiv[3],
                Servicedelation = massiv[4],
                Username = massiv[5],
                Userphone = massiv[6],

            };
            ConnectionOpen();
            sqlCommand1.ExecuteNonQuery();
            string sqlcommand2 = $"INSERT INTO timetable (id,mastername,date,time,service,servicedelation,username,userphone) VALUES ({timetable.Id},N'{timetable.Mastername}','{timetable.Date}'" +
                $",'{timetable.Time}',N'{timetable.Service}','{timetable.Servicedelation}',N'{timetable.Username}',N'{timetable.Userphone}')";
            SqlCommand sqlCommand2 = new SqlCommand(sqlcommand2, sqlConnection);
            sqlCommand2.ExecuteNonQuery();
            ConnectionClose();
        }
        public void dbDelete(string massiv)
        {
            int id=Convert.ToInt32(massiv);
            ConnectionOpen();
            string sqlcommand = $"DELETE FROM timetable WHERE id = '{id}'";
            SqlCommand sqlCommand = new SqlCommand(sqlcommand, sqlConnection);
            sqlCommand.ExecuteNonQuery();
            ConnectionClose();
        }
    }
}
