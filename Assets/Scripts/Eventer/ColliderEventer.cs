#nullable enable
using UnityEngine;

namespace Assets.Scripts.Eventer
{
    public delegate void Collider(Collider2D collision);

    public class TriggerEventer : MonoBehaviour
    {
        public Collider OnColliderEnter_ = delegate {  };

        public void OnTriggerEnter2D(Collider2D collider) {
            OnColliderEnter_(collider);
        }
    }
}