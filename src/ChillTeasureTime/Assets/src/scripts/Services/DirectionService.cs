using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DirectionService : Singleton<DirectionService>
{

    private readonly Dictionary<string, List<Direction>> _directionsMap = 
        new Dictionary<string, List<Direction>>
    {
        { "Main-JumpHelper", 
          new List<Direction>{
              new Line("Jupley", "Hmm. I bet you could jump on those rocks."), 
              new Line("Jupley", "Imagine a \"computer keyboard\". If there were a \"Z Button\" on it, then pressing it will incite a jump."),
              new Line("Jupley", "Ergo, pressing this \"Z Button\" will get you closer to the top of those rocks. QED."), 
          }},
        { "Main-Observer", 
        new List<Direction>{
            new Line("Rocko", "It's so peaceful here with everyone else gone. The twinkling stars remind me of all the coins and silver in my nest."), 
            new Line("Rocko", "Also my-bird spouse already left."), 
        }},
        {"Main-PeckHelp",
        new List<Direction>{
            new Line("Pokey", "I think I saw a crab hiding somewhere over here. They're tricky to peck, but they always seem to have shiny stuff in their claws."), 
        }},
        {"Main-FlapHelp",
        new List<Direction>{
            new Line("Pokey", "I'm practicing my flaps."), 
            new Line("Pokey", "\"X\"... \"X\"... \"X\"..."), 
            new Line("Pokey", "If I can flap hard enough at those sand piles, I can clear the way to the lighthouse!"), 
        }},
        {"Main-Mayor",
        new List<Direction>{
           new Line("Mayor Brachie", "Hello. Ready to leave?"),
            new LeaveChoicePrompt()
        }},
        {"Lighthouse-Coffee", new List<Direction>{
            new Line("Jittery", "I've got a pot brewing. Want some coffee?"),
            new CameraFadeOut(),
            new Line("", "You share some hot coffee and talk about life. Your heart is warmed."),
            new CameraFadeIn(),
            new Line("Jittery", "I've always got more on the burner.")
        }},
        {"Lighthouse-Treasure", new List<Direction>{
            new Line("Johnny", "I heard there's a bird-pirate's treasure on top of the lighthouse."),
            new Line("Johnny", "I'd like to think his name was Johnny Sparrow."),
            new Line("Johnny", "..."),
            new Line("Johnny", "My name is Johnny."),
            new Line("Johnny", "..."),
            new Line("Johnny", "I want to be a pirate.")
        }},
        {"Rival", new List<Direction>{
            //
            new Line("Winston \"Collecto\"", "Ah! I you're here to challenge me!"),
            new Line("Winston \"Collecto\"", "Ready to tango?"),
            new FightChoicePrompt()
        }},
        {"Villiage-Rival", new List<Direction>{
            //
            new Line("Winston \"Collecto\"", "Wow, you found my special training spot. This is where I survey the whole beach to look for shiny bits and bobs."),
            new Line("Winston \"Collecto\"", "I left a couple easy ones lying around here for you, I prefer more challenge to my collecting.")
        }},
        {"Villiage-Poppy", new List<Direction>{
            //
            new Line("Poppy", "I was going to head out earlier, but I'm still waiting on a friend."),
            new Line("Poppy", "Hopefully they'll get here before my feathers freeze off.")
        }},
         {"Test-Fight", new List<Direction>{
            new FightChoicePrompt()
        }}
    };

    public List<Direction> GetDirectionsById(string id)
    {
        if (!_directionsMap.ContainsKey(id))
        {
            Debug.LogError("Unable to load dialog with id " + id);
            return new List<Direction> {new Line("Tiny Bird", "So sad. I had some lines at '" + id + "', but I couldn't find them.")};
        }

        return _directionsMap[id];
    }
}
