# Scene 2 (With Animation) — Student Overview

**Scene:** `Scene 2 - Player Movement_Jump_Gravity_With_Animation.unity`  
**Script:** `Scripts/PlayerMovement.cs`  
**Animator:** `Animations/Player.controller` (+ clips: Idle, Walk, Jump, Fall, WallSlide)  
**Camera:** `CameraFollow2D` on **Main Camera** (`Assets/2. Platformer Example/Code/UtilityScripts/CameraFollow2D.cs`)

**Full platformer:** move, jump, gravity, **wall slide**, **wall jump**, and **sprite animation** driven by physics.

---

## Controls

Same as Scene 2 (No Animation): **A/D**, **Space**, wall slide/jump on **Layer 7** walls. Camera **follows** the player again.

---

## Player — components & hierarchy

| On **Player** | Role |
|---------------|------|
| **Sprite Renderer** | Stickman (Animator swaps sprites) |
| **Rigidbody 2D** / **Box Collider 2D** | Physics |
| **Player Input** | → `OnMove` / `OnJump` on **Player Movement** |
| **Player Movement** | Movement + wall logic + **Animator** updates |
| **Animator** | Controller **Player** |

| Child | Role |
|--------|------|
| **Ground check** | Layer **6** |
| **Wall check right** | Layer **7** |

Inspector links: **rb**, **animator**, **groundCheckPos**, **wallCheckPos**, layer masks **64** / **128**.

---

## Player Movement — logic + animation

Same core loop as **Intermediate** (`GroundCheck`, gravity, wall slide/jump, flip), plus in **Update**:

| Animator call | Meaning |
|---------------|---------|
| `SetFloat("xVelocity", …)` | Walk vs idle (absolute horizontal speed) |
| `SetFloat("yVelocity", …)` | Jump / fall blending |
| `SetBool("isWallSliding", …)` | Wall slide state |
| `SetTrigger("jump")` | Jump animation on jump / wall jump |

Open **`Animations/Player.controller`** in the Animator window to see transitions driven by those parameters.

**Compare scripts:** `PlayerMovementSimple` → `PlayerMovementIntermediate` → **`PlayerMovement`** (this scene).

---

## Camera

**Main Camera** → **Camera Follow 2D**: **Player** reference, **Smooth Speed**, **Offset** `(0, 0, -10)`. Matches Scene 1 behaviour while you cross the larger layout.

---

## Environment

Same as No Animation scene: **Ground**, platforms, **Wall Left/Right** under **Environment**. Use walls to trigger **WallSlide** animation and wall jumps.

---

## Suggested tasks

1. Play and watch **Animator** parameters change live (Window → Animation → Parameters while playing).  
2. Trigger **WallSlide** on a wall, then **jump** — see trigger + velocity change together.  
3. Diff **`PlayerMovement.cs`** vs **`PlayerMovementIntermediate.cs`** — only the **animator** lines and **`Start`** reference differ in purpose.

---

## Troubleshooting

- **T-pose / frozen sprite** — **Animator → Controller** = `Player.controller`; Console errors for missing parameters.  
- **Walk never plays** — **`xVelocity`** must be above 0 in controller transitions; move horizontally.  
- **Animation ok but no wall jump** — same layer/check fixes as No Animation overview.

**Next:** `Scene 3 - Tilemap.unity` — level building with tilemaps (often still uses **PlayerMovement** + this controller).

*CRE134 Graphics 2D*
