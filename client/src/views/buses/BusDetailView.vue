<template>
  <div class="space-y-6">
    <!-- Header -->
    <div class="flex items-center gap-4">
      <button @click="$router.back()" class="btn btn-ghost btn-circle">
        <ArrowRight class="w-6 h-6" />
      </button>
      <div>
        <h1 class="text-2xl font-bold">باص {{ bus?.busNumber }}</h1>
        <p class="text-base-content/60">{{ bus?.periodName }}</p>
      </div>
    </div>

    <div v-if="loading" class="flex justify-center py-12">
      <span class="loading loading-spinner loading-lg text-primary"></span>
    </div>

    <template v-else-if="bus">
      <!-- Stats Cards -->
      <div class="grid grid-cols-2 md:grid-cols-4 gap-4">
        <div class="stat bg-base-100 rounded-lg shadow">
          <div class="stat-title">السعة</div>
          <div class="stat-value">{{ bus.capacity }}</div>
        </div>
        <div class="stat bg-base-100 rounded-lg shadow">
          <div class="stat-title">الطلاب الحاليين</div>
          <div class="stat-value text-primary">{{ bus.currentStudentCount }}</div>
        </div>
        <div class="stat bg-base-100 rounded-lg shadow">
          <div class="stat-title">نسبة الاستخدام</div>
          <div class="stat-value" :class="utilizationClass">{{ bus.utilizationPercentage }}%</div>
        </div>
        <div class="stat bg-base-100 rounded-lg shadow">
          <div class="stat-title">الحالة</div>
          <div class="stat-value text-sm">
            <span :class="bus.isActive ? 'badge badge-success' : 'badge badge-error'">
              {{ bus.isActive ? 'نشط' : 'غير نشط' }}
            </span>
          </div>
        </div>
      </div>

      <!-- Details -->
      <div class="grid grid-cols-1 md:grid-cols-2 gap-6">
        <!-- Driver Info -->
        <div class="card bg-base-100 shadow">
          <div class="card-body">
            <h2 class="card-title text-lg">معلومات السائق</h2>
            <div class="space-y-3">
              <div class="flex items-center gap-3">
                <User class="w-5 h-5 text-base-content/60" />
                <span>{{ bus.driverName || 'غير محدد' }}</span>
              </div>
              <div class="flex items-center gap-3">
                <Phone class="w-5 h-5 text-base-content/60" />
                <span dir="ltr">{{ bus.driverPhoneNumber || 'غير محدد' }}</span>
              </div>
            </div>
          </div>
        </div>

        <!-- Route Info -->
        <div class="card bg-base-100 shadow">
          <div class="card-body">
            <h2 class="card-title text-lg">المسار</h2>
            <div class="flex items-center gap-3">
              <MapPin class="w-5 h-5 text-base-content/60" />
              <span>{{ bus.routeName || 'غير محدد' }}</span>
            </div>
          </div>
        </div>
      </div>

      <!-- Districts -->
      <div class="card bg-base-100 shadow">
        <div class="card-body">
          <h2 class="card-title text-lg">المناطق المخدومة</h2>
          <div v-if="bus.districts?.length" class="flex flex-wrap gap-2 mt-2">
            <span v-for="d in bus.districts" :key="d.districtId" class="badge badge-outline badge-lg">
              {{ d.districtNameAr }}
            </span>
          </div>
          <p v-else class="text-base-content/60">لا توجد مناطق مخصصة</p>
        </div>
      </div>

      <!-- Statistics -->
      <div v-if="statistics" class="card bg-base-100 shadow">
        <div class="card-body">
          <h2 class="card-title text-lg">إحصائيات الباص</h2>
          <div class="grid grid-cols-3 gap-4 mt-4">
            <div class="text-center">
              <div class="text-2xl font-bold text-primary">{{ statistics.totalStudents }}</div>
              <div class="text-sm text-base-content/60">إجمالي الطلاب</div>
            </div>
            <div class="text-center">
              <div class="text-2xl font-bold text-success">{{ statistics.activeStudents }}</div>
              <div class="text-sm text-base-content/60">نشط</div>
            </div>
            <div class="text-center">
              <div class="text-2xl font-bold text-warning">{{ statistics.suspendedStudents }}</div>
              <div class="text-sm text-base-content/60">موقوف</div>
            </div>
          </div>
        </div>
      </div>
    </template>

    <div v-else class="text-center py-12 text-base-content/60">
      الباص غير موجود
    </div>
  </div>
</template>

<script setup lang="ts">
import { computed, onMounted } from 'vue'
import { useRoute } from 'vue-router'
import { ArrowRight, User, Phone, MapPin } from 'lucide-vue-next'
import { useBusStore } from '@/stores/bus'

const route = useRoute()
const busStore = useBusStore()

const busId = computed(() => route.params.id as string)
const loading = computed(() => busStore.loading)
const bus = computed(() => busStore.currentBus)
const statistics = computed(() => busStore.statistics)

const utilizationClass = computed(() => {
  const pct = bus.value?.utilizationPercentage || 0
  if (pct >= 90) return 'text-error'
  if (pct >= 70) return 'text-warning'
  return 'text-success'
})

onMounted(async () => {
  if (busId.value) {
    await busStore.fetchBusById(busId.value)
    await busStore.fetchStatistics(busId.value)
  }
})
</script>
