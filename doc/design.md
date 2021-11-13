# Game Design Document
## Summary
Minimum wage is a simple, 2D endless-runner in which the player must collect and return shopping carts from a parking lot while avoiding collisions with obstacles such as cars and pedestrians. Points are awarded to the player for successfully returning carts. Speed increases as the round progresses, making it increasingly difficult to avoid obstacles, but the player can increase his/her chances of success by returning carts of various color combinations, providing the player with various benefits / power-ups.

---

## Gameplay Overview
### Interface
- **The game world is presented in a 2D, top-down fashion.**
	- _Not_ isometric or angled such that fronts/sides of world objects are visible. This is mostly because this is a game about precise positioning, and an angled perspective introduces positional ambiguity.
- **The player controls an endlessly-running character who is pushing a shopping cart through a parking lot.**
	- The player character is always located on the far left side of the screen.
	- The player character is always facing toward the right side of the scren.
- **The world around the player scrolls from right to left.**
	- This simulates player movement from left the right through the world.
- **The player controls the character's vertical movement**
	- Moving up/down on the screen is effectively strafing left/right
	- Input mechanism: Keyboard buttons, joystick, or directional pad.
	- Horizontal position is locked.
	- Rotation is locked.
- **Various powerups can be enabled by pressing buttons on conjunction with joystick movement**

### Gameplay Loop
- **The player must avoid colliding with obstacles (such as parked cars)**
	- The round ends when the player hits an obstacle
	- The current implementation provides a crude time-based obstacle spawning mechanism
	- The envisioned implementation would procedurally generate the world and obstacles based on a set of tiles with spawn point metadata
- **The player can collect shopping carts in a stack**
	- Carts are collected by ramming them with the front cart in the player's stack.
		- Carts are always oriented correctly to be picked up (facing right).
	- The player's stack of carts becomes increasingly difficult to control as the number of stacked carts increases.
- **Points are awared when the player returns carts to a designated cart return corral**
	- The player receives a baseline number of points per cart returned
	- To encourage the player to build and return larger stacks of carts, a bonus multiplier is provided based on the size of the stack of carts being returned.
- **The scroll speed of the world (i.e. the player character's forward movement speed) slowly increases over time.**
	- Left unattended, the scroll speed becomes insurmountably fast, and the player will unavoidably collide with an obstacle.
- **To provide a chance at survival given the ever-increasing speed, the player is afforded several unique abilities that are enabled by returning carts of specific colors**
	- See [Cart Color Effects](#cart-color-effects) for more details

### Context
- The player character is a minimum wage employee with telekinetic powers, working at an ordinary  grocery store located in a suspiciously long strip mall.
- The player character's job is to collect all of the shopping carts that customers have carelessly left strewn about the parking lot, and return them to designated cart corrals.
- The player character must avoid colliding with obstacles, lest he/she be held liable for damages/injury.

---

## Mockup
![Mockup](mockup.png)
- Add visual indicators for powerups
    - Could be UI elements, _or_ could be more implicit:
      - Slowdown: Reduced movement speed should be obvious
      - Magnetism: Front cart glows red, nearby carts also glow
      - Nudge: Add green trailing lines behind carts when boosted up/down
- Audio Effects for each ability will also help in the actual game

---

## Gameplay Details
_Exact mechanics and (especially) numerical values will likely change significantly as prototyping continues, so concepts are kept fairly general and qualitative for now._
### Cart Color Effects
Returning carts of a given color to the cart return corral results in a temporary or consumable benefit for the player. The goal here is to build and release tension, and to reward players for skill (and luck) in collecting carts.

- **Blue: Slow the scroll speed**
	- Intention is to help counteract the endless scroll speed increase.
	- Should be designed and tuned such that a skilled player can effectively undo some but not all of the speed increase since the previous cart return.
		- Alternatively, allow significant/arbitrary slow-down, but track an _intended_ speed over time and adjust the speed increase rate so that it wants to converge on the target.
- **Red: Magnetic carts**
	- Intention is simply to make it easier to pick up carts as speed increases.
	- Magnetic effect should require line-of-sight between the two carts.
	- Also should probably only apply to the free cart, not the player's stack.
- **Green: Slide carts**
    - Intention is to give the player a consumable up/down boost that applies to the entire stack of players carts in order to allow escaping challenging scenarios or reach a cart or return corral that would otherwise be too far.

**How do these effects play out over time?**
- Each green cart provides one slide consumable.
- Each red cart provides two seconds of magnetism, which is enabled manually by holding a button
- Each blue cart immediately slows the scroll speed down when returned.
	
---

## Procedural Generation
- A small set of tiles will serve as the building blocks for the game world
- Tiles contain metadata such as possible locations for obstacles:
  - Parking spaces _may have_ parked cars
  - Lanes _may have_ crossing vehicles or pedestrians
  - Carts _may be_ located randomly within restricted regions in each tile
- Tiles also contain static obstacles where appropriate
- Generation algorithm will enact semi-random selection of upcoming tiles
  - Avoid back-to-back repeats of tiles
- Populate tiles with semi-random obstacles based on density parameter(s)
  - May want to reduce density slightly as speed increases

---

## Visual Design
- Due to lack of artistic talent in the cretor, the 

---

## Sound Design
TODO