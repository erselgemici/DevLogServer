using DevLog.Domain.Entities.Common;

namespace DevLog.Domain.Entities
{
    // yorumlara verilen cevaplar (alt yorum)
    public class SubComment: BaseEntity
    {
   
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Body { get; set; }
        public DateTime CommentDate { get; set; }

        public Guid CommentId { get; set; }
        public virtual Comment Comment { get; set; }
    }
}
