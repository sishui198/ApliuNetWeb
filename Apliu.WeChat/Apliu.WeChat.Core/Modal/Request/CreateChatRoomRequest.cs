using System.Collections.Generic;

namespace Apliu.WeChat.Core.Modal.Request
{
    public class MemberItem
    {
        /// <summary>
        /// @
        /// </summary>
        public string UserName { get; set; }
    }
    class CreateChatRoomRequest
    {
        /// <summary>
        /// MemberCount
        /// </summary>
        public int MemberCount { get { return MemberList.Count; } }
        /// <summary>
        /// MemberList
        /// </summary>
        public List<MemberItem> MemberList { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Topic { get; set; }
        /// <summary>
        /// BaseRequest
        /// </summary>
        public BaseRequest BaseRequest { get; set; }
    }
}
