#nullable enable
using UnityEngine;

namespace Assets.Scripts.Eventer
{
    public delegate void Collider(Collider2D collision);

    public class TriggerEventer : MonoBehaviour
    {
        public Collider OnTriggerEnter = delegate { };

        // ReSharper disable once UnusedMember.Local
        private void OnTriggerEnter2D(Collider2D col)
        {
            OnTriggerEnter(col);
        }
    }
}
