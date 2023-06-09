using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using MySql;
using MySql.Data.MySqlClient;

public class EnrollmentsManager
{


    public void manage(MySqlConnection connection)
    {
        bool go = true;
        while (go)
        {
            string option = secondMenu("Enrollment");
            switch (option)
            {
                case "1":
                    Console.WriteLine("Add a Enrollment");
                    MySqlCommand cmd = new MySqlCommand("INSERT INTO enrollments(classId,studentId,professorId) VALUES(@v1,@v2,@v3);", connection);
                    int classId, studentId, professorId;
                    do
                    {
                        classId = Convert.ToInt32(read("Enter class id: "));
                    } while (!checkForId(classId, connection, "classes"));
                    cmd.Parameters.AddWithValue("@v1", classId);
                    do
                    {
                        studentId = Convert.ToInt32(read("Enter student id: "));
                    } while (!checkForId(studentId, connection, "students"));
                    cmd.Parameters.AddWithValue("@v2", studentId);
                    do
                    {
                        professorId = Convert.ToInt32(read("Enter professor id: "));
                    } while (!checkForId(professorId, connection, "professors"));
                    cmd.Parameters.AddWithValue("@v3", professorId);

                    int rows = cmd.ExecuteNonQuery();
                    if (rows > 0) Console.WriteLine("Added succesfully");
                    break;
                case "2":
                    cmd = new MySqlCommand("SELECT * FROM enrollments", connection);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (!reader.HasRows) Console.WriteLine("No Data");
                        else
                        {
                            Console.WriteLine("Enviroments: ");
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
                    cmd = new MySqlCommand("SELECT * FROM enrollments WHERE id = @id", connection);
                    int id;
                    do
                    {
                        id = Convert.ToInt32(read("Enter enrollment id: "));
                    } while (!checkForId(id, connection, "enrollments"));
                    cmd.Parameters.AddWithValue("@v1", id);

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
                    Console.WriteLine("Edit a Enviroment");
                    cmd = new MySqlCommand("UPDATE enrollments\r\nSET classId = @v1,\r\nstudentId = @v2,\r\nprofessorId = @v3\r\nWHERE id = @id;", connection);
                    do
                    {
                        id = Convert.ToInt32(read("Enter enrollment id: "));
                    } while (!checkForId(id, connection, "enrollments"));
                    cmd.Parameters.AddWithValue("@id", id);

                    do
                    {
                        classId = Convert.ToInt32(read("Enter class id: "));
                    } while (!checkForId(classId, connection, "classes"));
                    cmd.Parameters.AddWithValue("@v1", classId);
                    do
                    {
                        studentId = Convert.ToInt32(read("Enter student id: "));
                    } while (!checkForId(studentId, connection, "students"));
                    cmd.Parameters.AddWithValue("@v2", studentId);
                    do
                    {
                        professorId = Convert.ToInt32(read("Enter professor id: "));
                    } while (!checkForId(professorId, connection, "professors"));
                    cmd.Parameters.AddWithValue("@v3", professorId);

                    cmd .ExecuteNonQuery();
                    break;

                case "5":
                    try {
                        Console.WriteLine("Delete a Enviroment");
                        cmd = new MySqlCommand("DELETE FROM enrollments WHERE id = @id", connection);
                        do
                        {
                            id = Convert.ToInt32(read("Enter enrollment id: "));
                        } while (!checkForId(id, connection, "enrollments"));
                        cmd.Parameters.AddWithValue("@id", id);
                        cmd.ExecuteNonQuery();
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

    public bool checkForId(int id, MySqlConnection connection,string table)
    {
        MySqlCommand command = new MySqlCommand($"SELECT id FROM {table} WHERE id = @id", connection);
        command.Parameters.AddWithValue("@id", id);
        command.Parameters.AddWithValue("@table", table);
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

