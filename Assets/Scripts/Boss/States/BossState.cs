using System.Collections;
using UnityEngine;

public abstract class BossState : State
{
    protected BossCore core;

    protected Rigidbody2D body => core.body;
    protected Animator animator => core.animator;
    public AnimationClip anim;

    public BossCore boss;

    private void Awake()
    {
        boss = GetComponentInParent<BossCore>();
    }

    public void Setup(BossCore _core)
    {
        core = _core;
    }
}
