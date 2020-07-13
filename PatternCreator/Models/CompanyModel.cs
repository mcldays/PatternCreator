using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PatternCreator.Models
{
    public class CompanyModel
    {
        [Key]
        public int CompanyId { get; set; }
        [Required]
        public string CompanyName { get; set; }

        public virtual ICollection<UserModel> UserModels {get; set; }

        public CompanyModel() => UserModels = new List<UserModel>();
    }
    public class CompanyViewModel
    {
        public int CompanyId { get; set; }
        [Required]
        public string CompanyName { get; set; }

        public IEnumerable<UserModelViewModel> UserViewModels { get; set; }

    }
    public class CreateCompanyModel
    {
        [Required(ErrorMessage = "Введите имя компании")]
        public string CompanyName { get; set; }
    }
}