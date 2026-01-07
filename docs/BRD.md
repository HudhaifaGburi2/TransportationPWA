# Business Requirements Document (BRD)

## Project Title

Transport Unit Management System (TUMS)

## Document Purpose

This Business Requirements Document (BRD) defines the business needs, operational requirements, and high‑level functional scope for the Transport Unit Management System. The document serves as a reference for stakeholders, developers, and project managers to ensure a shared understanding of system objectives and requirements.

---

## 1. Business Background

The Transport Unit manages daily transportation for students of Quran and Islamic Studies circles. Current manual and semi‑manual processes lead to operational inefficiencies, delayed reporting, and a high risk of human error. A centralized digital system is required to manage student transportation, attendance, and reporting in a scalable and flexible manner.

---

## 2. Business Objectives

The system shall:

* Provide an easy‑to‑use interface for all staff roles
* Deliver fast response times and smooth navigation
* Simplify data entry and retrieval
* Support all departments involved in transport operations
* Allow future expansion and operational changes without major rework

---

## 3. Scope

### 3.1 In Scope

* Student transport management
* Bus and route organization by period
* Attendance tracking (arrival and return)
* Student suspension, reactivation, and leave management
* Weekly absence review and cleanup
* Statistical reports and daily summaries
* Temporary operational modes (bus merging, summer program)

### 3.2 Out of Scope

* Payroll management
* Vehicle maintenance tracking
* GPS or real‑time bus tracking (future consideration)

---

## 4. Stakeholders

* Transport Unit Management
* Registration and Statistics Department
* Supervisors (Attendance Monitors)
* Technology Department
* System Administrators

---

## 5. User Roles

### 5.1 Registration & Statistics Staff

Responsible for managing students, buses, suspensions, reactivations, leave records, and reports.

### 5.2 Supervisors

Responsible for daily attendance tracking and reporting at the bus level.

### 5.3 Administrators (Future)

Responsible for configuration, system settings, and master data management.

---

## 6. Business Requirements

### 6.1 Student Management

The system must:

* Store student personal and transport data
* Assign students to buses by period
* Support student status changes:

  * Active
  * Suspended
  * On Leave

Student master data includes:

* Full name
* Mobile number
* Registration number
* Class/Circle number
* District
* Home location
* Assigned period(s)

---

### 6.2 Bus and Period Management

The system must:

* Organize buses by period:

  * Fajr
  * Duha
  * Asr
  * Maghrib
  * Isha
* Display for each bus:

  * Bus number
  * Route
  * Assigned driver and contact number
  * Total number of students

---

### 6.3 Registration Operations

Registration staff must be able to:

* Add new students to buses
* Transfer students between buses
* Suspend students with a recorded reason
* Reactivate suspended students
* Grant student leave with justification and attachments

Default suspension reason:

* "Absent for three consecutive days" (editable)

Suspended students must be:

* Removed from active bus lists
* Added to a separate suspended students list
* Logged with suspension date

---

### 6.4 Student Leave Management

The system must:

* Allow leave requests from teachers and registration staff
* Require leave reason, date range, and attachment
* Visually distinguish students on leave
* Exclude students on leave from absence calculations

---

### 6.5 Attendance Management (Supervisors)

The system must allow supervisors to:

* Select period and bus
* View student lists with key identifiers
* Record attendance statuses:

  * Present (default)
  * Excused
  * Late
  * Absent

Attendance rules:

* Arrival and return attendance are independent
* Unregistered boarding students can be added numerically
* Completed attendance records must be visually indicated

---

### 6.6 Weekly Absence Review

The system must:

* Provide a consolidated weekly view of attendance
* Clearly mark students on leave
* Allow direct suspension of absent students from the same interface
* Require suspension reason entry

---

### 6.7 Statistical Reports and Harvesting

The system must provide:

* Student counts per bus, stop, and period
* Aggregated totals by:

  * Period
  * Morning sessions
  * Evening sessions
* A daily report page displaying:

  * Total buses
  * Total students (morning and evening)

---

### 6.8 Bus Merging Operations

The system must:

* Support temporary merging of buses with similar routes
* Provide a dedicated interface for merging operations
* Allow restoration to original bus configurations

---

### 6.9 Summer Program Mode

The system must:

* Support a temporary summer operational mode
* Load original data as a baseline
* Allow temporary additions and removals of students
* Isolate summer data from permanent operational records

---

## 7. Non‑Functional Requirements

* Web‑based Progressive Web Application (PWA)
* Responsive design (mobile, tablet, desktop)
* High performance and fast load times
* Secure role‑based access control
* Auditability for critical actions
* Scalable architecture for future enhancements

---

## 8. Assumptions and Constraints

* Users have basic digital literacy
* Internet access is generally available, with limited offline usage
* System will be developed using:

  * Vue.js (PWA Frontend)
  * .NET Web API (Backend)
  * SQL Server (Database)

---

## 9. Success Criteria

The system will be considered successful if:

* Daily transport operations are fully digitized
* Attendance and reporting accuracy improves
* Weekly cleanup operations are simplified
* Users can perform tasks with minimal training
* The system supports seasonal operational changes without data loss

---

## 10. Approval

This document is subject to review and approval by all key stakeholders before design and development phases begin.
