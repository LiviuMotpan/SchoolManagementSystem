
using MySql.Data.MySqlClient;

class Program
{
    public static MySqlConnection connection = new MySqlConnection("server=localhost;port=4000;database=school_management_system;username=root;password=;");

    public static ProfessorManager professorManager = new ProfessorManager();
    public static StudentsManager studentsManager = new StudentsManager();
    public static ClassesManager classesManager = new ClassesManager();
    public static EnrollmentsManager enrollmentsManager = new EnrollmentsManager();

    public static void Main(string[] args)
    {
        try
        {
            connection.Open();
            bool go = true;

            Console.WriteLine("######################--- Welcome to C# School Management System ---######################");
            while (go)
            {
                string option = mainMenu();
                switch (option)
                {
                    case "1":
                        professorManager.manage(connection);
                        break;
                    case "2":
                        studentsManager.manage(connection);
                        break;
                    case "3":
                        classesManager.manage(connection);
                        break;
                    case "4":
                        enrollmentsManager.manage(connection);
                        break;
                    default:
                        Console.WriteLine("Choose a available option");
                        break;
                }

                Console.Clear();
            }
        }catch(Exception ex)
        {
            Console.WriteLine(ex.ToString());
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

    public static string mainMenu()
    {
        Console.WriteLine("1. Manage Professors");
        Console.WriteLine("2. Manage Students");
        Console.WriteLine("3. Manage Classes");
        Console.WriteLine("4. Manage Enrollments");
        Console.WriteLine("5. Exit");
        return read("Choose an option : ");
    }

    
}