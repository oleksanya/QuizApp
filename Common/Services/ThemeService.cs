using Microsoft.JSInterop;

namespace Quiz.Common.Services
{
    public class ThemeService : IDisposable
    {
        private readonly IJSRuntime _jsRuntime;
        private DotNetObjectReference<ThemeService>? _dotNetRef;
        private bool _isDarkTheme = false;
        private bool _isInitialized = false;
        
        public event Action? OnThemeChanged;

        public ThemeService(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
        }

        public bool IsDarkTheme => _isDarkTheme;

        public async Task InitializeThemeAsync()
        {
            if (_isInitialized) return;

            try
            {
                _dotNetRef = DotNetObjectReference.Create(this);
                
                var savedTheme = await _jsRuntime.InvokeAsync<string?>("localStorage.getItem", "theme-preference");
                
                if (!string.IsNullOrEmpty(savedTheme))
                {
                    _isDarkTheme = savedTheme == "dark";
                }
                else
                {
                    // If no saved preference, detect system theme
                    _isDarkTheme = await _jsRuntime.InvokeAsync<bool>("themeUtils.getSystemTheme");
                }

                await ApplyThemeAsync();
                _isInitialized = true;
            }
            catch (Exception)
            {
                _isDarkTheme = false;
                _isInitialized = true;
            }
        }

        public async Task ToggleThemeAsync()
        {
            _isDarkTheme = !_isDarkTheme;
            await SaveThemePreferenceAsync();
            await ApplyThemeAsync();
            OnThemeChanged?.Invoke();
        }

        public async Task SetThemeAsync(bool isDark)
        {
            if (_isDarkTheme != isDark)
            {
                _isDarkTheme = isDark;
                await SaveThemePreferenceAsync();
                await ApplyThemeAsync();
                OnThemeChanged?.Invoke();
            }
        }

        [JSInvokable]
        public async Task OnSystemThemeChanged(bool isDark)
        {
            // Only respond to system theme changes if user hasn't set a preference
            var savedTheme = await _jsRuntime.InvokeAsync<string?>("localStorage.getItem", "theme-preference");
            if (string.IsNullOrEmpty(savedTheme))
            {
                await SetThemeAsync(isDark);
            }
        }

        private async Task SaveThemePreferenceAsync()
        {
             var themeValue = _isDarkTheme ? "dark" : "light";
             await _jsRuntime.InvokeVoidAsync("localStorage.setItem", "theme-preference", themeValue);
        }

        private async Task ApplyThemeAsync()
        {
            await _jsRuntime.InvokeVoidAsync("themeUtils.applyTheme", _isDarkTheme);
        }

        public void Dispose()
        {
            _dotNetRef?.Dispose();
        }
    }
}