# Phase 4: Self-Registration System

## Weeks 9-10 | GPS Capture & Admin Review

---

## Phase Overview

| Attribute | Details |
|-----------|---------|
| **Duration** | 2 Weeks |
| **Start Date** | _TBD_ |
| **End Date** | _TBD_ |
| **Status** | 🔴 Not Started |
| **Progress** | 0% |
| **Dependencies** | Phase 1, 2 & 3 Complete |

---

## Objectives

1. Create public-facing student registration form (mobile-optimized)
2. Integrate with central database view for student data pre-fill
3. Implement GPS location capture with map picker
4. Build admin review dashboard for registration requests
5. Automate student creation and bus assignment on approval
6. Implement notification system for request status updates

---

## Sprint 4.1: Student Self-Registration (Week 9)

### Backend Tasks

| Task | Status | Assignee | Notes |
|------|--------|----------|-------|
| Implement RegistrationRequest entity | ⬜ Pending | | Request model with all fields |
| Create public registration endpoint (no auth) | ⬜ Pending | | `/api/registration/public` |
| Integrate with central view for student data | ⬜ Pending | | Pre-fetch by registration number |
| Implement GPS coordinates storage | ⬜ Pending | | Latitude/Longitude validation |
| Create validation rules | ⬜ Pending | | Required fields, format checks |
| Add duplicate request detection | ⬜ Pending | | Prevent multiple pending requests |
| Implement rate limiting | ⬜ Pending | | Prevent abuse |

### Frontend Tasks

| Task | Status | Assignee | Notes |
|------|--------|----------|-------|
| Build public registration form (mobile-optimized) | ⬜ Pending | | Large touch targets |
| Implement registration number lookup | ⬜ Pending | | Fetch from central system |
| Display read-only student data | ⬜ Pending | | Pre-filled, non-editable |
| Implement GPS location capture | ⬜ Pending | | HTML5 Geolocation API |
| Create map-based location picker | ⬜ Pending | | Leaflet.js integration |
| Add district selector | ⬜ Pending | | Dropdown from districts |
| Form validation and submission | ⬜ Pending | | Client-side validation |
| Create success/error feedback | ⬜ Pending | | Clear status messages |

### Deliverables Checklist

- [ ] Public registration form accessible without login
- [ ] Student data fetched from central system by reg number
- [ ] GPS capture working on mobile devices
- [ ] Map picker allows manual adjustment
- [ ] District selection functional
- [ ] Form validates all required fields
- [ ] Submission creates pending request

---

## Sprint 4.2: Admin Review & Approval (Week 10)

### Backend Tasks

| Task | Status | Assignee | Notes |
|------|--------|----------|-------|
| Create registration review service | ⬜ Pending | | Review business logic |
| Implement approve/reject workflow | ⬜ Pending | | Status transitions |
| Create automatic student record on approval | ⬜ Pending | | Student creation |
| Implement bus assignment logic | ⬜ Pending | | District-based suggestions |
| Add audit trail for reviews | ⬜ Pending | | Log all actions |
| Create notification service | ⬜ Pending | | Email/SMS (optional) |
| Implement bulk approval | ⬜ Pending | | Approve multiple requests |

### Frontend Tasks

| Task | Status | Assignee | Notes |
|------|--------|----------|-------|
| Create pending requests dashboard | ⬜ Pending | | Filterable list view |
| Build request review interface | ⬜ Pending | | Detailed request view |
| Show student location on map | ⬜ Pending | | Map with marker |
| Implement approve/reject actions | ⬜ Pending | | Action buttons |
| Add bus assignment during approval | ⬜ Pending | | Bus selector |
| Create rejection reason dialog | ⬜ Pending | | Mandatory reason |
| Build request status indicators | ⬜ Pending | | Pending/Approved/Rejected |
| Implement district-based filtering | ⬜ Pending | | Filter by district |

### Deliverables Checklist

- [ ] Admin can view all pending requests
- [ ] Request details displayed with map
- [ ] Approve action creates student + assignment
- [ ] Reject action requires reason
- [ ] Bus assignment happens during approval
- [ ] All actions audited
- [ ] Filters work correctly

