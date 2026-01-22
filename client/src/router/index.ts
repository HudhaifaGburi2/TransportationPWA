import { createRouter, createWebHistory } from 'vue-router'
import type { RouteRecordRaw } from 'vue-router'
import { useAuthStore, TUMS_ROLES } from '@/stores/auth'

const routes: RouteRecordRaw[] = [
    // Auth routes
    {
        path: '/login',
        name: 'Login',
        component: () => import('@/views/auth/LoginView.vue'),
        meta: { requiresAuth: false, layout: 'blank' }
    },

    // Dashboard - accessible to all authenticated users
    {
        path: '/',
        name: 'Dashboard',
        component: () => import('@/views/dashboard/DashboardView.vue'),
        meta: { requiresAuth: true }
    },

    // Admin & Staff routes (Admin_Tums, Stuff_Tums, SYSTEM_ADMINISTRATOR)
    {
        path: '/districts',
        name: 'Districts',
        component: () => import('@/views/master-data/DistrictsView.vue'),
        meta: {
            requiresAuth: true,
            roles: [TUMS_ROLES.ADMIN, TUMS_ROLES.STAFF, TUMS_ROLES.SYSTEM_ADMIN]
        }
    },
    {
        path: '/locations',
        name: 'Locations',
        component: () => import('@/views/master-data/LocationsView.vue'),
        meta: {
            requiresAuth: true,
            roles: [TUMS_ROLES.ADMIN, TUMS_ROLES.STAFF, TUMS_ROLES.SYSTEM_ADMIN]
        }
    },

    // Bus Management routes (Phase 2)
    {
        path: '/buses',
        name: 'Buses',
        component: () => import('@/views/buses/BusListView.vue'),
        meta: {
            requiresAuth: true,
            roles: [TUMS_ROLES.ADMIN, TUMS_ROLES.STAFF, TUMS_ROLES.DRIVER, TUMS_ROLES.SYSTEM_ADMIN]
        }
    },
    {
        path: '/buses/:id',
        name: 'BusDetail',
        component: () => import('@/views/buses/BusDetailView.vue'),
        meta: {
            requiresAuth: true,
            roles: [TUMS_ROLES.ADMIN, TUMS_ROLES.STAFF, TUMS_ROLES.DRIVER, TUMS_ROLES.SYSTEM_ADMIN]
        }
    },
    {
        path: '/buses/:id/students',
        name: 'BusStudents',
        component: () => import('@/views/buses/BusStudentsView.vue'),
        meta: {
            requiresAuth: true,
            roles: [TUMS_ROLES.ADMIN, TUMS_ROLES.STAFF, TUMS_ROLES.SYSTEM_ADMIN]
        }
    },
    {
        path: '/routes',
        name: 'Routes',
        component: () => import('@/views/buses/RoutesView.vue'),
        meta: {
            requiresAuth: true,
            roles: [TUMS_ROLES.ADMIN, TUMS_ROLES.STAFF, TUMS_ROLES.SYSTEM_ADMIN]
        }
    },

    // Bus Management Module (Phase 2 - Full CRUD)
    {
        path: '/bus-management',
        name: 'BusManagementDashboard',
        component: () => import('@/views/busManagement/DashboardView.vue'),
        meta: {
            requiresAuth: true,
            roles: [TUMS_ROLES.ADMIN, TUMS_ROLES.STAFF, TUMS_ROLES.SYSTEM_ADMIN]
        }
    },
    {
        path: '/bus-management/drivers',
        name: 'DriversManagement',
        component: () => import('@/views/busManagement/DriversView.vue'),
        meta: {
            requiresAuth: true,
            roles: [TUMS_ROLES.ADMIN, TUMS_ROLES.STAFF, TUMS_ROLES.SYSTEM_ADMIN]
        }
    },
    {
        path: '/bus-management/routes',
        name: 'RoutesManagement',
        component: () => import('@/views/busManagement/RoutesView.vue'),
        meta: {
            requiresAuth: true,
            roles: [TUMS_ROLES.ADMIN, TUMS_ROLES.STAFF, TUMS_ROLES.SYSTEM_ADMIN]
        }
    },
    {
        path: '/bus-management/buses',
        name: 'BusesManagement',
        component: () => import('@/views/busManagement/BusesView.vue'),
        meta: {
            requiresAuth: true,
            roles: [TUMS_ROLES.ADMIN, TUMS_ROLES.STAFF, TUMS_ROLES.SYSTEM_ADMIN]
        }
    },

    {
        path: '/registration-requests',
        name: 'RegistrationRequests',
        component: () => import('@/views/admin/RegistrationRequestsView.vue'),
        meta: {
            requiresAuth: true,
            roles: [TUMS_ROLES.ADMIN, TUMS_ROLES.STAFF, TUMS_ROLES.SYSTEM_ADMIN]
        }
    },

    // Student Management routes (Phase 3)
    {
        path: '/students',
        name: 'StudentsList',
        component: () => import('@/views/students/StudentsListView.vue'),
        meta: {
            requiresAuth: true,
            roles: [TUMS_ROLES.ADMIN, TUMS_ROLES.STAFF, TUMS_ROLES.SYSTEM_ADMIN]
        }
    },
    {
        path: '/students/suspended',
        name: 'SuspendedStudents',
        component: () => import('@/views/students/SuspendedStudentsView.vue'),
        meta: {
            requiresAuth: true,
            roles: [TUMS_ROLES.ADMIN, TUMS_ROLES.STAFF, TUMS_ROLES.SYSTEM_ADMIN]
        }
    },
    {
        path: '/students/leaves',
        name: 'StudentLeaves',
        component: () => import('@/views/students/StudentLeavesView.vue'),
        meta: {
            requiresAuth: true,
            roles: [TUMS_ROLES.ADMIN, TUMS_ROLES.STAFF, TUMS_ROLES.SYSTEM_ADMIN]
        }
    },
    {
        path: '/students/:id',
        name: 'StudentDetail',
        component: () => import('@/views/students/StudentDetailView.vue'),
        meta: {
            requiresAuth: true,
            roles: [TUMS_ROLES.ADMIN, TUMS_ROLES.STAFF, TUMS_ROLES.SYSTEM_ADMIN]
        }
    },
    {
        path: '/students/:id/transfer',
        name: 'StudentTransfer',
        component: () => import('@/views/students/StudentTransferView.vue'),
        meta: {
            requiresAuth: true,
            roles: [TUMS_ROLES.ADMIN, TUMS_ROLES.STAFF, TUMS_ROLES.SYSTEM_ADMIN]
        }
    },

    // Student routes - accessible to verified students (dynamic check via CentralDB)
    {
        path: '/registration',
        name: 'Registration',
        component: () => import('@/views/student/RegistrationView.vue'),
        meta: {
            requiresAuth: true,
            requiresStudent: true
        }
    },
    {
        path: '/my-registration',
        name: 'MyRegistration',
        component: () => import('@/views/student/MyRegistrationView.vue'),
        meta: {
            requiresAuth: true,
            requiresStudent: true
        }
    },

    // Error pages
    {
        path: '/unauthorized',
        name: 'Unauthorized',
        component: () => import('@/views/errors/UnauthorizedView.vue'),
        meta: { requiresAuth: false, layout: 'blank' }
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
    const requiresStudent = to.meta.requiresStudent as boolean | undefined

    // Check authentication
    if (requiresAuth && !authStore.isAuthenticated) {
        next({ name: 'Login', query: { redirect: to.fullPath } })
        return
    }

    // Check role-based access for admin/staff/driver routes
    if (requiredRoles && requiredRoles.length > 0) {
        const hasRequiredRole = requiredRoles.some(role => authStore.hasRole(role))
        if (!hasRequiredRole) {
            next({ name: 'Unauthorized' })
            return
        }
    }

    // Check student access - verified via CentralDB view
    if (requiresStudent) {
        // Student status is verified dynamically via API call in DashboardView
        // If not verified yet or not a student, redirect to unauthorized
        if (!authStore.isStudent && !authStore.hasTumsRole) {
            next({ name: 'Unauthorized' })
            return
        }
    }

    // Redirect authenticated users away from login
    if (to.name === 'Login' && authStore.isAuthenticated) {
        next({ name: 'Dashboard' })
        return
    }

    next()
})

export default router
