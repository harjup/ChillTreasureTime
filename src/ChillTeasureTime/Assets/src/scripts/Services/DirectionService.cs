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
            new List<Direction>{new Line("Tiny Bird", "Im bird 3. Hello hello!")}}
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
