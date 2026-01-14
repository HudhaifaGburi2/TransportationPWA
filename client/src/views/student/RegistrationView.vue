<template>
  <div class="p-6 space-y-6">
    <!-- Page Header -->
    <div class="bg-gradient-to-l from-success/10 to-transparent p-6 rounded-xl">
      <div class="flex items-center gap-4">
        <div class="p-3 bg-success/20 rounded-xl">
          <ClipboardList class="w-8 h-8 text-success" />
        </div>
        <div>
          <h1 class="text-2xl font-bold text-base-content">التسجيل في خدمة النقل</h1>
          <p class="text-base-content/60 mt-1">قم بتعبئة بياناتك للتسجيل في نظام النقل الجامعي</p>
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
      <button @click="loadStudentInfo" class="btn btn-sm btn-ghost">إعادة المحاولة</button>
    </div>

    <!-- Registration Form -->
    <div v-else class="space-y-6">
      <!-- Student Info Card (Auto-filled - Read Only) -->
      <div class="bg-base-100 rounded-xl shadow-sm border border-base-200 overflow-hidden">
        <div class="bg-primary/5 px-6 py-4 border-b border-base-200">
          <h2 class="font-bold text-lg flex items-center gap-2">
            <User class="w-5 h-5 text-primary" />
            بيانات الطالب
          </h2>
          <p class="text-sm text-base-content/60">معلومات محملة تلقائياً من النظام</p>
        </div>
        <div class="p-6">
          <div class="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
            <div class="space-y-1">
              <label class="text-sm text-base-content/60">رقم الطالب</label>
              <p class="font-semibold text-lg" dir="ltr">{{ studentInfo?.studentId || '-' }}</p>
            </div>
            <div class="space-y-1">
              <label class="text-sm text-base-content/60">اسم الطالب</label>
              <p class="font-semibold text-lg">{{ studentInfo?.studentName || '-' }}</p>
            </div>
            <div class="space-y-1">
              <label class="text-sm text-base-content/60">الفترة</label>
              <p class="font-semibold">{{ studentInfo?.periodName || '-' }}</p>
            </div>
            <div class="space-y-1">
              <label class="text-sm text-base-content/60">نوع الحلقة</label>
              <p class="font-semibold">{{ studentInfo?.halaqaTypeName || '-' }}</p>
            </div>
            <div class="space-y-1">
              <label class="text-sm text-base-content/60">المعلم</label>
              <p class="font-semibold">{{ studentInfo?.teacherName || '-' }}</p>
            </div>
            <div class="space-y-1">
              <label class="text-sm text-base-content/60">الموقع</label>
              <p class="font-semibold">{{ studentInfo?.halaqaLocationName || '-' }}</p>
            </div>
          </div>
        </div>
      </div>

      <!-- Registration Form Card -->
      <form @submit.prevent="submitRegistration" class="bg-base-100 rounded-xl shadow-sm border border-base-200 overflow-hidden">
        <div class="bg-info/5 px-6 py-4 border-b border-base-200">
          <h2 class="font-bold text-lg flex items-center gap-2">
            <MapPin class="w-5 h-5 text-info" />
            بيانات العنوان
          </h2>
          <p class="text-sm text-base-content/60">أدخل معلومات عنوانك للتسجيل في خدمة النقل</p>
        </div>
        <div class="p-6 space-y-6">
          <!-- District Selection -->
          <div class="form-control">
            <label class="label">
              <span class="label-text font-medium">المنطقة <span class="text-error">*</span></span>
            </label>
            <select 
              v-model="form.districtId" 
              class="select select-bordered w-full"
              required
            >
              <option value="">اختر المنطقة</option>
              <option v-for="district in districts" :key="district.id" :value="district.id">
                {{ district.name }}
              </option>
            </select>
          </div>

          <!-- National Short Address -->
          <div class="form-control">
            <label class="label">
              <span class="label-text font-medium">العنوان الوطني المختصر <span class="text-error">*</span></span>
              <a href="https://splonline.com.sa/ar/national-address-1/" target="_blank" class="label-text-alt link link-primary">
                كيف أجد عنواني؟
              </a>
            </label>
            <div class="join w-full">
              <input 
                v-model="form.nationalShortAddress"
                type="text"
                class="input input-bordered join-item flex-1 uppercase"
                placeholder="ABCD1234"
                maxlength="8"
                pattern="[A-Za-z]{4}[0-9]{4}"
                dir="ltr"
                required
              />
              <span class="btn btn-square join-item btn-disabled">
                <Home class="w-5 h-5" />
              </span>
            </div>
            <label class="label">
              <span class="label-text-alt text-base-content/60">4 أحرف + 4 أرقام (مثال: RRRD2929)</span>
            </label>
          </div>

          <!-- Home Address (Optional) -->
          <div class="form-control">
            <label class="label">
              <span class="label-text font-medium">وصف إضافي للعنوان</span>
              <span class="label-text-alt text-base-content/50">اختياري</span>
            </label>
            <textarea 
              v-model="form.homeAddress"
              class="textarea textarea-bordered h-24"
              placeholder="أضف وصف إضافي للعنوان مثل: بجوار مسجد، قرب مدرسة..."
            ></textarea>
          </div>
        </div>

        <!-- Form Actions -->
        <div class="bg-base-200/50 px-6 py-4 flex flex-col sm:flex-row gap-3 justify-end">
          <router-link to="/" class="btn btn-ghost">
            إلغاء
          </router-link>
          <button 
            type="submit"
            class="btn btn-primary gap-2"
            :disabled="isSubmitting || !isFormValid"
          >
            <span v-if="isSubmitting" class="loading loading-spinner loading-sm"></span>
            <Send v-else class="w-5 h-5" />
            تقديم طلب التسجيل
          </button>
        </div>
      </form>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, computed, onMounted } from 'vue'
