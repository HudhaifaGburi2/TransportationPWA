<template>
  <div class="space-y-6">
    <!-- Header -->
    <div class="flex flex-col md:flex-row md:items-center md:justify-between gap-4">
      <div>
        <h1 class="text-2xl font-bold text-base-content">إدارة الباصات</h1>
        <p class="text-base-content/60 text-sm mt-1">عرض وإدارة جميع الباصات والسائقين</p>
      </div>
      <button @click="showAddModal = true" class="btn btn-primary gap-2">
        <Plus class="w-5 h-5" />
        إضافة باص
      </button>
    </div>

    <!-- Summary Cards -->
    <div class="grid grid-cols-2 md:grid-cols-4 gap-4">
      <div class="stat bg-base-100 rounded-lg shadow">
        <div class="stat-figure text-primary"><Bus class="w-8 h-8" /></div>
        <div class="stat-title">إجمالي الباصات</div>
        <div class="stat-value text-primary">{{ summary?.totalBuses || buses.length }}</div>
      </div>
      <div class="stat bg-base-100 rounded-lg shadow">
        <div class="stat-figure text-success"><CheckCircle class="w-8 h-8" /></div>
        <div class="stat-title">الباصات النشطة</div>
        <div class="stat-value text-success">{{ summary?.activeBuses || activeBusesCount }}</div>
      </div>
      <div class="stat bg-base-100 rounded-lg shadow">
        <div class="stat-figure text-info"><Users class="w-8 h-8" /></div>
        <div class="stat-title">إجمالي السعة</div>
        <div class="stat-value text-info">{{ summary?.totalCapacity || totalCapacity }}</div>
      </div>
      <div class="stat bg-base-100 rounded-lg shadow">
        <div class="stat-figure text-secondary"><User class="w-8 h-8" /></div>
        <div class="stat-title">السائقين</div>
        <div class="stat-value text-secondary">{{ summary?.totalDrivers || driversCount }}</div>
      </div>
    </div>

    <!-- Filters -->
    <div class="bg-base-100 p-4 rounded-lg shadow">
      <div class="flex flex-col md:flex-row gap-4">
        <div class="form-control flex-1 relative">
          <Search class="absolute right-3 top-1/2 -translate-y-1/2 w-5 h-5 text-base-content/40" />
          <input
            v-model="searchQuery"
            type="text"
            placeholder="بحث برقم الباص، اسم السائق، أو رقم الهاتف..."
            class="input input-bordered w-full pr-10"
          />
        </div>
        <select v-model="selectedPeriod" class="select select-bordered">
          <option :value="null">جميع الفترات</option>
          <option v-for="p in periods" :key="p.id" :value="p.id">{{ p.name }}</option>
        </select>
        <select v-model="selectedStatus" class="select select-bordered">
          <option :value="null">جميع الحالات</option>
          <option :value="true">نشط</option>
          <option :value="false">غير نشط</option>
        </select>
        <button @click="refreshData" class="btn btn-ghost gap-2" :disabled="loading">
          <RefreshCw class="w-4 h-4" :class="{ 'animate-spin': loading }" />
          تحديث
        </button>
      </div>
    </div>

    <!-- Period Tabs -->
    <div class="tabs tabs-boxed bg-base-100 p-2 overflow-x-auto">
      <button
        class="tab"
        :class="{ 'tab-active': activePeriodTab === null }"
        @click="activePeriodTab = null"
      >
        الكل ({{ buses.length }})
      </button>
      <button
        v-for="period in periods"
        :key="period.id"
        class="tab"
        :class="{ 'tab-active': activePeriodTab === period.id }"
        @click="activePeriodTab = period.id"
      >
        {{ period.name }} ({{ busesByPeriod[period.id]?.length || 0 }})
      </button>
    </div>

    <!-- Error State -->
    <div v-if="error" class="alert alert-error shadow-lg">
      <AlertCircle class="w-6 h-6" />
      <div>
        <h3 class="font-bold">خطأ في تحميل البيانات</h3>
        <div class="text-xs">{{ error }}</div>
      </div>
      <button @click="refreshData" class="btn btn-sm btn-ghost">إعادة المحاولة</button>
    </div>

    <!-- Loading Skeleton -->
    <div v-else-if="loading" class="card bg-base-100 shadow">
      <div class="overflow-x-auto">
        <table class="table">
          <thead>
            <tr>
              <th>رقم الباص</th>
              <th>السائق</th>
              <th>الهاتف</th>
              <th>المسار</th>
              <th>السعة</th>
              <th>الحالة</th>
              <th>الإجراءات</th>
            </tr>
          </thead>
          <tbody>
            <tr v-for="i in 8" :key="i" class="animate-pulse">
              <td><div class="h-4 bg-base-300 rounded w-12"></div></td>
              <td><div class="h-4 bg-base-300 rounded w-32"></div></td>
              <td><div class="h-4 bg-base-300 rounded w-24"></div></td>
              <td><div class="h-4 bg-base-300 rounded w-40"></div></td>
              <td><div class="h-4 bg-base-300 rounded w-8"></div></td>
              <td><div class="h-6 bg-base-300 rounded w-16"></div></td>
              <td><div class="h-8 bg-base-300 rounded w-20"></div></td>
            </tr>
          </tbody>
        </table>
      </div>
    </div>

    <!-- Empty State -->
    <div v-else-if="filteredBuses.length === 0" class="card bg-base-100 shadow">
      <div class="card-body items-center text-center py-16">
        <Bus class="w-20 h-20 text-base-content/20 mb-4" />
        <h3 class="text-xl font-bold">لا توجد باصات</h3>
        <p class="text-base-content/60">{{ searchQuery ? 'لم يتم العثور على نتائج للبحث' : 'ابدأ بإضافة باصات جديدة' }}</p>
        <button v-if="!searchQuery" @click="showAddModal = true" class="btn btn-primary mt-4 gap-2">
          <Plus class="w-5 h-5" />
          إضافة باص
        </button>
      </div>
    </div>

    <!-- Buses Table -->
    <div v-else class="card bg-base-100 shadow overflow-hidden">
      <div class="overflow-x-auto">
        <table class="table table-zebra">
          <thead class="bg-base-200">
            <tr>
              <th class="font-bold">رقم الباص</th>
              <th class="font-bold">السائق</th>
              <th class="font-bold">الهاتف</th>
              <th class="font-bold">المسار</th>
              <th class="font-bold">السعة</th>
              <th class="font-bold">الفترة</th>
              <th class="font-bold">الحالة</th>
              <th class="font-bold">الإجراءات</th>
            </tr>
          </thead>
          <tbody>
            <tr v-for="bus in filteredBuses" :key="bus.id" class="hover">
              <td>
                <div class="font-bold text-primary">{{ bus.busNumber || '-' }}</div>
              </td>
              <td>
                <div class="flex items-center gap-2">
                  <div class="avatar placeholder">
                    <div class="bg-neutral text-neutral-content rounded-full w-8">
                      <span class="text-xs">{{ (bus.driverName || '?')[0] }}</span>
                    </div>
                  </div>
                  <span>{{ bus.driverName || '-' }}</span>
                </div>
              </td>
              <td>
                <span v-if="bus.driverPhoneNumber" dir="ltr" class="font-mono text-sm">
                  {{ bus.driverPhoneNumber }}
                </span>
                <span v-else class="text-base-content/40">-</span>
              </td>
              <td>
                <span v-if="bus.routeName" class="badge badge-outline badge-sm">{{ bus.routeName }}</span>
                <span v-else class="text-base-content/40">-</span>
              </td>
              <td>
                <span class="badge badge-ghost">{{ bus.capacity }}</span>
              </td>
              <td>
                <span class="text-sm">{{ getPeriodName(bus.periodId) }}</span>
              </td>
              <td>
                <span :class="['badge badge-sm', bus.isActive ? 'badge-success' : 'badge-error']">
                  {{ bus.isActive ? 'نشط' : 'غير نشط' }}
                </span>
              </td>
              <td>
                <div class="flex gap-1">
                  <button @click="viewBus(bus)" class="btn btn-ghost btn-xs" title="عرض">
                    <Eye class="w-4 h-4" />
                  </button>
                  <button @click="editBus(bus)" class="btn btn-ghost btn-xs" title="تعديل">
                    <Pencil class="w-4 h-4" />
                  </button>
                  <button @click="confirmDelete(bus)" class="btn btn-ghost btn-xs text-error" title="حذف">
                    <Trash2 class="w-4 h-4" />
                  </button>
                </div>
              </td>
            </tr>
          </tbody>
        </table>
      </div>
      <div class="p-4 border-t border-base-200 text-sm text-base-content/60">
        عرض {{ filteredBuses.length }} من {{ buses.length }} باص
      </div>
    </div>

    <!-- Add/Edit Modal -->
    <dialog class="modal" :class="{ 'modal-open': showAddModal || showEditModal }">
      <div class="modal-box max-w-2xl">
        <button @click="closeModal" class="btn btn-sm btn-circle btn-ghost absolute left-2 top-2">✕</button>
        <h3 class="font-bold text-lg mb-4">{{ showEditModal ? 'تعديل الباص' : 'إضافة باص جديد' }}</h3>
        <BusForm
          :bus="selectedBus"
          :periods="periods"
          @submit="saveBus"
          @cancel="closeModal"
        />
      </div>
      <div class="modal-backdrop bg-black/50" @click="closeModal"></div>
    </dialog>

    <!-- Delete Confirmation -->
    <dialog class="modal" :class="{ 'modal-open': showDeleteModal }">
      <div class="modal-box max-w-sm text-center">
        <div class="w-16 h-16 bg-error/10 rounded-full flex items-center justify-center mx-auto mb-4">
          <AlertTriangle class="w-8 h-8 text-error" />
        </div>
        <h3 class="font-bold text-xl">تأكيد الحذف</h3>
        <p class="py-4">
          هل أنت متأكد من حذف الباص رقم<br/>
          <strong class="text-primary">{{ busToDelete?.busNumber || busToDelete?.plateNumber || '-' }}</strong>؟
        </p>
        <div class="flex gap-3 justify-center">
          <button class="btn btn-ghost" @click="showDeleteModal = false">إلغاء</button>
          <button class="btn btn-error" @click="deleteBusConfirmed" :disabled="loading">
            <span v-if="loading" class="loading loading-spinner loading-sm"></span>
            حذف
          </button>
        </div>
      </div>
      <div class="modal-backdrop bg-black/50" @click="showDeleteModal = false"></div>
    </dialog>
  </div>
