using System;

namespace TodoApi.Models
{
    public class TodoItem
    {
        public long Id {get; set;}
        public string Name {get; set;}
        public int Importance {get; set;}
        public DateTime DueDate {get; set;}

        public string Operator {get; set;}

        public bool IsCompleated {get;set;}

        public User User {get;set;}
    }
}