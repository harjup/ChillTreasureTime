using System;
using System.Collections;

public interface IExaminable
{
    IEnumerator StartSequence(Action doneCallback);
}
