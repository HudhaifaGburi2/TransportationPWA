<template>
  <dialog class="modal" :class="{ 'modal-open': isOpen }">
    <div class="modal-box max-w-lg">
      <button @click="close" class="btn btn-sm btn-circle btn-ghost absolute left-2 top-2">✕</button>
      <h3 class="font-bold text-lg flex items-center gap-2 text-warning">
        <Calendar class="w-5 h-5" />
        تسجيل إجازة
      </h3>

      <div v-if="student" class="mt-4 space-y-4">
        <div class="bg-base-200 rounded-lg p-4">
          <p class="font-semibold">{{ student.studentName }}</p>
          <p class="text-sm text-base-content/60">{{ student.studentId }}</p>
        </div>

        <div class="grid grid-cols-2 gap-4">
          <div class="form-control">
            <label class="label">
              <span class="label-text">تاريخ البداية <span class="text-error">*</span></span>
            </label>
            <input
              type="date"
              v-model="form.startDate"
              class="input input-bordered"
              :min="today"
            />
          </div>

          <div class="form-control">
            <label class="label">
              <span class="label-text">تاريخ الانتهاء <span class="text-error">*</span></span>
            </label>
            <input
              type="date"
              v-model="form.endDate"
              class="input input-bordered"
              :min="form.startDate || today"
            />
          </div>
        </div>

        <div class="form-control">
          <label class="label">
            <span class="label-text">سبب الإجازة <span class="text-error">*</span></span>
          </label>
          <textarea
            v-model="form.reason"
            class="textarea textarea-bordered h-20"
            placeholder="أدخل سبب الإجازة"
          ></textarea>
        </div>

        <div class="form-control">
          <label class="label">
            <span class="label-text">مرفق (اختياري)</span>
          </label>
          <input
            type="file"
            @change="handleFileChange"
            class="file-input file-input-bordered w-full"
            accept=".pdf,.jpg,.jpeg,.png"
          />
          <label class="label">
            <span class="label-text-alt text-base-content/50">PDF, JPG, PNG فقط</span>
          </label>
        </div>

        <div v-if="error" class="alert alert-error">
          <span>{{ error }}</span>
        </div>
      </div>

      <div class="modal-action">
        <button class="btn btn-ghost" @click="close" :disabled="loading">إلغاء</button>
        <button class="btn btn-warning" @click="submit" :disabled="loading || !isValid">
          <span v-if="loading" class="loading loading-spinner loading-sm"></span>
          تسجيل الإجازة
        </button>
      </div>
    </div>
    <div class="modal-backdrop bg-black/50" @click="close"></div>
  </dialog>
</template>

<script setup lang="ts">
import { ref, computed, watch } from 'vue'
import { Calendar } from 'lucide-vue-next'
import { useStudentStore } from '@/stores/students'

interface Student {
  id: string
  studentId: string
  studentName: string
}

const props = defineProps<{
  isOpen: boolean
  student: Student | null
}>()

const emit = defineEmits<{
  close: []
  success: [leave: any]
}>()

const studentStore = useStudentStore()
const loading = ref(false)
const error = ref('')
const attachment = ref<File | null>(null)

const form = ref({
  startDate: '',
  endDate: '',
  reason: ''
})

const today = computed(() => new Date().toISOString().split('T')[0])

const isValid = computed(() => {
  return form.value.startDate && form.value.endDate && form.value.reason.trim()
})

const handleFileChange = (event: Event) => {
  const target = event.target as HTMLInputElement
  if (target.files && target.files[0]) {
    attachment.value = target.files[0]
  }
}

const close = () => {
  form.value = { startDate: '', endDate: '', reason: '' }
  attachment.value = null
  error.value = ''
  emit('close')
}

const submit = async () => {
  if (!props.student || !isValid.value) return

  loading.value = true
  error.value = ''

  try {
    const result = await studentStore.createLeave({
      studentId: props.student.id,
      startDate: form.value.startDate,
      endDate: form.value.endDate,
      reason: form.value.reason,
      attachmentFileName: attachment.value?.name
    })
    emit('success', result)
    close()
  } catch (err: any) {
    error.value = err.response?.data?.message || 'فشل في تسجيل الإجازة'
  } finally {
    loading.value = false
  }
}

watch(() => props.isOpen, (newVal) => {
  if (!newVal) {
    form.value = { startDate: '', endDate: '', reason: '' }
    error.value = ''
  }
})
</script>
