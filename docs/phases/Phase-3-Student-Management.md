# Phase 3: Student Management

## Weeks 6-8 | Central Integration, Assignments & Lifecycle

---

## Phase Overview

| Attribute | Details |
|-----------|---------|
| **Duration** | 3 Weeks |
| **Start Date** | _TBD_ |
| **End Date** | _TBD_ |
| **Status** | 🔴 Not Started |
| **Progress** | 0% |
| **Dependencies** | Phase 1 & 2 Complete |

---

## Objectives

1. Integrate with central database view for student data
2. Implement complete student entity management
3. Create student-bus assignment system with split transport support
4. Build student lifecycle management (suspension, reactivation, leave)
5. Implement student transfer functionality
6. Create student history and timeline views

---

## Sprint 3.1: Student Registration from Central System (Week 6)

### Backend Tasks

| Task | Status | Assignee | Notes |
|------|--------|----------|-------|
| Create integration service for central DB view | ⬜ Pending | | `vw_Student_Halaqa_Teacher_information_Transportation_Dep` |
| Implement Student entity and repository | ⬜ Pending | | Full student model |
| Create StudentService | ⬜ Pending | | Business logic layer |
| Implement student search (by name, reg number, phone) | ⬜ Pending | | Multiple search criteria |
| Create StudentsController | ⬜ Pending | | REST API endpoints |
| Add external ID mapping | ⬜ Pending | | Link to central system |
| Implement data sync strategy | ⬜ Pending | | How to handle updates |

### Frontend Tasks

| Task | Status | Assignee | Notes |
|------|--------|----------|-------|
| Build student search interface | ⬜ Pending | | Advanced search form |
| Create student detail view | ⬜ Pending | | Full student info |
| Display Halaqa/Teacher information | ⬜ Pending | | From central system |
| Show student status badges | ⬜ Pending | | Active/Suspended/On Leave |
| Create student store (Pinia) | ⬜ Pending | | State management |
| Build student list view | ⬜ Pending | | With filters |
| Implement search results display | ⬜ Pending | | Paginated results |

### Deliverables Checklist

- [ ] Central DB view integration working
- [ ] Student data synced from central system
- [ ] Search returns accurate results
- [ ] Student details display correctly
- [ ] External ID mapping functional
- [ ] Halaqa/Teacher info visible

---

## Sprint 3.2: Student-Bus Assignment (Week 7)

### Backend Tasks

| Task | Status | Assignee | Notes |
|------|--------|----------|-------|
| Implement StudentBusAssignment entity | ⬜ Pending | | Assignment junction table |
| Create assignment service with validation | ⬜ Pending | | Business rules |
| Support different arrival/return buses | ⬜ Pending | | Split transport feature |
| Implement transfer functionality | ⬜ Pending | | Move between buses |
| Add capacity validation | ⬜ Pending | | Prevent overloading |
| Create StudentTransfer entity | ⬜ Pending | | Transfer history |
| Implement assignment history | ⬜ Pending | | Track changes |

### Frontend Tasks

| Task | Status | Assignee | Notes |
|------|--------|----------|-------|
| Create student assignment interface | ⬜ Pending | | Assign to bus UI |
| Build bus student list view | ⬜ Pending | | Students per bus |
| Implement transfer wizard | ⬜ Pending | | Multi-step transfer |
| Visual indicators for split transport | ⬜ Pending | | Arrival ≠ Return |
| Show warnings for capacity | ⬜ Pending | | Alert when full |
| Create drag-and-drop assignment | ⬜ Pending | | Optional UX enhancement |
| Build assignment confirmation dialog | ⬜ Pending | | Confirm actions |

### Deliverables Checklist

- [ ] Students can be assigned to buses
- [ ] Different arrival/return buses supported
- [ ] Transfer between buses works
- [ ] Capacity limits enforced
- [ ] Split transport visually distinct
- [ ] Assignment history tracked

---

## Sprint 3.3: Student Lifecycle Management (Week 8)

### Backend Tasks

| Task | Status | Assignee | Notes |
|------|--------|----------|-------|
| Implement StudentSuspension entity and service | ⬜ Pending | | Suspension model |
| Create suspension workflow | ⬜ Pending | | Business logic |
| Implement reactivation process | ⬜ Pending | | Return to active |
| Create StudentLeave entity and management | ⬜ Pending | | Leave records |
| Implement StudentTransfer history | ⬜ Pending | | Transfer log |
| Create lifecycle timeline endpoint | ⬜ Pending | | Student history |
| Add attachment support for leave | ⬜ Pending | | File uploads |

