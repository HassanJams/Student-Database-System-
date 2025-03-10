using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentDb
{
    internal class Graduate : Student
    {
        public decimal TuitionCredit { get; set; }
        public string FacultyAdvisor { get; set; }
        public override string StudentType { get; }

        public Graduate(string firstMidName, string lastName, double gradePtAvg, string emailAddress, decimal tuitionCredit, string facultyAdvisor)
            : base(firstMidName, lastName, gradePtAvg, emailAddress)
        {
            this.TuitionCredit = tuitionCredit;
            this.FacultyAdvisor = facultyAdvisor;
            this.StudentType = this.GetType().Name;
        }

        public override string ToString()
        {
            string display_str = base.ToString();
            display_str += $"Credit: {this.TuitionCredit}\n";
            display_str += $"FacAdv: {this.FacultyAdvisor}\n";
            display_str += $"Type: {this.StudentType}\n";
            return display_str;
        }
    }
}
