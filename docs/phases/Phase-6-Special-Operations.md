# Phase 6: Special Operations

## Weeks 15-16 | Bus Merging & Summer Program Mode

---

## Phase Overview

| Attribute | Details |
|-----------|---------|
| **Duration** | 2 Weeks |
| **Start Date** | _TBD_ |
| **End Date** | _TBD_ |
| **Status** | 🔴 Not Started |
| **Progress** | 0% |
| **Dependencies** | Phase 1-5 Complete |

---

## Objectives

1. Implement temporary bus merging functionality for low-demand periods
2. Create merge wizard with route compatibility checking
3. Build restoration mechanism for returning to original configuration
4. Implement Summer Program mode with data isolation
5. Support temporary students for summer-only registration
6. Create mode switching with clear indicators throughout UI

---

## Sprint 6.1: Bus Merging (Week 15)

### Backend Tasks

| Task | Status | Assignee | Notes |
|------|--------|----------|-------|
| Implement BusMergeOperation entity | ⬜ Pending | | Merge tracking model |
| Create BusMergeDetails entity | ⬜ Pending | | Source → Target mapping |
| Create merge service with validation | ⬜ Pending | | Route compatibility checks |
| Implement merge execution | ⬜ Pending | | Move students temporarily |
| Build restore functionality | ⬜ Pending | | Undo merge operation |
| Handle student reassignments | ⬜ Pending | | Temporary reassignment |
| Create merge history tracking | ⬜ Pending | | Audit trail |
| Add capacity validation for merged buses | ⬜ Pending | | Prevent overflow |

### Frontend Tasks

| Task | Status | Assignee | Notes |
|------|--------|----------|-------|
| Create bus merge interface | ⬜ Pending | | Entry point for merging |
| Build merge wizard (multi-step) | ⬜ Pending | | Step-by-step flow |
| Show route compatibility | ⬜ Pending | | Visual indicators |
| Display student counts preview | ⬜ Pending | | Before/after counts |
| Display merged bus status | ⬜ Pending | | Merged badge/indicator |
| Implement restore operation | ⬜ Pending | | Undo merge UI |
| Create merge history view | ⬜ Pending | | Past merge operations |
| Add confirmation dialogs | ⬜ Pending | | Confirm merge/restore |

### Deliverables Checklist

- [ ] Buses can be merged temporarily
- [ ] Route compatibility shown before merge
- [ ] Students moved to target bus
- [ ] Source bus marked as merged
- [ ] Merged buses display indicator
- [ ] Restore returns original config
- [ ] Merge history tracked
- [ ] Capacity limits enforced

---

## Sprint 6.2: Summer Program Mode (Week 16)

### Backend Tasks

| Task | Status | Assignee | Notes |
|------|--------|----------|-------|
| Implement SummerProgramData entity | ⬜ Pending | | Summer season tracking |
| Create SummerStudentAssignments entity | ⬜ Pending | | Summer-specific links |
| Create summer mode activation service | ⬜ Pending | | Enable/disable summer |
| Implement data isolation | ⬜ Pending | | Separate summer data |
| Support temporary students | ⬜ Pending | | Summer-only students |
| Build summer-specific reporting | ⬜ Pending | | Filtered reports |
| Create data snapshot for baseline | ⬜ Pending | | Copy permanent data |
| Implement summer mode deactivation | ⬜ Pending | | Clean return to normal |

### Frontend Tasks

| Task | Status | Assignee | Notes |
|------|--------|----------|-------|
| Create summer program toggle | ⬜ Pending | | Admin activation UI |
| Build summer student management | ⬜ Pending | | Temporary students |
| Show mode indicator throughout UI | ⬜ Pending | | Banner/badge |
| Implement summer-specific views | ⬜ Pending | | Filtered student lists |
| Create summer setup wizard | ⬜ Pending | | Initial configuration |
| Add summer statistics dashboard | ⬜ Pending | | Summer-only stats |
| Build deactivation confirmation | ⬜ Pending | | End summer mode |
| Display normal vs summer data clearly | ⬜ Pending | | Visual distinction |

