import apiClient from './axios.config'

export interface RegistrationStatistics {
    totalRequests: number
    pendingCount: number
    approvedCount: number
    rejectedCount: number
    todayCount: number
    thisWeekCount: number
    byDistrict: DistrictStat[]
}

export interface DistrictStat {
    districtId: string
    districtName: string
    count: number
}

export interface BusSuggestion {
    busId: string
    busNumber: string
    routeName?: string
    capacity: number
    currentStudentCount: number
    availableSeats: number
}

export const registrationApi = {
    async getStatistics(): Promise<RegistrationStatistics> {
        const response = await apiClient.get('/registration/statistics')
        if (response.data.success) {
            return response.data.data
        }
        throw new Error(response.data.message || 'Failed to load statistics')
    },

    async getBusSuggestions(districtId: string): Promise<BusSuggestion[]> {
        const response = await apiClient.get(`/registration/bus-suggestions/${districtId}`)
        if (response.data.success) {
            return response.data.data
        }
        throw new Error(response.data.message || 'Failed to load bus suggestions')
    },

    async assignToBus(requestId: string, busId: string, notes?: string): Promise<any> {
        const response = await apiClient.post(`/registration/${requestId}/assign-bus`, {
            busId,
            notes
        })
        if (response.data.success) {
            return response.data.data
        }
        throw new Error(response.data.message || 'Failed to assign bus')
    }
}
