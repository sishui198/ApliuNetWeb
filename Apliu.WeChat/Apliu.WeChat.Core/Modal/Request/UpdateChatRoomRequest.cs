namespace Apliu.WeChat.Core.Modal.Request
{

    class UpdateChatRoomRequest
    {
        /// <summary>
        /// @@
        /// </summary>
        public string ChatRoomName { get; set; }
        /// <summary>
        /// BaseRequest
        /// </summary>
        public BaseRequest BaseRequest { get; set; }
    }
    class AddMemberChatRoomRequest : UpdateChatRoomRequest
    {
        /// <summary>
        /// @
        /// </summary>
        public string AddMemberList { get; set; }
    }

    class DelMemberChatRoomRequest : UpdateChatRoomRequest
    {
        /// <summary>
        /// @
        /// </summary>
        public string DelMemberList { get; set; }
    }
}