</template>

<script setup lang="ts">
import { ref, computed, onMounted, watch } from 'vue'
import { useRouter } from 'vue-router'
import { 
  Plus, Search, RefreshCw, Bus, CheckCircle, Users, User,
  AlertCircle, AlertTriangle, Eye, Pencil, Trash2 
} from 'lucide-vue-next'
import { useBusStore, type Bus as BusType } from '@/stores/bus'
import BusForm from '@/components/buses/BusForm.vue'
import apiClient from '@/services/api/axios.config'

interface Period {
  id: number
  name: string
}

const router = useRouter()
const busStore = useBusStore()

const searchQuery = ref('')
const selectedPeriod = ref<number | null>(null)
const selectedStatus = ref<boolean | null>(null)
const activePeriodTab = ref<number | null>(null)
const periods = ref<Period[]>([])
const periodsLoading = ref(false)

const showAddModal = ref(false)
const showEditModal = ref(false)
const showDeleteModal = ref(false)
const selectedBus = ref<BusType | null>(null)
const busToDelete = ref<BusType | null>(null)

const loading = computed(() => busStore.loading)
const error = computed(() => busStore.error)
const buses = computed(() => busStore.buses)
const busesByPeriod = computed(() => busStore.busesByPeriod)
const summary = computed(() => busStore.summary)

