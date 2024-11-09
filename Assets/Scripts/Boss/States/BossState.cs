using System.Collections;
using UnityEngine;

public abstract class BossState : State
{
    protected BossCore core;

    protected Rigidbody2D body => core.body;
    protected Animator animator => core.animator;
    protected Boss boss => core.boss;
    public AnimationClip anim;

    public void Setup(BossCore _core)
    {
        core = _core;
    }
}
