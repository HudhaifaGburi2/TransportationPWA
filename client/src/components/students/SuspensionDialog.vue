<template>
  <dialog class="modal" :class="{ 'modal-open': isOpen }">
    <div class="modal-box">
      <button @click="close" class="btn btn-sm btn-circle btn-ghost absolute left-2 top-2">✕</button>
      <h3 class="font-bold text-lg flex items-center gap-2 text-error">
        <XCircle class="w-5 h-5" />
        إيقاف الطالب
      </h3>

      <div v-if="student" class="mt-4">
        <div class="bg-base-200 rounded-lg p-4 mb-4">
          <p class="font-semibold">{{ student.studentName }}</p>
          <p class="text-sm text-base-content/60">{{ student.studentId }}</p>
        </div>

        <div class="form-control">
          <label class="label">
            <span class="label-text">سبب الإيقاف</span>
          </label>
          <textarea
            v-model="reason"
            class="textarea textarea-bordered h-24"
            placeholder="غياب لمدة ثلاثة أيام متتالية"
          ></textarea>
          <label class="label">
            <span class="label-text-alt text-base-content/50">اتركه فارغاً لاستخدام السبب الافتراضي</span>
          </label>
        </div>
      </div>

      <div class="modal-action">
        <button class="btn btn-ghost" @click="close" :disabled="loading">إلغاء</button>
        <button class="btn btn-error" @click="confirm" :disabled="loading">
          <span v-if="loading" class="loading loading-spinner loading-sm"></span>
          تأكيد الإيقاف
        </button>
      </div>
    </div>
    <div class="modal-backdrop bg-black/50" @click="close"></div>
  </dialog>
</template>

<script setup lang="ts">
import { ref } from 'vue'
import { XCircle } from 'lucide-vue-next'
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
  success: [suspension: any]
}>()

const studentStore = useStudentStore()
const reason = ref('')
const loading = ref(false)

const close = () => {
  reason.value = ''
  emit('close')
}

const confirm = async () => {
  if (!props.student) return

  loading.value = true
  try {
    const result = await studentStore.suspendStudent(props.student.id, reason.value || undefined)
    emit('success', result)
    close()
  } catch (err) {
    console.error('Failed to suspend student:', err)
  } finally {
    loading.value = false
  }
}
</script>
