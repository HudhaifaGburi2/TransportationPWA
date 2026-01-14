<script setup lang="ts">
import { ref, computed, onMounted } from 'vue'
import { useAuthStore, TUMS_ROLES } from '@/stores/auth'
import { Bus, Users, MapPin, Calendar, Route, ClipboardList, FileText, User, Clock, CheckCircle, XCircle, AlertCircle } from 'lucide-vue-next'
import apiClient from '@/services/api/axios.config'
import BaseButton from '@/components/ui/BaseButton.vue'

const authStore = useAuthStore()

// Role checks
const isStudent = computed(() => authStore.hasRole(TUMS_ROLES.STUDENT))
const isAdminOrStaff = computed(() => authStore.hasAnyRole([TUMS_ROLES.ADMIN, TUMS_ROLES.STAFF, TUMS_ROLES.SYSTEM_ADMIN]))

// Student data
const studentInfo = ref<any>(null)
const registration = ref<any>(null)
const isLoadingStudent = ref(false)

const loadStudentData = async () => {
  if (!isStudent.value) return
  
  isLoadingStudent.value = true
  try {
    // Load student info
    const infoResponse = await apiClient.get('/registration/student-info')
    if (infoResponse.data.success) {
      studentInfo.value = infoResponse.data.data
    }
  } catch {
    // Student info not available
  }
  
  try {
    // Load registration status
    const regResponse = await apiClient.get('/registration/my-registration')
    if (regResponse.data.success) {
      registration.value = regResponse.data.data
    }
  } catch {
    // No registration found
  }
  
  isLoadingStudent.value = false
}

const registrationStatusText = computed(() => {
  if (!registration.value) return 'لم يتم التسجيل'
  switch (registration.value.status) {
    case 'Pending': return 'قيد المراجعة'
    case 'Approved': return 'مقبول'
    case 'Rejected': return 'مرفوض'
    default: return 'غير معروف'
  }
})

const registrationStatusClass = computed(() => {
  if (!registration.value) return 'badge-ghost'
  switch (registration.value.status) {
    case 'Pending': return 'badge-warning'
    case 'Approved': return 'badge-success'
    case 'Rejected': return 'badge-error'
    default: return 'badge-ghost'
  }
})

const stats = [
  { title: 'إجمالي الحافلات', value: '111', icon: Bus, color: 'bg-primary' },
  { title: 'الأحياء', value: '88', icon: MapPin, color: 'bg-secondary' },
  { title: 'مواقف السيارات', value: '8', icon: Calendar, color: 'bg-info' },
  { title: 'الطلاب المسجلين', value: '0', icon: Users, color: 'bg-success' },
]

onMounted(loadStudentData)
</script>

