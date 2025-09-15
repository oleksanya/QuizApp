using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace Quiz.Common.Components
{
    public partial class CustomErrorBoundary
    {
        private ErrorBoundary? _boundary;

        [Parameter] public RenderFragment? ChildContent { get; set; }

        private void Recover()
        {
            _boundary?.Recover();
        }
    }
}
