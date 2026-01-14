<template>
  <div class="min-h-screen bg-gradient-to-br from-base-200/50 to-base-100">
    <div class="container mx-auto px-4 py-8 max-w-4xl">
      <!-- Back Navigation -->
      <div class="mb-6">
        <router-link to="/" class="btn btn-ghost btn-sm gap-2">
          <ArrowRight class="w-4 h-4" />
          العودة للرئيسية
        </router-link>
      </div>

      <!-- Page Header -->
      <div class="text-center mb-8">
        <div class="inline-flex items-center justify-center w-20 h-20 bg-gradient-to-br from-info to-info/70 rounded-2xl shadow-lg mb-4">
          <FileSearch class="w-10 h-10 text-info-content" />
        </div>
        <h1 class="text-3xl font-bold text-base-content mb-2">متابعة طلب التسجيل</h1>
        <p class="text-base-content/60 max-w-md mx-auto">تتبع حالة طلبك في خدمة النقل</p>
      </div>

      <!-- Loading State -->
      <div v-if="isLoading" class="flex flex-col items-center justify-center py-20">
        <span class="loading loading-spinner loading-lg text-primary"></span>
        <p class="mt-4 text-base-content/60">جاري تحميل بيانات الطلب...</p>
      </div>

      <!-- Error State -->
      <div v-else-if="error" class="card bg-base-100 shadow-xl">
        <div class="card-body items-center text-center py-12">
          <div class="w-16 h-16 bg-error/10 rounded-full flex items-center justify-center mb-4">
            <AlertCircle class="w-8 h-8 text-error" />
          </div>
          <h2 class="card-title text-error mb-2">تعذر تحميل البيانات</h2>
          <p class="text-base-content/60 mb-6 max-w-sm">{{ error }}</p>
          <button @click="loadRegistration" class="btn btn-primary gap-2">
            <RefreshCw class="w-4 h-4" />
            إعادة المحاولة
          </button>
        </div>
      </div>

      <!-- No Registration Found -->
      <div v-else-if="!registration" class="card bg-base-100 shadow-xl">
        <div class="card-body items-center text-center py-16">
          <div class="relative mb-6">
            <div class="w-32 h-32 bg-gradient-to-br from-primary/20 to-primary/5 rounded-full flex items-center justify-center">
              <Bus class="w-16 h-16 text-primary" />
            </div>
            <div class="absolute -bottom-2 -right-2 w-12 h-12 bg-warning rounded-full flex items-center justify-center shadow-lg">
              <ClipboardList class="w-6 h-6 text-warning-content" />
            </div>
          </div>
          <h2 class="text-2xl font-bold text-base-content mb-3">لا يوجد طلب تسجيل</h2>
          <p class="text-base-content/60 mb-8 max-w-md">لم تقم بتقديم طلب للتسجيل في خدمة النقل بعد. سجّل الآن للاستفادة من خدمة النقل المجانية.</p>
          <router-link 
            to="/registration" 
            class="btn btn-primary btn-lg gap-3 px-8 py-4 rounded-2xl shadow-xl hover:shadow-2xl hover:scale-105 active:scale-95 transition-all duration-200 font-bold text-lg"
          >
            <Plus class="w-6 h-6" />
            <span>تقديم طلب تسجيل جديد</span>
          </router-link>
        </div>
      </div>

      <!-- Registration Details -->
      <div v-else class="space-y-6">
        <!-- Main Status Card -->
        <div class="card bg-base-100 shadow-xl overflow-hidden">
          <div :class="statusGradientClass">
            <div class="card-body">
              <div class="flex flex-col md:flex-row items-center gap-6">
                <div :class="statusIconBgClass" class="w-20 h-20 rounded-2xl flex items-center justify-center shadow-lg">
                  <component :is="statusIcon" class="w-10 h-10" :class="statusIconColorClass" />
                </div>
                <div class="text-center md:text-right flex-1">
                  <p class="text-sm opacity-70 mb-1">حالة الطلب</p>
                  <h2 class="text-3xl font-bold mb-2">{{ statusText }}</h2>
                  <p class="text-sm opacity-70">{{ statusDescription }}</p>
                </div>
                <div :class="statusBadgeClass" class="badge badge-lg gap-2 py-4 px-6 shadow">
                  <component :is="statusIcon" class="w-4 h-4" />
                  {{ statusText }}
                </div>
              </div>
            </div>
          </div>
          
          <!-- Progress Steps -->
          <div class="px-6 py-5 bg-base-200/30">
            <ul class="steps steps-horizontal w-full">
              <li class="step step-primary" data-content="✓">
                <span class="text-xs mt-2">تقديم الطلب</span>
              </li>
              <li class="step" :class="{ 'step-primary': registration.status !== 'Pending' }" :data-content="registration.status !== 'Pending' ? '✓' : '2'">
                <span class="text-xs mt-2">مراجعة الطلب</span>
              </li>
              <li class="step" :class="{ 'step-primary': registration.status === 'Approved', 'step-error': registration.status === 'Rejected' }" :data-content="registration.status === 'Approved' ? '✓' : registration.status === 'Rejected' ? '✗' : '3'">
                <span class="text-xs mt-2">{{ registration.status === 'Rejected' ? 'مرفوض' : 'مقبول' }}</span>
              </li>
            </ul>
          </div>
        </div>

        <!-- Info Cards Grid -->
        <div class="grid grid-cols-1 lg:grid-cols-2 gap-6">
          <!-- Student Info Card -->
          <div class="card bg-base-100 shadow-xl">
            <div class="card-body">
              <div class="flex items-center gap-3 mb-4">
                <div class="w-10 h-10 bg-primary/10 rounded-lg flex items-center justify-center">
                  <User class="w-5 h-5 text-primary" />
                </div>
                <h3 class="card-title text-lg">بيانات الطالب</h3>
              </div>
              <div class="space-y-4">
                <div class="flex justify-between items-center py-2 border-b border-base-200">
                  <span class="text-base-content/60 text-sm">رقم الطالب</span>
                  <span class="font-bold font-mono" dir="ltr">{{ registration.studentId }}</span>
                </div>
                <div class="flex justify-between items-center py-2 border-b border-base-200">
                  <span class="text-base-content/60 text-sm">الاسم</span>
                  <span class="font-bold">{{ registration.studentName }}</span>
                </div>
                <div class="flex justify-between items-center py-2">
                  <span class="text-base-content/60 text-sm">المعلم</span>
                  <span class="font-semibold">{{ registration.teacherName || '-' }}</span>
                </div>
              </div>
            </div>
          </div>

          <!-- Address Info Card -->
          <div class="card bg-base-100 shadow-xl">
            <div class="card-body">
              <div class="flex items-center gap-3 mb-4">
                <div class="w-10 h-10 bg-info/10 rounded-lg flex items-center justify-center">
                  <MapPin class="w-5 h-5 text-info" />
                </div>
                <h3 class="card-title text-lg">بيانات العنوان</h3>
              </div>
              <div class="space-y-4">
                <div class="flex justify-between items-center py-2 border-b border-base-200">
                  <span class="text-base-content/60 text-sm">المنطقة</span>
                  <span class="font-bold">{{ registration.district?.name || '-' }}</span>
                </div>
                <div class="flex justify-between items-center py-2 border-b border-base-200">
                  <span class="text-base-content/60 text-sm">العنوان الوطني</span>
                  <span class="font-bold font-mono tracking-wider" dir="ltr">{{ registration.nationalShortAddress || '-' }}</span>
                </div>
                <div v-if="registration.homeAddress" class="py-2">
                  <span class="text-base-content/60 text-sm block mb-1">وصف إضافي</span>
                  <span class="text-sm">{{ registration.homeAddress }}</span>
                </div>
              </div>
            </div>
          </div>
        </div>

        <!-- Timeline Card -->
        <div class="card bg-base-100 shadow-xl">
          <div class="card-body">
            <div class="flex items-center gap-3 mb-4">
              <div class="w-10 h-10 bg-secondary/10 rounded-lg flex items-center justify-center">
                <Calendar class="w-5 h-5 text-secondary" />
              </div>
              <h3 class="card-title text-lg">سجل الطلب</h3>
            </div>
            <div class="relative pr-8">
              <!-- Timeline Line -->
              <div class="absolute right-3 top-2 bottom-2 w-0.5 bg-base-300"></div>
              
              <!-- Submitted -->
              <div class="relative mb-6">
                <div class="absolute right-0 w-6 h-6 bg-primary rounded-full flex items-center justify-center -translate-x-1/2">
                  <Send class="w-3 h-3 text-primary-content" />
                </div>
                <div class="mr-8">
                  <p class="font-bold text-sm">تم تقديم الطلب</p>
                  <p class="text-xs text-base-content/60">{{ formatDate(registration.requestedAt) }}</p>
                </div>
              </div>
              
              <!-- Reviewed -->
              <div v-if="registration.reviewedAt" class="relative">
                <div class="absolute right-0 w-6 h-6 rounded-full flex items-center justify-center -translate-x-1/2" :class="registration.status === 'Approved' ? 'bg-success' : 'bg-error'">
                  <component :is="registration.status === 'Approved' ? CheckCircle : XCircle" class="w-3 h-3 text-white" />
                </div>
                <div class="mr-8">
                  <p class="font-bold text-sm">{{ registration.status === 'Approved' ? 'تمت الموافقة' : 'تم الرفض' }}</p>
                  <p class="text-xs text-base-content/60">{{ formatDate(registration.reviewedAt) }}</p>
                </div>
              </div>
              
              <!-- Pending Review -->
              <div v-else class="relative">
                <div class="absolute right-0 w-6 h-6 bg-warning rounded-full flex items-center justify-center -translate-x-1/2">
                  <Clock class="w-3 h-3 text-warning-content" />
                </div>
                <div class="mr-8">
                  <p class="font-bold text-sm">في انتظار المراجعة</p>
                  <p class="text-xs text-base-content/60">سيتم مراجعة طلبك قريباً</p>
                </div>
              </div>
            </div>
          </div>
        </div>

        <!-- Status Messages -->
        <div v-if="registration.status === 'Rejected' && registration.reviewNotes" class="alert alert-error shadow-lg">
          <XCircle class="w-6 h-6" />
          <div>
            <h3 class="font-bold">سبب الرفض</h3>
            <p class="text-sm">{{ registration.reviewNotes }}</p>
          </div>
          <router-link to="/registration" class="btn btn-sm btn-ghost">تقديم طلب جديد</router-link>
        </div>

        <div v-if="registration.status === 'Approved'" class="alert alert-success shadow-lg">
          <CheckCircle class="w-6 h-6" />
          <div>
            <h3 class="font-bold">مبارك! تم قبول طلبك</h3>
            <p class="text-sm">تم قبول طلب التسجيل في خدمة النقل. سيتم التواصل معك قريباً بتفاصيل الباص والمواعيد.</p>
          </div>
        </div>

        <div v-if="registration.status === 'Pending'" class="alert shadow-lg">
          <Clock class="w-6 h-6 text-warning" />
          <div>
            <h3 class="font-bold">طلبك قيد المراجعة</h3>
            <p class="text-sm">سيتم مراجعة طلبك من قبل إدارة النقل وإشعارك بالنتيجة خلال 48 ساعة.</p>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, computed, onMounted } from 'vue'
