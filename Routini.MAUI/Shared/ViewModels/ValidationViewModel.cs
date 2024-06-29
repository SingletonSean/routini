using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections;
using System.ComponentModel;

namespace Routini.MAUI.Shared.ViewModels
{
    public class ValidationViewModel : ObservableObject, INotifyDataErrorInfo
    {
        private readonly Dictionary<string, List<string>> _propertyNameToErrors = new Dictionary<string, List<string>>();

        public bool HasErrors => _propertyNameToErrors.Any();
        
        public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;

        public IEnumerable GetErrors(string? propertyName)
        {
            if (propertyName == null)
            {
                return _propertyNameToErrors.SelectMany(e => e.Value);
            }

            return _propertyNameToErrors[propertyName];
        }

        protected string? GetFirstError(string propertyName)
        {
            return _propertyNameToErrors.GetValueOrDefault(propertyName)?.FirstOrDefault();
        }

        protected void AddError(string propertyName, string errorMessage)
        {
            if (!_propertyNameToErrors.ContainsKey(propertyName))
            {
                _propertyNameToErrors.Add(propertyName, new List<string>());
            }

            _propertyNameToErrors[propertyName].Add(errorMessage);

            OnErrorsChanged(propertyName);
        }

        protected void ClearErrors(string propertyName)
        {
            _propertyNameToErrors.Remove(propertyName);

            OnErrorsChanged(propertyName);
        }

        private void OnErrorsChanged(string propertyName)
        {
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
        }
    }
}
