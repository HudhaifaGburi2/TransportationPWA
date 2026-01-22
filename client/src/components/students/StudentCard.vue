<template>
  <div class="card bg-base-100 shadow-sm hover:shadow-md transition-shadow cursor-pointer" @click="$emit('click', student)">
    <div class="card-body p-4">
      <div class="flex items-start justify-between">
        <div class="flex items-center gap-3">
          <div class="avatar placeholder">
            <div class="bg-primary/10 text-primary rounded-full w-12 h-12">
              <span class="text-lg font-bold">{{ initials }}</span>
            </div>
          </div>
          <div>
            <h3 class="font-semibold text-base-content">{{ student.studentName }}</h3>
            <p class="text-sm text-base-content/60">{{ student.studentId }}</p>
          </div>
        </div>
        <StudentStatusBadge :status="student.status" />
      </div>

      <div class="grid grid-cols-2 gap-2 mt-3 text-sm">
        <div v-if="student.districtName" class="flex items-center gap-1 text-base-content/70">
          <MapPin class="w-4 h-4" />
          <span>{{ student.districtName }}</span>
        </div>
        <div v-if="student.teacherName" class="flex items-center gap-1 text-base-content/70">
          <User class="w-4 h-4" />
          <span>{{ student.teacherName }}</span>
        </div>
        <div v-if="student.phoneNumber" class="flex items-center gap-1 text-base-content/70">
          <Phone class="w-4 h-4" />
          <span dir="ltr">{{ student.phoneNumber }}</span>
        </div>
        <div v-if="busNumber" class="flex items-center gap-1 text-base-content/70">
          <Bus class="w-4 h-4" />
          <span>باص {{ busNumber }}</span>
        </div>
      </div>

      <div v-if="showActions" class="card-actions justify-end mt-3 pt-3 border-t border-base-200">
        <slot name="actions">
          <button class="btn btn-ghost btn-sm" @click.stop="$emit('view', student)">
            <Eye class="w-4 h-4" />
            عرض
          </button>
        </slot>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { computed } from 'vue'
import { MapPin, User, Phone, Bus, Eye } from 'lucide-vue-next'
import StudentStatusBadge from './StudentStatusBadge.vue'

interface Student {
  id: string
  studentId: string
  studentName: string
  status: string
  districtName?: string
  teacherName?: string
  phoneNumber?: string
}

const props = withDefaults(defineProps<{
  student: Student
  busNumber?: string
  showActions?: boolean
}>(), {
  showActions: true
})

defineEmits<{
  click: [student: Student]
  view: [student: Student]
}>()

const initials = computed(() => {
  const name = props.student.studentName || ''
  return name.split(' ').map(n => n[0]).slice(0, 2).join('')
})
</script>