### Deliverables Checklist

- [ ] Summer mode can be activated
- [ ] Permanent data used as baseline
- [ ] Temporary students can be added
- [ ] Summer data isolated from permanent
- [ ] Mode indicator visible everywhere
- [ ] Reports filter by mode
- [ ] Deactivation cleans up properly
- [ ] Original data preserved

---

## Technical Specifications

### Database Schema (Phase 6)

```sql
-- Bus Merge Operations
CREATE TABLE BusMergeOperations (
    MergeId UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    
    MergeName NVARCHAR(200) NOT NULL,
    MergeReason NVARCHAR(500),
    
    StartDate DATE NOT NULL,
    EndDate DATE,
    
    IsActive BIT NOT NULL DEFAULT 1,
    IsRestored BIT NOT NULL DEFAULT 0,
    
    CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    CreatedBy UNIQUEIDENTIFIER NOT NULL,
    RestoredAt DATETIME2,
    RestoredBy UNIQUEIDENTIFIER,
    
    CONSTRAINT FK_BusMerge_CreatedBy FOREIGN KEY (CreatedBy) REFERENCES Users(UserId),
    CONSTRAINT FK_BusMerge_RestoredBy FOREIGN KEY (RestoredBy) REFERENCES Users(UserId),
    
    INDEX IX_BusMerge_Active (IsActive),
    INDEX IX_BusMerge_Dates (StartDate, EndDate)
);

-- Bus Merge Details (Source → Target mappings)
CREATE TABLE BusMergeDetails (
    MergeDetailId UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    MergeId UNIQUEIDENTIFIER NOT NULL,
    
    SourceBusId UNIQUEIDENTIFIER NOT NULL,
    TargetBusId UNIQUEIDENTIFIER NOT NULL,
    
    -- Snapshot of student counts at merge time
    SourceStudentCount INT NOT NULL DEFAULT 0,
    TargetStudentCountBefore INT NOT NULL DEFAULT 0,
    TargetStudentCountAfter INT NOT NULL DEFAULT 0,
    
    CONSTRAINT FK_MergeDetail_Merge FOREIGN KEY (MergeId) REFERENCES BusMergeOperations(MergeId) ON DELETE CASCADE,
    CONSTRAINT FK_MergeDetail_SourceBus FOREIGN KEY (SourceBusId) REFERENCES Buses(BusId),
    CONSTRAINT FK_MergeDetail_TargetBus FOREIGN KEY (TargetBusId) REFERENCES Buses(BusId),
    
    INDEX IX_MergeDetail_Merge (MergeId)
);

-- Student Merge Assignments (temporary reassignments during merge)
CREATE TABLE StudentMergeAssignments (
    MergeAssignmentId UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    MergeDetailId UNIQUEIDENTIFIER NOT NULL,
    StudentId UNIQUEIDENTIFIER NOT NULL,
    
    OriginalBusId UNIQUEIDENTIFIER NOT NULL,
    TemporaryBusId UNIQUEIDENTIFIER NOT NULL,
    
    CONSTRAINT FK_MergeAssignment_Detail FOREIGN KEY (MergeDetailId) REFERENCES BusMergeDetails(MergeDetailId) ON DELETE CASCADE,
    CONSTRAINT FK_MergeAssignment_Student FOREIGN KEY (StudentId) REFERENCES Students(StudentId),
    
    INDEX IX_MergeAssignment_Student (StudentId)
);

-- Summer Program Data
CREATE TABLE SummerProgramData (
    SummerDataId UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    
    Year INT NOT NULL,
    ProgramName NVARCHAR(200) NOT NULL,
    
    IsActive BIT NOT NULL DEFAULT 1,
    
    StartDate DATE NOT NULL,
    EndDate DATE NOT NULL,
    
    CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    CreatedBy UNIQUEIDENTIFIER NOT NULL,
    DeactivatedAt DATETIME2,
    DeactivatedBy UNIQUEIDENTIFIER,
    
    CONSTRAINT FK_SummerData_CreatedBy FOREIGN KEY (CreatedBy) REFERENCES Users(UserId),
    
    INDEX IX_SummerData_Year (Year),
    INDEX IX_SummerData_Active (IsActive),
    CONSTRAINT UQ_SummerData_Year UNIQUE (Year)
);

-- Summer Student Assignments
CREATE TABLE SummerStudentAssignments (
    SummerAssignmentId UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    SummerDataId UNIQUEIDENTIFIER NOT NULL,
    
    -- Can be permanent student or temporary
    StudentId UNIQUEIDENTIFIER,                    -- NULL for temporary
    TemporaryStudentName NVARCHAR(200),            -- For summer-only students
    TemporaryStudentPhone NVARCHAR(20),
    TemporaryDistrictId UNIQUEIDENTIFIER,
    
    BusId UNIQUEIDENTIFIER NOT NULL,
    
    IsActive BIT NOT NULL DEFAULT 1,
    CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    
    CONSTRAINT FK_SummerAssignment_SummerData FOREIGN KEY (SummerDataId) REFERENCES SummerProgramData(SummerDataId) ON DELETE CASCADE,
    CONSTRAINT FK_SummerAssignment_Student FOREIGN KEY (StudentId) REFERENCES Students(StudentId),
    CONSTRAINT FK_SummerAssignment_Bus FOREIGN KEY (BusId) REFERENCES Buses(BusId),
    
    INDEX IX_SummerAssignment_Summer (SummerDataId),
    INDEX IX_SummerAssignment_Bus (BusId)
);
```

