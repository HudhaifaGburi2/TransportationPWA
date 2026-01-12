/** @type {import('tailwindcss').Config} */
export default {
  content: [
    "./index.html",
    "./src/**/*.{vue,js,ts,jsx,tsx}",
  ],
  theme: {
    extend: {
      colors: {
        primary: {
          DEFAULT: '#3d5a4f',
          light: '#4d6f62',
          dark: '#2d4a3f',
        },
        secondary: {
          DEFAULT: '#b8935f',
          light: '#c9a570',
          dark: '#a07d4f',
        },
        success: '#28a745',
        warning: '#ffa726',
        danger: '#dc3545',
        info: '#17a2b8',
        neutral: '#6c757d',
        background: '#f8f9fa',
        card: '#ffffff',
        border: '#dee2e6',
      },
      fontFamily: {
        cairo: ['Cairo', 'Segoe UI', 'Tahoma', 'sans-serif'],
        roboto: ['Roboto', 'Segoe UI', 'sans-serif'],
      },
      borderRadius: {
        'sm': '6px',
        'md': '8px',
        'lg': '12px',
      },
      boxShadow: {
        'sm': '0 2px 4px rgba(0,0,0,0.08)',
        'md': '0 4px 8px rgba(0,0,0,0.1)',
        'lg': '0 8px 16px rgba(0,0,0,0.12)',
      },
      spacing: {
        'xs': '4px',
        'sm': '8px',
        'md': '16px',
        'lg': '24px',
        'xl': '32px',
        'xxl': '48px',
      },
    },
  },
  plugins: [],
}
