namespace Project1.Areas.Community.DTO
{
    public class CommentDTO
    {
        public Int32 idx { get; set; }
        public String? BoardCatCls { get; set; }
        public Int32? BoardIdx { get; set; }
        public Int32? ParentIdx { get; set; }
        public Int32? C_order { get; set; }
        public Int32? C_depth { get; set; }
        public String? C_content { get; set; }
        public String? C_author { get; set; }
        public DateTimeOffset? EnterDateTime { get; set; }
        public String? EnterUser { get; set; }
        public Int32 C_report { get; set; } = 0;
    }
}