---

## Technical Specifications

### Database Schema (Phase 4)

```sql
-- Registration Requests Table
CREATE TABLE RegistrationRequests (
    RequestId UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    
    -- From Central View (Pre-filled)
    ExternalStudentId NVARCHAR(50),
    StudentName NVARCHAR(200) NOT NULL,
    RegistrationNumber NVARCHAR(50) NOT NULL,
    HalaqaType NVARCHAR(100),
    TeacherName NVARCHAR(200),
    PhoneNumber NVARCHAR(20),
    
    -- Transport Request Details
    DistrictId UNIQUEIDENTIFIER NOT NULL,
    HomeAddress NVARCHAR(500),
    Latitude DECIMAL(10, 8) NOT NULL,
    Longitude DECIMAL(11, 8) NOT NULL,
    
    -- Request Status
    Status NVARCHAR(50) NOT NULL DEFAULT 'Pending', -- Pending, Approved, Rejected
    
    -- Review Information
    ReviewedAt DATETIME2,
    ReviewedBy UNIQUEIDENTIFIER,
    ReviewNotes NVARCHAR(1000),
    RejectionReason NVARCHAR(500),
    
    -- Assignment (if approved)
    AssignedBusId UNIQUEIDENTIFIER,
    CreatedStudentId UNIQUEIDENTIFIER,
    
    -- Audit
    RequestedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    IpAddress NVARCHAR(50),
    UserAgent NVARCHAR(500),
    
    CONSTRAINT FK_RegRequest_District FOREIGN KEY (DistrictId) REFERENCES Districts(DistrictId),
    CONSTRAINT FK_RegRequest_ReviewedBy FOREIGN KEY (ReviewedBy) REFERENCES Users(UserId),
    CONSTRAINT FK_RegRequest_AssignedBus FOREIGN KEY (AssignedBusId) REFERENCES Buses(BusId),
    CONSTRAINT FK_RegRequest_CreatedStudent FOREIGN KEY (CreatedStudentId) REFERENCES Students(StudentId),
    
    INDEX IX_RegRequest_Status (Status),
    INDEX IX_RegRequest_District (DistrictId),
    INDEX IX_RegRequest_RegistrationNumber (RegistrationNumber)
);
```

### Entity Models

```csharp
// RegistrationRequest Entity
public class RegistrationRequest : BaseEntity
{
    // Central System Data (Pre-filled)
    public string ExternalStudentId { get; set; }
    public string StudentName { get; set; }
    public string RegistrationNumber { get; set; }
    public string HalaqaType { get; set; }
    public string TeacherName { get; set; }
    public string PhoneNumber { get; set; }
    
    // Transport Request
    public Guid DistrictId { get; set; }
    public District District { get; set; }
    public string HomeAddress { get; set; }
    public decimal Latitude { get; set; }
    public decimal Longitude { get; set; }
    
    // Status
    public RegistrationStatus Status { get; set; } = RegistrationStatus.Pending;
    
    // Review
    public DateTime? ReviewedAt { get; set; }
    public Guid? ReviewedBy { get; set; }
    public User Reviewer { get; set; }
    public string ReviewNotes { get; set; }
    public string RejectionReason { get; set; }
    
    // Result
    public Guid? AssignedBusId { get; set; }
    public Bus AssignedBus { get; set; }
    public Guid? CreatedStudentId { get; set; }
    public Student CreatedStudent { get; set; }
    
    // Audit
    public DateTime RequestedAt { get; set; } = DateTime.UtcNow;
    public string IpAddress { get; set; }
    public string UserAgent { get; set; }
}

public enum RegistrationStatus
{
    Pending,
    Approved,
    Rejected
}
```

### DTOs

