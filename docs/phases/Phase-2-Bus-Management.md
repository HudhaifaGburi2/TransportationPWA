# Phase 2: Bus Management

## Weeks 4-5 | CRUD Operations & Statistics

---

## Phase Overview

| Attribute | Details |
|-----------|---------|
| **Duration** | 2 Weeks |
| **Start Date** | _TBD_ |
| **End Date** | _TBD_ |
| **Status** | 🔴 Not Started |
| **Progress** | 0% |
| **Dependencies** | Phase 1 Complete |

---

## Objectives

1. Implement complete Bus entity management (CRUD operations)
2. Create Route management for bus assignments
3. Establish bus-district associations for service coverage
4. Build period-based bus organization and display
5. Implement bus statistics and utilization reporting
6. Create visual route display with map integration

---

## Sprint 2.1: Bus Core Features (Week 4)

### Backend Tasks

| Task | Status | Assignee | Notes |
|------|--------|----------|-------|
| Implement Bus entity and repository | ⬜ Pending | | Bus model with all fields |
| Create Route entity and management | ⬜ Pending | | Route CRUD operations |
| Implement BusService with CRUD operations | ⬜ Pending | | Business logic layer |
| Create BusesController | ⬜ Pending | | REST API endpoints |
| Implement bus-district associations | ⬜ Pending | | BusDistricts junction table |
| Add validation rules | ⬜ Pending | | FluentValidation rules |
| Create bus capacity validation | ⬜ Pending | | Prevent over-assignment |

### Frontend Tasks

| Task | Status | Assignee | Notes |
|------|--------|----------|-------|
| Create bus list view (grouped by period) | ⬜ Pending | | Period tabs/accordion |
| Build bus detail view | ⬜ Pending | | Full bus information |
| Create bus form (add/edit) | ⬜ Pending | | Modal or page form |
| Implement bus card component | ⬜ Pending | | Reusable bus display |
| Add period selector component | ⬜ Pending | | Filter by period |
| Display driver information | ⬜ Pending | | Driver name, phone |
| Create bus store (Pinia) | ⬜ Pending | | State management |

### Deliverables Checklist

- [ ] Bus CRUD operations fully functional
- [ ] Routes can be created and assigned to buses
- [ ] Buses grouped by period in UI
- [ ] Bus detail page shows all information
- [ ] Driver contact information displayed
- [ ] Form validation working

---

## Sprint 2.2: Bus Operations & Reporting (Week 5)

### Backend Tasks

| Task | Status | Assignee | Notes |
|------|--------|----------|-------|
| Implement bus statistics endpoints | ⬜ Pending | | Student counts, capacity |
| Create bus capacity validation | ⬜ Pending | | Prevent overloading |
| Add bus utilization reporting | ⬜ Pending | | Usage percentages |
| Implement search and filtering | ⬜ Pending | | By period, route, district |
| Create bus summary endpoint | ⬜ Pending | | Dashboard data |
| Add route optimization hints | ⬜ Pending | | District clustering |

### Frontend Tasks

| Task | Status | Assignee | Notes |
|------|--------|----------|-------|
| Create bus statistics dashboard | ⬜ Pending | | Charts and metrics |
| Build route visualization (map integration) | ⬜ Pending | | Leaflet.js map |
| Add capacity indicators | ⬜ Pending | | Progress bars, warnings |
| Implement search and filters | ⬜ Pending | | Search bar, dropdowns |
| Create bus comparison view | ⬜ Pending | | Side-by-side stats |
| Build district coverage display | ⬜ Pending | | Which buses serve which districts |

### Deliverables Checklist

- [ ] Bus statistics displayed on dashboard
- [ ] Route map visualization working
- [ ] Capacity indicators show usage
- [ ] Search and filter functional
- [ ] District coverage visible
- [ ] Reports exportable (optional)

---

## Technical Specifications

### Database Schema (Phase 2)