### Entity Models

```csharp
// Bus Merge Operation Entity
public class BusMergeOperation : BaseEntity
{
    public string MergeName { get; set; }
    public string MergeReason { get; set; }
    
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    
    public bool IsActive { get; set; } = true;
    public bool IsRestored { get; set; } = false;
    
    public DateTime CreatedAt { get; set; }
    public Guid CreatedBy { get; set; }
    public User Creator { get; set; }
    
    public DateTime? RestoredAt { get; set; }
    public Guid? RestoredBy { get; set; }
    public User Restorer { get; set; }
    
    public ICollection<BusMergeDetail> Details { get; set; }
}

public class BusMergeDetail : BaseEntity
{
    public Guid MergeId { get; set; }
    public BusMergeOperation Merge { get; set; }
    
    public Guid SourceBusId { get; set; }
    public Bus SourceBus { get; set; }
    
    public Guid TargetBusId { get; set; }
    public Bus TargetBus { get; set; }
    
    public int SourceStudentCount { get; set; }
    public int TargetStudentCountBefore { get; set; }
    public int TargetStudentCountAfter { get; set; }
    
    public ICollection<StudentMergeAssignment> StudentAssignments { get; set; }
}

// Summer Program Entity
public class SummerProgramData : BaseEntity
{
    public int Year { get; set; }
    public string ProgramName { get; set; }
    
    public bool IsActive { get; set; } = true;
    
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    
    public DateTime CreatedAt { get; set; }
    public Guid CreatedBy { get; set; }
    
    public DateTime? DeactivatedAt { get; set; }
    public Guid? DeactivatedBy { get; set; }
    
    public ICollection<SummerStudentAssignment> StudentAssignments { get; set; }
}

public class SummerStudentAssignment : BaseEntity
{
    public Guid SummerDataId { get; set; }
    public SummerProgramData SummerData { get; set; }
    
    // Permanent student (nullable for temporary)
    public Guid? StudentId { get; set; }
    public Student Student { get; set; }
    
    // Temporary student fields
    public string TemporaryStudentName { get; set; }
    public string TemporaryStudentPhone { get; set; }
    public Guid? TemporaryDistrictId { get; set; }
    
    public Guid BusId { get; set; }
    public Bus Bus { get; set; }
    
    public bool IsActive { get; set; } = true;
    
    // Computed property
    public bool IsTemporaryStudent => StudentId == null;
}
```

