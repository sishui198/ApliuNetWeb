using System;

namespace Apliu.WeChat.Core.Modal.Request
{
    public class UploadMediaRequest
    {
        /// <summary>
        /// UPLOAD_MEDIA_TYPE_IMAGE: 1,
        /// UPLOAD_MEDIA_TYPE_VIDEO: 2,
        /// UPLOAD_MEDIA_TYPE_AUDIO: 3,
        /// UPLOAD_MEDIA_TYPE_ATTACHMENT: 4,
        /// </summary>
        public int UploadType { get; set; }
        /// <summary>
        /// BaseRequest
        /// </summary>
        public BaseRequest BaseRequest { get; set; }
        /// <summary>
        /// ClientMediaId
        /// </summary>
        public Int64 ClientMediaId { get; set; }
        /// <summary>
        /// TotalLen
        /// </summary>
        public long TotalLen { get; set; }
        /// <summary>
        /// StartPos
        /// </summary>
        public int StartPos { get; set; }
        /// <summary>
        /// DataLen
        /// </summary>
        public long DataLen { get; set; }
        /// <summary>
        /// MediaType
        /// </summary>
        public int MediaType { get; set; }
        /// <summary>
        /// @
        /// </summary>
        public string FromUserName { get; set; }
        /// <summary>
        /// @
        /// </summary>
        public string ToUserName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string FileMd5 { get; set; }
    }
}
