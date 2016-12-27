using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace EmitDemo
{
    public class HelloWorld
    {
        /// <summary>
        /// 用来调用动态方法的委托
        /// </summary>
        private delegate void HelloWorldDelegate();
        public static void Say()
        {
            //定义动态方法
            DynamicMethod method = new DynamicMethod("Hello", null, null);
            //创建MSIL生成器，为动态方法生成代码
            ILGenerator helloIL = method.GetILGenerator();
            //加载字符参数
            helloIL.Emit(OpCodes.Ldstr, "hello world！");
            //调用Console.WriteLine方法输出
            helloIL.Emit(OpCodes.Call, typeof(Console).GetMethod("WriteLine", new Type[] { typeof(string) }));
            //方法结束，返回
            helloIL.Emit(OpCodes.Ret);

            //完成动态方法的创建，获取一个可执行该动态方法的委托
            HelloWorldDelegate helloDelegate = method.CreateDelegate(typeof(HelloWorldDelegate)) as HelloWorldDelegate;

            //执行动态方法
            helloDelegate();
        }
    }
}