### Frontend Tasks

| Task | Status | Assignee | Notes |
|------|--------|----------|-------|
| Create suspended students list view | ⬜ Pending | | Filtered list |
| Build suspension dialog with reason | ⬜ Pending | | Default: "3 days absent" |
| Implement reactivation workflow | ⬜ Pending | | Select new/old bus |
| Create leave management interface | ⬜ Pending | | Date range, attachment |
| Display student history timeline | ⬜ Pending | | Visual timeline |
| Build status change confirmation | ⬜ Pending | | Confirm dialogs |
| Create leave calendar view | ⬜ Pending | | Optional enhancement |

### Deliverables Checklist

- [ ] Students can be suspended with reason
- [ ] Suspended students list accessible
- [ ] Reactivation restores/changes bus
- [ ] Leave records created with dates
- [ ] Attachments uploaded for leave
- [ ] Student timeline shows history
- [ ] Status changes audited

---

## Technical Specifications

### Database Schema (Phase 3)

```sql
-- Students Table
CREATE TABLE Students (
    StudentId UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    
    -- From Central View (Read-Only Reference)
    ExternalUserId INT,
    ExternalStudentId NVARCHAR(50),
    
    -- Student Information
    FullNameAr NVARCHAR(200) NOT NULL,
    FullNameEn NVARCHAR(200),
    RegistrationNumber NVARCHAR(50) NOT NULL UNIQUE,
    PhoneNumber NVARCHAR(20),
    
    -- Halaqa Information (from central view)
    HalaqaType NVARCHAR(100),
    Halaqa_section_Id UNIQUEIDENTIFIER,
    TeacherName NVARCHAR(200),
    ClassCircleNumber NVARCHAR(50),
    
    -- Transport-Specific Information
    DistrictId UNIQUEIDENTIFIER NOT NULL,
    HomeAddress NVARCHAR(500),
    Latitude DECIMAL(10, 8),
    Longitude DECIMAL(11, 8),
    
    -- Status
    Status NVARCHAR(50) NOT NULL DEFAULT 'Active',
    
    -- Audit
    CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    UpdatedAt DATETIME2,
    CreatedBy UNIQUEIDENTIFIER,
    UpdatedBy UNIQUEIDENTIFIER,
    IsDeleted BIT NOT NULL DEFAULT 0,
    
    CONSTRAINT FK_Students_Districts FOREIGN KEY (DistrictId) REFERENCES Districts(DistrictId),
    
    INDEX IX_Students_Status (Status),
    INDEX IX_Students_District (DistrictId),
    INDEX IX_Students_RegistrationNumber (RegistrationNumber)
);

-- Student-Bus Assignments
CREATE TABLE StudentBusAssignments (
    AssignmentId UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    StudentId UNIQUEIDENTIFIER NOT NULL,
    BusId UNIQUEIDENTIFIER NOT NULL,
    
    TransportType NVARCHAR(50) NOT NULL, -- Arrival, Return, Both
    ArrivalBusId UNIQUEIDENTIFIER,
    ReturnBusId UNIQUEIDENTIFIER,
    
    IsActive BIT NOT NULL DEFAULT 1,
    AssignedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    AssignedBy UNIQUEIDENTIFIER,
    
    CONSTRAINT FK_StudentBus_Student FOREIGN KEY (StudentId) REFERENCES Students(StudentId),
    CONSTRAINT FK_StudentBus_Bus FOREIGN KEY (BusId) REFERENCES Buses(BusId),
    
    INDEX IX_StudentBus_Student (StudentId),
    INDEX IX_StudentBus_Bus (BusId)
);

-- Student Suspensions
CREATE TABLE StudentSuspensions (
    SuspensionId UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    StudentId UNIQUEIDENTIFIER NOT NULL,
    BusId UNIQUEIDENTIFIER,
    
    Reason NVARCHAR(500) NOT NULL DEFAULT N'غياب لمدة ثلاثة أيام متتالية',
    SuspendedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    SuspendedBy UNIQUEIDENTIFIER NOT NULL,
    
    IsReactivated BIT NOT NULL DEFAULT 0,
    ReactivatedAt DATETIME2,
    ReactivatedBy UNIQUEIDENTIFIER,
    ReactivatedToBusId UNIQUEIDENTIFIER,
    
    CONSTRAINT FK_Suspension_Student FOREIGN KEY (StudentId) REFERENCES Students(StudentId),
    
    INDEX IX_Suspension_Student (StudentId),
    INDEX IX_Suspension_IsReactivated (IsReactivated)
);

-- Student Leaves
CREATE TABLE StudentLeaves (
    LeaveId UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    StudentId UNIQUEIDENTIFIER NOT NULL,
    
    Reason NVARCHAR(500) NOT NULL,
    AttachmentUrl NVARCHAR(500),
    
    StartDate DATE NOT NULL,
    EndDate DATE NOT NULL,
    
    RequestedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    RequestedBy UNIQUEIDENTIFIER NOT NULL,
    ApprovedAt DATETIME2,
    ApprovedBy UNIQUEIDENTIFIER,
    
    Status NVARCHAR(50) NOT NULL DEFAULT 'Active',
    
    CONSTRAINT FK_Leave_Student FOREIGN KEY (StudentId) REFERENCES Students(StudentId),
    
    INDEX IX_Leave_Student (StudentId),
    INDEX IX_Leave_Dates (StartDate, EndDate)
);

-- Student Transfers
CREATE TABLE StudentTransfers (
    TransferId UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    StudentId UNIQUEIDENTIFIER NOT NULL,
    
    FromBusId UNIQUEIDENTIFIER NOT NULL,
    ToBusId UNIQUEIDENTIFIER NOT NULL,
    
    Reason NVARCHAR(500),
    TransferredAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    TransferredBy UNIQUEIDENTIFIER NOT NULL,
    
    CONSTRAINT FK_Transfer_Student FOREIGN KEY (StudentId) REFERENCES Students(StudentId),
    
    INDEX IX_Transfer_Student (StudentId)
);
```