const activeBusesCount = computed(() => buses.value.filter(b => b.isActive).length)
const totalCapacity = computed(() => buses.value.reduce((sum, b) => sum + b.capacity, 0))
const driversCount = computed(() => buses.value.filter(b => b.driverName).length)

const filteredBuses = computed(() => {
  let result = buses.value

  if (activePeriodTab.value !== null) {
    result = result.filter(b => b.periodId === activePeriodTab.value)
  }

  if (searchQuery.value) {
    const query = searchQuery.value.toLowerCase()
    result = result.filter(b =>
      (b.busNumber || b.plateNumber || '').toLowerCase().includes(query) ||
      (b.driverName || '').toLowerCase().includes(query) ||
      (b.driverPhoneNumber || '').includes(query) ||
      (b.licensePlate || '').toLowerCase().includes(query)
    )
  }

  if (selectedPeriod.value !== null) {
    result = result.filter(b => b.periodId === selectedPeriod.value)
  }

  if (selectedStatus.value !== null) {
    result = result.filter(b => b.isActive === selectedStatus.value)
  }

  return result
})

const fetchPeriods = async () => {
  periodsLoading.value = true
  try {
    const response = await apiClient.get('/lookups/periods')
    if (response.data.success && response.data.data) {
      periods.value = response.data.data.map((p: any) => ({
        id: p.id || p.periodId,
        name: p.name || p.periodName || `الفترة ${p.id}`
      }))
    }
  } catch (e) {
    console.error('Error fetching periods:', e)
    periods.value = [
      { id: 1, name: 'الفترة الأولى' },
      { id: 2, name: 'الفترة الثانية' },
      { id: 3, name: 'الفترة الثالثة' },
      { id: 4, name: 'الفترة الرابعة' },
      { id: 5, name: 'الفترة الخامسة' }
    ]
  } finally {
    periodsLoading.value = false
  }
}

