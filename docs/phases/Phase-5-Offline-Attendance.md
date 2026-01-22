# Phase 5: Offline-First Attendance System

## Weeks 11-14 | IndexedDB, Sync & Weekly Review

---

## Phase Overview

| Attribute | Details |
|-----------|---------|
| **Duration** | 4 Weeks |
| **Start Date** | _TBD_ |
| **End Date** | _TBD_ |
| **Status** | 🔴 Not Started |
| **Progress** | 0% |
| **Dependencies** | Phase 1, 2 & 3 Complete |

---

## Objectives

1. Build offline-first attendance infrastructure using IndexedDB (Dexie.js)
2. Create high-performance supervisor attendance interface (< 5 seconds per bus)
3. Implement reliable sync mechanism with conflict resolution
4. Build weekly attendance review with direct suspension capability
5. Ensure zero data loss under any network conditions
6. Create sync status monitoring and error handling

---

## Sprint 5.1: Offline Infrastructure (Week 11)

### Backend Tasks

| Task | Status | Assignee | Notes |
|------|--------|----------|-------|
| Create Supervisor entity and repository | ⬜ Pending | | Supervisor model |
| Implement AttendanceSession entity | ⬜ Pending | | Session per bus/day |
| Create AttendanceRecord entity | ⬜ Pending | | Per-student records |
| Build sync endpoint with idempotency | ⬜ Pending | | POST `/api/attendance/sync` |
| Implement conflict resolution | ⬜ Pending | | Timestamp + ownership |
| Create duplicate detection | ⬜ Pending | | Reject duplicate sessions |
| Add sync validation rules | ⬜ Pending | | Data integrity checks |

### Frontend Tasks

| Task | Status | Assignee | Notes |
|------|--------|----------|-------|
| Configure Dexie.js schema for attendance | ⬜ Pending | | 3 object stores |
| Create sync service | ⬜ Pending | | SyncService class |
| Implement sync queue management | ⬜ Pending | | Queue CRUD operations |
| Build offline detection | ⬜ Pending | | navigator.onLine events |
| Create sync status indicators | ⬜ Pending | | Pending/Syncing/Error icons |
| Implement network reconnect handler | ⬜ Pending | | Auto-trigger sync |
| Create offline store (Pinia) | ⬜ Pending | | Offline state management |

### Deliverables Checklist

- [ ] IndexedDB schema created with 3 stores
- [ ] Data persists across browser refresh
- [ ] Sync queue tracks pending items
- [ ] Offline mode detected automatically
- [ ] Sync endpoint validates idempotency
- [ ] Conflict resolution works correctly

---

## Sprint 5.2: Supervisor Attendance Interface (Week 12)

### Backend Tasks

| Task | Status | Assignee | Notes |
|------|--------|----------|-------|
| Create supervisor assignment endpoints | ⬜ Pending | | Supervisor-bus links |
| Implement bus-supervisor linking | ⬜ Pending | | Many-to-many |
| Add attendance validation | ⬜ Pending | | Business rules |
| Create student list caching endpoint | ⬜ Pending | | For offline cache |
| Implement location validation | ⬜ Pending | | Optional GPS check |

### Frontend Tasks

| Task | Status | Assignee | Notes |
|------|--------|----------|-------|
| Create supervisor dashboard | ⬜ Pending | | Role-specific home |
| Build attendance capture interface | ⬜ Pending | | Main attendance UI |
| Implement default "all present" functionality | ⬜ Pending | | Pre-select all |
| Create fast bulk attendance entry | ⬜ Pending | | One-click deselect |
| Add unregistered student counter | ⬜ Pending | | Numeric input |
| Optimize for touch and speed | ⬜ Pending | | Large buttons, no lag |
| Implement period/location selector | ⬜ Pending | | Session context |
| Create bus selection interface | ⬜ Pending | | Assigned buses |
| Add attendance completion indicator | ⬜ Pending | | Per-bus checkmarks |

### Performance Targets

| Metric | Target | Measurement |
|--------|--------|-------------|
| Load student list | < 200ms | From IndexedDB |
| Toggle attendance status | < 50ms | UI response |
| Save attendance (local) | < 100ms | IndexedDB write |
| Full bus attendance entry | < 5 seconds | User workflow |

