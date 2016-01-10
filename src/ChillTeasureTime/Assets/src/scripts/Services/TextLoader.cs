using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TextLoader : Singleton<TextLoader>
{

    Dictionary<string, List<Line>> _lineMap = new Dictionary<string, List<Line>>()
    {
        {"Basic",  new List<Line> {new Line("Signpost", "I AM A BEAUTIFUL SIGNPOST THAT IS ALL"),new Line("Signpost", "ALSO, I AM MADE OUT OF WOOD"),new Line("Signpost", "ALRIGHT, I HAD A LITTLE EXTRA")}},
        {"ShinyNest", new List<Line> {new Line("Signpost", "Ermile's completely original shiny collection. Do not steal.")}}
        
    };

    public List<Line> GetLinesById(string id)
    {
        if (_lineMap.ContainsKey(id))
        {
            return _lineMap[id];
        }
        

        Debug.LogError("Could not find set of lines for id '" + id +"'");
        return new List<Line>
        {
            new Line("Signpost", "Oh woe is me, I am broken... I wish I could find my text at '" + id + "'...")
        };
    }
}
