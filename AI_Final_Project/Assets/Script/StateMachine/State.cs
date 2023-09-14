using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State : MonoBehaviour
{
    public interface IState
    {
        void Enter(StateMachine stateMachine);
        void Execute(StateMachine stateMachine);
        void Exit(StateMachine stateMachine);
    }
    public class IdleState : IState
    {
        private float idleTimer;
        private float timeToWander = 5f;
        public void Enter(StateMachine stateMachine)
        {
            idleTimer = 0;
        }

        public void Execute(StateMachine stateMachine)
        {
            idleTimer += Time.deltaTime;

            if (idleTimer >= timeToWander)
            {
                stateMachine.ChangeState(new WanderState());
            }

            GameObject detectedEnemy = stateMachine.DetectEnemies();
            if (detectedEnemy != null)
            {
                stateMachine.ChangeState(new AttackState(detectedEnemy));
                return;
            }
            else
            {
            }
        }

        public void Exit(StateMachine manager)
        {
            idleTimer = 0;
        }

    }


    public class WanderState : IState
    {
        private float wanderTimer;
        private float timeToIdle = 5f;
        private Vector3 nextWaypoint;
        private float speed = 2f;

        public void Enter(StateMachine manager)
        {
            wanderTimer = 0;
            SetRandomWaypoint(manager);
        }

        public void Execute(StateMachine manager)
        {
            wanderTimer += Time.deltaTime;

            Vector3 step = speed * Time.deltaTime * (nextWaypoint - manager.transform.position).normalized;
            manager.transform.position += step;

            if (Vector3.Distance(manager.transform.position, nextWaypoint) < 0.1f)
            {
                SetRandomWaypoint(manager);
            }

            if (wanderTimer >= timeToIdle)
            {
                manager.ChangeState(new IdleState());
            }

            GameObject detectedEnemy = manager.DetectEnemies();
            if (detectedEnemy != null)
            {
                manager.ChangeState(new AttackState(detectedEnemy));
            }
        }

        public void Exit(StateMachine manager)
        {
            wanderTimer = 0;
        }

        private void SetRandomWaypoint(StateMachine manager)
        {
            SpriteRenderer spriteRenderer = manager.boundarySquare.GetComponent<SpriteRenderer>();
            Vector2 spriteSize = spriteRenderer.sprite.bounds.size;
            Vector2 actualSize = spriteSize * manager.boundarySquare.transform.localScale;

            float halfWidth = actualSize.x / 2;
            float halfHeight = actualSize.y / 2;

            float minX = manager.boundarySquare.transform.position.x - halfWidth;
            float maxX = manager.boundarySquare.transform.position.x + halfWidth;

            float minY = manager.boundarySquare.transform.position.y - halfHeight;
            float maxY = manager.boundarySquare.transform.position.y + halfHeight;

            float newX = Random.Range(minX, maxX);
            float newY = Random.Range(minY, maxY);

            nextWaypoint = new Vector3(newX, newY, manager.transform.position.z);
        }
    }
    public class AttackState : IState
    {
        private GameObject targetEnemy;
        private float speed = 5.0f;
        private float attackRange = 1.0f;

        public AttackState(GameObject enemy)
        {
            this.targetEnemy = enemy;
        }

        public void Enter(StateMachine stateMachine)
        {
        }


        public void Execute(StateMachine stateMachine)
        {
            if (targetEnemy == null)
            {
                stateMachine.ChangeState(new IdleState());
                return;
            }
            float distanceToEnemy = Vector3.Distance(stateMachine.transform.position, targetEnemy.transform.position);
            GameObject detectedEnemy = stateMachine.DetectEnemies();
            if (distanceToEnemy <= attackRange)
            {
               Vector2 steeringForce = stateMachine.steeringModule.Seek(targetEnemy.transform.position);
               Vector2 newPosition = (Vector2)stateMachine.transform.position + steeringForce * Time.deltaTime;
               stateMachine.transform.position = newPosition;
               Debug.Log("Attacking the enemy!");
               Destroy(detectedEnemy);   
            }
            else
            {
                Vector3 step = speed * Time.deltaTime * (targetEnemy.transform.position - stateMachine.transform.position).normalized;
                stateMachine.transform.position += step;
            }
        }

        public void Exit(StateMachine stateMachine)
        {
        }
    }

}



