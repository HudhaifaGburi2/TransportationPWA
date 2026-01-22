<template>
  <MainLayout>
    <div class="space-y-6">
      <BackButton to="/students" label="العودة للطلاب" />

      <PageHeader title="طلبات الإجازات" subtitle="إدارة إجازات الطلاب" :icon="Calendar" />

      <div class="tabs tabs-boxed bg-base-100 p-1 rounded-xl">
        <a class="tab" :class="{ 'tab-active': activeTab === 'pending' }" @click="activeTab = 'pending'">
          قيد المراجعة ({{ pendingLeaves.length }})
        </a>
        <a class="tab" :class="{ 'tab-active': activeTab === 'active' }" @click="activeTab = 'active'">
          نشطة ({{ activeLeaves.length }})
        </a>
      </div>

      <div v-if="loading" class="flex justify-center py-16">
        <span class="loading loading-spinner loading-lg text-primary"></span>
      </div>

      <div v-else-if="currentLeaves.length === 0" class="text-center py-16">
        <Calendar class="w-16 h-16 mx-auto text-base-content/20 mb-4" />
        <p class="text-base-content/60">لا توجد إجازات {{ activeTab === 'pending' ? 'قيد المراجعة' : 'نشطة' }}</p>
      </div>

      <div v-else class="grid gap-4">
        <div v-for="leave in currentLeaves" :key="leave.id" class="bg-base-100 rounded-xl shadow-sm p-4">
          <div class="flex items-start justify-between">
            <div>
              <h3 class="font-semibold">{{ leave.studentName }}</h3>
              <p class="text-sm text-base-content/60 mt-1">{{ leave.reason }}</p>
              <div class="flex items-center gap-2 mt-2 text-sm text-base-content/70">
                <Calendar class="w-4 h-4" />
                {{ formatDate(leave.startDate) }} - {{ formatDate(leave.endDate) }}
              </div>
              <div v-if="leave.attachmentFileName" class="flex items-center gap-1 mt-1 text-xs text-info">
                <FileText class="w-3 h-3" />
                {{ leave.attachmentFileName }}
              </div>
            </div>
            <div class="flex gap-2">
              <button
                v-if="!leave.isApproved && !leave.isCancelled"
                class="btn btn-success btn-sm"
                @click="approveLeave(leave.id)"
                :disabled="processing"
              >
                موافقة
              </button>
              <button
                v-if="!leave.isCancelled"
                class="btn btn-ghost btn-sm text-error"
                @click="cancelLeave(leave.id)"
                :disabled="processing"
              >
                إلغاء
              </button>
            </div>
          </div>
        </div>
      </div>
    </div>
  </MainLayout>
</template>

<script setup lang="ts">
import { ref, computed, onMounted } from 'vue'
import { Calendar, FileText } from 'lucide-vue-next'
import MainLayout from '@/components/layout/MainLayout.vue'
import BackButton from '@/components/common/BackButton.vue'
import PageHeader from '@/components/common/PageHeader.vue'
import { useStudentStore, type StudentLeave } from '@/stores/students'
import apiClient from '@/services/api/axios.config'

const studentStore = useStudentStore()
const leaves = ref<StudentLeave[]>([])
const loading = ref(false)
const processing = ref(false)
const activeTab = ref('pending')

const pendingLeaves = computed(() => leaves.value.filter(l => !l.isApproved && !l.isCancelled))
const activeLeaves = computed(() => {
  const now = new Date()
  return leaves.value.filter(l => {
    if (!l.isApproved || l.isCancelled) return false
    const start = new Date(l.startDate)
    const end = new Date(l.endDate)
    return start <= now && end >= now
  })
})
const currentLeaves = computed(() => activeTab.value === 'pending' ? pendingLeaves.value : activeLeaves.value)

const formatDate = (dateStr: string) => {
  return new Date(dateStr).toLocaleDateString('ar-SA', {
    year: 'numeric',
    month: 'short',
    day: 'numeric'
  })
}

const loadLeaves = async () => {
  loading.value = true
  try {
    const response = await apiClient.get('/students/leaves/pending')
    if (response.data.success) {
      leaves.value = response.data.data
    }
  } catch (err) {
    console.error('Failed to load leaves:', err)
  } finally {
    loading.value = false
  }
}

const approveLeave = async (leaveId: string) => {
  processing.value = true
  try {
    await studentStore.approveLeave(leaveId)
    await loadLeaves()
  } catch (err) {
    console.error('Failed to approve leave:', err)
  } finally {
    processing.value = false
  }
}

const cancelLeave = async (leaveId: string) => {
  processing.value = true
  try {
    await apiClient.post(`/students/leaves/${leaveId}/cancel`, { reason: 'إلغاء بواسطة المسؤول' })
    await loadLeaves()
  } catch (err) {
    console.error('Failed to cancel leave:', err)
  } finally {
    processing.value = false
  }
}

onMounted(loadLeaves)
</script>