### Deliverables Checklist

- [ ] Supervisor can select period, location, bus
- [ ] Student list loads from local cache
- [ ] All students default to Present
- [ ] Tap to toggle absent (deselect)
- [ ] Unregistered count can be added
- [ ] Save completes in < 100ms
- [ ] Full workflow < 5 seconds per bus

---

## Sprint 5.3: Attendance Synchronization (Week 13)

### Backend Tasks

| Task | Status | Assignee | Notes |
|------|--------|----------|-------|
| Implement batch sync endpoint | ⬜ Pending | | Multiple sessions |
| Add sync logging | ⬜ Pending | | Track sync events |
| Create duplicate detection | ⬜ Pending | | By session_id |
| Implement sync audit trail | ⬜ Pending | | Who synced what when |
| Add partial sync recovery | ⬜ Pending | | Handle partial failures |
| Create sync statistics endpoint | ⬜ Pending | | For monitoring |

### Frontend Tasks

| Task | Status | Assignee | Notes |
|------|--------|----------|-------|
| Implement automatic sync on reconnect | ⬜ Pending | | online event listener |
| Add manual sync button | ⬜ Pending | | Force sync action |
| Create sync status dashboard | ⬜ Pending | | Pending/Failed items |
| Build retry mechanism with exponential backoff | ⬜ Pending | | 1s, 2s, 4s, 8s, 16s |
| Handle sync errors gracefully | ⬜ Pending | | User-friendly messages |
| Implement sync progress indicator | ⬜ Pending | | Progress bar/count |
| Create failed sync management | ⬜ Pending | | View/retry failed items |
| Add last sync timestamp display | ⬜ Pending | | "Last synced: 5 min ago" |

### Deliverables Checklist

- [ ] Sync triggers automatically on reconnect
- [ ] Manual sync button works
- [ ] Exponential backoff for retries
- [ ] Failed syncs visible to supervisor
- [ ] Retry failed sync manually
- [ ] Sync progress shown during sync
- [ ] Last sync time displayed
- [ ] No data loss guaranteed

---

## Sprint 5.4: Weekly Attendance Review (Week 14)

### Backend Tasks

| Task | Status | Assignee | Notes |
|------|--------|----------|-------|
| Create weekly attendance query endpoints | ⬜ Pending | | Date range queries |
| Implement leave status checking | ⬜ Pending | | Exclude from absence |
| Build attendance analytics | ⬜ Pending | | Absence patterns |
| Create absence summary by student | ⬜ Pending | | Count consecutive |
| Implement bulk suspension endpoint | ⬜ Pending | | Suspend multiple |

### Frontend Tasks

| Task | Status | Assignee | Notes |
|------|--------|----------|-------|
| Create weekly review interface | ⬜ Pending | | Calendar-style view |
| Display attendance by date/bus/period | ⬜ Pending | | Filterable grid |
| Visual indicators for students on leave | ⬜ Pending | | Different color/icon |
| Implement direct suspension from review | ⬜ Pending | | Quick action button |
| Add filters and search | ⬜ Pending | | By bus, period, status |
| Create absence streak highlighting | ⬜ Pending | | 3+ days = red |
| Build export functionality | ⬜ Pending | | Excel/PDF export |
| Implement print-friendly layout | ⬜ Pending | | Print stylesheet |

### Deliverables Checklist

- [ ] Weekly view shows 7 days of attendance
- [ ] Filter by bus, period works
- [ ] Leave students clearly marked
- [ ] Consecutive absences highlighted
- [ ] Suspend directly from review
- [ ] Export to Excel works
- [ ] Print layout available

---

## Technical Specifications

### IndexedDB Schema (Dexie.js)

