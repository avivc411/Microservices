using System;
using System.Runtime.Serialization;

namespace Consumer.Model
{
    [Serializable]
    public class Record
    {
        public string name { get; set; }
        public DateTime date { get; set; }
        public int age { get; set; }
        public string profession { get; set; }
    }
}