### Entity Models

```csharp
// Student Entity
public class Student : BaseEntity, IAuditable, ISoftDelete
{
    // External References
    public int? ExternalUserId { get; set; }
    public string ExternalStudentId { get; set; }
    
    // Student Information
    public string FullNameAr { get; set; }
    public string FullNameEn { get; set; }
    public string RegistrationNumber { get; set; }
    public string PhoneNumber { get; set; }
    
    // Halaqa Information
    public string HalaqaType { get; set; }
    public string TeacherName { get; set; }
    public string ClassCircleNumber { get; set; }
    
    // Transport Information
    public Guid DistrictId { get; set; }
    public District District { get; set; }
    public string HomeAddress { get; set; }
    public decimal? Latitude { get; set; }
    public decimal? Longitude { get; set; }
    
    // Status
    public StudentStatus Status { get; set; } = StudentStatus.Active;
    
    // Navigation
    public ICollection<StudentBusAssignment> BusAssignments { get; set; }
    public ICollection<StudentSuspension> Suspensions { get; set; }
    public ICollection<StudentLeave> Leaves { get; set; }
    public ICollection<StudentTransfer> Transfers { get; set; }
    
    // Audit
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public Guid? CreatedBy { get; set; }
    public Guid? UpdatedBy { get; set; }
    public bool IsDeleted { get; set; }
}

// StudentStatus Enum
public enum StudentStatus
{
    Active,
    Suspended,
    OnLeave
}
```

---

## API Endpoints (Phase 3)

### Students

| Method | Endpoint | Description | Auth |
|--------|----------|-------------|------|
| GET | `/api/students` | List students (paginated) | Yes |
| GET | `/api/students/{id}` | Get student by ID | Yes |
| GET | `/api/students/search` | Search students | Yes |
| GET | `/api/students/by-bus/{busId}` | Students on bus | Yes |
| GET | `/api/students/suspended` | Suspended students | Yes |
| GET | `/api/students/on-leave` | Students on leave | Yes |
| GET | `/api/students/{id}/timeline` | Student history | Yes |

### Assignments

| Method | Endpoint | Description | Auth |
|--------|----------|-------------|------|
| POST | `/api/students/{id}/assign` | Assign to bus | Staff |
| POST | `/api/students/{id}/transfer` | Transfer to bus | Staff |
| PUT | `/api/students/{id}/split-transport` | Set different buses | Staff |
| DELETE | `/api/students/{id}/unassign` | Remove from bus | Staff |

### Lifecycle

| Method | Endpoint | Description | Auth |
|--------|----------|-------------|------|
| POST | `/api/students/{id}/suspend` | Suspend student | Staff |
| POST | `/api/students/{id}/reactivate` | Reactivate student | Staff |
| POST | `/api/students/{id}/leave` | Create leave record | Staff |
| PUT | `/api/students/{id}/leave/{leaveId}` | Update leave | Staff |
| DELETE | `/api/students/{id}/leave/{leaveId}` | Cancel leave | Staff |

---

## Frontend Components

### Student Search