```csharp
// Registration Request DTOs
public class CreateRegistrationRequestDto
{
    public string RegistrationNumber { get; set; }
    public Guid DistrictId { get; set; }
    public string HomeAddress { get; set; }
    public decimal Latitude { get; set; }
    public decimal Longitude { get; set; }
}

public class RegistrationRequestDto
{
    public Guid RequestId { get; set; }
    public string StudentName { get; set; }
    public string RegistrationNumber { get; set; }
    public string HalaqaType { get; set; }
    public string TeacherName { get; set; }
    public string PhoneNumber { get; set; }
    public string DistrictName { get; set; }
    public string HomeAddress { get; set; }
    public decimal Latitude { get; set; }
    public decimal Longitude { get; set; }
    public string Status { get; set; }
    public DateTime RequestedAt { get; set; }
}

public class ReviewRegistrationDto
{
    public bool IsApproved { get; set; }
    public string Notes { get; set; }
    public string RejectionReason { get; set; } // Required if not approved
    public Guid? BusId { get; set; } // Required if approved
}

public class StudentLookupDto
{
    public string ExternalStudentId { get; set; }
    public string StudentName { get; set; }
    public string RegistrationNumber { get; set; }
    public string HalaqaType { get; set; }
    public string TeacherName { get; set; }
    public string PhoneNumber { get; set; }
    public bool IsEligible { get; set; }
    public string EligibilityMessage { get; set; }
}
```

---

## API Endpoints (Phase 4)

### Public Registration

| Method | Endpoint | Description | Auth |
|--------|----------|-------------|------|
| GET | `/api/registration/lookup/{regNumber}` | Lookup student from central | No |
| POST | `/api/registration/public` | Submit registration request | No |
| GET | `/api/registration/status/{regNumber}` | Check request status | No |

### Admin Review

| Method | Endpoint | Description | Auth |
|--------|----------|-------------|------|
| GET | `/api/registration/requests` | List all requests (paginated) | Staff |
| GET | `/api/registration/requests/pending` | List pending requests | Staff |
| GET | `/api/registration/requests/{id}` | Get request details | Staff |
| POST | `/api/registration/requests/{id}/review` | Approve/Reject request | Staff |
| POST | `/api/registration/requests/bulk-approve` | Bulk approve requests | Staff |
| GET | `/api/registration/requests/by-district/{id}` | Requests by district | Staff |
| GET | `/api/registration/statistics` | Registration stats | Staff |

### Bus Suggestions

| Method | Endpoint | Description | Auth |
|--------|----------|-------------|------|
| GET | `/api/registration/bus-suggestions/{districtId}` | Suggest buses for district | Staff |

---

## Frontend Components

### Public Registration Form

```vue
<!-- StudentRegistrationView.vue -->
<template>
  <div class="registration-form min-h-screen bg-base-200 p-4">
    <div class="card bg-base-100 shadow-xl max-w-md mx-auto">
      <div class="card-body">
        <h2 class="card-title text-center">طلب تسجيل للنقل</h2>
        
        <!-- Step 1: Registration Number Lookup -->
        <div v-if="step === 1">
          <div class="form-control">
            <label class="label">رقم التسجيل</label>
            <input v-model="regNumber" 
                   type="text" 
                   class="input input-bordered input-lg"
                   placeholder="أدخل رقم التسجيل" />
          </div>
          <button class="btn btn-primary btn-lg w-full mt-4" 
                  @click="lookupStudent"
                  :disabled="loading">
            بحث
          </button>
        </div>
        
        <!-- Step 2: Confirm Data & Add Location -->
        <div v-if="step === 2">
          <!-- Read-only Student Data -->
          <div class="bg-base-200 p-4 rounded-lg mb-4">
            <h3 class="font-bold mb-2">بيانات الطالب</h3>
            <p><strong>الاسم:</strong> {{ studentData.studentName }}</p>
            <p><strong>الحلقة:</strong> {{ studentData.halaqaType }}</p>
            <p><strong>المعلم:</strong> {{ studentData.teacherName }}</p>
          </div>
          
          <!-- District Selection -->
          <div class="form-control">
            <label class="label">الحي</label>
            <select v-model="districtId" class="select select-bordered select-lg">
              <option value="">اختر الحي</option>
              <option v-for="d in districts" :value="d.id">{{ d.nameAr }}</option>
            </select>
          </div>
          
          <!-- GPS Location -->
          <div class="form-control mt-4">
            <label class="label">الموقع</label>
            <button class="btn btn-outline btn-lg" @click="captureLocation">
              📍 تحديد موقعي
            </button>
            <p v-if="location" class="text-sm mt-2">
              تم تحديد الموقع: {{ location.lat.toFixed(6) }}, {{ location.lng.toFixed(6) }}
            </p>
          </div>
          
          <!-- Map -->
          <div id="map" class="h-64 rounded-lg mt-4" v-if="location"></div>
          
          <button class="btn btn-primary btn-lg w-full mt-4" 
                  @click="submitRequest"
                  :disabled="!isFormValid">
            إرسال الطلب
          </button>
        </div>
        
        <!-- Step 3: Success -->
        <div v-if="step === 3" class="text-center">
          <div class="text-6xl mb-4">✅</div>
          <h3 class="text-xl font-bold">تم إرسال الطلب بنجاح</h3>
          <p class="mt-2">سيتم مراجعة طلبك والرد عليك قريباً</p>
        </div>
      </div>
    </div>
  </div>
</template>
```

