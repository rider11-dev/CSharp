using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace EmitDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            //1、helloworld
            //HelloWorld.Say();

            //2、EmitBasicFlow
            //EmitBasicFlow.Do();

            //3、members
            //EmitClassMembers.Do();

            //4、Iterator
            //EmitIterator.Do();

            //5、EmitExceptionHandler
            //EmitExceptionHandler.Do();

            //6、DynamicViewModelBuilder
            //var usrViewModel = DynamicViewModelBuilder.GetInstance<IUserViewModel>(typeof(BaseModel), () =>
            //{
            //    var jsonFile = AppDomain.CurrentDomain.BaseDirectory.TrimEnd('/', '\\') + "/prop_attributes.json";
            //    var data = File.ReadAllText(jsonFile);
            //    return JsonConvert.DeserializeObject<IEnumerable<PropCustomAttrUnit>>(data);
            //});
            ////var maxLenAttr = (usrViewModel.GetType().GetProperty("username").GetCustomAttribute(typeof(MaxLengthAttribute)) as ValidationAttribute);

            //usrViewModel.username = "张鹏飞";
            //Console.WriteLine(usrViewModel.username);

            //7、EmitCtorInvoke
            //EmitCtorInvoke.Do();

            //8、EmitInterfaceMethods
            //EmitInterfaceMethods.Do();

            //9、EmitmapperCopy
            EmitmapperCopy.Do();

            Console.ReadKey();
        }
    }
}
