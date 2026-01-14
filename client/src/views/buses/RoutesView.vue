<template>
  <div class="space-y-6">
    <!-- Header -->
    <div class="flex flex-col md:flex-row md:items-center md:justify-between gap-4">
      <h1 class="text-2xl font-bold text-base-content">إدارة المسارات</h1>
      <button @click="showAddModal = true" class="btn btn-primary gap-2">
        <Plus class="w-5 h-5" />
        إضافة مسار
      </button>
    </div>

    <!-- Routes List -->
    <div v-if="loading" class="flex justify-center py-12">
      <span class="loading loading-spinner loading-lg text-primary"></span>
    </div>

    <div v-else-if="routes.length === 0" class="text-center py-12 text-base-content/60">
      لا توجد مسارات
    </div>

    <div v-else class="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-4">
      <div v-for="route in routes" :key="route.routeId" class="card bg-base-100 shadow">
        <div class="card-body">
          <div class="flex items-start justify-between">
            <div>
              <h3 class="card-title">{{ route.routeName }}</h3>
              <p v-if="route.routeDescription" class="text-sm text-base-content/60 mt-1">
                {{ route.routeDescription }}
              </p>
            </div>
            <div class="dropdown dropdown-end">
              <label tabindex="0" class="btn btn-ghost btn-sm btn-circle">
                <MoreVertical class="w-4 h-4" />
              </label>
              <ul tabindex="0" class="dropdown-content menu p-2 shadow bg-base-100 rounded-box w-40">
                <li><a @click="editRoute(route)"><Pencil class="w-4 h-4" /> تعديل</a></li>
                <li><a @click="confirmDelete(route)" class="text-error"><Trash2 class="w-4 h-4" /> حذف</a></li>
              </ul>
            </div>
          </div>
          <div class="flex items-center gap-4 mt-3">
            <span :class="route.isActive ? 'badge badge-success' : 'badge badge-error'" class="badge-sm">
              {{ route.isActive ? 'نشط' : 'غير نشط' }}
            </span>
            <span class="text-sm text-base-content/60">
              <Bus class="w-4 h-4 inline" /> {{ route.busCount }} باص
            </span>
          </div>
        </div>
      </div>
    </div>

    <!-- Add/Edit Modal -->
    <dialog :open="showAddModal || showEditModal" class="modal modal-open">
      <div class="modal-box">
        <h3 class="font-bold text-lg mb-4">{{ showEditModal ? 'تعديل المسار' : 'إضافة مسار جديد' }}</h3>
        <form @submit.prevent="saveRoute" class="space-y-4">
          <div class="form-control">
            <label class="label"><span class="label-text">اسم المسار *</span></label>
            <input v-model="form.routeName" type="text" class="input input-bordered" required />
          </div>
          <div class="form-control">
            <label class="label"><span class="label-text">الوصف</span></label>
            <textarea v-model="form.routeDescription" class="textarea textarea-bordered" rows="3"></textarea>
          </div>
          <div v-if="showEditModal" class="form-control">
            <label class="label cursor-pointer justify-start gap-3">
              <input v-model="form.isActive" type="checkbox" class="toggle toggle-primary" />
              <span class="label-text">المسار نشط</span>
            </label>
          </div>
          <div class="modal-action">
            <button type="button" class="btn btn-ghost" @click="closeModal">إلغاء</button>
            <button type="submit" class="btn btn-primary">{{ showEditModal ? 'حفظ' : 'إضافة' }}</button>
          </div>
        </form>
      </div>
      <div class="modal-backdrop" @click="closeModal"></div>
    </dialog>

    <!-- Delete Confirmation -->
    <dialog :open="showDeleteModal" class="modal modal-open">
      <div class="modal-box">
        <h3 class="font-bold text-lg">تأكيد الحذف</h3>
        <p class="py-4">هل أنت متأكد من حذف المسار "{{ routeToDelete?.routeName }}"؟</p>
        <div class="modal-action">
          <button class="btn btn-ghost" @click="showDeleteModal = false">إلغاء</button>
          <button class="btn btn-error" @click="deleteRouteConfirmed">حذف</button>
        </div>
      </div>
    </dialog>
  </div>
</template>

<script setup lang="ts">
import { ref, reactive, onMounted } from 'vue'
import { Plus, MoreVertical, Pencil, Trash2, Bus } from 'lucide-vue-next'
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
    const response = await apiClient.get('/api/v1/routes')
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
  try {
    if (showEditModal.value && selectedRoute.value) {
      await apiClient.put(`/api/v1/routes/${selectedRoute.value.routeId}`, form)
    } else {
      await apiClient.post('/api/v1/routes', form)
    }
    await fetchRoutes()
    closeModal()
  } catch (e) {
    console.error('Error saving route:', e)
  }
}

const deleteRouteConfirmed = async () => {
  if (routeToDelete.value) {
    try {
      await apiClient.delete(`/api/v1/routes/${routeToDelete.value.routeId}`)
      await fetchRoutes()
    } catch (e) {
      console.error('Error deleting route:', e)
    }
    showDeleteModal.value = false
    routeToDelete.value = null
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
