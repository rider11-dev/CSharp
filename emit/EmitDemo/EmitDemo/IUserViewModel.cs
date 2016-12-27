using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmitDemo
{
    public interface IUserViewModel
    {
        string username { get; set; }
        int age { get; set; }
        Sex sex { get; set; }
        DateTime birth { get; set; }
    }

    public class User
    {
        public string username { get; set; }
        public int age { get; set; }
        public Sex sex { get; set; }
        public DateTime birth { get; set; }
    }

    public enum Sex
    {
        female,
        male
    }
}
