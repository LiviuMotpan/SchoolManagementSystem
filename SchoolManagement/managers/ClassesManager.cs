using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using MySql;
using MySql.Data.MySqlClient;

public class ClassesManager
{


    public void manage(MySqlConnection connection)
    {
        bool go = true;
        while (go)
        {
            string option = secondMenu("Class");
            switch (option)
            {
                case "1":
                    Console.WriteLine("Add a Class");

                    MySqlCommand cmd = new MySqlCommand("INSERT INTO classes(name,yearCreated) VALUES(@v1,@v2);", connection);

                    string name = read("Enter a name: ");
                    int yearCreated = Convert.ToInt32(read("Enter year created: "));

                    cmd.Parameters.AddWithValue("@v1", name);
                    cmd.Parameters.AddWithValue("@v2", yearCreated);

                    int rows = cmd.ExecuteNonQuery();
                    if (rows > 0) Console.WriteLine("Added succesfully");
                    break;
                case "2":
                    cmd = new MySqlCommand("SELECT * FROM classes", connection);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (!reader.HasRows) Console.WriteLine("No data");
                        else
                        {
                            Console.WriteLine("Classes: ");

                            while (reader.Read())
                            {
                                Console.WriteLine($"Class - {reader.GetInt32(0)}");
                                Console.WriteLine($"Name - {reader.GetString(1)}");
                                Console.WriteLine($"Year Created - {reader.GetString(2)}");
                            }
                        }
                    }

                    break;
                case "3":
                    cmd = new MySqlCommand("SELECT * FROM classes WHERE id = @id", connection);
                    int id;
                    do
                    {
                        id = Convert.ToInt32(read("Enter id: "));
                    } while (!checkForId(id, connection));
                    cmd.Parameters.AddWithValue("@id", id);


                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Console.WriteLine($"Class - {reader.GetInt32(0)}");
                            Console.WriteLine($"Name - {reader.GetString(1)}");
                            Console.WriteLine($"Year Created - {reader.GetString(2)}");
                        }
                    }

                    break;
                case "4":
                    Console.WriteLine("Edit a class");
                    cmd = new MySqlCommand("UPDATE classes\r\nSET name = @v1,\r\nyearCreated = @v2\r\nWHERE id = @id;", connection);

                    do
                    {
                        id = Convert.ToInt32(read("Enter id: "));
                    } while (!checkForId(id, connection));
                    cmd.Parameters.AddWithValue("@id", id);
                    name = read("Enter a name: ");
                    yearCreated = Convert.ToInt32(read("Enter year created: "));

                    cmd.Parameters.AddWithValue("@v1", name);
                    cmd.Parameters.AddWithValue("@v2", yearCreated);

                    rows = cmd.ExecuteNonQuery();
                    if (rows > 0) Console.WriteLine("Added succesfully");

                    break;

                case "5":
                    try { 
                        Console.WriteLine("Delete a class");
                        cmd = new MySqlCommand("DELETE FROM classes WHERE id = @id", connection);

                        do
                        {
                            id = Convert.ToInt32(read("Enter id: "));
                        } while (!checkForId(id, connection));

                        cmd.Parameters.AddWithValue("@id", id);
                        rows = cmd.ExecuteNonQuery();
                        if (rows > 0) Console.WriteLine("Row affected succesfull");
                    }catch(MySqlException ex)
                            {
                        Console.WriteLine("If you want to delete a student you need to remove it from enrollments");
                    }
                    break;
                case "6":
                    go = false;
                    break;
                default:
                    Console.WriteLine("Enter a showed option");
                    break;

            }
            Console.WriteLine("Press enter to continue");
            string c = Console.ReadLine();
            Console.Clear();

        }
    }

    public static string read(string txt)
    {
        Console.Write(txt);
        return Console.ReadLine();
    }

    public static string read()
    {
        return Console.ReadLine();
    }
    public static string secondMenu(string option)
    {
        Console.WriteLine($"1. Add {option}");
        Console.WriteLine($"2. View all {option}");
        Console.WriteLine($"3. View {option} by id");
        Console.WriteLine($"4. Edit {option}");
        Console.WriteLine($"5. Delete {option}");
        Console.WriteLine("6. Back <-");
        return read("Choose an option : ");
    }

    public bool checkForId(int id, MySqlConnection connection)
    {
        MySqlCommand command = new MySqlCommand("SELECT id FROM classes WHERE id = @id", connection);
        command.Parameters.AddWithValue("@id", id);
        using (MySqlDataReader reader = command.ExecuteReader())
        {
            while (reader.Read())
            {
                return true;
            }
        }
        return false;
    }

}

