<template>
  <form @submit.prevent="handleSubmit" class="space-y-4">
    <div class="grid grid-cols-1 md:grid-cols-2 gap-4">
      <!-- Plate Number -->
      <div class="form-control">
        <label class="label"><span class="label-text">رقم اللوحة *</span></label>
        <input
          v-model="form.plateNumber"
          type="text"
          class="input input-bordered"
          :class="{ 'input-error': errors.plateNumber }"
          required
        />
        <label v-if="errors.plateNumber" class="label">
          <span class="label-text-alt text-error">{{ errors.plateNumber }}</span>
        </label>
      </div>

      <!-- Period -->
      <div class="form-control">
        <label class="label"><span class="label-text">الفترة *</span></label>
        <select v-model="form.periodId" class="select select-bordered" required>
          <option :value="0" disabled>اختر الفترة</option>
          <option v-for="p in periods" :key="p.id" :value="p.id">{{ p.name }}</option>
        </select>
      </div>

      <!-- Driver Name -->
      <div class="form-control">
        <label class="label"><span class="label-text">اسم السائق</span></label>
        <input v-model="form.driverName" type="text" class="input input-bordered" />
      </div>

      <!-- Driver Phone -->
      <div class="form-control">
        <label class="label"><span class="label-text">رقم هاتف السائق</span></label>
        <input v-model="form.driverPhoneNumber" type="tel" class="input input-bordered" dir="ltr" />
      </div>

      <!-- Capacity -->
      <div class="form-control">
        <label class="label"><span class="label-text">السعة *</span></label>
        <input
          v-model.number="form.capacity"
          type="number"
          min="1"
          max="100"
          class="input input-bordered"
          required
        />
      </div>

      <!-- Route -->
      <div class="form-control">
        <label class="label"><span class="label-text">المسار</span></label>
        <select v-model="form.routeId" class="select select-bordered">
          <option :value="undefined">بدون مسار</option>
          <!-- Routes would be loaded from API -->
        </select>
      </div>
    </div>

    <!-- Active Status (Edit only) -->
    <div v-if="bus" class="form-control">
      <label class="label cursor-pointer justify-start gap-3">
        <input v-model="form.isActive" type="checkbox" class="toggle toggle-primary" />
        <span class="label-text">الباص نشط</span>
      </label>
    </div>

    <!-- Actions -->
    <div class="flex justify-end gap-2 pt-4">
      <button type="button" class="btn btn-ghost" @click="$emit('cancel')">إلغاء</button>
      <button type="submit" class="btn btn-primary" :disabled="submitting">
        <span v-if="submitting" class="loading loading-spinner loading-sm"></span>
        {{ bus ? 'حفظ التغييرات' : 'إضافة' }}
      </button>
    </div>
  </form>
</template>

<script setup lang="ts">
import { ref, reactive, watch } from 'vue'
import type { Bus, CreateBusDto, UpdateBusDto } from '@/stores/bus'

interface Period {
  id: number
  name: string
}

const props = defineProps<{
  bus?: Bus | null
  periods: Period[]
}>()

const emit = defineEmits<{
  (e: 'submit', data: CreateBusDto | UpdateBusDto): void
  (e: 'cancel'): void
}>()

const submitting = ref(false)
const errors = reactive<Record<string, string>>({})

const form = reactive({
  plateNumber: '',
  periodId: 0,
  driverName: '',
  driverPhoneNumber: '',
  capacity: 30,
  routeId: undefined as string | undefined,
  districtIds: [] as string[],
  isActive: true
})

watch(() => props.bus, (newBus) => {
  if (newBus) {
    form.plateNumber = newBus.plateNumber
    form.periodId = newBus.periodId
    form.driverName = newBus.driverName || ''
    form.driverPhoneNumber = newBus.driverPhoneNumber || ''
    form.capacity = newBus.capacity
    form.routeId = newBus.routeId
    form.isActive = newBus.isActive
    form.districtIds = newBus.districts?.map(d => d.districtId) || []
  } else {
    resetForm()
  }
}, { immediate: true })

const resetForm = () => {
  form.plateNumber = ''
  form.periodId = 0
  form.driverName = ''
  form.driverPhoneNumber = ''
  form.capacity = 30
  form.routeId = undefined
  form.districtIds = []
  form.isActive = true
}

const validate = (): boolean => {
  Object.keys(errors).forEach(key => delete errors[key])

  if (!form.plateNumber.trim()) {
    errors.plateNumber = 'رقم اللوحة مطلوب'
  }

  if (!form.periodId || form.periodId === 0) {
    errors.periodId = 'الفترة مطلوبة'
  }

  if (form.capacity < 1 || form.capacity > 100) {
    errors.capacity = 'السعة يجب أن تكون بين 1 و 100'
  }

  return Object.keys(errors).length === 0
}

const handleSubmit = () => {
  if (!validate()) return

  submitting.value = true

  const data: CreateBusDto | UpdateBusDto = {
    plateNumber: form.plateNumber,
    periodId: form.periodId,
    driverName: form.driverName || undefined,
    driverPhoneNumber: form.driverPhoneNumber || undefined,
    capacity: form.capacity,
    routeId: form.routeId,
    districtIds: form.districtIds,
    ...(props.bus ? { isActive: form.isActive } : {})
  }

  emit('submit', data)
  submitting.value = false
}
</script>
