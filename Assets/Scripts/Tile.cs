using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public float speed;

    [SerializeField] private List<Transform> _points = new List<Transform>();
    [SerializeField] private GameObject _coin;

    void Start()
    {
        int randomPointIndex = Random.Range(0, _points.Count);
        GameObject newCoin = Instantiate(_coin, _points[randomPointIndex].position, Quaternion.identity);
        newCoin.transform.SetParent(transform);
    }

    private void FixedUpdate()
    {
        transform.Translate(Vector3.back * speed * Time.fixedDeltaTime);
    }
}
