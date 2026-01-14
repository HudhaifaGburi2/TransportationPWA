<template>
  <div class="min-h-screen bg-gradient-to-br from-base-200/50 to-base-100">
    <div class="container mx-auto px-4 py-8 max-w-4xl">
      <!-- Back Navigation -->
      <div class="mb-6">
        <BaseButton to="/" variant="ghost" size="sm">
          <template #icon>
            <ArrowRight />
          </template>
          العودة للرئيسية
        </BaseButton>
      </div>

      <!-- Page Header -->
      <div class="text-center mb-8">
        <div class="inline-flex items-center justify-center w-20 h-20 bg-gradient-to-br from-primary to-primary/70 rounded-2xl shadow-lg mb-4">
          <Bus class="w-10 h-10 text-primary-content" />
        </div>
        <h1 class="text-3xl font-bold text-base-content mb-2">التسجيل في خدمة النقل</h1>
        <p class="text-base-content/60 max-w-md mx-auto">قم بتعبئة بياناتك للاستفادة من خدمة النقل المجانية لطلاب الحلقات</p>
      </div>

      <!-- Loading State -->
      <div v-if="isLoading" class="flex flex-col items-center justify-center py-20">
        <div class="relative">
          <span class="loading loading-spinner loading-lg text-primary"></span>
        </div>
        <p class="mt-4 text-base-content/60">جاري تحميل بياناتك...</p>
      </div>

      <!-- Error State -->
      <div v-else-if="error" class="card bg-base-100 shadow-xl">
        <div class="card-body items-center text-center py-12">
          <div class="w-16 h-16 bg-error/10 rounded-full flex items-center justify-center mb-4">
            <AlertCircle class="w-8 h-8 text-error" />
          </div>
          <h2 class="card-title text-error mb-2">تعذر تحميل البيانات</h2>
          <p class="text-base-content/60 mb-6 max-w-sm">{{ error }}</p>
          <button @click="loadStudentInfo" class="btn btn-primary gap-2">
            <RefreshCw class="w-4 h-4" />
            إعادة المحاولة
          </button>
        </div>
      </div>

      <!-- Registration Form -->
      <div v-else class="space-y-6">
        <!-- Student Info Card -->
        <div class="card bg-base-100 shadow-xl overflow-hidden">
          <div class="bg-gradient-to-l from-primary/10 via-primary/5 to-transparent">
            <div class="card-body">
              <div class="flex items-center gap-3 mb-4">
                <div class="w-10 h-10 bg-primary/10 rounded-lg flex items-center justify-center">
                  <User class="w-5 h-5 text-primary" />
                </div>
                <div>
                  <h2 class="card-title text-lg">بيانات الطالب</h2>
                  <p class="text-sm text-base-content/50">تم تحميلها تلقائياً من النظام</p>
                </div>
                <div class="badge badge-success badge-outline gap-1 mr-auto">
                  <CheckCircle2 class="w-3 h-3" />
                  مُتحقق
                </div>
              </div>
              
              <div class="grid grid-cols-2 md:grid-cols-4 gap-4">
                <div class="bg-base-200/50 rounded-lg p-3">
                  <p class="text-xs text-base-content/50 mb-1">رقم الطالب</p>
                  <p class="font-bold text-base-content" dir="ltr">{{ studentInfo?.studentId || '-' }}</p>
                </div>
                <div class="bg-base-200/50 rounded-lg p-3 md:col-span-2">
                  <p class="text-xs text-base-content/50 mb-1">اسم الطالب</p>
                  <p class="font-bold text-base-content">{{ studentInfo?.studentName || '-' }}</p>
                </div>
                <div class="bg-base-200/50 rounded-lg p-3">
                  <p class="text-xs text-base-content/50 mb-1">رقم التسجيل</p>
                  <p class="font-bold text-sm" dir="ltr">{{ studentInfo?.studentHalaqaSecId || '-' }}</p>
                </div>
                <div class="bg-base-200/50 rounded-lg p-3 md:col-span-2">
                  <p class="text-xs text-base-content/50 mb-1">المعلم</p>
                  <p class="font-semibold text-sm">{{ studentInfo?.teacherName || '-' }}</p>
                </div>
                <div class="bg-base-200/50 rounded-lg p-3">
                  <p class="text-xs text-base-content/50 mb-1">الفترة</p>
                  <p class="font-semibold text-sm">{{ studentInfo?.periodName || '-' }}</p>
                </div>
                <div class="bg-base-200/50 rounded-lg p-3">
                  <p class="text-xs text-base-content/50 mb-1">نوع الحلقة</p>
                  <p class="font-semibold text-sm">{{ studentInfo?.halaqaTypeName || '-' }}</p>
                </div>
                <div class="bg-base-200/50 rounded-lg p-3">
                  <p class="text-xs text-base-content/50 mb-1">الموقع</p>
                  <p class="font-semibold text-sm">{{ studentInfo?.halaqaLocationName || '-' }}</p>
                </div>
              </div>
            </div>
          </div>
        </div>

        <!-- Address Form Card -->
        <form @submit.prevent="submitRegistration" class="card bg-base-100 shadow-xl">
          <div class="card-body">
            <div class="flex items-center gap-3 mb-6">
              <div class="w-12 h-12 bg-gradient-to-br from-info to-info/70 rounded-xl flex items-center justify-center shadow-lg">
                <MapPin class="w-6 h-6 text-white" />
              </div>
              <div>
                <h2 class="card-title text-xl">بيانات العنوان</h2>
                <p class="text-sm text-base-content/60">أدخل عنوانك الوطني لتحديد موقعك</p>
              </div>
            </div>

            <div class="space-y-5">
              <!-- District Selection -->
              <div class="form-control">
                <label class="label">
                  <span class="label-text font-semibold">المنطقة</span>
                  <span class="badge badge-ghost">اختياري</span>
                </label>
                <select 
                  v-model="form.districtId" 
                  class="select select-bordered select-lg w-full"
                >
                  <option value="">اختر المنطقة السكنية</option>
                  <option v-for="district in districts" :key="district.id" :value="district.id">
                    {{ district.districtNameAr }}
                  </option>
                </select>
              </div>

              <!-- National Short Address -->
              <div class="form-control">
                <label class="label">
                  <span class="label-text font-semibold">العنوان الوطني المختصر <span class="text-error">*</span></span>
                </label>
                <div class="relative">
                  <input 
                    v-model="form.nationalShortAddress"
                    type="text"
                    class="input input-bordered input-lg w-full uppercase tracking-widest text-center font-mono"
                    placeholder="XXXX0000"
                    maxlength="8"
                    pattern="[A-Za-z]{4}[0-9]{4}"
                    dir="ltr"
                    required
                  />
                  <div class="absolute left-3 top-1/2 -translate-y-1/2">
                    <Home class="w-5 h-5 text-base-content/30" />
                  </div>
                </div>
                <label class="label">
                  <span class="label-text-alt flex items-center gap-1">
                    <Info class="w-3 h-3" />
                    4 أحرف إنجليزية + 4 أرقام (مثال: RRRD2929)
                  </span>
                  <a href="https://splonline.com.sa/ar/national-address-1/" target="_blank" class="label-text-alt link link-primary flex items-center gap-1">
                    <ExternalLink class="w-3 h-3" />
                    كيف أجد عنواني؟
                  </a>
                </label>
              </div>

              <!-- Home Address (Optional) -->
              <div class="form-control">
                <label class="label">
                  <span class="label-text font-semibold">وصف إضافي للعنوان</span>
                  <span class="badge badge-ghost">اختياري</span>
                </label>
                <textarea 
                  v-model="form.homeAddress"
                  class="textarea textarea-bordered h-24 leading-relaxed"
                  placeholder="أضف وصف إضافي للعنوان مثل: بجوار مسجد الرحمة، قرب مدرسة الفاروق..."
                ></textarea>
              </div>
            </div>

            <div class="divider"></div>

            <!-- Form Actions -->
            <div class="flex flex-col-reverse sm:flex-row gap-3 justify-end">
              <BaseButton to="/" variant="ghost">
                <template #icon>
                  <X />
                </template>
                إلغاء
              </BaseButton>
              <BaseButton 
                type="submit"
                size="lg"
                :disabled="!isFormValid"
                :loading="isSubmitting"
              >
                <template #icon>
                  <Send />
                </template>
                تقديم طلب التسجيل
              </BaseButton>
            </div>
          </div>
        </form>

        <!-- Info Alert -->
        <div class="alert shadow-lg">
          <Info class="w-5 h-5 text-info" />
          <div>
            <h3 class="font-bold text-sm">ملاحظة هامة</h3>
            <p class="text-xs">سيتم مراجعة طلبك من قبل إدارة النقل وإشعارك بالنتيجة خلال 48 ساعة.</p>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, computed, onMounted } from 'vue'
