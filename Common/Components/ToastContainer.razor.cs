using Microsoft.AspNetCore.Components;
using Quiz.Common.Models;
using Quiz.Common.Services;

namespace Quiz.Common.Components
{
    public partial class ToastContainer
    {
        [Inject] private ToastService ToastService { get; set; } = default!;

        private List<ToastModel> _toasts = new();

        protected override void OnInitialized()
        {
            ToastService.OnToastsUpdated += UpdateToasts;
            _toasts = ToastService.Toasts.ToList();
        }

        private void UpdateToasts()
        {
            _toasts = ToastService.Toasts.ToList();
            InvokeAsync(StateHasChanged);
        }

        private Task RemoveToast(string id)
        {
            ToastService.RemoveToast(id);
            return Task.CompletedTask;
        }
    }
}
