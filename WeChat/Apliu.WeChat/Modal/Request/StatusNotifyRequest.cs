﻿using System;

namespace Apliu.Standard.WeChat.Modal.Request
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
        /// @66b04b8cc2009212a4987adee710fd16
        /// </summary>
        public string FromUserName { get; set; }
        /// <summary>
        /// @66b04b8cc2009212a4987adee710fd16
        /// </summary>
        public string ToUserName { get; set; }
        /// <summary>
        /// ClientMsgId
        /// </summary>
        public Int64 ClientMsgId { get; set; }
    }
}