### DTOs

```csharp
// Bus Merge DTOs
public class CreateBusMergeDto
{
    public string MergeName { get; set; }
    public string MergeReason { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public List<BusMergePairDto> MergePairs { get; set; }
}

public class BusMergePairDto
{
    public Guid SourceBusId { get; set; }
    public Guid TargetBusId { get; set; }
}

public class BusMergeOperationDto
{
    public Guid MergeId { get; set; }
    public string MergeName { get; set; }
    public string MergeReason { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public bool IsActive { get; set; }
    public bool IsRestored { get; set; }
    public List<BusMergeDetailDto> Details { get; set; }
}

public class BusMergeDetailDto
{
    public string SourceBusNumber { get; set; }
    public string TargetBusNumber { get; set; }
    public int StudentsMovedCount { get; set; }
}

// Summer Program DTOs
public class CreateSummerProgramDto
{
    public int Year { get; set; }
    public string ProgramName { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
}

public class SummerProgramDto
{
    public Guid SummerDataId { get; set; }
    public int Year { get; set; }
    public string ProgramName { get; set; }
    public bool IsActive { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int TotalStudents { get; set; }
    public int TemporaryStudents { get; set; }
    public int PermanentStudents { get; set; }
}

public class AddTemporaryStudentDto
{
    public string StudentName { get; set; }
    public string PhoneNumber { get; set; }
    public Guid DistrictId { get; set; }
    public Guid BusId { get; set; }
}
```

---

## API Endpoints (Phase 6)

### Bus Merging

| Method | Endpoint | Description | Auth |
|--------|----------|-------------|------|
| GET | `/api/bus-merge` | List all merge operations | Admin |
| GET | `/api/bus-merge/active` | Get active merges | Admin |
| GET | `/api/bus-merge/{id}` | Get merge details | Admin |
| POST | `/api/bus-merge` | Create merge operation | Admin |
| POST | `/api/bus-merge/{id}/restore` | Restore original config | Admin |
| GET | `/api/bus-merge/compatibility` | Check route compatibility | Admin |
| GET | `/api/bus-merge/preview` | Preview merge results | Admin |

### Summer Program

| Method | Endpoint | Description | Auth |
|--------|----------|-------------|------|
| GET | `/api/summer-program` | List all summer programs | Admin |
| GET | `/api/summer-program/active` | Get active program | Yes |
| GET | `/api/summer-program/{id}` | Get program details | Admin |
| POST | `/api/summer-program` | Create summer program | Admin |
| POST | `/api/summer-program/{id}/activate` | Activate summer mode | Admin |
| POST | `/api/summer-program/{id}/deactivate` | Deactivate summer mode | Admin |
| GET | `/api/summer-program/{id}/students` | Get summer students | Yes |
| POST | `/api/summer-program/{id}/students` | Add temporary student | Staff |
| PUT | `/api/summer-program/{id}/students/{studentId}` | Update assignment | Staff |
| DELETE | `/api/summer-program/{id}/students/{studentId}` | Remove student | Staff |

### System Status

| Method | Endpoint | Description | Auth |
|--------|----------|-------------|------|
| GET | `/api/system/mode` | Get current operation mode | Yes |

---

## Frontend Components

### Bus Merge Wizard

