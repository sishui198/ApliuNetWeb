using System;

namespace Apliu.WeChat.Core.Modal.Request
{
    public class StatusNotifyRequest
    {
        /// <summary>
        /// BaseRequest
        /// </summary>
        public BaseRequest BaseRequest { get; set; }
        /// <summary>
        /// Code
        /// </summary>
        public int Code { get; set; }
        /// <summary>
        /// @
        /// </summary>
        public string FromUserName { get; set; }
        /// <summary>
        /// @
        /// </summary>
        public string ToUserName { get; set; }
        /// <summary>
        /// ClientMsgId
        /// </summary>
        public Int64 ClientMsgId { get; set; }
    }
}
