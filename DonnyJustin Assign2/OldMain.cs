///////////////////////////////////////
/// Donny Kapic z1855273
/// Justin Roesner z1858242
/// CSCI 473 .NET programming
/// Assign 1
///////////////////////////////////////
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using static DonnyJustin_Assign2.Student;

namespace DonnyJustin_Assign2
{
    class OldMain
    {
        static void Mains(string[] args)
        {
            // List of Course objects
            List<Course> coursePool = new List<Course>();

            // Key: ZID    Value: Student object
            IDictionary<uint, Student> studentPool = new Dictionary<uint, Student>();

            // read student input file
            string[] studentLines = File.ReadAllLines("../../input_01.txt");

            // read coures input file
            string[] courseLines = File.ReadAllLines("../../input_02.txt");

            // add courses to coursePool
            for (int i = 0; i < courseLines.Length; i++)
            {
                Course course = new Course(courseLines[i]);
                coursePool.Add(course);
            }

            // print list of courses
            foreach (Course c in coursePool)
                Console.WriteLine(c.ToString());



            // add values to dictionary
            for (int i = 0; i < studentLines.Length; i++)
            {
                string[] tokens = studentLines[i].Split(',');
                uint zid = Convert.ToUInt32(tokens[0]);
                Student student = new Student(studentLines[i]);
                studentPool.Add(zid, student);
            }

            // Sort Dictionary by ZID and store in new Dictionary
            System.Collections.Generic.SortedDictionary<uint, Student> sortedPool = null;
            if (studentPool != null)
                sortedPool = new SortedDictionary<uint, Student>(studentPool);
                       bool runFlag = true;

            while (runFlag)
            {
                //print list
                Console.WriteLine("");
                Console.WriteLine("Please choose from the following options:");
                Console.WriteLine("");
                Console.WriteLine("1.Print Student List(All)");
                Console.WriteLine("2.Print Student List(Major)");
                Console.WriteLine("3.Print Student List(Academic Year)");
                Console.WriteLine("4.Print Course List");
                Console.WriteLine("5.Print Course Roster");
                Console.WriteLine("6.Enroll Student");
                Console.WriteLine("7.Drop Student");
                Console.WriteLine("8.Quit");
                Console.WriteLine("");

                //get input
                string userInput = Console.ReadLine();
                switch (userInput)
                {
                    case "1": //Print student list (all)
                        Console.WriteLine("Student list:");
                        foreach (KeyValuePair<uint, Student> s in sortedPool)
                        {
                            Console.WriteLine(s.ToString());
                        }
                        break;
                    case "2": //Print student list (Major)
                        Console.WriteLine("Enter a Major ");
                        string userInputMajor = Console.ReadLine();
                        foreach (KeyValuePair<uint, Student> s in sortedPool)
                        {
                            if (s.Value.getMajor() == userInputMajor)
                            {
                                Console.WriteLine(s.ToString());
                            }
                        }
                        break;
                    case "3": //Print student list (academic year)
                        Console.WriteLine("Enter an academic year (Freshman, Sophomore, Junior, Senior, PostBacc)");
                        string userInputYear= Console.ReadLine();
                        foreach (KeyValuePair<uint, Student> s in sortedPool)
                        {
                            Year tempEnum = (Year)Enum.Parse(typeof(Year), userInputYear);
                            if (s.Value.GetYear().Equals(tempEnum))
                            {
                                Console.WriteLine(s.ToString());
                            }
                        }
                        break;
                    case "4": //Print course list
                        Console.WriteLine("Course list:");
                        foreach (Course c in coursePool)
                        {
                            Console.WriteLine(c.ToString());
                        }
                        break;
                    case "5": //Print course roster
                        Console.WriteLine("Enter a course like CSCI 240-0001");
                        string userInputClass = Console.ReadLine();
                        string[] words = userInputClass.Split(' ' , '-'); //words[0] holds department, words[1] holds course, words[2] holds section
                        foreach (Course c in coursePool)
                        {
                            if (string.Equals(words[0], c.GetDepartmentCode()) && c.GetCourseNumber() == Convert.ToInt32(words[1], 10) && string.Equals(words[2], c.GetSectionNumber()))
                            {
                                Console.WriteLine(c.ToString());
                                Console.WriteLine("--------------------------------------------------------");

                                //print it's array of students
                                uint[] tempArray = c.GetStudentsEnrolled();
                                foreach(KeyValuePair<uint, Student> s in studentPool) 
                                {
                                    for (int i = 0; i < tempArray.Length; i++)
                                    {
                                        if (s.Key == tempArray[i])
                                        {
                                            s.Value.ToString();
                                            Console.WriteLine("z" + s.Value.getZid() + " " + s.Value.getLastName() + " " + s.Value.getFirstName() + " " + s.Value.getMajor());
                                        }
                                   }
                                }
                            }
                        }
                        break;
                    case "6":
                        { //Enroll student
                            Console.WriteLine("Please enter student ZID and course name.");
                            Console.WriteLine("'ZID' 'Dept' 'Course Number' 'Section Number'");
                            string userEnrollStudent = Console.ReadLine();

                            // parse user input. [0] = ZID   [1] = Department    [2] = Course    [3] = Section
                            string[] tokens = userEnrollStudent.Split();

                            // check if student exists
                            bool studentExists = false;
                            uint id = Convert.ToUInt32(tokens[0]);
                            foreach (KeyValuePair<uint, Student> kvp in studentPool)
                            {
                                if (id != kvp.Key)
                                {
                                    studentExists = false;
                                }
                                else
                                {
                                    studentExists = true;
                                    break;
                                }
                            }

                            // check if class exists
                            bool classExists = false;
                            uint courseNum = Convert.ToUInt32(tokens[2]);
                            foreach (Course c in coursePool)
                            {
                                if (tokens[1] == c.GetDepartmentCode() && courseNum == c.GetCourseNumber() && tokens[3] == c.GetSectionNumber())
                                {
                                    classExists = true;
                                }
                                else
                                    classExists = false;
                            }
                            // check if enrollement isn't at max
                            bool classFull = true;
                            foreach (Course c in coursePool)
                            {
                                // convert course number to string for comparison
                                string tempNum = Convert.ToString(c.GetCourseNumber());

                                // check for enrollment availability for that specific class
                                if (string.Equals(tokens[1], c.GetDepartmentCode()) && tokens[2] == tempNum && tokens[3] == c.GetSectionNumber())
                                {
                                    if (c.GetTotalCurrentlyEnrolled() > c.GetMaxCapacity())
                                    {
                                        classFull = true;
                                    }
                                    else
                                    {
                                        classFull = false;
                                    }

                                    //if ((studentExists == true) && (classExists == true) && (classFull == false))
                                    if (classFull == false)
                                    {
                                        uint zid = Convert.ToUInt32(tokens[0]);
                                        c.addStudent(zid);
                                        foreach (KeyValuePair<uint, Student> s in studentPool)
                                        {
                                            if (s.Key == zid && s.Value.getCreditHours() <= 18)
                                            {
                                                // add credit hours to student
                                                s.Value.setCreditHours(c.GetTotalCurrentlyEnrolled());
                                            }
                                        }
                                        break;
                                    }
                                }
                            }
                            break;
                        }
                    case "7": //Drop student
                        Console.WriteLine("Please enter student ZID and course name.");
                        Console.WriteLine("'ZID' 'Dept' 'Course Number' 'Section Number'");
                        string _userEnrollStudent = Console.ReadLine();

                        // parse user input. [0] = ZID   [1] = Department    [2] = Course    [3] = Section
                        string[] _tokens = _userEnrollStudent.Split();

                        // check if student exists
                        bool _studentExists = false;
                        uint _id = Convert.ToUInt32(_tokens[0]);
                        foreach (KeyValuePair<uint, Student> kvp in studentPool)
                        {
                            if (_id != kvp.Key)
                            {
                                _studentExists = false;
                            }
                            else
                            {
                                _studentExists = true;
                                break;
                            }
                        }

                        // check if class exists
                        bool _classExists = false;
                        uint _courseNum = Convert.ToUInt32(_tokens[2]);
                        foreach (Course c in coursePool)
                        {
                            if (_tokens[1] == c.GetDepartmentCode() && _courseNum == c.GetCourseNumber() && _tokens[3] == c.GetSectionNumber())
                            {
                                _classExists = true;
                            }
                            else
                                _classExists = false;
                        }
                        // check if enrollement isn't at max
                        bool _classFull = true;
                        foreach (Course c in coursePool)
                        {
                            // convert course number to string for comparison
                            string tempNum = Convert.ToString(c.GetCourseNumber());

                            // check for enrollment availability for that specific class
                            if (string.Equals(_tokens[1], c.GetDepartmentCode()) && _tokens[2] == tempNum && _tokens[3] == c.GetSectionNumber())
                            {
                                if (c.GetTotalCurrentlyEnrolled() > c.GetMaxCapacity())
                                {
                                    _classFull = true;
                                }
                                else
                                {
                                    _classFull = false;
                                }

                                //if ((studentExists == true) && (classExists == true) && (classFull == false))
                                if (_classFull == false)
                                {
                                    uint zid = Convert.ToUInt32(_tokens[0]);
                                    c.dropStudent(zid);
                                    break;
                                }
                            }
                        }
                        break;
                    case "8": //The rest of these will break the loop
                        runFlag = false;
                        break;
                    case "q":
                        runFlag = false;
                        break;
                    case "Q":
                        runFlag = false;
                        break;
                    case "quit":
                        runFlag = false;
                        break;
                    case "QUIT":
                        runFlag = false;
                        break;
                    case "exit":
                        runFlag = false;
                        break;
                    case "EXIT":
                        runFlag = false;
                        break;
                }
            }
        }
    }
}
