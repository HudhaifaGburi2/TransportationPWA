# Functional Requirements Document (FRD)

## Project Title

Transport Unit Management System (TUMS)

## Document Purpose

This Functional Requirements Document (FRD) translates the approved Business Requirements Document (BRD) into detailed, system-level functional requirements. It defines **what the system must do**, how users interact with it, and the rules governing system behavior.

---

## 1. System Overview

The Transport Unit Management System is a web-based Progressive Web Application (PWA) designed to manage student transportation, attendance, operational updates, and reporting. The system supports multiple user roles with role-based access and operates under different operational modes (normal, merged buses, summer program).

---

## 2. User Roles and Permissions

### 2.1 Registration & Statistics Officer

* Manage student records
* Assign and transfer students between buses
* Suspend and reactivate students
* Manage student leave records
* Perform weekly attendance cleanup
* View statistics and reports

### 2.2 Supervisor (Attendance Monitor)

* Record daily attendance (arrival and return)
* View bus and student information
* Enter unregistered student counts

### 2.3 System Administrator (Phase 2)

* Manage system configuration
* Define periods and routes
* Manage users and permissions

---

## 3. Functional Requirements

### 3.1 Authentication & Authorization

**FR-01** The system shall require user authentication.

**FR-02** The system shall enforce role-based access control.

**FR-03** Users shall only access functions assigned to their role.

---

### 3.2 Student Management

**FR-04** The system shall allow registration staff to create student records.

**FR-05** The system shall store mandatory student attributes:

* Full name
* Mobile number
* Registration number
* Class/Circle number
* District
* Home location
* Assigned transport period(s)

**FR-06** The system shall support student status values:

* Active
* Suspended
* On Leave

**FR-07** The system shall allow searching students by name, mobile number, or registration number.

---

### 3.3 Bus and Period Management

**FR-08** The system shall display transport periods:

* Fajr
* Duha
* Asr
* Maghrib
* Isha

**FR-09** The system shall display buses grouped by selected period.

**FR-10** For each bus, the system shall display:

* Bus number
* Route
* Driver name
* Driver mobile number
* Number of assigned students

**FR-11** The system shall display a detailed bus view showing assigned students.

---

### 3.4 Student Assignment Operations

**FR-12** The system shall allow adding a student to a bus.

**FR-13** The system shall allow transferring a student between buses.

**FR-14** The system shall allow assigning different buses for arrival and return.

**FR-15** The system shall visually distinguish students with split transport (arrival/return).

---

### 3.5 Student Suspension Management

**FR-16** The system shall allow registration staff to suspend a student from a bus.

**FR-17** The system shall populate a default suspension reason: "Absent for three consecutive days".

**FR-18** The system shall allow editing the suspension reason before confirmation.

**FR-19** Upon suspension, the system shall:

* Remove the student from the active bus
* Add the student to the suspended students list
* Record suspension date and reason

**FR-20** The system shall provide a dedicated suspended students list.

---

### 3.6 Student Reactivation

**FR-21** The system shall allow reactivating suspended students.

**FR-22** The system shall display the previous bus assignment during reactivation.

**FR-23** The system shall allow changing the bus during reactivation.

---

### 3.7 Student Leave Management

**FR-24** The system shall allow creating student leave records.

**FR-25** Leave records shall require:

* Leave reason
* Start date
* End date
* Attachment

**FR-26** The system shall visually mark students on leave.

**FR-27** Students on leave shall be excluded from absence calculations.

---

### 3.8 Attendance Management – Supervisors

**FR-28** The system shall allow supervisors to select period and bus.

**FR-29** The system shall display student lists including:

* Name
* Mobile number
* District
* Class/Circle number

**FR-30** The system shall support attendance statuses:

* Present (default)
* Excused
* Late
* Absent

**FR-31** The system shall allow independent attendance recording for arrival and return.

**FR-32** The system shall allow entering a numeric count of unregistered boarding students.

**FR-33** The system shall include unregistered students in attendance totals.

**FR-34** The system shall visually mark buses with completed attendance.

---

### 3.9 Weekly Attendance Review

**FR-35** The system shall provide a weekly attendance review screen.

**FR-36** The review screen shall display attendance by:

* Period
* Bus
* Date

**FR-37** Students on leave shall be visually distinguished in the review.

**FR-38** The system shall allow suspending students directly from the review screen.

---

### 3.10 Statistics and Reporting

**FR-39** The system shall provide student counts per bus.

**FR-40** The system shall provide student counts per stop.

**FR-41** The system shall provide student counts per period.

**FR-42** The system shall provide aggregated totals for:

* Morning periods
* Evening periods

**FR-43** The system shall provide a daily summary report including:

* Total buses
* Total students (morning and evening)

---

### 3.11 Bus Merging Operations

**FR-44** The system shall provide a dedicated bus merging interface.

**FR-45** The system shall allow temporary merging of buses with similar routes.

**FR-46** The system shall allow restoring original bus configurations.

---

### 3.12 Summer Program Mode

**FR-47** The system shall support a summer program operational mode.

**FR-48** The system shall load permanent data as a baseline for summer mode.

**FR-49** The system shall isolate summer data from permanent records.

---

## 4. Non-Functional Functional Constraints

**FR-50** The system shall support PWA offline caching for read-only views.

**FR-51** The system shall respond to standard user actions within acceptable performance limits.

---

## 5. Traceability

Each functional requirement in this document maps directly to business objectives defined in the BRD and will be traceable to system design, development tasks, and test cases.

