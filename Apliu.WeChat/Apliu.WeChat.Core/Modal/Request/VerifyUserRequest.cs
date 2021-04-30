using System.Collections.Generic;

namespace Apliu.WeChat.Core.Modal.Request
{
    public enum VERIFYUSER_OPCODE
    {
        VERIFYUSER_OPCODE_ADDCONTACT = 1,
        VERIFYUSER_OPCODE_SENDREQUEST = 2,
        VERIFYUSER_OPCODE_VERIFYOK = 3,
        VERIFYUSER_OPCODE_VERIFYREJECT = 4,
        VERIFYUSER_OPCODE_SENDERREPLY = 5,
        VERIFYUSER_OPCODE_RECVERREPLY = 6,
    }

    public enum ADDSCENE_PF
    {
        ADDSCENE_PF_QQ = 4,
        ADDSCENE_PF_EMAIL = 5,
        ADDSCENE_PF_CONTACT = 6,
        ADDSCENE_PF_WEIXIN = 7,
        ADDSCENE_PF_GROUP = 8,
        ADDSCENE_PF_UNKNOWN = 9,
        ADDSCENE_PF_MOBILE = 10,
        ADDSCENE_PF_WEB = 33,
    }

    public class VerifyUser
    {
        /// <summary>
        /// @
        /// </summary>
        public string Value { get; set; }
        /// <summary>
        /// @stranger
        /// </summary>
        public string VerifyUserTicket { get; set; }
    }

    public class VerifyUserRequest
    {
        /// <summary>
        /// BaseRequest
        /// </summary>
        public BaseRequest BaseRequest { get; set; }
        /// <summary>
        /// 操作码
        /// </summary>
        public VERIFYUSER_OPCODE Opcode { get; set; }
        /// <summary>
        /// VerifyUserListSize
        /// </summary>
        public int VerifyUserListSize
        {
            get
            {
                return VerifyUserList.Count;
            }
        }
        /// <summary>
        /// VerifyUserList
        /// </summary>
        public List<VerifyUser> VerifyUserList { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string VerifyContent { get; set; }
        /// <summary>
        /// SceneListCount
        /// </summary>
        public int SceneListCount
        {
            get
            {
                return SceneList.Count;
            }
        }
        /// <summary>
        /// 场景标识
        /// </summary>
        public List<int> SceneList { get; set; }
        /// <summary>
        /// @
        /// </summary>
        public string skey { get; set; }
    }
}
