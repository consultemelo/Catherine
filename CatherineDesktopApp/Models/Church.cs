using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CatherineDesktopApp.Data.Validation;
using CatherineDesktopApp.Services;
using CatherineDesktopApp.Shared;

namespace CatherineDesktopApp.Models
{
    // Church entity
    public class Church : IAuditableEntity, ISoftDeletableEntity
    {
        #region Keys
        [Key]
        public int ChurchId { get; set; }
        #endregion

        #region Data Fields
        [Required]
        [MaxLength(255)]
        public string ChurchName { get; set; }

        [Required]
        [MaxLength(255)]
        public string City { get; set; }

        [Required]
        [MaxLength(255)]
        public string Department { get; set; }

        [Required]
        [MaxLength(255)]
        public string Country { get; set; }
        #endregion

        #region Navigation Properties
        #endregion

        #region Soft Delete
        [Required]
        public bool IsDeleted { get; set; }
        #endregion

        #region Audit Fields
        [Required]
        public DateTime CreatedDate { get; set; }

        [Required]
        public DateTime ModifiedDate { get; set; }
        #endregion
    }

    public class ObservableChurch : LocalizedValidator, IAuditableEntity, ISoftDeletableEntity
    {
        private readonly Church _church;

        public Church Model => _church;

        public ObservableChurch(Church church, IResourceLocationService resourceService) : base(resourceService)
        {
            _church = church;
        }

        #region Computed Properties
        public string LocationDisplay => $"{City}, {Department}, {Country}";
        #endregion

        #region Keys
        [Key]
        public int ChurchId
        {
            get => _church.ChurchId;
            set => SetProperty(_church.ChurchId, value, _church, (c, i) => c.ChurchId = i, validate: true);
        }
        #endregion

        #region Data Fields
        [Required(AllowEmptyStrings = false, ErrorMessage = "ChurchChurchNameRequiredAttribute")]
        [MinLength(3, ErrorMessage = "ChurchChurchNameMinLengthAttribute")]
        [MaxLength(255, ErrorMessage = "ChurchChurchNameMaxLengthAttribute")]
        public string ChurchName
        {
            get => _church.ChurchName;
            set => SetProperty(_church.ChurchName, value, _church, (c, n) => c.ChurchName = n, validate: true);
        }

        [Required(AllowEmptyStrings = false, ErrorMessage = "ChurchCityRequiredAttribute")]
        [MinLength(3, ErrorMessage = "ChurchCityMinLengthAttribute")]
        [MaxLength(255, ErrorMessage = "ChurchCityMaxLengthAttribute")]
        public string City
        {
            get => _church.City;
            set => SetProperty(_church.City, value, _church, (c, n) => c.City = n, validate: true);
        }

        [Required(AllowEmptyStrings = false, ErrorMessage = "ChurchDepartmentRequiredAttribute")]
        [MinLength(3, ErrorMessage = "ChurchDepartmentMinLengthAttribute")]
        [MaxLength(255, ErrorMessage = "ChurchDepartmentMaxLengthAttribute")]
        public string Department
        {
            get => _church.Department;
            set => SetProperty(_church.Department, value, _church, (c, n) => c.Department = n, validate: true);
        }

        [Required(AllowEmptyStrings = false, ErrorMessage = "ChurchCountryRequiredAttribute")]
        [MinLength(3, ErrorMessage = "ChurchCountryMinLengthAttribute")]
        [MaxLength(255, ErrorMessage = "ChurchCountryMaxLengthAttribute")]
        public string Country
        {
            get => _church.Country;
            set => SetProperty(_church.Country, value, _church, (c, n) => c.Country = n, validate: true);
        }
        #endregion

        #region Soft Delete
        [Required]
        public bool IsDeleted
        {
            get => _church.IsDeleted;
            set => SetProperty(_church.IsDeleted, value, _church, (c, n) => c.IsDeleted = n, validate: true);
        }
        #endregion

        #region Audit Fields
        [Required]
        public DateTime CreatedDate
        {
            get => _church.CreatedDate;
            set => SetProperty(_church.CreatedDate, value, _church, (c, n) => c.CreatedDate = n, validate: true);
        }

        [Required]
        public DateTime ModifiedDate
        {
            get => _church.ModifiedDate;
            set => SetProperty(_church.ModifiedDate, value, _church, (c, n) => c.ModifiedDate = n, validate: true);
        }
        #endregion
    }
}
