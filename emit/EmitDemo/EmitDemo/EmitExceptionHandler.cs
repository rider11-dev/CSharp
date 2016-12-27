using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace EmitDemo
{
    public class EmitExceptionHandler
    {
        public static void Do()
        {
            string name = "EmitDemo.ExceptionHandler";
            string asmFileName = name + ".dll";

            AssemblyName asmName = new AssemblyName(name);
            AssemblyBuilder assBuilder = AppDomain.CurrentDomain.DefineDynamicAssembly(asmName, AssemblyBuilderAccess.RunAndSave);
            ModuleBuilder moduleBuilder = assBuilder.DefineDynamicModule(name, asmFileName);
            TypeBuilder typeBuilder = moduleBuilder.DefineType(name, TypeAttributes.Public);

            MethodBuilder methodBuilder = typeBuilder.DefineMethod("ConvertToInt32", MethodAttributes.Public | MethodAttributes.Static, typeof(Int32), new Type[] { typeof(string) });
            ILGenerator methodIL = methodBuilder.GetILGenerator();

            LocalBuilder num = methodIL.DeclareLocal(typeof(Int32));
            //int num = 0;
            methodIL.Emit(OpCodes.Ldc_I4_0);
            methodIL.Emit(OpCodes.Stloc_0);//从计算堆栈的顶部弹出当前值并将其存储到索引 0 处的局部变量列表中
            //begin try
            Label lblTry = methodIL.BeginExceptionBlock();
            //num = Convert.ToInt32(str);
            methodIL.Emit(OpCodes.Ldarg_0);
            methodIL.Emit(OpCodes.Call, typeof(Convert).GetMethod("ToInt32", new Type[] { typeof(string) }));
            methodIL.Emit(OpCodes.Stloc_0);
            //end try

            //begin catch,注意，这个时侯堆栈顶为异常信息ex
            methodIL.BeginCatchBlock(typeof(Exception));
            //Console.WriteLine(ex.Message);
            methodIL.Emit(OpCodes.Call, typeof(Exception).GetMethod("get_Message"));
            methodIL.Emit(OpCodes.Call, typeof(Console).GetMethod("WriteLine", new Type[] { typeof(string) }));
            methodIL.EndExceptionBlock();

            //return num
            methodIL.Emit(OpCodes.Ldloc_0);
            methodIL.Emit(OpCodes.Ret);

            Type type = typeBuilder.CreateType();
            assBuilder.Save(asmFileName);

            Console.WriteLine("input a string :");
            var str = Console.ReadLine();
            int numRst = Convert.ToInt32(type.GetMethod("ConvertToInt32").Invoke(null, new object[] { str }));
            Console.WriteLine("convert result:{0}", numRst);
        }
    }
}
