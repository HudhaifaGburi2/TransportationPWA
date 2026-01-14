<template>
  <div class="p-6 space-y-6">
    <!-- Page Header -->
    <div class="bg-gradient-to-l from-warning/10 to-transparent p-6 rounded-xl">
      <div class="flex flex-col md:flex-row md:items-center md:justify-between gap-4">
        <div>
          <h1 class="text-2xl font-bold text-base-content flex items-center gap-3">
            <div class="p-2 bg-warning/20 rounded-lg">
              <FileText class="w-6 h-6 text-warning" />
            </div>
            طلبات التسجيل
          </h1>
          <p class="text-base-content/60 mt-1">إدارة طلبات تسجيل الطلاب في نظام النقل</p>
        </div>
        <div class="flex gap-2">
          <button @click="refreshRequests" class="btn btn-ghost gap-2">
            <RefreshCw class="w-5 h-5" :class="{ 'animate-spin': loading }" />
            تحديث
          </button>
        </div>
      </div>
    </div>

    <!-- Stats Summary -->
    <div class="grid grid-cols-2 md:grid-cols-4 gap-4">
      <div class="stat bg-base-100 rounded-xl shadow-sm border border-base-200">
        <div class="stat-figure text-primary">
          <FileText class="w-8 h-8" />
        </div>
        <div class="stat-title">إجمالي الطلبات</div>
        <div class="stat-value text-primary">{{ requests.length }}</div>
      </div>
      <div class="stat bg-base-100 rounded-xl shadow-sm border border-base-200">
        <div class="stat-figure text-warning">
          <Clock class="w-8 h-8" />
        </div>
        <div class="stat-title">قيد الانتظار</div>
        <div class="stat-value text-warning">{{ pendingCount }}</div>
      </div>
      <div class="stat bg-base-100 rounded-xl shadow-sm border border-base-200">
        <div class="stat-figure text-success">
          <CheckCircle class="w-8 h-8" />
        </div>
        <div class="stat-title">مقبولة</div>
        <div class="stat-value text-success">{{ approvedCount }}</div>
      </div>
      <div class="stat bg-base-100 rounded-xl shadow-sm border border-base-200">
        <div class="stat-figure text-error">
          <XCircle class="w-8 h-8" />
        </div>
        <div class="stat-title">مرفوضة</div>
        <div class="stat-value text-error">{{ rejectedCount }}</div>
      </div>
    </div>

    <!-- Filters -->
    <div class="bg-base-100 p-4 rounded-xl shadow-sm border border-base-200">
      <div class="flex flex-col md:flex-row gap-4">
        <div class="form-control flex-1">
          <div class="input-group">
            <span class="bg-base-200">
              <Search class="w-5 h-5 text-base-content/60" />
            </span>
            <input
              v-model="searchQuery"
              type="text"
              placeholder="بحث باسم الطالب أو رقم الهوية..."
              class="input input-bordered flex-1"
            />
          </div>
        </div>
        <select v-model="filterStatus" class="select select-bordered w-full md:w-48">
          <option value="all">جميع الحالات</option>
          <option value="pending">قيد الانتظار</option>
          <option value="approved">مقبولة</option>
          <option value="rejected">مرفوضة</option>
        </select>
      </div>
    </div>

    <!-- Requests Table -->
    <div class="bg-base-100 rounded-xl shadow-sm border border-base-200 overflow-hidden">
      <div v-if="loading" class="flex justify-center py-16">
        <span class="loading loading-spinner loading-lg text-primary"></span>
      </div>

      <div v-else-if="filteredRequests.length === 0" class="text-center py-16">
        <div class="text-base-content/30 mb-4">
          <FileText class="w-16 h-16 mx-auto" />
        </div>
        <p class="text-base-content/60 text-lg">لا توجد طلبات تسجيل</p>
        <p class="text-base-content/40 text-sm mt-1">سيظهر هنا طلبات الطلاب الجدد</p>
      </div>

      <div v-else class="overflow-x-auto">
        <table class="table table-zebra">
          <thead class="bg-base-200">
            <tr>
              <th class="font-bold">#</th>
              <th class="font-bold">اسم الطالب</th>
              <th class="font-bold">رقم الهوية</th>
              <th class="font-bold">المنطقة</th>
              <th class="font-bold">الفترة</th>
              <th class="font-bold text-center">تاريخ الطلب</th>
              <th class="font-bold text-center">الحالة</th>
              <th class="font-bold text-center">الإجراءات</th>
            </tr>
          </thead>
          <tbody>
            <tr v-for="(request, index) in filteredRequests" :key="request.id" class="hover">
              <td class="font-medium">{{ index + 1 }}</td>
              <td>
                <div class="flex items-center gap-3">
                  <div class="avatar placeholder">
                    <div class="bg-primary/10 text-primary rounded-full w-10">
                      <span class="text-sm">{{ getInitials(request.studentName) }}</span>
                    </div>
                  </div>
                  <div>
                    <div class="font-semibold">{{ request.studentName }}</div>
                    <div class="text-xs text-base-content/60">{{ request.studentId }}</div>
                  </div>
                </div>
              </td>
              <td dir="ltr" class="font-mono">{{ request.nationalId }}</td>
              <td>{{ request.districtName }}</td>
              <td>{{ request.periodName }}</td>
              <td class="text-center text-sm">{{ formatDate(request.createdAt) }}</td>
              <td class="text-center">
                <span :class="getStatusBadgeClass(request.status)">
                  {{ getStatusText(request.status) }}
                </span>
              </td>
              <td class="text-center">
                <div v-if="request.status === 'pending'" class="flex items-center justify-center gap-1">
                  <button 
                    @click="approveRequest(request)" 
                    class="btn btn-success btn-sm gap-1"
                    :disabled="processing"
                  >
                    <Check class="w-4 h-4" />
                    قبول
                  </button>
                  <button 
                    @click="showRejectModal(request)" 
                    class="btn btn-error btn-sm gap-1"
                    :disabled="processing"
                  >
                    <X class="w-4 h-4" />
                    رفض
                  </button>
                </div>
                <div v-else class="flex items-center justify-center gap-1">
                  <button 
                    @click="viewDetails(request)" 
                    class="btn btn-ghost btn-sm btn-square tooltip" 
                    data-tip="عرض التفاصيل"
                  >
                    <Eye class="w-4 h-4" />
                  </button>
                </div>
              </td>
            </tr>
          </tbody>
        </table>
      </div>
    </div>

    <!-- Reject Modal -->
    <dialog :open="showRejectDialog" class="modal modal-open">
      <div class="modal-box max-w-md">
        <button @click="showRejectDialog = false" class="btn btn-sm btn-circle btn-ghost absolute left-2 top-2">✕</button>
        <div class="text-center mb-6">
          <div class="w-16 h-16 bg-error/10 rounded-full flex items-center justify-center mx-auto mb-3">
            <XCircle class="w-8 h-8 text-error" />
          </div>
          <h3 class="font-bold text-xl">رفض الطلب</h3>
          <p class="text-base-content/60 text-sm mt-1">سيتم إخطار الطالب بسبب الرفض</p>
        </div>
        <div class="form-control mb-4">
          <label class="label">
            <span class="label-text font-medium">سبب الرفض <span class="text-error">*</span></span>
          </label>
          <textarea 
            v-model="rejectReason" 
            class="textarea textarea-bordered h-24" 
            placeholder="أدخل سبب رفض الطلب..."
            required
          ></textarea>
        </div>
        <div class="flex gap-3">
          <button class="btn btn-ghost flex-1" @click="showRejectDialog = false">إلغاء</button>
          <button 
            class="btn btn-error flex-1 gap-2" 
            @click="confirmReject"
            :disabled="!rejectReason.trim() || processing"
          >
            <span v-if="processing" class="loading loading-spinner loading-sm"></span>
            تأكيد الرفض
          </button>
        </div>
      </div>
      <div class="modal-backdrop bg-black/50" @click="showRejectDialog = false"></div>
    </dialog>

    <!-- Details Modal -->
    <dialog :open="showDetailsDialog" class="modal modal-open">
      <div class="modal-box max-w-lg">
        <button @click="showDetailsDialog = false" class="btn btn-sm btn-circle btn-ghost absolute left-2 top-2">✕</button>
        <h3 class="font-bold text-xl mb-4">تفاصيل الطلب</h3>
        <div v-if="selectedRequest" class="space-y-4">
          <div class="grid grid-cols-2 gap-4">
            <div>
              <label class="text-sm text-base-content/60">اسم الطالب</label>
              <p class="font-semibold">{{ selectedRequest.studentName }}</p>
            </div>
            <div>
              <label class="text-sm text-base-content/60">رقم الهوية</label>
              <p class="font-mono" dir="ltr">{{ selectedRequest.nationalId }}</p>
            </div>
            <div>
              <label class="text-sm text-base-content/60">المنطقة</label>
              <p>{{ selectedRequest.districtName }}</p>
            </div>
            <div>
              <label class="text-sm text-base-content/60">الفترة</label>
              <p>{{ selectedRequest.periodName }}</p>
            </div>
            <div>
              <label class="text-sm text-base-content/60">تاريخ الطلب</label>
              <p>{{ formatDate(selectedRequest.createdAt) }}</p>
            </div>
            <div>
              <label class="text-sm text-base-content/60">الحالة</label>
              <p><span :class="getStatusBadgeClass(selectedRequest.status)">{{ getStatusText(selectedRequest.status) }}</span></p>
            </div>
          </div>
          <div v-if="selectedRequest.rejectionReason" class="bg-error/10 p-4 rounded-lg">
            <label class="text-sm text-error font-medium">سبب الرفض</label>
            <p class="mt-1">{{ selectedRequest.rejectionReason }}</p>
          </div>
        </div>
        <div class="modal-action">
          <button class="btn" @click="showDetailsDialog = false">إغلاق</button>
        </div>
      </div>
      <div class="modal-backdrop bg-black/50" @click="showDetailsDialog = false"></div>
    </dialog>
  </div>
