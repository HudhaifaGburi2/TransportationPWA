<template>
  <div class="card bg-base-100 shadow-md hover:shadow-lg transition-shadow">
    <div class="card-body p-4">
      <!-- Header -->
      <div class="flex items-start justify-between">
        <div>
          <h3 class="card-title text-lg">{{ bus.busNumber || bus.plateNumber }}</h3>
          <p class="text-sm text-base-content/60">{{ bus.periodName }}</p>
        </div>
        <div class="dropdown dropdown-end">
          <label tabindex="0" class="btn btn-ghost btn-sm btn-circle">
            <MoreVertical class="w-4 h-4" />
          </label>
          <ul tabindex="0" class="dropdown-content menu p-2 shadow bg-base-100 rounded-box w-40">
            <li><a @click="$emit('view', bus)"><Eye class="w-4 h-4" /> عرض</a></li>
            <li><a @click="$emit('edit', bus)"><Pencil class="w-4 h-4" /> تعديل</a></li>
            <li><a @click="$emit('delete', bus)" class="text-error"><Trash2 class="w-4 h-4" /> حذف</a></li>
          </ul>
        </div>
      </div>

      <!-- Status Badge -->
      <div class="flex gap-2 my-2">
        <span :class="bus.isActive ? 'badge badge-success' : 'badge badge-error'" class="badge-sm">
          {{ bus.isActive ? 'نشط' : 'غير نشط' }}
        </span>
        <span v-if="bus.isMerged" class="badge badge-warning badge-sm">مدمج</span>
      </div>

      <!-- Driver Info -->
      <div v-if="bus.driverName" class="flex items-center gap-2 text-sm">
        <User class="w-4 h-4 text-base-content/60" />
        <span>{{ bus.driverName }}</span>
      </div>
      <div v-if="bus.driverPhoneNumber" class="flex items-center gap-2 text-sm">
        <Phone class="w-4 h-4 text-base-content/60" />
        <span dir="ltr">{{ bus.driverPhoneNumber }}</span>
      </div>

      <!-- Route -->
      <div v-if="bus.routeName" class="flex items-center gap-2 text-sm">
        <MapPin class="w-4 h-4 text-base-content/60" />
        <span>{{ bus.routeName }}</span>
      </div>

      <!-- Capacity Progress -->
      <div class="mt-3">
        <div class="flex justify-between text-sm mb-1">
          <span>السعة</span>
          <span>{{ bus.currentStudentCount }} / {{ bus.capacity }}</span>
        </div>
        <progress
          class="progress w-full"
          :class="utilizationClass"
          :value="bus.utilizationPercentage"
          max="100"
        ></progress>
        <div class="text-xs text-left mt-1" :class="utilizationTextClass">
          {{ bus.utilizationPercentage }}%
        </div>
      </div>

      <!-- Districts -->
      <div v-if="bus.districts?.length" class="mt-2">
        <div class="flex flex-wrap gap-1">
          <span v-for="d in bus.districts.slice(0, 3)" :key="d.districtId" class="badge badge-outline badge-sm">
            {{ d.districtNameAr }}
          </span>
          <span v-if="bus.districts.length > 3" class="badge badge-outline badge-sm">
            +{{ bus.districts.length - 3 }}
          </span>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { computed } from 'vue'
import { MoreVertical, Eye, Pencil, Trash2, User, Phone, MapPin } from 'lucide-vue-next'
import type { Bus } from '@/stores/bus'

const props = defineProps<{
  bus: Bus
}>()

defineEmits<{
  (e: 'view', bus: Bus): void
  (e: 'edit', bus: Bus): void
  (e: 'delete', bus: Bus): void
}>()

const utilizationClass = computed(() => {
  const pct = props.bus.utilizationPercentage || 0
  if (pct >= 90) return 'progress-error'
  if (pct >= 70) return 'progress-warning'
  return 'progress-success'
})

const utilizationTextClass = computed(() => {
  const pct = props.bus.utilizationPercentage || 0
  if (pct >= 90) return 'text-error'
  if (pct >= 70) return 'text-warning'
  return 'text-success'
})
</script>
