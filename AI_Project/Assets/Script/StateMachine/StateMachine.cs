using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static State;

public class StateMachine : MonoBehaviour
{
    private IState currentState;

    public GameObject boundarySquare;
    public SteeringModule steeringModule;
    public Perception perception;

    public LayerMask obstacleLayer;

    void Start()
    {
        currentState = new IdleState();
        currentState.Enter(this);
        steeringModule = GetComponent<SteeringModule>();
        perception = GetComponent<Perception>();
        obstacleLayer = LayerMask.GetMask("ObstacleLayer");
    }

    void Update()
    {
        currentState.Execute(this);
    }

    public void ChangeState(IState newState)
    {
        currentState.Exit(this);
        currentState = newState;
        currentState.Enter(this);

        // Set the speed control based on the current state
        if (currentState is WanderState)
        {
            steeringModule.SetSpeedControl(SteeringModule.SpeedControl.Slow);
        }
        else if (currentState is AttackState)
        {
            steeringModule.SetSpeedControl(SteeringModule.SpeedControl.Fast);
        }
    }

    public GameObject DetectEnemies()
    {
        return perception.DetectClosestEnemy();
    }
}
