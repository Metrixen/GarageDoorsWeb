import { defineConfig } from 'vite';
import path from 'path';

export default defineConfig({
  root: path.resolve(__dirname, 'ClientApp'),
  build: {
    outDir: path.resolve(__dirname, 'wwwroot/dist'),
    emptyOutDir: true,
    rollupOptions: {
      input: path.resolve(__dirname, 'ClientApp/main.ts'),
      output: {
        entryFileNames: 'main.js',
        assetFileNames: 'main.[ext]'
      }
    }
  }
});
