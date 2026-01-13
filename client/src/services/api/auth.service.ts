import apiClient from './axios.config'
import type { LoginRequest, LoginResponse, UserInfo, ApiResponse } from '@/types/auth'

const LOGIN_GATEWAY_TOKEN = import.meta.env.VITE_LOGIN_GATEWAY_TOKEN || ''

export const authService = {
    async login(credentials: LoginRequest): Promise<LoginResponse> {
        const response = await apiClient.post<ApiResponse<LoginResponse>>('/auth/login', credentials, {
            headers: {
                'X-Login-Gateway-Token': LOGIN_GATEWAY_TOKEN
            }
        })

        if (!response.data.success || !response.data.data) {
            throw new Error(response.data.message || 'Login failed')
        }

        return response.data.data
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
