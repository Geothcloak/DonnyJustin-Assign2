﻿///////////////////////////////////////
/// Donny Kapic z1855273
/// Justin Roesner z1858242
/// CSCI 473 .NET programming
/// Assign 2
///////////////////////////////////////
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace DonnyJustin_Assign2
{
    public partial class Form1 : Form
    {
        List<Course> coursePool = new List<Course>();
        IDictionary<uint, Student> studentPool = new Dictionary<uint, Student>();
        System.Collections.Generic.SortedDictionary<uint, Student> sortedPool = null;

        public Form1()
        {
            InitializeComponent();

            // read student input file
            string[] studentLines = File.ReadAllLines("../../input_01.txt");

            // read coures input file
            string[] courseLines = File.ReadAllLines("../../input_02.txt");

            // read majors input file
            string[] majorLines = File.ReadAllLines("../../input_03.txt");

            // add courses to coursePool
            for (int i = 0; i < courseLines.Length; i++)
            {
                Course course = new Course(courseLines[i]);
                coursePool.Add(course);
            }
            // add values to dictionary
            for (int i = 0; i < studentLines.Length; i++)
            {
                string[] tokens = studentLines[i].Split(',');
                uint zid = Convert.ToUInt32(tokens[0]);
                Student student = new Student(studentLines[i]);
                studentPool.Add(zid, student);
            }

            // Sort Dictionary by ZID and store in new Dictionary
            if (studentPool != null)
                sortedPool = new SortedDictionary<uint, Student>(studentPool);

            // Sort Courses alphabetically
            var sortedCourses = sortCourses(coursePool);

            // print courses to second listbox
            foreach (Course c in sortedCourses)
                listBox2.Items.Add(c);
            foreach (KeyValuePair<uint, Student> s in sortedPool)
                listBox1.Items.Add("z" + s.Key + " ~ " + s.Value.getLastName() + ", " + s.Value.getFirstName());

            foreach (Course c in coursePool)
            {
                int index = comboBox3.FindString(c.GetDepartmentCode());
                if (index == -1)
                {
                    comboBox3.Items.Add(c.GetDepartmentCode());
                }
            }

            //add majors to dropdown
            foreach (string line in majorLines)
            {
                comboBox1.Items.Add((line));
            }
            comboBox2.Items.Add("Freshman");
            comboBox2.Items.Add("Sophomore");
            comboBox2.Items.Add("Junior");
            comboBox2.Items.Add("Senior");
            comboBox2.Items.Add("PostBacc");
        }

        private List<Course> sortCourses(List<Course> coursePool)
        {
            // Sort Courses alphabetically
            var sortedCourses = coursePool.OrderBy(c => c.GetDepartmentCode()).ToList();
            return sortedCourses;
        }

        // Print student info and class enrollment when clicked in listBox1
        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            // cast selected index to string
            string selectedStudent = listBox1.SelectedItem.ToString();

            // tokenize selected index
            string[] tokens = selectedStudent.Split(' ');
            tokens[0] = tokens[0].Remove(0, 1);

            int emptyCheck = 0;
            foreach (KeyValuePair<uint, Student> s in studentPool)
            {
                if (tokens[0] == s.Key.ToString())  // if zid exists in student pool, print zid
                {
                    Output_RichTextBox.Text += "\n" + s.Value + "\n";
                    Output_RichTextBox.Text += "-----------------------------------------------------------------" +
                                               "-----------------------------------\n";

                    foreach (Course c in coursePool)
                    {
                        uint[] tempArray = c.GetStudentsEnrolled();     // get list of zid's enrolled
                        for (int i = 0; i < tempArray.Length; i++)
                        {
                            if (s.Key == tempArray[i])                  // if zid is enrolled in class, print student info
                            {
                                Output_RichTextBox.Text += c + "\n";
                                emptyCheck++;
                            }
                        }
                    }

                }

            }

            if (emptyCheck == 0)
                Output_RichTextBox.Text += "This student is not enrolled in any classes.\n";

            //on student activated item change: 
            //Output_RichTextBox.Text += "Student selected: " + listBox1.SelectedItem as string + "\n";
        }

        private void Search_Button_Click(object sender, EventArgs e)
        {
            //if both text boxes are empty do nothing
            if (ZID_RichTextBox.Text.Length <= 0 && Course_RichTextBox.Text.Length <= 0)
            {
                //do nothing and refill lists
                listBox1.Items.Clear();
                foreach (KeyValuePair<uint, Student> s in sortedPool)
                    listBox1.Items.Add("z" + s.Key + " ~ " + s.Value.getLastName() + ", " + s.Value.getFirstName());
                listBox2.Items.Clear();
                var sortedCourses = sortCourses(coursePool);
                foreach (Course c in sortedCourses)
                    listBox2.Items.Add(c);

            }

            //if both textboxes are full sort by both
            else if (ZID_RichTextBox.Text.Length > 0 && Course_RichTextBox.Text.Length > 0)
            {
                List<Student> tempStudentList = new List<Student>();
                foreach (KeyValuePair<uint, Student> kvp in studentPool)
                {
                    if (kvp.Key.ToString().StartsWith(ZID_RichTextBox.Text))
                    { 
                        tempStudentList.Add(kvp.Value);
                    }
                }
                //clear list box
                listBox1.Items.Clear();

                var sortedStudents = tempStudentList.OrderBy(s => s.getZid()).ToList();

                //for loop print array
                foreach (Student s in sortedStudents)
                {
                    listBox1.Items.Add("z" + s.getZid() + " ~ " + s.getLastName() + ", " + s.getFirstName());
                }

                List<Course> tempCoursePool = new List<Course>();
                foreach (Course c in coursePool)
                {
                    if (string.Equals(c.GetDepartmentCode(), Course_RichTextBox.Text))
                    {
                        tempCoursePool.Add(c);
                    }
                }

                //clear list box
                listBox2.Items.Clear();
                var sortedCourses = sortCourses(tempCoursePool);

                foreach (Course c in sortedCourses)
                {
                    listBox2.Items.Add(c);
                }
            }
            //if only zid textbox is full
            else if (ZID_RichTextBox.Text.Length > 0)
            {
                listBox2.Items.Clear();
                foreach (Course c in coursePool)
                {
                    listBox2.Items.Add(c);
                }

                List<Student> tempStudentList = new List<Student>();
                foreach (KeyValuePair<uint, Student> kvp in studentPool)
                {
                    if (kvp.Key.ToString().StartsWith(ZID_RichTextBox.Text))
                    {
                        tempStudentList.Add(kvp.Value);
                    }
                }
                //clear list box
                listBox1.Items.Clear();
                var sortedStudents = tempStudentList.OrderBy(s => s.getZid()).ToList();

                //for loop print array
                foreach (Student s in sortedStudents)
                {
                    listBox1.Items.Add("z" + s.getZid() + " ~ " + s.getLastName() + ", " + s.getFirstName());
                }
            }

            //if only course textbox is full
            else
            {
                listBox1.Items.Clear();
                foreach (KeyValuePair<uint, Student> s in sortedPool)
                    listBox1.Items.Add("z" + s.Key + " ~ " + s.Value.getLastName() + ", " + s.Value.getFirstName());

                List<Course> tempCoursePool = new List<Course>();
                foreach (Course c in coursePool)
                {
                    if (string.Equals(c.GetDepartmentCode(), Course_RichTextBox.Text))
                    {
                        tempCoursePool.Add(c);
                    }
                }
                //clear list box
                listBox2.Items.Clear();

                foreach (Course c in tempCoursePool)
                {
                    listBox2.Items.Add(c);
                }
            }
        }

        // Enroll student into a class
        private void Enroll_Button_Click(object sender, EventArgs e)
        {
            // Cast selected student and course to string
            string selectedStudent = listBox1.SelectedItem.ToString();
            string selectedCourse = listBox2.SelectedItem.ToString();

            // parse student selected
            string[] studentTokens = selectedStudent.Split(' ');
            studentTokens[0] = studentTokens[0].Remove(0, 1);

            // parse course selected.   [0] = Department   [1] = Course   [2] = Section
            string[] tokens = selectedCourse.Split(' ', '-');

            // check if student exists
            bool studentExists = false;
            foreach (KeyValuePair<uint, Student> kvp in studentPool)
            {
                if (studentTokens[0] != kvp.Key.ToString())
                    studentExists = false;
                else
                {
                    studentExists = true;
                    break;
                }
            }

            // check if class exists
            bool classExists = false;
            foreach (Course c in coursePool)
            {
                if (tokens[0] == c.GetDepartmentCode() && tokens[1] == c.GetCourseNumber().ToString() && tokens[2] == c.GetSectionNumber())
                    classExists = true;
            }

            // check if class has available enrollment
            bool classFull = true;
            foreach (Course c in coursePool)
            {
                if (string.Equals(tokens[0], c.GetDepartmentCode()) && tokens[1] == c.GetCourseNumber().ToString() && tokens[2] == c.GetSectionNumber())
                {
                    if (c.GetTotalCurrentlyEnrolled() >= c.GetMaxCapacity())
                    {
                        classFull = true;
                        Output_RichTextBox.Text += "\nThis class is currently full.\n";
                    }
                    else
                        classFull = false;

                    // if student and class exists and class is not full, enroll student
                    if ((studentExists == true && classExists == true && classFull == false))
                    {
                        uint zid = Convert.ToUInt32(studentTokens[0]);

                        foreach (KeyValuePair<uint, Student> s in studentPool)
                        {
                            if (s.Key == zid && s.Value.getCreditHours() <= 18)
                            {
                                // add credit hours to student
                                s.Value.setCreditHours(c.GetCreditHours());
                                c.addStudent(zid);
                                Output_RichTextBox.Text += "\nStudent added successfully.\n";
                            }
                        }
                    }
                }
            }

            // sort courses
            var sortedCourses = sortCourses(coursePool);

            // update course list
            listBox2.Items.Clear();
            foreach (Course c in sortedCourses)
                listBox2.Items.Add(c);
        }

        // Print list of students enrolled in a class
        private void CourseRoster_Button_Click_1(object sender, EventArgs e)
        {
            // Cast selected course to string
            string selectedCourse = listBox2.SelectedItem.ToString();

            int emptyCheck = 0;
            foreach (Course c in coursePool)
            {
                if (selectedCourse == c.ToString())                // find selected course in List
                {
                    Output_RichTextBox.Text += "\nCourse: " + selectedCourse + "\n";
                    Output_RichTextBox.Text += "-----------------------------------------------\n";

                    uint[] tempArray = c.GetStudentsEnrolled();     // get list of zid's enrolled
                    foreach (KeyValuePair<uint, Student> s in studentPool)
                    {
                        for (int i = 0; i < tempArray.Length; i++)
                        {
                            if (s.Key == tempArray[i])              // if student is enrolled in the class, print their info
                            {
                                Output_RichTextBox.Text += "z" + tempArray[i] + "   " + s.Value.getLastName() + ", "
                                                        + s.Value.getFirstName() + "   " + s.Value.getMajor() + "\n";
                                emptyCheck++;
                            }
                        }
                    }
                }
            }

            if (emptyCheck == 0)
                Output_RichTextBox.Text += "Class is empty.\n";
        }

        // Drop student from a selected class
        private void Drop_Button_Click(object sender, EventArgs e)
        {
            string selectedStudent = listBox1.SelectedItem.ToString();
            string selectedCourse = listBox2.SelectedItem.ToString();

            // parse student selected
            string[] studentTokens = selectedStudent.Split(' ');
            studentTokens[0] = studentTokens[0].Remove(0, 1);

            // parse course selected.   [0] = Department   [1] = Course   [2] = Section
            string[] tokens = selectedCourse.Split(' ', '-');

            // check if student exists
            bool studentExists = false;
            //uint id = Convert.ToUInt64(ZID_RichTextBox.Text.ToString());
            foreach (KeyValuePair<uint, Student> kvp in studentPool)
            {
                if (studentTokens[0] != kvp.Key.ToString())
                    studentExists = false;
                else
                {
                    studentExists = true;
                    break;
                }
            }

            // check if class exists
            bool classExists = false;
            foreach (Course c in coursePool)
            {
                if (tokens[0] == c.GetDepartmentCode() && tokens[1] == c.GetCourseNumber().ToString() && tokens[2] == c.GetSectionNumber())
                    classExists = true;
            }

            // check if class has available enrollment
            foreach (Course c in coursePool)
            {
                if (string.Equals(tokens[0], c.GetDepartmentCode()) && tokens[1] == c.GetCourseNumber().ToString() && tokens[2] == c.GetSectionNumber())
                {
                    if ((studentExists == true && classExists == true))
                    {
                        uint zid = Convert.ToUInt32(studentTokens[0]);

                        foreach (KeyValuePair<uint, Student> s in studentPool)
                        {
                            if (s.Key == zid)
                            {
                                // add credit hours to student
                                s.Value.setCreditHours(c.GetTotalCurrentlyEnrolled());
                                c.dropStudent(zid);
                                Output_RichTextBox.Text += "\nStudent dropped successfully.\n";
                            }
                        }
                    }
                }
            }

            // update course list
            listBox2.Items.Clear();
            foreach (Course c in coursePool)
                listBox2.Items.Add(c);
        }

        // Add a course to the course list
        private void AddCourse_Button_Click(object sender, EventArgs e)
        {
            string courseCode = (string)comboBox3.SelectedItem;
            string courseNumber = textBox4.Text;
            string sectionNumber = textBox3.Text;
            string creditHours = creditHours_TextBox.Text;
            string capacity = numericUpDown1.Value.ToString();
            string combined = courseCode + "," + courseNumber + "," + sectionNumber + "," + creditHours + "," + capacity;

            if (courseNumber.Length > 4)
            {
                Output_RichTextBox.Text += "\nDepartment number must be exactly 4 characters long.\n";
                return;
            }
            if (sectionNumber.Length != 4)
            {
                Output_RichTextBox.Text += "\nSection number must be exactly 4 characters long.\n";
                return;
            }
            if (Convert.ToUInt32(creditHours) < 0 || Convert.ToUInt32(creditHours) > 6)
            {
                Output_RichTextBox.Text += "\nCredit hours must be in the range [0,6].\n";
            }

            Output_RichTextBox.Text += courseCode + " " + courseNumber + "-" + sectionNumber + " (0/" + capacity + ")" + "\n";

            Course course = new Course(combined);
            coursePool.Add(course);

            // Sort Courses alphabetically
            var sortedCourses = sortCourses(coursePool);

            // update course list
            listBox2.Items.Clear();
            foreach (Course c in sortedCourses)
                listBox2.Items.Add(c);
        }

        // Add a student to the student list
        private void AddStudent_Button_Click(object sender, EventArgs e)
        {
            //CLICK on Add student
            string studentName = textBox1.Text;
            string zid = textBox2.Text;
            string studentMajor = (string)comboBox1.SelectedItem;
            string academicYear = (string)comboBox2.SelectedItem;
            Output_RichTextBox.Text += "z" + zid + "," + studentName + ", " + studentMajor + ", " + academicYear + ", " + "0" + "\n";

            ushort year = 5;

            if (comboBox2.Items.Equals("Freshman"))
                year = 0;
            else if (comboBox2.Items.Contains("Sophomore"))
                year = 1;
            else if (comboBox2.Items.Contains("Junior"))
                year = 2;
            else if (comboBox2.Items.Contains("Senior"))
                year = 3;
            else if (comboBox2.Items.Contains("PostBacc"))
                year = 4;
            else
                return;
            

            string combined = zid + "," + studentName + "," + studentMajor + "," + year.ToString() + "," + "0";
            string[] tokens = combined.Split(',');
            uint _zid = Convert.ToUInt32(tokens[0]);

            Student student = new Student(combined);
            studentPool.Add(_zid, student);

            // update course list
            // Sort Dictionary by ZID and store in new Dictionary
            if (studentPool != null)
                sortedPool = new SortedDictionary<uint, Student>(studentPool);

            listBox1.Items.Clear();
            foreach (KeyValuePair<uint, Student> s in sortedPool)
                listBox1.Items.Add("z" + s.Key + " ~ " + s.Value.getLastName() + ", " + s.Value.getFirstName());
        }

        // Clear the output text box
        private void Clear_Button_Click(object sender, EventArgs e)
        {
            Output_RichTextBox.Text = "";
        }
    }
}