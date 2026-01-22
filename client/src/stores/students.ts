import { defineStore } from 'pinia'
import { ref, computed } from 'vue'
import apiClient from '@/services/api/axios.config'

export interface Student {
    id: string
    studentUserId: number
    studentId: string
    studentName: string
    halaqaTypeCode?: string
    halaqaSectionId?: string
    halaqaGender?: string
    periodId?: number
    ageGroupId?: number
    halaqaLocationId?: number
    teacherName?: string
    districtId: string
    districtName?: string
    homeAddress?: string
    latitude?: number
    longitude?: number
    phoneNumber?: string
    status: string
}

export interface StudentAssignment {
    id: string
    studentId: string
    studentName?: string
    busId: string
    busNumber?: string
    transportType: string
    arrivalBusId?: string
    arrivalBusNumber?: string
    returnBusId?: string
    returnBusNumber?: string
    isActive: boolean
    assignedAt: string
}

export interface StudentSuspension {
    id: string
    studentId: string
    studentName?: string
    busId?: string
    busNumber?: string
    reason: string
    suspendedAt: string
    isReactivated: boolean
    reactivatedAt?: string
    newBusIdAfterReactivation?: string
    newBusNumberAfterReactivation?: string
    reactivationNotes?: string
}

export interface StudentLeave {
    id: string
    studentId: string
    studentName?: string
    startDate: string
    endDate: string
    reason: string
    attachmentUrl?: string
    attachmentFileName?: string
    isApproved: boolean
    approvedAt?: string
    isCancelled: boolean
    cancelledAt?: string
    cancellationReason?: string
    createdAt: string
}

export interface StudentTransfer {
    id: string
    studentId: string
    studentName?: string
    fromBusId: string
    fromBusNumber?: string
    toBusId: string
    toBusNumber?: string
    reason?: string
    transferredAt: string
    effectiveDate: string
}

export interface TimelineEvent {
    id: string
    eventType: string
    description: string
    occurredAt: string
    details: Record<string, any>
}

export interface SearchQuery {
    searchTerm?: string
    status?: string
    districtId?: string
    busId?: string
    periodId?: number
    pageNumber: number
    pageSize: number
}

export interface PagedResult<T> {
    items: T[]
    totalCount: number
    pageNumber: number
    pageSize: number
    totalPages: number
    hasPreviousPage: boolean
    hasNextPage: boolean
}

