namespace Apliu.WeChat.Core.Modal.Response
{
    public class UploadMediaResponse
    {
        /// <summary>
        /// BaseResponse
        /// </summary>
        public BaseResponse BaseResponse { get; set; }
        /// <summary>
        /// @
        /// </summary>
        public string MediaId { get; set; }
        /// <summary>
        /// 图片大小
        /// </summary>
        public int StartPos { get; set; }
        /// <summary>
        /// 缩略图高度，媒体文件会有，用来设置缩略图控件图片大小
        /// </summary>
        public int CDNThumbImgHeight { get; set; }
        /// <summary>
        /// 缩略图宽度，媒体文件会有，用来设置缩略图控件图片大小
        /// </summary>
        public int CDNThumbImgWidth { get; set; }
    }
}
