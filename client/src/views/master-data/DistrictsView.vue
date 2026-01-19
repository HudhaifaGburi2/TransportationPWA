<script setup lang="ts">
import { ref, computed, onMounted } from 'vue'
import { ArrowRight, Plus, Search, X, Edit, AlertCircle } from 'lucide-vue-next'
import apiClient from '@/services/api/axios.config'

interface District {
  id: string
  districtId?: string
  districtNameAr: string
  districtNameEn?: string
  isActive: boolean
}

const districts = ref<District[]>([])
const searchQuery = ref('')
const isLoading = ref(false)
const error = ref<string | null>(null)

// Modal state
const showModal = ref(false)
const isEditing = ref(false)
const isSaving = ref(false)
const formError = ref<string | null>(null)
const selectedDistrict = ref<District | null>(null)
const form = ref({
  districtNameAr: '',
  districtNameEn: '',
  isActive: true
})

async function fetchDistricts() {
  isLoading.value = true
  error.value = null
  try {
    const response = await apiClient.get('/districts')
    districts.value = response.data.data || response.data || []
  } catch (err: any) {
    console.error('Failed to fetch districts:', err)
    error.value = err.response?.data?.message || 'فشل في تحميل الأحياء'
  } finally {
    isLoading.value = false
  }
}

const filteredDistricts = computed(() => {
  if (!searchQuery.value) return districts.value
  return districts.value.filter(d => 
    d.districtNameAr.includes(searchQuery.value) ||
    d.districtNameEn?.toLowerCase().includes(searchQuery.value.toLowerCase())
  )
})

function openAddModal() {
  isEditing.value = false
  selectedDistrict.value = null
  form.value = { districtNameAr: '', districtNameEn: '', isActive: true }
  formError.value = null
  showModal.value = true
}

function openEditModal(district: District) {
  isEditing.value = true
  selectedDistrict.value = district
  form.value = {
    districtNameAr: district.districtNameAr,
    districtNameEn: district.districtNameEn || '',
    isActive: district.isActive
  }
  formError.value = null
  showModal.value = true
}

function closeModal() {
  showModal.value = false
  selectedDistrict.value = null
  formError.value = null
}

async function saveDistrict() {
  if (!form.value.districtNameAr.trim()) {
    formError.value = 'اسم الحي مطلوب'
    return
  }
  
  isSaving.value = true
  formError.value = null
  
  try {
    if (isEditing.value && selectedDistrict.value) {
      await apiClient.put(`/districts/${selectedDistrict.value.id || selectedDistrict.value.districtId}`, form.value)
    } else {
      await apiClient.post('/districts', form.value)
    }
    closeModal()
    await fetchDistricts()
  } catch (err: any) {
    console.error('Failed to save district:', err)
    formError.value = err.response?.data?.message || 'فشل في حفظ الحي'
  } finally {
    isSaving.value = false
  }
}

onMounted(() => {
  fetchDistricts()
})
</script>

