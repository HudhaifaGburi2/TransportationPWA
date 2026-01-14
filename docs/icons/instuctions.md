Below is a **clean, explicit, and enforceable instruction set** written **for an implementation agent (Vue / Frontend Engineer)**.
This is suitable to be used **as-is** in an agent task, backlog item, or system prompt.

---

# Task: Standardize Registration Button Design (TUMS)

## Objective

Implement a **standard, reusable registration button** across the TUMS frontend that **fully complies** with the TUMS Design System & Visual Identity Guide and **Vue 3 best practices**.

This standard must eliminate inconsistent button designs, layout issues (split icon/text), and RTL misalignment.

---

## Scope

* Vue 3
* Composition API
* RTL-first (Arabic)
* Design-system driven
* Reusable UI component

---

## Mandatory Requirements

### 1. Component Architecture

* Create a reusable base button component:

  ```
  components/ui/BaseButton.vue
  ```

* All primary actions (including **تسجيل جديد**) must use this component.
* Direct usage of raw `<button>` or `<a class="btn">` for primary actions is **not allowed**.

---

### 2. Visual Design Compliance (Must Match Exactly)

#### Primary Button Styling

| Property      | Value                       |
| ------------- | --------------------------- |
| Background    | `#3d5a4f` (Primary Green)   |
| Text Color    | `#ffffff`                   |
| Border Radius | `8px`                       |
| Padding       | `12px 24px`                 |
| Font Size     | `14px`                      |
| Font Weight   | `600`                       |
| Font Family   | Cairo                       |
| Shadow        | `0 2px 4px rgba(0,0,0,0.1)` |
| Min Height    | `44px`                      |

#### States

* **Hover**:

  * Background: `#4d6f62`
  * Shadow: medium
  * Slight lift (translateY -1px)
* **Active**:

  * Background: `#2d4a3f`
  * Scale: `0.98`
* **Disabled**:

  * Background: `#e9ecef`
  * Text: `#6c757d`
  * Cursor: `not-allowed`

---

### 3. Layout & Alignment Rules

* Button must use `inline-flex`
* Icon and text must never split into separate visual blocks
* Spacing between icon and text: `12px`
* Text must never wrap (use `white-space: nowrap`)
* Entire button must be clickable

---

### 4. RTL Rules (Critical)

* RTL is the default layout direction
* Icon must appear **to the right of the text**
* Use `flex-direction: row-reverse` automatically when `dir="rtl"`
* No hardcoded left/right margins

---

### 5. Accessibility (Non-Negotiable)

* Minimum clickable area: `44x44px`
* Proper `role="button"`
* Support `disabled` and `aria-disabled`
* Button label must be descriptive (no icon-only CTAs for registration)

---

### 6. Vue Implementation Rules

* Use Composition API (`<script setup>`)
* Support these props:

  * `variant` (primary, secondary, outline)
  * `to` (Vue Router)
  * `href` (external links)
  * `disabled`
* Automatically render:

  * `<RouterLink>` when `to` is provided
  * `<a>` when `href` is provided
  * `<button>` otherwise

---

### 7. Standard Registration Button Usage

**Required usage pattern:**

```vue
<BaseButton to="/registration">
  <template #icon>
    <ClipboardListIcon />
  </template>
  تسجيل جديد
</BaseButton>
```

* No inline styling
* No duplicated CSS
* No custom overrides per screen

---

### 8. CSS & Tokens

* Use CSS variables defined in the TUMS design system:

  * `--primary-green`
  * `--secondary-gold`
  * `--shadow-sm`, `--shadow-md`
* Do not hardcode colors outside the design tokens

---

### 9. Code Review Acceptance Criteria

The task is considered **complete** only if:

* Registration button appears identical across all screens
* No icon/text split occurs in any viewport
* RTL layout is correct
* Hover/active states match the design system
* All registration actions use the shared component
* No visual regressions on mobile

---

### 10. Explicit Prohibitions

* ❌ No Bootstrap `.btn` usage for primary CTAs
* ❌ No Tailwind utility buttons for registration
* ❌ No duplicated button CSS
* ❌ No per-page overrides

---

## Outcome

A single, authoritative **Registration Button standard** that:

* Reflects TUMS brand identity
* Is easy to maintain
* Is consistent, accessible, and RTL-safe
* Aligns with Vue 3 best practices

---
