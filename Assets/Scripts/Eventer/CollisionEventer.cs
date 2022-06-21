#nullable enable
using UnityEngine;

namespace Assets.Scripts.Eventer
{
    public delegate void Collision(Collision2D collision);

    public class CollisionEventer : MonoBehaviour
    {
        public event Collision OnCollisionEnter = delegate {  };

        public void OnCollisionEnter2D(Collision2D collision) {
            OnCollisionEnter(collision);
        }
    }
}