```vue
<!-- StudentSearchView.vue -->
<template>
  <div class="student-search">
    <div class="search-form">
      <input v-model="searchQuery" 
             placeholder="Search by name, reg number, or phone..."
             @input="debounceSearch" />
      
      <select v-model="statusFilter">
        <option value="">All Statuses</option>
        <option value="Active">Active</option>
        <option value="Suspended">Suspended</option>
        <option value="OnLeave">On Leave</option>
      </select>
      
      <select v-model="districtFilter">
        <option value="">All Districts</option>
        <option v-for="d in districts" :value="d.id">{{ d.nameAr }}</option>
      </select>
    </div>
    
    <div class="results">
      <StudentCard v-for="student in results" 
                   :key="student.id"
                   :student="student" />
    </div>
  </div>
</template>
```

### Student Status Badge

```vue
<!-- StudentStatusBadge.vue -->
<template>
  <span class="badge" :class="badgeClass">
    {{ statusText }}
  </span>
</template>

<script setup>
const props = defineProps(['status']);

const badgeClass = computed(() => ({
  'badge-success': props.status === 'Active',
  'badge-error': props.status === 'Suspended',
  'badge-warning': props.status === 'OnLeave'
}));

const statusText = computed(() => ({
  'Active': 'نشط',
  'Suspended': 'موقوف',
  'OnLeave': 'إجازة'
}[props.status]));
</script>
```

### Suspension Dialog

```vue
<!-- SuspensionDialog.vue -->
<template>
  <dialog class="modal" :open="isOpen">
    <div class="modal-box">
      <h3 class="font-bold text-lg">إيقاف الطالب</h3>
      
      <div class="py-4">
        <p>الطالب: {{ student.fullNameAr }}</p>
        <p>الباص: {{ student.busNumber }}</p>
        
        <div class="form-control">
          <label class="label">سبب الإيقاف</label>
          <textarea v-model="reason" 
                    class="textarea textarea-bordered"
                    rows="3"></textarea>
        </div>
      </div>
      
      <div class="modal-action">
        <button class="btn btn-error" @click="confirm">تأكيد الإيقاف</button>
        <button class="btn" @click="close">إلغاء</button>
      </div>
    </div>
  </dialog>
</template>

<script setup>
const reason = ref('غياب لمدة ثلاثة أيام متتالية');
</script>
```

---

## Acceptance Criteria

### Sprint 3.1
- [ ] Central DB view data accessible
- [ ] Students searchable by multiple criteria
- [ ] Student details display all fields
- [ ] External ID mapping works correctly
- [ ] Halaqa/Teacher info from central system visible

### Sprint 3.2
- [ ] Students assigned to buses successfully
- [ ] Split transport (arrival ≠ return) functional
- [ ] Transfer wizard completes without errors
- [ ] Capacity warnings display when bus full
- [ ] Assignment history recorded

### Sprint 3.3
- [ ] Suspension with reason works
- [ ] Default reason pre-populated
- [ ] Reactivation allows bus selection
- [ ] Leave records with date range created
- [ ] Attachments upload successfully
- [ ] Student timeline displays history

---

## Achievement Tracking

### Week 6 Progress

| Category | Planned | Completed | Percentage |
|----------|---------|-----------|------------|
| Backend Tasks | 7 | 0 | 0% |
| Frontend Tasks | 7 | 0 | 0% |
| **Total** | **14** | **0** | **0%** |

### Week 7 Progress

| Category | Planned | Completed | Percentage |
|----------|---------|-----------|------------|
| Backend Tasks | 7 | 0 | 0% |
| Frontend Tasks | 7 | 0 | 0% |
| **Total** | **14** | **0** | **0%** |

### Week 8 Progress

| Category | Planned | Completed | Percentage |
|----------|---------|-----------|------------|
| Backend Tasks | 7 | 0 | 0% |
| Frontend Tasks | 7 | 0 | 0% |
| **Total** | **14** | **0** | **0%** |

---

## Phase Summary

### Overall Progress

```
Phase 3 Completion: ░░░░░░░░░░ 0%
```

| Sprint | Status | Completion |
|--------|--------|------------|
| Sprint 3.1 - Central Integration | 🔴 Not Started | 0% |
| Sprint 3.2 - Bus Assignment | 🔴 Not Started | 0% |
| Sprint 3.3 - Lifecycle Management | 🔴 Not Started | 0% |

### Key Milestones

- [ ] 🎯 Central DB integration complete
- [ ] 🎯 Student assignment system functional
- [ ] 🎯 Split transport working
- [ ] 🎯 Lifecycle management complete

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
