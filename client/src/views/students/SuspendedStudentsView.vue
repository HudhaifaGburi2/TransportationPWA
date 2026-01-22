<template>
  <MainLayout>
    <div class="space-y-6">
      <BackButton to="/students" label="العودة للطلاب" />

      <PageHeader title="الطلاب الموقوفون" subtitle="قائمة الطلاب الموقوفين عن النقل" :icon="XCircle" />

      <div v-if="loading" class="flex justify-center py-16">
        <span class="loading loading-spinner loading-lg text-primary"></span>
      </div>

      <div v-else-if="suspensions.length === 0" class="text-center py-16">
        <CheckCircle class="w-16 h-16 mx-auto text-success/30 mb-4" />
        <p class="text-base-content/60">لا يوجد طلاب موقوفون حالياً</p>
      </div>

      <div v-else class="grid gap-4">
        <div v-for="s in suspensions" :key="s.id" class="bg-base-100 rounded-xl shadow-sm p-4">
          <div class="flex items-start justify-between">
            <div class="flex items-center gap-3">
              <div class="avatar placeholder">
                <div class="bg-error/10 text-error rounded-full w-12 h-12">
                  <XCircle class="w-6 h-6" />
                </div>
              </div>
              <div>
                <h3 class="font-semibold">{{ s.studentName }}</h3>
                <p class="text-sm text-base-content/60">{{ s.reason }}</p>
                <p class="text-xs text-base-content/40 mt-1">
                  تاريخ الإيقاف: {{ formatDate(s.suspendedAt) }}
                </p>
              </div>
            </div>
            <button class="btn btn-success btn-sm gap-2" @click="openReactivateDialog(s)">
              <CheckCircle class="w-4 h-4" />
              إعادة التفعيل
            </button>
          </div>
        </div>
      </div>
    </div>

    <ReactivationDialog
      :is-open="showReactivateDialog"
      :suspension="selectedSuspension"
      @close="showReactivateDialog = false"
      @success="onReactivateSuccess"
    />
  </MainLayout>
</template>

<script setup lang="ts">
import { ref, onMounted } from 'vue'
import { XCircle, CheckCircle } from 'lucide-vue-next'
import MainLayout from '@/components/layout/MainLayout.vue'
import BackButton from '@/components/common/BackButton.vue'
import PageHeader from '@/components/common/PageHeader.vue'
import ReactivationDialog from '@/components/students/ReactivationDialog.vue'
import { useStudentStore, type StudentSuspension } from '@/stores/students'

const studentStore = useStudentStore()
const suspensions = ref<StudentSuspension[]>([])
const loading = ref(false)
const showReactivateDialog = ref(false)
const selectedSuspension = ref<StudentSuspension | null>(null)

const formatDate = (dateStr: string) => {
  return new Date(dateStr).toLocaleDateString('ar-SA', {
    year: 'numeric',
    month: 'short',
    day: 'numeric'
  })
}

const loadSuspensions = async () => {
  loading.value = true
  await studentStore.getSuspendedStudents()
  suspensions.value = studentStore.suspendedStudents
  loading.value = false
}

const openReactivateDialog = (suspension: StudentSuspension) => {
  selectedSuspension.value = suspension
  showReactivateDialog.value = true
}

const onReactivateSuccess = () => {
  loadSuspensions()
}

onMounted(loadSuspensions)
</script>
