using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace EmitDemo
{
    public class EmitCtorInvoke
    {
        public static void Do()
        {
            string name = "EmitDemo.CtorInvoke";
            string asmFileName = name + ".dll";

            AssemblyName asmName = new AssemblyName(name);
            AssemblyBuilder assBuilder = AppDomain.CurrentDomain.DefineDynamicAssembly(asmName, AssemblyBuilderAccess.RunAndSave);
            ModuleBuilder moduleBuilder = assBuilder.DefineDynamicModule(name, asmFileName);
            TypeBuilder typeBuilder = moduleBuilder.DefineType(name, TypeAttributes.Public, typeof(TargetMethods.Base));

            ConstructorBuilder ctorBuilder = null;
            var pCtors = typeof(TargetMethods.Base).GetConstructors(BindingFlags.Public | BindingFlags.Instance);
            foreach (var pCtor in pCtors)
            {
                var pCtorParams = pCtor.GetParameters();
                if (pCtorParams == null || pCtorParams.Length < 1)
                {
                    //忽略0参数构造函数，因为默认已包含
                    continue;
                }
                var paramTypes = pCtorParams.Select(p => p.ParameterType).ToArray();
                ctorBuilder = typeBuilder.DefineConstructor(pCtor.Attributes, pCtor.CallingConvention, paramTypes);
                ILGenerator ctorIL = ctorBuilder.GetILGenerator();
                ctorIL.Emit(OpCodes.Ldarg_0);
                ctorIL.Emit(OpCodes.Ldarg_1);//加载第一个参数
                if (pCtorParams.Length >= 2)
                {
                    ctorIL.Emit(OpCodes.Ldarg_2);
                }
                if (pCtorParams.Length >= 3)
                {
                    ctorIL.Emit(OpCodes.Ldarg_3);
                }
                //The ldarg.s instruction is an efficient encoding for loading arguments indexed from 4 through 255.
                //https://msdn.microsoft.com/en-us/library/system.reflection.emit.opcodes.ldarg_s.aspx
                if (pCtorParams.Length >= 4)//第4个以及以后的参数
                {
                    for (int idx = 3; idx < pCtorParams.Length; idx++)
                    {
                        ctorIL.Emit(OpCodes.Ldarg_S, idx + 1);
                    }
                }
                ctorIL.Emit(OpCodes.Call, pCtor);
                ctorIL.Emit(OpCodes.Ret);
            }

            MethodBuilder copyMBuilder = typeBuilder.DefineMethod("test", MethodAttributes.Public, null, Type.EmptyTypes);
            ILGenerator copyIL = copyMBuilder.GetILGenerator();
            Label lblExit = copyIL.DefineLabel();
            Label lblCopy = copyIL.DefineLabel();
            copyIL.Emit(OpCodes.Ldarg_1);
            copyIL.Emit(OpCodes.Ldnull);
            copyIL.Emit(OpCodes.Ceq);
            copyIL.Emit(OpCodes.Stloc_1);
            copyIL.Emit(OpCodes.Ldloc_1);
            copyIL.Emit(OpCodes.Brfalse_S, lblCopy);//不为空，跳转到copy主体
            copyIL.Emit(OpCodes.Br_S, lblExit);//为空，跳转到结束标记
            copyIL.MarkLabel(lblCopy);//标记开始复制操作


            copyIL.MarkLabel(lblExit);//标记结束
            copyIL.Emit(OpCodes.Ret);


            Type type = typeBuilder.CreateType();
            assBuilder.Save(asmFileName);

            var ins = Activator.CreateInstance(type, "张鹏飞", true, 27, DateTime.Now) as TargetMethods.Base;

            Console.WriteLine(ins.ToString());

        }
    }
}
