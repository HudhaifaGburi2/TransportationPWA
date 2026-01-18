import { defineStore } from 'pinia'
import { ref, computed } from 'vue'
import apiClient from '@/services/api/axios.config'

// Types
export interface Driver {
  id: string
  fullName: string
  phoneNumber: string
  isActive: boolean
  createdAt: string
}

export interface Route {
  id: string
  name: string
  code: string
  district: string
  meetingPoint: string
  meetingPointLatitude?: number
  meetingPointLongitude?: number
  pickupTime: string
  dropoffTime: string
  capacity: number
  isActive: boolean
  createdAt: string
}

export interface Bus {
  id: string
  busNumber: string
  licensePlate: string
  capacity: number
  isActive: boolean
  createdAt: string
}

export interface Statistics {
  totalBuses: number
  activeBuses: number
  inactiveBuses: number
  totalCapacity: number
  totalDrivers: number
  activeDrivers: number
  totalRoutes: number
  activeRoutes: number
}

export interface CreateDriverDto {
  fullName: string
  phoneNumber: string
}

export interface UpdateDriverDto {
  fullName: string
  phoneNumber: string
  isActive: boolean
}

export interface CreateRouteDto {
  name: string
  code: string
  district: string
  meetingPoint: string
  meetingPointLatitude?: number
  meetingPointLongitude?: number
  pickupTime: string
  dropoffTime: string
  capacity: number
}

export interface UpdateRouteDto extends CreateRouteDto {
  isActive: boolean
}

export interface CreateBusDto {
  busNumber: string
  licensePlate: string
  capacity: number
}

export interface UpdateBusDto {
  busNumber: string
  licensePlate: string
  capacity: number
  isActive: boolean
}

