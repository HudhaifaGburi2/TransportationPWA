<script setup lang="ts">
import { ref, computed } from 'vue'
import { useRouter, useRoute } from 'vue-router'
import { useAuthStore } from '@/stores/auth'
import { Eye, EyeOff, Loader2 } from 'lucide-vue-next'

const router = useRouter()
const route = useRoute()
const authStore = useAuthStore()

const username = ref('')
const password = ref('')
const rememberMe = ref(false)
const showPassword = ref(false)

const isLoading = computed(() => authStore.isLoading)
const error = computed(() => authStore.error)

const isFormValid = computed(() => {
  return username.value.trim().length > 0 && password.value.length > 0
})

async function handleLogin() {
  if (!isFormValid.value) return

  const success = await authStore.login({
    username: username.value,
    password: password.value
  })

  if (success) {
    const redirect = route.query.redirect as string || '/'
    router.push(redirect)
  }
}

function togglePassword() {
  showPassword.value = !showPassword.value
}
</script>

<template>
  <div class="min-h-screen bg-background flex items-center justify-center p-4">
    <div class="w-full max-w-md">
      <!-- Login Card -->
      <div class="bg-white rounded-xl shadow-lg p-8 border border-border">
        <!-- Logo & Title -->
        <div class="text-center mb-8">
          <div class="w-20 h-20 bg-primary rounded-full flex items-center justify-center mx-auto mb-4">
            <svg class="w-10 h-10 text-white" fill="currentColor" viewBox="0 0 24 24">
              <path d="M17 20H7V10L12 5L17 10V20ZM12 8.5L9 11.5V18H15V11.5L12 8.5Z"/>
            </svg>
          </div>
          <h1 class="text-2xl font-bold text-gray-800 font-cairo mb-2">
            نظام إدارة وحدة النقل
          </h1>
          <p class="text-neutral text-sm">
            تسجيل الدخول للمتابعة
          </p>
        </div>

        <!-- Error Message -->
        <div v-if="error" class="mb-6 p-4 bg-red-50 border border-danger rounded-lg">
          <p class="text-danger text-sm text-center">{{ error }}</p>
        </div>

        <!-- Login Form -->
        <form @submit.prevent="handleLogin" class="space-y-6">
          <!-- Username Field -->
          <div>
            <label for="username" class="block text-sm font-medium text-gray-700 mb-2">
              اسم المستخدم
            </label>
            <input
              id="username"
              v-model="username"
              type="text"
              autocomplete="username"
              required
              class="input"
              :class="{ 'input-error': error }"
              placeholder="أدخل اسم المستخدم"
            />
          </div>

          <!-- Password Field -->
          <div>
            <label for="password" class="block text-sm font-medium text-gray-700 mb-2">
              كلمة المرور
            </label>
            <div class="relative">
              <input
                id="password"
                v-model="password"
                :type="showPassword ? 'text' : 'password'"
                autocomplete="current-password"
                required
                class="input pl-12"
                :class="{ 'input-error': error }"
                placeholder="أدخل كلمة المرور"
              />
              <button
                type="button"
                @click="togglePassword"
                class="absolute left-3 top-1/2 -translate-y-1/2 text-neutral hover:text-primary transition-colors"
              >
                <EyeOff v-if="showPassword" class="w-5 h-5" />
                <Eye v-else class="w-5 h-5" />
              </button>
            </div>
          </div>

          <!-- Remember Me -->
          <div class="flex items-center justify-between">
            <label class="flex items-center gap-2 cursor-pointer">
              <input
                v-model="rememberMe"
                type="checkbox"
                class="w-4 h-4 rounded border-border text-primary focus:ring-primary"
              />
              <span class="text-sm text-gray-600">تذكرني</span>
            </label>
            <a href="#" class="text-sm text-primary hover:text-primary-dark transition-colors">
              نسيت كلمة المرور؟
            </a>
          </div>

          <!-- Submit Button -->
          <button
            type="submit"
            :disabled="!isFormValid || isLoading"
            class="w-full btn btn-primary flex items-center justify-center gap-2"
            :class="{ 'opacity-70 cursor-not-allowed': !isFormValid || isLoading }"
          >
            <Loader2 v-if="isLoading" class="w-5 h-5 animate-spin" />
            <span>{{ isLoading ? 'جاري تسجيل الدخول...' : 'تسجيل الدخول' }}</span>
          </button>
        </form>

        <!-- Footer -->
        <div class="mt-8 pt-6 border-t border-border text-center">
          <p class="text-sm text-neutral">
            © {{ new Date().getFullYear() }} نظام إدارة وحدة النقل - جميع الحقوق محفوظة
          </p>
        </div>
      </div>
    </div>
  </div>
</template>
