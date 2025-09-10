using Microsoft.AspNetCore.Components;
using Quiz.Common.Services;

namespace Quiz.Layout
{
    public partial class MainLayout : LayoutComponentBase, IDisposable
    {
        [Inject] private ThemeService ThemeService { get; set; } = default!;

        private bool _isDarkTheme = false;

        protected override async Task OnInitializedAsync()
        {
            await ThemeService.InitializeThemeAsync();
            _isDarkTheme = ThemeService.IsDarkTheme;
            ThemeService.OnThemeChanged += OnThemeChanged;
        }

        private async Task OnThemeToggle()
        {
            await ThemeService.ToggleThemeAsync();
        }

        private void OnThemeChanged()
        {
            _isDarkTheme = ThemeService.IsDarkTheme;
            StateHasChanged();
        }

        public void Dispose()
        {
            ThemeService.OnThemeChanged -= OnThemeChanged;
        }
    }
}