export const useBusManagementStore = defineStore('busManagement', () => {
  // State
  const drivers = ref<Driver[]>([])
  const routes = ref<Route[]>([])
  const buses = ref<Bus[]>([])
  const statistics = ref<Statistics | null>(null)
  const loading = ref(false)
  const error = ref<string | null>(null)

  // Computed
  const activeDrivers = computed(() => drivers.value.filter(d => d.isActive))
  const activeRoutes = computed(() => routes.value.filter(r => r.isActive))
  const activeBuses = computed(() => buses.value.filter(b => b.isActive))

  // Driver Actions
  async function fetchDrivers(query?: { search?: string; isActive?: boolean }) {
    loading.value = true
    error.value = null
    try {
      const params = new URLSearchParams()
      if (query?.search) params.append('search', query.search)
      if (query?.isActive !== undefined) params.append('isActive', String(query.isActive))

      const response = await apiClient.get(`/busmanagement/drivers?${params}`)
      if (response.data.success) {
        drivers.value = response.data.data
      } else {
        error.value = response.data.message
      }
    } catch (err: any) {
      error.value = err.response?.data?.message || 'فشل في تحميل السائقين'
    } finally {
      loading.value = false
    }
  }

  async function createDriver(dto: CreateDriverDto) {
    loading.value = true
    error.value = null
    try {
      const response = await apiClient.post('/busmanagement/drivers', dto)
      if (response.data.success) {
        drivers.value.push(response.data.data)
        return response.data.data
      } else {
        error.value = response.data.message
        return null
      }
    } catch (err: any) {
      error.value = err.response?.data?.message || 'فشل في إضافة السائق'
      return null
    } finally {
      loading.value = false
    }
  }

  async function updateDriver(id: string, dto: UpdateDriverDto) {
    loading.value = true
    error.value = null
    try {
      const response = await apiClient.put(`/busmanagement/drivers/${id}`, dto)
      if (response.data.success) {
        const index = drivers.value.findIndex(d => d.id === id)
        if (index !== -1) drivers.value[index] = response.data.data
        return response.data.data
      } else {
        error.value = response.data.message
        return null
      }
    } catch (err: any) {
      error.value = err.response?.data?.message || 'فشل في تحديث بيانات السائق'
      return null
    } finally {
      loading.value = false
    }
  }

  async function deleteDriver(id: string) {
    loading.value = true
    error.value = null
    try {
      const response = await apiClient.delete(`/busmanagement/drivers/${id}`)
      if (response.data.success) {
        drivers.value = drivers.value.filter(d => d.id !== id)
        return true
      } else {
        error.value = response.data.message
        return false
      }
    } catch (err: any) {
      error.value = err.response?.data?.message || 'فشل في حذف السائق'
      return false
    } finally {
      loading.value = false
    }
  }

  // Route Actions
  async function fetchRoutes(query?: { search?: string; isActive?: boolean }) {
    loading.value = true
    error.value = null
    try {
      const params = new URLSearchParams()
      if (query?.search) params.append('search', query.search)
      if (query?.isActive !== undefined) params.append('isActive', String(query.isActive))

      const response = await apiClient.get(`/busmanagement/routes?${params}`)
      if (response.data.success) {
        routes.value = response.data.data
      } else {
        error.value = response.data.message
      }
    } catch (err: any) {
      error.value = err.response?.data?.message || 'فشل في تحميل المسارات'
    } finally {
      loading.value = false
    }
  }

  async function createRoute(dto: CreateRouteDto) {
    loading.value = true
    error.value = null
    try {
      const response = await apiClient.post('/busmanagement/routes', dto)
      if (response.data.success) {
        routes.value.push(response.data.data)
        return response.data.data
      } else {
        error.value = response.data.message
        return null
      }
    } catch (err: any) {
      error.value = err.response?.data?.message || 'فشل في إضافة المسار'
      return null
    } finally {
      loading.value = false
    }
  }

  async function updateRoute(id: string, dto: UpdateRouteDto) {
    loading.value = true
    error.value = null
    try {
      const response = await apiClient.put(`/busmanagement/routes/${id}`, dto)
      if (response.data.success) {
        const index = routes.value.findIndex(r => r.id === id)
        if (index !== -1) routes.value[index] = response.data.data
        return response.data.data
      } else {
        error.value = response.data.message
        return null
      }
    } catch (err: any) {
      error.value = err.response?.data?.message || 'فشل في تحديث بيانات المسار'
      return null
    } finally {
      loading.value = false
    }
  }

  async function deleteRoute(id: string) {
    loading.value = true
    error.value = null
    try {
      const response = await apiClient.delete(`/busmanagement/routes/${id}`)
      if (response.data.success) {
        routes.value = routes.value.filter(r => r.id !== id)
        return true
      } else {
        error.value = response.data.message
        return false
      }
    } catch (err: any) {
      error.value = err.response?.data?.message || 'فشل في حذف المسار'
      return false
    } finally {
      loading.value = false
    }
  }

  // Bus Actions
  async function fetchBuses(query?: { search?: string; isActive?: boolean }) {
    loading.value = true
    error.value = null
    try {
      const params = new URLSearchParams()
      if (query?.search) params.append('search', query.search)
      if (query?.isActive !== undefined) params.append('isActive', String(query.isActive))

      const response = await apiClient.get(`/busmanagement/buses?${params}`)
      if (response.data.success) {
        buses.value = response.data.data
      } else {
        error.value = response.data.message
      }
    } catch (err: any) {
      error.value = err.response?.data?.message || 'فشل في تحميل الباصات'
    } finally {
      loading.value = false
    }
  }

  async function createBus(dto: CreateBusDto) {
    loading.value = true
    error.value = null
    try {
      const response = await apiClient.post('/busmanagement/buses', dto)
      if (response.data.success) {
        buses.value.push(response.data.data)
        return response.data.data
      } else {
        error.value = response.data.message
        return null
      }
    } catch (err: any) {
      error.value = err.response?.data?.message || 'فشل في إضافة الباص'
      return null
    } finally {
      loading.value = false
    }
  }

  async function updateBus(id: string, dto: UpdateBusDto) {
    loading.value = true
    error.value = null
    try {
      const response = await apiClient.put(`/busmanagement/buses/${id}`, dto)
      if (response.data.success) {
        const index = buses.value.findIndex(b => b.id === id)
        if (index !== -1) buses.value[index] = response.data.data
        return response.data.data
      } else {
        error.value = response.data.message
        return null
      }
    } catch (err: any) {
      error.value = err.response?.data?.message || 'فشل في تحديث بيانات الباص'
      return null
    } finally {
      loading.value = false
    }
  }

  async function deleteBus(id: string) {
    loading.value = true
    error.value = null
    try {
      const response = await apiClient.delete(`/busmanagement/buses/${id}`)
      if (response.data.success) {
        buses.value = buses.value.filter(b => b.id !== id)
        return true
      } else {
        error.value = response.data.message
        return false
      }
    } catch (err: any) {
      error.value = err.response?.data?.message || 'فشل في حذف الباص'
      return false
    } finally {
      loading.value = false
    }
  }

  // Statistics
  async function fetchStatistics() {
    loading.value = true
    error.value = null
    try {
      const response = await apiClient.get('/busmanagement/statistics')
      if (response.data.success) {
        statistics.value = response.data.data
      } else {
        error.value = response.data.message
      }
    } catch (err: any) {
      error.value = err.response?.data?.message || 'فشل في تحميل الإحصائيات'
    } finally {
      loading.value = false
    }
  }

  return {
    // State
    drivers,
    routes,
    buses,
    statistics,
    loading,
    error,
    // Computed
    activeDrivers,
    activeRoutes,
    activeBuses,
    // Driver Actions
    fetchDrivers,
    createDriver,
    updateDriver,
    deleteDriver,
    // Route Actions
    fetchRoutes,
    createRoute,
    updateRoute,
    deleteRoute,
    // Bus Actions
    fetchBuses,
    createBus,
    updateBus,
    deleteBus,
    // Statistics
    fetchStatistics
  }
})
