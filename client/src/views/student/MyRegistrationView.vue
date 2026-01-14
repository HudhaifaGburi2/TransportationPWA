<template>
  <div class="p-6 space-y-6">
    <!-- Page Header -->
    <div class="bg-gradient-to-l from-info/10 to-transparent p-6 rounded-xl">
      <div class="flex items-center gap-4">
        <div class="p-3 bg-info/20 rounded-xl">
          <FileText class="w-8 h-8 text-info" />
        </div>
        <div>
          <h1 class="text-2xl font-bold text-base-content">طلب التسجيل الخاص بي</h1>
          <p class="text-base-content/60 mt-1">متابعة حالة طلب التسجيل في خدمة النقل</p>
        </div>
      </div>
    </div>

    <!-- Loading State -->
    <div v-if="isLoading" class="flex justify-center py-16">
      <span class="loading loading-spinner loading-lg text-primary"></span>
    </div>

    <!-- Error State -->
    <div v-else-if="error" class="alert alert-error">
      <AlertCircle class="w-6 h-6" />
      <span>{{ error }}</span>
      <button @click="loadRegistration" class="btn btn-sm btn-ghost">إعادة المحاولة</button>
    </div>

    <!-- No Registration Found -->
    <div v-else-if="!registration" class="bg-base-100 rounded-xl shadow-sm border border-base-200 overflow-hidden">
      <div class="bg-gradient-to-b from-primary/5 to-transparent p-12 text-center">
        <div class="w-24 h-24 mx-auto mb-6 bg-primary/10 rounded-full flex items-center justify-center">
          <ClipboardList class="w-12 h-12 text-primary" />
        </div>
        <h2 class="text-2xl font-bold text-base-content mb-3">لم يتم العثور على طلب تسجيل</h2>
        <p class="text-base-content/60 mb-8 max-w-md mx-auto">لم تقم بتقديم طلب تسجيل في خدمة النقل بعد. قم بتقديم طلب جديد للاستفادة من خدمة النقل.</p>
        <router-link to="/registration" class="btn btn-primary btn-lg gap-3 shadow-lg hover:shadow-xl transition-all">
          <Plus class="w-6 h-6" />
          تقديم طلب تسجيل جديد
        </router-link>
      </div>
    </div>

    <!-- Registration Details -->
    <div v-else class="space-y-6">
      <!-- Status Card -->
      <div class="bg-base-100 rounded-xl shadow-sm border border-base-200 overflow-hidden">
        <div class="p-6">
          <div class="flex flex-col md:flex-row md:items-center md:justify-between gap-4">
            <div class="flex items-center gap-4">
              <div :class="statusIconClass" class="p-3 rounded-xl">
                <component :is="statusIcon" class="w-8 h-8" />
              </div>
              <div>
                <p class="text-sm text-base-content/60">حالة الطلب</p>
                <p class="text-xl font-bold">{{ statusText }}</p>
              </div>
            </div>
            <div :class="statusBadgeClass" class="badge badge-lg gap-2 py-4 px-6">
              <component :is="statusIcon" class="w-4 h-4" />
              {{ statusText }}
            </div>
          </div>
        </div>
        
        <!-- Progress Steps -->
        <div class="bg-base-200/50 px-6 py-4">
          <ul class="steps steps-horizontal w-full">
            <li class="step" :class="{ 'step-primary': true }">تقديم الطلب</li>
            <li class="step" :class="{ 'step-primary': registration.status !== 'Pending' }">مراجعة الطلب</li>
            <li class="step" :class="{ 'step-primary': registration.status === 'Approved', 'step-error': registration.status === 'Rejected' }">
              {{ registration.status === 'Rejected' ? 'مرفوض' : 'مقبول' }}
            </li>
          </ul>
        </div>
      </div>

      <!-- Student Info -->
      <div class="bg-base-100 rounded-xl shadow-sm border border-base-200 overflow-hidden">
        <div class="bg-primary/5 px-6 py-4 border-b border-base-200">
          <h2 class="font-bold text-lg flex items-center gap-2">
            <User class="w-5 h-5 text-primary" />
            بيانات الطالب
          </h2>
        </div>
        <div class="p-6">
          <div class="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
            <div class="space-y-1">
              <label class="text-sm text-base-content/60">رقم الطالب</label>
              <p class="font-semibold" dir="ltr">{{ registration.studentId }}</p>
            </div>
            <div class="space-y-1">
              <label class="text-sm text-base-content/60">اسم الطالب</label>
              <p class="font-semibold">{{ registration.studentName }}</p>
            </div>
            <div class="space-y-1">
              <label class="text-sm text-base-content/60">المعلم</label>
              <p class="font-semibold">{{ registration.teacherName || '-' }}</p>
            </div>
          </div>
        </div>
      </div>

      <!-- Address Info -->
      <div class="bg-base-100 rounded-xl shadow-sm border border-base-200 overflow-hidden">
        <div class="bg-info/5 px-6 py-4 border-b border-base-200">
          <h2 class="font-bold text-lg flex items-center gap-2">
            <MapPin class="w-5 h-5 text-info" />
            بيانات العنوان
          </h2>
        </div>
        <div class="p-6">
          <div class="grid grid-cols-1 md:grid-cols-2 gap-6">
            <div class="space-y-1">
              <label class="text-sm text-base-content/60">المنطقة</label>
              <p class="font-semibold">{{ registration.district?.name || '-' }}</p>
            </div>
            <div class="space-y-1">
              <label class="text-sm text-base-content/60">العنوان الوطني المختصر</label>
              <p class="font-semibold font-mono" dir="ltr">{{ registration.nationalShortAddress || '-' }}</p>
            </div>
            <div v-if="registration.homeAddress" class="md:col-span-2 space-y-1">
              <label class="text-sm text-base-content/60">وصف العنوان</label>
              <p class="font-semibold">{{ registration.homeAddress }}</p>
            </div>
          </div>
        </div>
      </div>

      <!-- Dates Info -->
      <div class="bg-base-100 rounded-xl shadow-sm border border-base-200 overflow-hidden">
        <div class="bg-secondary/5 px-6 py-4 border-b border-base-200">
          <h2 class="font-bold text-lg flex items-center gap-2">
            <Calendar class="w-5 h-5 text-secondary" />
            تواريخ الطلب
          </h2>
        </div>
        <div class="p-6">
          <div class="grid grid-cols-1 md:grid-cols-2 gap-6">
            <div class="space-y-1">
              <label class="text-sm text-base-content/60">تاريخ التقديم</label>
              <p class="font-semibold">{{ formatDate(registration.requestedAt) }}</p>
            </div>
            <div v-if="registration.reviewedAt" class="space-y-1">
              <label class="text-sm text-base-content/60">تاريخ المراجعة</label>
              <p class="font-semibold">{{ formatDate(registration.reviewedAt) }}</p>
            </div>
          </div>
        </div>
      </div>

      <!-- Review Notes (if rejected) -->
      <div v-if="registration.status === 'Rejected' && registration.reviewNotes" class="alert alert-error">
        <AlertCircle class="w-6 h-6" />
        <div>
          <h3 class="font-bold">سبب الرفض</h3>
          <p>{{ registration.reviewNotes }}</p>
        </div>
      </div>

      <!-- Approval Message -->
      <div v-if="registration.status === 'Approved'" class="alert alert-success">
        <CheckCircle class="w-6 h-6" />
        <div>
          <h3 class="font-bold">تم قبول طلبك</h3>
          <p>تم قبول طلب التسجيل في خدمة النقل. سيتم التواصل معك قريباً بتفاصيل الباص.</p>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, computed, onMounted } from 'vue'
