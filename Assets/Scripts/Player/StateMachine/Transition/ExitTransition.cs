using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IExitTransition
{
    public void IsExit(float elapsedTime, float targetTime);
}
