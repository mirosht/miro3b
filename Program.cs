using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRUDusers
{
    class Program
    {
        static OleDbConnection dbConnection;
        static void Main(string[] args)
        {
                dbConnection = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\Users\Abadjiev\Documents\Visual Studio 2012\Projects\CRUDusers\CRUDusers\data\users.accdb");
                Console.Write("Potrebitel : ");
                string username = Console.ReadLine();
                Console.Write("Parola :");
                string password = Console.ReadLine();

                if (login(username, password))
                {
                    Read();
                    while (true)
                    {
                        Console.WriteLine("1-Insert user, 2-Update user,3-Delete user,0-Exit program");
                        int action = int.Parse(Console.ReadLine());
  
                        switch (action)
                        {

                            case 1:
                               
                                try
                                {
                                    Console.Write("Vavedete potrebitel ");
                                    string usernameAdd = Console.ReadLine();
                                    Console.Write("Vavedete  parola: ");
                                    string passwordAdd = Console.ReadLine();
                                    Console.Write(" email: ");
                                    string emailAdd = Console.ReadLine();
                                    Console.WriteLine();
                                    AddUser(usernameAdd, passwordAdd, emailAdd);
                                    
                                }
                                catch (OleDbException e)
                                {
                                    Console.WriteLine(e.Message);
                                }
                                break;

                            case 2:
                                Console.Write("Who to update ?? ");
                                try
                                {
                                    int idUpdate = int.Parse(Console.ReadLine());
                                    Console.Write("Nov username: ");
                                    string usernameUpdate = Console.ReadLine();
                                    Console.Write("Nov password: ");
                                    string passwordUpdate = Console.ReadLine();
                                    Console.Write("Nov email: ");
                                    string emailUpdate = Console.ReadLine();
                                    Console.WriteLine();
                                    UpdateUser(idUpdate, usernameUpdate, passwordUpdate, emailUpdate);
                                    
                                }
                                catch (OleDbException e)
                                {
                                    Console.WriteLine(e.Message);
                                }
                                break;

                            case 3:
                                Console.WriteLine("Kogo da iztriq? ");
                                try
                                {
                                    int delLine = int.Parse(Console.ReadLine());
                                    Console.WriteLine();
                                    Delete(delLine);                                   
                                }
                                catch (Exception e)
                                {
                                    Console.WriteLine(e.Message);
                                }
                                break;
                            case 0:
                                Environment.Exit(0);
                                break;
                        }

                    }
                    
                }
                else {
                    Console.WriteLine("Greshen potrebitel ili parola");
                }
                Console.ReadKey(true);
            
        }
        public static bool login(string username, string password) {
            OleDbCommand User = new OleDbCommand("SELECT COUNT(*) FROM users WHERE username=@potrebitel AND password=@parola");
            User.Parameters.AddRange(new[] { new OleDbParameter("@potrebitel", username) });
            User.Parameters.AddRange(new[] { new OleDbParameter("@parola", password) });
            dbConnection.Open();

            User.Connection = dbConnection;
            try
            {
                int numberOfRows = (int)User.ExecuteScalar();
                if (numberOfRows == 1)
                {
                    dbConnection.Close();
                    return true;
                }
                else
                {
                    dbConnection.Close();
                    return false;
                }
            }
            catch (Exception ex) {
                Console.WriteLine(ex.Message);
            }
            dbConnection.Close();
            return false;
        }
        public static void AddUser(string username, string pass, string email)
        {
           try
            {
                dbConnection.Open();
                OleDbCommand aCommand = new OleDbCommand("INSERT INTO users (username, [password], email) VALUES (@potrebitel,@parola,@email)", dbConnection);
                aCommand.Parameters.AddRange(new[] { new OleDbParameter("@potrebitel", username) });
                aCommand.Parameters.AddRange(new[] { new OleDbParameter("@parola", pass) });
                aCommand.Parameters.AddRange(new[] { new OleDbParameter("@email", email) });
                int affectedRows = aCommand.ExecuteNonQuery();
                dbConnection.Close();
                Console.WriteLine("Number of rows affected {0} from INSERT", affectedRows);
                Read();
                Console.WriteLine("User {0} was successfully inserted!",username);
            }
           catch (OleDbException e)
            {
                Console.WriteLine(e.Errors[0].Message);
            }
        }

        public static void UpdateUser(int ID, string username, string password, string email)
        {
            try
            {
                dbConnection.Open();

                OleDbCommand aCommand = new OleDbCommand("UPDATE users SET username = @potrebitel, password = @parola, email = @email WHERE ID = @user_id", dbConnection);               
                aCommand.Parameters.AddRange(new[] { new OleDbParameter("@potrebitel", username) });
                aCommand.Parameters.AddRange(new[] { new OleDbParameter("@parola", password) });
                aCommand.Parameters.AddRange(new[] { new OleDbParameter("@email", email) });
                aCommand.Parameters.AddRange(new[] { new OleDbParameter("@user_id", ID) });
                int rowsAffected = aCommand.ExecuteNonQuery();
                dbConnection.Close();
                Console.WriteLine("Number of rows affected {0} from UPDATE", rowsAffected);
                Read();
                Console.WriteLine("Number {0} was successfully updaed!",ID);
            }
            catch (OleDbException e)
            {
                Console.WriteLine(e.Errors[0].Message);

            }
        }

        public static void Delete(int user_id)
        {
            try
            {
                dbConnection.Open();
                OleDbCommand aCommand = new OleDbCommand("DELETE FROM users WHERE ID = @user_id", dbConnection);
                aCommand.Parameters.AddRange(new[] { new OleDbParameter("@user_id", user_id) });
                int numberOfRows = aCommand.ExecuteNonQuery();
                dbConnection.Close();
                Console.WriteLine("{0} row has been DELETED", numberOfRows);
                Read();
                Console.WriteLine("user with ID - {0} was successfully DELETED", user_id);

            }
            catch (OleDbException e)
            {
                Console.WriteLine(e.Errors[0].Message);

            }

        }
        public static void Read()
        {
            try
            {
                dbConnection.Open();
                OleDbCommand iCommand = new OleDbCommand("SELECT * from users", dbConnection);
                OleDbDataReader iReader = iCommand.ExecuteReader();
                Console.WriteLine("All users :");
                while (iReader.Read())
                {
                    Console.WriteLine("{0} \t {1}", iReader.GetInt32(0).ToString(), iReader.GetString(1));
                }
                iReader.Close();
                dbConnection.Close();
            }
            catch (OleDbException e)
            {
                Console.WriteLine(e.Errors[0].Message);
            }
        }
    }
}
