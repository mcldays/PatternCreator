using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing;
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

        [Column(TypeName = "date")]
        public DateTime StartDate { get; set; }

        public int OrganizationId { get; set; }
        public virtual Organization Organization { get; set; }
        
        public int BlankNumber { get; set; }
        public string ProtocolName { get; set; }
        public virtual ProtocolModel ProtocolModel { get; set; }

        public int SpecialtyId { get; set; }
        [ForeignKey("SpecialtyId")]
        public virtual SpecialtyModel SpecialtyModel { get; set; }
        public string EducationTime { get; set; }

        public string HandWriteFields { get; set; }
        public string CertificationWork { get; set; }

    }
    public class DocumentSaveModel
    {
        public int DocumentId { get; set; }
        public string Date { get; set; }
        public string StartDate { get; set; }

    }

    public class UserDocs
    {
        public UserDocs(DocumentModel model)
        {
            DocumentId = model.DocumentId;
            PatternName = model.PictureModel.Name;
            ProtocolName = model.ProtocolName;
            Prefix = model.SpecialtyModel.Prefix;
            PatternId = model.PatternId;
        }
        public int DocumentId { get; set; }
        public int PatternId { get; set; }
        public string PatternName { get; set; }
        public string ProtocolName { get; set; }
        public string Prefix { get; set; }


    }
    public class DocumentViewModel
    {
        public DocumentViewModel(DocumentModel model)
        {
            DocumentId = model.DocumentId;
            PatternId = model.PatternId;
            UserId = model.UserId;
            Date = model.Date;
            StartDate = model.StartDate;
        }
        public int DocumentId { get; set; }
        public int PatternId { get; set; }
        
        public int UserId { get; set; }
        [Required]
        public DateTime Date { get; set; }
        [Required]
        public DateTime StartDate { get; set; }
    }
    public class DocumentPrintViewModel
    {
        public DocumentPrintViewModel()
        {
            Items = new List<ItemModel>();
            Stamps = new List<Stamp>();
            Photos = new List<Stamp>();
            Images = new List<StaticImageViewModel>();
        }
        public TypeDoc Identity { get; set; }

        public int NaturalWidth { get; set; }
        public int NaturalHeight { get; set; }
        public int Image { get; set; }
        public List<ItemModel> Items { get; set; }
        public List<Stamp> Stamps { get; set; }
        public List<Stamp> Photos { get; set; }
        public List<StaticImageViewModel> Images { get; set; }
        public int DocId { get; set; }
        public static int[] Patterns = {45};
        public enum TypeDoc
        {
            WorkIdentity,
            WorkSecurity,
            WorkHighSecurity,
            WorkHighSecurityWithQuality,
            Ptm,
            Pplam,
            Other
        }
}
    public class ItemModel
    {
        public RectangleF Rectf { get; set; }
        public StringFormat Format { get; set; }
        public FontStyle Weight { get; set; }
        public int FontSize { get; set; }
        public string Text { get; set; }
        public bool Underline { get; set; }
    }
    public class Stamp
    {
        public RectangleF Rectf { get; set; }
        public string OrgPath { get; set; }
        public int UserId { get; set; }
    }
    public class UserNameModel
    {
        public string FIO { get; set; }
        public List<DocumentViewModel> DocumentModels { get; set; }
    }
    public class GetDocumentsViewModel
    {
        public int[] Templates { get; set; }
        public int[] Companies { get; set; }
        public int[] Users { get; set; }

    }
    public class ArchiveDocumentViewModel
    {
        public int DocumentId { get; set; }
        public string ProtocolName { get; set; }
        public string UserFIO { get; set; }
        public string CompanyName { get; set; }
        public string PictureName { get; set; }

        public DateTime Date { get; set; }
        public ArchiveDocumentViewModel(DocumentModel model)
        {
            DocumentId = model.DocumentId;
            ProtocolName = model.ProtocolName;
            PictureName = model.PictureModel.Name;
            UserFIO = model.UserModel.Surname + " " + model.UserModel.Name[0] + ". " +
                      (string.IsNullOrEmpty(model.UserModel.Patronymic)
                          ? ""
                          : model.UserModel.Patronymic[0] + ".");
            CompanyName = model.UserModel.CompanyModel.CompanyName;
            Date = model.Date;
        }
    }
}