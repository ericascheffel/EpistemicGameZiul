INCLUDE Globals.ink

-> main

==turnOn==
Turning on!
~ computer_on = "true"
-> DONE

==notTurnON==
Ok then.

-> DONE

==main==
Look! It's a PC.
Whould you like to try it?
    + [yes]
        -> turnOn
    + [No]
        -> notTurnON
    
-> END