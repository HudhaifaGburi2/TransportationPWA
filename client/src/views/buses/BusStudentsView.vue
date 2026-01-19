<template>
  <div class="p-6 space-y-6">
    <!-- Back Navigation -->
    <div class="flex items-center gap-4">
      <router-link to="/buses" class="btn btn-ghost btn-sm gap-2">
        <ArrowRight class="w-4 h-4" />
        قائمة الباصات
      </router-link>
      <div class="text-sm breadcrumbs">
        <ul>
          <li><router-link to="/">الرئيسية</router-link></li>
          <li><router-link to="/buses">الباصات</router-link></li>
          <li>طلاب الباص</li>
        </ul>
      </div>
    </div>

    <!-- Page Header -->
    <div class="bg-gradient-to-l from-info/10 to-transparent p-6 rounded-xl">
      <div class="flex flex-col md:flex-row md:items-center md:justify-between gap-4">
        <div>
          <h1 class="text-2xl font-bold text-base-content flex items-center gap-3">
            <div class="p-2 bg-info/20 rounded-lg">
              <Users class="w-6 h-6 text-info" />
            </div>
            طلاب الباص {{ busNumber }}
          </h1>
          <p class="text-base-content/60 mt-1">عرض الطلاب المسجلين في هذا الباص</p>
        </div>
        <div class="flex gap-2">
          <button @click="fetchStudents" class="btn btn-ghost gap-2">
            <RefreshCw class="w-5 h-5" :class="{ 'animate-spin': loading }" />
            تحديث
          </button>
        </div>
      </div>
    </div>

    <!-- Stats -->
    <div class="stats shadow w-full">
      <div class="stat">
        <div class="stat-figure text-primary">
          <Bus class="w-8 h-8" />
        </div>
        <div class="stat-title">رقم الباص</div>
        <div class="stat-value text-primary">{{ busNumber }}</div>
      </div>
      <div class="stat">
        <div class="stat-figure text-info">
          <Users class="w-8 h-8" />
        </div>
        <div class="stat-title">عدد الطلاب</div>
        <div class="stat-value text-info">{{ students.length }}</div>
      </div>
      <div class="stat">
        <div class="stat-figure text-success">
          <CheckCircle class="w-8 h-8" />
        </div>
        <div class="stat-title">السعة</div>
        <div class="stat-value text-success">{{ busCapacity }}</div>
      </div>
    </div>

    <!-- Students Table -->
    <div class="bg-base-100 rounded-xl shadow-sm border border-base-200 overflow-hidden">
      <div v-if="loading" class="flex justify-center py-16">
        <span class="loading loading-spinner loading-lg text-primary"></span>
      </div>

      <div v-else-if="error" class="text-center py-16">
        <div class="text-error/30 mb-4">
          <XCircle class="w-16 h-16 mx-auto" />
        </div>
        <p class="text-error text-lg">{{ error }}</p>
        <button @click="fetchStudents" class="btn btn-primary btn-sm mt-4">إعادة المحاولة</button>
      </div>

      <div v-else-if="students.length === 0" class="text-center py-16">
        <div class="text-base-content/30 mb-4">
          <Users class="w-16 h-16 mx-auto" />
        </div>
        <p class="text-base-content/60 text-lg">لا يوجد طلاب مسجلين في هذا الباص</p>
      </div>

      <div v-else class="overflow-x-auto">
        <table class="table table-zebra">
          <thead class="bg-base-200">
            <tr>
              <th class="font-bold">#</th>
              <th class="font-bold">اسم الطالب</th>
              <th class="font-bold">رقم الطالب</th>
              <th class="font-bold">المنطقة</th>
              <th class="font-bold">الفترة</th>
              <th class="font-bold text-center">تاريخ التسجيل</th>
            </tr>
          </thead>
          <tbody>
            <tr v-for="(student, index) in students" :key="student.id" class="hover">
              <td class="font-medium">{{ index + 1 }}</td>
              <td>
                <div class="flex items-center gap-3">
                  <div class="avatar placeholder">
                    <div class="bg-primary/10 text-primary rounded-full w-10">
                      <span class="text-sm">{{ getInitials(student.studentName) }}</span>
                    </div>
                  </div>
                  <div>
                    <div class="font-semibold">{{ student.studentName }}</div>
                  </div>
                </div>
              </td>
              <td class="font-mono">{{ student.studentId }}</td>
              <td>{{ student.districtName || '-' }}</td>
              <td>{{ student.periodName || '-' }}</td>
              <td class="text-center text-sm">{{ formatDate(student.assignedAt) }}</td>
            </tr>
          </tbody>
        </table>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, onMounted } from 'vue'
import { useRoute } from 'vue-router'
import { Users, Bus, RefreshCw, CheckCircle, XCircle, ArrowRight } from 'lucide-vue-next'
import apiClient from '@/services/api/axios.config'

interface StudentAssignment {
  id: string
  studentId: string
  studentName: string
  districtName?: string
  periodName?: string
  assignedAt: string
}

const route = useRoute()
const busId = route.params.id as string

const students = ref<StudentAssignment[]>([])
const busNumber = ref('')
const busCapacity = ref(0)
const loading = ref(false)
const error = ref<string | null>(null)

const fetchStudents = async () => {
  loading.value = true
  error.value = null
  try {
    const response = await apiClient.get(`/busmanagement/buses/${busId}/students`)
    if (response.data.success && response.data.data) {
      const data = response.data.data
      busNumber.value = data.busNumber || ''
      busCapacity.value = data.capacity || 0
      students.value = (data.students || []).map((s: any) => ({
        id: s.id || s.studentId,
        studentId: s.studentId,
        studentName: s.studentName,
        districtName: s.districtName,
        periodName: s.periodName,
        assignedAt: s.assignedAt || s.createdAt
      }))
    }
  } catch (err: any) {
    console.error('Error fetching students:', err)
    if (err.response?.status === 403) {
      error.value = 'ليس لديك صلاحية للوصول لهذه الصفحة'
    } else if (err.response?.status === 404) {
      error.value = 'الباص غير موجود'
    } else {
      error.value = err.response?.data?.message || 'فشل في تحميل بيانات الطلاب'
    }
  } finally {
    loading.value = false
  }
}

const getInitials = (name: string) => {
  return name?.split(' ').map(n => n[0]).slice(0, 2).join('') || '?'
}

const formatDate = (dateStr: string) => {
  if (!dateStr) return '-'
  return new Date(dateStr).toLocaleDateString('ar-SA', {
    year: 'numeric',
    month: 'short',
    day: 'numeric'
  })
}

onMounted(fetchStudents)
</script>
