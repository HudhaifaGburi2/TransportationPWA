<template>
  <div class="bg-gradient-to-l from-primary/10 to-transparent p-6 rounded-xl">
    <div class="flex flex-col md:flex-row md:items-center md:justify-between gap-4">
      <div>
        <h1 class="text-2xl font-bold text-base-content flex items-center gap-3">
          <div v-if="icon" class="p-2 bg-primary/20 rounded-lg">
            <component :is="icon" class="w-6 h-6 text-primary" />
          </div>
          {{ title }}
        </h1>
        <p v-if="subtitle" class="text-base-content/60 mt-1">{{ subtitle }}</p>
      </div>
      <div class="flex gap-2">
        <slot name="actions" />
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import type { Component } from 'vue'

defineProps<{
  title: string
  subtitle?: string
  icon?: Component
}>()
</script>
