using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct ColliderShape
{
    public List<Vector2> points;

    public ColliderShape(List<Vector2> points)
    {
        this.points = points;
    }
}