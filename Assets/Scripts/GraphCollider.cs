#nullable enable
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public struct GraphColliderOptions {
    public double Unit;
    public double Step;
}

[RequireComponent(typeof(EdgeCollider2D))]
public class GraphCollider : MonoBehaviour
{
    public GraphColliderOptions Options {
        get => _options;
        set {
            _options = value;
            UpdateCollider();
        }
    }
    public IReadOnlyList<double> YArray {
        get => _yArray;
        set {
            _yArray = value;
            UpdateCollider();
        }
    }

    private GraphColliderOptions _options;
    private EdgeCollider2D? _edgeCollider2D;
    IReadOnlyList<double> _yArray = Array.AsReadOnly(new double[] {});

    // Start is called before the first frame update
    void Start()
    {
        _edgeCollider2D = GetComponent<EdgeCollider2D>();
        UpdateCollider();
    }

    public void UpdateCollider()
    {
        if(_edgeCollider2D == null) return;

        List<Vector2> points = new List<Vector2>(_yArray.Count);
        for (int i = 0; i < _yArray.Count; i++) {
            points.Add(new Vector2(
                (float)(i * _options.Step * _options.Unit),
                (float)(_yArray[i] * _options.Unit)
            ));
        }

        // Debug.Log(points);
        // Debug.Log(points.Count);

        _edgeCollider2D.SetPoints(points);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