export const useStudentStore = defineStore('students', () => {
    // State
    const students = ref<Student[]>([])
    const selectedStudent = ref<Student | null>(null)
    const suspendedStudents = ref<StudentSuspension[]>([])
    const pendingLeaves = ref<StudentLeave[]>([])
    const loading = ref(false)
    const error = ref<string | null>(null)
    const pagination = ref({
        totalCount: 0,
        pageNumber: 1,
        pageSize: 50,
        totalPages: 0
    })

    // Search state
    const searchQuery = ref<SearchQuery>({
        searchTerm: '',
        status: '',
        districtId: '',
        pageNumber: 1,
        pageSize: 50
    })

    // Computed
    const hasStudents = computed(() => students.value.length > 0)
    const activeStudents = computed(() => students.value.filter(s => s.status?.toLowerCase() === 'active'))

    // Actions
    async function searchStudents(query?: Partial<SearchQuery>) {
        if (query) {
            searchQuery.value = { ...searchQuery.value, ...query }
        }

        loading.value = true
        error.value = null

        try {
            const params = new URLSearchParams()
            if (searchQuery.value.searchTerm) params.append('searchTerm', searchQuery.value.searchTerm)
            if (searchQuery.value.status) params.append('status', searchQuery.value.status)
            if (searchQuery.value.districtId) params.append('districtId', searchQuery.value.districtId)
            if (searchQuery.value.busId) params.append('busId', searchQuery.value.busId)
            if (searchQuery.value.periodId) params.append('periodId', String(searchQuery.value.periodId))
            params.append('pageNumber', String(searchQuery.value.pageNumber))
            params.append('pageSize', String(searchQuery.value.pageSize))

            const response = await apiClient.get(`/students?${params.toString()}`)
            if (response.data.success) {
                const result = response.data.data as PagedResult<Student>
                students.value = result.items
                pagination.value = {
                    totalCount: result.totalCount,
                    pageNumber: result.pageNumber,
                    pageSize: result.pageSize,
                    totalPages: result.totalPages
                }
            }
        } catch (err: any) {
            error.value = err.response?.data?.message || 'فشل في تحميل الطلاب'
            console.error('Error searching students:', err)
        } finally {
            loading.value = false
        }
    }

    async function getStudentById(id: string) {
        loading.value = true
        error.value = null

        try {
            const response = await apiClient.get(`/students/${id}`)
            if (response.data.success) {
                selectedStudent.value = response.data.data
                return response.data.data
            }
        } catch (err: any) {
            error.value = err.response?.data?.message || 'فشل في تحميل بيانات الطالب'
            console.error('Error fetching student:', err)
        } finally {
            loading.value = false
        }
        return null
    }

    async function getSuspendedStudents() {
        loading.value = true
        try {
            const response = await apiClient.get('/students/suspended')
            if (response.data.success) {
                suspendedStudents.value = response.data.data
            }
        } catch (err: any) {
            console.error('Error fetching suspended students:', err)
        } finally {
            loading.value = false
        }
    }

    async function getPendingLeaves() {
        loading.value = true
        try {
            const response = await apiClient.get('/students/leaves/pending')
            if (response.data.success) {
                pendingLeaves.value = response.data.data
            }
        } catch (err: any) {
            console.error('Error fetching pending leaves:', err)
        } finally {
            loading.value = false
        }
    }

    async function suspendStudent(studentId: string, reason?: string) {
        loading.value = true
        error.value = null

        try {
            const response = await apiClient.post('/students/suspensions', { studentId, reason })
            if (response.data.success) {
                await searchStudents()
                return response.data.data
            }
        } catch (err: any) {
            error.value = err.response?.data?.message || 'فشل في إيقاف الطالب'
            throw err
        } finally {
            loading.value = false
        }
    }

    async function reactivateStudent(suspensionId: string, newBusId?: string, notes?: string) {
        loading.value = true
        error.value = null

        try {
            const response = await apiClient.post(`/students/suspensions/${suspensionId}/reactivate`, { newBusId, notes })
            if (response.data.success) {
                await getSuspendedStudents()
                return response.data.data
            }
        } catch (err: any) {
            error.value = err.response?.data?.message || 'فشل في إعادة تفعيل الطالب'
            throw err
        } finally {
            loading.value = false
        }
    }

    async function createLeave(data: { studentId: string; startDate: string; endDate: string; reason: string; attachmentUrl?: string; attachmentFileName?: string }) {
        loading.value = true
        error.value = null

        try {
            const response = await apiClient.post('/students/leaves', data)
            if (response.data.success) {
                return response.data.data
            }
        } catch (err: any) {
            error.value = err.response?.data?.message || 'فشل في إنشاء الإجازة'
            throw err
        } finally {
            loading.value = false
        }
    }

    async function approveLeave(leaveId: string) {
        loading.value = true
        try {
            const response = await apiClient.post(`/students/leaves/${leaveId}/approve`)
            if (response.data.success) {
                await getPendingLeaves()
                return response.data.data
            }
        } catch (err: any) {
            error.value = err.response?.data?.message || 'فشل في الموافقة على الإجازة'
            throw err
        } finally {
            loading.value = false
        }
    }

    async function transferStudent(studentId: string, toBusId: string, reason?: string, effectiveDate?: string) {
        loading.value = true
        error.value = null

        try {
            const response = await apiClient.post('/students/transfers', { studentId, toBusId, reason, effectiveDate })
            if (response.data.success) {
                return response.data.data
            }
        } catch (err: any) {
            error.value = err.response?.data?.message || 'فشل في نقل الطالب'
            throw err
        } finally {
            loading.value = false
        }
    }

    async function assignStudentToBus(data: { studentId: string; busId: string; transportType?: string; arrivalBusId?: string; returnBusId?: string }) {
        loading.value = true
        error.value = null

        try {
            const response = await apiClient.post('/students/assignments', data)
            if (response.data.success) {
                return response.data.data
            }
        } catch (err: any) {
            error.value = err.response?.data?.message || 'فشل في تعيين الطالب للباص'
            throw err
        } finally {
            loading.value = false
        }
    }

    async function getStudentTimeline(studentId: string) {
        try {
            const response = await apiClient.get(`/students/${studentId}/timeline`)
            if (response.data.success) {
                return response.data.data as { studentId: string; studentName: string; events: TimelineEvent[] }
            }
        } catch (err: any) {
            console.error('Error fetching student timeline:', err)
        }
        return null
    }

    function resetSearch() {
        searchQuery.value = {
            searchTerm: '',
            status: '',
            districtId: '',
            pageNumber: 1,
            pageSize: 50
        }
        students.value = []
    }

    function clearError() {
        error.value = null
    }

    return {
        // State
        students,
        selectedStudent,
        suspendedStudents,
        pendingLeaves,
        loading,
        error,
        pagination,
        searchQuery,

        // Computed
        hasStudents,
        activeStudents,

        // Actions
        searchStudents,
        getStudentById,
        getSuspendedStudents,
        getPendingLeaves,
        suspendStudent,
        reactivateStudent,
        createLeave,
        approveLeave,
        transferStudent,
        assignStudentToBus,
        getStudentTimeline,
        resetSearch,
        clearError
    }
})
