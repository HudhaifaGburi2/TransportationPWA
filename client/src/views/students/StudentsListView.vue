<template>
  <MainLayout>
    <div class="space-y-6">
      <!-- Page Header -->
      <PageHeader title="إدارة الطلاب" subtitle="البحث وإدارة طلاب النقل" :icon="Users">
        <template #actions>
          <button @click="refreshStudents" class="btn btn-ghost btn-sm gap-2">
            <RefreshCw class="w-4 h-4" :class="{ 'animate-spin': loading }" />
            تحديث
          </button>
        </template>
      </PageHeader>

      <!-- Search and Filters -->
      <div class="bg-base-100 rounded-xl p-4 shadow-sm">
        <div class="flex flex-col md:flex-row gap-4">
          <div class="flex-1">
            <div class="relative">
              <Search class="absolute right-3 top-1/2 -translate-y-1/2 w-5 h-5 text-base-content/40" />
              <input
                v-model="searchTerm"
                type="text"
                placeholder="البحث بالاسم أو رقم الطالب أو الهاتف..."
                class="input input-bordered w-full pr-10"
                @input="onSearchInput"
              />
            </div>
          </div>

          <div class="flex gap-2">
            <select v-model="statusFilter" @change="applyFilters" class="select select-bordered">
              <option value="">كل الحالات</option>
              <option value="Active">نشط</option>
              <option value="Suspended">موقوف</option>
              <option value="OnLeave">في إجازة</option>
              <option value="Inactive">غير نشط</option>
            </select>

            <select v-model="districtFilter" @change="applyFilters" class="select select-bordered">
              <option value="">كل المناطق</option>
              <option v-for="d in districts" :key="d.id" :value="d.id">{{ d.districtNameAr }}</option>
            </select>
          </div>
        </div>
      </div>

      <!-- Stats Cards -->
      <div class="grid grid-cols-2 md:grid-cols-4 gap-4">
        <div class="stat bg-base-100 rounded-xl shadow-sm">
          <div class="stat-figure text-primary">
            <Users class="w-8 h-8" />
          </div>
          <div class="stat-title">إجمالي الطلاب</div>
          <div class="stat-value text-primary">{{ pagination.totalCount }}</div>
        </div>

        <div class="stat bg-base-100 rounded-xl shadow-sm cursor-pointer hover:bg-base-200" @click="filterByStatus('Active')">
          <div class="stat-figure text-success">
            <CheckCircle class="w-8 h-8" />
          </div>
          <div class="stat-title">نشط</div>
          <div class="stat-value text-success">{{ activeCount }}</div>
        </div>

        <div class="stat bg-base-100 rounded-xl shadow-sm cursor-pointer hover:bg-base-200" @click="goToSuspended">
          <div class="stat-figure text-error">
            <XCircle class="w-8 h-8" />
          </div>
          <div class="stat-title">موقوف</div>
          <div class="stat-value text-error">{{ suspendedCount }}</div>
        </div>

        <div class="stat bg-base-100 rounded-xl shadow-sm cursor-pointer hover:bg-base-200" @click="goToLeaves">
          <div class="stat-figure text-warning">
            <Calendar class="w-8 h-8" />
          </div>
          <div class="stat-title">في إجازة</div>
          <div class="stat-value text-warning">{{ onLeaveCount }}</div>
        </div>
      </div>

      <!-- Loading State -->
      <div v-if="loading" class="flex justify-center py-16">
        <span class="loading loading-spinner loading-lg text-primary"></span>
      </div>

      <!-- Error State -->
      <div v-else-if="error" class="text-center py-16">
        <XCircle class="w-16 h-16 mx-auto text-error/30 mb-4" />
        <p class="text-error">{{ error }}</p>
        <button @click="refreshStudents" class="btn btn-primary btn-sm mt-4">إعادة المحاولة</button>
      </div>

      <!-- Empty State -->
      <div v-else-if="students.length === 0" class="text-center py-16">
        <Users class="w-16 h-16 mx-auto text-base-content/20 mb-4" />
        <p class="text-base-content/60">لا توجد نتائج</p>
        <p class="text-sm text-base-content/40 mt-1">جرب تغيير معايير البحث</p>
      </div>

      <!-- Students Grid -->
      <div v-else class="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-4">
        <StudentCard
          v-for="student in students"
          :key="student.id"
          :student="student"
          @click="viewStudent(student)"
          @view="viewStudent"
        >
          <template #actions>
            <div class="dropdown dropdown-end">
              <label tabindex="0" class="btn btn-ghost btn-sm">
                <MoreVertical class="w-4 h-4" />
              </label>
              <ul tabindex="0" class="dropdown-content z-10 menu p-2 shadow bg-base-100 rounded-box w-52">
                <li><a @click.stop="viewStudent(student)"><Eye class="w-4 h-4" /> عرض التفاصيل</a></li>
                <li v-if="student.status === 'Active'">
                  <a @click.stop="openSuspendDialog(student)" class="text-error">
                    <XCircle class="w-4 h-4" /> إيقاف
                  </a>
                </li>
                <li v-if="student.status === 'Active'">
                  <a @click.stop="openLeaveDialog(student)" class="text-warning">
                    <Calendar class="w-4 h-4" /> تسجيل إجازة
                  </a>
                </li>
                <li v-if="student.status === 'Active'">
                  <a @click.stop="openTransferDialog(student)" class="text-info">
                    <ArrowLeftRight class="w-4 h-4" /> نقل لباص آخر
                  </a>
                </li>
              </ul>
            </div>
          </template>
        </StudentCard>
      </div>

      <!-- Pagination -->
      <div v-if="pagination.totalPages > 1" class="flex justify-center gap-2">
        <button
          class="btn btn-sm"
          :disabled="!pagination.hasPreviousPage"
          @click="goToPage(pagination.pageNumber - 1)"
        >
          السابق
        </button>
        <span class="btn btn-sm btn-ghost">
          {{ pagination.pageNumber }} / {{ pagination.totalPages }}
        </span>
        <button
          class="btn btn-sm"
          :disabled="!pagination.hasNextPage"
          @click="goToPage(pagination.pageNumber + 1)"
        >
          التالي
        </button>
      </div>
    </div>

    <!-- Dialogs -->
    <SuspensionDialog
      :is-open="showSuspendDialog"
      :student="selectedStudent"
      @close="showSuspendDialog = false"
      @success="onSuspendSuccess"
    />

    <LeaveFormDialog
      :is-open="showLeaveDialog"
      :student="selectedStudent"
      @close="showLeaveDialog = false"
      @success="onLeaveSuccess"
    />
  </MainLayout>
