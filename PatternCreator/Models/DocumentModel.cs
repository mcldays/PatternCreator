using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace PatternCreator.Models
{
    public class DocumentModel
    {
        [Key]
        public int DocumentId { get; set; }
        public int PatternId { get; set; }
        [ForeignKey("PatternId")]
        public virtual PictureModel PictureModel { get; set; }
        public int UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual UserModel UserModel { get; set; }
        [Column(TypeName = "date")]
        public DateTime Date { get; set; }

    }

    public class DocumentSaveModel
    {
        public int DocumentId { get; set; }
        public string Date { get; set; }
    }
    public class DocumentViewModel
    {
        public DocumentViewModel(DocumentModel model)
        {
            DocumentId = model.DocumentId;
            PatternId = model.PatternId;
            UserId = model.UserId;
            Date = model.Date;
        }
        public int DocumentId { get; set; }
        public int PatternId { get; set; }
        
        public int UserId { get; set; }
        [Required]
        public DateTime Date { get; set; }
    }

    public class UserNameModel
    {
        public string FIO { get; set; }
        public List<DocumentViewModel> DocumentModels { get; set; }
    }
}