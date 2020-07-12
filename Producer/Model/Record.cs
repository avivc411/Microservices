using System;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Producer.Model
{
    [Serializable]
    public class Record
    {
        public string name { get; set; }
        public DateTime date { get; set; }
        [JsonIgnore]
        public int age { get; set; }
        public string profession { get; set; }

        public Record()
        {
        }

        public Record(Record other)
        {
            if (other == null)
                return;
            name = other.name;
            date = other.date;
            age = other.age;
            profession = other.profession;
        }
    }
}