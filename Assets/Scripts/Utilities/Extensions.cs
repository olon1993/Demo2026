using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheFrozenBanana
{
    public static class Extensions
    {
        static public Vector3 XZPlane(this Vector3 vec)
        {
            return new Vector3(vec.x, 0f, vec.z);
        }
        static public Vector2 XZPlane(this Vector2 vec)
        {
            return new Vector2(vec.x, vec.y);
        }
    }
}
