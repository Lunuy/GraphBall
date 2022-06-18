#nullable enable
using UnityEngine;

namespace Assets.Scripts
{
    public struct GridderOptions {
        public double MinX;
        public double MaxX;
        public double MinY;
    }

    [ExecuteInEditMode]
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

        private void UpdateGrid() {
            if(_spriteRenderer == null) return;

            var maxY = _options.MinY + (_options.MaxX - _options.MinX) * (transform.localScale.y / transform.localScale.x);
            
            var tiling = new Vector2((float)(_options.MaxX - _options.MinX), (float)(maxY - _options.MinY));
            var offset = new Vector2((float)_options.MinX, (float)_options.MinY);

            _spriteRenderer.material.SetVector("_Tiling", tiling);
            _spriteRenderer.material.SetVector("_Offset", offset);
        }
        
        // ReSharper disable once UnusedMember.Local
        private void Start()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            UpdateGrid();
        }
        
        // ReSharper disable once UnusedMember.Local
        private void Update()
        {
            UpdateGrid();
        }
    }
}
