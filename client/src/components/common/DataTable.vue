<template>
  <div class="overflow-x-auto">
    <table class="table table-zebra w-full">
      <thead>
        <tr>
          <th v-for="column in columns" :key="column.key" class="bg-base-200">
            <button 
              v-if="column.sortable" 
              @click="handleSort(column.key)"
              class="flex items-center gap-1 hover:text-primary"
            >
              {{ column.label }}
              <span v-if="sortKey === column.key">
                {{ sortOrder === 'asc' ? '↑' : '↓' }}
              </span>
            </button>
            <span v-else>{{ column.label }}</span>
          </th>
          <th v-if="$slots.actions" class="bg-base-200">الإجراءات</th>
        </tr>
      </thead>
      <tbody>
        <tr v-if="loading">
          <td :colspan="columns.length + ($slots.actions ? 1 : 0)" class="text-center py-8">
            <span class="loading loading-spinner loading-lg text-primary"></span>
          </td>
        </tr>
        <tr v-else-if="!data || data.length === 0">
          <td :colspan="columns.length + ($slots.actions ? 1 : 0)" class="text-center py-8 text-base-content/60">
            {{ emptyMessage }}
          </td>
        </tr>
        <tr v-else v-for="(row, index) in data" :key="row.id || index" class="hover">
          <td v-for="column in columns" :key="column.key">
            <slot :name="`cell-${column.key}`" :row="row" :value="row[column.key]">
              {{ row[column.key] }}
            </slot>
          </td>
          <td v-if="$slots.actions">
            <slot name="actions" :row="row" :index="index"></slot>
          </td>
        </tr>
      </tbody>
    </table>
  </div>
</template>

<script setup lang="ts">
import { ref } from 'vue'

export interface Column {
  key: string
  label: string
  sortable?: boolean
}

interface Props {
  columns: Column[]
  data: any[]
  loading?: boolean
  emptyMessage?: string
}

const props = withDefaults(defineProps<Props>(), {
  loading: false,
  emptyMessage: 'لا توجد بيانات'
})

const emit = defineEmits<{
  (e: 'sort', key: string, order: 'asc' | 'desc'): void
}>()

const sortKey = ref<string | null>(null)
const sortOrder = ref<'asc' | 'desc'>('asc')

const handleSort = (key: string) => {
  if (sortKey.value === key) {
    sortOrder.value = sortOrder.value === 'asc' ? 'desc' : 'asc'
  } else {
    sortKey.value = key
    sortOrder.value = 'asc'
  }
  emit('sort', key, sortOrder.value)
}
</script>
