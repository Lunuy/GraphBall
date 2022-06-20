#nullable enable
using UnityEngine;

namespace Assets.Scripts.Eventer
{
    public delegate void Collision(Collision2D collision);

    public class CollisionEventer : MonoBehaviour
    {
        public Collision OnCollisionEnter_ = delegate {  };

        public void OnCollisionEnter2D(Collision2D collision) {
            OnCollisionEnter_(collision);
        }
    }
}