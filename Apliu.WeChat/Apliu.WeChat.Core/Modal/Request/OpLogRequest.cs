namespace Apliu.WeChat.Core.Modal.Request
{
    public enum CmdIdType
    {
        TOPCONTACT = 3,
        MODREMARKNAME = 2
    }
    class OpLogRequest
    {
        /// <summary>
        /// @
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// TOPCONTACT: 3,
        /// MODREMARKNAME: 2
        /// </summary>
        public CmdIdType CmdId { get; set; }
        /// <summary>
        /// Leestar
        /// </summary>
        public string RemarkName { get; set; }
        /// <summary>
        /// BaseRequest
        /// </summary>
        public BaseRequest BaseRequest { get; set; }
    }

    class TopOpLogRequest : OpLogRequest
    {
        public int OP { get; set; }
    }

}
