using System;
using UnityEngine;
using UnityEngine.Events;

public interface IMovementAI
{
    enum State
    {
        Seeking,
        Follow,
        Dead
    }

    void SetState(State state);

    void ApplyForce(Transform transform);
}
