<template>
  <div class="text-center py-16">
    <div class="text-base-content/30 mb-4">
      <component :is="icon" class="w-16 h-16 mx-auto" />
    </div>
    <p class="text-base-content/60 text-lg">{{ title }}</p>
    <p v-if="subtitle" class="text-base-content/40 text-sm mt-1">{{ subtitle }}</p>
    <slot name="action" />
  </div>
</template>

<script setup lang="ts">
import type { Component } from 'vue'
import { FileText } from 'lucide-vue-next'

withDefaults(defineProps<{
  title: string
  subtitle?: string
  icon?: Component
}>(), {
  icon: FileText as any
})
</script>
