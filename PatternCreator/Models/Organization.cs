using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PatternCreator.Models
{
    public class Organization
    {
        public int OrganizationId { get; set; }
        public string OrganizationName { get; set; }
        public string License { get; set; }
        public virtual ICollection<DocumentModel> Documents { get; set; }

        public Organization()
        {
            Documents = new List<DocumentModel>();
        }
    }
    public class OrganizationViewModel
    {
        public int OrganizationId { get; set; }
        [Required]
        public string OrganizationName { get; set; }
        [Required]
        public string License { get; set; }
        

        public OrganizationViewModel(Organization model)
        {
            OrganizationId = model?.OrganizationId??0;
            OrganizationName = model?.OrganizationName;
            License = model?.License;
        }

        public OrganizationViewModel()
        {
            
        }
    }
}