import { FileText, AlertCircle, ClipboardList, Plus, User, MapPin, Calendar, CheckCircle, XCircle, Clock } from 'lucide-vue-next'
import apiClient from '@/services/api/axios.config'

interface Registration {
  id: string
  studentUserId: number
  studentId: string
  studentName: string
  teacherName?: string
  districtId: string
  nationalShortAddress: string
  homeAddress?: string
  status: 'Pending' | 'Approved' | 'Rejected'
  requestedAt: string
  reviewedAt?: string
  reviewNotes?: string
  district?: {
    id: string
    name: string
  }
}

const isLoading = ref(false)
const error = ref<string | null>(null)
const registration = ref<Registration | null>(null)

const statusText = computed(() => {
  switch (registration.value?.status) {
    case 'Pending': return 'قيد المراجعة'
    case 'Approved': return 'مقبول'
    case 'Rejected': return 'مرفوض'
    default: return 'غير معروف'
  }
})

const statusIcon = computed(() => {
  switch (registration.value?.status) {
    case 'Pending': return Clock
    case 'Approved': return CheckCircle
    case 'Rejected': return XCircle
    default: return Clock
  }
})

const statusIconClass = computed(() => {
  switch (registration.value?.status) {
    case 'Pending': return 'bg-warning/20 text-warning'
    case 'Approved': return 'bg-success/20 text-success'
    case 'Rejected': return 'bg-error/20 text-error'
    default: return 'bg-base-200 text-base-content/60'
  }
})

const statusBadgeClass = computed(() => {
  switch (registration.value?.status) {
    case 'Pending': return 'badge-warning'
    case 'Approved': return 'badge-success'
    case 'Rejected': return 'badge-error'
    default: return 'badge-ghost'
  }
})

const formatDate = (dateStr: string) => {
  return new Date(dateStr).toLocaleDateString('ar-SA', {
    year: 'numeric',
    month: 'long',
    day: 'numeric',
    hour: '2-digit',
    minute: '2-digit'
  })
}

const loadRegistration = async () => {
  isLoading.value = true
  error.value = null
  try {
    const response = await apiClient.get('/registration/my-registration')
    if (response.data.success) {
      registration.value = response.data.data
    }
  } catch (err: any) {
    if (err.response?.status === 404) {
      registration.value = null
    } else {
      error.value = err.response?.data?.message || 'حدث خطأ أثناء تحميل البيانات'
    }
  } finally {
    isLoading.value = false
  }
}

onMounted(loadRegistration)
</script>
