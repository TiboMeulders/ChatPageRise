/** @type {import('tailwindcss').Config} */
module.exports = {
    content: [
        "./**/*.{razor,html,cs}",
        "./wwwroot/index.html",
        "./wwwroot/index.html",
        "!./node_modules/**",  // Explicitly exclude node_modules
        "!./bin/**",           // Exclude build output
        "!./obj/**"            // Exclude obj folder
    ],
    theme: {
        extend: {
            colors: {
                'orange': {
                    50: '#fff7ed',
                    100: '#ffedd5',
                    200: '#fed7aa',
                    300: '#fdba74',
                    400: '#fb923c',
                    500: '#f97316',
                    600: '#ea580c',
                    700: '#c2410c',
                    800: '#9a3412',
                    900: '#7c2d12',
                }
            }
        },
    },
    plugins: [
        require('@tailwindcss/forms'),
        require('@tailwindcss/typography'),
    ],
}