using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace EmitDemo
{
    public interface ICopyable
    {
        void CopyTo(object target);
    }

    public interface ITestModel : ICopyable
    {
        string username { get; set; }
        string pwd { get; set; }
    }

    public class TestModel : ITestModel
    {
        public string username { get; set; }
        public string pwd { get; set; }

        public void CopyTo(object target)
        {
            if (target == null)
            {
                return;
            }
            var ins = target as ITestModel;

            ins.username = this.username;
            ins.pwd = this.pwd;
        }
    }

    public class EmitInterfaceMethods
    {
        public static void Do()
        {
            string name = "EmitDemo.EmitInterfaceMethods";
            string asmFileName = name + ".dll";

            AssemblyName asmName = new AssemblyName(name);
            AssemblyBuilder assBuilder = AppDomain.CurrentDomain.DefineDynamicAssembly(asmName, AssemblyBuilderAccess.RunAndSave);
            ModuleBuilder moduleBuilder = assBuilder.DefineDynamicModule(name, asmFileName);
            Type iType = typeof(ITestModel);
            TypeBuilder typeBuilder = moduleBuilder.DefineType(name, TypeAttributes.Public, null);
            typeBuilder.AddInterfaceImplementation(iType);

            var props = typeof(ITestModel).GetProperties();
            foreach (var prop in props)
            {
                DefineProperty(typeBuilder, prop);
            }

            DefinCopyTo(typeBuilder, iType, props);

            Type newType = typeBuilder.CreateType();
            assBuilder.Save(asmFileName);

            var ins = Activator.CreateInstance(newType) as ITestModel;
            ins.username = "张鹏飞";
            ins.pwd = "6个a";

            var insTarget = Activator.CreateInstance(newType) as ITestModel;
            Console.WriteLine("insTarget,username:{0},pwd:{1}", insTarget.username, insTarget.pwd);
            ins.CopyTo(insTarget);

            Console.WriteLine("insTarget,username:{0},pwd:{1}", insTarget.username, insTarget.pwd);
        }

        private static void DefineProperty(TypeBuilder typeBuilder, PropertyInfo prop)
        {
            //定义私有字段
            FieldBuilder fldBuilder = typeBuilder.DefineField("_" + prop.Name, prop.PropertyType, FieldAttributes.Private);

            //定义属性
            PropertyBuilder propBuilder = typeBuilder.DefineProperty(prop.Name, PropertyAttributes.None, prop.PropertyType, null);
            //1、属性基本设置
            MethodAttributes getSetAttr = MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.HideBySig |
                MethodAttributes.Virtual | MethodAttributes.NewSlot | MethodAttributes.Final;
            //getter
            MethodBuilder getterBuilder = typeBuilder.DefineMethod("get_" + prop.Name, getSetAttr, prop.PropertyType, null);
            ILGenerator getterIL = getterBuilder.GetILGenerator();
            getterIL.Emit(OpCodes.Ldarg_0);
            getterIL.Emit(OpCodes.Ldfld, fldBuilder);//加载字段的值
            getterIL.Emit(OpCodes.Ret);
            //setter
            MethodBuilder setterBuilder = typeBuilder.DefineMethod("set_" + prop.Name, getSetAttr, null, new Type[] { prop.PropertyType });
            ILGenerator setterIL = setterBuilder.GetILGenerator();

            setterIL.Emit(OpCodes.Ldarg_0);
            setterIL.Emit(OpCodes.Ldarg_1);
            setterIL.Emit(OpCodes.Stfld, fldBuilder);//替换字段的值
            //调用父类方法，通知更改
            if (typeBuilder.BaseType == typeof(BaseModel))
            {
                setterIL.Emit(OpCodes.Ldarg_0);
                setterIL.Emit(OpCodes.Ldstr, prop.Name);
                setterIL.Emit(OpCodes.Call, typeof(BaseModel).GetMethod("RaisePropertyChanged"));
            }
            setterIL.Emit(OpCodes.Ret);
            propBuilder.SetGetMethod(getterBuilder);
            propBuilder.SetSetMethod(setterBuilder);
        }

        private static void DefinCopyTo(TypeBuilder typeBuilder, Type iType, PropertyInfo[] props)
        {
            var method = typeof(ICopyable).GetMethod("CopyTo");

            var copyParams = method.GetParameters();
            //实现接口方法时，MethodAttributes.Virtual不能少
            MethodBuilder copyMBuilder = typeBuilder.DefineMethod(method.Name,
                MethodAttributes.Public | MethodAttributes.Final | MethodAttributes.HideBySig | MethodAttributes.NewSlot | MethodAttributes.Virtual,
                CallingConventions.HasThis | CallingConventions.Standard,
                method.ReturnType,
                copyParams == null ? null : copyParams.Select(p => p.ParameterType).ToArray());
            ILGenerator copyIL = copyMBuilder.GetILGenerator();
            Label lblExit = copyIL.DefineLabel();
            Label lblCopy = copyIL.DefineLabel();
            LocalBuilder localIType = copyIL.DeclareLocal(iType);
            LocalBuilder isnull = copyIL.DeclareLocal(typeof(bool));

            //是否为空
            copyIL.Emit(OpCodes.Ldarg_1);
            copyIL.Emit(OpCodes.Ldnull);
            copyIL.Emit(OpCodes.Ceq);
            copyIL.Emit(OpCodes.Stloc_1);
            copyIL.Emit(OpCodes.Ldloc_1);
            copyIL.Emit(OpCodes.Brfalse_S, lblCopy);//不为空，跳转到copy主体
            copyIL.Emit(OpCodes.Br_S, lblExit);//为空，跳转到结束标记

            copyIL.MarkLabel(lblCopy);//标记开始复制操作
            copyIL.Emit(OpCodes.Ldarg_1);
            copyIL.Emit(OpCodes.Isinst, iType);
            copyIL.Emit(OpCodes.Stloc_0);
            if (props != null && props.Length > 0)
            {
                foreach (var prop in props)
                {
                    copyIL.Emit(OpCodes.Ldloc_0);
                    copyIL.Emit(OpCodes.Ldarg_0);
                    copyIL.Emit(OpCodes.Call, iType.GetMethod("get_" + prop.Name));
                    copyIL.Emit(OpCodes.Callvirt, iType.GetMethod("set_" + prop.Name, new Type[] { prop.PropertyType }));
                }
            }

            copyIL.MarkLabel(lblExit);//标记结束
            copyIL.Emit(OpCodes.Ret);

        }
    }
}
