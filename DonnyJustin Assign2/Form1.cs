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
        public Form1()
        {
            InitializeComponent();

            // List of Course objects
            //List<Course> coursePool = new List<Course>();

            // Key: ZID    Value: Student object
            IDictionary<uint, Student> studentPool = new Dictionary<uint, Student>();

            // read student input file
            string[] studentLines = File.ReadAllLines("../../input_01.txt");

            // read coures input file
            string[] courseLines = File.ReadAllLines("../../input_02.txt");

            // read majors input file
            string[] majorLines = File.ReadAllLines("../../input03.txt");

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
                listBox1.Items.Add(s.ToString());
            foreach (Course c in coursePool)
            {
                //TODO
                //add unique department codes
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

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            //print class roster
            //richTextBox1.Text = "you hit the button for print!";
            // print list of courses
            foreach (Course c in coursePool)
                richTextBox1.Text += c.ToString() + "\n";
        }
        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            //on student activated item change: 
            richTextBox1.Text += "test test test \n";
        }

        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void button6_Click(object sender, EventArgs e)
        {
            //CLICK on Add course
            string courseCode = (string)comboBox3.SelectedItem;
            string courseNumber = textBox4.Text;
            string sectionNumber = textBox3.Text;
            string capacity = numericUpDown1.Value.ToString();
            richTextBox1.Text += courseCode + " " + courseNumber + "-" + sectionNumber + " (0/" + capacity + ")" + "\n";
        }

        private void button5_Click(object sender, EventArgs e)
        {
            //CLICK on Add student
            string studentName = textBox1.Text;
            string zid = textBox2.Text;
            string studentMajor = (string)comboBox1.SelectedItem;
            string academicYear = (string)comboBox2.SelectedItem;
            richTextBox1.Text += zid + "," + studentName + "," + studentMajor + "," + academicYear + "0" + "\n";
 
        }
    }
}
