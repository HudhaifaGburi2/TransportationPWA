<template>
  <div class="container mx-auto p-4 max-w-7xl">
    <!-- Header -->
    <div class="flex flex-col sm:flex-row justify-between items-start sm:items-center gap-4 mb-6">
      <div>
        <h1 class="text-2xl font-bold text-base-content">إدارة السائقين</h1>
        <p class="text-base-content/60 mt-1">سائقو الباصات المعينون للنقل</p>
      </div>
      <button @click="openAddModal" class="btn btn-primary gap-2">
        <UserPlus class="w-5 h-5" />
        إضافة سائق
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
                placeholder="البحث بالاسم أو رقم الهاتف..."
                class="input input-bordered w-full pr-10"
                @input="debouncedSearch"
              />
            </div>
          </div>
          <select v-model="statusFilter" class="select select-bordered w-full sm:w-48" @change="loadDrivers">
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
          <Users class="w-8 h-8" />
        </div>
        <div class="stat-title">إجمالي السائقين</div>
        <div class="stat-value text-primary">{{ drivers.length }}</div>
      </div>
      <div class="stat bg-base-100 rounded-box shadow-sm">
        <div class="stat-figure text-success">
          <UserCheck class="w-8 h-8" />
        </div>
        <div class="stat-title">السائقون النشطون</div>
        <div class="stat-value text-success">{{ activeDrivers.length }}</div>
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
      <button @click="loadDrivers" class="btn btn-sm btn-ghost">إعادة المحاولة</button>
    </div>

    <!-- Empty State -->
    <div v-else-if="drivers.length === 0" class="card bg-base-100 shadow-sm">
      <div class="card-body items-center text-center py-12">
        <Users class="w-16 h-16 text-base-content/20 mb-4" />
        <h3 class="text-lg font-semibold">لا يوجد سائقون</h3>
        <p class="text-base-content/60">ابدأ بإضافة السائقين للنظام</p>
        <button @click="openAddModal" class="btn btn-primary mt-4">
          <UserPlus class="w-5 h-5" />
          إضافة سائق
        </button>
      </div>
    </div>

    <!-- Drivers Table -->
    <div v-else class="card bg-base-100 shadow-sm overflow-hidden">
      <div class="overflow-x-auto">
        <table class="table table-zebra">
          <thead>
            <tr>
              <th>الاسم</th>
              <th>رقم الهاتف</th>
              <th>الحالة</th>
              <th>الإجراءات</th>
            </tr>
          </thead>
          <tbody>
            <tr v-for="driver in drivers" :key="driver.id">
              <td class="font-medium">{{ driver.fullName }}</td>
              <td dir="ltr" class="text-right">{{ formatPhone(driver.phoneNumber) }}</td>
              <td>
                <span :class="['badge', driver.isActive ? 'badge-success' : 'badge-ghost']">
                  {{ driver.isActive ? 'نشط' : 'غير نشط' }}
                </span>
              </td>
              <td>
                <div class="flex gap-1">
                  <button @click="openEditModal(driver)" class="btn btn-ghost btn-xs">
                    <Pencil class="w-4 h-4" />
                  </button>
                  <button @click="confirmDelete(driver)" class="btn btn-ghost btn-xs text-error">
                    <Trash2 class="w-4 h-4" />
                  </button>
                </div>
              </td>
            </tr>
          </tbody>
        </table>
      </div>
    </div>

    <!-- Add/Edit Modal -->
    <dialog ref="formModal" class="modal">
      <div class="modal-box w-11/12 max-w-lg">
        <h3 class="font-bold text-lg mb-4">{{ isEditing ? 'تعديل بيانات السائق' : 'إضافة سائق جديد' }}</h3>
        <form @submit.prevent="submitForm">
          <div class="form-control mb-4">
            <label class="label"><span class="label-text">الاسم الكامل *</span></label>
            <input v-model="form.fullName" type="text" class="input input-bordered" required />
          </div>
          <div class="form-control mb-4">
            <label class="label"><span class="label-text">رقم الهاتف *</span></label>
            <input v-model="form.phoneNumber" type="tel" dir="ltr" class="input input-bordered text-right" required />
          </div>
          <div v-if="isEditing" class="form-control mb-4">
            <label class="label cursor-pointer justify-start gap-3">
              <input v-model="form.isActive" type="checkbox" class="toggle toggle-success" />
              <span class="label-text">نشط</span>
            </label>
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
        <p class="py-4">هل أنت متأكد من حذف السائق <strong>{{ driverToDelete?.fullName }}</strong>؟</p>
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
import { useBusManagementStore, type Driver, type CreateDriverDto, type UpdateDriverDto } from '@/stores/busManagement'
import { storeToRefs } from 'pinia'
import { 
  Search, Users, UserPlus, UserCheck, AlertCircle, 
  Pencil, Trash2 
} from 'lucide-vue-next'

const store = useBusManagementStore()
const { drivers, loading, error } = storeToRefs(store)

const searchQuery = ref('')
const statusFilter = ref('')
const formModal = ref<HTMLDialogElement>()
const deleteModal = ref<HTMLDialogElement>()
const isEditing = ref(false)
const editingId = ref<string | null>(null)
const driverToDelete = ref<Driver | null>(null)

const form = ref<CreateDriverDto & { isActive?: boolean }>({
  fullName: '',
  phoneNumber: '',
  isActive: true
})

const activeDrivers = computed(() => drivers.value.filter((d: Driver) => d.isActive))

let searchTimeout: ReturnType<typeof setTimeout>
function debouncedSearch() {
  clearTimeout(searchTimeout)
  searchTimeout = setTimeout(() => loadDrivers(), 300)
}

async function loadDrivers() {
  await store.fetchDrivers({
    search: searchQuery.value || undefined,
    isActive: statusFilter.value ? statusFilter.value === 'true' : undefined
  })
}

function formatPhone(phone: string) {
  if (!phone) return ''
  return phone.startsWith('0') ? phone : `0${phone}`
}

function formatDate(dateStr: string) {
  if (!dateStr) return ''
  return new Date(dateStr).toLocaleDateString('ar-SA')
}

function openAddModal() {
  isEditing.value = false
  editingId.value = null
  form.value = {
    fullName: '',
    phoneNumber: '',
    isActive: true
  }
  formModal.value?.showModal()
}

function openEditModal(driver: Driver) {
  isEditing.value = true
  editingId.value = driver.id
  form.value = {
    fullName: driver.fullName,
    phoneNumber: driver.phoneNumber,
    isActive: driver.isActive
  }
  formModal.value?.showModal()
}

function closeModal() {
  formModal.value?.close()
}

async function submitForm() {
  if (isEditing.value && editingId.value) {
    const result = await store.updateDriver(editingId.value, form.value as UpdateDriverDto)
    if (result) closeModal()
  } else {
    const result = await store.createDriver(form.value)
    if (result) closeModal()
  }
}

function confirmDelete(driver: Driver) {
  driverToDelete.value = driver
  deleteModal.value?.showModal()
}

async function handleDelete() {
  if (driverToDelete.value) {
    const success = await store.deleteDriver(driverToDelete.value.id)
    if (success) {
      deleteModal.value?.close()
      driverToDelete.value = null
    }
  }
}

onMounted(() => {
  loadDrivers()
})
</script>
