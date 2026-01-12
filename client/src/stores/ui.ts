import { defineStore } from 'pinia'
import { ref } from 'vue'

export const useUiStore = defineStore('ui', () => {
    const isSidebarOpen = ref(true)
    const isLoading = ref(false)
    const loadingMessage = ref('')
    const toast = ref<{ message: string; type: 'success' | 'error' | 'warning' | 'info'; show: boolean }>({
        message: '',
        type: 'info',
        show: false
    })

    function toggleSidebar(): void {
        isSidebarOpen.value = !isSidebarOpen.value
    }

    function setSidebarOpen(open: boolean): void {
        isSidebarOpen.value = open
    }

    function setLoading(loading: boolean, message = ''): void {
        isLoading.value = loading
        loadingMessage.value = message
    }

    function showToast(message: string, type: 'success' | 'error' | 'warning' | 'info' = 'info'): void {
        toast.value = { message, type, show: true }
        setTimeout(() => {
            toast.value.show = false
        }, 4000)
    }

    function hideToast(): void {
        toast.value.show = false
    }

    return {
        isSidebarOpen,
        isLoading,
        loadingMessage,
        toast,
        toggleSidebar,
        setSidebarOpen,
        setLoading,
        showToast,
        hideToast
    }
})
