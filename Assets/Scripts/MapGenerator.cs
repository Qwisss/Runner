using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MapGenerator : MonoBehaviour
{
    public static MapGenerator instance;
    public List<GameObject> maps = new List<GameObject>();
    public List<GameObject> activeMaps = new List<GameObject>();
    public float laneOffset = 2.5f;


    [SerializeField] private GameObject _obstacleTopPrefab;
    [SerializeField] private GameObject _obstacleBotPrefab;
    [SerializeField] private GameObject _obstacleWallPrefab;
    [SerializeField] private GameObject _rampPrefab;
    [SerializeField] private GameObject _coinPrefab;

    private int _itemSpace = 15;
    private int _itemCountInMap = 5;
    private int coinsCountInItem = 10;
    private int mapSize;
    private float coinsHeight = 0.5f;

    
    struct MapItem
    {
        public void SetValue(GameObject obstacle, TrackPos trackPos, CoinsStyle coinStyle)
        {
            this.obstacle = obstacle;
            this.trackPos = trackPos;
            this.coinStyle = coinStyle;
        }
        public GameObject obstacle;
        public TrackPos trackPos;
        public CoinsStyle coinStyle;
    }
    enum TrackPos
    {
        Left = -1, Center , Right = +1
    }
    enum CoinsStyle
    {
        Line, Jump, Ramp
    }

    private void Awake()
    {
        instance = this; mapSize = _itemCountInMap * _itemSpace;
        maps.Add(MakeMap1());       
        maps.Add(MakeMap2());
        maps.Add(MakeMap1());
        maps.Add(MakeMap2());

        foreach (GameObject map in maps)
        {
            map.SetActive(false);
        }
    }

   
    private void Update()
    {
        if (RoadGenerator.instance.speed == 0)
        {
            return;
        }
        foreach(GameObject map in activeMaps)
        {
            map.transform.position -= new Vector3(0, 0, RoadGenerator.instance.speed * Time.deltaTime);
        }
        if (activeMaps[0].transform.position.z < -mapSize)
        {
            RemoveFirstActiveMap();
            AddActiveMap();
        }
    }

    private void RemoveFirstActiveMap()
    {
        activeMaps[0].SetActive(false);
        maps.Add(activeMaps[0]);
        activeMaps.RemoveAt(0);
    }

    public void ResetMaps()
    {
        while(activeMaps.Count > 0) 
        {
            RemoveFirstActiveMap();      
        }
        AddActiveMap();
        AddActiveMap();
    }
    private void AddActiveMap()
    {
        int r = Random.Range(0, maps.Count);
        GameObject go = maps[r];
        go.SetActive(true);
        foreach (Transform child in go.transform)
        {
            child.gameObject.SetActive(true);
        }
        go.transform.position = activeMaps.Count > 0 ?
                                activeMaps[activeMaps.Count - 1].transform.position + Vector3.forward * mapSize :
                                new Vector3(0, 0, 10);
        maps.RemoveAt(r);
        activeMaps.Add(go);
    }
    GameObject MakeMap1()
    {
        GameObject result = new GameObject("Map1");
        result.transform.SetParent(transform);
        MapItem item = new MapItem();
        for (int i = 0; i < _itemCountInMap; i++)
        {
            item.SetValue(null,TrackPos.Center,CoinsStyle.Line);
            

            if (i == 2)
            {
                item.SetValue(_obstacleWallPrefab, TrackPos.Left, CoinsStyle.Line);
            }
            else if (i == 3)
            {
                item.SetValue(_rampPrefab, TrackPos.Left, CoinsStyle.Ramp);
            }
            else if (i == 4)
            {
                item.SetValue(_obstacleBotPrefab, TrackPos.Left, CoinsStyle.Jump);
            }
            Vector3 obstaclePos = new Vector3((int)item.trackPos * laneOffset, 0, i * _itemSpace);
            CreateCoins(item.coinStyle, obstaclePos, result);

            if (item.obstacle != null)
            {
                GameObject go = Instantiate(item.obstacle, obstaclePos, Quaternion.identity);
                go.transform.SetParent(result.transform);

            }
        }
        return result;
    }
    GameObject MakeMap2()
    {
        GameObject result = new GameObject("Map1");
        result.transform.SetParent(transform);
        MapItem item = new MapItem();
        for (int i = 0; i < _itemCountInMap; i++)
        {
            item.SetValue(null, TrackPos.Center, CoinsStyle.Line);


            if (i == 2)
            {
                item.SetValue(_obstacleBotPrefab, TrackPos.Left, CoinsStyle.Jump);
            }
            else if (i == 3)
            {
                item.SetValue(_rampPrefab, TrackPos.Left, CoinsStyle.Ramp);
            }
            else if (i == 4)
            {
                item.SetValue(_obstacleBotPrefab, TrackPos.Right, CoinsStyle.Jump);
            }
            Vector3 obstaclePos = new Vector3((int)item.trackPos * laneOffset, 0, i * _itemSpace);
            CreateCoins(item.coinStyle, obstaclePos, result);

            if (item.obstacle != null)
            {
                GameObject go = Instantiate(item.obstacle, obstaclePos, Quaternion.identity);
                go.transform.SetParent(result.transform);

            }
        }
        return result;
    }

    private void CreateCoins(CoinsStyle style, Vector3 pos, GameObject parentObject)
    {
        Vector3 coinPos = Vector3.zero;
        if (style == CoinsStyle.Line)
        {
            for (int i = -coinsCountInItem / 2; i < coinsCountInItem / 2 ; i++)
            {
                coinPos.y = coinsHeight;
                coinPos.z = i * ((float)_itemSpace / coinsCountInItem);
                GameObject go = Instantiate(_coinPrefab, coinPos + pos, Quaternion.identity);
                go.transform.SetParent(parentObject.transform);
            }
        }
        else if (style == CoinsStyle.Jump)
        {
            for (int i = -coinsCountInItem / 2; i < coinsCountInItem / 2; i++)
            {
                coinPos.y = Mathf.Max(-1/2f * Mathf.Pow(i,2)+3, coinsHeight);
                coinPos.z = i * ((float)_itemSpace / coinsCountInItem);
                GameObject go = Instantiate(_coinPrefab, coinPos + pos, Quaternion.identity);
                go.transform.SetParent(parentObject.transform);
            }
        }
        else if (style == CoinsStyle.Ramp)
        {
            for (int i = -coinsCountInItem / 2; i < coinsCountInItem / 2; i++)
            {
                coinPos.y = Mathf.Min(Mathf.Max(0.7f * (i+2), coinsHeight), 3.0f);
                coinPos.z = i * ((float)_itemSpace / coinsCountInItem);
                GameObject go = Instantiate(_coinPrefab, coinPos + pos, Quaternion.identity);
                go.transform.SetParent(parentObject.transform);
            }
        }
    }
}
