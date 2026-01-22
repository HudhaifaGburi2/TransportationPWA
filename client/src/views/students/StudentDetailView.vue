<template>
  <MainLayout>
    <div class="space-y-6">
      <!-- Back Button -->
      <BackButton to="/students" label="العودة للطلاب" />

      <!-- Loading State -->
      <div v-if="loading" class="flex justify-center py-16">
        <span class="loading loading-spinner loading-lg text-primary"></span>
      </div>

      <!-- Error State -->
      <div v-else-if="error" class="text-center py-16">
        <XCircle class="w-16 h-16 mx-auto text-error/30 mb-4" />
        <p class="text-error">{{ error }}</p>
        <button @click="loadStudent" class="btn btn-primary btn-sm mt-4">إعادة المحاولة</button>
      </div>

      <!-- Student Details -->
      <template v-else-if="student">
        <!-- Header Card -->
        <div class="bg-base-100 rounded-xl shadow-sm p-6">
          <div class="flex flex-col md:flex-row md:items-center md:justify-between gap-4">
            <div class="flex items-center gap-4">
              <div class="avatar placeholder">
                <div class="bg-primary/10 text-primary rounded-full w-16 h-16">
                  <span class="text-2xl font-bold">{{ initials }}</span>
                </div>
              </div>
              <div>
                <h1 class="text-2xl font-bold">{{ student.studentName }}</h1>
                <p class="text-base-content/60">{{ student.studentId }}</p>
                <StudentStatusBadge :status="student.status" class="mt-2" />
              </div>
            </div>

            <div class="flex gap-2">
              <button
                v-if="student.status === 'Active'"
                class="btn btn-error btn-sm gap-2"
                @click="showSuspendDialog = true"
              >
                <XCircle class="w-4 h-4" />
                إيقاف
              </button>
              <button
                v-if="student.status === 'Active'"
                class="btn btn-warning btn-sm gap-2"
                @click="showLeaveDialog = true"
              >
                <Calendar class="w-4 h-4" />
                إجازة
              </button>
              <button
                v-if="student.status === 'Active'"
                class="btn btn-info btn-sm gap-2"
                @click="$router.push(`/students/${student.id}/transfer`)"
              >
                <ArrowLeftRight class="w-4 h-4" />
                نقل
              </button>
            </div>
          </div>
        </div>

        <!-- Info Cards Grid -->
        <div class="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-4">
          <!-- Contact Info -->
          <div class="bg-base-100 rounded-xl shadow-sm p-4">
            <h3 class="font-semibold mb-3 flex items-center gap-2">
              <Phone class="w-4 h-4" />
              معلومات الاتصال
            </h3>
            <div class="space-y-2 text-sm">
              <div v-if="student.phoneNumber" class="flex justify-between">
                <span class="text-base-content/60">الهاتف</span>
                <span dir="ltr">{{ student.phoneNumber }}</span>
              </div>
              <div v-if="student.homeAddress" class="flex justify-between">
                <span class="text-base-content/60">العنوان</span>
                <span>{{ student.homeAddress }}</span>
              </div>
            </div>
          </div>

          <!-- Halaqa Info -->
          <div class="bg-base-100 rounded-xl shadow-sm p-4">
            <h3 class="font-semibold mb-3 flex items-center gap-2">
              <BookOpen class="w-4 h-4" />
              معلومات الحلقة
            </h3>
            <div class="space-y-2 text-sm">
              <div v-if="student.teacherName" class="flex justify-between">
                <span class="text-base-content/60">المعلم</span>
                <span>{{ student.teacherName }}</span>
              </div>
              <div v-if="student.halaqaTypeCode" class="flex justify-between">
                <span class="text-base-content/60">نوع الحلقة</span>
                <span>{{ student.halaqaTypeCode }}</span>
              </div>
              <div v-if="student.periodId" class="flex justify-between">
                <span class="text-base-content/60">الفترة</span>
                <span>{{ getPeriodName(student.periodId) }}</span>
              </div>
            </div>
          </div>

          <!-- District Info -->
          <div class="bg-base-100 rounded-xl shadow-sm p-4">
            <h3 class="font-semibold mb-3 flex items-center gap-2">
              <MapPin class="w-4 h-4" />
              المنطقة والنقل
            </h3>
            <div class="space-y-2 text-sm">
              <div v-if="student.districtName" class="flex justify-between">
                <span class="text-base-content/60">المنطقة</span>
                <span>{{ student.districtName }}</span>
              </div>
              <div v-if="assignment" class="flex justify-between">
                <span class="text-base-content/60">الباص</span>
                <span class="badge badge-primary">{{ assignment.busNumber }}</span>
              </div>
              <div v-if="assignment">
                <TransportTypeBadge
                  :transport-type="assignment.transportType"
                  :arrival-bus="assignment.arrivalBusNumber"
                  :return-bus="assignment.returnBusNumber"
                />
              </div>
            </div>
          </div>
        </div>

        <!-- Tabs -->
        <div class="tabs tabs-boxed bg-base-100 p-1 rounded-xl">
          <a
            class="tab"
            :class="{ 'tab-active': activeTab === 'timeline' }"
            @click="activeTab = 'timeline'"
          >
            سجل الأحداث
          </a>
          <a
            class="tab"
            :class="{ 'tab-active': activeTab === 'suspensions' }"
            @click="activeTab = 'suspensions'"
          >
            سجل الإيقافات
          </a>
          <a
            class="tab"
            :class="{ 'tab-active': activeTab === 'leaves' }"
            @click="activeTab = 'leaves'"
          >
            سجل الإجازات
          </a>
          <a
            class="tab"
            :class="{ 'tab-active': activeTab === 'transfers' }"
            @click="activeTab = 'transfers'"
          >
            سجل النقل
          </a>
        </div>

        <!-- Tab Content -->
        <div class="bg-base-100 rounded-xl shadow-sm p-6">
          <StudentTimeline
            v-if="activeTab === 'timeline'"
            :events="timeline?.events || []"
            :loading="loadingTimeline"
          />

          <!-- Suspensions Tab -->
          <div v-else-if="activeTab === 'suspensions'">
            <div v-if="suspensions.length === 0" class="text-center py-8 text-base-content/50">
              لا يوجد سجل إيقافات
            </div>
            <div v-else class="space-y-3">
              <div v-for="s in suspensions" :key="s.id" class="border border-base-300 rounded-lg p-4">
                <div class="flex justify-between items-start">
                  <div>
                    <span class="badge" :class="s.isReactivated ? 'badge-success' : 'badge-error'">
                      {{ s.isReactivated ? 'تم التفعيل' : 'موقوف' }}
                    </span>
                    <p class="mt-2">{{ s.reason }}</p>
                  </div>
                  <span class="text-xs text-base-content/50">{{ formatDate(s.suspendedAt) }}</span>
                </div>
              </div>
            </div>
          </div>

          <!-- Leaves Tab -->
          <div v-else-if="activeTab === 'leaves'">
            <div v-if="leaves.length === 0" class="text-center py-8 text-base-content/50">
              لا يوجد سجل إجازات
            </div>
            <div v-else class="space-y-3">
              <div v-for="l in leaves" :key="l.id" class="border border-base-300 rounded-lg p-4">
                <div class="flex justify-between items-start">
                  <div>
                    <span class="badge" :class="l.isApproved ? 'badge-success' : l.isCancelled ? 'badge-error' : 'badge-warning'">
                      {{ l.isApproved ? 'موافق عليها' : l.isCancelled ? 'ملغاة' : 'قيد المراجعة' }}
                    </span>
                    <p class="mt-2">{{ l.reason }}</p>
                    <p class="text-sm text-base-content/60 mt-1">
                      {{ formatDate(l.startDate) }} - {{ formatDate(l.endDate) }}
                    </p>
                  </div>
                </div>
              </div>
            </div>
          </div>

          <!-- Transfers Tab -->
          <div v-else-if="activeTab === 'transfers'">
            <div v-if="transfers.length === 0" class="text-center py-8 text-base-content/50">
              لا يوجد سجل نقل
            </div>
            <div v-else class="space-y-3">
              <div v-for="t in transfers" :key="t.id" class="border border-base-300 rounded-lg p-4">
                <div class="flex justify-between items-start">
                  <div>
                    <div class="flex items-center gap-2">
                      <span class="badge badge-ghost">{{ t.fromBusNumber }}</span>
                      <ArrowLeftRight class="w-4 h-4" />
                      <span class="badge badge-primary">{{ t.toBusNumber }}</span>
                    </div>
                    <p v-if="t.reason" class="mt-2 text-sm">{{ t.reason }}</p>
                  </div>
                  <span class="text-xs text-base-content/50">{{ formatDate(t.transferredAt) }}</span>
                </div>
              </div>
            </div>
          </div>
        </div>
      </template>
    </div>

    <!-- Dialogs -->
    <SuspensionDialog
      :is-open="showSuspendDialog"
      :student="student"
      @close="showSuspendDialog = false"
      @success="onSuspendSuccess"
    />

    <LeaveFormDialog
      :is-open="showLeaveDialog"
      :student="student"
      @close="showLeaveDialog = false"
      @success="onLeaveSuccess"
    />
  </MainLayout>
