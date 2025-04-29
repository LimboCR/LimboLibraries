using UnityEngine;

namespace Limbo.Utils.MonoBe
{
    public static class MonoBehaviourUtils
    {
        #region World/Local Positioning Structs
        /// <summary>
        /// Structs build to provide World positioning for GameObjects. Use new World(Vector3 Position, Quaternion Rotation).
        /// </summary>
        public struct World
        {
            public Vector3 Position;
            public Quaternion Rotation;
            public World(Vector3? position = null, Quaternion? rotation = null)
            {
                Position = position ?? default;
                Rotation = rotation ?? default;
            }
            public static World Default() => new(Vector3.zero, Quaternion.identity);
        }

        /// <summary>
        /// Structs build to provide Local positioning for GameObjects. Use new Local(Vector3 Position, Quaternion Rotation).
        /// </summary>
        public struct Local
        {
            public Vector3 Position;
            public Quaternion Rotation;
            public Local(Vector3? position = null, Quaternion? rotation = null)
            {
                Position = position ?? default;
                Rotation = rotation ?? default;
            }
            public static Local Default() => new(Vector3.zero, Quaternion.identity);
        }
        #endregion

        #region Instantiate Empty GameObject
        /*
         * Allows easier way of instantiationg empty GameObjects and assigning them the right way.
         * Returns instantiated GameObject.
         */

        /// <summary>
        /// Safely Instantiates new empty gameobject without unnecessary duplications, returnig refernce to self.
        /// </summary>
        /// <param name="objectName">Name of the object</param>
        /// <param name="world">World positioning</param>
        public static GameObject InstantiateEmpty(string objectName, World world)
        {
            GameObject current = new GameObject(objectName); // Set name of the object
            current.transform.position = world.Position;     // Set position
            current.transform.rotation = world.Rotation;     // Reset rotation

            return current;
        }

        /// <summary>
        /// Safely Instantiates new empty gameobject without unnecessary duplications, returnig refernce to self.
        /// </summary>
        /// <param name="objectName">Name of the object</param>
        /// <param name="world">World positioning</param>
        /// <param name="parrent">Parent GameObject</param>
        /// <returns></returns>
        public static GameObject InstantiateEmpty(string objectName, World world, Transform parrent)
        {
            GameObject current = new GameObject(objectName); // Set name of the object
            current.transform.SetParent(parrent);            // Set parent directly
            current.transform.position = world.Position;     // Set position
            current.transform.rotation = world.Rotation;     // Reset rotation

            return current;
        }

        /// <summary>
        /// Safely Instantiates new empty gameobject without unnecessary duplications, returnig refernce to self.
        /// </summary>
        /// <param name="objectName">Name of the object</param>
        /// <param name="local">Local positioning positioning</param>
        /// <param name="parrent">Parent GameObject</param>
        /// <returns></returns>
        public static GameObject InstantiateEmpty(string objectName, Local local, Transform parrent)
        {
            GameObject current = new GameObject(objectName);  // Set name of the object
            current.transform.SetParent(parrent);             // Set parent directly
            current.transform.localPosition = local.Position; // Set position
            current.transform.localRotation = local.Rotation; // Set rotation

            return current;
        }
        #endregion
    }
}

