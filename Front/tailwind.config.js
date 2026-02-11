/** @type {import('tailwindcss').Config} */
module.exports = {
  content: [
    "./src/**/*.{html,ts}", 
  ],
  theme: {
    extend: {
      // الـ Design System بتاعنا اللي اتفقنا عليه
      colors: {
        brand: {
          primary: '#1E40AF',
          secondary: '#64748B',
        }
      }
    },
  },
  plugins: [],
}