---
Below is a **formal Functional Requirements (FR) specification** for the **new features you described**, written in clear technical English and aligned with your existing **FRD / Architecture**.
This can be **added as a new FR section** to the FRD or used as a standalone appendix.

---

# Functional Requirements – Student Registration & Offline Attendance

## FR-A: Student Self-Registration Feature

### FR-A1: Student Registration Request

* The system shall allow students to submit a **transport registration request** to the Registration Department.
* The registration form shall be accessible via **mobile devices (PWA)**.

### FR-A2: Read-Only Student Data Source

* The student registration form shall preload student information in **read-only mode** from the **central database view**:

  ```
  vw_Student_Halaqa_Teacher_information_Transportation_Dep
  ```

* The following information shall be fetched and displayed automatically:

  * Student name
  * Registration number
  * Halaqa / class
  * Teacher information
  * Any other fields exposed by the view

* The system shall **not allow modification** of centrally sourced data.

### FR-A3: District Selection

* The student shall be required to select their **district** from a predefined list.
* Districts shall be loaded from the **Transportation Database** (read/write).

### FR-A4: GPS Location Capture

* The system shall allow students to capture their **exact home location** using:

  * Mobile GPS (HTML5 Geolocation API)
* GPS coordinates (latitude, longitude) shall be stored with the request.
* Manual map pin adjustment shall be supported (if GPS accuracy is low).

### FR-A5: Registration Request Status

* Each registration request shall have a status:

  * Pending
  * Approved
  * Rejected

---

## FR-B: Admin Review & Bus Assignment

### FR-B1: Admin Review Queue

* Administrators shall have access to a **registration requests dashboard**.
* Requests shall be filterable by:

  * District
  * Status
  * Date

### FR-B2: Approve / Reject Request

* Admins shall be able to:

  * Approve a request
  * Reject a request with a mandatory rejection reason

### FR-B3: Bus Assignment on Approval

* Upon approval, the admin shall assign the student to a **bus responsible for the selected district**.
* Bus and district mappings shall be stored in the **Transportation Database**.
* The system shall prevent assigning a student to a bus outside their district unless explicitly overridden.

### FR-B4: Audit Trail

* All admin actions (approve / reject / assign bus) shall be logged with:

  * Admin user
  * Timestamp
  * Action details

---

## FR-C: Attendance Management (High-Performance & Offline)

### FR-C1: Attendance Scope

* Attendance shall be recorded **per bus**, where each bus typically has **10–15 students**.
* Attendance locations are **basements of mosques**, with parking areas such as:

  * A8–A11
  * B8–B11
* The system shall support adding **future parking locations** dynamically.

### FR-C2: Supervisor Assignment

* Each Supervisor shall be assigned to:

  * A group of buses
  * A specific parking/location
* Multiple supervisors may operate **simultaneously in the same location**.

### FR-C3: Offline-First Attendance

* The attendance module shall function with:

  * Weak internet
  * Intermittent internet
  * No internet
* Attendance data shall be stored **locally on the device** when offline.

### FR-C4: Local Data Persistence

* The system shall use local storage (IndexedDB or equivalent) to:

  * Save attendance records
  * Prevent data loss on page refresh or app close
* Data shall remain available until successfully synced.

### FR-C5: Data Synchronization

* When internet connectivity is restored:

  * The system shall automatically sync pending attendance records
  * Conflict resolution shall prioritize:

    * Latest timestamp
    * Supervisor ownership
* Sync status shall be visible to the supervisor (e.g., “Pending Sync”, “Synced”).

---

## FR-D: Fast Bulk Attendance Entry

### FR-D1: Default Attendance State

* When a supervisor opens a bus attendance screen:

  * All students shall be **selected as Present by default**.

### FR-D2: Bulk Attendance Action

* The supervisor shall be able to:

  * Select all students in a bus with one action
  * Deselect only absent students
* This enables **one-click bulk attendance marking**.

### FR-D3: Attendance Performance

* The system shall allow marking attendance for a full bus (10–15 students) in **under 5 seconds**.
* No individual per-student save operations shall be required.

### FR-D4: Minimal UI Interaction

* Attendance UI shall:

  * Avoid page reloads
  * Use large touch-friendly controls
  * Work efficiently in low-light basement environments

---

## FR-E: Reliability & Data Integrity

### FR-E1: No Data Loss Guarantee

* Attendance records shall never be lost due to:

  * Network failure
  * App refresh
  * Device sleep

### FR-E2: Supervisor Accountability

* Each attendance record shall be linked to:

  * Supervisor ID
  * Bus
  * Location
  * Date and time

### FR-E3: Sync Validation

* The backend shall validate:

  * Duplicate submissions
  * Partial sync failures
* Supervisors shall be notified of sync success or failure.

---

## FR-F: Non-Functional (Critical)

* Attendance module must be **offline-first**
* Sync must be **asynchronous and non-blocking**
* UI must be optimized for **speed, not form-heavy input**
* System must scale to:

  * Many supervisors
  * Same location
  * Same time window

---

## Summary

This feature set introduces:

* Student self-registration integrated with a **central read-only DB**
* Admin-controlled approval and bus assignment
* **Mission-critical offline attendance**
* High-speed bulk attendance optimized for real-world conditions

---

### Next logical steps (recommended)

1. **Offline Sync Technical Design (IndexedDB + Sync Queue)**
2. **ERD additions for Registration Requests & Attendance Sync**
3. **Attendance UX wireframe (1-screen flow)**
4. **API design for offline batch sync**

Tell me which one you want next, and I will design it at **implementation level**.
