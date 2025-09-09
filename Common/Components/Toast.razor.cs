using Microsoft.AspNetCore.Components;
using Quiz.Common.Models;
using Quiz.Common.Services;

namespace Quiz.Common.Components
{
    public partial class Toast
    {
        [Parameter] public ToastModel ToastModel { get; set; } = default!;
        [Parameter] public EventCallback<string> OnRemove { get; set; }

        [Inject] private ToastService ToastService { get; set; } = default!;

        private bool IsVisible { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await Task.Delay(50);
            IsVisible = true;
        }

        private async Task OnCloseClick()
        {
            await OnRemove.InvokeAsync(ToastModel.Id);
        }
    }
}
