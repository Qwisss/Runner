using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    [SerializeField] private AudioSource _audioSource;    
    [SerializeField] private TextMeshProUGUI _coinCount;

    private int _coinsCount;

    void Awake()
    {
        _coinCount.text = PlayerPrefs.GetInt("coins").ToString();
    }

    public void AddCoin()
    {
        int coins = PlayerPrefs.GetInt("coins");
        PlayerPrefs.SetInt("coins", coins + 1);
        _coinCount.text = (coins + 1).ToString();
        _audioSource.Play();

    }
}