```vue
<!-- BusMergeView.vue -->
<template>
  <div class="bus-merge-view">
    <div class="flex justify-between items-center mb-6">
      <h1 class="text-2xl font-bold">دمج الباصات</h1>
      <button class="btn btn-primary" @click="startNewMerge">
        + دمج جديد
      </button>
    </div>
    
    <!-- Active Merges -->
    <div v-if="activeMerges.length > 0" class="mb-8">
      <h2 class="text-lg font-bold mb-4">الدمج النشط</h2>
      <div class="grid gap-4">
        <div v-for="merge in activeMerges" 
             :key="merge.mergeId"
             class="card bg-warning/20 shadow">
          <div class="card-body">
            <h3 class="card-title">{{ merge.mergeName }}</h3>
            <p>{{ merge.mergeReason }}</p>
            <p class="text-sm">من: {{ formatDate(merge.startDate) }}</p>
            
            <div class="mt-4">
              <div v-for="detail in merge.details" class="flex items-center gap-2 mb-2">
                <span class="badge badge-outline">{{ detail.sourceBusNumber }}</span>
                <span>→</span>
                <span class="badge badge-primary">{{ detail.targetBusNumber }}</span>
                <span class="text-sm">({{ detail.studentsMovedCount }} طالب)</span>
              </div>
            </div>
            
            <div class="card-actions justify-end">
              <button class="btn btn-error" @click="restoreMerge(merge.mergeId)">
                استعادة الأصل
              </button>
            </div>
          </div>
        </div>
      </div>
    </div>
    
    <!-- Merge Wizard Modal -->
    <dialog class="modal" :open="showWizard">
      <div class="modal-box max-w-3xl">
        <!-- Step 1: Basic Info -->
        <div v-if="wizardStep === 1">
          <h3 class="font-bold text-lg">الخطوة 1: معلومات الدمج</h3>
          <div class="form-control mt-4">
            <label class="label">اسم العملية</label>
            <input v-model="mergeData.name" class="input input-bordered" />
          </div>
          <div class="form-control mt-4">
            <label class="label">السبب</label>
            <textarea v-model="mergeData.reason" class="textarea textarea-bordered"></textarea>
          </div>
          <div class="grid grid-cols-2 gap-4 mt-4">
            <div class="form-control">
              <label class="label">تاريخ البدء</label>
              <input type="date" v-model="mergeData.startDate" class="input input-bordered" />
            </div>
            <div class="form-control">
              <label class="label">تاريخ الانتهاء (اختياري)</label>
              <input type="date" v-model="mergeData.endDate" class="input input-bordered" />
            </div>
          </div>
        </div>
        
        <!-- Step 2: Select Buses -->
        <div v-if="wizardStep === 2">
          <h3 class="font-bold text-lg">الخطوة 2: اختيار الباصات</h3>
          <p class="text-sm opacity-70 mt-2">اختر الباص المصدر والهدف لكل عملية دمج</p>
          
          <div v-for="(pair, index) in mergeData.pairs" :key="index" 
               class="card bg-base-200 mt-4">
            <div class="card-body">
              <div class="grid grid-cols-2 gap-4">
                <div class="form-control">
                  <label class="label">الباص المصدر (سيتم إخلاؤه)</label>
                  <select v-model="pair.sourceId" class="select select-bordered">
                    <option value="">اختر الباص</option>
                    <option v-for="bus in availableBuses" :value="bus.busId">
                      {{ bus.busNumber }} ({{ bus.studentCount }} طالب)
                    </option>
                  </select>
                </div>
                <div class="form-control">
                  <label class="label">الباص الهدف (سيستقبل الطلاب)</label>
                  <select v-model="pair.targetId" class="select select-bordered">
                    <option value="">اختر الباص</option>
                    <option v-for="bus in availableBuses" :value="bus.busId">
                      {{ bus.busNumber }} ({{ bus.studentCount }}/{{ bus.capacity }})
                    </option>
                  </select>
                </div>
              </div>
              
              <!-- Compatibility Check -->
              <div v-if="pair.sourceId && pair.targetId" class="mt-2">
                <div v-if="checkCompatibility(pair)" class="alert alert-success">
                  ✓ متوافق - السعة كافية
                </div>
                <div v-else class="alert alert-error">
                  ✗ غير متوافق - السعة غير كافية
                </div>
              </div>
              
              <button class="btn btn-sm btn-ghost text-error self-end" 
                      @click="removePair(index)"
                      v-if="mergeData.pairs.length > 1">
                حذف
              </button>
            </div>
          </div>
          
          <button class="btn btn-ghost mt-4" @click="addPair">
            + إضافة دمج آخر
          </button>
        </div>
        
        <!-- Step 3: Preview & Confirm -->
        <div v-if="wizardStep === 3">
          <h3 class="font-bold text-lg">الخطوة 3: المراجعة والتأكيد</h3>
          
          <div class="alert alert-warning mt-4">
            <span>⚠️ هذه العملية ستنقل الطلاب مؤقتاً. يمكن التراجع عنها لاحقاً.</span>
          </div>
          
          <div class="overflow-x-auto mt-4">
            <table class="table">
              <thead>
                <tr>
                  <th>من</th>
                  <th>إلى</th>
                  <th>عدد الطلاب</th>
                  <th>السعة بعد الدمج</th>
                </tr>
              </thead>
              <tbody>
                <tr v-for="preview in mergePreview">
                  <td>{{ preview.sourceBus }}</td>
                  <td>{{ preview.targetBus }}</td>
                  <td>{{ preview.studentsToMove }}</td>
                  <td>{{ preview.newCapacity }}/{{ preview.maxCapacity }}</td>
                </tr>
              </tbody>
            </table>
          </div>
        </div>
        
        <div class="modal-action">
          <button class="btn" @click="prevStep" v-if="wizardStep > 1">السابق</button>
          <button class="btn btn-primary" @click="nextStep" v-if="wizardStep < 3">التالي</button>
          <button class="btn btn-success" @click="executeMerge" v-if="wizardStep === 3">
            تنفيذ الدمج
          </button>
          <button class="btn btn-ghost" @click="closeWizard">إلغاء</button>
        </div>
      </div>
    </dialog>
  </div>
</template>
```

