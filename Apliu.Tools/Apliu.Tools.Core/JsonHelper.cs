using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Xml.Serialization;

namespace Apliu.Tools.Core
{
    public class JsonHelper
    {
        /// <summary>
        /// 将json转成url参数
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public static string JsonConvertUrl(string json)
        {
            string urlparams = string.Empty;
            JArray jo = (JArray)JsonConvert.DeserializeObject(json);
            foreach (JToken child in jo)
            {
                JProperty jPro = child as JProperty;
                if (!string.IsNullOrEmpty(urlparams)) urlparams += "&";
                urlparams += jPro.Name + "=" + jPro.Value;
            }
            return urlparams;
        }

        /// <summary>
        /// 将实体类转换成XML格式的字符串
        /// </summary>
        /// <typeparam name="EveryType"></typeparam>
        /// <param name="objType"></param>
        /// <returns></returns>
        public static String XmlSerialize<EveryType>(EveryType objType)
        {
            using (StringWriter sw = new StringWriter())
            {
                XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
                //ns.Add("", "");
                XmlSerializer serializer = new XmlSerializer(objType.GetType());
                serializer.Serialize(sw, objType, ns);
                sw.Close();
                String strXml = sw.ToString();
                return strXml;
            }
        }

        /// <summary>
        /// 将XML格式的字符串转换换成实体类
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="strXML"></param>
        /// <returns></returns>
        public static EveryType XmlDeserialize<EveryType>(String strXML) where EveryType : class
        {
            try
            {
                using (StringReader sr = new StringReader(strXML))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(EveryType));
                    return serializer.Deserialize(sr) as EveryType;
                }
            }
            catch (Exception)
            {
                return default(EveryType);
            }
        }

        /// <summary>
        /// Json序列化
        /// </summary>
        /// <typeparam name="EveryType"></typeparam>
        /// <param name="objType"></param>
        /// <returns></returns>
        public static String JsonSerialize<EveryType>(EveryType objType)
        {
            JsonSerializerSettings setting = new JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };
            string Json = JsonConvert.SerializeObject(objType, setting);
            return Json;
        }

        /// <summary>
        /// Json反序列化
        /// </summary>
        /// <typeparam name="EveryType"></typeparam>
        /// <param name="strJson"></param>
        /// <returns></returns>
        public static EveryType JsonDeserialize<EveryType>(String strJson)
        {
            return JsonConvert.DeserializeObject<EveryType>(strJson);
        }
        /// <summary>
        /// Json数组转实体类
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="strJson"></param>
        /// <returns></returns>
        public static List<T> JsonDeserializeList<T>(string strJson)
        {
            List<T> objs = JsonConvert.DeserializeObject<List<T>>(strJson);
            return objs;
        }

        /// <summary>  
        /// 把对象序列化为字节数组  
        /// </summary>  
        public static byte[] SerializeObject(object obj)
        {
            if (obj == null)
                return null;
            var str = JsonSerialize(obj);
            return Encoding.Default.GetBytes(str);
        }

        /// <summary>  
        /// 把字节数组反序列化成对象  
        /// </summary>  
        public static T DeserializeObject<T>(byte[] bytes)
        {
            if (bytes == null)
                return default(T);

            var str = Encoding.Default.GetString(bytes);
            return JsonDeserialize<T>(str);
        }
    }
}
