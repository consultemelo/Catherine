using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using CatherineDesktopApp.Data.Validation;
using CatherineDesktopApp.Models;
using CatherineDesktopApp.Services;
using CatherineDesktopApp.Shared;
using CommunityToolkit.Mvvm.ComponentModel;

namespace CatherineDesktopApp.Models
{
    public enum PriestTitle
    {
        [ResourceDescription($"{nameof(PriestTitle)}{nameof(PriestTitle.Deacon)}")]
        Deacon,

        [ResourceDescription($"{nameof(PriestTitle)}{nameof(PriestTitle.Fr)}")]
        Fr,

        [ResourceDescription($"{nameof(PriestTitle)}{nameof(PriestTitle.Bp)}")]
        Bp
    }

    public enum PriestPosition
    {
        [ResourceDescription($"{nameof(PriestPosition)}{nameof(PriestPosition.ParochialVicar)}")]
        ParochialVicar,

        [ResourceDescription($"{nameof(PriestPosition)}{nameof(PriestPosition.Chaplain)}")]
        Chaplain,

        [ResourceDescription($"{nameof(PriestPosition)}{nameof(PriestPosition.Administrator)}")]
        Administrator,

        [ResourceDescription($"{nameof(PriestPosition)}{nameof(PriestPosition.ParishPriest)}")]
        ParishPriest,

        [ResourceDescription($"{nameof(PriestPosition)}{nameof(PriestPosition.Retired)}")]
        Retired
    }


    public class Priest : IEquatable<Priest>, IAuditableEntity, ISoftDeletableEntity
    {
        public Priest()
        {
            Title = PriestTitle.Deacon;
            Position = PriestPosition.ParochialVicar;
        }

        #region Keys
        [Key]
        public int PriestId { get; set; }
        #endregion

        #region Data Fields
        [Required]
        public PriestTitle Title { get; set; }

        [Required]
        public PriestPosition Position { get; set; }

        [Required]
        [MaxLength(Constants.PriestFirstNamesMaxLengthAttribute)]
        public string FirstNames { get; set; }

        [Required]
        [MaxLength(Constants.PriestLastNamesMaxLengthAttribute)]
        public string LastNames { get; set; }
        #endregion

        #region Navigation Properties

        #endregion

        #region Audit Fields
        [Required]
        public DateTime CreatedDate { get; set; }

        [Required]
        public DateTime ModifiedDate { get; set; }
        #endregion

        #region Soft Delete
        [Required]
        public bool IsDeleted { get; set; }
        #endregion

        #region Equality
        public bool Equals(Priest other)
        {
            if (other is null) return false;

            return FirstNames == other.FirstNames &&
                LastNames == other.LastNames &&
                Title == other.Title &&
                Position == other.Position &&
                CreatedDate == other.CreatedDate;
        }
        #endregion
    }

    public class ObservablePriest : LocalizedValidator, ISoftDeletableEntity, IAuditableEntity
    {
        private readonly Priest _priest;

        public Priest Model => _priest;


        public ObservablePriest(Priest priest, IResourceLocationService resourceService) : base(resourceService)
        {
            _priest = priest;
        }

        #region Keys
        public int PriestId
        {
            get => _priest.PriestId;
            set => SetProperty(_priest.PriestId, value, _priest, (p, i) => p.PriestId = i, validate: true);
        }
        #endregion

        #region Data Fields

        public string TitleDisplay => _priest.Title.Description();

        [Required(ErrorMessage = "PriestPriestTitleRequiredAttribute")]
        public int? TitleValue
        {
            get => Convert.ToInt32(_priest.Title);
            set
            {
                if (Enum.IsDefined(typeof(PriestTitle), value))
                {
                    SetProperty(_priest.Title, (PriestTitle)value, _priest, (p, t) => p.Title = t, validate: true);
                }
            }

        }

        public string PositionDisplay => _priest.Position.Description();

        [Required(ErrorMessage = "PriestPriestPositionRequiredAttribute")]
        public int? PositionValue
        {
            get => Convert.ToInt32(_priest.Position);
            set
            {
                if (Enum.IsDefined(typeof(PriestPosition), value))
                {
                    SetProperty(_priest.Position, (PriestPosition)value, _priest, (p, t) => p.Position = t, validate: true);
                }

            }
        }

        [Required(AllowEmptyStrings = false, ErrorMessage = "PriestFirstNamesRequiredAttribute")]
        [MinLength(Constants.PriestFirstNamesMinLengthAttribute, ErrorMessage = "PriestFirstNamesMinLengthAttribute")]
        [MaxLength(Constants.PriestFirstNamesMaxLengthAttribute, ErrorMessage = "PriestFirstNamesMaxLengthAttribute")]
        public string FirstNames
        {
            get => _priest.FirstNames;
            set => SetProperty(_priest.FirstNames, value, _priest, (p, t) => p.FirstNames = t, validate: true);
        }

        [Required(AllowEmptyStrings = false, ErrorMessage = "PriestLastNamesRequiredAttribute")]
        [MinLength(Constants.PriestLastNamesMinLengthAttribute, ErrorMessage = "PriestLastNamesMinLengthAttribute")]
        [MaxLength(Constants.PriestLastNamesMaxLengthAttribute, ErrorMessage = "PriestLastNamesMaxLengthAttribute")]
        public string LastNames
        {
            get => _priest.LastNames;
            set => SetProperty(_priest.LastNames, value, _priest, (p, t) => p.LastNames = t, validate: true);
        }
        #endregion

        #region Audit Fields
        public DateTime CreatedDate
        {
            get => _priest.CreatedDate;
            set => SetProperty(_priest.CreatedDate, value, _priest, (p, t) => p.CreatedDate = t, validate: true);
        }

        public DateTime ModifiedDate
        {
            get => _priest.ModifiedDate;
            set => SetProperty(_priest.ModifiedDate, value, _priest, (p, t) => p.ModifiedDate = t, validate: true);
        }
        #endregion

        #region Soft Delete
        public bool IsDeleted
        {
            get => _priest.IsDeleted;
            set => SetProperty(_priest.IsDeleted, value, _priest, (p, t) => p.IsDeleted = t, validate: true);
        }
        #endregion

    }
}
