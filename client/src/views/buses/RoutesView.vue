<template>
  <div class="p-6 space-y-6">
    <!-- Page Header -->
    <div class="bg-gradient-to-l from-primary/10 to-transparent p-6 rounded-xl">
      <div class="flex flex-col md:flex-row md:items-center md:justify-between gap-4">
        <div>
          <h1 class="text-2xl font-bold text-base-content flex items-center gap-3">
            <div class="p-2 bg-primary/20 rounded-lg">
              <RouteIcon class="w-6 h-6 text-primary" />
            </div>
            إدارة المسارات
          </h1>
          <p class="text-base-content/60 mt-1">إضافة وتعديل مسارات الباصات</p>
        </div>
        <button @click="showAddModal = true" class="btn btn-primary gap-2 shadow-lg">
          <Plus class="w-5 h-5" />
          إضافة مسار جديد
        </button>
      </div>
    </div>

    <!-- Stats Summary -->
    <div class="grid grid-cols-2 md:grid-cols-4 gap-4">
      <div class="stat bg-base-100 rounded-xl shadow-sm border border-base-200">
        <div class="stat-figure text-primary">
          <RouteIcon class="w-8 h-8" />
        </div>
        <div class="stat-title">إجمالي المسارات</div>
        <div class="stat-value text-primary">{{ routes.length }}</div>
      </div>
      <div class="stat bg-base-100 rounded-xl shadow-sm border border-base-200">
        <div class="stat-figure text-success">
          <CheckCircle class="w-8 h-8" />
        </div>
        <div class="stat-title">المسارات النشطة</div>
        <div class="stat-value text-success">{{ activeRoutesCount }}</div>
      </div>
      <div class="stat bg-base-100 rounded-xl shadow-sm border border-base-200">
        <div class="stat-figure text-error">
          <XCircle class="w-8 h-8" />
        </div>
        <div class="stat-title">غير نشطة</div>
        <div class="stat-value text-error">{{ routes.length - activeRoutesCount }}</div>
      </div>
      <div class="stat bg-base-100 rounded-xl shadow-sm border border-base-200">
        <div class="stat-figure text-info">
          <Bus class="w-8 h-8" />
        </div>
        <div class="stat-title">إجمالي الباصات</div>
        <div class="stat-value text-info">{{ totalBusCount }}</div>
      </div>
    </div>

    <!-- Search & Filter -->
    <div class="bg-base-100 p-4 rounded-xl shadow-sm border border-base-200">
      <div class="flex flex-col md:flex-row gap-4">
        <div class="form-control flex-1">
          <div class="input-group">
            <span class="bg-base-200">
              <Search class="w-5 h-5 text-base-content/60" />
            </span>
            <input
              v-model="searchQuery"
              type="text"
              placeholder="بحث باسم المسار..."
              class="input input-bordered flex-1"
            />
          </div>
        </div>
        <select v-model="filterStatus" class="select select-bordered w-full md:w-48">
          <option value="all">جميع الحالات</option>
          <option value="active">نشط فقط</option>
          <option value="inactive">غير نشط فقط</option>
        </select>
      </div>
    </div>

    <!-- Routes Table -->
    <div class="bg-base-100 rounded-xl shadow-sm border border-base-200 overflow-hidden">
      <div v-if="loading" class="flex justify-center py-16">
        <span class="loading loading-spinner loading-lg text-primary"></span>
      </div>

      <div v-else-if="filteredRoutes.length === 0" class="text-center py-16">
        <div class="text-base-content/30 mb-4">
          <RouteIcon class="w-16 h-16 mx-auto" />
        </div>
        <p class="text-base-content/60 text-lg">لا توجد مسارات</p>
        <p class="text-base-content/40 text-sm mt-1">ابدأ بإضافة مسار جديد</p>
      </div>

      <div v-else class="overflow-x-auto">
        <table class="table table-zebra">
          <thead class="bg-base-200">
            <tr>
              <th class="font-bold">#</th>
              <th class="font-bold">اسم المسار</th>
              <th class="font-bold">الوصف</th>
              <th class="font-bold text-center">عدد الباصات</th>
              <th class="font-bold text-center">الحالة</th>
              <th class="font-bold text-center">الإجراءات</th>
            </tr>
          </thead>
          <tbody>
            <tr v-for="(route, index) in filteredRoutes" :key="route.routeId" class="hover">
              <td class="font-medium">{{ index + 1 }}</td>
              <td>
                <div class="flex items-center gap-3">
                  <div class="p-2 bg-primary/10 rounded-lg">
                    <RouteIcon class="w-5 h-5 text-primary" />
                  </div>
                  <span class="font-semibold">{{ route.routeName }}</span>
                </div>
              </td>
              <td class="text-base-content/70 max-w-xs truncate">
                {{ route.routeDescription || '-' }}
              </td>
              <td class="text-center">
                <div class="badge badge-info gap-1">
                  <Bus class="w-3 h-3" />
                  {{ route.busCount }}
                </div>
              </td>
              <td class="text-center">
                <span :class="route.isActive ? 'badge badge-success' : 'badge badge-error'">
                  {{ route.isActive ? 'نشط' : 'غير نشط' }}
                </span>
              </td>
              <td class="text-center">
                <div class="flex items-center justify-center gap-1">
                  <button @click="editRoute(route)" class="btn btn-ghost btn-sm btn-square tooltip" data-tip="تعديل">
                    <Pencil class="w-4 h-4 text-info" />
                  </button>
                  <button @click="confirmDelete(route)" class="btn btn-ghost btn-sm btn-square tooltip" data-tip="حذف">
                    <Trash2 class="w-4 h-4 text-error" />
                  </button>
                </div>
              </td>
            </tr>
          </tbody>
        </table>
      </div>
    </div>

    <!-- Add/Edit Modal -->
    <dialog :open="showAddModal || showEditModal" class="modal modal-open">
      <div class="modal-box max-w-lg">
        <button @click="closeModal" class="btn btn-sm btn-circle btn-ghost absolute left-2 top-2">✕</button>
        <div class="text-center mb-6">
          <div class="w-16 h-16 bg-primary/10 rounded-full flex items-center justify-center mx-auto mb-3">
            <RouteIcon class="w-8 h-8 text-primary" />
          </div>
          <h3 class="font-bold text-xl">{{ showEditModal ? 'تعديل المسار' : 'إضافة مسار جديد' }}</h3>
          <p class="text-base-content/60 text-sm mt-1">{{ showEditModal ? 'قم بتعديل بيانات المسار' : 'أدخل بيانات المسار الجديد' }}</p>
        </div>
        <form @submit.prevent="saveRoute" class="space-y-5">
          <div class="form-control">
            <label class="label">
              <span class="label-text font-medium">اسم المسار <span class="text-error">*</span></span>
            </label>
            <div class="input-group">
              <span class="bg-base-200">
                <RouteIcon class="w-5 h-5 text-base-content/60" />
              </span>
              <input 
                v-model="form.routeName" 
                type="text" 
                class="input input-bordered flex-1" 
                placeholder="مثال: المسار الشمالي"
                required 
              />
            </div>
          </div>
          <div class="form-control">
            <label class="label">
              <span class="label-text font-medium">الوصف</span>
              <span class="label-text-alt text-base-content/50">اختياري</span>
            </label>
            <textarea 
              v-model="form.routeDescription" 
              class="textarea textarea-bordered h-24" 
              placeholder="وصف المسار والمناطق التي يغطيها..."
            ></textarea>
          </div>
          <div v-if="showEditModal" class="form-control bg-base-200/50 p-4 rounded-lg">
            <label class="label cursor-pointer justify-between">
              <div>
                <span class="label-text font-medium">حالة المسار</span>
                <p class="text-xs text-base-content/60 mt-0.5">تفعيل أو تعطيل المسار</p>
              </div>
              <input v-model="form.isActive" type="checkbox" class="toggle toggle-primary toggle-lg" />
            </label>
          </div>
          <div class="divider"></div>
          <div class="flex gap-3">
            <button type="button" class="btn btn-ghost flex-1" @click="closeModal">إلغاء</button>
            <button type="submit" class="btn btn-primary flex-1 gap-2" :disabled="saving">
              <span v-if="saving" class="loading loading-spinner loading-sm"></span>
              <Save v-else class="w-4 h-4" />
              {{ showEditModal ? 'حفظ التغييرات' : 'إضافة المسار' }}
            </button>
          </div>
        </form>
      </div>
      <div class="modal-backdrop bg-black/50" @click="closeModal"></div>
    </dialog>

    <!-- Delete Confirmation -->
    <dialog :open="showDeleteModal" class="modal modal-open">
      <div class="modal-box max-w-sm text-center">
        <div class="w-16 h-16 bg-error/10 rounded-full flex items-center justify-center mx-auto mb-4">
          <AlertTriangle class="w-8 h-8 text-error" />
        </div>
        <h3 class="font-bold text-xl">تأكيد الحذف</h3>
        <p class="py-4 text-base-content/70">
          هل أنت متأكد من حذف المسار<br/>
          <span class="font-bold text-base-content">"{{ routeToDelete?.routeName }}"</span>؟
        </p>
        <p class="text-sm text-warning bg-warning/10 p-3 rounded-lg mb-4">
          ⚠️ لا يمكن التراجع عن هذا الإجراء
        </p>
        <div class="flex gap-3">
          <button class="btn btn-ghost flex-1" @click="showDeleteModal = false">إلغاء</button>
          <button class="btn btn-error flex-1 gap-2" @click="deleteRouteConfirmed" :disabled="deleting">
            <span v-if="deleting" class="loading loading-spinner loading-sm"></span>
            <Trash2 v-else class="w-4 h-4" />
            حذف
          </button>
        </div>
      </div>
      <div class="modal-backdrop bg-black/50" @click="showDeleteModal = false"></div>
    </dialog>
  </div>
