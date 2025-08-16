using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IActiveDrag
{
    // Cancel immediately without scoring or pot checks
    void CancelDragSilent();
}