import { useRouter } from 'vue-router'
import { Bus, User, MapPin, Home, Send, AlertCircle, RefreshCw, CheckCircle2, Info, ExternalLink, X, ArrowRight } from 'lucide-vue-next'
import apiClient from '@/services/api/axios.config'
import BaseButton from '@/components/ui/BaseButton.vue'

interface StudentInfo {
  studentUserId: number
  studentId: string
  studentName: string
  halaqaTypeCode?: string
  halaqaTypeName?: string
  halaqaSectionId?: number
  studentHalaqaSecId?: number
  halaqaGender?: string
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
  districtNameAr: string
  districtNameEn?: string
  isActive: boolean
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
  return addressPattern.test(form.value.nationalShortAddress)
})

const checkExistingRegistration = async () => {
  try {
    const response = await apiClient.get('/registration/my-registration')
    if (response.data.success && response.data.data) {
      router.push('/my-registration')
      return true
    }
  } catch {
    // No existing registration - continue
  }
  return false
}

const loadStudentInfo = async () => {
  isLoading.value = true
  error.value = null
  
  // Check if student already has a registration
  const hasExisting = await checkExistingRegistration()
  if (hasExisting) return
  
  try {
    const [studentResponse, districtsResponse] = await Promise.all([
      apiClient.get('/registration/student-info'),
      apiClient.get('/districts')
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
    const response = await apiClient.post('/registration', {
      districtId: form.value.districtId || null,
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
