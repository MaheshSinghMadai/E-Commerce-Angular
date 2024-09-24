/** @type {import('tailwindcss').Config} */
module.exports = {
  content: [
    "./src/**/*.{html,ts}",
  ],
  theme: {
    extend: {
      fontFamily: {
        sans: ['"Inter var"', 'sans-serif'], // Set "Inter var" as the default font family
      },
    },
  },
  plugins: [],
  important : true
}