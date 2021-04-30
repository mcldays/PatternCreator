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
        public string СhairmanName { get; set; }
        public string TeacherName { get; set; }
        public string SecretaryName { get; set; }
        public string СhairmanSignature { get; set; }
        public string TeacherSignature { get; set; }
        public string SecretarySignature { get; set; }
        public string License { get; set; }
        public string Stamp { get; set; }

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
        [Required]
        public string СhairmanName { get; set; }
        [Required]
        public string TeacherName { get; set; }
        [Required]
        public string SecretaryName { get; set; }
        public string СhairmanSignature { get; set; }
        public string TeacherSignature { get; set; }
        public string SecretarySignature { get; set; }
        public string Stamp { get; set; }

        public OrganizationViewModel(Organization model)
        {
            OrganizationId = model?.OrganizationId??0;
            OrganizationName = model?.OrganizationName;
            License = model?.License;
            TeacherName = model?.TeacherName;
            СhairmanName = model?.СhairmanName;
            SecretaryName = model?.SecretaryName;
            Stamp = model?.Stamp;
            TeacherSignature = model?.TeacherSignature;
            СhairmanSignature = model?.СhairmanSignature;
            SecretarySignature = model?.SecretarySignature;
        }

        public OrganizationViewModel()
        {
            
        }
    }
}