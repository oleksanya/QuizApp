using Quiz.Common.Models;

namespace Quiz.Common.Services
{
    public class ToastService
    {
        private readonly List<ToastModel> _toasts = new();
        
        public event Action? OnToastsUpdated;
        
        public IReadOnlyList<ToastModel> Toasts => _toasts.AsReadOnly();
        
        public void ShowSuccess(string message, string? title = null, int durationMs = 5000)
        {
            ShowToast(message, ToastType.Success, title, durationMs);
        }
        
        public void ShowError(string message, string? title = null, int durationMs = 8000)
        {
            ShowToast(message, ToastType.Error, title, durationMs);
        }
        
        public void ShowWarning(string message, string? title = null, int durationMs = 6000)
        {
            ShowToast(message, ToastType.Warning, title, durationMs);
        }
        
        public void ShowInfo(string message, string? title = null, int durationMs = 5000)
        {
            ShowToast(message, ToastType.Info, title, durationMs);
        }
        
        private void ShowToast(string message, ToastType type, string? title = null, int durationMs = 5000)
        {
            if (string.IsNullOrWhiteSpace(message))
                return;
                
            var toast = new ToastModel
            {
                Message = message,
                Type = type,
                Title = title,
                DurationMs = durationMs
            };
            
            _toasts.Add(toast);
            OnToastsUpdated?.Invoke();
            
            _ = Task.Delay(durationMs).ContinueWith(_ => RemoveToast(toast.Id));
        }
        
        public void RemoveToast(string id)
        {
            var toast = _toasts.FirstOrDefault(t => t.Id == id);
            if (toast != null)
            {
                _toasts.Remove(toast);
                OnToastsUpdated?.Invoke();
            }
        }
        
        public void RemoveAllToasts()
        {
            _toasts.Clear();
            OnToastsUpdated?.Invoke();
        }
    }
}