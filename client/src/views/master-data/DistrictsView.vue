<script setup lang="ts">
import { ref, onMounted } from 'vue'
import { ArrowRight, Plus, Search } from 'lucide-vue-next'
import apiClient from '@/services/api/axios.config'

interface District {
  districtId: string
  districtNameAr: string
  districtNameEn?: string
  isActive: boolean
}

const districts = ref<District[]>([])
const searchQuery = ref('')
const isLoading = ref(false)

async function fetchDistricts() {
  isLoading.value = true
  try {
    const response = await apiClient.get('/districts')
    districts.value = response.data.data || response.data || []
  } catch (error) {
    console.error('Failed to fetch districts:', error)
  } finally {
    isLoading.value = false
  }
}

const filteredDistricts = () => {
  if (!searchQuery.value) return districts.value
  return districts.value.filter(d => 
    d.districtNameAr.includes(searchQuery.value) ||
    d.districtNameEn?.toLowerCase().includes(searchQuery.value.toLowerCase())
  )
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
      <!-- Actions Bar -->
      <div class="flex flex-col sm:flex-row gap-4 mb-6">
        <div class="relative flex-1">
          <Search class="absolute right-3 top-1/2 -translate-y-1/2 w-5 h-5 text-neutral" />
          <input
            v-model="searchQuery"
            type="text"
            placeholder="بحث في الأحياء..."
            class="input pr-10"
          />
        </div>
        <button class="btn btn-primary flex items-center gap-2">
          <Plus class="w-5 h-5" />
          <span>إضافة حي جديد</span>
        </button>
      </div>

      <!-- Districts Table -->
      <div class="card overflow-hidden">
        <div class="overflow-x-auto">
          <table class="w-full">
            <thead class="bg-background">
              <tr>
                <th class="text-right px-4 py-3 text-sm font-semibold text-gray-700">#</th>
                <th class="text-right px-4 py-3 text-sm font-semibold text-gray-700">اسم الحي</th>
                <th class="text-right px-4 py-3 text-sm font-semibold text-gray-700">الحالة</th>
                <th class="text-right px-4 py-3 text-sm font-semibold text-gray-700">الإجراءات</th>
              </tr>
            </thead>
            <tbody class="divide-y divide-border">
              <tr v-if="isLoading">
                <td colspan="4" class="px-4 py-8 text-center text-neutral">
                  جاري التحميل...
                </td>
              </tr>
              <tr v-else-if="filteredDistricts().length === 0">
                <td colspan="4" class="px-4 py-8 text-center text-neutral">
                  لا توجد أحياء
                </td>
              </tr>
              <tr v-for="(district, index) in filteredDistricts()" :key="district.districtId" class="hover:bg-background/50">
                <td class="px-4 py-3 text-sm">{{ index + 1 }}</td>
                <td class="px-4 py-3 text-sm font-medium">{{ district.districtNameAr }}</td>
                <td class="px-4 py-3">
                  <span :class="[district.isActive ? 'badge-success' : 'badge-danger', 'badge']">
                    {{ district.isActive ? 'نشط' : 'غير نشط' }}
                  </span>
                </td>
                <td class="px-4 py-3">
                  <button class="text-secondary hover:text-secondary-light text-sm">
                    تعديل
                  </button>
                </td>
              </tr>
            </tbody>
          </table>
        </div>
      </div>
    </main>
  </div>
</template>
