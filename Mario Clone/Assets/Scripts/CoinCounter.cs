using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CoinCounter : MonoBehaviour
{
    TMP_Text countText;

    void Start()
    {
        countText = GetComponent<TMP_Text>();
    }

    void Update()
    {
        if(countText.text != Coins.totalCoins.ToString())
        {
            countText.text = Coins.totalCoins.ToString();
        }
    }
}