<template>
  <div class="min-h-screen bg-background">
    <!-- Header -->
    <header class="bg-primary text-white shadow-md">
      <div class="container mx-auto px-4 py-4">
        <div class="flex items-center gap-4">
          <router-link to="/" class="p-2 hover:bg-white/20 rounded-lg transition-colors">
            <ArrowRight class="w-5 h-5" />
          </router-link>
          <h1 class="text-xl font-bold font-cairo">إدارة الأحياء</h1>
        </div>
      </div>
    </header>

    <!-- Main Content -->
    <main class="container mx-auto px-4 py-8">
      <!-- Error Alert -->
      <div v-if="error" class="alert alert-error mb-4 flex items-center gap-2">
        <AlertCircle class="w-5 h-5" />
        <span>{{ error }}</span>
        <button class="btn btn-ghost btn-sm mr-auto" @click="fetchDistricts">إعادة المحاولة</button>
      </div>

      <!-- Actions Bar -->
      <div class="flex flex-col sm:flex-row gap-4 mb-6">
        <div class="relative flex-1">
          <Search class="absolute right-3 top-1/2 -translate-y-1/2 w-5 h-5 text-neutral" />
          <input
            v-model="searchQuery"
            type="text"
            placeholder="بحث في الأحياء..."
            class="input input-bordered pr-10 w-full"
          />
        </div>
        <button class="btn btn-primary flex items-center gap-2" @click="openAddModal">
          <Plus class="w-5 h-5" />
          <span>إضافة حي جديد</span>
        </button>
      </div>

      <!-- Districts Table -->
      <div class="card bg-base-100 shadow overflow-hidden">
        <div class="overflow-x-auto">
          <table class="table w-full">
            <thead>
              <tr>
                <th class="text-right">#</th>
                <th class="text-right">اسم الحي (عربي)</th>
                <th class="text-right">اسم الحي (إنجليزي)</th>
                <th class="text-right">الحالة</th>
                <th class="text-right">الإجراءات</th>
              </tr>
            </thead>
            <tbody>
              <tr v-if="isLoading">
                <td colspan="5" class="text-center py-8">
                  <span class="loading loading-spinner loading-md"></span>
                  <p class="mt-2 text-base-content/60">جاري التحميل...</p>
                </td>
              </tr>
              <tr v-else-if="filteredDistricts.length === 0">
                <td colspan="5" class="text-center py-8 text-base-content/60">
                  لا توجد أحياء
                </td>
              </tr>
              <tr v-for="(district, index) in filteredDistricts" :key="district.districtId" class="hover">
                <td>{{ index + 1 }}</td>
                <td class="font-medium">{{ district.districtNameAr }}</td>
                <td>{{ district.districtNameEn || '-' }}</td>
                <td>
                  <span :class="district.isActive ? 'badge badge-success' : 'badge badge-error'">
                    {{ district.isActive ? 'نشط' : 'غير نشط' }}
                  </span>
                </td>
                <td>
                  <button class="btn btn-ghost btn-sm gap-1" @click="openEditModal(district)">
                    <Edit class="w-4 h-4" />
                    تعديل
                  </button>
                </td>
              </tr>
            </tbody>
          </table>
        </div>
      </div>
    </main>

    <!-- Add/Edit Modal -->
    <dialog class="modal" :class="{ 'modal-open': showModal }">
      <div class="modal-box">
        <button class="btn btn-sm btn-circle btn-ghost absolute left-2 top-2" @click="closeModal">
          <X class="w-4 h-4" />
        </button>
        <h3 class="font-bold text-lg mb-4">{{ isEditing ? 'تعديل الحي' : 'إضافة حي جديد' }}</h3>
        
        <div v-if="formError" class="alert alert-error mb-4">
          <AlertCircle class="w-5 h-5" />
          <span>{{ formError }}</span>
        </div>
        
        <form @submit.prevent="saveDistrict" class="space-y-4">
          <div class="form-control">
            <label class="label"><span class="label-text">اسم الحي (عربي) <span class="text-error">*</span></span></label>
            <input v-model="form.districtNameAr" type="text" class="input input-bordered" required />
          </div>
          <div class="form-control">
            <label class="label"><span class="label-text">اسم الحي (إنجليزي)</span></label>
            <input v-model="form.districtNameEn" type="text" class="input input-bordered" dir="ltr" />
          </div>
          <div class="form-control">
            <label class="label cursor-pointer justify-start gap-3">
              <input v-model="form.isActive" type="checkbox" class="toggle toggle-success" />
              <span class="label-text">نشط</span>
            </label>
          </div>
          <div class="modal-action">
            <button type="button" class="btn btn-ghost" @click="closeModal">إلغاء</button>
            <button type="submit" class="btn btn-primary" :disabled="isSaving">
              <span v-if="isSaving" class="loading loading-spinner loading-sm"></span>
              {{ isEditing ? 'حفظ التعديلات' : 'إضافة' }}
            </button>
          </div>
        </form>
      </div>
      <div class="modal-backdrop bg-black/50" @click="closeModal"></div>
    </dialog>
  </div>
</template>
