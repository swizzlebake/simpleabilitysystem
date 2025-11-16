# simpleabilitysystem
A simple C# ability system in Unity to try out some things

The goal for this repository was to create a small simple ability system that allows for attributes (such as health, speed and range) and abilities (attack, move) that can be activated by entities. It contains a small visualzation of this with two types of entities, a wandering bomber and a seeking bomber which wander around the world fighting. There is no death of entities. 

I tried to keep the structures lean, a clean separtion between serialized data and runtime data. A clean separation of visuals from business logic. Though I would have liked to add in some requirements from the visuals for "wind up" functionality, where the ability must wait for the visuals to finish before completing. I did not have time for this right now. 

My biggest issue with the code is the use of strings, I'd prefer something with a string name but some backing field like an int/guid that can be relied upon in cases of renaming. 