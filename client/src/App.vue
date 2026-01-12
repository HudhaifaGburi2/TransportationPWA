<script setup lang="ts">
import { useUiStore } from '@/stores/ui'

const uiStore = useUiStore()
</script>

<template>
  <div id="app">
    <router-view />
    
    <!-- Toast Notification -->
    <Teleport to="body">
      <Transition name="toast">
        <div
          v-if="uiStore.toast.show"
          class="fixed bottom-4 left-4 right-4 md:left-auto md:right-4 md:w-96 z-50"
        >
          <div
            :class="[
              'px-4 py-3 rounded-lg shadow-lg text-white flex items-center gap-3',
              {
                'bg-success': uiStore.toast.type === 'success',
                'bg-danger': uiStore.toast.type === 'error',
                'bg-warning': uiStore.toast.type === 'warning',
                'bg-info': uiStore.toast.type === 'info'
              }
            ]"
          >
            <span class="flex-1">{{ uiStore.toast.message }}</span>
            <button @click="uiStore.hideToast()" class="text-white/80 hover:text-white">
              ✕
            </button>
          </div>
        </div>
      </Transition>
    </Teleport>

    <!-- Loading Overlay -->
    <Teleport to="body">
      <Transition name="fade">
        <div
          v-if="uiStore.isLoading"
          class="fixed inset-0 bg-black/50 flex items-center justify-center z-50"
        >
          <div class="bg-white rounded-lg p-6 flex flex-col items-center gap-4">
            <div class="w-10 h-10 border-4 border-primary border-t-transparent rounded-full animate-spin"></div>
            <p class="text-gray-700">{{ uiStore.loadingMessage || 'جاري التحميل...' }}</p>
          </div>
        </div>
      </Transition>
    </Teleport>
  </div>
</template>

<style>
.toast-enter-active,
.toast-leave-active {
  transition: all 0.3s ease;
}

.toast-enter-from,
.toast-leave-to {
  opacity: 0;
  transform: translateY(20px);
}

.fade-enter-active,
.fade-leave-active {
  transition: opacity 0.2s ease;
}

.fade-enter-from,
.fade-leave-to {
  opacity: 0;
}
</style>
