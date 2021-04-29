using System;
using System.Collections.Generic;
using System.Linq;

namespace Apliu.Database.Core.Extensions
{
    public static class ObjectExtension
    {
        /// <summary>
        /// 获取属性值（仅首层属性）
        /// </summary>
        /// <param name="obj">对象</param>
        /// <returns></returns>
        public static Dictionary<string, object> GetPropertyValues(this object obj)
        {
            if (obj == null)
                return null;
            if (obj is Dictionary<string, object>)
                return obj as Dictionary<string, object>;

            var ps = obj.GetType().GetProperties();
            var vs = ps.ToDictionary(u => u.Name, u => u.GetValue(obj));
            return vs;
        }
    }
}
