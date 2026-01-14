<script setup lang="ts">
import { computed } from 'vue'
import { RouterLink } from 'vue-router'

interface Props {
  variant?: 'primary' | 'secondary' | 'outline' | 'ghost' | 'danger'
  size?: 'sm' | 'md' | 'lg'
  to?: string
  href?: string
  disabled?: boolean
  loading?: boolean
  block?: boolean
}

const props = withDefaults(defineProps<Props>(), {
  variant: 'primary',
  size: 'md',
  disabled: false,
  loading: false,
  block: false
})

const componentType = computed(() => {
  if (props.to) return RouterLink
  if (props.href) return 'a'
  return 'button'
})

const componentProps = computed(() => {
  if (props.to) return { to: props.to }
  if (props.href) return { href: props.href, target: '_blank', rel: 'noopener noreferrer' }
  return { type: 'button', disabled: props.disabled || props.loading }
})

const buttonClasses = computed(() => [
  'base-button',
  `base-button--${props.variant}`,
  `base-button--${props.size}`,
  {
    'base-button--disabled': props.disabled,
    'base-button--loading': props.loading,
    'base-button--block': props.block
  }
])
</script>

<template>
  <component
    :is="componentType"
    v-bind="componentProps"
    :class="buttonClasses"
    :aria-disabled="disabled || loading"
    role="button"
  >
    <span v-if="loading" class="base-button__loader">
      <span class="loading loading-spinner loading-sm"></span>
    </span>
    <span class="base-button__content" :class="{ 'opacity-0': loading }">
      <span v-if="$slots.icon" class="base-button__icon">
        <slot name="icon" />
      </span>
      <span class="base-button__text">
        <slot />
      </span>
    </span>
  </component>
</template>

<style scoped>
/* Base Button Styles - TUMS Design System */
.base-button {
  /* Layout */
  display: inline-flex;
  align-items: center;
  justify-content: center;
  position: relative;
  
  /* Typography */
  font-family: 'Cairo', 'Segoe UI', Tahoma, sans-serif;
  font-weight: 600;
  white-space: nowrap;
  text-decoration: none;
  
  /* Border */
  border: none;
  border-radius: 8px;
  
  /* Cursor */
  cursor: pointer;
  
  /* Transitions */
  transition: all 0.3s ease;
}

/* Content wrapper */
.base-button__content {
  display: inline-flex;
  align-items: center;
  justify-content: center;
  gap: 12px;
  flex-direction: row-reverse; /* RTL: Icon on right */
}

.base-button__icon {
  display: inline-flex;
  align-items: center;
  justify-content: center;
}

.base-button__loader {
  position: absolute;
  inset: 0;
  display: flex;
  align-items: center;
  justify-content: center;
}

/* ======================== */
/* VARIANTS                 */
/* ======================== */

/* Primary (Default) */
.base-button--primary {
  background-color: #3d5a4f;
  color: #ffffff;
  box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);
}

.base-button--primary:hover:not(.base-button--disabled) {
  background-color: #4d6f62;
  box-shadow: 0 4px 8px rgba(0, 0, 0, 0.15);
  transform: translateY(-1px);
}

.base-button--primary:active:not(.base-button--disabled) {
  background-color: #2d4a3f;
  transform: scale(0.98);
}

/* Secondary (Gold) */
.base-button--secondary {
  background-color: #b8935f;
  color: #ffffff;
  box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);
}

.base-button--secondary:hover:not(.base-button--disabled) {
  background-color: #c9a570;
  box-shadow: 0 4px 8px rgba(0, 0, 0, 0.15);
  transform: translateY(-1px);
}

.base-button--secondary:active:not(.base-button--disabled) {
  background-color: #a68350;
  transform: scale(0.98);
}

/* Outline */
.base-button--outline {
  background-color: transparent;
  color: #3d5a4f;
  border: 2px solid #3d5a4f;
}

.base-button--outline:hover:not(.base-button--disabled) {
  background-color: #3d5a4f;
  color: #ffffff;
}

.base-button--outline:active:not(.base-button--disabled) {
  background-color: #2d4a3f;
  border-color: #2d4a3f;
  transform: scale(0.98);
}

/* Ghost */
.base-button--ghost {
  background-color: transparent;
  color: #3d5a4f;
}

.base-button--ghost:hover:not(.base-button--disabled) {
  background-color: rgba(61, 90, 79, 0.1);
}

.base-button--ghost:active:not(.base-button--disabled) {
  background-color: rgba(61, 90, 79, 0.2);
}

/* Danger */
.base-button--danger {
  background-color: #dc3545;
  color: #ffffff;
  box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);
}

.base-button--danger:hover:not(.base-button--disabled) {
  background-color: #c82333;
  box-shadow: 0 4px 8px rgba(0, 0, 0, 0.15);
  transform: translateY(-1px);
}

.base-button--danger:active:not(.base-button--disabled) {
  background-color: #bd2130;
  transform: scale(0.98);
}

/* ======================== */
/* SIZES                    */
/* ======================== */

.base-button--sm {
  padding: 8px 16px;
  font-size: 12px;
  min-height: 36px;
}

.base-button--sm .base-button__icon :deep(svg) {
  width: 14px;
  height: 14px;
}

.base-button--md {
  padding: 12px 24px;
  font-size: 14px;
  min-height: 44px;
}

.base-button--md .base-button__icon :deep(svg) {
  width: 18px;
  height: 18px;
}

.base-button--lg {
  padding: 16px 32px;
  font-size: 16px;
  min-height: 52px;
}

.base-button--lg .base-button__icon :deep(svg) {
  width: 22px;
  height: 22px;
}

/* ======================== */
/* STATES                   */
/* ======================== */

.base-button--disabled {
  background-color: #e9ecef !important;
  color: #6c757d !important;
  cursor: not-allowed;
  box-shadow: none !important;
  transform: none !important;
  border-color: #e9ecef !important;
}

.base-button--block {
  width: 100%;
}

/* ======================== */
/* FOCUS                    */
/* ======================== */

.base-button:focus-visible {
  outline: 2px solid #3d5a4f;
  outline-offset: 2px;
}
</style>
