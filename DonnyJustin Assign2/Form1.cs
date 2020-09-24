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

        public Form1()
        {
            InitializeComponent();

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

            // print courses to second listbox
            foreach (Course c in coursePool)
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
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }


        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            //on student activated item change: 
            //Output_RichTextBox.Text += "Student selected: " + listBox1.SelectedItem as string + "\n";
        }

        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            //on course activated item change: 
            //Output_RichTextBox.Text += "Course selected: " + listBox2.SelectedItem as string + "\n";
        }

        private void Search_Button_Click(object sender, EventArgs e)
        {
            // if there is a zid in the textbox, search for that student
            if (ZID_RichTextBox.Text.Length <= 0 && Course_RichTextBox.Text.Length <= 0)
            {
                Output_RichTextBox.Text += "Please enter a ZID and/or a course.\n";
            }

            if (ZID_RichTextBox.Text.Length > 0)
            {
                foreach (KeyValuePair<uint, Student> s in studentPool)
                {
                    if (ZID_RichTextBox.Text == s.Key.ToString())
                    {
                        Output_RichTextBox.Text += s.Value + "\n";
                        Output_RichTextBox.Text += "-----------------------------------------------------------------\n";

                        foreach (Course c in coursePool)
                        {
                            uint[] tempArray = c.GetStudentsEnrolled();     // get list of zid's enrolled
                            for (int i = 0; i < tempArray.Length; i++)
                            {
                                if (s.Key == tempArray[i])
                                    Output_RichTextBox.Text += c + "\n";
                            }
                        }

                    }

                }
            }

            if (Course_RichTextBox.Text.Length > 0)
            {
                foreach (Course c in coursePool)
                    if (Course_RichTextBox.Text == c.GetDepartmentCode())
                    {
                        Output_RichTextBox.Text += c.ToString() + "\n";
                    }
            }

        }

        private void Enroll_Button_Click(object sender, EventArgs e)
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

                    if ((studentExists == true && classExists == true && classFull == false))
                    {
                        uint zid = Convert.ToUInt32(studentTokens[0]);

                        foreach (KeyValuePair<uint, Student> s in studentPool)
                        {
                            if (s.Key == zid && s.Value.getCreditHours() <= 18)
                            {
                                // add credit hours to student
                                s.Value.setCreditHours(c.GetTotalCurrentlyEnrolled());
                                c.addStudent(zid);
                                Output_RichTextBox.Text += "\nStudent added successfully.\n";
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

        private void CourseRoster_Button_Click_1(object sender, EventArgs e)
        {
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
                                                        + s.Value.getFirstName() + "   " + s.Value.getMajor() +"\n";
                                emptyCheck++;
                            }
                        }
                    }
                }
            }

            if (emptyCheck == 0)
                Output_RichTextBox.Text += "Class is empty.\n";

           
        }

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

                    if ((studentExists == true && classExists == true && classFull == false))
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

            // update course list
            listBox2.Items.Clear();
            foreach (Course c in coursePool)
                listBox2.Items.Add(c);
        }

        private void AddStudent_Button_Click(object sender, EventArgs e)
        {
            //CLICK on Add student
            string studentName = textBox1.Text;
            string zid = textBox2.Text;
            string studentMajor = (string)comboBox1.SelectedItem;
            string academicYear = (string)comboBox2.SelectedItem;
            string combined = zid + "," + studentName + "," + studentMajor + "," + academicYear + "," + "0";
            Output_RichTextBox.Text += zid + "," + studentName + "," + studentMajor + "," + academicYear + "," + "0" + "\n";

            string[] tokens = combined.Split(',');
            uint _zid = Convert.ToUInt32(tokens[0]);
            Student student = new Student(combined);
            studentPool.Add(_zid, student);

            foreach (KeyValuePair<uint, Student> s in studentPool)
                listBox1.Items.Add("z" + s.Key + " ~ " + s.Value.getLastName() + ", " + s.Value.getFirstName());
        }
    }

}