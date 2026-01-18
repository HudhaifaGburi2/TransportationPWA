<template>
  <div class="space-y-6">
    <!-- Header -->
    <div class="flex flex-col md:flex-row md:items-center md:justify-between gap-4">
      <h1 class="text-2xl font-bold text-base-content">إدارة الباصات</h1>
      <button @click="showAddModal = true" class="btn btn-primary gap-2">
        <Plus class="w-5 h-5" />
        إضافة باص
      </button>
    </div>

    <!-- Summary Cards -->
    <div class="grid grid-cols-2 md:grid-cols-4 gap-4">
      <div class="stat bg-base-100 rounded-lg shadow">
        <div class="stat-title">إجمالي الباصات</div>
        <div class="stat-value text-primary">{{ summary?.totalBuses || 0 }}</div>
      </div>
      <div class="stat bg-base-100 rounded-lg shadow">
        <div class="stat-title">الباصات النشطة</div>
        <div class="stat-value text-success">{{ summary?.activeBuses || 0 }}</div>
      </div>
      <div class="stat bg-base-100 rounded-lg shadow">
        <div class="stat-title">إجمالي السعة</div>
        <div class="stat-value">{{ summary?.totalCapacity || 0 }}</div>
      </div>
      <div class="stat bg-base-100 rounded-lg shadow">
        <div class="stat-title">إجمالي المسارات</div>
        <div class="stat-value text-info">{{ summary?.totalRoutes || 0 }}</div>
      </div>
    </div>

    <!-- Filters -->
    <div class="bg-base-100 p-4 rounded-lg shadow">
      <div class="flex flex-col md:flex-row gap-4">
        <div class="form-control flex-1">
          <input
            v-model="searchQuery"
            type="text"
            placeholder="بحث برقم الباص أو اسم السائق..."
            class="input input-bordered w-full"
            @input="debouncedSearch"
          />
        </div>
        <select v-model="selectedPeriod" class="select select-bordered" @change="filterBuses">
          <option :value="null">جميع الفترات</option>
          <option v-for="p in periods" :key="p.id" :value="p.id">{{ p.name }}</option>
        </select>
        <select v-model="selectedStatus" class="select select-bordered" @change="filterBuses">
          <option :value="null">جميع الحالات</option>
          <option :value="true">نشط</option>
          <option :value="false">غير نشط</option>
        </select>
      </div>
    </div>

    <!-- Period Tabs -->
    <div class="tabs tabs-boxed bg-base-100 p-2">
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

    <!-- Bus List -->
    <div v-if="loading" class="flex justify-center py-12">
      <span class="loading loading-spinner loading-lg text-primary"></span>
    </div>

    <div v-else-if="filteredBuses.length === 0" class="text-center py-12 text-base-content/60">
      لا توجد باصات
    </div>

    <div v-else class="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-4">
      <BusCard
        v-for="bus in filteredBuses"
        :key="bus.id || bus.busId"
        :bus="bus"
        @edit="editBus"
        @delete="confirmDelete"
        @view="viewBus"
      />
    </div>

    <!-- Add/Edit Modal -->
    <dialog :open="showAddModal || showEditModal" class="modal modal-open">
      <div class="modal-box max-w-2xl">
        <h3 class="font-bold text-lg mb-4">{{ showEditModal ? 'تعديل الباص' : 'إضافة باص جديد' }}</h3>
        <BusForm
          :bus="selectedBus"
          :periods="periods"
          @submit="saveBus"
          @cancel="closeModal"
        />
      </div>
      <div class="modal-backdrop" @click="closeModal"></div>
    </dialog>

    <!-- Delete Confirmation -->
    <dialog :open="showDeleteModal" class="modal modal-open">
      <div class="modal-box">
        <h3 class="font-bold text-lg">تأكيد الحذف</h3>
        <p class="py-4">هل أنت متأكد من حذف الباص رقم {{ busToDelete?.busNumber || busToDelete?.plateNumber || busToDelete?.licensePlate }}؟</p>
        <div class="modal-action">
          <button class="btn btn-ghost" @click="showDeleteModal = false">إلغاء</button>
          <button class="btn btn-error" @click="deleteBusConfirmed">حذف</button>
        </div>
      </div>
    </dialog>
  </div>
</template>

<script setup lang="ts">
import { ref, computed, onMounted } from 'vue'
import { useRouter } from 'vue-router'
import { Plus } from 'lucide-vue-next'
import { useBusStore, type Bus } from '@/stores/bus'
import BusCard from '@/components/buses/BusCard.vue'
import BusForm from '@/components/buses/BusForm.vue'

const router = useRouter()
const busStore = useBusStore()

const searchQuery = ref('')
const selectedPeriod = ref<number | null>(null)
const selectedStatus = ref<boolean | null>(null)
const activePeriodTab = ref(1)

const showAddModal = ref(false)
const showEditModal = ref(false)
const showDeleteModal = ref(false)
const selectedBus = ref<Bus | null>(null)
const busToDelete = ref<Bus | null>(null)

const loading = computed(() => busStore.loading)
const buses = computed(() => busStore.buses)
const busesByPeriod = computed(() => busStore.busesByPeriod)
const summary = computed(() => busStore.summary)

const periods = [
  { id: 1, name: 'الفترة الأولى' },
  { id: 2, name: 'الفترة الثانية' },
  { id: 3, name: 'الفترة الثالثة' },
  { id: 4, name: 'الفترة الرابعة' },
  { id: 5, name: 'الفترة الخامسة' }
]

const filteredBuses = computed(() => {
  let result = buses.value

  if (activePeriodTab.value) {
    result = result.filter(b => b.periodId === activePeriodTab.value)
  }

  if (searchQuery.value) {
    const query = searchQuery.value.toLowerCase()
    result = result.filter(b =>
      (b.busNumber || b.plateNumber || '').toLowerCase().includes(query) ||
      b.driverName?.toLowerCase().includes(query)
    )
  }

  if (selectedStatus.value !== null) {
    result = result.filter(b => b.isActive === selectedStatus.value)
  }

  return result
})

let searchTimeout: ReturnType<typeof setTimeout>
const debouncedSearch = () => {
  clearTimeout(searchTimeout)
  searchTimeout = setTimeout(() => {
    filterBuses()
  }, 300)
}

const filterBuses = () => {
  busStore.fetchBuses({
    periodId: selectedPeriod.value ?? undefined,
    isActive: selectedStatus.value ?? undefined,
    search: searchQuery.value || undefined
  })
}

const editBus = (bus: Bus) => {
  selectedBus.value = bus
  showEditModal.value = true
}

const viewBus = (bus: Bus) => {
  router.push({ name: 'BusDetail', params: { id: bus.id || bus.busId } })
}

const confirmDelete = (bus: Bus) => {
  busToDelete.value = bus
  showDeleteModal.value = true
}

const deleteBusConfirmed = async () => {
  if (busToDelete.value) {
    await busStore.deleteBus(busToDelete.value.id || busToDelete.value.busId!)
    showDeleteModal.value = false
    busToDelete.value = null
  }
}

const saveBus = async (busData: any) => {
  if (showEditModal.value && selectedBus.value) {
    await busStore.updateBus(selectedBus.value.id || selectedBus.value.busId!, busData)
  } else {
    await busStore.createBus(busData)
  }
  closeModal()
}

const closeModal = () => {
  showAddModal.value = false
  showEditModal.value = false
  selectedBus.value = null
}

onMounted(async () => {
  await busStore.fetchBuses()
  await busStore.fetchSummary()
})
</script>
