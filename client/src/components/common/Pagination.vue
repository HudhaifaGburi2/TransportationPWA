<template>
  <div class="flex items-center justify-between mt-4">
    <div class="text-sm text-base-content/70">
      عرض {{ startItem }} - {{ endItem }} من {{ totalItems }}
    </div>
    <div class="join">
      <button 
        class="join-item btn btn-sm"
        :disabled="currentPage === 1"
        @click="goToPage(1)"
      >
        «
      </button>
      <button 
        class="join-item btn btn-sm"
        :disabled="currentPage === 1"
        @click="goToPage(currentPage - 1)"
      >
        ‹
      </button>
      
      <template v-for="page in visiblePages" :key="page">
        <button 
          v-if="page === '...'"
          class="join-item btn btn-sm btn-disabled"
        >
          ...
        </button>
        <button 
          v-else
          class="join-item btn btn-sm"
          :class="{ 'btn-primary': page === currentPage }"
          @click="goToPage(page as number)"
        >
          {{ page }}
        </button>
      </template>
      
      <button 
        class="join-item btn btn-sm"
        :disabled="currentPage === totalPages"
        @click="goToPage(currentPage + 1)"
      >
        ›
      </button>
      <button 
        class="join-item btn btn-sm"
        :disabled="currentPage === totalPages"
        @click="goToPage(totalPages)"
      >
        »
      </button>
    </div>
  </div>
</template>

<script setup lang="ts">
import { computed } from 'vue'

interface Props {
  currentPage: number
  totalItems: number
  pageSize: number
}

const props = defineProps<Props>()

const emit = defineEmits<{
  (e: 'page-change', page: number): void
}>()

const totalPages = computed(() => Math.ceil(props.totalItems / props.pageSize))

const startItem = computed(() => {
  if (props.totalItems === 0) return 0
  return (props.currentPage - 1) * props.pageSize + 1
})

const endItem = computed(() => {
  return Math.min(props.currentPage * props.pageSize, props.totalItems)
})

const visiblePages = computed(() => {
  const pages: (number | string)[] = []
  const total = totalPages.value
  const current = props.currentPage

  if (total <= 7) {
    for (let i = 1; i <= total; i++) pages.push(i)
  } else {
    pages.push(1)
    
    if (current > 3) pages.push('...')
    
    const start = Math.max(2, current - 1)
    const end = Math.min(total - 1, current + 1)
    
    for (let i = start; i <= end; i++) pages.push(i)
    
    if (current < total - 2) pages.push('...')
    
    pages.push(total)
  }
  
  return pages
})

const goToPage = (page: number) => {
  if (page >= 1 && page <= totalPages.value && page !== props.currentPage) {
    emit('page-change', page)
  }
}
</script>
