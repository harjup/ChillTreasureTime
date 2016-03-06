using System.Collections.Generic;
using System.Linq;

public class ScrubList<T>
{
    private List<T> _items;

    private int _index;

    public ScrubList(List<T> items)
    {
        _items = items;
        _index = 0;
    }

    public List<T> All()
    {
        return _items;
    }

    public List<T> AllInactive()
    {
        return _items.Except(new List<T> {Current()}).ToList();
    } 

    public T Current()
    {
        return _items[_index];
    }

    public T Next()
    {
        var nextIndex = _index + 1;
        if (nextIndex <= _items.Count - 1)
        {
            _index = nextIndex;
        }

        return _items[_index];
    }

    public T Previous()
    {
        var nextIndex = _index - 1;
        if (nextIndex >= 0)
        {
            _index = nextIndex;
        }

        return _items[_index];
    }

    public T First()
    {
        _index = 0;
        return _items[_index];
    }
}