import { useRouter } from 'vue-router'
import { ClipboardList, User, MapPin, Home, Send, AlertCircle } from 'lucide-vue-next'
import apiClient from '@/services/api/axios.config'

interface StudentInfo {
  studentUserId: number
  studentId: string
  studentName: string
  halaqaTypeCode?: string
  halaqaTypeName?: string
  halaqaSectionId?: string
  periodId?: number
  periodName?: string
  periodCode?: string
  ageGroupId?: number
  ageGroupName?: string
  halaqaLocationId?: number
  halaqaLocationName?: string
  teacherName?: string
}

interface District {
  id: string
  name: string
}

const router = useRouter()

const isLoading = ref(false)
const isSubmitting = ref(false)
const error = ref<string | null>(null)

const studentInfo = ref<StudentInfo | null>(null)
const districts = ref<District[]>([])

const form = ref({
  districtId: '',
  nationalShortAddress: '',
  homeAddress: ''
})

const isFormValid = computed(() => {
  const addressPattern = /^[A-Za-z]{4}\d{4}$/
  return form.value.districtId && addressPattern.test(form.value.nationalShortAddress)
})

const loadStudentInfo = async () => {
  isLoading.value = true
  error.value = null
  try {
    const [studentResponse, districtsResponse] = await Promise.all([
      apiClient.get('/api/v1/registration/student-info'),
      apiClient.get('/api/v1/districts')
    ])
    
    if (studentResponse.data.success) {
      studentInfo.value = studentResponse.data.data
    }
    
    if (districtsResponse.data.success) {
      districts.value = districtsResponse.data.data
    }
  } catch (err: any) {
    error.value = err.response?.data?.message || 'حدث خطأ أثناء تحميل البيانات'
  } finally {
    isLoading.value = false
  }
}

const submitRegistration = async () => {
  if (!isFormValid.value) return
  
  isSubmitting.value = true
  error.value = null
  
  try {
    const response = await apiClient.post('/api/v1/registration', {
      districtId: form.value.districtId,
      nationalShortAddress: form.value.nationalShortAddress.toUpperCase(),
      homeAddress: form.value.homeAddress || null
    })
    
    if (response.data.success) {
      router.push('/my-registration')
    } else {
      error.value = response.data.message || 'حدث خطأ أثناء تقديم الطلب'
    }
  } catch (err: any) {
    error.value = err.response?.data?.message || 'حدث خطأ أثناء تقديم الطلب'
  } finally {
    isSubmitting.value = false
  }
}

onMounted(loadStudentInfo)
</script>
