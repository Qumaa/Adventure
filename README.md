# Adventure
SOLID principles and patterns personal playground

Goals:
1. Reliable ground, walls and ceiling sensing for 2d physics (based on contact filters)
2. Animator driver thing, extensible to any set of states and animations. Either custom state implementation or through the mecanim
3. Start experimenting with AI for allies and enemies
4. Player controller must support multiple characters

# Notes and logs
- First make a working code, then split it to standalone scripts and finally to abstractions and realisations. Building it like this from scratch takes way too much effort and often goes the wrong way around, requiring you to rebuild what you have often
- Interface type fields use SerializeReference attribute to be present in editor (same for any abstract value type, however not clear to what extent. Props, base classes, generic?)
