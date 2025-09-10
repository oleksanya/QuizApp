window.themeUtils = {
    applyTheme: function(isDark) {
        const themeClass = isDark ? 'dark-theme' : 'light-theme';
        document.documentElement.className = themeClass;
    },
    
    getSystemTheme: function() {
        return window.matchMedia('(prefers-color-scheme: dark)').matches;
    },
    
    // Listen for system theme changes
    watchSystemTheme: function(dotNetRef) {
        const mediaQuery = window.matchMedia('(prefers-color-scheme: dark)');
        
        function handleThemeChange(e) {
            dotNetRef.invokeMethodAsync('OnSystemThemeChanged', e.matches);
        }
        
        mediaQuery.addEventListener('change', handleThemeChange);
        
        return function() {
            mediaQuery.removeEventListener('change', handleThemeChange);
        };
    }
};