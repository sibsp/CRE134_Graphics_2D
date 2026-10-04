# MyFirstScene — Student Overview

**Scene:** `Assets/1. Starter Examples/MyFirstScene.unity`  
**Player prefab:** `Assets/1. Starter Examples/Prefabs/Player.prefab`  
**Script:** `Assets/1. Starter Examples/Scripts/PlayerMovementSimple.cs`

This is your first **playable** 2D scene: move and jump on **Ground**, while **Champagne_Animation_0** shows a separate sprite animation example in the same level.

---

## Play mode — controls

| Input | Action |
|--------|--------|
| **A / D** or **← / →** | Move left / right |
| **Space** (hold) | Full jump |
| **Space** (tap) | Shorter jump |
| **W / S / ↑ / ↓** | Also bound to **Move** (horizontal part used) |

Focus the **Game** view before testing. You get **double jump** (`maxJumps = 2`) when grounded checks succeed.

---

## Player — hierarchy & components

Select **Player** in the Hierarchy (instance of the prefab above).

| Part | What it does |
|------|----------------|
| **Player** (root) | Physics body, input, and movement logic |
| **Ground check** (child) | Empty transform at the feet; used by `PlayerMovementSimple` for “on ground?” |
| **Wall check** (child) | Disabled in this prefab — used in later, advanced movement scenes |

**Components on Player:**

| Component | Role |
|-----------|------|
| **Sprite Renderer** | Draws the player (green square in the prefab) |
| **Rigidbody 2D** | Gravity and velocity; rotation frozen |
| **Box Collider 2D** | Solid collision with **Ground** |
| **Player Input** | Reads **Input System** actions and calls methods on your script |
| **Player Movement Simple** | Your C# — walk, jump, ground check, gravity, flip |

---

## Player Movement Simple — what the script does

Runs in **Update** each frame:

1. **GroundCheck** — `Physics2D.OverlapBox` at **Ground check** against **Ground Layer** (Layer **6**). Resets jump count when grounded.
2. **ProcessGravity** — stronger fall when moving downward; caps max fall speed.
3. **Movement** — sets horizontal velocity from input × `moveSpeed`.
4. **Flip** — flips scale on X when changing direction.

**Input callbacks** (wired by **Player Input**, not called from `Update`):

- `OnMove` — reads horizontal axis from the **Move** action.
- `OnJump` — applies jump force on **performed** / short hop on **canceled**.

**Inspector fields to know:** `rb`, `groundCheckPos`, `groundLayer`, `moveSpeed`, `jumpForce`, `maxJumps`, gravity settings. In Play mode, select Player and enable **Gizmos** to see the blue **ground check** box.

**Input asset:** `Assets/Input Actions/InputSystem_Actions.inputactions` (assigned on **Player Input** → **Actions**).

---

## Other scene objects

| Object | Purpose |
|--------|---------|
| **Ground** | Scaled sprite + **Box Collider 2D** on **Layer 6** — must match **Player → Ground Layer** mask |
| **Champagne_Animation_0** | **Animator** + clip `Champagne_start` — looping animation only (no player script) |
| **Main Camera** | Orthographic 2D + URP |
| **Global Light 2D** | Standard URP 2D lighting |

---

## Quick checks if something breaks

- **Falls through floor** — **Ground** must be Layer **6**; **Player Movement Simple → Ground Layer** must include that layer.
- **No movement** — **Player Input** behaviour **Invoke Unity Events**; **Move** / **Jump** must call `OnMove` / `OnJump` on **Player Movement Simple**.
- **No jump** — **Ground check** position/size; watch the blue gizmo vs the platform.
- **Input does nothing** — Project uses **Input System** (not legacy Input Manager).

---

## Progression in this folder

| Stage | Script / scene |
|--------|----------------|
| **This scene** | `PlayerMovementSimple.cs` |
| More features (walls, animation) | `PlayerMovementIntermediate.cs`, `PlayerMovement.cs`, Scene 2 variants |
| Level building | `Scene 3 - Tilemap.unity` |

Save a copy of the scene before big experiments (**File → Save As**).

*CRE134 Graphics 2D*
