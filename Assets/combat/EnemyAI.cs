using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    private enum State
    {
        Roaming
    }
    private State state;
    private EnemyPathfinding enemyPathfinding; // Corrected the variable name

    private void Awake()
    {
        enemyPathfinding = GetComponent<EnemyPathfinding>(); // Corrected the component name
        state = State.Roaming;
    }

    private void Start()
    {
        StartCoroutine(RoamingRoutine());
    }

    private IEnumerator RoamingRoutine()
    {
        while (state == State.Roaming)
        {
            Vector2 roamposition = GetRoamingPosition();
            enemyPathfinding.MoveTo(roamposition);
            yield return new WaitForSeconds(2f);
        }
    }

    private Vector2 GetRoamingPosition()
    {
        return new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
    }
}
