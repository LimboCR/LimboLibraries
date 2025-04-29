using System.Collections.Generic;
using UnityEngine;

namespace Limbo.Utils
{
    public static class LoopUtils
    {
        /*
         * Instead of constatly writing TryGetCompenent inside new foreaches use this, to include getting component logic inside foreach.
         * Getting something out of script if it exist will be included later
         */

        public static IEnumerable<T> Component<T>(this IEnumerable<GameObject> gameObjects) where T : Component
        {
            foreach (var go in gameObjects)
            {
                if (go.TryGetComponent<T>(out var comp))
                    yield return comp;
            }
        }

        public static IEnumerable<(GameObject go, T component)> ComponentGO<T>(this IEnumerable<GameObject> gameObjects) where T : Component
        {
            foreach (var go in gameObjects)
            {
                if (go.TryGetComponent<T>(out var comp))
                    yield return (go, comp);
            }
        }
    }
}