### Admin Review Dashboard

```vue
<!-- PendingRequestsView.vue -->
<template>
  <div class="pending-requests">
    <div class="flex justify-between items-center mb-6">
      <h1 class="text-2xl font-bold">طلبات التسجيل المعلقة</h1>
      <div class="stats shadow">
        <div class="stat">
          <div class="stat-title">معلق</div>
          <div class="stat-value text-warning">{{ stats.pending }}</div>
        </div>
        <div class="stat">
          <div class="stat-title">مقبول</div>
          <div class="stat-value text-success">{{ stats.approved }}</div>
        </div>
        <div class="stat">
          <div class="stat-title">مرفوض</div>
          <div class="stat-value text-error">{{ stats.rejected }}</div>
        </div>
      </div>
    </div>
    
    <!-- Filters -->
    <div class="flex gap-4 mb-4">
      <select v-model="statusFilter" class="select select-bordered">
        <option value="">كل الحالات</option>
        <option value="Pending">معلق</option>
        <option value="Approved">مقبول</option>
        <option value="Rejected">مرفوض</option>
      </select>
      
      <select v-model="districtFilter" class="select select-bordered">
        <option value="">كل الأحياء</option>
        <option v-for="d in districts" :value="d.id">{{ d.nameAr }}</option>
      </select>
    </div>
    
    <!-- Requests Table -->
    <div class="overflow-x-auto">
      <table class="table">
        <thead>
          <tr>
            <th>الاسم</th>
            <th>رقم التسجيل</th>
            <th>الحي</th>
            <th>الحلقة</th>
            <th>تاريخ الطلب</th>
            <th>الحالة</th>
            <th>إجراءات</th>
          </tr>
        </thead>
        <tbody>
          <tr v-for="req in requests" :key="req.requestId">
            <td>{{ req.studentName }}</td>
            <td>{{ req.registrationNumber }}</td>
            <td>{{ req.districtName }}</td>
            <td>{{ req.halaqaType }}</td>
            <td>{{ formatDate(req.requestedAt) }}</td>
            <td>
              <span class="badge" :class="statusBadge(req.status)">
                {{ statusText(req.status) }}
              </span>
            </td>
            <td>
              <button class="btn btn-sm btn-primary" 
                      @click="reviewRequest(req)"
                      v-if="req.status === 'Pending'">
                مراجعة
              </button>
            </td>
          </tr>
        </tbody>
      </table>
    </div>
  </div>
</template>
```

### Review Modal

