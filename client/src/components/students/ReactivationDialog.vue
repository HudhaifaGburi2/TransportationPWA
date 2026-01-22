<template>
  <dialog class="modal" :class="{ 'modal-open': isOpen }">
    <div class="modal-box">
      <button @click="close" class="btn btn-sm btn-circle btn-ghost absolute left-2 top-2">✕</button>
      <h3 class="font-bold text-lg flex items-center gap-2 text-success">
        <CheckCircle class="w-5 h-5" />
        إعادة تفعيل الطالب
      </h3>

      <div v-if="suspension" class="mt-4 space-y-4">
        <div class="bg-base-200 rounded-lg p-4">
          <p class="font-semibold">{{ suspension.studentName }}</p>
          <p class="text-sm text-base-content/60">سبب الإيقاف: {{ suspension.reason }}</p>
          <p class="text-xs text-base-content/40 mt-1">
            تاريخ الإيقاف: {{ formatDate(suspension.suspendedAt) }}
          </p>
        </div>

        <div class="form-control">
          <label class="label">
            <span class="label-text">تعيين باص جديد</span>
          </label>
          <select v-model="selectedBusId" class="select select-bordered w-full">
            <option value="">بدون تعيين باص</option>
            <option v-for="bus in buses" :key="bus.busId" :value="bus.busId">
              باص {{ bus.busNumber }} ({{ bus.capacity }} مقعد)
            </option>
          </select>
        </div>

        <div class="form-control">
          <label class="label">
            <span class="label-text">ملاحظات</span>
          </label>
          <textarea
            v-model="notes"
            class="textarea textarea-bordered"
            placeholder="ملاحظات إضافية (اختياري)"
          ></textarea>
        </div>
      </div>

      <div class="modal-action">
        <button class="btn btn-ghost" @click="close" :disabled="loading">إلغاء</button>
        <button class="btn btn-success" @click="confirm" :disabled="loading">
          <span v-if="loading" class="loading loading-spinner loading-sm"></span>
          تأكيد التفعيل
        </button>
      </div>
    </div>
    <div class="modal-backdrop bg-black/50" @click="close"></div>
  </dialog>
</template>

<script setup lang="ts">
import { ref, onMounted } from 'vue'
import { CheckCircle } from 'lucide-vue-next'
import { useStudentStore, type StudentSuspension } from '@/stores/students'
import apiClient from '@/services/api/axios.config'

interface Bus {
  busId: string
  busNumber: string
  capacity: number
}

const props = defineProps<{
  isOpen: boolean
  suspension: StudentSuspension | null
}>()

const emit = defineEmits<{
  close: []
  success: [result: any]
}>()

const studentStore = useStudentStore()
const buses = ref<Bus[]>([])
const selectedBusId = ref('')
const notes = ref('')
const loading = ref(false)

const fetchBuses = async () => {
  try {
    const response = await apiClient.get('/busmanagement/buses')
    if (response.data.success) {
      buses.value = response.data.data
    }
  } catch (err) {
    console.error('Failed to fetch buses:', err)
  }
}

const formatDate = (dateStr: string) => {
  return new Date(dateStr).toLocaleDateString('ar-SA', {
    year: 'numeric',
    month: 'short',
    day: 'numeric'
  })
}

const close = () => {
  selectedBusId.value = ''
  notes.value = ''
  emit('close')
}

const confirm = async () => {
  if (!props.suspension) return

  loading.value = true
  try {
    const result = await studentStore.reactivateStudent(
      props.suspension.id,
      selectedBusId.value || undefined,
      notes.value || undefined
    )
    emit('success', result)
    close()
  } catch (err) {
    console.error('Failed to reactivate student:', err)
  } finally {
    loading.value = false
  }
}

onMounted(() => {
  fetchBuses()
})
</script>
