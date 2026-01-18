<template>
  <div class="container mx-auto p-4 max-w-7xl">
    <!-- Header -->
    <div class="mb-6">
      <h1 class="text-2xl font-bold text-base-content">لوحة التحكم - إدارة النقل</h1>
      <p class="text-base-content/60 mt-1">نظرة عامة على أسطول النقل والإحصائيات</p>
    </div>

    <!-- Loading State -->
    <div v-if="loading" class="flex justify-center py-12">
      <span class="loading loading-spinner loading-lg text-primary"></span>
    </div>

    <!-- Error State -->
    <div v-else-if="error" class="alert alert-error mb-6">
      <AlertCircle class="w-5 h-5" />
      <span>{{ error }}</span>
      <button @click="loadData" class="btn btn-sm btn-ghost">إعادة المحاولة</button>
    </div>

    <template v-else>
      <!-- Main Stats -->
      <div class="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-4 gap-4 mb-8">
        <div class="stat bg-base-100 rounded-box shadow-sm">
          <div class="stat-figure text-primary">
            <Bus class="w-10 h-10" />
          </div>
          <div class="stat-title">الباصات</div>
          <div class="stat-value text-primary">{{ statistics?.totalBuses || 0 }}</div>
          <div class="stat-desc">
            <span class="text-success">{{ statistics?.activeBuses || 0 }} نشط</span> | 
            <span class="text-error">{{ statistics?.inactiveBuses || 0 }} متوقف</span>
          </div>
        </div>

        <div class="stat bg-base-100 rounded-box shadow-sm">
          <div class="stat-figure text-secondary">
            <Users class="w-10 h-10" />
          </div>
          <div class="stat-title">السائقون</div>
          <div class="stat-value text-secondary">{{ statistics?.totalDrivers || 0 }}</div>
          <div class="stat-desc">
            <span class="text-success">{{ statistics?.activeDrivers || 0 }} نشط</span>
          </div>
        </div>

        <div class="stat bg-base-100 rounded-box shadow-sm">
          <div class="stat-figure text-accent">
            <MapPin class="w-10 h-10" />
          </div>
          <div class="stat-title">المسارات</div>
          <div class="stat-value text-accent">{{ statistics?.totalRoutes || 0 }}</div>
          <div class="stat-desc">
            <span class="text-success">{{ statistics?.activeRoutes || 0 }} نشط</span>
          </div>
        </div>

        <div class="stat bg-base-100 rounded-box shadow-sm">
          <div class="stat-figure text-info">
            <Armchair class="w-10 h-10" />
          </div>
          <div class="stat-title">السعة الإجمالية</div>
          <div class="stat-value text-info">{{ statistics?.totalCapacity || 0 }}</div>
          <div class="stat-desc">مقعد متاح</div>
        </div>
      </div>

      <!-- Quick Actions -->
      <div class="card bg-base-100 shadow-sm">
        <div class="card-body">
          <h2 class="card-title mb-4">
            <Zap class="w-5 h-5" />
            إجراءات سريعة
          </h2>
          <div class="grid grid-cols-1 sm:grid-cols-3 gap-4">
            <router-link to="/bus-management/drivers" class="btn btn-outline gap-2">
              <Users class="w-5 h-5" />
              إدارة السائقين
            </router-link>
            <router-link to="/bus-management/routes" class="btn btn-outline gap-2">
              <MapPin class="w-5 h-5" />
              إدارة المسارات
            </router-link>
            <router-link to="/bus-management/buses" class="btn btn-outline gap-2">
              <Bus class="w-5 h-5" />
              إدارة الباصات
            </router-link>
          </div>
        </div>
      </div>
    </template>
  </div>
</template>

<script setup lang="ts">
import { onMounted } from 'vue'
import { useBusManagementStore } from '@/stores/busManagement'
import { storeToRefs } from 'pinia'
import { 
  Bus as BusIcon, Users, MapPin, Armchair, AlertCircle, Zap
} from 'lucide-vue-next'

const Bus = BusIcon

const store = useBusManagementStore()
const { statistics, loading, error } = storeToRefs(store)

async function loadData() {
  await store.fetchStatistics()
}

onMounted(() => {
  loadData()
})
</script>
