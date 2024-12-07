using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss3 : BossCore
{

    public SpriteRenderer sprite;
    private EnemyHealth health;
    private float maxHeath;

    public Boss3Arm leftArm;
    public Boss3Arm rightArm;
    public BossState idleState;
    public BossState chaseState;
    public BossState armPunchState;
    public BossState armSmashState;
    public BossState armLaserState;
    public BossState spinAttackState;

    public Vector2 movePosition;
    public float angularSpeed = 15f;
    public float circleRad = 0.1f;
    private float currentAngle;

    private BossState[] phaseOneStates;
    private BossState[] phaseTwoStates;
    private BossState[] phaseThreeStates;

    public enum Phase {
        PhaseOne,
        PhaseTwo,
        PhaseThree
    }

    public Phase currentPhase;
    
    private bool enterPhaseTwo;
    private bool enterPhaseThree;

    private void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
        health = GetComponent<EnemyHealth>();
        body = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        movePosition = this.transform.position;
        SetupInstances();
        maxHeath = health.currentHealth.Value;
        stateMachine.SetState(idleState);
        Debug.Log(maxHeath);
        phaseOneStates = new BossState[] {
            chaseState, armPunchState
        };

        phaseTwoStates = new BossState[] {
            chaseState, armPunchState, armLaserState, armSmashState, armSmashState, armLaserState, armLaserState
        };

        phaseThreeStates = new BossState[] {
            chaseState, armSmashState, armSmashState, armLaserState, armLaserState
        };

        currentPhase = Phase.PhaseOne;
    }

    private void Update() 
    {
        if (currentPhase == Phase.PhaseOne && health.currentHealth.Value <= 45f)
        {
            currentPhase = Phase.PhaseTwo;
        }
        if (currentPhase == Phase.PhaseTwo && health.currentHealth.Value <= 20f)
        {
            currentPhase = Phase.PhaseThree;
        }

        if (state.IsComplete)
        {
            SelectState();
        }
        state.FrameUpdate();  
    }

    private void FixedUpdate()
    {
        state.PhysicsUpdate();
        MoveInCircle();
    }

    private void SelectState()
    {
        if (!isAggro)
        {
            stateMachine.SetState(idleState);
            return;
        }
        int choice;
        BossState selectedState = idleState;
        switch (currentPhase)
        {
            case Phase.PhaseOne:
                choice = Random.Range(0, phaseOneStates.Length);
                selectedState = phaseOneStates[choice];
                break;
            
            case Phase.PhaseTwo:
                if (!enterPhaseTwo)
                {
                    enterPhaseTwo = true;
                    selectedState = armLaserState;
                    break;
                }
                choice = Random.Range(0, phaseTwoStates.Length);
                selectedState = phaseTwoStates[choice];
                break;
            
            case Phase.PhaseThree:
                if (!enterPhaseThree)
                {
                    enterPhaseThree = true;
                    selectedState = spinAttackState;
                    break;
                }
                choice = Random.Range(0, phaseThreeStates.Length);
                selectedState = phaseThreeStates[choice];
                break;
        }
        stateMachine.SetState(selectedState, true);
    }

    public void MoveInCircle()
    {
        currentAngle += angularSpeed * Time.deltaTime;
        Vector2 offset = new Vector2 (Mathf.Sin(currentAngle), Mathf.Cos(currentAngle)) * circleRad;
        
        body.MovePosition(movePosition + offset);
    }

    public Vector2 GetTargetPosition()
    {
        return target != null ? target.transform.position : Vector2.zero;
    }
}
