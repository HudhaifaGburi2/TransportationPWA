<template>
  <div class="flex min-h-screen bg-base-200" data-theme="tums">
    <!-- Sidebar -->
    <Sidebar v-if="!isMobile" />
    
    <!-- Mobile Drawer -->
    <div v-if="isMobile" class="drawer">
      <input id="mobile-drawer" type="checkbox" class="drawer-toggle" v-model="drawerOpen" />
      <div class="drawer-side z-50">
        <label for="mobile-drawer" class="drawer-overlay"></label>
        <Sidebar @click="drawerOpen = false" />
      </div>
    </div>

    <!-- Main Content -->
    <div class="flex-1 flex flex-col">
      <!-- Header -->
      <header class="bg-base-100 shadow-sm sticky top-0 z-40">
        <div class="flex items-center justify-between px-4 py-3">
          <div class="flex items-center gap-3">
            <button 
              v-if="isMobile"
              @click="drawerOpen = true"
              class="btn btn-ghost btn-square"
            >
              <Menu class="w-6 h-6" />
            </button>
            <h2 class="text-lg font-semibold text-base-content">{{ pageTitle }}</h2>
          </div>
          
          <div class="flex items-center gap-2">
            <!-- Sync Status -->
            <div v-if="hasPendingSync" class="badge badge-warning gap-1">
              <RefreshCw class="w-3 h-3 animate-spin" />
              مزامنة
            </div>
            
            <!-- Online/Offline Status -->
            <div :class="isOnline ? 'badge badge-success' : 'badge badge-error'" class="gap-1">
              <Wifi v-if="isOnline" class="w-3 h-3" />
              <WifiOff v-else class="w-3 h-3" />
              {{ isOnline ? 'متصل' : 'غير متصل' }}
            </div>
          </div>
        </div>
      </header>

      <!-- Page Content -->
      <main class="flex-1 p-4 md:p-6">
        <slot />
      </main>

      <!-- Footer -->
      <footer class="bg-base-100 border-t border-base-300 py-3 px-4 text-center text-sm text-base-content/60">
        نظام إدارة وحدة النقل © {{ currentYear }}
      </footer>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, computed, onMounted, onUnmounted } from 'vue'
import { useRoute } from 'vue-router'
import { Menu, Wifi, WifiOff, RefreshCw } from 'lucide-vue-next'
import Sidebar from './Sidebar.vue'

const route = useRoute()
const drawerOpen = ref(false)
const isMobile = ref(false)
const isOnline = ref(navigator.onLine)
const hasPendingSync = ref(false)

const pageTitle = computed(() => {
  const titles: Record<string, string> = {
    '/': 'لوحة التحكم',
    '/districts': 'إدارة المناطق',
    '/locations': 'إدارة المواقع',
    '/registration': 'التسجيل في النقل',
    '/my-registration': 'طلب التسجيل'
  }
  return titles[route.path] || 'نظام النقل'
})

const currentYear = computed(() => new Date().getFullYear())

const checkMobile = () => {
  isMobile.value = window.innerWidth < 1024
}

const handleOnline = () => { isOnline.value = true }
const handleOffline = () => { isOnline.value = false }

onMounted(() => {
  checkMobile()
  window.addEventListener('resize', checkMobile)
  window.addEventListener('online', handleOnline)
  window.addEventListener('offline', handleOffline)
})

onUnmounted(() => {
  window.removeEventListener('resize', checkMobile)
  window.removeEventListener('online', handleOnline)
  window.removeEventListener('offline', handleOffline)
})
</script>
