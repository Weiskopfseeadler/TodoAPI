using System.Security.Cryptography;
using System.Text;
using System;
using System.Linq;
using TodoApi.Models;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace TodoApi.Services
{

    public static class SecurityService
    {
        private static Dictionary<string,int> dict = new Dictionary<string, int>();
        public static bool CanAuthenticate(byte[] inputpassword, byte[] storedPassword)
        {
            for(int i = 0; i < inputpassword.Length; i++)
            {
                if (inputpassword[i] != storedPassword[i]) return false;
             
            }
            return true;
        }

        public static byte[] Hash(User user)
        {
            byte[] salt = creatSalt(user);
            byte[] hashedInput = user.Pwhash;
            using(SHA512 chippers = SHA512.Create())
            {
                if (hashedInput == null)
                {
                     hashedInput = toByte("a");
                }
                hashedInput = chippers.ComputeHash(hashedInput);
                using(MD5 chippers2 = MD5.Create())
                {
                    salt = chippers2.ComputeHash(salt);

                    using(SHA384 chippers3 = SHA384.Create())
                    {
                        hashedInput = Combine(hashedInput,salt);
                        hashedInput = chippers3.ComputeHash(hashedInput);

                    hashedInput = chippers3.ComputeHash(chippers2.ComputeHash(chippers.ComputeHash(hashedInput)));  
                    }
                }
            }
            return hashedInput;
        }

        private static byte[] Combine(params byte[][] arrays)
        {
            byte[] rv = new byte[arrays.Sum(a => a.Length)];
            int offset = 0;
            foreach (byte[] array in arrays) 
            {
                System.Buffer.BlockCopy(array, 0, rv, offset, array.Length);
                offset += array.Length;
            }
            return rv;
        }

        public static byte[] toByte(string text)
        {
                return  Encoding.ASCII.GetBytes(text);
        }
        public static byte[] creatSalt(User user)
        {
            if (dict.Count<25)
            {
                dict.Clear();
                setDict();
            }
            string salt ="";
            int i;
            string c;
            var regexChar = new Regex("^[a-z ]*$");
            var regexNumber = new Regex("^[a-z0-9 ]*$");
  
             i = user.Name.Length % Convert.ToInt32(user.Email.Length);         
            for (int e = 0; e < 3; e++)
            {
                if (i!=0)
                {
                       i = user.Name.Length%i;
                }
             if (i>= user.Email.Length)
            {
                i =  user.Email.Length-1;
            } 
                    
            salt = salt + user.Email.ElementAt(i);
            c=Convert.ToString(user.Email.ElementAt(i));
            c=  c.ToLower();
            if(regexChar.IsMatch(c)){
                
                i = dict[c];
            }
             if (i!=0)
                {
                     i = user.Name.Length%i;  
                }
            if (i>= user.Name.Length)
            {
                i =  user.Name.Length-1;
            } 
         
            salt= salt+user.Name.ElementAt(i);
            c =Convert.ToString(user.Email.ElementAt(i));
            c  = c.ToLower();
                if(!regexChar.IsMatch(c))
                {
                    System.Console.WriteLine("++++++++++++++++++++++++++++++++++++++++++++++++++++");
                    System.Console.WriteLine(c);
                    i = dict[c];
                }
                if (!regexNumber.IsMatch(c))
                {
                    System.Console.WriteLine("==================================================");
                    System.Console.WriteLine(c);
                    i = Convert.ToInt32(c);
                }
            
            }
            return toByte(salt);
        }

        private static void setDict()
        {
            
            dict.Add("a",3);
            dict.Add("b",4);
            dict.Add("c",1);
            dict.Add("d",4);
            dict.Add("e",6);
            dict.Add("f",9);
            dict.Add("g",2);
            dict.Add("h",6);
            dict.Add("i",5);
            dict.Add("j",3);
            dict.Add("k",7);
            dict.Add("l",8);
            dict.Add("m",1);
            dict.Add("n",7);
            dict.Add("o",8);
            dict.Add("p",4);
            dict.Add("q",6);
            dict.Add("r",1);
            dict.Add("s",2);
            dict.Add("t",9);
            dict.Add("u",3);
            dict.Add("v",5);
            dict.Add("w",5);
            dict.Add("x",6);
            dict.Add("y",6);
            dict.Add("z",5);
        }
    }
}