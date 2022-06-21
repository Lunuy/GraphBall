#nullable enable
using UnityEngine;

namespace Assets.Scripts.Graph
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
        private GridderOptions _options = new() {
            MinX = 0,
            MaxX = 10,
            MinY = 0
        };

        private static readonly int TilingNameId = Shader.PropertyToID("_Tiling");
        private static readonly int OffsetNameId = Shader.PropertyToID("_Offset");
        
        private void UpdateGrid() {
            if(_spriteRenderer == null) return;

            var maxY = _options.MinY + (_options.MaxX - _options.MinX) * (transform.localScale.y / transform.localScale.x);
            
            var tiling = new Vector2((float)(_options.MaxX - _options.MinX), (float)(maxY - _options.MinY));
            var offset = new Vector2((float)-_options.MinX, (float)-_options.MinY);

            _spriteRenderer.sharedMaterial.SetVector(TilingNameId, tiling);
            _spriteRenderer.sharedMaterial.SetVector(OffsetNameId, offset);
        }
        
        // ReSharper disable once UnusedMember.Local
        private void Start()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _spriteRenderer!.sharedMaterial = new Material(_spriteRenderer.sharedMaterial);
            UpdateGrid();
        }
        
        // ReSharper disable once UnusedMember.Local
        private void Update()
        {
            UpdateGrid();
        }
    }
}
