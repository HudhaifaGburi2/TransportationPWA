import { defineStore } from 'pinia'
import { ref, computed } from 'vue'
import apiClient from '@/services/api/axios.config'

export interface Bus {
    busId: string
    busNumber: string
    periodId: number
    periodName?: string
    routeId?: string
    routeName?: string
    driverName?: string
    driverPhoneNumber?: string
    capacity: number
    currentStudentCount: number
    utilizationPercentage: number
    isActive: boolean
    isMerged: boolean
    mergedWithBusId?: string
    districts: DistrictInfo[]
}

export interface DistrictInfo {
    districtId: string
    districtNameAr: string
    districtNameEn?: string
}

export interface BusStatistics {
    busId: string
    busNumber: string
    totalStudents: number
    activeStudents: number
    suspendedStudents: number
    capacity: number
    utilizationRate: number
    districtBreakdown: DistrictStudentCount[]
}

export interface DistrictStudentCount {
    districtId: string
    districtName: string
    studentCount: number
}

export interface BusSummary {
    totalBuses: number
    activeBuses: number
    inactiveBuses: number
    totalCapacity: number
    totalStudentsAssigned: number
    overallUtilization: number
    byPeriod: PeriodBusSummary[]
}

export interface PeriodBusSummary {
    periodId: number
    periodName: string
    busCount: number
    studentCount: number
    totalCapacity: number
    utilization: number
}

export interface CreateBusDto {
    busNumber: string
    periodId: number
    routeId?: string
    driverName?: string
    driverPhoneNumber?: string
    capacity: number
    districtIds: string[]
}

export interface UpdateBusDto extends CreateBusDto {
    isActive: boolean
}

export interface BusQuery {
    periodId?: number
    routeId?: string
    districtId?: string
    isActive?: boolean
    search?: string
    pageNumber?: number
    pageSize?: number
}

export const useBusStore = defineStore('bus', () => {
    const buses = ref<Bus[]>([])
    const currentBus = ref<Bus | null>(null)
    const statistics = ref<BusStatistics | null>(null)
    const summary = ref<BusSummary | null>(null)
    const loading = ref(false)
    const error = ref<string | null>(null)

    const busesByPeriod = computed(() => {
        const grouped: Record<number, Bus[]> = {}
        buses.value.forEach(bus => {
            const periodId = bus.periodId
            if (periodId !== undefined) {
                if (!grouped[periodId]) {
                    grouped[periodId] = []
                }
                grouped[periodId].push(bus)
            }
        })
        return grouped
    })

    const activeBuses = computed(() => buses.value.filter(b => b.isActive))
    const totalCapacity = computed(() => activeBuses.value.reduce((sum, b) => sum + b.capacity, 0))
    const totalStudents = computed(() => activeBuses.value.reduce((sum, b) => sum + b.currentStudentCount, 0))

    async function fetchBuses(query?: BusQuery) {
        loading.value = true
        error.value = null
        try {
            const params = new URLSearchParams()
            if (query?.periodId) params.append('periodId', query.periodId.toString())
            if (query?.routeId) params.append('routeId', query.routeId)
            if (query?.isActive !== undefined) params.append('isActive', query.isActive.toString())
            if (query?.search) params.append('search', query.search)

            const response = await apiClient.get(`/buses?${params}`)
            if (response.data.success) {
                buses.value = response.data.data
            } else {
                error.value = response.data.message
            }
        } catch (e: any) {
            error.value = e.response?.data?.message || 'حدث خطأ في تحميل الباصات'
        } finally {
            loading.value = false
        }
    }

    async function fetchBusesByPeriod(periodId: number) {
        loading.value = true
        error.value = null
        try {
            const response = await apiClient.get(`/buses/by-period/${periodId}`)
            if (response.data.success) {
                buses.value = response.data.data
            } else {
                error.value = response.data.message
            }
        } catch (e: any) {
            error.value = e.response?.data?.message || 'حدث خطأ في تحميل الباصات'
        } finally {
            loading.value = false
        }
    }

    async function fetchBusById(id: string) {
        loading.value = true
        error.value = null
        try {
            const response = await apiClient.get(`/buses/${id}`)
            if (response.data.success) {
                currentBus.value = response.data.data
                return response.data.data
            } else {
                error.value = response.data.message
            }
        } catch (e: any) {
            error.value = e.response?.data?.message || 'حدث خطأ في تحميل بيانات الباص'
        } finally {
            loading.value = false
        }
        return null
    }

    async function createBus(dto: CreateBusDto) {
        loading.value = true
        error.value = null
        try {
            const response = await apiClient.post('/buses', dto)
            if (response.data.success) {
                buses.value.push(response.data.data)
                return response.data.data
            } else {
                error.value = response.data.message
            }
        } catch (e: any) {
            error.value = e.response?.data?.message || 'حدث خطأ في إنشاء الباص'
        } finally {
            loading.value = false
        }
        return null
    }

    async function updateBus(id: string, dto: UpdateBusDto) {
        loading.value = true
        error.value = null
        try {
            const response = await apiClient.put(`/buses/${id}`, dto)
            if (response.data.success) {
                const index = buses.value.findIndex(b => b.busId === id)
                if (index !== -1) {
                    buses.value[index] = response.data.data
                }
                return response.data.data
            } else {
                error.value = response.data.message
            }
        } catch (e: any) {
            error.value = e.response?.data?.message || 'حدث خطأ في تحديث الباص'
        } finally {
            loading.value = false
        }
        return null
    }

    async function deleteBus(id: string) {
        loading.value = true
        error.value = null
        try {
            const response = await apiClient.delete(`/buses/${id}`)
            if (response.data.success) {
                buses.value = buses.value.filter(b => b.busId !== id)
                return true
            } else {
                error.value = response.data.message
            }
        } catch (e: any) {
            error.value = e.response?.data?.message || 'حدث خطأ في حذف الباص'
        } finally {
            loading.value = false
        }
        return false
    }

    async function fetchStatistics(busId: string) {
        loading.value = true
        error.value = null
        try {
            const response = await apiClient.get(`/buses/${busId}/statistics`)
            if (response.data.success) {
                statistics.value = response.data.data
                return response.data.data
            } else {
                error.value = response.data.message
            }
        } catch (e: any) {
            error.value = e.response?.data?.message || 'حدث خطأ في تحميل إحصائيات الباص'
        } finally {
            loading.value = false
        }
        return null
    }

    async function fetchSummary() {
        loading.value = true
        error.value = null
        try {
            const response = await apiClient.get('/buses/summary')
            if (response.data.success) {
                summary.value = response.data.data
                return response.data.data
            } else {
                error.value = response.data.message
            }
        } catch (e: any) {
            error.value = e.response?.data?.message || 'حدث خطأ في تحميل ملخص الباصات'
        } finally {
            loading.value = false
        }
        return null
    }

    function clearError() {
        error.value = null
    }

    return {
        buses,
        currentBus,
        statistics,
        summary,
        loading,
        error,
        busesByPeriod,
        activeBuses,
        totalCapacity,
        totalStudents,
        fetchBuses,
        fetchBusesByPeriod,
        fetchBusById,
        createBus,
        updateBus,
        deleteBus,
        fetchStatistics,
        fetchSummary,
        clearError
    }
})
