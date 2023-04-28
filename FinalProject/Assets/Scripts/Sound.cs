using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sound
{
    public Sound(Vector3 _pos, float _range)
    {
        pos = _pos;
        range = _range;
    }

    public readonly Vector3 pos; // sound location
    public readonly float range; // internsity of the sound
}
