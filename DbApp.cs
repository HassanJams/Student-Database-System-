using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace StudentDb
{
    internal class DbApp
    {
        private List<Student> students = new List<Student>();
        public DbApp()
        {
            // Some things that might be handled in the constructor

            //For Testing 
            StudentDbTester();
            // (INPUT) Read in the data from permanent storage (flat file)
            ReadStudentDataFromInputFile();

            // (PROCESS) Run the application
            RunDatabaseApp();

            
            // (OUTPUT) Write the data back out to permanent storage
            WriteDataToOutputFile();

        }


        public void ReadStudentDataFromInputFileCSV()
        {
            string inputFileName = "student.csv";

            try
            {
                var lines = File.ReadAllLines(inputFileName);

                foreach (var line in lines.Skip(1))
                {
                    var fields = line.Split(',');
                    string firstMidName = fields[0].Trim();
                    string lastName = fields[1].Trim();
                    double gradePtAvg;

                    if (double.TryParse(fields[2].Trim(), out gradePtAvg))
                    {
                        string emailAddress = fields[3].Trim();

                        // Create new Student object and add it to students list
                        Student student = new Student(firstMidName, lastName, gradePtAvg, emailAddress);
                        students.Add(student);
                    }
                    else
                    {
                        Console.WriteLine($"Invalid GPA value in line: {line}");
                    }
                }


            }

            catch (Exception ex){
                Console.WriteLine($"Error reading file: {ex.Message}");
            }
        }

        public void ReadStudentDataFromInputFile()
        {
            try
            {
                // read the JSON file as a string and deserialize it back
                // into Student sub-type objects and place them into
                // the students List
                string jsonFromFile = File.ReadAllText(Constants.StudentOutputJSONFile);

                // null coalesce
                students = JsonConvert.DeserializeObject<List<Student>>(jsonFromFile) ?? new List<Student>();

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error reading file: {ex.Message}");
            }
        }
        private Student? CheckIfEmailExists(string email)
        {
            
            var student = students.FirstOrDefault(student => student.EmailAddress == email);
            if (student == null)
            {

              Console.WriteLine($"{email} NOT FOUND.");
                return null;
            }
            else
            {
                // Found it!
                Console.WriteLine($"FOUND email address: {email}");
                return student;
            }
        }

        public void RunDatabaseApp()
        {
            while (true)
            {
                // First, display the main menu
                DisplayMainMenu();
                //Get the user selection 
                char selection = GetUserSelection();
                Console.WriteLine();
                //Switch for user selection 
                switch (char.ToLower(selection))
                {
                    case 'a':
                        //Add a record
                        AddStudentRecord(); 
                        break;

                    case 'f':
                        //Fine a record
                        FindStudentRecord();
                        break;
                    case 'm':
                        //Modify a Record
                        ModifyStudentRecord();
                        break;
                    case 'd':
                        //Delete a record
                        DeleteStudentRecord();
                        break;
                    case 'p':
                        //Print all records
                        PrintAllStudentRecords();
                        break;
                    case 'k':
                        //Print emails only
                        PrintAllStudentRecordKeys();
                        break;
                    case 's':
                        //Save and exit
                        SaveStudentDataAndExit();
                        break;
                    case 'e':
                        //Exit without saving
                        //TODO Add Confirmation of Exit
                        Environment.Exit(0);
                        break;
                    default:
                        Console.WriteLine($"ERROR: {selection} is not a valid input. Select again.");
                        break;

                }
            }

        }

        
        private void AddStudentRecord()
        {
            // First, search if user already exists
            Console.Write("ENTER email address to add: ");
            string email = Console.ReadLine();
            Student stu = CheckIfEmailExists(email);
            if (stu == null)
            {
                // Record was NOT FOUND -- go ahead and add
                Console.WriteLine($"Adding new student w/ Email: {email}");

                // Gather initial student data
                // Get first and middle name
                Console.Write("ENTER first and middle name: ");
                string first = Console.ReadLine();

                // Get last name
                Console.Write("ENTER last name: ");
                string last = Console.ReadLine();

                // Get GPA
                Console.Write("ENTER grade pt. average: ");
                double gpa = double.Parse(Console.ReadLine());

                // We already have the email, but need to ask if the student
                // is an undergrad or graduate student
                Console.Write(" [U]ndergrad or [G]raduate student? ");
                string studentType = Console.ReadLine();

                // Branch out to student type
                if (studentType.ToLower() == "u")
                {
                    Console.WriteLine("[1] Freshman, [2] Sophomore, [3] Junior, [4] Senior");
                    Console.Write("ENTER year/rank in school from above choices: ");
                    YearRank rank = (YearRank)int.Parse(Console.ReadLine());

                    Console.Write("ENTER major degree program: ");
                    string major = Console.ReadLine();

                    Undergrad undergrad = new Undergrad(
                        firstMidName: first,
                        lastName: last,
                        gradePtAvg: gpa,
                        emailAddress: email,
                        rank: rank,
                        degreeMajor: major
                    );

                    students.Add(undergrad);
                }
                // Graduate student
                if (studentType.ToLower() == "g")
                {
                    // Gather grad student info
                    Console.WriteLine("ENTER the tuition credit for this student (no commas): $");
                    decimal credit = decimal.Parse(Console.ReadLine());

                    Console.Write("ENTER full name of faculty advisor: ");
                    string advisor = Console.ReadLine();

                    Graduate graduate = new Graduate(
                        firstMidName: first,
                        lastName: last,
                        gradePtAvg: gpa,
                        emailAddress: email,
                        tuitionCredit: credit,
                        facultyAdvisor: advisor
                    );

                    students.Add(graduate);
                }

            }
            else
            {
                Console.WriteLine($"RECORD FOUND! Can't add student {email}!");
                Console.WriteLine("Record already exists!");
            }


        }

        private void FindStudentRecord()
        {
            // Ask user for email to search for
            Console.Write("Please enter the student email to search for: ");
            string email = Console.ReadLine();
            var student = CheckIfEmailExists(email);
            if (student != null)
            {
                Console.WriteLine();
                Console.WriteLine(student);
            }

        }

        private void ModifyStudentRecord()
        {
            // Ask for student email to modify
            Console.Write("Enter the email of the student to modify: ");
            string email = Console.ReadLine();

            // Find the student
            Student student = CheckIfEmailExists(email);

            if (student == null)
            {
                Console.WriteLine("Student not found.");
                return;
            }

            Console.WriteLine($"\nEditing record for: {student}");

            // Modify common properties
            Console.Write("Enter new first and middle name (leave blank to keep current): ");
            string first = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(first)) student.FirstMidName = first;

            Console.Write("Enter new last name (leave blank to keep current): ");
            string last = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(last)) student.LastName = last;

            Console.Write("Enter new GPA (leave blank to keep current): ");
            string gpaInput = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(gpaInput) && double.TryParse(gpaInput, out double gpa))
                student.GradePtAvg = gpa;

            // Modify based on student type
            if (student is Undergrad undergrad)
            {
                Console.Write("Enter new major (leave blank to keep current): ");
                string major = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(major)) undergrad.DegreeMajor = major;

                Console.WriteLine("[1] Freshman, [2] Sophomore, [3] Junior, [4] Senior");
                Console.Write("Enter new rank (leave blank to keep current): ");
                string rankInput = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(rankInput) && int.TryParse(rankInput, out int rank))
                    undergrad.Rank = (YearRank)rank;
            }
            else if (student is Graduate graduate)
            {
                Console.Write("Enter new tuition credit (leave blank to keep current): ");
                string creditInput = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(creditInput) && decimal.TryParse(creditInput, out decimal credit))
                    graduate.TuitionCredit = credit;

                Console.Write("Enter new faculty advisor (leave blank to keep current): ");
                string advisor = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(advisor)) graduate.FacultyAdvisor = advisor;
            }

            Console.WriteLine("\nStudent record updated successfully!");
        }

        private void DeleteStudentRecord()
        {
            // Ask for student email to delete
            Console.Write("Enter the email of the student to delete: ");
            string email = Console.ReadLine();

            // Find the student
            Student student = CheckIfEmailExists(email);

            if (student == null)
            {
                Console.WriteLine("Student not found.");
                return;
            }

            // Confirm deletion
            Console.Write($"Are you sure you want to delete {student.FirstMidName} {student.LastName}? (y/n): ");
            string confirmation = Console.ReadLine().ToLower();

            if (confirmation == "y")
            {
                students.Remove(student);
                Console.WriteLine("Student record deleted successfully.");
            }
            else
            {
                Console.WriteLine("Deletion canceled.");
            }

        }

        private void PrintAllStudentRecords()
        {
            Console.WriteLine("***** Printing all student records in file *****");
            Console.WriteLine();
            foreach (var student in students)
            {
                Console.WriteLine(student);
            }
            Console.WriteLine("***** Done printing all student records in file *****");
            Console.WriteLine();

        }

        private void PrintAllStudentRecordKeys()
        {
            Console.WriteLine("***** Printing all student email adresses in file *****");
            Console.WriteLine();
            foreach (var student in students)
            {
                Console.WriteLine(student.EmailAddress);
            }
            Console.WriteLine("***** Done printing all student email adresses in file *****");
            Console.WriteLine();

        }

        private void SaveStudentDataAndExit()
        {
            Console.WriteLine("Saving data and Exit. ");
            WriteDataToOutputFile();
            Environment.Exit(0);
        }

        /// <summary>
        /// Gets the key entered by user without having to hit enter
        /// </summary>
        /// <returns></returns>
        private char GetUserSelection()
        {
            ConsoleKeyInfo key = Console.ReadKey();
            return key.KeyChar;
        }



        private void DisplayMainMenu()
        {
            string menu = string.Empty;
            menu += "****************************************\n";
            menu += "******** Student Database App *********\n";
            menu += "****************************************\n";
            menu += "[A]dd a student record     (C in CRUD - Create)\n";
            menu += "[F]ind a student record    (R in CRUD - Read)\n";
            menu += "[M]odify a student record  (U in CRUD - Update)\n";
            menu += "[D]elete a student record  (D in CRUD - Delete)\n";
            menu += "[P]rint all records in current db storage\n";
            menu += "Print all primary [K]eys (email addresses)\n";
            menu += "[S]ave data to file and exit app\n";
            menu += "[E]xit app without saving changes\n";
            menu += "\n";
            menu += "User Key Selection: ";

            Console.Write(menu);
        }


        private void WriteDataToOutputFileText()
        {
            // Create an object that attaches to a file on disk
            StreamWriter outFile = new StreamWriter(Constants.StudentOutputTextFile);

            // For user to see
            Console.WriteLine("Outputting student data to the output file.");

            // Use the reference to the file above to write the file
            foreach (var student in students)
            {
                // Show each student to user for now
                Console.WriteLine(student);
                outFile.WriteLine(student);
            }

            // Close the resource
            outFile.Close();

        }


        private void WriteDataToOutputFileCSV()
        {
            // Create a list to hold the CSV lines
            var csvLines = new List<string>();

            // Add the header line
            csvLines.Add("FirstName,LastName,GPA,Email");

            foreach (var student in students)
            {
                string line = student.ToCSVFormat();
                csvLines.Add(line);
            }

            File.WriteAllLines(Constants.StudentOutputCSVFile, csvLines);
        }

        private void WriteDataToOutputFile()
        {
            // Creates a JSON styled string of all Student types in students List
            // and saves the file
            string json = JsonConvert.SerializeObject(students, Formatting.Indented);
            File.WriteAllText(Constants.StudentOutputJSONFile, json);
        }
        public void StudentDbTester()
        {
            Student stu1 = new Student();
            Student stu2 = new Student();
            Student stu3 = new Student();

            // Does not scale well -- should have a constructor
            stu1.FirstMidName = "Allison Amy";
            stu1.LastName = "Adams";
            stu1.GradePtAvg = 3.95;
            stu1.EmailAddress = "aaadams@uw.edu";

            Student stu4 = new Student(
                firstMidName: "John James",
                lastName: "Jones",
                gradePtAvg: 3.86,
                emailAddress: "jjjones@uw.edu"
            );

            stu2 = new Student(
                firstMidName: "Bob Bobo",
                lastName: "Bobbins",
                gradePtAvg: 2.17,
                emailAddress: "bbbobbins@uw.edu"
            );

            stu3 = new Student(
                firstMidName: "Mary Marie",
                lastName: "Masterson",
                gradePtAvg: 4.0,
                emailAddress: "mmmasterson@uw.edu"
            );
            Undergrad stu5 = new Undergrad(
             firstMidName: "Johnny John",
             lastName: "Johnson",
             gradePtAvg: 2.85,
             emailAddress: "jjjohnson@uw.edu",
             rank: YearRank.Junior,
             degreeMajor: "IT"
             );

            Graduate stu6 = new Graduate(
             firstMidName: "Tom Thomas",
             lastName: "Thompson",
             gradePtAvg: 3.75,
             emailAddress: "ttthopmson@uw.edu",
             tuitionCredit: 550.75m,
             facultyAdvisor: "Derek"
             );


            // Output can be done individually
            // Need a ToString() method
            //Console.WriteLine(stu1);
            //Console.WriteLine(stu2);
            //Console.WriteLine(stu3);
            //Console.WriteLine(stu4);

            //Adding 4 students to students instance list
            students.Add(stu1);
            students.Add(stu2);
            students.Add(stu3);
            students.Add(stu4);
            students.Add(stu5);
            students.Add(stu6);
        }
    }
}
