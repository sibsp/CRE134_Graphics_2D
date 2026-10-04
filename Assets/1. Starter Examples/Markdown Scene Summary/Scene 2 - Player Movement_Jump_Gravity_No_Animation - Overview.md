# Scene 2 (No Animation) — Student Overview

**Scene:** `Scene 2 - Player Movement_Jump_Gravity_No_Animation.unity`  
**Script:** `Scripts/PlayerMovementIntermediate.cs`  
**Player:** **Player** in Hierarchy (built in scene — stickman sprite, **no Animator**)

Same platform/wall layout as the animated Scene 2 variant, but movement is **code + sprite flip only** — focus on **physics and wall mechanics** before adding animation.

---

## Controls

| Input | Action |
|--------|--------|
| **A / D** | Move |
| **Space** | Jump (ground / air); **wall jump** when touching a wall |
| **Hold toward wall while falling** | **Wall slide** (slow fall) |

Still uses **Player Input** + `InputSystem_Actions` → `OnMove` / `OnJump` on **Player Movement Intermediate**.

---

## Player — components & hierarchy

| On **Player** | Role |
|---------------|------|
| **Sprite Renderer** | Stickman (static pose; no animation clips) |
| **Rigidbody 2D** / **Box Collider 2D** | Platformer physics |
| **Player Input** | Wired to **Player Movement Intermediate** |
| **Player Movement Intermediate** | Full movement logic for this scene |

| Child | Role |
|--------|------|
| **Ground check** | Layer **6** — platforms & ground |
| **Wall check right** | Layer **7** — **Wall Left** / **Wall Right** |

**Layer masks (Inspector):** **Ground Layer** = bit **64** (Layer 6); **Wall Layer** = bit **128** (Layer 7).

---

## Player Movement Intermediate — what’s new vs Scene 1

Everything from **Simple**, plus:

1. **ProcessWallSlide** — slide down walls when airborne, touching wall, and pressing into it.
2. **ProcessWallJump** — arms a short window to jump **away** from the wall (**Space**).
3. **WallCheck** — overlap box at **Wall check right** on **Wall Layer**.
4. **isWallJumping** — briefly **locks horizontal input** so the wall jump feels directed.
5. **Gizmos** — blue = ground check, pink = wall check (select Player in Scene view).

Still **no Animator** — facing uses **transform scale flip** like Scene 1.

---

## Environment (practice layout)

Under **Environment** / **Wall and Platform**:

- **Ground** + coloured **Platform 1–4** (Layer **6**).
- **Wall Left** / **Wall Right** (Layer **7**) — use these to test slide and wall jump.

**Main Camera** is **fixed** (no follow script in this scene) — good for studying movement in one view.

---

## Suggested tasks

1. Wall-slide down **Wall Left**, then **Space** to wall-jump to a platform.  
2. With Player selected, enable **Gizmos** and align check boxes with colliders.  
3. Read **`OnJump`** in the script — ground jump vs **wallJumpTimer** branch.

---

## Troubleshooting

- **Wall slide never triggers** — **Wall Layer** mask; press **into** the wall while falling.  
- **Wall jump does nothing** — must be sliding/touching wall; timer set in **ProcessWallJump**.  
- **Stuck can’t move horizontally** — landed while wall-jump flag stuck; script resets on ground — check **Ground check**.

**Next:** `Scene 2 - Player Movement_Jump_Gravity_With_Animation.unity` — same mechanics + **PlayerMovement** + **Animator**.

*CRE134 Graphics 2D*
