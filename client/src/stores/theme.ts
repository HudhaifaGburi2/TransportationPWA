import { defineStore } from 'pinia'
import { ref, watch } from 'vue'

export type ThemeMode = 'tums' | 'tums-dark'

const THEME_STORAGE_KEY = 'tums-theme-preference'
const FIRST_VISIT_KEY = 'tums-first-visit'

export const useThemeStore = defineStore('theme', () => {
    const currentTheme = ref<ThemeMode>('tums')
    const isInitialized = ref(false)

    /**
     * Initialize theme on app start
     * - First visit: respect system preference, then default to light
     * - Subsequent visits: use persisted preference
     */
    function initializeTheme(): void {
        const storedTheme = localStorage.getItem(THEME_STORAGE_KEY) as ThemeMode | null
        const isFirstVisit = !localStorage.getItem(FIRST_VISIT_KEY)

        if (isFirstVisit) {
            // First visit - check system preference
            localStorage.setItem(FIRST_VISIT_KEY, 'false')

            const prefersDark = window.matchMedia('(prefers-color-scheme: dark)').matches
            currentTheme.value = prefersDark ? 'tums-dark' : 'tums'

            // Persist the initial choice
            localStorage.setItem(THEME_STORAGE_KEY, currentTheme.value)
        } else if (storedTheme && (storedTheme === 'tums' || storedTheme === 'tums-dark')) {
            // Subsequent visit - use stored preference
            currentTheme.value = storedTheme
        } else {
            // Fallback to light theme
            currentTheme.value = 'tums'
            localStorage.setItem(THEME_STORAGE_KEY, 'tums')
        }

        applyTheme(currentTheme.value)
        isInitialized.value = true
    }

    /**
     * Apply theme to document
     */
    function applyTheme(theme: ThemeMode): void {
        document.documentElement.setAttribute('data-theme', theme)

        // Update color-scheme for browser UI elements
        document.documentElement.style.colorScheme = theme === 'tums-dark' ? 'dark' : 'light'

        // Update meta theme-color for mobile browsers
        const metaThemeColor = document.querySelector('meta[name="theme-color"]')
        if (metaThemeColor) {
            metaThemeColor.setAttribute('content', theme === 'tums-dark' ? '#1a1a2e' : '#3d5a4f')
        }
    }

    /**
     * Toggle between light and dark themes
     */
    function toggleTheme(): void {
        currentTheme.value = currentTheme.value === 'tums' ? 'tums-dark' : 'tums'
    }

    /**
     * Set specific theme
     */
    function setTheme(theme: ThemeMode): void {
        currentTheme.value = theme
    }

    /**
     * Check if current theme is dark
     */
    function isDark(): boolean {
        return currentTheme.value === 'tums-dark'
    }

    // Watch for theme changes and persist
    watch(currentTheme, (newTheme) => {
        localStorage.setItem(THEME_STORAGE_KEY, newTheme)
        applyTheme(newTheme)
    })

    return {
        currentTheme,
        isInitialized,
        initializeTheme,
        toggleTheme,
        setTheme,
        isDark
    }
})
