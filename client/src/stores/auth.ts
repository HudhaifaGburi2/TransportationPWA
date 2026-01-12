import { defineStore } from 'pinia'
import { ref, computed } from 'vue'
import type { LoginRequest, LoginResponse, UserInfo } from '@/types/auth'
import { authService } from '@/services/api/auth.service'

export const useAuthStore = defineStore('auth', () => {
    const token = ref<string | null>(localStorage.getItem('token'))
    const refreshToken = ref<string | null>(localStorage.getItem('refreshToken'))
    const user = ref<UserInfo | null>(null)
    const isLoading = ref(false)
    const error = ref<string | null>(null)

    const isAuthenticated = computed(() => !!token.value)
    const userRoles = computed(() => user.value?.roles || [])
    const fullName = computed(() => user.value?.fullName || '')

    function hasRole(role: string): boolean {
        return userRoles.value.includes(role)
    }

    function hasAnyRole(roles: string[]): boolean {
        return roles.some(role => hasRole(role))
    }

    async function login(credentials: LoginRequest): Promise<boolean> {
        isLoading.value = true
        error.value = null

        try {
            const response: LoginResponse = await authService.login(credentials)

            token.value = response.token
            refreshToken.value = response.refreshToken || null
            user.value = {
                userId: response.userId,
                username: response.username || '',
                fullName: response.fullName || '',
                roles: response.roles || []
            }

            localStorage.setItem('token', response.token)
            if (response.refreshToken) {
                localStorage.setItem('refreshToken', response.refreshToken)
            }

            return true
        } catch (err: any) {
            error.value = err.response?.data?.message || 'فشل تسجيل الدخول'
            return false
        } finally {
            isLoading.value = false
        }
    }

    async function logout(): Promise<void> {
        try {
            await authService.logout()
        } catch {
            // Ignore logout errors
        } finally {
            clearAuth()
        }
    }

    function clearAuth(): void {
        token.value = null
        refreshToken.value = null
        user.value = null
        localStorage.removeItem('token')
        localStorage.removeItem('refreshToken')
    }

    async function fetchCurrentUser(): Promise<void> {
        if (!token.value) return

        try {
            const userInfo = await authService.getCurrentUser()
            user.value = userInfo
        } catch {
            clearAuth()
        }
    }

    async function initializeAuth(): Promise<void> {
        if (token.value) {
            await fetchCurrentUser()
        }
    }

    return {
        token,
        refreshToken,
        user,
        isLoading,
        error,
        isAuthenticated,
        userRoles,
        fullName,
        hasRole,
        hasAnyRole,
        login,
        logout,
        clearAuth,
        fetchCurrentUser,
        initializeAuth
    }
})
