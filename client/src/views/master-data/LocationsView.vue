<script setup lang="ts">
import { ref, onMounted } from 'vue'
import { ArrowRight, Plus, Search } from 'lucide-vue-next'
import apiClient from '@/services/api/axios.config'

interface Location {
  locationId: string
  locationCode: string
  locationName: string
  locationType?: string
  isActive: boolean
}

const locations = ref<Location[]>([])
const searchQuery = ref('')
const isLoading = ref(false)

async function fetchLocations() {
  isLoading.value = true
  try {
    const response = await apiClient.get('/locations')
    locations.value = response.data.data || response.data || []
  } catch (error) {
    console.error('Failed to fetch locations:', error)
  } finally {
    isLoading.value = false
  }
}

const filteredLocations = () => {
  if (!searchQuery.value) return locations.value
  return locations.value.filter(l => 
    l.locationName.includes(searchQuery.value) ||
    l.locationCode.toLowerCase().includes(searchQuery.value.toLowerCase())
  )
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
      <!-- Actions Bar -->
      <div class="flex flex-col sm:flex-row gap-4 mb-6">
        <div class="relative flex-1">
          <Search class="absolute right-3 top-1/2 -translate-y-1/2 w-5 h-5 text-neutral" />
          <input
            v-model="searchQuery"
            type="text"
            placeholder="بحث في المواقف..."
            class="input pr-10"
          />
        </div>
        <button class="btn btn-primary flex items-center gap-2">
          <Plus class="w-5 h-5" />
          <span>إضافة موقف جديد</span>
        </button>
      </div>

      <!-- Locations Grid -->
      <div class="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-4">
        <div v-if="isLoading" class="col-span-full text-center py-8 text-neutral">
          جاري التحميل...
        </div>
        <div v-else-if="filteredLocations().length === 0" class="col-span-full text-center py-8 text-neutral">
          لا توجد مواقف
        </div>
        <div
          v-for="location in filteredLocations()"
          :key="location.locationId"
          class="card hover:shadow-md transition-shadow"
        >
          <div class="flex items-center justify-between mb-3">
            <span class="text-2xl font-bold text-primary">{{ location.locationCode }}</span>
            <span :class="[location.isActive ? 'badge-success' : 'badge-danger', 'badge']">
              {{ location.isActive ? 'نشط' : 'غير نشط' }}
            </span>
          </div>
          <p class="text-gray-700 font-medium mb-2">{{ location.locationName }}</p>
          <p class="text-sm text-neutral">{{ location.locationType || 'موقف سيارات' }}</p>
        </div>
      </div>
    </main>
  </div>
</template>
