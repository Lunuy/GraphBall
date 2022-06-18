#nullable enable
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

namespace Assets.Scripts.Loader
{
    [RequireComponent(typeof(LoaderUi))]
    public class GlobalSceneLoader : MonoBehaviour
    {
        public static bool Initialized => _instance != null;
        
        public static IReadOnlyDictionary<string, List<Object>> SceneData
        {
            get
            {
                if (_instance == null)
                {
                    throw new Exception("GlobalSceneLoader is not initialized");
                }

                return _instance._sceneData;
            }
        }

        private const string LoaderSceneName = "Loader";

        private static GlobalSceneLoader? _instance;

        private string _currentLoadedLeafScene = LoaderSceneName;
        private LoaderUi _loaderUi = null!;
        private Dictionary<string, List<Object>> _sceneData = null!;

        // ReSharper disable once UnusedMember.Local
        private void Awake()
        {
            if (_instance != null)
            {
                throw new Exception("GlobalSceneLoader already exists");
            }

            _instance = this;
            _loaderUi = GetComponent<LoaderUi>();
            _sceneData = new Dictionary<string, List<Object>>();
        }

        public static IEnumerator InitializeLoader(string? callerScene = null, Action? activateSceneAction = null)
        {
            yield return SceneManager.LoadSceneAsync(LoaderSceneName, LoadSceneMode.Additive);
            var globalSceneLoader = _instance!;
            globalSceneLoader._currentLoadedLeafScene = LoaderSceneName;

            if (callerScene == null) yield break;

            globalSceneLoader._loaderUi.Subtitle = "";
            globalSceneLoader._loaderUi.ShowLoadingScreen();
            
            var queryResult = DependencySolver.Query(globalSceneLoader._currentLoadedLeafScene, callerScene);

            while (queryResult.UnloadScenes.Count > 0)
            {
                var unloadSceneName = queryResult.UnloadScenes.Pop()!;
                yield return UnloadSingleScene(globalSceneLoader, unloadSceneName);
            }

            while (queryResult.LoadScenes.Count > 1)
            {
                var loadSceneName = queryResult.LoadScenes.Pop()!;
                yield return LoadSingleScene(globalSceneLoader, loadSceneName);
            }

            activateSceneAction?.Invoke();
            globalSceneLoader._loaderUi.HideLoadingScreen();

            if (queryResult.LoadScenes.Count == 0) yield break;
            globalSceneLoader._currentLoadedLeafScene = queryResult.LoadScenes.Pop()!;
        }

        private static IEnumerator LoadSingleScene(GlobalSceneLoader globalSceneLoader, string sceneName)
        {
            globalSceneLoader._loaderUi.Subtitle = $"Loading {sceneName}";
            yield return SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        }

        private static IEnumerator UnloadSingleScene(GlobalSceneLoader globalSceneLoader, string sceneName)
        {
            globalSceneLoader._loaderUi.Subtitle = $"Unloading {sceneName}";
            yield return SceneManager.UnloadSceneAsync(sceneName);
        }

        public static IEnumerator LoadScene(string sceneName)
        {
            if (_instance == null)
            {
                throw new Exception("GlobalSceneLoader is not initialized");
            }

            _instance._loaderUi.Subtitle = "";
            _instance._loaderUi.ShowLoadingScreen();

            var queryResult = DependencySolver.Query(_instance._currentLoadedLeafScene, sceneName);

            while (queryResult.UnloadScenes.Count > 0)
            {
                var unloadSceneName = queryResult.UnloadScenes.Pop()!;
                yield return UnloadSingleScene(_instance, unloadSceneName);
            }

            while (queryResult.LoadScenes.Count > 0)
            {
                var loadSceneName = queryResult.LoadScenes.Pop()!;
                yield return LoadSingleScene(_instance, loadSceneName);
            }

            _instance._loaderUi.HideLoadingScreen();
        }

        public static void RegisterSceneData(string sceneName, List<Object> sceneData)
        {
            if (_instance == null)
            {
                throw new Exception("GlobalSceneLoader is not initialized");
            }

            _instance._sceneData.Add(sceneName, sceneData);
        }

        public static void UnRegisterSceneData(string sceneName)
        {
            if (_instance == null) return;

            if (_instance._sceneData.ContainsKey(sceneName))
            {
                _instance._sceneData.Remove(sceneName);
            }
        }
    }
}
