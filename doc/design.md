# Game Design Document
## Summary
Minimum wage is a simple, 2D endless-runner in which the player must collect and return shopping carts from a parking lot while avoiding collisions with obstacles such as cars and pedestrians. Points are awarded to the player continuously over time. Speed increases as the round progresses, making it increasingly difficult to avoid obstacles, but the player can increase his/her chances of success by returning carts of various color combinations, providing the player with various benefits / power-ups.

---

## Gameplay Overview
### Interface
- **The game world is presented in a 2D, top-down fashion.**
	- Tentatively intended to _not_ be isometric or angled such that fronts/sides of world objects are visible. This is mostly because this is a game about precise positioning, and an angled perspective introduces positional ambiguity.
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
- **No other character control is afforded to the player.**
	- However, the player can pause/restart the round with a configurable key/button.

### Gameplay Loop
- **The player's fundamental goal is to avoid colliding with in-world obstacles (such as cars and pedestrians) for as long as possible.**
	- The gameplay world and positions of obstacles are procedurally generated based on a set of world tiles with spawn point metadata.
- **The player starts each round with zero points, and accumulates points continuously over time.**
	- Points are accumulated at a base rate times a multiplier
	- Additional points are earned when returning carts
- **The scroll speed of the world (i.e. the player character's forward movement speed) slowly increases over time.**
	- Left unattended, the scroll speed quickly becomes insurmountably fast, and the player will unavoidably collide with an obstacle.
- **To maximize points and mitigate the ever-increasing speed, the player must collect shopping carts scattered throughout the game world and return them to a designated corral.**
	- Carts are collected by ramming them with the front cart in the player's stack.
		- Carts are always oriented correctly to be picked up (facing right).
	- The player's stack of carts becomes increasingly difficult to control as the number of stacked carts increases.
		- To motivate the player to collect a larger stack, a multiplier is applies to the point accumulation rate based on the number of carts in the player's stack at a given time.
	- Carts can be returned to a cart corral, which will appear periodicaly in the world.
	- Shopping carts come in various colors, and each color has an effect that benefits the player when the cart is returned to the corral.
		- See [Cart Color Effects](#cart-color-effects) for more details

### Context
- The player character is a minimum wage employee working at a store located in a suspiciously long strip mall.
- The player character's job is to collect all of the shopping carts that customers have carelessly left strewn about the parking lot, and return them to a designated cart corral.
- The player character must avoid colliding with obstacles such as cars and pedestrians, lest he/she be held liable for damages/injury.
- Player score is money made on the job


---

## Mockup
![Mockup](mockup.png)
- Add visual indicators for cart bonuses
    - Could be UI elements, _or_ could be more implicit:
      - Slowdown: Reduced movement speed should be obvious
      - Magnetism: Front cart glows red, nearby carts also glow
      - Agility: Player character's feet burn with green flames

---
## Gameplay Details
_Note: Exact mechanics and (especially) numerical values will likely change significantly as prototyping continues, so concepts are kept fairly general and qualitative for now._
### Cart Color Effects
Returning carts of a given color to the cart return corral results in a temporary benefit for the player. The goal here is to build and release tension, and to reward players for skill (and luck) in collecting carts.

- **Blue: Slow the scroll speed**
	- Intention is to help counteract the endless scroll speed increase.
	- Should be designed and tuned such that a skilled player can effectively undo some but not all of the speed increase since the previous cart return.
		- Alternatively, allow significant/arbitrary slow-down, but track an _intended_ speed over time and adjust the speed increase rate so that it wants to converge on the target.
- **Red: Magnetic carts**
	- Intention is simply to make it easier to pick up carts as speed increases.
	- Magnetic effect should require line-of-sight between the two carts.
	- Also should probably only apply to the free cart, not the player's stack.
- **Green: Player agility**
    - Intention is to allow the player to navigate more effectively as the pace of the game increases.

**TODO: Need to work out how the effects play out over time:**
- Full effect for a limited duration, then goes away entirely?
    - Does the number of carts impact the effect, the duration, or both?
- Full effect initially, but effect continually decays?
    - Easier to reason about, but it might not be as fun to the player.
- Could be different for each!
	
### Cart Color Combos
- In general, turning in more carts should increase the benefit to the player
- To encourage more selective cart colleciton, collecting carts of the same color consecutively will award a X% bonus relative to collecting the same carts in mixed order.

### Point Collection
- Points are collected at a base rate of 10 points per second times a multiplier (default 1.0)
- Each extra cart the player has in his/her stack awards a X% multiplier increase
- Returning carts provides bulk bonus points

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
TODO

---

## Sound Design
TODO