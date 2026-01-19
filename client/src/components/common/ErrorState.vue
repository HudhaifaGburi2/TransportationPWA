<template>
  <div class="text-center py-16">
    <div class="text-error/30 mb-4">
      <XCircle class="w-16 h-16 mx-auto" />
    </div>
    <p class="text-error text-lg">{{ message }}</p>
    <button v-if="onRetry" @click="onRetry" class="btn btn-primary btn-sm mt-4">
      إعادة المحاولة
    </button>
  </div>
</template>

<script setup lang="ts">
import { XCircle } from 'lucide-vue-next'

defineProps<{
  message: string
  onRetry?: () => void
}>()
</script>
