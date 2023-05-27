using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TileGenerator : MonoBehaviour
{
    [SerializeField] private GameObject _tilePrefab;
    [SerializeField] private float _speed;
    [SerializeField] private float _maxCount;
    [SerializeField] private List<Tile> _tiles = new List<Tile>();
    [SerializeField] private Transform _tileHolder;
    

    private void Start()
    {
        _tiles.First().speed = _speed;
        for (int i = 0; i < _maxCount; i++)
        {
            GenerateTile();
        }
    }
    void Update()
    {
        if (_tiles.Count < _maxCount)
        {
            GenerateTile();
        }
    }

    private void GenerateTile()
    {
       GameObject newTileObgect = Instantiate(_tilePrefab, _tiles.Last().transform.position + Vector3.forward * _tilePrefab.transform.localScale.z, Quaternion.identity); 
       Tile newTile = newTileObgect.GetComponent<Tile>();
       newTile.speed = _speed;
       _tiles.Add(newTile);
        newTileObgect.transform.SetParent(_tileHolder);


    }
    private void OnTriggerEnter(Collider other)
    {
        _tiles.Remove(other.GetComponent<Tile>());
        Destroy(other.gameObject);
    }
}