</template>

<script setup lang="ts">
import { ref, reactive, computed, onMounted } from 'vue'
import { Plus, Pencil, Trash2, Bus, Search, Save, CheckCircle, XCircle, AlertTriangle, Route as RouteIcon } from 'lucide-vue-next'
import apiClient from '@/services/api/axios.config'

interface Route {
  routeId: string
  routeName: string
  routeDescription?: string
  isActive: boolean
  busCount: number
}

const routes = ref<Route[]>([])
const loading = ref(false)
const saving = ref(false)
const deleting = ref(false)
const searchQuery = ref('')
const filterStatus = ref('all')

const activeRoutesCount = computed(() => routes.value.filter(r => r.isActive).length)
const totalBusCount = computed(() => routes.value.reduce((sum, r) => sum + r.busCount, 0))

const filteredRoutes = computed(() => {
  let result = routes.value
  
  if (searchQuery.value) {
    const query = searchQuery.value.toLowerCase()
    result = result.filter(r => r.routeName.toLowerCase().includes(query))
  }
  
  if (filterStatus.value === 'active') {
    result = result.filter(r => r.isActive)
  } else if (filterStatus.value === 'inactive') {
    result = result.filter(r => !r.isActive)
  }
  
  return result
})

const showAddModal = ref(false)
const showEditModal = ref(false)
const showDeleteModal = ref(false)
const selectedRoute = ref<Route | null>(null)
const routeToDelete = ref<Route | null>(null)

