///////////////////////////////////////
/// Donny Kapic z1855273
/// Justin Roesner z1858242
/// CSCI 473 .NET programming
/// Assign 1
///////////////////////////////////////
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Text;

namespace DonnyJustin_Assign2
{
    public class Student : IComparable
    {
        readonly uint zid;
        private string firstName;
        private string lastName;
        private string major;
        public enum Year { Freshman, Sophomore, Junior, Senior, PostBacc };
        private Year? year;
        private ushort? creditHours;
        private float? gpa;

        // default constructor
        public Student()
        {
            firstName = null;
            lastName = null;
            major = null;
            year = null;
            creditHours = null;
            gpa = null;
        }

        // alternate constructor
        public Student(string line)
        {
            // parse line with ',' as delimiter
            string[] tokens = line.Split(',');

            uint id = Convert.ToUInt32(tokens[0]);
            this.zid = id;
            setLastName(tokens[1]);
            setFirstName(tokens[2]);
            setMajor(tokens[3]);

            // convert year to ushort
            ushort year = Convert.ToUInt16(tokens[4]);
            SetYear(year);

            // convert gpa to float
            float tempGpa = float.Parse(tokens[5], System.Globalization.CultureInfo.InvariantCulture);
            setGpa(tempGpa);
        }
        public int CompareTo(Object alpha)
        {
            if (alpha == null) throw new ArgumentNullException();

            Student rightOp = alpha as Student;

            if (rightOp != null)
                return zid.CompareTo(rightOp.zid);
            else
                throw new ArgumentException("[Student]:CompareTo argument is not a Student");
        }
        void setGpa(float gpa)
        {
            //gpa needs to be between [0,4]
            if (gpa >= 0 && gpa <= 4)
                this.gpa = gpa;
            else
                Console.WriteLine("GPA must be >= 0 and <= 4");
        }
        public void SetYear(ushort year)
        {
            switch (year)
            {
                case 0:
                    this.year = Year.Freshman;
                    break;
                case 1:
                    this.year = Year.Sophomore;
                    break;
                case 2:
                    this.year = Year.Junior;
                    break;
                case 3:
                    this.year = Year.Senior;
                    break;
                case 4:
                    this.year = Year.PostBacc;
                    break;
                default:
                    Console.WriteLine("Invalid input for Academic Year. Must be [0,4]");
                    break;
            }
        }
        void setMajor(string major)
        {
            this.major = major;
        }
        void setLastName(string lastName)
        {
            this.lastName = lastName;
        }
        void setFirstName(string firstName)
        {
            this.firstName = firstName;
        }
        public void setCreditHours(ushort? credithours)
        {
            //add this courses credit hours to student's total credit hours
            this.creditHours += creditHours;
        }
        public override string ToString()
        {
            //return a formatted string.
            string keyValue = "Z" + this.getZid() + " -- " + this.getLastName() + ", " + this.getFirstName()
                                  + " [" + this.GetYear() + "] (" + this.getMajor() + ") |" + this.getGpa() + "|";
            return keyValue;
        }
        public uint getZid()
        {
            return this.zid;
        }
        public float? getGpa()
        {
            return this.gpa;
        }
        public Year GetYear()
        {
            return (Year)year;
        }
        public string getMajor()
        {
            return this.major;
        }
        public string getLastName()
        {
            return this.lastName;
        }
        public string getFirstName()
        {
            return this.firstName;
        }
        public int getCreditHours()
        {
            int hours = Convert.ToUInt16(this.creditHours);
            return hours;
        }
    }
}






