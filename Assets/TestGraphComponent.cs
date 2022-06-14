#nullable enable
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(GraphSamplerComponent), typeof(GameGraphController))]
public class TestGraphComponent : MonoBehaviour
{
    private GraphSamplerComponent? _graphSampler;

    // Start is called before the first frame update
    void Start()
    {
        _graphSampler = GetComponent<GraphSamplerComponent>();
        _graphSampler.Variables = new double[1]{0};
        _graphSampler.Function = (t, x) => Math.Sin(t[0]*x - t[0]);
    }

    // Update is called once per frame
    void Update()
    {
        _graphSampler!.Variables = new double[1]{ _graphSampler.Variables[0] + Time.deltaTime * 2 };
    }
}
