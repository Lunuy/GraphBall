using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Utility
{
    internal static class YieldInstructionCache
    {
        public static readonly WaitForEndOfFrame WaitForEndOfFrame = new();
        public static readonly WaitForFixedUpdate WaitForFixedUpdate = new();

        private static readonly Dictionary<float, WaitForSeconds> TimeInterval = new(new FloatComparer());

        public static WaitForSeconds WaitForSeconds(float seconds)
        {
            if (!TimeInterval.TryGetValue(seconds, out var waitForSeconds))
                TimeInterval.Add(seconds, waitForSeconds = new WaitForSeconds(seconds));
            return waitForSeconds;
        }

        private class FloatComparer : IEqualityComparer<float>
        {
            // ReSharper disable once CompareOfFloatsByEqualityOperator
            bool IEqualityComparer<float>.Equals(float x, float y) => x == y;

            int IEqualityComparer<float>.GetHashCode(float obj) => obj.GetHashCode();
        }
    }
}