</template>

<script setup lang="ts">
import { ref, computed, onMounted } from 'vue'
import { useRoute, useRouter } from 'vue-router'
import { XCircle, Calendar, ArrowLeftRight, Phone, BookOpen, MapPin } from 'lucide-vue-next'
import MainLayout from '@/components/layout/MainLayout.vue'
import BackButton from '@/components/common/BackButton.vue'
import StudentStatusBadge from '@/components/students/StudentStatusBadge.vue'
import TransportTypeBadge from '@/components/students/TransportTypeBadge.vue'
import StudentTimeline from '@/components/students/StudentTimeline.vue'
import SuspensionDialog from '@/components/students/SuspensionDialog.vue'
import LeaveFormDialog from '@/components/students/LeaveFormDialog.vue'
import { useStudentStore, type Student, type StudentAssignment, type StudentSuspension, type StudentLeave, type StudentTransfer, type TimelineEvent } from '@/stores/students'
import apiClient from '@/services/api/axios.config'

const route = useRoute()
const router = useRouter()
const studentStore = useStudentStore()

const studentId = computed(() => route.params.id as string)

// State
const student = ref<Student | null>(null)
const assignment = ref<StudentAssignment | null>(null)
const suspensions = ref<StudentSuspension[]>([])
const leaves = ref<StudentLeave[]>([])
const transfers = ref<StudentTransfer[]>([])
const timeline = ref<{ studentId: string; studentName: string; events: TimelineEvent[] } | null>(null)
const loading = ref(false)
const loadingTimeline = ref(false)
const error = ref('')
const activeTab = ref('timeline')
const showSuspendDialog = ref(false)
const showLeaveDialog = ref(false)

