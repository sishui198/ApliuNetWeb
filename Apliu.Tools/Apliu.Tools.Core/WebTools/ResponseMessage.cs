using Newtonsoft.Json;

namespace Apliu.Tools.Core.Web
{
    public class ResponseMessage
    {
        /// <summary>
        /// 返回码
        /// </summary>
        public int code { get; set; }
        /// <summary>
        /// 执行结果
        /// </summary>
        public string result { get; set; }
        /// <summary>
        /// 返回信息
        /// </summary>
        public string msg { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string remark { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}