const form = reactive({
  routeName: '',
  routeDescription: '',
  isActive: true
})

const fetchRoutes = async () => {
  loading.value = true
  try {
    const response = await apiClient.get('/busmanagement/routes')
    if (response.data.success) {
      routes.value = response.data.data
    }
  } catch (e) {
    console.error('Error fetching routes:', e)
  } finally {
    loading.value = false
  }
}

const editRoute = (route: Route) => {
  selectedRoute.value = route
  form.routeName = route.routeName
  form.routeDescription = route.routeDescription || ''
  form.isActive = route.isActive
  showEditModal.value = true
}

const confirmDelete = (route: Route) => {
  routeToDelete.value = route
  showDeleteModal.value = true
}

const saveRoute = async () => {
  saving.value = true
  try {
    if (showEditModal.value && selectedRoute.value) {
      await apiClient.put(`/busmanagement/routes/${selectedRoute.value.routeId}`, form)
    } else {
      await apiClient.post('/busmanagement/routes', form)
    }
    await fetchRoutes()
    closeModal()
  } catch (e) {
    console.error('Error saving route:', e)
  } finally {
    saving.value = false
  }
}

const deleteRouteConfirmed = async () => {
  if (routeToDelete.value) {
    deleting.value = true
    try {
      await apiClient.delete(`/busmanagement/routes/${routeToDelete.value.routeId}`)
      await fetchRoutes()
    } catch (e) {
      console.error('Error deleting route:', e)
    } finally {
      deleting.value = false
      showDeleteModal.value = false
      routeToDelete.value = null
    }
  }
}

const closeModal = () => {
  showAddModal.value = false
  showEditModal.value = false
  selectedRoute.value = null
  form.routeName = ''
  form.routeDescription = ''
  form.isActive = true
}

onMounted(fetchRoutes)
</script>
