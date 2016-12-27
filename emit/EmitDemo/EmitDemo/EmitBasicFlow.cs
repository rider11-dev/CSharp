using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace EmitDemo
{
    public class EmitBasicFlow
    {
        public static void Do()
        {
            string name = "EmitDemo.EmitBasicFlow";
            //string fileName = AppDomain.CurrentDomain.BaseDirectory.TrimEnd('\\', '/') + "/" + name + ".dll";
            string asmFileName = name + ".dll";
            //1、构建程序集
            AssemblyName assName = new AssemblyName(name);
            AssemblyBuilder assBuilder = AppDomain.CurrentDomain.DefineDynamicAssembly(assName, AssemblyBuilderAccess.RunAndSave);

            //2、定义模块
            ModuleBuilder moduleBuilder = assBuilder.DefineDynamicModule(name, asmFileName);

            //3、创建动态类型，EmitDemo.Fibonacci是命名空间，Fibonacci是类名
            TypeBuilder typeBuilder = moduleBuilder.DefineType(name + ".Fibonacci", TypeAttributes.Public);

            //4、定义方法
            /*  Fibonacci数列计算逻辑
             *  1) 如果传入的参数是1，跳转到第六步执行；
                   
                2) 如果传入的参数是2，跳转到第六步执行；
                   
                3) 将传入的参数减1，然后递归调用自身；
                   
                4) 将传入的参数减2，然后递归调用自身；
                   
                5) 将递归调用的结果相加，跳转到第七步执行；
                   
                6) 设置堆栈顶的值为1；
                   
                7) 返回堆栈顶的元素作为结果。
             *  
             */
            //DefineMethod参数：方法名称，修饰符，返回值类型传入参数的类型数组
            MethodBuilder methodBuilder = typeBuilder.DefineMethod("Calc", MethodAttributes.Public, typeof(Int32), new Type[] { typeof(Int32) });

            //5、实现方法
            ILGenerator calcIL = methodBuilder.GetILGenerator();
            //定义标签lblReturn1，用来设置返回值为1
            Label lblReturn1 = calcIL.DefineLabel();
            //定义标签lblFinalResult1，用来返回最终结果
            Label lblFinalResult1 = calcIL.DefineLabel();

            // 1）加载参数1，和整数1，相比较，如果相等则设置返回值为1
            calcIL.Emit(OpCodes.Ldarg_1);
            calcIL.Emit(OpCodes.Ldc_I4_1);
            calcIL.Emit(OpCodes.Beq_S, lblReturn1);
            // 2）加载参数2，和整数2比较，如果相等则设置返回值为1
            calcIL.Emit(OpCodes.Ldarg_1);
            calcIL.Emit(OpCodes.Ldc_I4_2);
            calcIL.Emit(OpCodes.Beq_S, lblReturn1);
            // 3）加载参数0和1，将参数1减去1，递归调用自身
            calcIL.Emit(OpCodes.Ldarg_0);
            calcIL.Emit(OpCodes.Ldarg_1);
            calcIL.Emit(OpCodes.Ldc_I4_1);
            calcIL.Emit(OpCodes.Sub);
            calcIL.Emit(OpCodes.Call, methodBuilder);
            // 4）加载参数0和1，将参数1减去2，递归调用自身
            calcIL.Emit(OpCodes.Ldarg_0);
            calcIL.Emit(OpCodes.Ldarg_1);
            calcIL.Emit(OpCodes.Ldc_I4_2);
            calcIL.Emit(OpCodes.Sub);
            calcIL.Emit(OpCodes.Call, methodBuilder);
            // 5）将递归调用的结果相加，并返回
            calcIL.Emit(OpCodes.Add);
            calcIL.Emit(OpCodes.Br, lblFinalResult1);
            // 6）处理标签
            calcIL.MarkLabel(lblReturn1);
            calcIL.Emit(OpCodes.Ldc_I4_1);
            calcIL.MarkLabel(lblFinalResult1);
            calcIL.Emit(OpCodes.Ret);

            //6、创建类型
            Type type = typeBuilder.CreateType();
            assBuilder.Save(asmFileName);

            object obj = Activator.CreateInstance(type);
            for (var idx = 1; idx < 10; idx++)
            {
                Console.WriteLine(type.GetMethod("Calc").Invoke(obj, new object[] { idx }));
            }
        }
    }
}
