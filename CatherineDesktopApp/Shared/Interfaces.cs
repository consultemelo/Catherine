using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatherineDesktopApp.Shared
{
    public interface IEmptyObject
    {
        bool IsEmpty { get; }
    }

    public interface IAuditableEntity
    {
        DateTime CreatedDate { get; set; }
        string CreatedBy { get; set; }
        DateTime ModifiedDate { get; set; }
        string ModifiedBy { get; set; }
    }

    public interface ISoftDeletableEntity
    {
        bool IsDeleted { get; set; }
    }

    public interface INavigableCollection
    {
        bool CanAddItem { get; }
        bool CanEditItem { get; }
        bool IsItemViewOpen { get; set; }
        bool AllowItemViewToClose { get; set; }
    }

    public interface IEditableDetail
    {
        public RelayCommand StartAdditionCommand { get; }
        public void StartAddition();

        public RelayCommand StartEditionCommand { get; }
        public void StartEdition();

        public RelayCommand CancelEditCommand { get; }
        public void CancelEdit();
    }
}
