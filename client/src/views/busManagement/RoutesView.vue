<template>
  <div class="container mx-auto p-4 max-w-7xl">
    <!-- Header -->
    <div class="flex flex-col sm:flex-row justify-between items-start sm:items-center gap-4 mb-6">
      <div>
        <h1 class="text-2xl font-bold text-base-content">إدارة المسارات</h1>
        <p class="text-base-content/60 mt-1">إدارة مسارات النقل ونقاط التجمع</p>
      </div>
      <button @click="openAddModal" class="btn btn-primary gap-2">
        <MapPinPlus class="w-5 h-5" />
        إضافة مسار
      </button>
    </div>

    <!-- Search & Filters -->
    <div class="card bg-base-100 shadow-sm mb-6">
      <div class="card-body p-4">
        <div class="flex flex-col sm:flex-row gap-4">
          <div class="flex-1">
            <div class="relative">
              <Search class="absolute right-3 top-1/2 -translate-y-1/2 w-5 h-5 text-base-content/40" />
              <input
                v-model="searchQuery"
                type="text"
                placeholder="البحث بالاسم أو الرمز أو الحي..."
                class="input input-bordered w-full pr-10"
                @input="debouncedSearch"
              />
            </div>
          </div>
          <select v-model="statusFilter" class="select select-bordered w-full sm:w-48" @change="loadRoutes">
            <option value="">جميع الحالات</option>
            <option value="true">نشط</option>
            <option value="false">غير نشط</option>
          </select>
        </div>
      </div>
    </div>

    <!-- Stats Cards -->
    <div class="grid grid-cols-1 sm:grid-cols-2 gap-4 mb-6">
      <div class="stat bg-base-100 rounded-box shadow-sm">
        <div class="stat-figure text-primary">
          <MapPin class="w-8 h-8" />
        </div>
        <div class="stat-title">إجمالي المسارات</div>
        <div class="stat-value text-primary">{{ routes.length }}</div>
      </div>
      <div class="stat bg-base-100 rounded-box shadow-sm">
        <div class="stat-figure text-success">
          <Navigation class="w-8 h-8" />
        </div>
        <div class="stat-title">المسارات النشطة</div>
        <div class="stat-value text-success">{{ activeRoutes.length }}</div>
      </div>
    </div>

    <!-- Loading State -->
    <div v-if="loading" class="flex justify-center py-12">
      <span class="loading loading-spinner loading-lg text-primary"></span>
    </div>

    <!-- Error State -->
    <div v-else-if="error" class="alert alert-error mb-6">
      <AlertCircle class="w-5 h-5" />
      <span>{{ error }}</span>
      <button @click="loadRoutes" class="btn btn-sm btn-ghost">إعادة المحاولة</button>
    </div>

    <!-- Empty State -->
    <div v-else-if="routes.length === 0" class="card bg-base-100 shadow-sm">
      <div class="card-body items-center text-center py-12">
        <MapPin class="w-16 h-16 text-base-content/20 mb-4" />
        <h3 class="text-lg font-semibold">لا يوجد مسارات</h3>
        <p class="text-base-content/60">ابدأ بإضافة المسارات للنظام</p>
        <button @click="openAddModal" class="btn btn-primary mt-4">
          <MapPinPlus class="w-5 h-5" />
          إضافة مسار
        </button>
      </div>
    </div>

    <!-- Routes Grid -->
    <div v-else class="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-4">
      <div v-for="route in routes" :key="route.id" class="card bg-base-100 shadow-sm">
        <div class="card-body p-4">
          <div class="flex justify-between items-start">
            <div>
              <h3 class="font-bold text-lg">{{ route.name }}</h3>
              <p v-if="route.description" class="text-sm text-base-content/60">{{ route.description }}</p>
            </div>
            <span :class="['badge', route.isActive ? 'badge-success' : 'badge-ghost']">
              {{ route.isActive ? 'نشط' : 'غير نشط' }}
            </span>
          </div>

          <div class="card-actions justify-end mt-4 pt-3 border-t border-base-200">
            <button @click="openEditModal(route)" class="btn btn-ghost btn-sm">
              <Pencil class="w-4 h-4" />
              تعديل
            </button>
            <button @click="confirmDelete(route)" class="btn btn-ghost btn-sm text-error">
              <Trash2 class="w-4 h-4" />
              حذف
            </button>
          </div>
        </div>
      </div>
    </div>

    <!-- Add/Edit Modal -->
    <dialog ref="formModal" class="modal">
      <div class="modal-box w-11/12 max-w-2xl">
        <h3 class="font-bold text-lg mb-4">{{ isEditing ? 'تعديل بيانات المسار' : 'إضافة مسار جديد' }}</h3>
        <form @submit.prevent="submitForm">
          <div class="grid grid-cols-1 gap-4">
            <div class="form-control">
              <label class="label"><span class="label-text">اسم المسار *</span></label>
              <input v-model="form.name" type="text" class="input input-bordered" required />
            </div>
            <div class="form-control">
              <label class="label"><span class="label-text">الوصف</span></label>
              <textarea v-model="form.description" class="textarea textarea-bordered" rows="3"></textarea>
            </div>
            <div v-if="isEditing" class="form-control">
              <label class="label"><span class="label-text">الحالة</span></label>
              <label class="label cursor-pointer justify-start gap-3">
                <input v-model="form.isActive" type="checkbox" class="toggle toggle-success" />
                <span class="label-text">نشط</span>
              </label>
            </div>
          </div>
          <div class="modal-action">
            <button type="button" class="btn btn-ghost" @click="closeModal">إلغاء</button>
            <button type="submit" class="btn btn-primary" :disabled="loading">
              <span v-if="loading" class="loading loading-spinner loading-sm"></span>
              {{ isEditing ? 'حفظ التعديلات' : 'إضافة' }}
            </button>
          </div>
        </form>
      </div>
      <form method="dialog" class="modal-backdrop"><button>close</button></form>
    </dialog>

    <!-- Delete Confirmation Modal -->
    <dialog ref="deleteModal" class="modal">
      <div class="modal-box">
        <h3 class="font-bold text-lg text-error">تأكيد الحذف</h3>
        <p class="py-4">هل أنت متأكد من حذف المسار <strong>{{ routeToDelete?.name }}</strong>؟</p>
        <div class="modal-action">
          <button class="btn btn-ghost" @click="deleteModal?.close()">إلغاء</button>
          <button class="btn btn-error" @click="handleDelete" :disabled="loading">
            <span v-if="loading" class="loading loading-spinner loading-sm"></span>
            حذف
          </button>
        </div>
      </div>
      <form method="dialog" class="modal-backdrop"><button>close</button></form>
    </dialog>
  </div>