```typescript
// src/services/offline/db.ts
import Dexie, { Table } from 'dexie';

// Type Definitions
export interface AttendanceSession {
  sessionId: string;           // UUID, Primary Key
  supervisorId: string;
  busId: string;
  periodId: string;
  locationCode: string;        // A8, A9, B10, etc.
  attendanceDate: string;      // YYYY-MM-DD
  attendanceType: 'Arrival' | 'Return';
  unregisteredCount: number;
  status: 'draft' | 'pending_sync' | 'synced' | 'error';
  createdAt: string;           // ISO-8601
  syncedAt?: string;
  errorMessage?: string;
}

export interface AttendanceRecord {
  recordId: string;            // UUID, Primary Key
  sessionId: string;           // FK to AttendanceSession
  studentId: string;
  studentName: string;         // Cached for offline display
  attendanceStatus: 'Present' | 'Absent' | 'Late' | 'Excused';
}

export interface SyncQueueItem {
  queueId: string;             // UUID, Primary Key
  entityType: 'attendance_session';
  entityId: string;            // session_id
  retryCount: number;
  lastAttemptAt?: string;
  syncStatus: 'pending' | 'syncing' | 'failed';
  errorMessage?: string;
}

export interface CachedStudent {
  studentId: string;
  busId: string;
  studentName: string;
  registrationNumber: string;
  phoneNumber: string;
  districtName: string;
  cachedAt: string;
}

// Dexie Database Class
class AttendanceDB extends Dexie {
  attendanceSessions!: Table<AttendanceSession, string>;
  attendanceRecords!: Table<AttendanceRecord, string>;
  syncQueue!: Table<SyncQueueItem, string>;
  cachedStudents!: Table<CachedStudent, string>;

  constructor() {
    super('transport_attendance_db');
    
    this.version(1).stores({
      attendanceSessions: 'sessionId, supervisorId, busId, status, attendanceDate',
      attendanceRecords: 'recordId, sessionId, studentId',
      syncQueue: 'queueId, entityType, syncStatus, lastAttemptAt',
      cachedStudents: 'studentId, busId, cachedAt'
    });
  }
}

export const db = new AttendanceDB();
```

### Sync Service Implementation

