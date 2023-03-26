using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coins : MonoBehaviour
{
    public static int totalCoins = 0;

    void Awake()
    {
        GetComponent<Collider2D>().isTrigger = true;
    }

    private void OnTriggerEnter2D(Collider2D coinCollision)
    {
        if (coinCollision.CompareTag("Player"))
        {
            totalCoins++;
            Destroy(gameObject);
        }
    }
}
