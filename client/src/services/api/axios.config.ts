import axios from 'axios'
import type { AxiosInstance, InternalAxiosRequestConfig, AxiosResponse, AxiosError } from 'axios'
import { useAuthStore } from '@/stores/auth'

const API_BASE_URL = import.meta.env.VITE_API_URL || 'https://localhost:5001/api/v1'

const apiClient: AxiosInstance = axios.create({
    baseURL: API_BASE_URL,
    timeout: 30000,
    headers: {
        'Content-Type': 'application/json',
        'Accept': 'application/json'
    }
})

apiClient.interceptors.request.use(
    (config: InternalAxiosRequestConfig) => {
        const token = localStorage.getItem('token')
        if (token && config.headers) {
            config.headers.Authorization = `Bearer ${token}`
        }
        return config
    },
    (error: AxiosError) => {
        return Promise.reject(error)
    }
)

apiClient.interceptors.response.use(
    (response: AxiosResponse) => {
        return response
    },
    async (error: AxiosError) => {
        const originalRequest = error.config as InternalAxiosRequestConfig & { _retry?: boolean }

        if (error.response?.status === 401 && !originalRequest._retry) {
            originalRequest._retry = true

            const refreshToken = localStorage.getItem('refreshToken')
            if (refreshToken) {
                try {
                    const response = await axios.post(`${API_BASE_URL}/auth/refresh`, {
                        refreshToken
                    })

                    const { token } = response.data
                    localStorage.setItem('token', token)

                    if (originalRequest.headers) {
                        originalRequest.headers.Authorization = `Bearer ${token}`
                    }

                    return apiClient(originalRequest)
                } catch {
                    const authStore = useAuthStore()
                    authStore.clearAuth()
                    window.location.href = '/login'
                }
            } else {
                const authStore = useAuthStore()
                authStore.clearAuth()
                window.location.href = '/login'
            }
        }

        return Promise.reject(error)
    }
)

export default apiClient
