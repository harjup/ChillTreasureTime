using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DirectionService : Singleton<DirectionService>
{

    private readonly Dictionary<string, List<Direction>> _directionsMap = 
        new Dictionary<string, List<Direction>>
    {
        {"1", 
            new List<Direction>{
                new Line("Tiny Bird", "Nice Bauble You Have Here."),
                new Line("Tiny Bird", "You know, as you gain shiny stuff, you're going to attract more attention."),
                new Line("Tiny Bird", "To do that, you'll need some tricks. Here's WING FLAP."),
                new GetWingFlap(),
                new Line("Tiny Bird", "If you press X, you can flap your wings and blow all sorts of stuff around! Try it on plants or sand piles!"),
            }
                },
        {"2", 
            new List<Direction>{new Line("Tiny Bird", "Im bird 2. Hello.")}},
        {"3", 
            new List<Direction>{new Line("Tiny Bird", "Im bird 3. Hello hello!")}},
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
