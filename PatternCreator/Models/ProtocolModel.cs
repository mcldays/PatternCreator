using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace PatternCreator.Models
{
    public class ProtocolModel
    {
        
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string ProtocolName { get; set; }
        
        public virtual ICollection<DocumentModel> DocumentModels { get; set; }
        
        public ProtocolModel()
        {
            DocumentModels = new List<DocumentModel>();
        }
    }
}