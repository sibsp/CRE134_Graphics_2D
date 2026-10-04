# Scene 1 — Simple move and jump — Student Overview

**Scene:** `Scene 1 - Simple move and jump.unity`  
**Player:** Prefab `Prefabs/Player.prefab` (instance in scene)  
**Script:** `Scripts/PlayerMovementSimple.cs`  
**Also on camera:** `Assets/2. Platformer Example/Code/UtilityScripts/CameraFollow2D.cs`

Step up from **MyFirstScene**: same basic move/jump, but a **larger level** (platforms) and a **camera that follows the player**.

---

## Controls

| Input | Action |
|--------|--------|
| **A / D** or **← / →** | Move |
| **Space** (hold / tap) | Full / short jump |
| **Double jump** | Yes, when grounded check passes |

Input: **Player Input** → `Assets/Input Actions/InputSystem_Actions.inputactions` → **Move** / **Jump** call `OnMove` / `OnJump`.

---

## Player — components & hierarchy

| On **Player** | Role |
|---------------|------|
| **Sprite Renderer** | Green square (prefab default) |
| **Rigidbody 2D** | Physics; rotation frozen |
| **Box Collider 2D** | Collides with platforms |
| **Player Input** | Input System events |
| **Player Movement Simple** | Walk, jump, gravity, ground check, flip |

| Child | Role |
|--------|------|
| **Ground check** | Feet overlap test for jumps |
| **Wall check** | In prefab but **unused** in this script (used in Scene 2) |

**Layers:** **Ground Layer** = Layer **6** (`Ground`, platforms under **Environment**).

---

## Player Movement Simple (this scene)

Same as MyFirstScene: `GroundCheck` → `ProcessGravity` → horizontal move → `Flip`; `OnMove` / `OnJump` from input. **No** wall slide or wall jump. **No** Animator.

Open the script and compare to **`PlayerMovementIntermediate.cs`** before Scene 2.

---

## Main Camera — Camera Follow 2D

| Field | Typical use |
|--------|----------------|
| **Player** | Drag the **Player** transform |
| **Smooth Speed** | How smoothly the camera catches up (~0.125) |
| **Offset** | Usually `(0, 0, -10)` for 2D |

Runs in **LateUpdate** so it follows after the player moves.

---

## Level layout

- **Environment** — **Ground**, **Platform 1–3**, extra ground; all on **Layer 6** with **Box Collider 2D**.
- **Global Light 2D** — URP 2D lighting.

**Try:** reach the higher platforms with move + double jump; watch the camera track you.

---

## Troubleshooting

- **Falls through floor** — Ground Layer mask vs object Layer **6**.
- **Camera stuck** — **Camera Follow 2D → Player** assigned?
- **No input** — **Player Input** behaviour = **Invoke Unity Events**; events wired to **Player Movement Simple**.

**Next:** `Scene 2 - Player Movement_Jump_Gravity_No_Animation.unity` — walls + **PlayerMovementIntermediate**.

*CRE134 Graphics 2D*