```typescript
// src/services/offline/sync.service.ts
import { db, AttendanceSession, AttendanceRecord, SyncQueueItem } from './db';
import { attendanceApi } from '../api/attendance.service';

export class SyncService {
  private isOnline: boolean = navigator.onLine;
  private isSyncing: boolean = false;
  private maxRetries: number = 5;

  constructor() {
    // Listen for online/offline events
    window.addEventListener('online', () => this.handleOnline());
    window.addEventListener('offline', () => this.handleOffline());
  }

  private handleOnline() {
    this.isOnline = true;
    this.syncPendingAttendance();
  }

  private handleOffline() {
    this.isOnline = false;
  }

  // Save attendance locally
  async saveAttendance(
    session: Omit<AttendanceSession, 'status'>,
    records: Omit<AttendanceRecord, 'recordId'>[]
  ): Promise<void> {
    const sessionWithStatus: AttendanceSession = {
      ...session,
      status: 'pending_sync'
    };

    const recordsWithIds: AttendanceRecord[] = records.map(r => ({
      ...r,
      recordId: crypto.randomUUID()
    }));

    // Transaction for atomicity
    await db.transaction('rw', db.attendanceSessions, db.attendanceRecords, db.syncQueue, async () => {
      await db.attendanceSessions.add(sessionWithStatus);
      await db.attendanceRecords.bulkAdd(recordsWithIds);
      await db.syncQueue.add({
        queueId: crypto.randomUUID(),
        entityType: 'attendance_session',
        entityId: session.sessionId,
        retryCount: 0,
        syncStatus: 'pending'
      });
    });

    // Try immediate sync if online
    if (this.isOnline) {
      this.syncPendingAttendance();
    }
  }

  // Sync all pending items
  async syncPendingAttendance(): Promise<void> {
    if (this.isSyncing || !this.isOnline) return;

    this.isSyncing = true;

    try {
      const pendingItems = await db.syncQueue
        .where('syncStatus')
        .equals('pending')
        .sortBy('lastAttemptAt');

      for (const item of pendingItems) {
        await this.syncItem(item);
      }
    } finally {
      this.isSyncing = false;
    }
  }

  private async syncItem(item: SyncQueueItem): Promise<void> {
    // Mark as syncing
    await db.syncQueue.update(item.queueId, { syncStatus: 'syncing' });

    try {
      const session = await db.attendanceSessions.get(item.entityId);
      if (!session) throw new Error('Session not found');

      const records = await db.attendanceRecords
        .where('sessionId')
        .equals(item.entityId)
        .toArray();

      // Send to API
      const response = await attendanceApi.sync({
        sessionId: session.sessionId,
        supervisorId: session.supervisorId,
        busId: session.busId,
        periodId: session.periodId,
        locationCode: session.locationCode,
        attendanceDate: session.attendanceDate,
        attendanceType: session.attendanceType,
        unregisteredCount: session.unregisteredCount,
        timestamp: session.createdAt,
        records: records.map(r => ({
          studentId: r.studentId,
          status: r.attendanceStatus
        }))
      });

      if (response.success) {
        // Mark as synced
        await db.attendanceSessions.update(session.sessionId, {
          status: 'synced',
          syncedAt: new Date().toISOString()
        });
        await db.syncQueue.delete(item.queueId);
      }
    } catch (error) {
      await this.handleSyncError(item, error);
    }
  }

  private async handleSyncError(item: SyncQueueItem, error: any): Promise<void> {
    const retryCount = item.retryCount + 1;

    if (retryCount < this.maxRetries) {
      // Schedule retry with exponential backoff
      const backoffMs = Math.pow(2, retryCount) * 1000;
      
      await db.syncQueue.update(item.queueId, {
        syncStatus: 'pending',
        retryCount,
        lastAttemptAt: new Date().toISOString(),
        errorMessage: error.message
      });

      setTimeout(() => this.syncPendingAttendance(), backoffMs);
    } else {
      // Mark as failed
      await db.syncQueue.update(item.queueId, {
        syncStatus: 'failed',
        errorMessage: error.message
      });
      await db.attendanceSessions.update(item.entityId, {
        status: 'error',
        errorMessage: error.message
      });
    }
  }

  // Get sync status for UI
  async getSyncStatus(): Promise<{
    pending: number;
    syncing: number;
    failed: number;
    lastSyncTime: string | null;
  }> {
    const [pending, syncing, failed, lastSynced] = await Promise.all([
      db.syncQueue.where('syncStatus').equals('pending').count(),
      db.syncQueue.where('syncStatus').equals('syncing').count(),
      db.syncQueue.where('syncStatus').equals('failed').count(),
      db.attendanceSessions.where('status').equals('synced').last()
    ]);

    return {
      pending,
      syncing,
      failed,
      lastSyncTime: lastSynced?.syncedAt || null
    };
  }

  // Retry failed sync
  async retryFailed(queueId: string): Promise<void> {
    await db.syncQueue.update(queueId, {
      syncStatus: 'pending',
      retryCount: 0
    });
    this.syncPendingAttendance();
  }

  // Retry all failed
  async retryAllFailed(): Promise<void> {
    await db.syncQueue
      .where('syncStatus')
      .equals('failed')
      .modify({ syncStatus: 'pending', retryCount: 0 });
    this.syncPendingAttendance();
  }
}

export const syncService = new SyncService();
```

### Database Schema (Backend)