</template>

<script setup lang="ts">
import { ref, computed, onMounted, watch } from 'vue'
import { useRouter } from 'vue-router'
import { 
  Users, Search, RefreshCw, CheckCircle, XCircle, Calendar, 
  MoreVertical, Eye, ArrowLeftRight 
} from 'lucide-vue-next'
import MainLayout from '@/components/layout/MainLayout.vue'
import PageHeader from '@/components/common/PageHeader.vue'
import StudentCard from '@/components/students/StudentCard.vue'
import SuspensionDialog from '@/components/students/SuspensionDialog.vue'
import LeaveFormDialog from '@/components/students/LeaveFormDialog.vue'
import { useStudentStore, type Student } from '@/stores/students'
import { useDebouncedFn } from '@/composables/useDebounce'
import apiClient from '@/services/api/axios.config'

const router = useRouter()
const studentStore = useStudentStore()

// State
const searchTerm = ref('')
const statusFilter = ref('')
const districtFilter = ref('')
const districts = ref<{ id: string; districtNameAr: string }[]>([])
const showSuspendDialog = ref(false)
const showLeaveDialog = ref(false)
const selectedStudent = ref<Student | null>(null)

// Computed from store
const students = computed(() => studentStore.students)
const loading = computed(() => studentStore.loading)
const error = computed(() => studentStore.error)
const pagination = computed(() => ({
  ...studentStore.pagination,
  hasPreviousPage: studentStore.pagination.pageNumber > 1,
  hasNextPage: studentStore.pagination.pageNumber < studentStore.pagination.totalPages
}))

// Stats
const activeCount = computed(() => students.value.filter(s => s.status?.toLowerCase() === 'active').length)
const suspendedCount = computed(() => students.value.filter(s => s.status?.toLowerCase() === 'suspended').length)
const onLeaveCount = computed(() => students.value.filter(s => s.status?.toLowerCase() === 'onleave').length)

// Methods
const fetchDistricts = async () => {
  try {
    const response = await apiClient.get('/districts')
    if (response.data.success) {
      districts.value = response.data.data
    }
  } catch (err) {
    console.error('Failed to fetch districts:', err)
  }
}

const refreshStudents = () => {
  studentStore.searchStudents({
    searchTerm: searchTerm.value,
    status: statusFilter.value,
    districtId: districtFilter.value
  })
}

const debouncedSearch = useDebouncedFn(refreshStudents, 400)

const onSearchInput = () => {
  debouncedSearch()
}

const applyFilters = () => {
  refreshStudents()
}

const filterByStatus = (status: string) => {
  statusFilter.value = status
  applyFilters()
}

const goToPage = (page: number) => {
  studentStore.searchStudents({ pageNumber: page })
}

const viewStudent = (student: Student) => {
  router.push(`/students/${student.id}`)
}

const goToSuspended = () => {
  router.push('/students/suspended')
}

const goToLeaves = () => {
  router.push('/students/leaves')
}

const openSuspendDialog = (student: Student) => {
  selectedStudent.value = student
  showSuspendDialog.value = true
}

const openLeaveDialog = (student: Student) => {
  selectedStudent.value = student
  showLeaveDialog.value = true
}

const openTransferDialog = (student: Student) => {
  router.push(`/students/${student.id}/transfer`)
}

const onSuspendSuccess = () => {
  refreshStudents()
}

const onLeaveSuccess = () => {
  refreshStudents()
}

onMounted(() => {
  fetchDistricts()
  refreshStudents()
})
</script>
