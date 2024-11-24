using System.Collections;
using UnityEngine;

public abstract class State : MonoBehaviour
{
    public bool IsComplete = false;
    protected float startTime;
    public float ElapsedTime => Time.time - startTime;

    public void Initialize()
    {
        IsComplete = false;
        startTime = Time.time;
    }

    public virtual void Enter() {}
    public virtual void FrameUpdate() {}
    public virtual void PhysicsUpdate() {}
    public virtual void Exit() {}
    public virtual IEnumerator DelayedExit(float _time)
    {
        yield return new WaitForSeconds(_time);
        Exit();
    }

}