const initials = computed(() => {
  const name = student.value?.studentName || ''
  return name.split(' ').map(n => n[0]).slice(0, 2).join('')
})

const periods: Record<number, string> = {
  1: 'العصر',
  2: 'المغرب',
  3: 'الضحى'
}

const getPeriodName = (id: number) => periods[id] || `فترة ${id}`

const formatDate = (dateStr: string) => {
  return new Date(dateStr).toLocaleDateString('ar-SA', {
    year: 'numeric',
    month: 'short',
    day: 'numeric'
  })
}

const loadStudent = async () => {
  loading.value = true
  error.value = ''

  try {
    const response = await apiClient.get(`/students/${studentId.value}`)
    if (response.data.success) {
      student.value = response.data.data
    }
  } catch (err: any) {
    error.value = err.response?.data?.message || 'فشل في تحميل بيانات الطالب'
  } finally {
    loading.value = false
  }
}

const loadAssignment = async () => {
  try {
    const response = await apiClient.get(`/students/${studentId.value}/assignment`)
    if (response.data.success) {
      assignment.value = response.data.data
    }
  } catch (err) {
    // No assignment exists, which is fine
  }
}

const loadTimeline = async () => {
  loadingTimeline.value = true
  try {
    timeline.value = await studentStore.getStudentTimeline(studentId.value)
  } finally {
    loadingTimeline.value = false
  }
}

const loadHistory = async () => {
  try {
    const [suspRes, leavesRes, transfersRes] = await Promise.all([
      apiClient.get(`/students/${studentId.value}/suspensions`),
      apiClient.get(`/students/${studentId.value}/leaves`),
      apiClient.get(`/students/${studentId.value}/transfers`)
    ])

    if (suspRes.data.success) suspensions.value = suspRes.data.data
    if (leavesRes.data.success) leaves.value = leavesRes.data.data
    if (transfersRes.data.success) transfers.value = transfersRes.data.data
  } catch (err) {
    console.error('Failed to load history:', err)
  }
}

const onSuspendSuccess = () => {
  loadStudent()
  loadTimeline()
  loadHistory()
}

const onLeaveSuccess = () => {
  loadStudent()
  loadTimeline()
  loadHistory()
}

onMounted(async () => {
  await loadStudent()
  loadAssignment()
  loadTimeline()
  loadHistory()
})
</script>
