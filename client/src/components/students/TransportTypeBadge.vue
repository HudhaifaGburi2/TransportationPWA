<template>
  <div class="flex items-center gap-1">
    <span :class="badgeClass" class="badge badge-sm gap-1">
      <component :is="icon" class="w-3 h-3" />
      {{ label }}
    </span>
    <div v-if="isSplit" class="flex items-center gap-1 text-xs text-base-content/60">
      <span v-if="arrivalBus" class="flex items-center gap-0.5">
        <ArrowDown class="w-3 h-3 text-success" />
        {{ arrivalBus }}
      </span>
      <span v-if="returnBus" class="flex items-center gap-0.5">
        <ArrowUp class="w-3 h-3 text-info" />
        {{ returnBus }}
      </span>
    </div>
  </div>
</template>

<script setup lang="ts">
import { computed } from 'vue'
import { ArrowDown, ArrowUp, ArrowLeftRight } from 'lucide-vue-next'

const props = defineProps<{
  transportType: string
  arrivalBus?: string
  returnBus?: string
}>()

const typeConfig = {
  arrival: { label: 'ذهاب فقط', class: 'badge-success', icon: ArrowDown },
  return: { label: 'عودة فقط', class: 'badge-info', icon: ArrowUp },
  both: { label: 'ذهاب وعودة', class: 'badge-primary', icon: ArrowLeftRight }
}

const normalizedType = computed(() => props.transportType?.toLowerCase() || 'both')
const config = computed(() => typeConfig[normalizedType.value as keyof typeof typeConfig] || typeConfig.both)

const badgeClass = computed(() => config.value.class)
const label = computed(() => config.value.label)
const icon = computed(() => config.value.icon)
const isSplit = computed(() => normalizedType.value !== 'both' && (props.arrivalBus || props.returnBus))
</script>