</template>

<script setup lang="ts">
import { ref, computed, onMounted } from 'vue'
import { FileText, Search, RefreshCw, Clock, CheckCircle, XCircle, Check, X, Eye } from 'lucide-vue-next'

interface RegistrationRequest {
  id: string
  studentId: string
  studentName: string
  nationalId: string
  districtId: string
  districtName: string
  periodId: number
  periodName: string
  status: 'pending' | 'approved' | 'rejected'
  createdAt: string
  rejectionReason?: string
}

const requests = ref<RegistrationRequest[]>([])
const loading = ref(false)
const processing = ref(false)
const searchQuery = ref('')
const filterStatus = ref('all')

const showRejectDialog = ref(false)
const showDetailsDialog = ref(false)
const selectedRequest = ref<RegistrationRequest | null>(null)
const rejectReason = ref('')

// Computed stats
const pendingCount = computed(() => requests.value.filter(r => r.status === 'pending').length)
const approvedCount = computed(() => requests.value.filter(r => r.status === 'approved').length)
const rejectedCount = computed(() => requests.value.filter(r => r.status === 'rejected').length)

const filteredRequests = computed(() => {
  let result = requests.value
  
  if (searchQuery.value) {
    const query = searchQuery.value.toLowerCase()
    result = result.filter(r => 
      r.studentName.toLowerCase().includes(query) ||
      r.nationalId.includes(query)
    )
  }
  
  if (filterStatus.value !== 'all') {
    result = result.filter(r => r.status === filterStatus.value)
  }
  
  return result
})

