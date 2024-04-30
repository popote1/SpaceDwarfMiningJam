using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugMapGenerator : MonoBehaviour {
    [SerializeField] private Vector2Int _mapSize = new Vector2Int(50,50);
    [SerializeField] private SpriteRenderer _tileSprite;

    [Space(10)] 
    [SerializeField] private bool _generateInUpdate;

    [Space(10)] 
    [SerializeField] private bool _generateCircle;
    [Range(0, 5)] [SerializeField] private float _maxdistanceFactor = 1;
    [Range(0, 2)] [SerializeField] private float _mindistanceFactor = 0;
    
    [Space(10)] 
    [SerializeField] private bool _generateCenterCircle;
    [Range(0, 5)] [SerializeField] private float _maxdistanceCentreFactor = 1;
    [Range(0, 2)] [SerializeField] private float _mindistanceCentreFactor = 0;

    [Space(10)]
    [SerializeField] private bool _generatePerlineNoise;
    [SerializeField] private Vector2 _PerlinOffset;
    [SerializeField] private float _PerlinZoom;
    
    [Space(10)]
    [SerializeField] private bool _generateTorus;
    [SerializeField] private bool _generateHardTorus;
    [SerializeField][Range(0, 1)] private float _torusRadius=0.5f;
    [SerializeField] private float _torusThikness = 5;
    [Space(10)]
    [SerializeField] private bool _generateSampled;

    [SerializeField] private bool _generateSampledTorus;

    [SerializeField] private bool _usThreashhold;
    [Range(0, 1)] [SerializeField] private float _threashHold = 0.5f;
    
    
    
    private SpriteRenderer[,] _tiles;

    private void Start() {
        _tiles = new SpriteRenderer[_mapSize.x, _mapSize.y];
        for (int x = 0; x < _mapSize.x; x++) {
            for (int z = 0; z < _mapSize.y; z++)
            {
                _tiles[x, z] = Instantiate(_tileSprite, new Vector3(x, 0, z), Quaternion.identity);
                _tiles[x, z].transform.forward = Vector3.up;
            }
        }
    }

    private void Update()
    {
        if (_generateInUpdate)
        {
            ManageTerrainColor();
        }
    }

    private void ManageTerrainColor() {
        if (_generateCircle) {
            for (int x = 0; x < _mapSize.x; x++)
            {
                for (int z = 0; z < _mapSize.y; z++)
                {
                    float value = GetCircleValue(new Vector2(x, z));
                    _tiles[x, z].color = new Color(value, value, value, 1);
                }
            }
        }
        
        if (_generateCenterCircle) {
            for (int x = 0; x < _mapSize.x; x++) {
                for (int z = 0; z < _mapSize.y; z++) {
                    float value = GetCircleCenterValue((new Vector2(x, z)));
                    _tiles[x, z].color = new Color(value, value, value, 1);
                }
            }
        }
        
        if (_generatePerlineNoise) {
            for (int x = 0; x < _mapSize.x; x++) {
                for (int z = 0; z < _mapSize.y; z++) {
                    float value = GetPerlinValue(x ,z);
                    _tiles[x, z].color = new Color(value, value, value, 1);
                }
            }
        }

        if (_generateTorus)
        {
            for (int x = 0; x < _mapSize.x; x++) {
                for (int z = 0; z < _mapSize.y; z++) {
                    float value = GetTorusValue(new Vector2(x,z));
                    _tiles[x, z].color = new Color(value, value, value, 1);
                }
            }
        }
        if (_generateHardTorus)
        {
            for (int x = 0; x < _mapSize.x; x++) {
                for (int z = 0; z < _mapSize.y; z++) {
                    float value = GetHardTorusValue(new Vector2(x,z));
                    _tiles[x, z].color = new Color(value, value, value, 1);
                }
            }
        }

        if (_generateSampled) {
            for (int x = 0; x < _mapSize.x; x++) {
                for (int z = 0; z < _mapSize.y; z++)
                {
                    Vector2 pos = new Vector2(x, z);
                    float value = GetCircleValue(pos) * GetPerlinValue(x, z) * GetCircleCenterValue(pos);
                    if (_usThreashhold) {
                        if (value < _threashHold) _tiles[x, z].color = Color.black;
                        else _tiles[x, z].color = Color.white;
                    }
                    else
                    {
                        _tiles[x, z].color = new Color(value, value, value, 1);
                    }
                }
            }
        }
        if (_generateSampledTorus) {
            for (int x = 0; x < _mapSize.x; x++) {
                for (int z = 0; z < _mapSize.y; z++)
                {
                    Vector2 pos = new Vector2(x, z);
                    float value = GetPerlinValue(x, z) * GetHardTorusValue(pos);
                    if (_usThreashhold) {
                        if (value < _threashHold) _tiles[x, z].color = Color.black;
                        else _tiles[x, z].color = Color.white;
                    }
                    else
                    {
                        _tiles[x, z].color = new Color(value, value, value, 1);
                    }
                }
            }
        }
    }

    private float GetPerlinValue(float x , float y) {
        return 1-Mathf.Clamp01(Mathf.PerlinNoise((x+_PerlinOffset.x)*_PerlinZoom, (y+_PerlinOffset.y)*_PerlinZoom));
    }

    private float GetCircleValue(Vector2 pos)
    {
        float radius = _mapSize.x / 2f;
        Vector2 center = _mapSize / 2;
        float dist = Vector2.Distance(pos, center)*_maxdistanceFactor;
        return 1-( dist / radius-_mindistanceFactor);
    }
    private float GetCircleCenterValue(Vector2 pos)
    {
        float radius = _mapSize.x / 2f;
        Vector2 center = _mapSize / 2;
        float dist = Vector2.Distance(pos, center)*_maxdistanceCentreFactor;
        return 1-( dist / radius-_mindistanceCentreFactor);
    }

    private float GetTorusValue(Vector2 pos)
    {
        float dist = Vector2.Distance(pos, _mapSize / 2);
        float radius = _mapSize.x / 2f * _torusRadius; 
        if (dist > radius) {
            return 1- (dist-radius)/(_torusThikness/2f);
        }
        else
        {
            return (dist - (radius - (_torusThikness / 2f)) )/ (_torusThikness / 2f);
        }
    }
    private float GetHardTorusValue(Vector2 pos)
    {
        float dist = Vector2.Distance(pos, _mapSize / 2);
        float radius = _mapSize.x / 2f * _torusRadius;
        float innerRadius = _mapSize.x / 2f * _torusRadius -_torusThikness / 2;
        float outerRadius = _mapSize.x / 2f * _torusRadius + _torusThikness / 2;
        if (innerRadius < dist && dist < outerRadius)
        {
            return 1;
        }
         return 0;
    }
}