const getPeriodName = (periodId: number) => {
  const period = periods.value.find(p => p.id === periodId)
  return period?.name || `الفترة ${periodId}`
}

const refreshData = async () => {
  await busStore.fetchBuses()
  await busStore.fetchSummary()
}

const editBus = (bus: BusType) => {
  selectedBus.value = bus
  showEditModal.value = true
}

const viewBus = (bus: BusType) => {
  router.push({ name: 'BusDetail', params: { id: bus.id || bus.busId } })
}

const confirmDelete = (bus: BusType) => {
  busToDelete.value = bus
  showDeleteModal.value = true
}

const deleteBusConfirmed = async () => {
  if (busToDelete.value) {
    const success = await busStore.deleteBus(busToDelete.value.id || busToDelete.value.busId!)
    if (success) {
      showDeleteModal.value = false
      busToDelete.value = null
    }
  }
}

const saveBus = async (busData: any) => {
  let success = false
  if (showEditModal.value && selectedBus.value) {
    success = !!(await busStore.updateBus(selectedBus.value.id || selectedBus.value.busId!, busData))
  } else {
    success = !!(await busStore.createBus(busData))
  }
  if (success) closeModal()
}

const closeModal = () => {
  showAddModal.value = false
  showEditModal.value = false
  selectedBus.value = null
}

watch([selectedPeriod, selectedStatus], () => {
  busStore.fetchBuses({
    periodId: selectedPeriod.value ?? undefined,
    isActive: selectedStatus.value ?? undefined,
    search: searchQuery.value || undefined
  })
})

onMounted(async () => {
  await Promise.all([
    fetchPeriods(),
    busStore.fetchBuses(),
    busStore.fetchSummary()
  ])
})
</script>
