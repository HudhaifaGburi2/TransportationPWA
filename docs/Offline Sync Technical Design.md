# Offline Sync Technical Design

## Attendance Module (IndexedDB + Sync Queue)

---

## 1. Design Objectives

This document defines the technical design for **offline-first attendance capture and synchronization**, optimized for:

* Weak or no internet connectivity
* Fast bulk attendance entry
* Zero data loss
* Multi-supervisor, multi-location concurrency

The design applies to the **Supervisor Attendance Module** within the Transport Unit Management System.

---

## 2. Offline-First Strategy

### Core Principle

> **Local-first, sync-later**

All attendance operations are executed locally on the supervisor’s device and synchronized asynchronously with the backend when connectivity becomes available.

---

## 3. Technology Stack

### Client Side (PWA)

* Vue 3
* IndexedDB (via Dexie.js or equivalent)
* Service Worker
* Background Sync (where supported)

### Server Side

* .NET Web API
* SQL Server
* Idempotent sync endpoints

---

## 4. Local Data Model (IndexedDB)

### 4.1 Database Name

```
transport_attendance_db
```

### 4.2 Object Stores

#### 4.2.1 attendance_sessions

Stores one attendance operation per bus.

Fields:

* session_id (UUID, PK)
* supervisor_id
* bus_id
* period
* location_code (A8, A9, B10, etc.)
* attendance_type (arrival | return)
* created_at (timestamp)
* status (draft | pending_sync | synced | error)

---

#### 4.2.2 attendance_records

Stores per-student attendance data.

Fields:

* record_id (UUID, PK)
* session_id (FK)
* student_id
* attendance_status (present | absent | late | excused)

---

#### 4.2.3 sync_queue

Controls synchronization lifecycle.

Fields:

* queue_id (UUID, PK)
* entity_type (attendance_session)
* entity_id (session_id)
* retry_count
* last_attempt_at
* sync_status (pending | syncing | failed)

---

## 5. Attendance Capture Flow (Offline)

1. Supervisor selects:

   * Location
   * Period
   * Bus
2. System loads student list from local cache
3. All students default to **Present**
4. Supervisor deselects absent students
5. Supervisor clicks **Save Attendance**
6. System:

   * Creates attendance_session
   * Creates attendance_records in bulk
   * Pushes session_id to sync_queue
   * Marks session as pending_sync

> No network request is required at this stage.

---

## 6. Bulk Attendance Optimization

* UI uses **single state object** per bus
* One save action per bus (no per-student API calls)
* IndexedDB bulkPut operations
* Write latency target: < 100 ms

---

## 7. Sync Trigger Mechanisms

Synchronization is triggered by:

* Network reconnect event
* App startup
* Manual “Sync Now” action
* Background Sync (if supported)

---

## 8. Sync Execution Flow

1. Read oldest pending item from sync_queue
2. Lock queue item (syncing)
3. Load attendance_session + records
4. POST to backend sync endpoint
5. Backend validates idempotency
6. On success:

   * Mark session as synced
   * Remove item from sync_queue
7. On failure:

   * Increment retry_count
   * Mark status as failed

---

## 9. Backend Sync API Design

### Endpoint

```
POST /api/attendance/sync
```

### Payload Structure

```
{
  "sessionId": "uuid",
  "supervisorId": "uuid",
  "busId": "uuid",
  "period": "Asr",
  "attendanceType": "arrival",
  "locationCode": "A9",
  "timestamp": "ISO-8601",
  "records": [
    { "studentId": "uuid", "status": "present" },
    { "studentId": "uuid", "status": "absent" }
  ]
}
```

---

## 10. Idempotency & Conflict Handling

### 10.1 Idempotency

* session_id is globally unique
* Backend rejects duplicate session submissions

### 10.2 Conflict Resolution Rules

Priority order:

1. Latest timestamp
2. Supervisor ownership
3. Manual admin override (if needed)

---

## 11. Error Handling & Retry Policy

* Max retries: configurable (default = 5)
* Exponential backoff
* Failed syncs visible in UI
* Manual retry supported

---

## 12. Supervisor UX Indicators

* Offline banner (No Internet)
* Sync status icons:

  * Pending
  * Syncing
  * Synced
  * Error
* Last sync timestamp

---

## 13. Security Considerations

* JWT stored securely
* Tokens refreshed before sync
* Local data encrypted (where supported)

---

## 14. Performance Targets

* Attendance entry per bus: < 5 seconds
* Local save latency: < 100 ms
* Sync payload size optimized (compressed JSON)

---

## 15. Future Enhancements

* Background Sync API
* Push notifications for sync failures
* GPS-based location validation

---

## 16. Traceability

* Aligns with FR-C (Offline Attendance)
* Aligns with FR-D (Bulk Attendance)
* Aligns with Architecture Diagram

---

## 17. Approval

This technical design is subject to review prior to development implementation.
