using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace RstrntAPI.DataAccess.Massive
{
    // Massive's dynamic type is problematic in a strongly typed environment
    public static class Mapper
    {
        public static T ToEntity<T>(this ExpandoObject item) where T : new()
        {
            if (item == null)
                return default(T);

            var properties = typeof(T).GetProperties().ToDictionary(x => x.Name, x => x);

            var output = new T();

            foreach(var t in item)
            {
                PropertyInfo prop;
                if(properties.TryGetValue(t.Key, out prop))
                {
                    if (t.Value == null)
                        continue;

                    if (t.Value.GetType() == prop.PropertyType || 
                        t.Value.GetType() == Nullable.GetUnderlyingType(prop.PropertyType))
                        prop.SetValue(output, t.Value);
                    else if(t.Value.GetType().Name.Substring(0, 3) == (Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType).Name.Substring(0, 3)) // Hilariously horrible hack for 64vs32bit
                        if(Int32.MaxValue > (Int64)t.Value) // if it will cause a buffer overflow, just throw it out
                            prop.SetValue(output, Convert.ToInt32(t.Value));
                }
            }

            return output;
        }
    }
}
