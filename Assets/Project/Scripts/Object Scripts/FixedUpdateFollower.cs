using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class FixedUpdateFollower : TransformFollower
{
    private void FixedUpdate()
    {
        FollowStep(Time.fixedDeltaTime);
    }
}
