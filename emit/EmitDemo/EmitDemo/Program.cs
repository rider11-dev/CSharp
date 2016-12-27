using System;
using System.Collections.Generic;
using System.Linq;
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

            //6、DynamicTypeBuilder
            var usrViewModel = DynamicViewModelBuilder.GetInstance<IUserViewModel>();
            usrViewModel.username = "张鹏飞";
            Console.WriteLine(usrViewModel.username);

            Console.ReadKey();
        }
    }
}
