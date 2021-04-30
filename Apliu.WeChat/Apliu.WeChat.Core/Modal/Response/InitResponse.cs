using System.Collections.Generic;

namespace Apliu.WeChat.Core.Modal.Response
{
    public class InitResponse
    {
        /// <summary>
        /// BaseResponse
        /// </summary>
        public BaseResponse BaseResponse { get; set; }
        /// <summary>
        /// Count
        /// </summary>
        public int Count { get; set; }
        /// <summary>
        /// ContactList
        /// </summary>
        public List<Contact> ContactList { get; set; }
        /// <summary>
        /// SyncKey
        /// </summary>
        public SyncKey SyncKey { get; set; }
        /// <summary>
        /// User
        /// </summary>
        public User User { get; set; }
        /// <summary>
        /// filehelper,@,@,
        /// </summary>
        public string ChatSet { get; set; }
        /// <summary>
        /// @
        /// </summary>
        public string SKey { get; set; }
        /// <summary>
        /// ClientVersion
        /// </summary>
        public int ClientVersion { get; set; }
        /// <summary>
        /// SystemTime
        /// </summary>
        public int SystemTime { get; set; }
        /// <summary>
        /// GrayScale
        /// </summary>
        public int GrayScale { get; set; }
        /// <summary>
        /// InviteStartCount
        /// </summary>
        public int InviteStartCount { get; set; }
        /// <summary>
        /// MPSubscribeMsgCount
        /// </summary>
        public int MPSubscribeMsgCount { get; set; }
        /// <summary>
        /// MPSubscribeMsgList
        /// </summary>
        public List<MPSubscribeMsg> MPSubscribeMsgList { get; set; }
        /// <summary>
        /// ClickReportInterval
        /// </summary>
        public int ClickReportInterval { get; set; }
    }

}
