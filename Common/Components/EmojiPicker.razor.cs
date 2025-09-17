using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Quiz.Common.Components
{
    public partial class EmojiPicker
    {
        [Inject] public required IJSRuntime JS { get; set; }
        [Parameter] public string? InputSelector { get; set; }

        private string ButtonId = $"btn_{Guid.NewGuid()}";
        private string PickerId = $"picker_{Guid.NewGuid()}";

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender && !string.IsNullOrEmpty(InputSelector))
            {
                await JS.InvokeVoidAsync("emojiMartInterop.init", ButtonId, PickerId, InputSelector);
            }
        }
    }
}
