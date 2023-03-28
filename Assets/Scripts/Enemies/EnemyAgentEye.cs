using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAgentEye : MonoBehaviour
{
    EnemyAgent agent;

    private void Start()
    {
        agent = GetComponent<EnemyAgent>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Player player = collision.GetComponent<Player>();
        if(player !=null)
        {
            agent.target = player.gameObject;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        Player player = collision.GetComponent<Player>();
        if (player != null)
        {
            agent.target = null;
        }
    }
}
