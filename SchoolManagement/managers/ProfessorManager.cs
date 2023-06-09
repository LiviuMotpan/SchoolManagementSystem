using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using MySql;
using MySql.Data.MySqlClient;

public class ProfessorManager
{


    public void manage(MySqlConnection connection)
    {
        bool go = true;
        while(go)
        {
            string option = secondMenu("Professor");
            switch (option)
            {
                case "1":
                    Console.WriteLine("Add a professor");
                    MySqlCommand cmd = new MySqlCommand("INSERT INTO professors(firstName,lastName,email,birthdate,address,phoneNr,subjectTaught) VALUES(@v1,@v2,@v3,@v4,@v5,@v6,@v7);", connection);

                    string firstName = read("Enter first name: ");
                    string lastName = read("Enter last name: ");

                    Regex emailR = new Regex(@"^[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,}$");
                    string email;
                    do
                    {
                        email = read("Enter email: ");
                    } while (!emailR.IsMatch(email));

                    Regex dateR = new Regex(@"^(0?[1-9]|1[0-2])/(0?[1-9]|[12][0-9]|3[01])/(19|20)\d{2}$");
                    string birthdate;
                    do
                    {
                        birthdate = read("Enter birthdate(MM/DD/YYYY): ");
                    } while (!dateR.IsMatch(birthdate));
                    string address = read("Enter address: ");
                    string phoneNr = read("Enter a phone number: ");
                    string subjectTaught = read("Enter subjectTaught: ");

                    cmd.Parameters.AddWithValue("@v1", firstName);
                    cmd.Parameters.AddWithValue("@v2", lastName);
                    cmd.Parameters.AddWithValue("@v3", email);
                    cmd.Parameters.AddWithValue("@v4", birthdate);
                    cmd.Parameters.AddWithValue("@v5", address);
                    cmd.Parameters.AddWithValue("@v6", phoneNr);
                    cmd.Parameters.AddWithValue("@v7", subjectTaught);

                    int rows = cmd.ExecuteNonQuery();
                    if (rows > 0) Console.WriteLine("Added succesfully");
                    break;
                case "2":
                    cmd = new MySqlCommand("SELECT * FROM professors", connection);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (!reader.HasRows) Console.WriteLine("No data");
                        else
                        {
                            while (reader.Read())
                            {
                                Console.WriteLine($"Professor - {reader.GetInt32(0)}");
                                Console.WriteLine($"First Name - {reader.GetString(1)}");
                                Console.WriteLine($"Last Name - {reader.GetString(2)}");
                                Console.WriteLine($"Birhtdate - {reader.GetString(3)}");
                                Console.WriteLine($"Address - {reader.GetString(4)}");
                                Console.WriteLine($"Phone Number - {reader.GetString(5)}");
                                Console.WriteLine($"Email - {reader.GetString(6)}");
                                Console.WriteLine($"Subject Taught - {reader.GetString(7)}\n\n");
                            }
                        }
                    }

                    break;
                case "3":
                    cmd = new MySqlCommand("SELECT * FROM professors WHERE id = @id", connection);
                    int id;
                    do
                    {
                        id = Convert.ToInt32(read("Enter id: "));
                    } while (!checkForId(id,connection));
                    cmd.Parameters.AddWithValue("@id", id);


                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Console.WriteLine($"Professor - {reader.GetInt32(0)}");
                            Console.WriteLine($"First Name - {reader.GetString(1)}");
                            Console.WriteLine($"Last Name - {reader.GetString(2)}");
                            Console.WriteLine($"Birhtdate - {reader.GetString(3)}");
                            Console.WriteLine($"Address - {reader.GetString(4)}");
                            Console.WriteLine($"Phone Number - {reader.GetString(5)}");
                            Console.WriteLine($"Email - {reader.GetString(6)}");
                            Console.WriteLine($"Subject Taught - {reader.GetString(7)}\n\n");
                        }
                    }

                    break;
                case "4":
                    Console.WriteLine("Edit a proffesor");
                    cmd = new MySqlCommand("UPDATE professors\r\nSET firstName = @v1,\r\nlastName = @v2,\r\nemail = @v3,\r\nbirthdate = @v4,\r\naddress = @v5,\r\nphoneNr = @v6,\r\nsubjectTaught = @v7\r\nWHERE id = @id;", connection);
                    do
                    {
                        id = Convert.ToInt32(read("Enter id: "));
                    } while (!checkForId(id,connection));

                    firstName = read("Enter first name: ");
                    lastName = read("Enter last name: ");
                    
                    emailR = new Regex(@"^[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,}$");
                    do
                    {
                        email = read("Enter email: ");
                    } while (!emailR.IsMatch(email));

                    dateR = new Regex(@"^(0?[1-9]|1[0-2])/(0?[1-9]|[12][0-9]|3[01])/(19|20)\d{2}$");
                    do
                    {
                        birthdate = read("Enter birthdate(MM/DD/YYYY): ");
                    } while (!dateR.IsMatch(birthdate));
                    address = read("Enter address: ");
                    phoneNr = read("Enter a phone number: ");
                    subjectTaught = read("Enter subjectTaught: ");

                    cmd.Parameters.AddWithValue("@v1", firstName);
                    cmd.Parameters.AddWithValue("@v2", lastName);
                    cmd.Parameters.AddWithValue("@v3", email);
                    cmd.Parameters.AddWithValue("@v4", birthdate);
                    cmd.Parameters.AddWithValue("@v5", address);
                    cmd.Parameters.AddWithValue("@v6", phoneNr);
                    cmd.Parameters.AddWithValue("@v7", subjectTaught);
                    cmd.Parameters.AddWithValue("@id", id);

                    rows = cmd.ExecuteNonQuery();
                    if (rows > 0) Console.WriteLine("Added succesfully");

                    break;

                case "5":
                    try { 
                        Console.WriteLine("Remove a professor");

                        cmd = new MySqlCommand("DELETE FROM professors WHERE id = @id", connection);

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

    public bool checkForId(int id,MySqlConnection connection)
    {
        MySqlCommand command = new MySqlCommand("SELECT id FROM professors WHERE id = @id",connection);
        command.Parameters.AddWithValue("@id", id);
        using(MySqlDataReader reader = command.ExecuteReader())
        {
            while(reader.Read())
            {
                return true;
            }
        }
        return false;
    }

}

