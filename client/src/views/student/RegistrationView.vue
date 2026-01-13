<template>
  <div class="container mx-auto px-4 py-6">
    <div class="bg-white rounded-lg shadow-lg p-6">
      <h1 class="text-2xl font-bold text-gray-800 mb-6">التسجيل في خدمة النقل</h1>
      
      <div v-if="isLoading" class="flex justify-center py-8">
        <div class="animate-spin rounded-full h-8 w-8 border-b-2 border-primary-600"></div>
      </div>

      <div v-else-if="error" class="bg-red-50 text-red-700 p-4 rounded-lg mb-4">
        {{ error }}
      </div>

      <form v-else @submit.prevent="submitRegistration" class="space-y-6">
        <div class="grid grid-cols-1 md:grid-cols-2 gap-6">
          <div>
            <label class="block text-sm font-medium text-gray-700 mb-1">المنطقة</label>
            <select 
              v-model="form.districtId" 
              class="w-full border border-gray-300 rounded-lg px-3 py-2 focus:ring-2 focus:ring-primary-500"
              required
            >
              <option value="">اختر المنطقة</option>
              <option v-for="district in districts" :key="district.id" :value="district.id">
                {{ district.name }}
              </option>
            </select>
          </div>

          <div>
            <label class="block text-sm font-medium text-gray-700 mb-1">الموقع</label>
            <select 
              v-model="form.locationId" 
              class="w-full border border-gray-300 rounded-lg px-3 py-2 focus:ring-2 focus:ring-primary-500"
              required
            >
              <option value="">اختر الموقع</option>
              <option v-for="location in locations" :key="location.id" :value="location.id">
                {{ location.name }}
              </option>
            </select>
          </div>
        </div>

        <div>
          <label class="block text-sm font-medium text-gray-700 mb-1">ملاحظات</label>
          <textarea 
            v-model="form.notes"
            rows="3"
            class="w-full border border-gray-300 rounded-lg px-3 py-2 focus:ring-2 focus:ring-primary-500"
            placeholder="أضف أي ملاحظات إضافية..."
          ></textarea>
        </div>

        <div class="flex justify-end">
          <button 
            type="submit"
            :disabled="isSubmitting"
            class="bg-primary-600 text-white px-6 py-2 rounded-lg hover:bg-primary-700 disabled:opacity-50 disabled:cursor-not-allowed transition-colors"
          >
            <span v-if="isSubmitting">جاري التسجيل...</span>
            <span v-else>تقديم طلب التسجيل</span>
          </button>
        </div>
      </form>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, onMounted } from 'vue'
import { useRouter } from 'vue-router'

const router = useRouter()

const isLoading = ref(false)
const isSubmitting = ref(false)
const error = ref<string | null>(null)

const districts = ref<Array<{ id: string; name: string }>>([])
const locations = ref<Array<{ id: string; name: string }>>([])

const form = ref({
  districtId: '',
  locationId: '',
  notes: ''
})

onMounted(async () => {
  isLoading.value = true
  try {
    // TODO: Fetch districts and locations from API
  } catch (err: any) {
    error.value = err.message || 'حدث خطأ أثناء تحميل البيانات'
  } finally {
    isLoading.value = false
  }
})

const submitRegistration = async () => {
  isSubmitting.value = true
  try {
    // TODO: Submit registration to API
    router.push('/my-registration')
  } catch (err: any) {
    error.value = err.message || 'حدث خطأ أثناء تقديم الطلب'
  } finally {
    isSubmitting.value = false
  }
}
</script>