<template>
  <div class="min-h-screen bg-background">
    <!-- Header -->
    <header class="bg-primary text-white shadow-md">
      <div class="container mx-auto px-4 py-4 flex items-center justify-between">
        <div class="flex items-center gap-4">
          <div class="w-10 h-10 bg-white/20 rounded-lg flex items-center justify-center">
            <Bus class="w-6 h-6" />
          </div>
          <div>
            <h1 class="text-xl font-bold font-cairo">نظام إدارة وحدة النقل</h1>
            <p class="text-sm text-white/80">لوحة التحكم</p>
          </div>
        </div>
        <div class="flex items-center gap-4">
          <span class="text-sm">مرحباً، {{ authStore.fullName || 'مستخدم' }}</span>
          <button
            @click="authStore.logout()"
            class="px-4 py-2 bg-white/20 hover:bg-white/30 rounded-lg text-sm transition-colors"
          >
            تسجيل الخروج
          </button>
        </div>
      </div>
    </header>

    <!-- Main Content -->
    <main class="container mx-auto px-4 py-8">
      <!-- Welcome Card (Admin) -->
      <div v-if="isAdminOrStaff" class="card mb-8">
        <h2 class="text-xl font-bold text-gray-800 mb-2">مرحباً بك في نظام إدارة وحدة النقل</h2>
        <p class="text-neutral">يمكنك من هنا إدارة الحافلات والأحياء والطلاب وتتبع الحضور والانصراف</p>
      </div>

      <!-- Student Dashboard -->
      <div v-if="isStudent" class="space-y-6 mb-8">
        <!-- Loading -->
        <div v-if="isLoadingStudent" class="flex justify-center py-8">
          <span class="loading loading-spinner loading-lg text-primary"></span>
        </div>

        <!-- Student Info Card -->
        <div v-else-if="studentInfo" class="card bg-base-100 shadow-lg overflow-hidden">
          <div class="bg-gradient-to-l from-primary/10 via-primary/5 to-transparent">
            <div class="card-body">
              <div class="flex items-center gap-3 mb-4">
                <div class="w-10 h-10 bg-primary/10 rounded-lg flex items-center justify-center">
                  <User class="w-5 h-5 text-primary" />
                </div>
                <div>
                  <h2 class="card-title text-lg">بيانات الطالب</h2>
                  <p class="text-sm text-base-content/50">معلوماتك في النظام</p>
                </div>
                <div class="badge mr-auto" :class="registrationStatusClass">
                  {{ registrationStatusText }}
                </div>
              </div>
              
              <div class="grid grid-cols-2 md:grid-cols-4 gap-4">
                <div class="bg-base-200/50 rounded-lg p-3">
                  <p class="text-xs text-base-content/50 mb-1">رقم الطالب</p>
                  <p class="font-bold text-base-content" dir="ltr">{{ studentInfo.studentId || '-' }}</p>
                </div>
                <div class="bg-base-200/50 rounded-lg p-3 md:col-span-2">
                  <p class="text-xs text-base-content/50 mb-1">اسم الطالب</p>
                  <p class="font-bold text-base-content">{{ studentInfo.studentName || '-' }}</p>
                </div>
                <div class="bg-base-200/50 rounded-lg p-3">
                  <p class="text-xs text-base-content/50 mb-1">رقم التسجيل</p>
                  <p class="font-bold text-sm" dir="ltr">{{ studentInfo.studentHalaqaSecId || '-' }}</p>
                </div>
                <div class="bg-base-200/50 rounded-lg p-3 md:col-span-2">
                  <p class="text-xs text-base-content/50 mb-1">المعلم</p>
                  <p class="font-semibold text-sm">{{ studentInfo.teacherName || '-' }}</p>
                </div>
                <div class="bg-base-200/50 rounded-lg p-3">
                  <p class="text-xs text-base-content/50 mb-1">الفترة</p>
                  <p class="font-semibold text-sm">{{ studentInfo.periodName || '-' }}</p>
                </div>
                <div class="bg-base-200/50 rounded-lg p-3">
                  <p class="text-xs text-base-content/50 mb-1">الموقع</p>
                  <p class="font-semibold text-sm">{{ studentInfo.halaqaLocationName || '-' }}</p>
                </div>
              </div>
            </div>
          </div>
        </div>

        <!-- Registration Status Card -->
        <div v-if="registration" class="card bg-base-100 shadow-lg">
          <div class="card-body">
            <div class="flex items-center gap-3 mb-4">
              <div class="w-10 h-10 rounded-lg flex items-center justify-center" :class="{
                'bg-warning/10': registration.status === 'Pending',
                'bg-success/10': registration.status === 'Approved',
                'bg-error/10': registration.status === 'Rejected'
              }">
                <Clock v-if="registration.status === 'Pending'" class="w-5 h-5 text-warning" />
                <CheckCircle v-else-if="registration.status === 'Approved'" class="w-5 h-5 text-success" />
                <XCircle v-else class="w-5 h-5 text-error" />
              </div>
              <div>
                <h2 class="card-title text-lg">حالة طلب التسجيل</h2>
                <p class="text-sm text-base-content/50">{{ registrationStatusText }}</p>
              </div>
            </div>

            <!-- Assigned Bus (if approved) -->
            <div v-if="registration.status === 'Approved' && registration.assignedBus" class="alert alert-success">
              <Bus class="w-6 h-6" />
              <div>
                <h3 class="font-bold">تم تعيين الباص</h3>
                <p class="text-sm">باص رقم: {{ registration.assignedBus.busNumber }}</p>
              </div>
            </div>

            <!-- Pending Message -->
            <div v-else-if="registration.status === 'Pending'" class="alert">
              <Clock class="w-6 h-6 text-warning" />
              <div>
                <h3 class="font-bold">طلبك قيد المراجعة</h3>
                <p class="text-sm">سيتم مراجعة طلبك وإشعارك بالنتيجة قريباً</p>
              </div>
            </div>

            <!-- Approved but no bus -->
            <div v-else-if="registration.status === 'Approved'" class="alert alert-success">
              <CheckCircle class="w-6 h-6" />
              <div>
                <h3 class="font-bold">تم قبول طلبك</h3>
                <p class="text-sm">سيتم تعيين الباص لك قريباً</p>
              </div>
            </div>

            <!-- Rejected -->
            <div v-else-if="registration.status === 'Rejected'" class="alert alert-error">
              <AlertCircle class="w-6 h-6" />
              <div>
                <h3 class="font-bold">تم رفض الطلب</h3>
                <p class="text-sm">{{ registration.reviewNotes || 'يرجى التواصل مع الإدارة' }}</p>
              </div>
            </div>
          </div>
        </div>

        <!-- No Registration -->
        <div v-else-if="!isLoadingStudent && !registration" class="card bg-base-100 shadow-lg">
          <div class="card-body items-center text-center py-8">
            <ClipboardList class="w-12 h-12 text-primary mb-4" />
            <h2 class="card-title">لم تقم بالتسجيل بعد</h2>
            <p class="text-base-content/60 mb-4">سجل الآن للاستفادة من خدمة النقل المجانية</p>
            <BaseButton to="/registration" size="lg">
              <template #icon>
                <ClipboardList />
              </template>
              تسجيل جديد
            </BaseButton>
          </div>
        </div>
      </div>

      <!-- Stats Grid (Admin/Staff Only) -->
      <div v-if="isAdminOrStaff" class="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-6 mb-8">
        <div v-for="stat in stats" :key="stat.title" class="card hover:shadow-md transition-shadow">
          <div class="flex items-center gap-4">
            <div :class="[stat.color, 'w-12 h-12 rounded-lg flex items-center justify-center']">
              <component :is="stat.icon" class="w-6 h-6 text-white" />
            </div>
            <div>
              <p class="text-sm text-neutral">{{ stat.title }}</p>
              <p class="text-2xl font-bold text-gray-800">{{ stat.value }}</p>
            </div>
          </div>
        </div>
      </div>

      <!-- Quick Actions -->
      <div class="card">
        <h3 class="text-lg font-bold text-gray-800 mb-4">الإجراءات السريعة</h3>
        <div class="grid grid-cols-2 md:grid-cols-4 gap-4">
          <!-- Admin/Staff Actions -->
          <router-link
            v-if="isAdminOrStaff"
            to="/buses"
            class="flex flex-col items-center gap-2 p-4 bg-background rounded-lg hover:bg-primary/5 transition-colors"
          >
            <Bus class="w-8 h-8 text-primary" />
            <span class="text-sm font-medium">إدارة الباصات</span>
          </router-link>
          <router-link
            v-if="isAdminOrStaff"
            to="/routes"
            class="flex flex-col items-center gap-2 p-4 bg-background rounded-lg hover:bg-primary/5 transition-colors"
          >
            <Route class="w-8 h-8 text-info" />
            <span class="text-sm font-medium">المسارات</span>
          </router-link>
          <router-link
            v-if="isAdminOrStaff"
            to="/districts"
            class="flex flex-col items-center gap-2 p-4 bg-background rounded-lg hover:bg-primary/5 transition-colors"
          >
            <MapPin class="w-8 h-8 text-secondary" />
            <span class="text-sm font-medium">المناطق</span>
          </router-link>
          <router-link
            v-if="isAdminOrStaff"
            to="/locations"
            class="flex flex-col items-center gap-2 p-4 bg-background rounded-lg hover:bg-primary/5 transition-colors"
          >
            <Calendar class="w-8 h-8 text-accent" />
            <span class="text-sm font-medium">المواقع</span>
          </router-link>
          <router-link
            v-if="isAdminOrStaff"
            to="/registration-requests"
            class="flex flex-col items-center gap-2 p-4 bg-background rounded-lg hover:bg-primary/5 transition-colors"
          >
            <FileText class="w-8 h-8 text-warning" />
            <span class="text-sm font-medium">طلبات التسجيل</span>
          </router-link>
          
          <!-- Student Actions -->
          <router-link
            v-if="isStudent"
            to="/registration"
            class="flex flex-col items-center gap-2 p-4 bg-background rounded-lg hover:bg-primary/5 transition-colors"
          >
            <ClipboardList class="w-8 h-8 text-success" />
            <span class="text-sm font-medium">تسجيل جديد</span>
          </router-link>
          <router-link
            v-if="isStudent"
            to="/my-registration"
            class="flex flex-col items-center gap-2 p-4 bg-background rounded-lg hover:bg-primary/5 transition-colors"
          >
            <Users class="w-8 h-8 text-primary" />
            <span class="text-sm font-medium">طلبي</span>
          </router-link>
        </div>
      </div>
    </main>
  </div>
</template>