import { FileSearch, AlertCircle, ClipboardList, Plus, User, MapPin, Calendar, CheckCircle, XCircle, Clock, RefreshCw, Bus, Send, ArrowRight } from 'lucide-vue-next'
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

const statusDescription = computed(() => {
  switch (registration.value?.status) {
    case 'Pending': return 'طلبك قيد المراجعة من قبل إدارة النقل'
    case 'Approved': return 'تم قبول طلبك في خدمة النقل'
    case 'Rejected': return 'للأسف تم رفض طلبك'
    default: return ''
  }
})

const statusGradientClass = computed(() => {
  switch (registration.value?.status) {
    case 'Pending': return 'bg-gradient-to-l from-warning/10 via-warning/5 to-transparent'
    case 'Approved': return 'bg-gradient-to-l from-success/10 via-success/5 to-transparent'
    case 'Rejected': return 'bg-gradient-to-l from-error/10 via-error/5 to-transparent'
    default: return ''
  }
})

const statusIconBgClass = computed(() => {
  switch (registration.value?.status) {
    case 'Pending': return 'bg-warning'
    case 'Approved': return 'bg-success'
    case 'Rejected': return 'bg-error'
    default: return 'bg-base-300'
  }
})

const statusIconColorClass = computed(() => {
  switch (registration.value?.status) {
    case 'Pending': return 'text-warning-content'
    case 'Approved': return 'text-success-content'
    case 'Rejected': return 'text-error-content'
    default: return 'text-base-content'
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
