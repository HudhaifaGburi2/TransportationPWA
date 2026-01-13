export interface LoginRequest {
    username: string
    password: string
}

export interface LoginResponse {
    token: string
    refreshToken?: string
    userId: string
    username?: string
    fullName?: string
    roles?: string[]
    expiresAt?: string
    halaqaLocationId?: number
}

export interface UserInfo {
    userId: string
    username: string
    fullName: string
    roles: string[]
    halaqaLocationId?: number
}

export interface ApiResponse<T> {
    success: boolean
    data?: T
    message?: string
    errors?: string[]
}