```sql
-- Routes Table
CREATE TABLE Routes (
    RouteId UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    RouteName NVARCHAR(200) NOT NULL,
    RouteDescription NVARCHAR(MAX),
    IsActive BIT NOT NULL DEFAULT 1,
    CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE()
);

-- Buses Table
CREATE TABLE Buses (
    BusId UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    BusNumber NVARCHAR(20) NOT NULL,
    PeriodId UNIQUEIDENTIFIER NOT NULL,
    RouteId UNIQUEIDENTIFIER,
    
    -- Driver Information
    DriverName NVARCHAR(200),
    DriverPhoneNumber NVARCHAR(20),
    
    -- Capacity
    Capacity INT NOT NULL DEFAULT 30,
    
    -- Status
    IsActive BIT NOT NULL DEFAULT 1,
    IsMerged BIT NOT NULL DEFAULT 0,
    MergedWithBusId UNIQUEIDENTIFIER,
    
    -- Audit
    CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    UpdatedAt DATETIME2,
    
    CONSTRAINT FK_Buses_Periods FOREIGN KEY (PeriodId) REFERENCES Periods(PeriodId),
    CONSTRAINT FK_Buses_Routes FOREIGN KEY (RouteId) REFERENCES Routes(RouteId),
    
    INDEX IX_Buses_Period (PeriodId),
    INDEX IX_Buses_BusNumber (BusNumber)
);

-- Bus-District Junction Table
CREATE TABLE BusDistricts (
    BusDistrictId UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    BusId UNIQUEIDENTIFIER NOT NULL,
    DistrictId UNIQUEIDENTIFIER NOT NULL,
    
    CONSTRAINT FK_BusDistricts_Bus FOREIGN KEY (BusId) REFERENCES Buses(BusId) ON DELETE CASCADE,
    CONSTRAINT FK_BusDistricts_District FOREIGN KEY (DistrictId) REFERENCES Districts(DistrictId),
    CONSTRAINT UQ_BusDistricts UNIQUE (BusId, DistrictId)
);
```

### Entity Models

```csharp
// Bus Entity
public class Bus : BaseEntity, IAuditable
{
    public string BusNumber { get; set; }
    public Guid PeriodId { get; set; }
    public Period Period { get; set; }
    public Guid? RouteId { get; set; }
    public Route Route { get; set; }
    
    // Driver
    public string DriverName { get; set; }
    public string DriverPhoneNumber { get; set; }
    
    // Capacity
    public int Capacity { get; set; } = 30;
    
    // Status
    public bool IsActive { get; set; } = true;
    public bool IsMerged { get; set; } = false;
    public Guid? MergedWithBusId { get; set; }
    
    // Navigation
    public ICollection<BusDistrict> Districts { get; set; }
    public ICollection<StudentBusAssignment> StudentAssignments { get; set; }
    
    // Audit
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

// Route Entity
public class Route : BaseEntity
{
    public string RouteName { get; set; }
    public string RouteDescription { get; set; }
    public bool IsActive { get; set; } = true;
    
    public ICollection<Bus> Buses { get; set; }
}
```

### DTOs

```csharp
// Bus DTOs
public class BusDto
{
    public Guid BusId { get; set; }
    public string BusNumber { get; set; }
    public string PeriodName { get; set; }
    public string RouteName { get; set; }
    public string DriverName { get; set; }
    public string DriverPhoneNumber { get; set; }
    public int Capacity { get; set; }
    public int CurrentStudentCount { get; set; }
    public decimal UtilizationPercentage { get; set; }
    public bool IsActive { get; set; }
    public List<string> Districts { get; set; }
}

public class CreateBusDto
{
    public string BusNumber { get; set; }
    public Guid PeriodId { get; set; }
    public Guid? RouteId { get; set; }
    public string DriverName { get; set; }
    public string DriverPhoneNumber { get; set; }
    public int Capacity { get; set; }
    public List<Guid> DistrictIds { get; set; }
}

public class BusStatisticsDto
{
    public Guid BusId { get; set; }
    public string BusNumber { get; set; }
    public int TotalStudents { get; set; }
    public int ActiveStudents { get; set; }
    public int SuspendedStudents { get; set; }
    public int Capacity { get; set; }
    public decimal UtilizationRate { get; set; }
    public List<DistrictStudentCount> DistrictBreakdown { get; set; }
}
```

---

## API Endpoints (Phase 2)

### Buses

| Method | Endpoint | Description | Auth |
|--------|----------|-------------|------|
| GET | `/api/buses` | List all buses | Yes |
| GET | `/api/buses/{id}` | Get bus by ID | Yes |
| GET | `/api/buses/by-period/{periodId}` | Get buses by period | Yes |
| POST | `/api/buses` | Create bus | Admin/Staff |
| PUT | `/api/buses/{id}` | Update bus | Admin/Staff |
| DELETE | `/api/buses/{id}` | Delete bus | Admin |
| GET | `/api/buses/{id}/students` | Get students on bus | Yes |
| GET | `/api/buses/{id}/statistics` | Get bus statistics | Yes |
| POST | `/api/buses/{id}/districts` | Assign districts | Admin/Staff |

### Routes

