<template>
  <MainLayout>
    <div class="space-y-6">
      <BackButton :to="`/students/${studentId}`" label="العودة لتفاصيل الطالب" />

      <PageHeader title="نقل الطالب" subtitle="نقل الطالب إلى باص آخر" :icon="ArrowLeftRight" />

      <div v-if="loading" class="flex justify-center py-16">
        <span class="loading loading-spinner loading-lg text-primary"></span>
      </div>

      <template v-else-if="student && currentAssignment">
        <div class="bg-base-100 rounded-xl shadow-sm p-6">
          <!-- Current Assignment -->
          <div class="mb-6">
            <h3 class="font-semibold mb-3">الوضع الحالي</h3>
            <div class="flex items-center gap-4 bg-base-200 rounded-lg p-4">
              <div class="avatar placeholder">
                <div class="bg-primary/10 text-primary rounded-full w-12 h-12">
                  <span class="font-bold">{{ student.studentName.split(' ').map((n: string) => n[0]).slice(0, 2).join('') }}</span>
                </div>
              </div>
              <div>
                <p class="font-semibold">{{ student.studentName }}</p>
                <p class="text-sm text-base-content/60">الباص الحالي: {{ currentAssignment.busNumber }}</p>
              </div>
            </div>
          </div>

          <!-- Transfer Form -->
          <div class="space-y-4">
            <div class="form-control">
              <label class="label">
                <span class="label-text">الباص الجديد <span class="text-error">*</span></span>
              </label>
              <select v-model="selectedBusId" class="select select-bordered w-full">
                <option value="">اختر الباص</option>
                <option
                  v-for="bus in availableBuses"
                  :key="bus.busId"
                  :value="bus.busId"
                  :disabled="bus.busId === currentAssignment.busId"
                >
                  باص {{ bus.busNumber }} ({{ bus.capacity }} مقعد)
                  {{ bus.busId === currentAssignment.busId ? '(الحالي)' : '' }}
                </option>
              </select>
            </div>

            <div class="form-control">
              <label class="label">
                <span class="label-text">سبب النقل</span>
              </label>
              <textarea
                v-model="reason"
                class="textarea textarea-bordered h-24"
                placeholder="أدخل سبب النقل (اختياري)"
              ></textarea>
            </div>

            <div class="form-control">
              <label class="label">
                <span class="label-text">تاريخ السريان</span>
              </label>
              <input
                type="date"
                v-model="effectiveDate"
                class="input input-bordered"
                :min="today"
              />
              <label class="label">
                <span class="label-text-alt text-base-content/50">اتركه فارغاً للتطبيق الفوري</span>
              </label>
            </div>

            <div v-if="error" class="alert alert-error">
              <span>{{ error }}</span>
            </div>

            <div class="flex justify-end gap-2 pt-4">
              <button class="btn btn-ghost" @click="$router.back()">إلغاء</button>
              <button
                class="btn btn-info"
                @click="confirmTransfer"
                :disabled="!selectedBusId || processing"
              >
                <span v-if="processing" class="loading loading-spinner loading-sm"></span>
                تأكيد النقل
              </button>
            </div>
          </div>
        </div>
      </template>

      <div v-else-if="!loading" class="text-center py-16">
        <XCircle class="w-16 h-16 mx-auto text-error/30 mb-4" />
        <p class="text-base-content/60">الطالب ليس لديه تعيين حالي للنقل منه</p>
      </div>
    </div>
  </MainLayout>
</template>

<script setup lang="ts">
import { ref, computed, onMounted } from 'vue'
import { useRoute, useRouter } from 'vue-router'
import { ArrowLeftRight, XCircle } from 'lucide-vue-next'
import MainLayout from '@/components/layout/MainLayout.vue'
import BackButton from '@/components/common/BackButton.vue'
import PageHeader from '@/components/common/PageHeader.vue'
import { useStudentStore, type Student, type StudentAssignment } from '@/stores/students'
import apiClient from '@/services/api/axios.config'

interface Bus {
  busId: string
  busNumber: string
  capacity: number
}

const route = useRoute()
const router = useRouter()
const studentStore = useStudentStore()

const studentId = computed(() => route.params.id as string)

const student = ref<Student | null>(null)
const currentAssignment = ref<StudentAssignment | null>(null)
const buses = ref<Bus[]>([])
const loading = ref(false)
const processing = ref(false)
const error = ref('')
const selectedBusId = ref('')
const reason = ref('')
const effectiveDate = ref('')

const today = computed(() => new Date().toISOString().split('T')[0])
const availableBuses = computed(() => buses.value.filter(b => b.busId !== currentAssignment.value?.busId))

const loadData = async () => {
  loading.value = true
  try {
    const [studentRes, assignmentRes, busesRes] = await Promise.all([
      apiClient.get(`/students/${studentId.value}`),
      apiClient.get(`/students/${studentId.value}/assignment`).catch(() => ({ data: { success: false } })),
      apiClient.get('/busmanagement/buses')
    ])

    if (studentRes.data.success) {
      student.value = studentRes.data.data
    }

    if (assignmentRes.data.success) {
      currentAssignment.value = assignmentRes.data.data
    }

    if (busesRes.data.success) {
      buses.value = busesRes.data.data
    }
  } catch (err) {
    console.error('Failed to load data:', err)
  } finally {
    loading.value = false
  }
}

const confirmTransfer = async () => {
  if (!selectedBusId.value) return

  processing.value = true
  error.value = ''

  try {
    await studentStore.transferStudent(
      studentId.value,
      selectedBusId.value,
      reason.value || undefined,
      effectiveDate.value || undefined
    )
    router.push(`/students/${studentId.value}`)
  } catch (err: any) {
    error.value = err.response?.data?.message || 'فشل في نقل الطالب'
  } finally {
    processing.value = false
  }
}

onMounted(loadData)
</script>
