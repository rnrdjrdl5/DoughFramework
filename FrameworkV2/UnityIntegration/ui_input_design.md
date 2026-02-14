# UI & Input Design Summary (Final)

## 0. Overall Core Summary

UI is the physical layer responsible for presentation and input. Input is classified and propagated
in the physical layer, and only meaningful state changes are processed in the logical layer (Realm).

------------------------------------------------------------------------

## 1. Basic Definition of UI

- UI is the layer that renders data
- It does not judge rules or own state
- It only opens/closes/shows
- Interpretation of meaning caused by UI is the Realm's responsibility

------------------------------------------------------------------------

## 2. UI Classification

### 2.1 Core UI (Core / Persistent UI)

- Always present on screen
- Cannot be closed
- Directly communicates with the game world (e.g., HUD, player status, skill slots, crosshair)
- Meaning: a visual representation of Presence

### 2.2 Panel UI (Panel / Interaction UI, Non-modal)

- Can be opened and closed
- Subject to Esc (per policy)
- Can acquire input focus, but does not fully block lower-layer input
- Temporary context (e.g., panels, inventory, side menu, tooltip/toast per policy)

### 2.3 Modal UI (Modal)

- Can be opened and closed
- Always blocks lower (UI/Core/World) input and traps focus inside
- Sorted at the top layer (above Panels)
- Examples: settings window, confirm/warning dialog, menus that require a blocker

### 2.4 System UI (System / Global UI)

- Always top-most
- Must not be covered by other UI
- Represents global game state (e.g., loading screen, system errors, network warnings)
- Esc availability follows each UI's policy

------------------------------------------------------------------------

## 3. Realm-Based UI Separation

- System UI: global, not separated by Realm
- Core UI / Panel UI / Modal UI: bound to Realm context

------------------------------------------------------------------------

## 4. Core Principles of UI Input Handling

- Input handling is the physical layer's responsibility
- Includes keyboard/mouse/touch/pad
- Includes focus, Z-order, Esc handling
- Logical layer receives only input results (Events)

------------------------------------------------------------------------

## 5. Esc Handling Rules

- Esc means exiting the active window (modal first if present; otherwise top-most panel)
- Evaluate from the top and handle only UIs that intend to consume Esc
- Once handled, it is not propagated downward (consumed)

------------------------------------------------------------------------

## 6. Input Priority Within a Realm

1. Modal UI
2. Panel UI (non-modal)
3. Core UI
4. Presence / Player Ability
5. World Interaction

- Character movement input is always the lowest priority
- If any UI is open, movement input is consumed

------------------------------------------------------------------------

## 7. Input Classification

### 7.1 Broadcast Input

- Delivered to multiple Realms simultaneously
- Only subscribed Realms receive it
- No cross-Realm consumption (e.g., Pause, global actions, simultaneous movement)

### 7.2 Focus Input

- Delivered only to the currently active Realm
- Cross-Realm consumption applies (e.g., click, UI actions, Esc)

------------------------------------------------------------------------

## 8. Inter-Realm vs Intra-Realm Handling

- Inter-Realm: determine delivery targets via Broadcast / Focus
- Intra-Realm: always apply consumption rules
- One Input results in exactly one meaningful handling

------------------------------------------------------------------------

## 9. UI Open/Close and the Logical Layer

- UI open/close: physical layer
- Determine whether state change is needed: logical layer
- What is delivered is an Event (fact), not a Command

------------------------------------------------------------------------

## 10. Multi-Context UI

- UI root is based on Realm (context), not screen
- System UI is global
- Input branches as Routed / Broadcast (if a modal exists, do not propagate to lower layers)

------------------------------------------------------------------------

## 11. Final Summary Sentence

UI handles presentation and input, Input is classified/propagated in the physical layer,
and Realm interprets whether UI results are meaningful state changes.