</template>

<script setup lang="ts">
import { ref, onMounted, computed } from 'vue'
import { useBusManagementStore, type Route, type CreateRouteDto, type UpdateRouteDto } from '@/stores/busManagement'
import { storeToRefs } from 'pinia'
import { 
  Search, MapPin, MapPinPlus, Navigation, AlertCircle,
  Pencil, Trash2 
} from 'lucide-vue-next'

const store = useBusManagementStore()
const { routes, loading, error } = storeToRefs(store)

const searchQuery = ref('')
const statusFilter = ref('')
const formModal = ref<HTMLDialogElement>()
const deleteModal = ref<HTMLDialogElement>()
const isEditing = ref(false)
const editingId = ref<string | null>(null)
const routeToDelete = ref<Route | null>(null)

const form = ref<CreateRouteDto & { isActive?: boolean }>({
  name: '',
  description: '',
  isActive: true
})

const activeRoutes = computed(() => routes.value.filter(r => r.isActive))

let searchTimeout: ReturnType<typeof setTimeout>
function debouncedSearch() {
  clearTimeout(searchTimeout)
  searchTimeout = setTimeout(() => loadRoutes(), 300)
}

async function loadRoutes() {
  await store.fetchRoutes({
    search: searchQuery.value || undefined,
    isActive: statusFilter.value ? statusFilter.value === 'true' : undefined
  })
}

function openAddModal() {
  isEditing.value = false
  editingId.value = null
  form.value = {
    name: '',
    description: '',
    isActive: true
  }
  formModal.value?.showModal()
}

function openEditModal(route: Route) {
  isEditing.value = true
  editingId.value = route.id
  form.value = {
    name: route.name,
    description: route.description || '',
    isActive: route.isActive
  }
  formModal.value?.showModal()
}

function closeModal() {
  formModal.value?.close()
}

async function submitForm() {
  if (isEditing.value && editingId.value) {
    const result = await store.updateRoute(editingId.value, form.value as UpdateRouteDto)
    if (result) closeModal()
  } else {
    const result = await store.createRoute(form.value)
    if (result) closeModal()
  }
}

function confirmDelete(route: Route) {
  routeToDelete.value = route
  deleteModal.value?.showModal()
}

async function handleDelete() {
  if (routeToDelete.value) {
    const success = await store.deleteRoute(routeToDelete.value.id)
    if (success) {
      deleteModal.value?.close()
      routeToDelete.value = null
    }
  }
}

onMounted(() => {
  loadRoutes()
})
</script>
