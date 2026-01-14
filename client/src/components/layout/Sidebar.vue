<template>
  <aside class="bg-primary text-primary-content w-64 min-h-screen flex flex-col">
    <!-- Logo -->
    <div class="p-4 border-b border-primary-content/20">
      <h1 class="text-xl font-bold text-center">نظام النقل</h1>
      <p class="text-xs text-center text-primary-content/70">TUMS</p>
    </div>

    <!-- User Info -->
    <div class="p-4 border-b border-primary-content/20">
      <div class="flex items-center gap-3">
        <div class="avatar placeholder">
          <div class="bg-primary-content/20 text-primary-content rounded-full w-10">
            <span class="text-sm">{{ userInitials }}</span>
          </div>
        </div>
        <div class="flex-1 min-w-0">
          <p class="text-sm font-medium truncate">{{ fullName }}</p>
          <p class="text-xs text-primary-content/70 truncate">{{ roleName }}</p>
        </div>
      </div>
    </div>

    <!-- Navigation -->
    <nav class="flex-1 p-4">
      <ul class="menu menu-sm">
        <li v-for="item in visibleMenuItems" :key="item.path">
          <router-link 
            :to="item.path"
            class="flex items-center gap-3 py-2 px-3 rounded-lg hover:bg-primary-content/10"
            :class="{ 'bg-primary-content/20': isActive(item.path) }"
          >
            <component :is="item.icon" class="w-5 h-5" />
            <span>{{ item.label }}</span>
          </router-link>
        </li>
      </ul>
    </nav>

    <!-- Logout -->
    <div class="p-4 border-t border-primary-content/20">
      <button 
        @click="handleLogout"
        class="btn btn-ghost btn-block text-primary-content hover:bg-primary-content/10"
      >
        <LogOut class="w-5 h-5" />
        <span>تسجيل الخروج</span>
      </button>
    </div>
  </aside>
</template>

<script setup lang="ts">
import { computed } from 'vue'
import { useRoute } from 'vue-router'
import { useAuthStore, TUMS_ROLES } from '@/stores/auth'
import { 
  LayoutDashboard, 
  MapPin, 
  Map, 
  Users, 
  ClipboardList,
  LogOut,
  Bus,
  Route,
  FileText
} from 'lucide-vue-next'

const route = useRoute()
const authStore = useAuthStore()

interface MenuItem {
  path: string
  label: string
  icon: any
  roles: string[]
}

const menuItems: MenuItem[] = [
  {
    path: '/',
    label: 'لوحة التحكم',
    icon: LayoutDashboard,
    roles: [] // All authenticated users
  },
  {
    path: '/buses',
    label: 'إدارة الباصات',
    icon: Bus,
    roles: [TUMS_ROLES.ADMIN, TUMS_ROLES.STAFF, TUMS_ROLES.DRIVER, TUMS_ROLES.SYSTEM_ADMIN]
  },
  {
    path: '/routes',
    label: 'المسارات',
    icon: Route,
    roles: [TUMS_ROLES.ADMIN, TUMS_ROLES.STAFF, TUMS_ROLES.SYSTEM_ADMIN]
  },
  {
    path: '/registration-requests',
    label: 'طلبات التسجيل',
    icon: FileText,
    roles: [TUMS_ROLES.ADMIN, TUMS_ROLES.STAFF, TUMS_ROLES.SYSTEM_ADMIN]
  },
  {
    path: '/districts',
    label: 'المناطق',
    icon: Map,
    roles: [TUMS_ROLES.ADMIN, TUMS_ROLES.STAFF, TUMS_ROLES.SYSTEM_ADMIN]
  },
  {
    path: '/locations',
    label: 'المواقع',
    icon: MapPin,
    roles: [TUMS_ROLES.ADMIN, TUMS_ROLES.STAFF, TUMS_ROLES.SYSTEM_ADMIN]
  },
  {
    path: '/registration',
    label: 'التسجيل',
    icon: ClipboardList,
    roles: [TUMS_ROLES.STUDENT]
  },
  {
    path: '/my-registration',
    label: 'طلبي',
    icon: Users,
    roles: [TUMS_ROLES.STUDENT]
  }
]

const visibleMenuItems = computed(() => {
  return menuItems.filter(item => {
    if (item.roles.length === 0) return true
    return item.roles.some(role => authStore.hasRole(role))
  })
})

const fullName = computed(() => authStore.fullName || 'مستخدم')

const userInitials = computed(() => {
  const name = authStore.fullName || ''
  return name.split(' ').map(n => n[0]).join('').slice(0, 2) || 'م'
})

const roleName = computed(() => {
  if (authStore.hasRole(TUMS_ROLES.ADMIN)) return 'مدير النظام'
  if (authStore.hasRole(TUMS_ROLES.STAFF)) return 'موظف'
  if (authStore.hasRole(TUMS_ROLES.DRIVER)) return 'سائق'
  if (authStore.hasRole(TUMS_ROLES.STUDENT)) return 'طالب'
  if (authStore.hasRole(TUMS_ROLES.SYSTEM_ADMIN)) return 'مدير عام'
  return 'مستخدم'
})

const isActive = (path: string) => {
  return route.path === path
}

const handleLogout = () => {
  authStore.logout()
}
</script>
