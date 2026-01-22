import { ref, computed } from 'vue'

export interface GeolocationCoords {
    lat: number
    lng: number
    accuracy?: number
}

export function useGeolocation() {
    const loading = ref(false)
    const error = ref<string | null>(null)
    const coords = ref<GeolocationCoords | null>(null)

    const isSupported = computed(() => 'geolocation' in navigator)
    const hasLocation = computed(() => coords.value !== null)

    const getCurrentPosition = async (): Promise<GeolocationCoords | null> => {
        if (!isSupported.value) {
            error.value = 'المتصفح لا يدعم تحديد الموقع'
            return null
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

            coords.value = {
                lat: position.coords.latitude,
                lng: position.coords.longitude,
                accuracy: position.coords.accuracy
            }

            return coords.value
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
            return null
        } finally {
            loading.value = false
        }
    }

    const clearLocation = () => {
        coords.value = null
        error.value = null
    }

    const getGoogleMapsUrl = (location?: GeolocationCoords | null) => {
        const loc = location ?? coords.value
        if (!loc) return ''
        return `https://www.google.com/maps?q=${loc.lat},${loc.lng}`
    }

    return {
        loading,
        error,
        coords,
        isSupported,
        hasLocation,
        getCurrentPosition,
        clearLocation,
        getGoogleMapsUrl
    }
}
