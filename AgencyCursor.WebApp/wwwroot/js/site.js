// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Dark mode toggle
(function () {
    var DARK_MODE_KEY = 'darkMode';

    function applyDarkMode(enabled) {
        document.body.classList.toggle('dark-mode', enabled);
        var btn = document.getElementById('darkModeToggle');
        if (btn) {
            btn.textContent = enabled ? '☀️ Light Mode' : '🌙 Dark Mode';
        }
    }

    function getPreference() {
        var stored = localStorage.getItem(DARK_MODE_KEY);
        if (stored !== null) {
            return stored === 'true';
        }
        return window.matchMedia && window.matchMedia('(prefers-color-scheme: dark)').matches;
    }

    function toggleDarkMode() {
        var enabled = !document.body.classList.contains('dark-mode');
        localStorage.setItem(DARK_MODE_KEY, enabled);
        applyDarkMode(enabled);
    }

    document.addEventListener('DOMContentLoaded', function () {
        applyDarkMode(getPreference());
        var btn = document.getElementById('darkModeToggle');
        if (btn) {
            btn.addEventListener('click', toggleDarkMode);
        }
    });
})();
