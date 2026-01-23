import { defineConfig } from 'vite'
import react from '@vitejs/plugin-react-swc'
import path from 'node:path'
import tailwindcss from '@tailwindcss/vite'
import vitePluginSvgr from 'vite-plugin-svgr'

// https://vite.dev/config/
export default defineConfig({
  plugins: [react(), tailwindcss(), vitePluginSvgr()],
  resolve: {
    alias: {
      "@": path.resolve(__dirname, "./src")
    }
  },
  base: "/bookmark-manager-app/"
})
