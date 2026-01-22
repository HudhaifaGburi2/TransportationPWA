<template>
  <div class="space-y-4">
    <h3 class="font-semibold text-lg flex items-center gap-2">
      <Clock class="w-5 h-5" />
      سجل الأحداث
    </h3>

    <div v-if="loading" class="flex justify-center py-8">
      <span class="loading loading-spinner loading-lg text-primary"></span>
    </div>

    <div v-else-if="events.length === 0" class="text-center py-8 text-base-content/50">
      <History class="w-12 h-12 mx-auto mb-2 opacity-30" />
      <p>لا توجد أحداث مسجلة</p>
    </div>

    <div v-else class="relative">
      <div class="absolute right-4 top-0 bottom-0 w-0.5 bg-base-300"></div>

      <div v-for="event in events" :key="event.id" class="relative pr-10 pb-6">
        <div :class="getEventIconClass(event.eventType)" class="absolute right-2 w-5 h-5 rounded-full flex items-center justify-center">
          <component :is="getEventIcon(event.eventType)" class="w-3 h-3 text-white" />
        </div>

        <div class="bg-base-100 border border-base-300 rounded-lg p-4">
          <div class="flex items-start justify-between">
            <div>
              <span :class="getEventBadgeClass(event.eventType)" class="badge badge-sm">
                {{ getEventLabel(event.eventType) }}
              </span>
              <p class="mt-2 text-sm">{{ event.description }}</p>
            </div>
            <span class="text-xs text-base-content/50">{{ formatDate(event.occurredAt) }}</span>
          </div>

          <div v-if="hasDetails(event)" class="mt-2 pt-2 border-t border-base-200 text-xs text-base-content/60">
            <div v-if="event.details.reason" class="flex gap-1">
              <span class="font-medium">السبب:</span>
              <span>{{ event.details.reason }}</span>
            </div>
            <div v-if="event.details.notes" class="flex gap-1">
              <span class="font-medium">ملاحظات:</span>
              <span>{{ event.details.notes }}</span>
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { Clock, History, XCircle, CheckCircle, Calendar, ArrowLeftRight } from 'lucide-vue-next'
import type { TimelineEvent } from '@/stores/students'

defineProps<{
  events: TimelineEvent[]
  loading?: boolean
}>()

const eventConfig: Record<string, { label: string; icon: any; iconClass: string; badgeClass: string }> = {
  suspension: { label: 'إيقاف', icon: XCircle, iconClass: 'bg-error', badgeClass: 'badge-error' },
  reactivation: { label: 'تفعيل', icon: CheckCircle, iconClass: 'bg-success', badgeClass: 'badge-success' },
  leave: { label: 'إجازة', icon: Calendar, iconClass: 'bg-warning', badgeClass: 'badge-warning' },
  transfer: { label: 'نقل', icon: ArrowLeftRight, iconClass: 'bg-info', badgeClass: 'badge-info' }
}

const getEventConfig = (type: string) => {
  const config = eventConfig[type.toLowerCase() as keyof typeof eventConfig]
  return config ?? eventConfig.suspension
}

const getEventIcon = (type: string) => getEventConfig(type)?.icon ?? XCircle
const getEventIconClass = (type: string) => getEventConfig(type)?.iconClass ?? 'bg-error'
const getEventBadgeClass = (type: string) => getEventConfig(type)?.badgeClass ?? 'badge-error'
const getEventLabel = (type: string) => getEventConfig(type)?.label ?? type

const formatDate = (dateStr: string) => {
  return new Date(dateStr).toLocaleDateString('ar-SA', {
    year: 'numeric',
    month: 'short',
    day: 'numeric',
    hour: '2-digit',
    minute: '2-digit'
  })
}

const hasDetails = (event: TimelineEvent) => {
  return event.details && (event.details.reason || event.details.notes)
}
</script>
