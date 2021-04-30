using System.Collections.Generic;

namespace Apliu.WeChat.Core.Modal.Response
{
    public class CreateChatRoomResponse
    {/// <summary>
     /// BaseResponse
     /// </summary>
        public BaseResponse BaseResponse { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Topic { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string PYInitial { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string QuanPin { get; set; }
        /// <summary>
        /// MemberCount
        /// </summary>
        public int MemberCount { get; set; }
        /// <summary>
        /// MemberList
        /// </summary>
        public List<Member> MemberList { get; set; }
        /// <summary>
        /// @@
        /// </summary>
        public string ChatRoomName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string BlackList { get; set; }
    }
}
