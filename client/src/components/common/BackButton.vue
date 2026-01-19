<template>
  <div class="flex items-center gap-4">
    <button @click="goBack" class="btn btn-ghost btn-sm gap-2">
      <ArrowRight class="w-4 h-4" />
      {{ label }}
    </button>
    <div v-if="breadcrumbs.length" class="text-sm breadcrumbs">
      <ul>
        <li v-for="(crumb, index) in breadcrumbs" :key="index">
          <router-link v-if="crumb.path" :to="crumb.path">{{ crumb.label }}</router-link>
          <span v-else>{{ crumb.label }}</span>
        </li>
      </ul>
    </div>
  </div>
</template>

<script setup lang="ts">
import { useRouter } from 'vue-router'
import { ArrowRight } from 'lucide-vue-next'

interface Breadcrumb {
  label: string
  path?: string
}

const props = withDefaults(defineProps<{
  label?: string
  to?: string
  breadcrumbs?: Breadcrumb[]
}>(), {
  label: 'رجوع',
  breadcrumbs: () => []
})

const router = useRouter()

const goBack = () => {
  if (props.to) {
    router.push(props.to)
  } else {
    router.back()
  }
}
</script>
