using System.Collections.Generic;

namespace Apliu.WeChat.Core.Modal.Request
{
    public class ChatRoom
    {
        /// <summary>
        /// @@
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string EncryChatRoomId { get; set; }
    }

    public class BatchGetContactRequest
    {
        /// <summary>
        /// BaseRequest
        /// </summary>
        public BaseRequest BaseRequest { get; set; }
        /// <summary>
        /// Count
        /// </summary>
        public int Count { get { return List.Count; } }
        /// <summary>
        /// List
        /// </summary>
        public List<ChatRoom> List { get; set; }
    }

}
