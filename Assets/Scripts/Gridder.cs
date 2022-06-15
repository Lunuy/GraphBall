#nullable enable
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    public struct GridderOptions {
        public double MinX;
        public double MaxX;
        public double MinY;
    }

    [RequireComponent(typeof(SpriteRenderer))]
    public class Gridder : MonoBehaviour
    {
        public GridderOptions Options {
            get => _options;
            set {
                _options = value;
                UpdateGrid();
            }
        }

        private SpriteRenderer? _spriteRenderer;
        private GridderOptions _options = new GridderOptions {
            MinX = 0,
            MaxX = 10,
            MinY = 0
        };

        void UpdateGrid() {
            if(_spriteRenderer == null) return;

            double maxY = _options.MinY + (_options.MaxX - _options.MinX) * (transform.localScale.y / transform.localScale.x);
            
            Vector2 tiling = new Vector2((float)(_options.MaxX - _options.MinX), (float)(maxY - _options.MinY));
            Vector2 offset = new Vector2((float)_options.MinX, (float)_options.MinY);

            this._spriteRenderer.material.SetVector("_Tiling", tiling);
            this._spriteRenderer.material.SetVector("_Offset", offset);
        }

        // Start is called before the first frame update
        void Start()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            UpdateGrid();
        }

        // Update is called once per frame
        void Update()
        {
            UpdateGrid();
        }
    }
}