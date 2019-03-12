using System;
using System.Collections.Generic;

namespace TodoApi.Models
{

    public class User
    {
        public long Id {get;set;}
        public string Name {get;set;}

        public string Email {get;set;}
        public byte[] Pwhash {get;set;}
        public IEnumerable<TodoItem> TodoItem {get;set;}
    }
}