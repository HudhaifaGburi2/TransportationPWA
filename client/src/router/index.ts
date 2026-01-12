import { createRouter, createWebHistory } from 'vue-router'
import type { RouteRecordRaw } from 'vue-router'
import { useAuthStore } from '@/stores/auth'

const routes: RouteRecordRaw[] = [
    {
        path: '/login',
        name: 'Login',
        component: () => import('@/views/auth/LoginView.vue'),
        meta: { requiresAuth: false, layout: 'blank' }
    },
    {
        path: '/',
        name: 'Dashboard',
        component: () => import('@/views/dashboard/DashboardView.vue'),
        meta: { requiresAuth: true }
    },
    {
        path: '/districts',
        name: 'Districts',
        component: () => import('@/views/master-data/DistrictsView.vue'),
        meta: { requiresAuth: true, roles: ['Admin', 'Staff'] }
    },
    {
        path: '/locations',
        name: 'Locations',
        component: () => import('@/views/master-data/LocationsView.vue'),
        meta: { requiresAuth: true, roles: ['Admin', 'Staff'] }
    },
    {
        path: '/:pathMatch(.*)*',
        name: 'NotFound',
        component: () => import('@/views/errors/NotFoundView.vue'),
        meta: { requiresAuth: false, layout: 'blank' }
    }
]

const router = createRouter({
    history: createWebHistory(),
    routes
})

router.beforeEach(async (to, _from, next) => {
    const authStore = useAuthStore()
    const requiresAuth = to.meta.requiresAuth !== false
    const requiredRoles = to.meta.roles as string[] | undefined

    if (requiresAuth && !authStore.isAuthenticated) {
        next({ name: 'Login', query: { redirect: to.fullPath } })
        return
    }

    if (requiredRoles && requiredRoles.length > 0) {
        const hasRole = requiredRoles.some(role => authStore.hasRole(role))
        if (!hasRole) {
            next({ name: 'Dashboard' })
            return
        }
    }

    if (to.name === 'Login' && authStore.isAuthenticated) {
        next({ name: 'Dashboard' })
        return
    }

    next()
})

export default router
