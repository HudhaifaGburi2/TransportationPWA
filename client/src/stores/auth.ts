import { defineStore } from 'pinia'
import { ref, computed } from 'vue'
import type { LoginRequest, LoginResponse, UserInfo } from '@/types/auth'
import { authService } from '@/services/api/auth.service'

// TUMS Role Constants
export const TUMS_ROLES = {
    ADMIN: 'Admin_Tums',
    STAFF: 'Stuff_Tums',
    DRIVER: 'Driver_Tums',
    STUDENT: 'STUDENT',
    SYSTEM_ADMIN: 'SYSTEM_ADMINISTRATOR'
} as const

// Allowed student locations for TUMS
export const ALLOWED_STUDENT_LOCATIONS = [1, 2]

// Helper to safely parse JSON from localStorage
function getStoredUser(): UserInfo | null {
    try {
        const stored = localStorage.getItem('user')
        return stored ? JSON.parse(stored) : null
    } catch {
        return null
    }
}

export const useAuthStore = defineStore('auth', () => {
    const token = ref<string | null>(localStorage.getItem('token'))
    const refreshToken = ref<string | null>(localStorage.getItem('refreshToken'))
    const user = ref<UserInfo | null>(getStoredUser())
    const isLoading = ref(false)
    const error = ref<string | null>(null)
    const isInitialized = ref(false)

    const isAuthenticated = computed(() => !!token.value)
    const userRoles = computed(() => user.value?.roles || [])
    const fullName = computed(() => user.value?.fullName || '')

    // Role-based computed properties
    const isAdmin = computed(() =>
        hasAnyRole([TUMS_ROLES.ADMIN, TUMS_ROLES.SYSTEM_ADMIN])
    )
    const isStaff = computed(() =>
        hasAnyRole([TUMS_ROLES.ADMIN, TUMS_ROLES.STAFF, TUMS_ROLES.SYSTEM_ADMIN])
    )
    const isDriver = computed(() =>
        hasAnyRole([TUMS_ROLES.DRIVER, TUMS_ROLES.ADMIN, TUMS_ROLES.SYSTEM_ADMIN])
    )
    const isStudent = computed(() => hasRole(TUMS_ROLES.STUDENT))

    // Check if student is from allowed locations
    const isAllowedStudent = computed(() => {
        if (!isStudent.value) return false
        const locationId = user.value?.halaqaLocationId
        return locationId !== undefined && ALLOWED_STUDENT_LOCATIONS.includes(locationId)
    })

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
                roles: response.roles || [],
                halaqaLocationId: response.halaqaLocationId
            }

            localStorage.setItem('token', response.token)
            localStorage.setItem('user', JSON.stringify(user.value))
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
            // Redirect to login page
            window.location.href = '/login'
        }
    }

    function clearAuth(): void {
        token.value = null
        refreshToken.value = null
        user.value = null
        localStorage.removeItem('token')
        localStorage.removeItem('refreshToken')
        localStorage.removeItem('user')
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
        if (token.value && !user.value) {
            try {
                await fetchCurrentUser()
            } catch {
                // If fetching user fails but we have stored user, keep it
                const storedUser = getStoredUser()
                if (storedUser) {
                    user.value = storedUser
                }
            }
        }
        isInitialized.value = true
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
        isAdmin,
        isStaff,
        isDriver,
        isStudent,
        isAllowedStudent,
        isInitialized,
        hasRole,
        hasAnyRole,
        login,
        logout,
        clearAuth,
        fetchCurrentUser,
        initializeAuth
    }
})