### Summer Program Toggle

```vue
<!-- SummerProgramView.vue -->
<template>
  <div class="summer-program-view">
    <!-- Current Mode Banner -->
    <div v-if="isSummerMode" class="alert alert-info mb-6">
      <span class="text-xl">☀️</span>
      <div>
        <h3 class="font-bold">وضع البرنامج الصيفي نشط</h3>
        <p>{{ activeSummer.programName }} - {{ activeSummer.year }}</p>
      </div>
      <button class="btn btn-sm btn-error" @click="confirmDeactivate">
        إنهاء البرنامج الصيفي
      </button>
    </div>
    
    <!-- Summer Program Management -->
    <div class="card bg-base-100 shadow-xl">
      <div class="card-body">
        <h2 class="card-title">إدارة البرنامج الصيفي</h2>
        
        <div v-if="!isSummerMode">
          <p class="mb-4">لبدء البرنامج الصيفي، أدخل المعلومات التالية:</p>
          
          <div class="grid grid-cols-2 gap-4">
            <div class="form-control">
              <label class="label">السنة</label>
              <input type="number" v-model="newProgram.year" class="input input-bordered" />
            </div>
            <div class="form-control">
              <label class="label">اسم البرنامج</label>
              <input v-model="newProgram.name" class="input input-bordered" 
                     placeholder="البرنامج الصيفي 2026" />
            </div>
            <div class="form-control">
              <label class="label">تاريخ البدء</label>
              <input type="date" v-model="newProgram.startDate" class="input input-bordered" />
            </div>
            <div class="form-control">
              <label class="label">تاريخ الانتهاء</label>
              <input type="date" v-model="newProgram.endDate" class="input input-bordered" />
            </div>
          </div>
          
          <button class="btn btn-primary mt-4" @click="activateSummer">
            ☀️ تفعيل البرنامج الصيفي
          </button>
        </div>
        
        <!-- Summer Statistics -->
        <div v-else class="stats shadow mt-4">
          <div class="stat">
            <div class="stat-title">إجمالي الطلاب</div>
            <div class="stat-value">{{ activeSummer.totalStudents }}</div>
          </div>
          <div class="stat">
            <div class="stat-title">طلاب دائمون</div>
            <div class="stat-value text-primary">{{ activeSummer.permanentStudents }}</div>
          </div>
          <div class="stat">
            <div class="stat-title">طلاب مؤقتون</div>
            <div class="stat-value text-secondary">{{ activeSummer.temporaryStudents }}</div>
          </div>
        </div>
      </div>
    </div>
    
    <!-- Temporary Students Section -->
    <div v-if="isSummerMode" class="card bg-base-100 shadow-xl mt-6">
      <div class="card-body">
        <div class="flex justify-between items-center">
          <h2 class="card-title">الطلاب المؤقتون</h2>
          <button class="btn btn-primary btn-sm" @click="showAddTemporary = true">
            + إضافة طالب مؤقت
          </button>
        </div>
        
        <div class="overflow-x-auto mt-4">
          <table class="table">
            <thead>
              <tr>
                <th>الاسم</th>
                <th>الهاتف</th>
                <th>الحي</th>
                <th>الباص</th>
                <th>إجراءات</th>
              </tr>
            </thead>
            <tbody>
              <tr v-for="student in temporaryStudents" :key="student.id">
                <td>{{ student.name }}</td>
                <td>{{ student.phone }}</td>
                <td>{{ student.district }}</td>
                <td>{{ student.busNumber }}</td>
                <td>
                  <button class="btn btn-sm btn-ghost text-error" @click="removeTemp(student.id)">
                    حذف
                  </button>
                </td>
              </tr>
            </tbody>
          </table>
        </div>
      </div>
    </div>
  </div>
</template>
```

