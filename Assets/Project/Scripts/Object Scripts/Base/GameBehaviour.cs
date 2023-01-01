using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GameBehaviour : MonoBehaviour
{
    // references
    protected virtual void SetUpValues() { return; }
    private void Awake() => SetUpValues();

    // debug
    protected void DrawRay(Vector2 from, Vector2 offset, bool triggered = false) =>
        Extensions.DrawRay(from, offset, triggered);

    protected void DrawLine(Vector2 from, Vector2 to, bool triggered = false) =>
        Extensions.DrawLine(from, to, triggered);
}