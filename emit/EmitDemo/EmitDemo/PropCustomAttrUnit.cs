using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmitDemo
{
    public class PropCustomAttrUnit
    {
        public string prop_name { get; set; }
        public IList<PropCustomAttr> attrs { get; set; }
        public bool HasAttr
        {
            get { return attrs != null && attrs.Count > 0; }
        }
        public bool check(out string msg)
        {
            msg = "";
            //1、prop_name是否为空
            if (string.IsNullOrEmpty(prop_name))
            {
                msg = "属性名称prop_name不能为空";
                return false;
            }
            //2、attrs校验
            if (HasAttr)
            {
                foreach (var attr in attrs)
                {
                    if (!attr.check(out msg))
                    {
                        msg = string.Format("属性：{0}，错误：{1}", prop_name, msg);
                        return false;
                    }
                }
            }
            return true;
        }
    }

    public class PropCustomAttr
    {
        public string attr_type { get; set; }
        public string ctor_arg_types { get; set; }
        public object[] ctor_arg_values { get; set; }
        public string error_msg { get; set; }
        public string error_msg_res_name { get; set; }
        public string error_msg_res_type { get; set; }

        public bool check(out string msg)
        {
            msg = "";
            //1、特性类型校验
            if (string.IsNullOrEmpty(this.attr_type))
            {
                msg = "特性类型attrtype不能为空";
                return false;
            }
            var attrtype = Type.GetType(this.attr_type);
            if (attrtype == null)
            {
                msg = string.Format("未识别的特性类型：{0}", this.attr_type);
                return false;
            }
            //2、构造函数参数校验
            if (!string.IsNullOrWhiteSpace(ctor_arg_types))
            {
                var types = ctor_arg_types.Split(',');
                if (types != null && types.Length > 0)
                {
                    //构造函数参数类型是否都合法
                    bool rst = types.ToList().Exists(s => Type.GetType(s) == null);
                    if (rst == true)
                    {
                        msg = "未识别的特性构造函数参数类型（ctor_arg_types）:" + ctor_arg_types;
                        return false;
                    }
                    //构造函数参数值数量是否与类型数量一致
                    if (ctor_arg_values == null || ctor_arg_values.Length != types.Length)
                    {
                        msg = "构造函数参数值（ctor_arg_values）数量必须与参数类型（ctor_arg_types）数量一致";
                        return false;
                    }
                }
            }
            //3、error设置校验——error_msgres_type
            if (!string.IsNullOrWhiteSpace(error_msg_res_type))
            {
                var restype = Type.GetType(error_msg_res_type);
                if (restype == null)
                {
                    msg = string.Format("未识别的特性消息来源类型（error_msg_res_type）：" + error_msg_res_type);
                    return false;
                }
            }

            return true;
        }
    }
}
