# Transport Unit Management System

## User Flow Diagrams (Per Role) & System Architecture Diagram

---

# Part A: User Flow Diagrams (Per Role)

> Note: The following are **logical user flow diagrams** expressed in structured flow format. They are suitable for UX design, wireframing, and validation with business stakeholders.

---

## 1. Registration & Statistics Officer – User Flows

### 1.1 Login & Dashboard Flow

Start
→ Login
→ System validates credentials
→ Load Registration Dashboard
→ Display:

* Periods summary
* Buses count
* Suspended students count
* Quick actions

End

---

### 1.2 Add New Student Flow

Start
→ Select Period
→ Select Bus
→ Click "Add Student"
→ Search Student (Registration No / Mobile / Name)
→ Student Found?

* No → Show error message
* Yes → Display student details
  → Confirm Add
  → Assign student to bus
  → Update bus student count
  → Show success confirmation

End

---

### 1.3 Transfer Student Flow

Start
→ Select Period
→ Select Bus
→ Select Student
→ Click "Transfer"
→ Select Target Bus
→ Confirm Transfer
→ Remove student from current bus
→ Add student to target bus
→ Update both bus counts

End

---

### 1.4 Suspend Student Flow

Start
→ Select Period
→ Select Bus
→ Select Student
→ Click "Suspend"
→ System shows default reason: "Absent for three consecutive days"
→ Edit reason (optional)
→ Confirm Suspension
→ Remove student from bus
→ Add student to Suspended List
→ Record suspension date & reason

End

---

### 1.5 Reactivate Student Flow

Start
→ Open Suspended Students List
→ Search by Registration Number
→ Select Student
→ System displays previous bus
→ Change bus (optional)
→ Confirm Reactivation
→ Assign student to selected bus
→ Remove student from Suspended List

End

---

### 1.6 Grant Student Leave Flow

Start
→ Select Student
→ Click "Grant Leave"
→ Enter Leave Reason
→ Attach Supporting Document
→ Select From Date / To Date
→ Confirm Leave
→ Mark student as "On Leave"
→ Display student in distinct color

End

---

### 1.7 Weekly Attendance Review Flow

Start
→ Open Weekly Review Screen
→ Select Period
→ Select Bus
→ Select Date Range
→ System displays attendance records
→ Students on leave shown in different color
→ Select Absent Student
→ Click "Suspend"
→ Enter Suspension Reason
→ Confirm

End

---

## 2. Supervisor (Attendance Monitor) – User Flows

### 2.1 Attendance Recording Flow (Arrival / Return)

Start
→ Login
→ Select Period
→ Select Bus
→ System loads student list
→ Default status: Present
→ Update status per student:

* Present
* Excused
* Late
* Absent
  → Enter number of unregistered boarding students
  → Save Attendance
  → System calculates totals
  → Mark bus as "Attendance Completed"

End

---

### 2.2 Return Attendance Flow

Start
→ Select Period
→ Select Bus
→ Select "Return Attendance"
→ Load student list
→ Record return statuses
→ Save
→ Update return totals

End

---

## 3. Administrator (Phase 2) – User Flows

### 3.1 System Configuration Flow

Start
→ Login as Admin
→ Open System Settings
→ Manage:

* Periods
* Routes
* Buses
* Users & Roles
  → Save Configuration

End

---

# Part B: System Architecture Diagram (Logical)

## 1. High-Level Architecture Overview

Client Layer (Frontend)
→ Application Layer (Backend API)
→ Data Layer (Database)

---

## 2. Architecture Components

### 2.1 Client Layer (Frontend)

* Progressive Web Application (PWA)
* Built with Vue.js
* Responsibilities:

  * User Interface
  * Role-based views
  * Client-side validation
  * Offline read-only cache
  * API communication

Devices:

* Mobile
* Tablet
* Desktop

---

### 2.2 Application Layer (Backend)

* .NET Web API
* Clean Architecture pattern

Logical Layers:

1. API Layer

   * Controllers
   * Authentication & Authorization

2. Application Layer

   * Use Cases
   * Business Rules
   * Validation

3. Domain Layer

   * Entities (Student, Bus, Attendance, etc.)
   * Domain Logic

4. Infrastructure Layer

   * Database access
   * File storage (attachments)
   * Logging & auditing

---

### 2.3 Data Layer

* SQL Server
* Responsibilities:

  * Persistent storage
  * Historical data
  * Audit logs

Core Data Groups:

* Students
* Buses & Routes
* Attendance Records
* Suspensions
* Leave Records
* Seasonal Data (Summer / Merged Buses)

---

## 3. Data Flow Overview

User Action (Frontend)
→ API Request (HTTPS)
→ Authentication & Authorization
→ Business Logic Execution
→ Database Read/Write
→ API Response
→ UI Update

---

## 4. Cross-Cutting Concerns

* Security (JWT / Role-based access)
* Performance optimization
* Audit logging for critical actions
* Error handling & monitoring

---

## 5. Deployment Overview (Logical)

* Frontend hosted as PWA
* Backend hosted on Application Server
* Database hosted on SQL Server
* Optional future integration:

  * Reporting services
  * SMS / Notification services

---

## 6. Traceability

* User flows map directly to FRD requirements
* Architecture supports scalability, performance, and seasonal operational modes

---

## 7. Approval

This document is subject to validation prior to UX design and technical implementation.
