<script setup lang="ts">
import { useAuthStore } from '@/stores/auth'
import { Bus, Users, MapPin, Calendar } from 'lucide-vue-next'

const authStore = useAuthStore()

const stats = [
  { title: 'إجمالي الحافلات', value: '111', icon: Bus, color: 'bg-primary' },
  { title: 'الأحياء', value: '88', icon: MapPin, color: 'bg-secondary' },
  { title: 'مواقف السيارات', value: '8', icon: Calendar, color: 'bg-info' },
  { title: 'الطلاب المسجلين', value: '0', icon: Users, color: 'bg-success' },
]
</script>

<template>
  <div class="min-h-screen bg-background">
    <!-- Header -->
    <header class="bg-primary text-white shadow-md">
      <div class="container mx-auto px-4 py-4 flex items-center justify-between">
        <div class="flex items-center gap-4">
          <div class="w-10 h-10 bg-white/20 rounded-lg flex items-center justify-center">
            <Bus class="w-6 h-6" />
          </div>
          <div>
            <h1 class="text-xl font-bold font-cairo">نظام إدارة وحدة النقل</h1>
            <p class="text-sm text-white/80">لوحة التحكم</p>
          </div>
        </div>
        <div class="flex items-center gap-4">
          <span class="text-sm">مرحباً، {{ authStore.fullName || 'مستخدم' }}</span>
          <button
            @click="authStore.logout()"
            class="px-4 py-2 bg-white/20 hover:bg-white/30 rounded-lg text-sm transition-colors"
          >
            تسجيل الخروج
          </button>
        </div>
      </div>
    </header>

    <!-- Main Content -->
    <main class="container mx-auto px-4 py-8">
      <!-- Welcome Card -->
      <div class="card mb-8">
        <h2 class="text-xl font-bold text-gray-800 mb-2">مرحباً بك في نظام إدارة وحدة النقل</h2>
        <p class="text-neutral">يمكنك من هنا إدارة الحافلات والأحياء والطلاب وتتبع الحضور والانصراف</p>
      </div>

      <!-- Stats Grid -->
      <div class="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-6 mb-8">
        <div v-for="stat in stats" :key="stat.title" class="card hover:shadow-md transition-shadow">
          <div class="flex items-center gap-4">
            <div :class="[stat.color, 'w-12 h-12 rounded-lg flex items-center justify-center']">
              <component :is="stat.icon" class="w-6 h-6 text-white" />
            </div>
            <div>
              <p class="text-sm text-neutral">{{ stat.title }}</p>
              <p class="text-2xl font-bold text-gray-800">{{ stat.value }}</p>
            </div>
          </div>
        </div>
      </div>

      <!-- Quick Actions -->
      <div class="card">
        <h3 class="text-lg font-bold text-gray-800 mb-4">الإجراءات السريعة</h3>
        <div class="grid grid-cols-2 md:grid-cols-4 gap-4">
          <router-link
            to="/districts"
            class="flex flex-col items-center gap-2 p-4 bg-background rounded-lg hover:bg-primary/5 transition-colors"
          >
            <MapPin class="w-8 h-8 text-primary" />
            <span class="text-sm font-medium">إدارة الأحياء</span>
          </router-link>
          <router-link
            to="/locations"
            class="flex flex-col items-center gap-2 p-4 bg-background rounded-lg hover:bg-primary/5 transition-colors"
          >
            <Calendar class="w-8 h-8 text-secondary" />
            <span class="text-sm font-medium">المواقف</span>
          </router-link>
          <button
            class="flex flex-col items-center gap-2 p-4 bg-background rounded-lg hover:bg-primary/5 transition-colors"
          >
            <Bus class="w-8 h-8 text-info" />
            <span class="text-sm font-medium">الحافلات</span>
          </button>
          <button
            class="flex flex-col items-center gap-2 p-4 bg-background rounded-lg hover:bg-primary/5 transition-colors"
          >
            <Users class="w-8 h-8 text-success" />
            <span class="text-sm font-medium">الطلاب</span>
          </button>
        </div>
      </div>
    </main>
  </div>
</template>
