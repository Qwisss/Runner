using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AnimationItem : MonoBehaviour
{
  
    [SerializeField] private int _indexItem;
    [SerializeField] TextMeshProUGUI _objectName;   
    [SerializeField] private AnimationClip animationClip;
    [SerializeField] private int _price;
    [SerializeField] private int _access;
    [SerializeField] GameObject _block;
    [SerializeField] TextMeshProUGUI _objectPrice;
    [SerializeField] TextMeshProUGUI _CoinCount;
    [SerializeField] private bool _isEquip = false;
    

    private void Awake()
    {
        //PlayerPrefs.SetInt("coins", 1000); //AddMoney
        AccessUpdate();       
        
    }
    private void Start()
    {
        EquipItem();
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

    public void EquipItem()
    {
        _isEquip = true;
        AccessUpdate();
        if (_access == 1)
        {
            AccessUpdate();
            if ( _indexItem == 0)
            {
                DataHolder.runIndexForPlayerController = 0;
                AccessUpdate();              
                Debug.LogError(DataHolder.runIndexForPlayerController);
            }
            if (_indexItem == 1)
            {
                DataHolder.runIndexForPlayerController = 1;
                AccessUpdate();
                
                Debug.LogError(DataHolder.runIndexForPlayerController);
            }
            if (_indexItem == 2)
            {
                DataHolder.runIndexForPlayerController = 2;
                AccessUpdate();
                
                Debug.LogError(DataHolder.runIndexForPlayerController);
            }
            AccessUpdate();
        }
        
    }
  

    public void AccessUpdate()
    {
        _access = PlayerPrefs.GetInt(_objectName + "Access");
        _objectPrice.text = _price.ToString();
        _CoinCount.text = PlayerPrefs.GetInt("coins").ToString();


        if (_access == 1)
        {
            _block.SetActive(false);
            _objectPrice.gameObject.SetActive(false);
        }
    }

}
