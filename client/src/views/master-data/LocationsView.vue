<script setup lang="ts">
import { ref, computed, onMounted } from 'vue'
import { ArrowRight, Plus, Search, X, Edit, AlertCircle } from 'lucide-vue-next'
import apiClient from '@/services/api/axios.config'

interface Location {
  id: string
  locationId?: string
  locationCode: string
  locationName: string
  locationType?: string
  isActive: boolean
}

const locations = ref<Location[]>([])
const searchQuery = ref('')
const isLoading = ref(false)
const error = ref<string | null>(null)

// Modal state
const showModal = ref(false)
const isEditing = ref(false)
const isSaving = ref(false)
const formError = ref<string | null>(null)
const selectedLocation = ref<Location | null>(null)
const form = ref({
  locationCode: '',
  locationName: '',
  locationType: '',
  isActive: true
})

async function fetchLocations() {
  isLoading.value = true
  error.value = null
  try {
    const response = await apiClient.get('/locations')
    locations.value = response.data.data || response.data || []
  } catch (err: any) {
    console.error('Failed to fetch locations:', err)
    error.value = err.response?.data?.message || 'فشل في تحميل المواقف'
  } finally {
    isLoading.value = false
  }
}

const filteredLocations = computed(() => {
  if (!searchQuery.value) return locations.value
  return locations.value.filter(l => 
    l.locationName.includes(searchQuery.value) ||
    l.locationCode.toLowerCase().includes(searchQuery.value.toLowerCase())
  )
})

function openAddModal() {
  isEditing.value = false
  selectedLocation.value = null
  form.value = { locationCode: '', locationName: '', locationType: '', isActive: true }
  formError.value = null
  showModal.value = true
}

function openEditModal(location: Location) {
  isEditing.value = true
  selectedLocation.value = location
  form.value = {
    locationCode: location.locationCode,
    locationName: location.locationName,
    locationType: location.locationType || '',
    isActive: location.isActive
  }
  formError.value = null
  showModal.value = true
}

function closeModal() {
  showModal.value = false
  selectedLocation.value = null
  formError.value = null
}

async function saveLocation() {
  if (!form.value.locationCode.trim() || !form.value.locationName.trim()) {
    formError.value = 'كود الموقف واسم الموقف مطلوبان'
    return
  }
  
  isSaving.value = true
  formError.value = null
  
  try {
    if (isEditing.value && selectedLocation.value) {
      await apiClient.put(`/locations/${selectedLocation.value.id || selectedLocation.value.locationId}`, form.value)
    } else {
      await apiClient.post('/locations', form.value)
    }
    closeModal()
    await fetchLocations()
  } catch (err: any) {
    console.error('Failed to save location:', err)
    formError.value = err.response?.data?.message || 'فشل في حفظ الموقف'
  } finally {
    isSaving.value = false
  }
}

onMounted(() => {
  fetchLocations()
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
          <h1 class="text-xl font-bold font-cairo">إدارة المواقف</h1>
        </div>
      </div>
    </header>

    <!-- Main Content -->
    <main class="container mx-auto px-4 py-8">
      <!-- Error Alert -->
      <div v-if="error" class="alert alert-error mb-4 flex items-center gap-2">
        <AlertCircle class="w-5 h-5" />
        <span>{{ error }}</span>
        <button class="btn btn-ghost btn-sm mr-auto" @click="fetchLocations">إعادة المحاولة</button>
      </div>

      <!-- Actions Bar -->
      <div class="flex flex-col sm:flex-row gap-4 mb-6">
        <div class="relative flex-1">
          <Search class="absolute right-3 top-1/2 -translate-y-1/2 w-5 h-5 text-neutral" />
          <input
            v-model="searchQuery"
            type="text"
            placeholder="بحث في المواقف..."
            class="input input-bordered pr-10 w-full"
          />
        </div>
        <button class="btn btn-primary flex items-center gap-2" @click="openAddModal">
          <Plus class="w-5 h-5" />
          <span>إضافة موقف جديد</span>
        </button>
      </div>

      <!-- Locations Grid -->
      <div v-if="isLoading" class="flex justify-center py-12">
        <span class="loading loading-spinner loading-lg text-primary"></span>
      </div>
      <div v-else-if="filteredLocations.length === 0" class="text-center py-12 text-base-content/60">
        لا توجد مواقف
      </div>
      <div v-else class="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-4">
        <div
          v-for="location in filteredLocations"
          :key="location.locationId"
          class="card bg-base-100 shadow hover:shadow-lg transition-shadow p-4"
        >
          <div class="flex items-center justify-between mb-3">
            <span class="text-2xl font-bold text-primary">{{ location.locationCode }}</span>
            <span :class="location.isActive ? 'badge badge-success' : 'badge badge-error'">
              {{ location.isActive ? 'نشط' : 'غير نشط' }}
            </span>
          </div>
          <p class="text-base-content font-medium mb-2">{{ location.locationName }}</p>
          <p class="text-sm text-base-content/60 mb-3">{{ location.locationType || 'موقف' }}</p>
          <button class="btn btn-ghost btn-sm gap-1" @click="openEditModal(location)">
            <Edit class="w-4 h-4" />
            تعديل
          </button>
        </div>
      </div>
    </main>

    <!-- Add/Edit Modal -->
    <dialog class="modal" :class="{ 'modal-open': showModal }">
      <div class="modal-box">
        <button class="btn btn-sm btn-circle btn-ghost absolute left-2 top-2" @click="closeModal">
          <X class="w-4 h-4" />
        </button>
        <h3 class="font-bold text-lg mb-4">{{ isEditing ? 'تعديل الموقف' : 'إضافة موقف جديد' }}</h3>
        
        <div v-if="formError" class="alert alert-error mb-4">
          <AlertCircle class="w-5 h-5" />
          <span>{{ formError }}</span>
        </div>
        
        <form @submit.prevent="saveLocation" class="space-y-4">
          <div class="form-control">
            <label class="label"><span class="label-text">كود الموقف <span class="text-error">*</span></span></label>
            <input v-model="form.locationCode" type="text" class="input input-bordered" dir="ltr" required />
          </div>
          <div class="form-control">
            <label class="label"><span class="label-text">اسم الموقف <span class="text-error">*</span></span></label>
            <input v-model="form.locationName" type="text" class="input input-bordered" required />
          </div>
          <div class="form-control">
            <label class="label"><span class="label-text">نوع الموقف</span></label>
            <input v-model="form.locationType" type="text" class="input input-bordered" />
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
