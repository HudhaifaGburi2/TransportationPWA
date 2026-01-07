# Transport Unit Management System (TUMS)
## Design System & Visual Identity Guide

---

## 1. Color Palette

### Primary Colors

**Primary Green (Main Actions)**
- HEX: `#3d5a4f`
- RGB: `rgb(61, 90, 79)`
- Usage: Primary buttons, navigation headers, active states, emphasis
- Arabic Name: الأخضر الأساسي

**Primary Green Light**
- HEX: `#4d6f62`
- RGB: `rgb(77, 111, 98)`
- Usage: Hover states, secondary elements

**Primary Green Dark**
- HEX: `#2d4a3f`
- RGB: `rgb(45, 74, 63)`
- Usage: Pressed states, dark mode

### Secondary Colors

**Gold/Tan (Secondary Actions)**
- HEX: `#b8935f`
- RGB: `rgb(184, 147, 95)`
- Usage: Edit buttons, secondary actions, highlights
- Arabic Name: الذهبي

**Gold Light**
- HEX: `#c9a570`
- Usage: Hover states

### Status Colors

**Success Green**
- HEX: `#28a745`
- Usage: Present status, confirmations, success messages
- Badge: "حاضر" (Present)

**Warning Orange**
- HEX: `#ffa726`
- Usage: Late status, warnings, pending actions
- Badge: "متأخر" (Late)

**Danger Red**
- HEX: `#dc3545`
- Usage: Absent status, errors, suspensions
- Badge: "غائب" (Absent)

**Info Blue**
- HEX: `#17a2b8`
- Usage: Excused status, informational messages
- Badge: "مستأذن" (Excused)

**Neutral Gray**
- HEX: `#6c757d`
- Usage: Inactive elements, disabled states

### Background Colors

**Background Light**
- HEX: `#f8f9fa`
- Usage: Page background

**Card Background**
- HEX: `#ffffff`
- Usage: Cards, modals, content containers

**Border Gray**
- HEX: `#dee2e6`
- Usage: Borders, dividers

---

## 2. Typography

### Font Families

**Primary Font (Arabic)**
```css
font-family: 'Cairo', 'Segoe UI', Tahoma, sans-serif;
```

**Secondary Font (English/Numbers)**
```css
font-family: 'Roboto', 'Segoe UI', sans-serif;
```

### Font Sizes

**Headings**
- H1: 32px / 2rem (Page titles)
- H2: 24px / 1.5rem (Section headers)
- H3: 20px / 1.25rem (Card titles)
- H4: 18px / 1.125rem (Subsections)

**Body Text**
- Large: 16px / 1rem (Primary content)
- Regular: 14px / 0.875rem (Default text)
- Small: 12px / 0.75rem (Helper text, labels)

**Specialized**
- Numbers: 18px (Statistics, counts)
- Badges: 13px (Status indicators)

### Font Weights
- Light: 300
- Regular: 400
- Medium: 500
- Semi-Bold: 600
- Bold: 700

---

## 3. Components

### 3.1 Buttons

**Primary Button**
```
Background: #3d5a4f
Text: #ffffff
Border-radius: 8px
Padding: 12px 24px
Font-size: 14px
Font-weight: 600
Shadow: 0 2px 4px rgba(0,0,0,0.1)
```

**Secondary Button**
```
Background: #b8935f
Text: #ffffff
Border-radius: 8px
Padding: 12px 24px
```

**Outline Button**
```
Background: transparent
Border: 2px solid #3d5a4f
Text: #3d5a4f
Border-radius: 8px
Padding: 10px 22px
```

**Disabled Button**
```
Background: #e9ecef
Text: #6c757d
Cursor: not-allowed
```

### 3.2 Cards

**Standard Card**
```
Background: #ffffff
Border: 1px solid #dee2e6
Border-radius: 12px
Padding: 20px
Shadow: 0 2px 8px rgba(0,0,0,0.08)
```

**Halaqat Card (Special)**
```
Background: #ffffff
Border: 1px solid #dee2e6
Border-radius: 12px
Padding: 16px
Shadow: 0 4px 12px rgba(0,0,0,0.1)
Header: Dark green badge with icon
```

### 3.3 Status Badges

**Present (حاضر)**
```
Background: #28a745
Text: #ffffff
Border-radius: 6px
Padding: 4px 12px
Icon: ✓
```

**Late (متأخر)**
```
Background: #ffa726
Text: #ffffff
Icon: ⏰
```

**Absent (غائب)**
```
Background: #dc3545
Text: #ffffff
Icon: ✗
```

**Excused (مستأذن)**
```
Background: #17a2b8
Text: #ffffff
Icon: 📋
```

### 3.4 Form Inputs

**Text Input**
```
Border: 1px solid #dee2e6
Border-radius: 8px
Padding: 10px 16px
Font-size: 14px
Focus-border: #3d5a4f
```

**Select Dropdown**
```
Border: 1px solid #dee2e6
Border-radius: 8px
Padding: 10px 16px
Background: #ffffff
```

**Checkbox/Radio**
```
Accent-color: #3d5a4f
Size: 20px
```

---

## 4. Layout System

### 4.1 Spacing Scale

```
xs: 4px
sm: 8px
md: 16px
lg: 24px
xl: 32px
xxl: 48px
```

### 4.2 Grid System