```vue
<!-- ReviewRequestModal.vue -->
<template>
  <dialog class="modal" :open="isOpen">
    <div class="modal-box max-w-3xl">
      <h3 class="font-bold text-lg">مراجعة طلب التسجيل</h3>
      
      <div class="grid grid-cols-2 gap-4 mt-4">
        <!-- Student Info -->
        <div>
          <h4 class="font-bold mb-2">بيانات الطالب</h4>
          <p><strong>الاسم:</strong> {{ request.studentName }}</p>
          <p><strong>رقم التسجيل:</strong> {{ request.registrationNumber }}</p>
          <p><strong>الحلقة:</strong> {{ request.halaqaType }}</p>
          <p><strong>المعلم:</strong> {{ request.teacherName }}</p>
          <p><strong>الحي:</strong> {{ request.districtName }}</p>
          <p><strong>العنوان:</strong> {{ request.homeAddress }}</p>
        </div>
        
        <!-- Map -->
        <div>
          <h4 class="font-bold mb-2">الموقع</h4>
          <div id="review-map" class="h-48 rounded-lg"></div>
        </div>
      </div>
      
      <!-- Bus Assignment -->
      <div class="form-control mt-4" v-if="isApproving">
        <label class="label">تعيين الباص</label>
        <select v-model="selectedBusId" class="select select-bordered">
          <option value="">اختر الباص</option>
          <option v-for="bus in suggestedBuses" :value="bus.busId">
            {{ bus.busNumber }} - {{ bus.routeName }} 
            ({{ bus.currentStudentCount }}/{{ bus.capacity }})
          </option>
        </select>
      </div>
      
      <!-- Notes / Rejection Reason -->
      <div class="form-control mt-4">
        <label class="label">{{ isApproving ? 'ملاحظات' : 'سبب الرفض' }}</label>
        <textarea v-model="notes" 
                  class="textarea textarea-bordered"
                  rows="2"
                  :required="!isApproving"></textarea>
      </div>
      
      <div class="modal-action">
        <button class="btn btn-success" @click="approve" :disabled="!canApprove">
          قبول
        </button>
        <button class="btn btn-error" @click="reject" :disabled="!canReject">
          رفض
        </button>
        <button class="btn" @click="close">إلغاء</button>
      </div>
    </div>
  </dialog>
</template>
```

---

## Acceptance Criteria

### Sprint 4.1
- [ ] Public form accessible at `/register`
- [ ] Registration number lookup returns student data
- [ ] Read-only fields cannot be modified
- [ ] GPS capture works on mobile
- [ ] Map shows and allows pin adjustment
- [ ] District selection required
- [ ] Form validation prevents incomplete submission
- [ ] Success message shown after submission

### Sprint 4.2
- [ ] Admin dashboard shows pending requests count
- [ ] Filters by status and district work
- [ ] Review modal shows all request details
- [ ] Map displays student location
- [ ] Bus suggestions based on district
- [ ] Approve creates student and assignment
- [ ] Reject requires reason
- [ ] All actions logged in audit

---

## Risks & Mitigations

| Risk | Impact | Mitigation |
|------|--------|------------|
| GPS accuracy in basements | Medium | Allow manual map pin adjustment |
| Central DB unavailable | High | Cache recent lookups, show error gracefully |
| Spam/abuse of public form | Medium | Rate limiting, CAPTCHA (optional) |
| Duplicate requests | Low | Detect existing pending requests |

---

## Achievement Tracking

### Week 9 Progress

| Category | Planned | Completed | Percentage |
|----------|---------|-----------|------------|
| Backend Tasks | 7 | 0 | 0% |
| Frontend Tasks | 8 | 0 | 0% |
| **Total** | **15** | **0** | **0%** |

### Week 10 Progress

| Category | Planned | Completed | Percentage |
|----------|---------|-----------|------------|
| Backend Tasks | 7 | 0 | 0% |
| Frontend Tasks | 8 | 0 | 0% |
| **Total** | **15** | **0** | **0%** |

---

## Phase Summary

### Overall Progress

```
Phase 4 Completion: ░░░░░░░░░░ 0%
```

| Sprint | Status | Completion |
|--------|--------|------------|
| Sprint 4.1 - Self-Registration | 🔴 Not Started | 0% |
| Sprint 4.2 - Admin Review | 🔴 Not Started | 0% |

### Key Milestones

- [ ] 🎯 Public registration form live
- [ ] 🎯 GPS capture functional
- [ ] 🎯 Admin review dashboard complete
- [ ] 🎯 Auto student creation on approval

---

## Notes & Decisions

### Decision Log

| Date | Decision | Rationale |
|------|----------|-----------|
| | | |

### Blockers

| Issue | Status | Resolution |
|-------|--------|------------|
| | | |

---

**Phase Owner:** _TBD_  
**Last Updated:** _TBD_  
**Next Review:** _TBD_