const fetchRequests = async () => {
  loading.value = true
  try {
    // TODO: Replace with actual API call
    // const response = await apiClient.get('/api/v1/registration-requests')
    // requests.value = response.data.data
    
    // Mock data for now
    requests.value = [
      {
        id: '1',
        studentId: 'STU001',
        studentName: 'أحمد محمد العلي',
        nationalId: '1234567890',
        districtId: 'd1',
        districtName: 'الرياض',
        periodId: 1,
        periodName: 'الفترة الأولى',
        status: 'pending',
        createdAt: new Date().toISOString()
      },
      {
        id: '2',
        studentId: 'STU002',
        studentName: 'خالد عبدالله السعيد',
        nationalId: '0987654321',
        districtId: 'd2',
        districtName: 'جدة',
        periodId: 2,
        periodName: 'الفترة الثانية',
        status: 'approved',
        createdAt: new Date(Date.now() - 86400000).toISOString()
      },
      {
        id: '3',
        studentId: 'STU003',
        studentName: 'محمد سعد القحطاني',
        nationalId: '1122334455',
        districtId: 'd1',
        districtName: 'الرياض',
        periodId: 1,
        periodName: 'الفترة الأولى',
        status: 'rejected',
        createdAt: new Date(Date.now() - 172800000).toISOString(),
        rejectionReason: 'عدم استيفاء الشروط المطلوبة'
      }
    ]
  } catch (e) {
    console.error('Error fetching requests:', e)
  } finally {
    loading.value = false
  }
}

const refreshRequests = () => {
  fetchRequests()
}

const getInitials = (name: string) => {
  return name.split(' ').map(n => n[0]).slice(0, 2).join('')
}

const formatDate = (dateStr: string) => {
  return new Date(dateStr).toLocaleDateString('ar-SA', {
    year: 'numeric',
    month: 'short',
    day: 'numeric'
  })
}

const getStatusText = (status: string) => {
  switch (status) {
    case 'pending': return 'قيد الانتظار'
    case 'approved': return 'مقبول'
    case 'rejected': return 'مرفوض'
    default: return status
  }
}

const getStatusBadgeClass = (status: string) => {
  switch (status) {
    case 'pending': return 'badge badge-warning'
    case 'approved': return 'badge badge-success'
    case 'rejected': return 'badge badge-error'
    default: return 'badge'
  }
}

const approveRequest = async (request: RegistrationRequest) => {
  processing.value = true
  try {
    // TODO: API call to approve
    // await apiClient.post(`/api/v1/registration-requests/${request.id}/approve`)
    request.status = 'approved'
  } catch (e) {
    console.error('Error approving request:', e)
  } finally {
    processing.value = false
  }
}

const showRejectModal = (request: RegistrationRequest) => {
  selectedRequest.value = request
  rejectReason.value = ''
  showRejectDialog.value = true
}

const confirmReject = async () => {
  if (!selectedRequest.value || !rejectReason.value.trim()) return
  
  processing.value = true
  try {
    // TODO: API call to reject
    // await apiClient.post(`/api/v1/registration-requests/${selectedRequest.value.id}/reject`, { reason: rejectReason.value })
    selectedRequest.value.status = 'rejected'
    selectedRequest.value.rejectionReason = rejectReason.value
    showRejectDialog.value = false
  } catch (e) {
    console.error('Error rejecting request:', e)
  } finally {
    processing.value = false
  }
}

const viewDetails = (request: RegistrationRequest) => {
  selectedRequest.value = request
  showDetailsDialog.value = true
}

onMounted(fetchRequests)
</script>
