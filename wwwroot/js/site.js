document.addEventListener('DOMContentLoaded', function () {
    const toggleBtn = document.getElementById('darkModeToggle');
    const pageBody = document.getElementById('pageBody');

    if (!toggleBtn || !pageBody) return;

    const savedTheme = localStorage.getItem('theme');
    if (savedTheme === 'dark') {
        pageBody.classList.add('dark-mode');
    }

    toggleBtn.addEventListener('click', () => {
        pageBody.classList.toggle('dark-mode');
        const currentTheme = pageBody.classList.contains('dark-mode') ? 'dark' : 'light';
        localStorage.setItem('theme', currentTheme);
    });
});
