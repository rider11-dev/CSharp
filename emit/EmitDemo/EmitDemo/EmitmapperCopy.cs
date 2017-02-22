using EmitMapper;
using EmitMapper.MappingConfiguration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmitDemo
{
    public class EmitmapperCopy
    {
        public class IUser
        {
            string name { get; set; }
            int age { get; set; }
        }

        public class User : IUser
        {
            public string name { get; set; }
            public int age { get; set; }

            public override string ToString()
            {
                return string.Format("User:{0},{1}", name, age);
            }
        }

        public class User1 : IUser
        {
            public string name { get; set; }
            public int age { get; set; }

            public override string ToString()
            {
                return string.Format("User1:{0},{1}", name, age);
            }
        }

        public static void Do()
        {
            var usrFrom = new User { name = "张鹏飞", age = 28 };
            var data = JsonConvert.SerializeObject(usrFrom);
            var jObj = JsonConvert.DeserializeObject(data) as JObject;
            User1 usrTo = new User1();
            Console.WriteLine(usrTo);
            //Map<User, User1>(usrFrom, usrTo);
            Map(typeof(User), typeof(User1), usrFrom, usrTo);
            Console.WriteLine(usrTo);

        }

        public static void Map<TFrom, TTo>(TFrom from, TTo to, IMappingConfigurator mappingConfigurator = null)
        {
            //EmitMapper内部有缓存机制，故不需要再维护Mapper的缓存
            var mapper = mappingConfigurator == null ?
                ObjectMapperManager.DefaultInstance.GetMapper<TFrom, TTo>() :
                ObjectMapperManager.DefaultInstance.GetMapper<TFrom, TTo>(mappingConfigurator);
            mapper.Map(from, to);
        }

        public static void Map(Type fromType, Type toType, object from, object to)
        {
            var mapper = ObjectMapperManager.DefaultInstance.GetMapperImpl(fromType, toType, new DefaultMapConfig());
            mapper.Map(from, to, null);
        }
    }
}
