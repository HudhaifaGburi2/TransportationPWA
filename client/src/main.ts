import { createApp } from 'vue'
import { createPinia } from 'pinia'
import router from './router'
import App from './App.vue'
import './assets/styles/main.css'

const app = createApp(App)
const pinia = createPinia()

app.use(pinia)
app.use(router)

// Initialize auth state before mounting
import { useAuthStore } from './stores/auth'
const authStore = useAuthStore(pinia)
authStore.initializeAuth().then(() => {
    app.mount('#app')
})
