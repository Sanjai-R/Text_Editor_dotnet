namespace TextEditor.Models
{
    public class DocumentModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string UserId { get; set; } // Foreign key for user who created the document
        public virtual UserModel User { get; set; }
    }

}
