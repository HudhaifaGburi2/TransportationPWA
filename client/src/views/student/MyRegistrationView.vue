<template>
  <div class="container mx-auto px-4 py-6">
    <div class="bg-white rounded-lg shadow-lg p-6">
      <h1 class="text-2xl font-bold text-gray-800 mb-6">طلب التسجيل الخاص بي</h1>
      
      <div v-if="isLoading" class="flex justify-center py-8">
        <div class="animate-spin rounded-full h-8 w-8 border-b-2 border-primary-600"></div>
      </div>

      <div v-else-if="!registration" class="text-center py-8">
        <div class="w-16 h-16 mx-auto mb-4 bg-gray-100 rounded-full flex items-center justify-center">
          <svg class="w-8 h-8 text-gray-400" fill="none" stroke="currentColor" viewBox="0 0 24 24">
            <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M9 12h6m-6 4h6m2 5H7a2 2 0 01-2-2V5a2 2 0 012-2h5.586a1 1 0 01.707.293l5.414 5.414a1 1 0 01.293.707V19a2 2 0 01-2 2z" />
          </svg>
        </div>
        <p class="text-gray-600 mb-4">لم يتم العثور على طلب تسجيل</p>
        <router-link 
          to="/registration"
          class="inline-block bg-primary-600 text-white px-6 py-2 rounded-lg hover:bg-primary-700 transition-colors"
        >
          تقديم طلب جديد
        </router-link>
      </div>

      <div v-else class="space-y-6">
        <div class="flex items-center justify-between p-4 bg-gray-50 rounded-lg">
          <span class="text-gray-600">حالة الطلب:</span>
          <span 
            :class="statusClass"
            class="px-3 py-1 rounded-full text-sm font-medium"
          >
            {{ statusText }}
          </span>
        </div>

        <div class="grid grid-cols-1 md:grid-cols-2 gap-4">
          <div class="p-4 border rounded-lg">
            <span class="text-sm text-gray-500">المنطقة</span>
            <p class="font-medium">{{ registration.districtName }}</p>
          </div>
          <div class="p-4 border rounded-lg">
            <span class="text-sm text-gray-500">الموقع</span>
            <p class="font-medium">{{ registration.locationName }}</p>
          </div>
          <div class="p-4 border rounded-lg">
            <span class="text-sm text-gray-500">تاريخ التقديم</span>
            <p class="font-medium">{{ formatDate(registration.createdAt) }}</p>
          </div>
          <div class="p-4 border rounded-lg">
            <span class="text-sm text-gray-500">آخر تحديث</span>
            <p class="font-medium">{{ formatDate(registration.updatedAt) }}</p>
          </div>
        </div>

        <div v-if="registration.notes" class="p-4 border rounded-lg">
          <span class="text-sm text-gray-500">ملاحظات</span>
          <p class="mt-1">{{ registration.notes }}</p>
        </div>

        <div v-if="registration.reviewNotes" class="p-4 bg-yellow-50 border border-yellow-200 rounded-lg">
          <span class="text-sm text-yellow-700 font-medium">ملاحظات المراجعة</span>
          <p class="mt-1 text-yellow-800">{{ registration.reviewNotes }}</p>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, computed, onMounted } from 'vue'

interface Registration {
  id: string
  status: 'pending' | 'approved' | 'rejected'
  districtName: string
  locationName: string
  notes?: string
  reviewNotes?: string
  createdAt: string
  updatedAt: string
}

const isLoading = ref(false)
const registration = ref<Registration | null>(null)

const statusText = computed(() => {
  switch (registration.value?.status) {
    case 'pending': return 'قيد المراجعة'
    case 'approved': return 'مقبول'
    case 'rejected': return 'مرفوض'
    default: return 'غير معروف'
  }
})

const statusClass = computed(() => {
  switch (registration.value?.status) {
    case 'pending': return 'bg-yellow-100 text-yellow-800'
    case 'approved': return 'bg-green-100 text-green-800'
    case 'rejected': return 'bg-red-100 text-red-800'
    default: return 'bg-gray-100 text-gray-800'
  }
})

const formatDate = (dateStr: string) => {
  return new Date(dateStr).toLocaleDateString('ar-SA', {
    year: 'numeric',
    month: 'long',
    day: 'numeric'
  })
}

onMounted(async () => {
  isLoading.value = true
  try {
    // TODO: Fetch registration from API
  } finally {
    isLoading.value = false
  }
})
</script>
