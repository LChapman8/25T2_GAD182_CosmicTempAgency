using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DragRegistry
{
    private static readonly HashSet<IActiveDrag> _active = new();

    public static void Register(IActiveDrag drag) { if (drag != null) _active.Add(drag); }
    public static void Unregister(IActiveDrag drag) { if (drag != null) _active.Remove(drag); }

    // Nuke everything being dragged *without* triggering pot logic or mistakes
    public static void CancelAllSilent()
    {
        // copy to avoid collection modification while iterating
        var snapshot = new List<IActiveDrag>(_active);
        foreach (var d in snapshot) d.CancelDragSilent();
        _active.Clear();
    }
}
