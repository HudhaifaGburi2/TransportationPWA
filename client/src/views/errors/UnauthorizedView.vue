<template>
  <div class="min-h-screen flex items-center justify-center bg-gray-100 px-4">
    <div class="max-w-md w-full text-center">
      <div class="bg-white rounded-lg shadow-lg p-8">
        <div class="w-16 h-16 mx-auto mb-4 bg-red-100 rounded-full flex items-center justify-center">
          <svg class="w-8 h-8 text-red-600" fill="none" stroke="currentColor" viewBox="0 0 24 24">
            <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M12 9v2m0 4h.01m-6.938 4h13.856c1.54 0 2.502-1.667 1.732-3L13.732 4c-.77-1.333-2.694-1.333-3.464 0L3.34 16c-.77 1.333.192 3 1.732 3z" />
          </svg>
        </div>
        <h1 class="text-2xl font-bold text-gray-800 mb-2">غير مصرح</h1>
        <p class="text-gray-600 mb-6">عذراً، ليس لديك صلاحية للوصول إلى هذه الصفحة.</p>
        <div class="space-y-3">
          <BaseButton to="/" block>
            <template #icon>
              <Home />
            </template>
            العودة للرئيسية
          </BaseButton>
          <BaseButton @click="logout" variant="outline" block>
            <template #icon>
              <LogOut />
            </template>
            تسجيل الخروج
          </BaseButton>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { useAuthStore } from '@/stores/auth'
import { Home, LogOut } from 'lucide-vue-next'
import BaseButton from '@/components/ui/BaseButton.vue'

const authStore = useAuthStore()

const logout = () => {
  authStore.logout()
}
</script>