### Mode Indicator Component

```vue
<!-- ModeIndicator.vue -->
<template>
  <div v-if="isSummerMode" class="badge badge-warning gap-1">
    <span>☀️</span>
    <span>صيفي</span>
  </div>
  <div v-else-if="hasMergedBuses" class="badge badge-info gap-1">
    <span>🔀</span>
    <span>دمج نشط</span>
  </div>
</template>
```

---

## Acceptance Criteria

### Sprint 6.1 - Bus Merging
- [ ] Merge wizard guides through process
- [ ] Route compatibility checked before merge
- [ ] Capacity validation prevents overflow
- [ ] Students moved to target bus
- [ ] Source bus marked as merged (inactive)
- [ ] Merged buses display indicator
- [ ] Restore reverts all changes
- [ ] Merge history tracked and viewable

### Sprint 6.2 - Summer Program
- [ ] Summer mode can be activated
- [ ] Permanent data copied as baseline
- [ ] Temporary students can be added
- [ ] Summer data isolated from permanent
- [ ] Mode indicator visible throughout UI
- [ ] Reports show summer data only
- [ ] Deactivation preserves permanent data
- [ ] Clear distinction between modes

---

## Achievement Tracking

### Week 15 Progress

| Category | Planned | Completed | Percentage |
|----------|---------|-----------|------------|
| Backend Tasks | 8 | 0 | 0% |
| Frontend Tasks | 8 | 0 | 0% |
| **Total** | **16** | **0** | **0%** |

### Week 16 Progress

| Category | Planned | Completed | Percentage |
|----------|---------|-----------|------------|
| Backend Tasks | 8 | 0 | 0% |
| Frontend Tasks | 8 | 0 | 0% |
| **Total** | **16** | **0** | **0%** |

---

## Phase Summary

### Overall Progress

```
Phase 6 Completion: ░░░░░░░░░░ 0%
```

| Sprint | Status | Completion |
|--------|--------|------------|
| Sprint 6.1 - Bus Merging | 🔴 Not Started | 0% |
| Sprint 6.2 - Summer Program | 🔴 Not Started | 0% |

### Key Milestones

- [ ] 🎯 Bus merging functional
- [ ] 🎯 Merge restoration working
- [ ] 🎯 Summer mode activation
- [ ] 🎯 Temporary students supported

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
