///////////////////////////////////////
/// Donny Kapic z1855273
/// Justin Roesner z1858242
/// CSCI 473 .NET programming
/// Assign 1
///////////////////////////////////////
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
// random comment
namespace DonnyJustin_Assign2
{
    public class Course : IComparable
    {
        private string departmentCode; //at most 4 characters all capitalized
        private uint? courseNumber; //range[100,499] 
        private string sectionNumber; //exactly 4 alphanumeric characters
        private ushort? creditHours; //range of [0,6]
        private uint[] studentsEnrolled; //doesn't have to be an array
        private ushort? totalCurrentlyEnrolled;
        private ushort? maxCapacity;

        public Course()
        {
            departmentCode = null; //at most 4 characters all capitalized
            courseNumber = null; //range[100,499] 
            sectionNumber = null; //exactly 4 alphanumeric characters
            creditHours = null; //range of [0,6]
            studentsEnrolled = null; 
            totalCurrentlyEnrolled = null;
            maxCapacity = null;
        }

        public Course(string line)
        {
            string[] tokens = line.Split(' ', ',');

            SetDepartmentCode(tokens[0]);       // set department code

            uint courseNumber = Convert.ToUInt32(tokens[1]);
            SetCourseNumber(courseNumber);      // set course number

            SetSectionNumber(tokens[2]);        // set section number

            ushort creditHours = Convert.ToUInt16(tokens[3]);   // set number of students currently enrolled
            SetCreditHours(creditHours);

            ushort classCapacity = Convert.ToUInt16(tokens[4]); // set max class capacity
            SetCapacity(classCapacity);

            studentsEnrolled = new uint[Convert.ToInt32(maxCapacity)]; //initialize students array to beable to hold maxcapacity

            totalCurrentlyEnrolled = 0;
        }

        public override string ToString()
        {
            //format and return course string
            string course = this.GetDepartmentCode() + " " + this.GetCourseNumber() + "-" + this.GetSectionNumber() + " (" +
                            this.GetTotalCurrentlyEnrolled() + "/" + this.GetMaxCapacity() + ")";
            return course;
        }

        public void addStudent(uint zid)
        {
            for (int i = 0; i < studentsEnrolled.Length; i++)
            {
                if (studentsEnrolled[i] == 0)
                {
                    studentsEnrolled[i] = zid;
                    this.totalCurrentlyEnrolled += 1;
                    break;
                }
            }
        }

        public void dropStudent(uint zid)
        {
            for (int i = 0; i < studentsEnrolled.Length; i++)
            {
                //loop through array if zid matches then drop the student
                if (studentsEnrolled[i] == zid)
                {
                    studentsEnrolled[i] = 0;
                    this.totalCurrentlyEnrolled -= 1;
                }
            }
        }
        //////////////////////////////////////////////
        //setters and getters go here.
        public void SetDepartmentCode(string _departmentCode)
        {
            bool isUpper;

            // check for 4 characters
            if (_departmentCode.Length > 4)
                throw new System.ArgumentException("Deparment code must be at most 4 characters.");

            //at most 4 characters all capitalized
            for (int i = 0; i < _departmentCode.Length; i++)
            {
                // check for upper case
                if (!(isUpper = Char.IsUpper(_departmentCode, i)))
                    throw new System.ArgumentException("All department code characters must be capitalized.");
            }

            departmentCode = _departmentCode;
        }
        public void SetCourseNumber(uint? _courseNumber)
        {
            //range[100,499] 
            if (_courseNumber < 100 || _courseNumber > 499)
                throw new System.ArgumentException("Course number must be in the range [100,499]");
            else
                courseNumber = _courseNumber;
        }
        public void SetSectionNumber(string _sectionNumber)
        {
            //exactly 4 alphanumeric characters
            bool isAlphaNum;

            if (_sectionNumber.Length != 4)
                throw new System.ArgumentException("Section Number must be exactly 4 characters.");

            for (int i = 0; i < _sectionNumber.Length; i++)
            {
                // check for alphanumeric characters
                if (!(isAlphaNum = Char.IsLetterOrDigit(_sectionNumber, i)))
                    throw new System.ArgumentException("Section Number must be alphanumeric.");
            }

            sectionNumber = _sectionNumber;
        }
        public void SetCreditHours(ushort? _creditHours)
        {
            //range of [0,6]
            if (_creditHours < 0 || _creditHours > 6)
                throw new System.ArgumentException("Credit hours must be in the range [0,6]");
            else
                creditHours = _creditHours;
        }
        public void SetTotalCurrentlyEnrolled(ushort? _totalCurrentlyEnrolled)
        {
            this.totalCurrentlyEnrolled = _totalCurrentlyEnrolled;
        }
        public void SetCapacity(ushort? _maxCapacity)
        {
            this.maxCapacity = _maxCapacity;
        }
        public string GetSectionNumber()
        {
            return sectionNumber;
        }
        public string GetDepartmentCode()
        {
            return departmentCode;
        }
        public uint? GetCourseNumber()
        {
            return courseNumber;
        }
        public ushort? GetCreditHours()
        {
            return creditHours;
        }
        public uint[] GetStudentsEnrolled()
        {
            return studentsEnrolled;
        }
        public ushort? GetTotalCurrentlyEnrolled()
        {
            return totalCurrentlyEnrolled;
        }
        public ushort? GetMaxCapacity()
        {
            return maxCapacity;
        }
        //////////////////////////////////////////////

        //// Icomparable interface 
        //allow course objects to be sorted
        //first by department code, then course number
        public int CompareTo(Object alpha)
        {
            if (alpha == null) throw new ArgumentNullException();

            Course rightOp = alpha as Course;
            if (rightOp != null)
                return departmentCode.CompareTo(rightOp.departmentCode);
            else
                throw new ArgumentException("[Course]:CompareTo arguement is not a Course");
        }
      
        //// IEnumerable interface 
        //allow for you to use a foreach loop to iterate over the array/collection
    }
    public class Courses : IEnumerable
    {
        private Course[] _courses;
        public Courses(Course[] pArray)
        {
            _courses = new Course[pArray.Length];

            for (int i = 0; i < pArray.Length; i++)
            {
                _courses[i] = pArray[i];
            }
        }

        // Implementation for the GetEnumerator method.
        IEnumerator IEnumerable.GetEnumerator()
        {
            return (IEnumerator)GetEnumerator();
        }

        public CoursesEnum GetEnumerator()
        {
            return new CoursesEnum(_courses);
        }
    }
    // When you implement IEnumerable, you must also implement IEnumerator.
    public class CoursesEnum : IEnumerator
    {
        public Course[] _courses;

        // Enumerators are positioned before the first element
        // until the first MoveNext() call.
        int position = -1;

        public CoursesEnum(Course[] list)
        {
            _courses = list;
        }

        public bool MoveNext()
        {
            position++;
            return (position < _courses.Length);
        }

        public void Reset()
        {
            position = -1;
        }

        object IEnumerator.Current
        {
            get
            {
                return Current;
            }
        }

        public Course Current
        {
            get
            {
                try
                {
                    return _courses[position];
                }
                catch (IndexOutOfRangeException)
                {
                    throw new InvalidOperationException();
                }
            }
        }
    }
}
