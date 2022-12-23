# Adventure
SOLID principles and patterns personal playground

Goals:
1. Reliable ground, walls and ceiling sensing for 2d physics (based on contact filters) <|> Done!
2. Animator driver thing, extensible to any set of states and animations. Either custom state implementation or through the mecanim <|> Not even started
3. Start experimenting with AI for allies and enemies <|> Not even started
4. Player controller must support multiple characters <|> In progress

# Notes and logs
- First make a working code, then split it to standalone scripts and finally to abstractions and concretions. Building it "cleanly" from scratch takes way too much effort and often goes wrong, making you erase your progress and start over again
- Interface type fields use SerializeReference attribute to be present in editor (same for any abstract value type, however not clear to what extent. Props, base classes, generic?) !!! SEE NEXT PARAGRAPH !!!
- Don't try to use interfaces as an "universal data container for many classes", this is not their intentional use. Interfaces are used in patterns and to group different classes by providing them same methods. Using an interface in try get component to interact() or apply_damage() is the example
- 3 layers of abstracion, interface -> abstract base -> concrete class. Only create previous layer if you got more than one instance on the current one. There's no need to create an interface for every base class unless you're sure there eventually will be more of them, same goes for the base. You don't need a base class for a button script that executes a single line of code in the OnClick callback
- IMPORTANT: know the difference between abstract base and interfaces, and know when and what to use. Abstract base defines WHAT THE OBJECT IS, while interfaces only define WHAT THE OBJECT MUST DO. Don't use them for other purposes
- Referencing base classes is not a crime. Yes, it makes dependencies more critical, but copying needed functionality to an interface and "cleanly" using it instead doesn't make your code any clearer, but overcomplicates it. See above for more notes


# Note 1: Animations to add
Male character:
1. Idle
2. Idle with sword
3. Walk
4. Crouch Walk
5. Run
6. Run Wild
7. Wall Run
8. Crouch
9. Slide
10. Attack Sword1
11. Attack Sword2
12. Attack Sword3
13. Punch1 
14. Punch2
15. Punch3
16. Kick1
17. Kick2
18. Air Attack1
19. Air Attack2
20. Air Attack3
21. Air Drop Kick 
22. Cast Spell
23. Use Item
24. Jump
25. Fall
26. Somersault
27. Corner Grab
28. Corner Climb
29. Corner Jump
30. Sword Draw
31. Sword Sheathe
32. Wall Slide
33. Wall Climb
34. Knocked Down
35. Recover
36. Hurt
37. Die

