import apiClient from './axios.config'
import type { LoginRequest, LoginResponse, UserInfo } from '@/types/auth'

export const authService = {
    async login(credentials: LoginRequest): Promise<LoginResponse> {
        const response = await apiClient.post<LoginResponse>('/auth/login', credentials)
        return response.data
    },

    async logout(): Promise<void> {
        await apiClient.post('/auth/logout')
    },

    async getCurrentUser(): Promise<UserInfo> {
        const response = await apiClient.get<UserInfo>('/auth/me')
        return response.data
    },

    async refreshToken(refreshToken: string): Promise<{ token: string }> {
        const response = await apiClient.post<{ token: string }>('/auth/refresh', { refreshToken })
        return response.data
    },

    async validateToken(): Promise<boolean> {
        try {
            await apiClient.get('/auth/validate')
            return true
        } catch {
            return false
        }
    }
}