| Method | Endpoint | Description | Auth |
|--------|----------|-------------|------|
| GET | `/api/routes` | List all routes | Yes |
| GET | `/api/routes/{id}` | Get route by ID | Yes |
| POST | `/api/routes` | Create route | Admin |
| PUT | `/api/routes/{id}` | Update route | Admin |
| DELETE | `/api/routes/{id}` | Delete route | Admin |

### Statistics

| Method | Endpoint | Description | Auth |
|--------|----------|-------------|------|
| GET | `/api/buses/statistics/summary` | Overall bus stats | Yes |
| GET | `/api/buses/statistics/by-period` | Stats grouped by period | Yes |
| GET | `/api/buses/statistics/utilization` | Capacity utilization | Yes |

---

## Frontend Components

### Bus List View

```vue
<!-- BusListView.vue Structure -->
<template>
  <div class="bus-list-view">
    <!-- Period Tabs -->
    <div class="tabs">
      <button v-for="period in periods" 
              :class="{ active: selectedPeriod === period.id }"
              @click="selectPeriod(period.id)">
        {{ period.nameAr }}
      </button>
    </div>
    
    <!-- Search & Filters -->
    <SearchBar v-model="searchQuery" />
    
    <!-- Bus Grid -->
    <div class="bus-grid">
      <BusCard v-for="bus in filteredBuses" 
               :key="bus.busId"
               :bus="bus"
               @click="viewBus(bus.busId)" />
    </div>
  </div>
</template>
```

### Bus Card Component

```vue
<!-- BusCard.vue -->
<template>
  <div class="card bg-base-100 shadow-xl">
    <div class="card-body">
      <h2 class="card-title">
        <span class="bus-number">{{ bus.busNumber }}</span>
        <span class="badge" :class="statusBadge">{{ bus.isActive ? 'Active' : 'Inactive' }}</span>
      </h2>
      
      <p class="route-name">{{ bus.routeName }}</p>
      
      <!-- Capacity Progress -->
      <div class="capacity-indicator">
        <progress class="progress" 
                  :value="bus.currentStudentCount" 
                  :max="bus.capacity"
                  :class="capacityClass"></progress>
        <span>{{ bus.currentStudentCount }}/{{ bus.capacity }}</span>
      </div>
      
      <!-- Driver Info -->
      <div class="driver-info">
        <span>{{ bus.driverName }}</span>
        <a :href="'tel:' + bus.driverPhoneNumber">{{ bus.driverPhoneNumber }}</a>
      </div>
      
      <!-- Districts -->
      <div class="districts">
        <span v-for="district in bus.districts" class="badge badge-outline">
          {{ district }}
        </span>
      </div>
    </div>
  </div>
</template>
```

---

## Acceptance Criteria

### Sprint 2.1
- [ ] Buses can be created with all required fields
- [ ] Buses display grouped by period
- [ ] Bus details show driver information
- [ ] Routes can be assigned to buses
- [ ] Districts can be linked to buses
- [ ] Form validation prevents invalid data

### Sprint 2.2
- [ ] Bus statistics calculate correctly
- [ ] Capacity indicators show color-coded status
- [ ] Route map displays (Leaflet integration)
- [ ] Search filters work across bus properties
- [ ] Utilization report generates accurately
- [ ] Dashboard shows summary statistics

---

## Risks & Mitigations

| Risk | Impact | Mitigation |
|------|--------|------------|
| Map integration complexity | Medium | Start with simple markers, enhance later |
| Performance with many buses | Low | Implement pagination and lazy loading |
| Capacity calculation accuracy | Medium | Unit tests for calculation logic |
| Mobile responsiveness | Medium | Test on multiple devices early |

---

## Achievement Tracking

### Week 4 Progress

| Category | Planned | Completed | Percentage |
|----------|---------|-----------|------------|
| Backend Tasks | 7 | 0 | 0% |
| Frontend Tasks | 7 | 0 | 0% |
| **Total** | **14** | **0** | **0%** |

### Week 5 Progress

| Category | Planned | Completed | Percentage |
|----------|---------|-----------|------------|
| Backend Tasks | 6 | 0 | 0% |
| Frontend Tasks | 6 | 0 | 0% |
| **Total** | **12** | **0** | **0%** |

---

## Phase Summary

### Overall Progress

```
Phase 2 Completion: ░░░░░░░░░░ 0%
```

| Sprint | Status | Completion |
|--------|--------|------------|
| Sprint 2.1 - Bus Core | 🔴 Not Started | 0% |
| Sprint 2.2 - Bus Operations | 🔴 Not Started | 0% |

### Key Milestones

- [ ] 🎯 Bus CRUD operations complete
- [ ] 🎯 Period-based grouping functional
- [ ] 🎯 Bus statistics dashboard live
- [ ] 🎯 Route visualization working

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
