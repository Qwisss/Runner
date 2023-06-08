using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadGenerator : MonoBehaviour
{
    [SerializeField] private GameObject _roadPrefab;
    [SerializeField] private List<GameObject> _roads = new List<GameObject>();
    [SerializeField] private float _maxSpeed;
    [SerializeField] private float _speed;
    [SerializeField] private int _maxRoadCount;

    

    private void Start()
    {
        
        ResetLevel();
        //StartLevel();
    }

    private void Update()
    {
        if (_speed == 0)
        {
            return;
        }

        foreach (GameObject _road in _roads)
        {
            _road.transform.position -= new Vector3(0, 0, _speed * Time.deltaTime);
        }
        if (_roads[0].transform.position.z < -20)
        {
            Destroy(_roads[0]);
            _roads.RemoveAt(0);

            CreateNextRoad();
        }
    }

    private void CreateNextRoad()
    {
        Vector3 pos = Vector3.zero;
        if (_roads.Count > 0)
        {
            pos = _roads[_roads.Count - 1].transform.position + new Vector3(0, 0, 20);
        }
        GameObject go = Instantiate(_roadPrefab,pos, Quaternion.identity);
        go.transform.SetParent(transform);
        _roads.Add(go);
    }

    public void StartLevel()
    {
        Cursor.lockState = CursorLockMode.Locked;
        FindObjectOfType<Player>().StartRun();
        _speed = _maxSpeed;
        SwipeSystem.instance.enabled = true;
    }


    public void ResetLevel()
    {
        _speed = 0;
        while (_roads.Count > 0) 
        {
            Destroy(_roads[0]);
            _roads.RemoveAt(0);
        }
        for (int i = 0; i < _maxRoadCount; i++)
        {
            CreateNextRoad();
        }
        SwipeSystem.instance.enabled = false;
    }
}
