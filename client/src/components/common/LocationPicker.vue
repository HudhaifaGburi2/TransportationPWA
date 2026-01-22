<template>
  <div class="location-picker">
    <div class="flex flex-col gap-3">
      <!-- GPS Capture Button -->
      <button
        type="button"
        class="btn btn-outline btn-lg gap-2"
        :class="{ 'btn-success': hasLocation, 'btn-error': error }"
        @click="captureLocation"
        :disabled="loading"
      >
        <span v-if="loading" class="loading loading-spinner loading-sm"></span>
        <MapPin v-else class="w-5 h-5" />
        {{ buttonText }}
      </button>

      <!-- Error Message -->
      <div v-if="error" class="alert alert-error text-sm py-2">
        <AlertCircle class="w-4 h-4" />
        <span>{{ error }}</span>
      </div>

      <!-- Location Display -->
      <div v-if="hasLocation" class="bg-success/10 border border-success/30 rounded-lg p-4">
        <div class="flex items-center justify-between">
          <div class="flex items-center gap-2">
            <CheckCircle class="w-5 h-5 text-success" />
            <span class="font-semibold text-success">تم تحديد الموقع</span>
          </div>
          <button type="button" class="btn btn-ghost btn-xs" @click="clearLocation">
            <X class="w-4 h-4" />
          </button>
        </div>
        <div class="text-sm text-base-content/70 mt-2" dir="ltr">
          {{ latitude?.toFixed(6) }}, {{ longitude?.toFixed(6) }}
        </div>
        <a
          :href="googleMapsUrl"
          target="_blank"
          class="link link-primary text-sm flex items-center gap-1 mt-2"
        >
          <ExternalLink class="w-3 h-3" />
          عرض على خرائط جوجل
        </a>
      </div>

      <!-- Map Container (optional) -->
      <div
        v-if="showMap && hasLocation"
        ref="mapContainer"
        class="h-48 rounded-lg border border-base-300 overflow-hidden"
      >
        <div class="w-full h-full bg-base-200 flex items-center justify-center">
          <a :href="googleMapsUrl" target="_blank" class="btn btn-ghost btn-sm gap-2">
            <MapPin class="w-4 h-4" />
            عرض الموقع على الخريطة
          </a>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, computed, watch } from 'vue'
import { MapPin, AlertCircle, CheckCircle, X, ExternalLink } from 'lucide-vue-next'

const props = withDefaults(defineProps<{
  modelValue?: { lat: number; lng: number } | null
  showMap?: boolean
}>(), {
  showMap: false
})

const emit = defineEmits<{
  'update:modelValue': [value: { lat: number; lng: number } | null]
  'captured': [coords: { lat: number; lng: number }]
  'error': [message: string]
}>()

const loading = ref(false)
const error = ref<string | null>(null)
const latitude = ref<number | null>(props.modelValue?.lat ?? null)
const longitude = ref<number | null>(props.modelValue?.lng ?? null)

const hasLocation = computed(() => latitude.value !== null && longitude.value !== null)

const buttonText = computed(() => {
  if (loading.value) return 'جاري تحديد الموقع...'
  if (hasLocation.value) return 'تحديث الموقع'
  return '📍 تحديد موقعي'
})

const googleMapsUrl = computed(() => {
  if (!hasLocation.value) return ''
  return `https://www.google.com/maps?q=${latitude.value},${longitude.value}`
})

const captureLocation = async () => {
  if (!navigator.geolocation) {
    error.value = 'المتصفح لا يدعم تحديد الموقع'
    emit('error', error.value)
    return
  }

  loading.value = true
  error.value = null

  try {
    const position = await new Promise<GeolocationPosition>((resolve, reject) => {
      navigator.geolocation.getCurrentPosition(resolve, reject, {
        enableHighAccuracy: true,
        timeout: 15000,
        maximumAge: 0
      })
    })

    latitude.value = position.coords.latitude
    longitude.value = position.coords.longitude

    const coords = { lat: latitude.value, lng: longitude.value }
    emit('update:modelValue', coords)
    emit('captured', coords)
  } catch (err: any) {
    if (err.code === 1) {
      error.value = 'تم رفض الإذن بتحديد الموقع. يرجى السماح بالوصول للموقع.'
    } else if (err.code === 2) {
      error.value = 'تعذر تحديد الموقع. تأكد من تفعيل GPS.'
    } else if (err.code === 3) {
      error.value = 'انتهت مهلة تحديد الموقع. حاول مرة أخرى.'
    } else {
      error.value = 'حدث خطأ أثناء تحديد الموقع'
    }
    emit('error', error.value)
  } finally {
    loading.value = false
  }
}

const clearLocation = () => {
  latitude.value = null
  longitude.value = null
  emit('update:modelValue', null)
}

watch(() => props.modelValue, (newVal) => {
  if (newVal) {
    latitude.value = newVal.lat
    longitude.value = newVal.lng
  } else {
    latitude.value = null
    longitude.value = null
  }
})
</script>
