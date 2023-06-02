using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopMenu : MonoBehaviour
{

    [SerializeField] private string _objectName;
    [SerializeField] private int _price;
    [SerializeField] private int _access;
    [SerializeField] GameObject block;
    [SerializeField] TextMeshProUGUI _objectPrice;
    [SerializeField] TextMeshProUGUI _CoinCount;

    void Awake()
    {
        //PlayerPrefs.SetInt("coins", 1000); //AddMoney
        AccessUpdate();
    }
    private void AccessUpdate()
    {
        _access = PlayerPrefs.GetInt(_objectName + "Access");
        _objectPrice.text = _price.ToString();
        _CoinCount.text = PlayerPrefs.GetInt("coins").ToString();

        if (_access == 1)
        {
            block.SetActive(false);
            _objectPrice.gameObject.SetActive(false);
        }
    }

    public void BuyStaff()
    {
        int coins = PlayerPrefs.GetInt("coins");

        if (_access == 0)
        {
            if (coins >= _price)
            {
                PlayerPrefs.SetInt(_objectName + "Access", 1);
                PlayerPrefs.SetInt("coins", coins - _price);
                AccessUpdate();
            }
        }
    }
}
