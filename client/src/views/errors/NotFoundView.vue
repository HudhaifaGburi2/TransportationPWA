<script setup lang="ts">
import { useRouter } from 'vue-router'
import { Home } from 'lucide-vue-next'

const router = useRouter()
</script>

<template>
  <div class="min-h-screen bg-background flex items-center justify-center p-4">
    <div class="text-center">
      <h1 class="text-8xl font-bold text-primary mb-4">404</h1>
      <h2 class="text-2xl font-bold text-gray-800 mb-2">الصفحة غير موجودة</h2>
      <p class="text-neutral mb-8">عذراً، الصفحة التي تبحث عنها غير موجودة</p>
      <button
        @click="router.push('/')"
        class="btn btn-primary inline-flex items-center gap-2"
      >
        <Home class="w-5 h-5" />
        <span>العودة للرئيسية</span>
      </button>
    </div>
  </div>
</template>
