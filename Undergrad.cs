using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic;

namespace StudentDb
{
    public enum YearRank
    {
        //Our YearRank type represents the students 
        //Progress and placement in the undergrad degree 
        Freshamn = 1, 
        Sophomore = 2,
        Junior = 3,
        Senior = 4,
    }

    //Inherits the student class and all code from it 

    //Chained constructor that uses base class constructor  
    internal class Undergrad : Student
    {
        public YearRank Rank { get; set; }

        public string DegreeMajor { get; set;  }

        public override string StudentType { get; }

        public Undergrad(string firstMidName, string lastName, double gradePtAvg, string emailAddress, YearRank rank, string degreeMajor)
        : base(firstMidName, lastName, gradePtAvg, emailAddress)
        {
            this.Rank = rank;
            this.DegreeMajor = degreeMajor;
            this.StudentType = this.GetType().Name;
        }
        public override string ToString()
        {
            string display_str = base.ToString();
            display_str += $" Rank: {this.Rank}\n";
            display_str += $"Major: {this.DegreeMajor}\n";
            display_str += $"Type: {this.StudentType}\n";


            return display_str;
        }
    }
}