```sql
-- Supervisors Table
CREATE TABLE Supervisors (
    SupervisorId UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    UserId UNIQUEIDENTIFIER NOT NULL UNIQUE,
    LocationId UNIQUEIDENTIFIER,
    IsActive BIT NOT NULL DEFAULT 1,
    
    CONSTRAINT FK_Supervisor_User FOREIGN KEY (UserId) REFERENCES Users(UserId),
    CONSTRAINT FK_Supervisor_Location FOREIGN KEY (LocationId) REFERENCES Locations(LocationId)
);

-- Supervisor-Bus Assignments
CREATE TABLE SupervisorBusAssignments (
    AssignmentId UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    SupervisorId UNIQUEIDENTIFIER NOT NULL,
    BusId UNIQUEIDENTIFIER NOT NULL,
    IsActive BIT NOT NULL DEFAULT 1,
    AssignedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    
    CONSTRAINT FK_SupervisorBus_Supervisor FOREIGN KEY (SupervisorId) REFERENCES Supervisors(SupervisorId),
    CONSTRAINT FK_SupervisorBus_Bus FOREIGN KEY (BusId) REFERENCES Buses(BusId),
    CONSTRAINT UQ_SupervisorBus UNIQUE (SupervisorId, BusId)
);

-- Attendance Sessions
CREATE TABLE AttendanceSessions (
    SessionId UNIQUEIDENTIFIER PRIMARY KEY,  -- From client
    SupervisorId UNIQUEIDENTIFIER NOT NULL,
    BusId UNIQUEIDENTIFIER NOT NULL,
    PeriodId UNIQUEIDENTIFIER NOT NULL,
    LocationId UNIQUEIDENTIFIER,
    
    AttendanceDate DATE NOT NULL,
    AttendanceType NVARCHAR(50) NOT NULL,  -- Arrival, Return
    UnregisteredStudentsCount INT NOT NULL DEFAULT 0,
    
    SyncStatus NVARCHAR(50) NOT NULL DEFAULT 'Synced',
    
    CreatedAt DATETIME2 NOT NULL,
    CreatedOffline BIT NOT NULL DEFAULT 0,
    SyncedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    
    CONSTRAINT FK_AttSession_Supervisor FOREIGN KEY (SupervisorId) REFERENCES Supervisors(SupervisorId),
    CONSTRAINT FK_AttSession_Bus FOREIGN KEY (BusId) REFERENCES Buses(BusId),
    CONSTRAINT FK_AttSession_Period FOREIGN KEY (PeriodId) REFERENCES Periods(PeriodId),
    
    INDEX IX_AttSession_Date (AttendanceDate),
    INDEX IX_AttSession_Bus (BusId),
    CONSTRAINT UQ_AttSession UNIQUE (BusId, AttendanceDate, AttendanceType)
);

-- Attendance Records
CREATE TABLE AttendanceRecords (
    RecordId UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    SessionId UNIQUEIDENTIFIER NOT NULL,
    StudentId UNIQUEIDENTIFIER NOT NULL,
    
    AttendanceStatus NVARCHAR(50) NOT NULL DEFAULT 'Present',
    Notes NVARCHAR(500),
    
    CONSTRAINT FK_AttRecord_Session FOREIGN KEY (SessionId) REFERENCES AttendanceSessions(SessionId) ON DELETE CASCADE,
    CONSTRAINT FK_AttRecord_Student FOREIGN KEY (StudentId) REFERENCES Students(StudentId),
    
    INDEX IX_AttRecord_Session (SessionId),
    INDEX IX_AttRecord_Student (StudentId)
);
```

---

## API Endpoints (Phase 5)

### Sync

| Method | Endpoint | Description | Auth |
|--------|----------|-------------|------|
| POST | `/api/attendance/sync` | Sync single session | Supervisor |
| POST | `/api/attendance/sync/batch` | Sync multiple sessions | Supervisor |
| GET | `/api/attendance/sync/status` | Get sync status | Supervisor |

### Attendance

| Method | Endpoint | Description | Auth |
|--------|----------|-------------|------|
| GET | `/api/attendance/sessions` | List sessions | Yes |
| GET | `/api/attendance/sessions/{id}` | Get session details | Yes |
| GET | `/api/attendance/by-bus/{busId}` | Attendance by bus | Yes |
| GET | `/api/attendance/by-date/{date}` | Attendance by date | Yes |
| GET | `/api/attendance/weekly` | Weekly summary | Staff |

### Supervisor

| Method | Endpoint | Description | Auth |
|--------|----------|-------------|------|
| GET | `/api/supervisors/me` | Current supervisor info | Supervisor |
| GET | `/api/supervisors/me/buses` | Assigned buses | Supervisor |
| GET | `/api/supervisors/me/students/{busId}` | Students for caching | Supervisor |

---

## Frontend Components

### Attendance Capture Interface

