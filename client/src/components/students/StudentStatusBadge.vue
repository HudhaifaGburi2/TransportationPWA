<template>
  <span :class="badgeClass" class="badge gap-1">
    <component :is="icon" class="w-3 h-3" />
    {{ statusText }}
  </span>
</template>

<script setup lang="ts">
import { computed } from 'vue'
import { CheckCircle, XCircle, Clock, UserX } from 'lucide-vue-next'

const props = defineProps<{
  status: string
}>()

const statusConfig = {
  active: { text: 'نشط', class: 'badge-success', icon: CheckCircle },
  suspended: { text: 'موقوف', class: 'badge-error', icon: XCircle },
  onleave: { text: 'في إجازة', class: 'badge-warning', icon: Clock },
  inactive: { text: 'غير نشط', class: 'badge-ghost', icon: UserX }
}

const normalizedStatus = computed(() => props.status?.toLowerCase().replace(/\s/g, '') || 'active')

const config = computed(() => statusConfig[normalizedStatus.value as keyof typeof statusConfig] || statusConfig.active)

const badgeClass = computed(() => config.value.class)
const statusText = computed(() => config.value.text)
const icon = computed(() => config.value.icon)
</script>
