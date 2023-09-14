using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Perception : MonoBehaviour
{
    public float detectionRadius = 5f;

    public GameObject DetectClosestEnemy()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, detectionRadius);
        GameObject closestEnemy = null;
        float closestDistance = detectionRadius;

        foreach (Collider2D hit in hits)
        {
            if (hit.tag == "Enemy") 
            {
                float distance = Vector2.Distance(transform.position, hit.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestEnemy = hit.gameObject;
                }
            }
        }

        return closestEnemy; 
    }
}