```vue
<!-- AttendanceView.vue -->
<template>
  <div class="attendance-view min-h-screen bg-base-200">
    <!-- Offline Banner -->
    <div v-if="!isOnline" class="alert alert-warning rounded-none">
      <span>⚠️ وضع غير متصل - سيتم المزامنة عند الاتصال</span>
    </div>
    
    <!-- Sync Status Bar -->
    <div class="bg-base-100 p-2 flex justify-between items-center border-b">
      <SyncStatusIndicator :status="syncStatus" />
      <button class="btn btn-sm btn-ghost" @click="manualSync" :disabled="!isOnline">
        🔄 مزامنة
      </button>
    </div>
    
    <!-- Period & Location Selection -->
    <div class="p-4 bg-base-100 shadow">
      <div class="flex gap-2">
        <select v-model="selectedPeriod" class="select select-bordered flex-1">
          <option v-for="p in periods" :value="p.id">{{ p.nameAr }}</option>
        </select>
        <select v-model="selectedLocation" class="select select-bordered flex-1">
          <option v-for="l in locations" :value="l.code">{{ l.name }}</option>
        </select>
      </div>
    </div>
    
    <!-- Bus Selection -->
    <div class="p-4">
      <h2 class="text-lg font-bold mb-2">اختر الباص</h2>
      <div class="grid grid-cols-2 gap-2">
        <button v-for="bus in assignedBuses" 
                :key="bus.busId"
                class="btn btn-lg"
                :class="getBusButtonClass(bus)"
                @click="selectBus(bus)">
          <span class="text-2xl">{{ bus.busNumber }}</span>
          <span v-if="bus.isCompleted" class="text-xl">✓</span>
        </button>
      </div>
    </div>
    
    <!-- Student List (when bus selected) -->
    <div v-if="selectedBus" class="p-4">
      <div class="flex justify-between items-center mb-4">
        <h2 class="text-lg font-bold">الباص {{ selectedBus.busNumber }}</h2>
        <span class="badge badge-lg">{{ presentCount }}/{{ students.length }}</span>
      </div>
      
      <!-- Student Cards -->
      <div class="space-y-2">
        <div v-for="student in students" 
             :key="student.studentId"
             class="card bg-base-100 shadow cursor-pointer"
             :class="{ 'ring-2 ring-success': student.isPresent, 'opacity-50': !student.isPresent }"
             @click="toggleAttendance(student)">
          <div class="card-body p-4 flex-row items-center justify-between">
            <div>
              <h3 class="font-bold">{{ student.studentName }}</h3>
              <p class="text-sm opacity-70">{{ student.districtName }}</p>
            </div>
            <div class="text-3xl">
              {{ student.isPresent ? '✓' : '✗' }}
            </div>
          </div>
        </div>
      </div>
      
      <!-- Unregistered Count -->
      <div class="form-control mt-4">
        <label class="label">طلاب غير مسجلين</label>
        <input type="number" 
               v-model.number="unregisteredCount" 
               class="input input-bordered input-lg"
               min="0" />
      </div>
      
      <!-- Save Button -->
      <button class="btn btn-primary btn-lg w-full mt-4" 
              @click="saveAttendance"
              :disabled="saving">
        {{ saving ? 'جاري الحفظ...' : 'حفظ الحضور' }}
      </button>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, computed, onMounted } from 'vue';
import { db } from '@/services/offline/db';
import { syncService } from '@/services/offline/sync.service';

const isOnline = ref(navigator.onLine);
const selectedPeriod = ref('');
const selectedLocation = ref('');
const selectedBus = ref(null);
const students = ref([]);
const unregisteredCount = ref(0);
const saving = ref(false);

// All students default to present
onMounted(async () => {
  students.value = students.value.map(s => ({ ...s, isPresent: true }));
});

const toggleAttendance = (student) => {
  student.isPresent = !student.isPresent;
};

const presentCount = computed(() => 
  students.value.filter(s => s.isPresent).length
);

const saveAttendance = async () => {
  saving.value = true;
  
  const session = {
    sessionId: crypto.randomUUID(),
    supervisorId: currentSupervisorId,
    busId: selectedBus.value.busId,
    periodId: selectedPeriod.value,
    locationCode: selectedLocation.value,
    attendanceDate: new Date().toISOString().split('T')[0],
    attendanceType: 'Arrival', // or Return based on context
    unregisteredCount: unregisteredCount.value,
    createdAt: new Date().toISOString()
  };
  
  const records = students.value.map(s => ({
    sessionId: session.sessionId,
    studentId: s.studentId,
    studentName: s.studentName,
    attendanceStatus: s.isPresent ? 'Present' : 'Absent'
  }));
  
  await syncService.saveAttendance(session, records);
  
  saving.value = false;
  // Show success, mark bus as complete, reset
};
</script>
```

