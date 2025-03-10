using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JsonSubTypes;
using Newtonsoft.Json;

namespace StudentDb
{

    [JsonConverter(typeof(JsonSubtypes), "StudentType")]
    [JsonSubtypes.KnownSubType(typeof(Undergrad), "Undergrad")]
    [JsonSubtypes.KnownSubType(typeof(Graduate), "Graduate")]
    internal class Student
    {
        public string FirstMidName { get; set; }
        public string LastName { get; set; }
        public double GradePtAvg { get; set; }
        public string EmailAddress { get; set; }

        public virtual string StudentType { get;  }

        public Student()
        {
            this.StudentType = this.GetType().Name;
        }
        public Student(string firstMidName, string lastName, double gradePtAvg, string emailAddress)
        {
            this.FirstMidName = firstMidName;
            this.LastName = lastName;
            this.GradePtAvg = gradePtAvg;
            this.EmailAddress = emailAddress;
            this.StudentType = this.GetType().Name;
        }

        public override string ToString()
        {
            string display_str = "***** Student Record *****\n";
            display_str += $"First: {this.FirstMidName}\n";
            display_str += $" Last: {this.LastName}\n";
            display_str += $"  GPA: {this.GradePtAvg}\n";
            display_str += $"Email: {this.EmailAddress}\n";

            return display_str;
        }
        public string ToCSVFormat()
          => $"{this.FirstMidName},{this.LastName},{this.GradePtAvg},{this.EmailAddress}";


    }

}
