using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CatherineDesktopApp.Services;

namespace CatherineDesktopApp.Data.Validation
{
    public class LocalizedValidator : ObservableValidator
    {

        private readonly IResourceLocationService _resourceService;

        private readonly Dictionary<string, int> _intConstants = new Dictionary<string, int>();

        public LocalizedValidator(IResourceLocationService resourceService)
        {
            _resourceService = resourceService;
            BuildConstantDictionary();
            base.ErrorsChanged += OnErrorsChanged;
        }

        private void BuildConstantDictionary() { }

        private IEnumerable<ValidationResult> _localizedValidationResults = new List<ValidationResult>();
        public IEnumerable<ValidationResult> LocalizedValidationResults
        {
            get => _localizedValidationResults;
            set
            {
                if (SetProperty(ref _localizedValidationResults, value))
                {
                    //Update the indexer to reflect the changed errors in the UI.
                    OnPropertyChanged("Item[]");
                }
            }
        }

        public string LocalizedMessage => string.Join(Environment.NewLine, LocalizedValidationResults.Select(e => e.ErrorMessage).Distinct());

        private void OnErrorsChanged(object? sender, DataErrorsChangedEventArgs e)
        {
            if (GetErrors().Any())
            {
                LocalizedValidationResults = GetErrors()
                                    .SelectMany(r => r.MemberNames, (r, memberName) => (memberName, r.ErrorMessage ?? "UndefinedError"))
                                    .ToList()
                                    .Select(x => LocalizeAndFormatError(x));
            }
            else
            {
                LocalizedValidationResults = Enumerable.Empty<ValidationResult>();
            }
            //Update the indexer to reflect the changed errors in the UI.
            OnPropertyChanged("Item[]");
        }

        public IEnumerable<ValidationResult> this[string propertyName]
        {
            get
            {
                return LocalizedValidationResults?.Where(x => x.MemberNames.Contains(propertyName)) ?? Enumerable.Empty<ValidationResult>();
            }
        }

        public new void ValidateAllProperties()
        {
            base.ValidateAllProperties();
        }

        #region Helper Methods
        private ValidationResult LocalizeAndFormatError((string MemberName, string ErrorMessage) originalResult)
        {
            string localizedMessage = _resourceService.GetErrorMessageString(originalResult.ErrorMessage);

            if (_intConstants.ContainsKey(originalResult.ErrorMessage))
            {
                localizedMessage = string.Format(localizedMessage, _intConstants[originalResult.ErrorMessage]);
            }

            return new ValidationResult(localizedMessage, new List<string> { originalResult.MemberName });
        }
        #endregion
    }
}

