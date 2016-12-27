using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace EmitDemo
{
    public class EmitIterator
    {
        public static void Do()
        {
            string name = "EmitDemo.Iterator";
            string asmFileName = name + ".dll";

            AssemblyName asmName = new AssemblyName(name);
            AssemblyBuilder assBuilder = AppDomain.CurrentDomain.DefineDynamicAssembly(asmName, AssemblyBuilderAccess.RunAndSave);
            ModuleBuilder moduleBuilder = assBuilder.DefineDynamicModule(name, asmFileName);
            TypeBuilder typeBuilder = moduleBuilder.DefineType(name, TypeAttributes.Public);

            //
            MakeForMethod(typeBuilder);
            MakeForeachMethod(typeBuilder);

            Type type = typeBuilder.CreateType();
            assBuilder.Save(asmFileName);

            Console.WriteLine("please input an int array:");
            string input = Console.ReadLine();
            var arr = input.Split(',');
            var ints = arr.Select(s => Convert.ToInt32(s)).ToArray();

            var rst1 = Convert.ToInt32(type.GetMethod("ForMethod").Invoke(null, new object[] { ints }));
            var rst2 = Convert.ToInt32(type.GetMethod("ForeachMethod").Invoke(null, new object[] { ints }));
            Console.WriteLine("result of ForMethod is :{0}", rst1);
            Console.WriteLine("result of ForeachMethod is :{0}", rst2);
        }

        static void MakeForMethod(TypeBuilder typeBuilder)
        {
            //定义一个传入参数为Int32[]，返回值为Int32的方法
            //注意,非静态方法的第一参数是this指针,而Static方法的第一个参数既是传入的第一个参数,所以在使用时要注意
            MethodBuilder methodBuilder = typeBuilder.DefineMethod("ForMethod", MethodAttributes.Public | MethodAttributes.Static, typeof(Int32), new Type[] { typeof(Int32[]) });
            ILGenerator methodIL = methodBuilder.GetILGenerator();

            //局部变量
            LocalBuilder sum = methodIL.DeclareLocal(typeof(Int32));
            LocalBuilder idx = methodIL.DeclareLocal(typeof(Int32));

            Label lblCompare = methodIL.DefineLabel();
            Label lblEnterLoop = methodIL.DefineLabel();

            //int sum=0;
            methodIL.Emit(OpCodes.Ldc_I4_0);
            methodIL.Emit(OpCodes.Stloc_0);//从计算堆栈的顶部弹出当前值并将其存储到索引 0 处（sum）的局部变量列表中
            //int idx=0;
            methodIL.Emit(OpCodes.Ldc_I4_0);
            methodIL.Emit(OpCodes.Stloc_1);//从计算堆栈的顶部弹出当前值并将其存储到索引 1 处（idx）的局部变量列表中
            methodIL.Emit(OpCodes.Br, lblCompare);//Br,无条件地将控制转移到目标指令

            methodIL.MarkLabel(lblEnterLoop);//表示从下面开始进入循环体
            //sum += ints[idx];
            //其中Ldelem_I4用来加载一个数组中的Int32类型的元素
            methodIL.Emit(OpCodes.Ldloc_0);//将索引 0 处的局部变量加载到计算堆栈上
            methodIL.Emit(OpCodes.Ldarg_0);
            methodIL.Emit(OpCodes.Ldloc_1);
            methodIL.Emit(OpCodes.Ldelem_I4);
            methodIL.Emit(OpCodes.Add);
            methodIL.Emit(OpCodes.Stloc_0);
            //idx++
            methodIL.Emit(OpCodes.Ldloc_1);
            methodIL.Emit(OpCodes.Ldc_I4_1);//将整数值 1 作为 int32 推送到计算堆栈上
            methodIL.Emit(OpCodes.Add);
            methodIL.Emit(OpCodes.Stloc_1);

            //定义一个标签，表示从下面开始进入循环的比较
            methodIL.MarkLabel(lblCompare);
            //idx < ints.Length
            methodIL.Emit(OpCodes.Ldloc_1);
            methodIL.Emit(OpCodes.Ldarg_0);
            methodIL.Emit(OpCodes.Ldlen);//将从零开始的、一维数组的元素的数目推送到计算堆栈上
            methodIL.Emit(OpCodes.Conv_I4);//将位于计算堆栈顶部的值转换为 int32
            methodIL.Emit(OpCodes.Clt);//比较两个值。如果第一个值小于第二个值，则将整数值 1 (int32) 推送到计算堆栈上；反之，将 0 (int32) 推送到计算堆栈上。
            methodIL.Emit(OpCodes.Brtrue_S, lblEnterLoop);//如果 value 为 true、非空或非零，则将控制转移到目标指令（短格式）

            //return sum
            methodIL.Emit(OpCodes.Ldloc_0);
            methodIL.Emit(OpCodes.Ret);
        }

        static void MakeForeachMethod(TypeBuilder typeBuilder)
        {
            MethodBuilder methodBuilder = typeBuilder.DefineMethod("ForeachMethod", MethodAttributes.Public | MethodAttributes.Static, typeof(Int32), new Type[] { typeof(Int32[]) });
            ILGenerator methodIL = methodBuilder.GetILGenerator();

            //用来保存求和结果的局部变量
            LocalBuilder sum = methodIL.DeclareLocal(typeof(Int32));
            //foreach 中的 int i 
            LocalBuilder i = methodIL.DeclareLocal(typeof(Int32));
            //用来保存传入的数组
            LocalBuilder ints = methodIL.DeclareLocal(typeof(Int32[]));
            //数组循环用临时变量
            LocalBuilder index = methodIL.DeclareLocal(typeof(Int32));

            Label compareLabel = methodIL.DefineLabel();
            Label enterLoopLabel = methodIL.DefineLabel();

            //int sum = 0;
            methodIL.Emit(OpCodes.Ldc_I4_0);
            methodIL.Emit(OpCodes.Stloc_0);
            //ints = ints
            methodIL.Emit(OpCodes.Ldarg_0);
            methodIL.Emit(OpCodes.Stloc_2);
            //int index = 0
            methodIL.Emit(OpCodes.Ldc_I4_0);
            methodIL.Emit(OpCodes.Stloc_3);
            methodIL.Emit(OpCodes.Br, compareLabel);

            //定义一个标签，表示从下面开始进入循环体
            methodIL.MarkLabel(enterLoopLabel);
            //其中Ldelem_I4用来加载一个数组中的Int32类型的元素
            //加载 i = ints[index]
            methodIL.Emit(OpCodes.Ldloc_2);
            methodIL.Emit(OpCodes.Ldloc_3);
            methodIL.Emit(OpCodes.Ldelem_I4);
            methodIL.Emit(OpCodes.Stloc_1);
            //sum += i;
            methodIL.Emit(OpCodes.Ldloc_0);
            methodIL.Emit(OpCodes.Ldloc_1);
            methodIL.Emit(OpCodes.Add);
            methodIL.Emit(OpCodes.Stloc_0);

            //index++
            methodIL.Emit(OpCodes.Ldloc_3);
            methodIL.Emit(OpCodes.Ldc_I4_1);
            methodIL.Emit(OpCodes.Add);
            methodIL.Emit(OpCodes.Stloc_3);

            //定义一个标签，表示从下面开始进入循环的比较
            methodIL.MarkLabel(compareLabel);
            //index < ints.Length
            methodIL.Emit(OpCodes.Ldloc_3);
            methodIL.Emit(OpCodes.Ldloc_2);
            methodIL.Emit(OpCodes.Ldlen);
            methodIL.Emit(OpCodes.Conv_I4);
            methodIL.Emit(OpCodes.Clt);
            methodIL.Emit(OpCodes.Brtrue_S, enterLoopLabel);

            //return sum;
            methodIL.Emit(OpCodes.Ldloc_0);
            methodIL.Emit(OpCodes.Ret);
        }
    }
}