### Sync Status Indicator

```vue
<!-- SyncStatusIndicator.vue -->
<template>
  <div class="flex items-center gap-2">
    <div class="flex items-center gap-1">
      <span v-if="status.syncing > 0" class="loading loading-spinner loading-sm"></span>
      <span v-else-if="status.pending > 0" class="text-warning">⏳</span>
      <span v-else-if="status.failed > 0" class="text-error">❌</span>
      <span v-else class="text-success">✓</span>
    </div>
    
    <span class="text-sm">
      <span v-if="status.syncing > 0">جاري المزامنة ({{ status.syncing }})</span>
      <span v-else-if="status.pending > 0">في الانتظار ({{ status.pending }})</span>
      <span v-else-if="status.failed > 0" class="text-error">فشل ({{ status.failed }})</span>
      <span v-else-if="status.lastSyncTime">آخر مزامنة: {{ formatTime(status.lastSyncTime) }}</span>
      <span v-else>متزامن</span>
    </span>
  </div>
</template>
```

---

## Acceptance Criteria

### Sprint 5.1

- [ ] IndexedDB stores created successfully
- [ ] Data persists after browser close/refresh
- [ ] Sync queue tracks pending items
- [ ] Offline mode detected automatically
- [ ] Sync endpoint rejects duplicates (idempotent)

### Sprint 5.2

- [ ] Supervisor sees only assigned buses
- [ ] Student list loads < 200ms from cache
- [ ] All students pre-selected as Present
- [ ] Tap toggles to Absent instantly
- [ ] Unregistered count editable
- [ ] Full bus attendance < 5 seconds

### Sprint 5.3

- [ ] Auto-sync on network reconnect
- [ ] Manual sync button works
- [ ] Exponential backoff for failures
- [ ] Failed syncs displayed to user
- [ ] Retry failed items manually
- [ ] Zero data loss verified

### Sprint 5.4

- [ ] Weekly view shows 7 days
- [ ] Leave students marked differently
- [ ] Consecutive absences highlighted
- [ ] Suspend from review works
- [ ] Export to Excel functional

---

## Achievement Tracking

### Week 11 Progress

| Category | Planned | Completed | Percentage |
|----------|---------|-----------|------------|
| Backend Tasks | 7 | 0 | 0% |
| Frontend Tasks | 7 | 0 | 0% |
| **Total** | **14** | **0** | **0%** |

### Week 12 Progress

| Category | Planned | Completed | Percentage |
|----------|---------|-----------|------------|
| Backend Tasks | 5 | 0 | 0% |
| Frontend Tasks | 9 | 0 | 0% |
| **Total** | **14** | **0** | **0%** |

### Week 13 Progress

| Category | Planned | Completed | Percentage |
|----------|---------|-----------|------------|
| Backend Tasks | 6 | 0 | 0% |
| Frontend Tasks | 8 | 0 | 0% |
| **Total** | **14** | **0** | **0%** |

### Week 14 Progress

| Category | Planned | Completed | Percentage |
|----------|---------|-----------|------------|
| Backend Tasks | 5 | 0 | 0% |
| Frontend Tasks | 8 | 0 | 0% |
| **Total** | **13** | **0** | **0%** |

---

## Phase Summary

### Overall Progress

```
Phase 5 Completion: ░░░░░░░░░░ 0%
```

| Sprint | Status | Completion |
|--------|--------|------------|
| Sprint 5.1 - Offline Infrastructure | 🔴 Not Started | 0% |
| Sprint 5.2 - Attendance Interface | 🔴 Not Started | 0% |
| Sprint 5.3 - Synchronization | 🔴 Not Started | 0% |
| Sprint 5.4 - Weekly Review | 🔴 Not Started | 0% |

### Key Milestones

- [ ] 🎯 Offline-first infrastructure working
- [ ] 🎯 < 5 second attendance entry
- [ ] 🎯 Zero data loss sync system
- [ ] 🎯 Weekly review with suspension

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
