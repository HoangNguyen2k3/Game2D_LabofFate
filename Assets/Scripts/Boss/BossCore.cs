using UnityEngine;

public abstract class BossCore : MonoBehaviour
{
    public GameObject target;
    public Rigidbody2D body;
    public Animator animator;
    public StateMachine stateMachine;

    public BossState state => (BossState)stateMachine.state;
    public Boss boss;

    public void SetupInstances()
    {
        stateMachine = new StateMachine();

        BossState[] childStates = GetComponentsInChildren<BossState>();
        foreach (BossState state in childStates)
        {
            state.Setup(this);
        }

    }
}
