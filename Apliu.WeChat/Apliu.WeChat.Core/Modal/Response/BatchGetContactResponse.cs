using System.Collections.Generic;

namespace Apliu.WeChat.Core.Modal.Response
{
    public class BatchGetContactResponse
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
    }
}
