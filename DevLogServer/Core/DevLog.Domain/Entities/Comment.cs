using DevLog.Domain.Entities.Common;

namespace DevLog.Domain.Entities
{
    // bloglara yapılan yorumlar
    public class Comment: BaseEntity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Body { get; set; }
        public DateTime CommentDate { get; set; }
        // yoruma verilen cevaplar
        public virtual IList<SubComment> SubComments { get; set; }
        public Guid BlogId { get; set; }
        public virtual Blog Blog { get; set; }


    }
}
