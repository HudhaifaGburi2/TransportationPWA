<script setup lang="ts">
import { computed } from 'vue'
import { useThemeStore } from '@/stores/theme'
import { Sun, Moon } from 'lucide-vue-next'

const themeStore = useThemeStore()

const isDark = computed(() => themeStore.isDark())

const toggleTheme = () => {
    themeStore.toggleTheme()
}
</script>

<template>
  <button
    @click="toggleTheme"
    class="theme-toggle"
    :aria-label="isDark ? 'التبديل إلى الوضع الفاتح' : 'التبديل إلى الوضع الداكن'"
    :title="isDark ? 'الوضع الفاتح' : 'الوضع الداكن'"
  >
    <span class="theme-toggle__icon">
      <Transition name="theme-icon" mode="out-in">
        <Moon v-if="isDark" class="w-5 h-5" />
        <Sun v-else class="w-5 h-5" />
      </Transition>
    </span>
  </button>
</template>

<style scoped>
.theme-toggle {
  display: inline-flex;
  align-items: center;
  justify-content: center;
  width: 40px;
  height: 40px;
  border-radius: 8px;
  background-color: rgba(255, 255, 255, 0.1);
  color: inherit;
  border: none;
  cursor: pointer;
  transition: all 0.3s ease;
}

.theme-toggle:hover {
  background-color: rgba(255, 255, 255, 0.2);
  transform: scale(1.05);
}

.theme-toggle:active {
  transform: scale(0.95);
}

.theme-toggle__icon {
  display: flex;
  align-items: center;
  justify-content: center;
}

/* Icon transition */
.theme-icon-enter-active,
.theme-icon-leave-active {
  transition: all 0.2s ease;
}

.theme-icon-enter-from {
  opacity: 0;
  transform: rotate(-90deg) scale(0.5);
}

.theme-icon-leave-to {
  opacity: 0;
  transform: rotate(90deg) scale(0.5);
}
</style>
