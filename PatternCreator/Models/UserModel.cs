using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PatternCreator.Models
{
    public class UserModel
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
        [Required]
        public string Name_DativeCase { get; set; }

        [Required]
        public string Surname { get; set; }
        [Required]
        public string Surname_DativeCase { get; set; }

        public string Patronymic { get; set; }
        public string Patronymic_DativeCase { get; set; }

        public string Position { get; set; }
        public string Education { get; set; }

        public virtual ICollection<DocumentModel> DocumentModels { get; set; }

        public int CompanyId { get; set; }

        public virtual CompanyModel CompanyModel { get; set; }

        public UserModel()
        {
            DocumentModels = new List<DocumentModel>();
        }

    }

    public class UserModelViewModel
    {
        public UserModelViewModel(UserModel model = null)
        {   
            if (model==null)
                return;
            Id = model.Id;
            Name = model.Name;
            Name_DativeCase = model.Name_DativeCase;
            Surname = model.Surname;
            Surname_DativeCase = model.Surname_DativeCase;
            Patronymic = model.Patronymic;
            Patronymic_DativeCase = model.Patronymic_DativeCase;
            CompanyId = model.CompanyId;
            Position = model.Position;
            Education = model.Education;
        }
        public UserModelViewModel() { }

        public int Id { get; set; }
        [Required(ErrorMessage = "Поле \"Имя\" не может быть пустым.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Поле \"Имя (Д.П.)\" не может быть пустым.")]
        public string Name_DativeCase { get; set; }
        [Required(ErrorMessage = "Поле \"Фамилия\" не может быть пустым.")]
        public string Surname { get; set; }
        [Required(ErrorMessage = "Поле \"Фамилия (Д.П.)\" не может быть пустым.")]
        public string Surname_DativeCase { get; set; }
        public string Patronymic { get; set; }
        public string Patronymic_DativeCase { get; set; }

        public string Position { get; set; }
        public string Education { get; set; }
        [Required(ErrorMessage = "Выберите компанию.")]
        [Range(0, Int32.MaxValue, ErrorMessage = "Выберите компанию.")]
        public int CompanyId { get; set; }
    }
}