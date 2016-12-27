using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmitDemo
{
    public class BaseModel
    {
        public void RaisePropertyChanged(string propName)
        {
            Console.WriteLine("value of property {0} has changed", propName);
        }
    }
}
