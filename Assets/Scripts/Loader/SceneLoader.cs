#nullable enable
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.Loader
{
    // ReSharper disable once UnusedMember.Global
    public class SceneLoader : MonoBehaviour
    {
        public UnityEvent<IReadOnlyDictionary<string, List<Object>>> OnBeforeLoad = null!;
        public List<Object> SceneData = null!;
        private readonly List<GameObject> _rootObjects = new();

        // ReSharper disable once UnusedMember.Local
        private void Awake()
        {
            if (!GlobalSceneLoader.Initialized)
            {
                StartCoroutine(Initialize());
            }
            else
            {
                OnBeforeLoad.Invoke(GlobalSceneLoader.SceneData);
                GlobalSceneLoader.RegisterSceneData(gameObject.scene.name, SceneData);
            }
        }

        // ReSharper disable once UnusedMember.Local
        private void OnDestroy()
        {
            OnBeforeLoad.RemoveAllListeners();
            SceneData.Clear();
            GlobalSceneLoader.UnRegisterSceneData(gameObject.scene.name);
        }

        private IEnumerator Initialize()
        {
            gameObject.scene.GetRootGameObjects(_rootObjects);
            SetGameObjectsActive(_rootObjects, false);
            
            yield return GlobalSceneLoader.InitializeLoader(gameObject.scene.name, () =>
            {
                SetGameObjectsActive(_rootObjects, true);
            });
            _rootObjects.Clear();

            OnBeforeLoad.Invoke(GlobalSceneLoader.SceneData);
            GlobalSceneLoader.RegisterSceneData(gameObject.scene.name, SceneData);
        }

        private void SetGameObjectsActive(IReadOnlyList<GameObject> objects, bool value)
        {
            // ReSharper disable once ForCanBeConvertedToForeach
            for (var i = 0; i < objects.Count; i++)
            {
                var item = objects[i];
                if (item == gameObject) continue;

                item.SetActive(value);
            }
        }
    }
}
