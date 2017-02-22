using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        [Required(ErrorMessage ="不能为空")]
        [MaxLength(10, ErrorMessage = "最大长度为{1}个字符")]
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