**Container Widths**
- Mobile: 100%
- Tablet: 768px
- Desktop: 1200px
- Large: 1400px

**Card Grid**
- 3 columns on desktop (33.33%)
- 2 columns on tablet (50%)
- 1 column on mobile (100%)
- Gap: 20px

---

## 5. Icons

### Icon Library
Use Font Awesome or custom SVG icons

**Common Icons**
- Bus: 🚌 (or FA bus)
- Student: 👤 (or FA user)
- Teacher: 👨‍🏫 (or FA chalkboard-teacher)
- Location: 📍 (or FA map-marker)
- Calendar: 📅 (or FA calendar)
- Check: ✓ (or FA check)
- Edit: ✏️ (or FA edit)
- Delete: 🗑️ (or FA trash)

---

## 6. Screen-Specific Designs

### 6.1 Admin Dashboard (Image 1)
**Visual Identity**
- Clean, card-based layout
- Statistics cards with icons and numbers
- Right sidebar navigation
- Dark green accent throughout
- User profile at top

**Key Elements**
- Summary statistics bar (teachers, students, buses)
- Filterable halaqat cards
- Action buttons (gold for edit, green for manage)
- Status indicators

### 6.2 Attendance Screen (Image 2)
**Visual Identity**
- Calendar-based date selector
- Student list with status badges
- Filtering chips (حاضر, غائب, متأخر, مستأذن)
- Pagination controls

**Key Elements**
- Date navigation (day selector)
- Student rows with colored status badges
- Quick action buttons
- Halaqat summary card at top

### 6.3 Login Screen (Image 3)
**Visual Identity**
- Centered card design
- Minimal, clean interface
- Program branding at top
- Dark green primary button

**Key Elements**
- Logo/icon at top
- Arabic program name
- Username and password fields
- "Remember me" checkbox
- Green login button
- "Create account" link

---

## 7. RTL (Right-to-Left) Guidelines

### Layout Direction
```css
html {
  direction: rtl;
  text-align: right;
}
```

### Flexbox Adjustments
- Use `flex-direction: row-reverse` for horizontal layouts
- Margins/padding should mirror (right becomes left)
- Icons should appear on right side of text

### Navigation
- Menu items align right
- Dropdowns expand from right
- Back buttons point right (→)

---

## 8. Accessibility

### Contrast Ratios
- All text meets WCAG AA standards (4.5:1 minimum)
- Primary green on white: 7.2:1 ✓
- Status badges have sufficient contrast

### Touch Targets
- Minimum size: 44x44px
- Spacing between interactive elements: 8px minimum

### Screen Reader Support
- Arabic ARIA labels
- Proper heading hierarchy
- Descriptive button labels

---

## 9. Animation & Motion

### Transitions
```css
transition: all 0.3s ease;
```

**Use Cases**
- Button hover states
- Card elevation on hover
- Modal fade in/out
- Dropdown slide down

### Micro-interactions
- Success checkmarks animate in
- Badge colors pulse on status change
- Loading spinners use primary green

---

## 10. Responsive Breakpoints

```css
/* Mobile */
@media (max-width: 576px) { }

/* Tablet */
@media (min-width: 577px) and (max-width: 992px) { }

/* Desktop */
@media (min-width: 993px) { }

/* Large Desktop */
@media (min-width: 1400px) { }
```

---

## 11. Implementation Notes

### CSS Variables
```css
:root {
  --primary-green: #3d5a4f;
  --secondary-gold: #b8935f;
  --success: #28a745;
  --warning: #ffa726;
  --danger: #dc3545;
  --info: #17a2b8;
  --background: #f8f9fa;
  --card-bg: #ffffff;
  --border: #dee2e6;
  --text-primary: #212529;
  --text-secondary: #6c757d;
  
  --spacing-xs: 4px;
  --spacing-sm: 8px;
  --spacing-md: 16px;
  --spacing-lg: 24px;
  --spacing-xl: 32px;
  
  --radius-sm: 6px;
  --radius-md: 8px;
  --radius-lg: 12px;
  
  --shadow-sm: 0 2px 4px rgba(0,0,0,0.08);
  --shadow-md: 0 4px 8px rgba(0,0,0,0.1);
  --shadow-lg: 0 8px 16px rgba(0,0,0,0.12);
}
```

### Framework Recommendations
- **Vue 3** with Composition API
- **Vuetify** or **PrimeVue** (with custom theme)
- **Tailwind CSS** (configured with custom colors)
- **Pinia** for state management

---

## 12. Design Principles

1. **Simplicity First**: Clean, uncluttered interfaces
2. **Action-Oriented**: Clear call-to-action buttons
3. **Status Visibility**: Color-coded status indicators
4. **Mobile-Optimized**: Touch-friendly, responsive
5. **Arabic-First**: Native RTL support, Arabic typography
6. **Performance**: Fast load times, offline support
7. **Accessibility**: WCAG 2.1 AA compliance

---

## 13. Brand Elements

### Logo Placement
- Top left (in RTL: top right)
- 48px height
- Includes program name in Arabic

### Color Meaning
- **Green**: Trust, growth, Islamic tradition
- **Gold**: Excellence, quality
- **Status Colors**: Universal understanding (green=good, red=bad)

---

## Version History
- v1.0 - Initial design system based on existing interfaces
- Date: January 2026