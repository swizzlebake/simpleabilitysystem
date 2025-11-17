<a href="https://github.com/swizzlebake/simpleabilitysystem">Simple Ability System</a> © 2025 by <a href="https://github.com/swizzlebake">Emily Peck</a> is licensed under <a href="https://creativecommons.org/licenses/by/4.0/">CC BY 4.0</a><img src="https://mirrors.creativecommons.org/presskit/icons/cc.svg" alt="" style="max-width: 1em;max-height:1em;margin-left: .2em;"><img src="https://mirrors.creativecommons.org/presskit/icons/by.svg" alt="" style="max-width: 1em;max-height:1em;margin-left: .2em;">

# simpleabilitysystem

A simple C# ability system in Unity to try out some things

# goals
The goal for this repository was to create a small simple ability system that allows for attributes (such as health, speed and range) and abilities (attack, move) that can be activated by entities. It contains a small visualzation of this with two types of entities, a wandering bomber and a seeking bomber which wander around the world fighting. There is no death of entities. 

I tried to keep the structures lean, a clean separtion between serialized data and runtime data. A clean separation of visuals from business logic. Though I would have liked to add in some requirements from the visuals for "wind up" functionality, where the ability must wait for the visuals to finish before completing. I did not implement this now.

# structure
Core classes worth noting:

	Trait -> contains a set of attributes and a set of abilities
	Ability -> handles the action of performing an ability and possibly applying effects
	Effect -> modifies attributes for a duration (duration effects not implemented yet)
	FloatAttribute -> an attribute backed by a float, some light editor tooling is used to show this in inspectors
	AbilitySystem -> manages a set of abiltiies (via traits) and active effects
	Entity -> the root object to query for and keep references to in the world
	GameTag -> a tag used as a marker for references or to handle game tag events

Config:

	DataConfig -> contains methods to be overidden by subclasses to return prefab and traits

Visual:

	EntityVisuals -> handles the visual representation of an event or change to attributes

# thoughts
My biggest issue with the code is the use of strings, I'd prefer something with a string name but some backing field like an int/guid that can be relied upon in cases of renaming. 

One could implement some of the things mentioned above, like wind up abilities and duration based effects. 