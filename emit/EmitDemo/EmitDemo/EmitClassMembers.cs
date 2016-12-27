using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace EmitDemo
{
    public class EmitClassMembers
    {
        public static void Do()
        {
            string name = "EmitDemo.EmitClassMembers";
            //string fileName = AppDomain.CurrentDomain.BaseDirectory.TrimEnd('\\', '/') + "/" + name + ".dll";
            string asmFileName = name + ".dll";
            //1、构建程序集
            AssemblyName assName = new AssemblyName(name);
            AssemblyBuilder assBuilder = AppDomain.CurrentDomain.DefineDynamicAssembly(assName, AssemblyBuilderAccess.RunAndSave);
            //2、定义模块
            ModuleBuilder moduleBuilder = assBuilder.DefineDynamicModule(name, asmFileName);
            //3、定义类型
            TypeBuilder typeBuilder = moduleBuilder.DefineType(name + ".Add", TypeAttributes.Public);

            //4、定义私有字段
            FieldBuilder fieldABuilder = typeBuilder.DefineField("_a", typeof(Int32), FieldAttributes.Private);
            FieldBuilder fieldBBuilder = typeBuilder.DefineField("_b", typeof(Int32), FieldAttributes.Private);
            fieldABuilder.SetConstant(0);
            fieldBBuilder.SetConstant(0);

            //5、定义属性
            //A
            PropertyBuilder propABuilder = typeBuilder.DefineProperty("A", PropertyAttributes.None, typeof(Int32), null);
            //A——getter
            MethodBuilder propAGetterBuilder = typeBuilder.DefineMethod("get", MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.HideBySig,
                typeof(Int32), Type.EmptyTypes);
            ILGenerator getterAIL = propAGetterBuilder.GetILGenerator();
            getterAIL.Emit(OpCodes.Ldarg_0);
            getterAIL.Emit(OpCodes.Ldfld, fieldABuilder);//查找字段A的值
            getterAIL.Emit(OpCodes.Ret);
            //A——setter
            MethodBuilder propASetterBuilder = typeBuilder.DefineMethod("set", MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.HideBySig,
                null, new Type[] { typeof(Int32) });
            ILGenerator setterAIL = propASetterBuilder.GetILGenerator();
            setterAIL.Emit(OpCodes.Ldarg_0);
            setterAIL.Emit(OpCodes.Ldarg_1);
            setterAIL.Emit(OpCodes.Stfld, fieldABuilder);//替换字段的值
            setterAIL.Emit(OpCodes.Ret);

            propABuilder.SetGetMethod(propAGetterBuilder);
            propABuilder.SetSetMethod(propASetterBuilder);

            //B
            PropertyBuilder propBBuilder = typeBuilder.DefineProperty("B", PropertyAttributes.None, typeof(Int32), null);
            //B——getter
            MethodBuilder propBGetterBuilder = typeBuilder.DefineMethod("get", MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.HideBySig,
                typeof(Int32), Type.EmptyTypes);
            ILGenerator getterBIL = propBGetterBuilder.GetILGenerator();
            getterBIL.Emit(OpCodes.Ldarg_0);
            getterBIL.Emit(OpCodes.Ldfld, fieldBBuilder);//查找字段B的值
            getterBIL.Emit(OpCodes.Ret);
            //B——setter
            MethodBuilder propBSetterBuilder = typeBuilder.DefineMethod("set", MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.HideBySig,
                typeof(Int32), new Type[] { typeof(Int32) });
            ILGenerator setterBIL = propBSetterBuilder.GetILGenerator();
            setterBIL.Emit(OpCodes.Ldarg_0);
            setterBIL.Emit(OpCodes.Ldarg_1);
            setterBIL.Emit(OpCodes.Stfld, fieldBBuilder);
            setterBIL.Emit(OpCodes.Ret);

            propBBuilder.SetGetMethod(propBGetterBuilder);
            propBBuilder.SetSetMethod(propBSetterBuilder);

            //6、定义构造函数,CallingConventions.HasThis代表实例方法
            ConstructorBuilder ctorBuilder = typeBuilder.DefineConstructor(MethodAttributes.Public, CallingConventions.HasThis, new Type[] { typeof(Int32), typeof(Int32) });
            ILGenerator ctorIL = ctorBuilder.GetILGenerator();
            //加载参数1，填充到私有字段_a
            ctorIL.Emit(OpCodes.Ldarg_0);
            ctorIL.Emit(OpCodes.Ldarg_1);
            ctorIL.Emit(OpCodes.Stfld, fieldABuilder);
            //加载参数2，填充到私有字段_b
            ctorIL.Emit(OpCodes.Ldarg_0);
            ctorIL.Emit(OpCodes.Ldarg_2);
            ctorIL.Emit(OpCodes.Stfld, fieldBBuilder);
            ctorIL.Emit(OpCodes.Ret);

            //7、定义方法
            MethodBuilder calcMethodBuilder = typeBuilder.DefineMethod("Calc", MethodAttributes.Public, typeof(Int32), Type.EmptyTypes);
            ILGenerator calcIL = calcMethodBuilder.GetILGenerator();
            calcIL.Emit(OpCodes.Ldarg_0);
            calcIL.Emit(OpCodes.Ldfld, fieldABuilder);
            calcIL.Emit(OpCodes.Ldarg_0);
            calcIL.Emit(OpCodes.Ldfld, fieldBBuilder);
            calcIL.Emit(OpCodes.Add);
            calcIL.Emit(OpCodes.Ret);

            Type type = typeBuilder.CreateType();
            assBuilder.Save(asmFileName);
            Console.WriteLine("please input A:");
            var A = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("please input B:");
            var B = Convert.ToInt32(Console.ReadLine());
            object obj = Activator.CreateInstance(type, A, B);
            var sum = type.GetMethod("Calc").Invoke(obj, null);
            Console.WriteLine("the result of {0} + {1} is {2}", A, B, sum);
        }
    }
}
