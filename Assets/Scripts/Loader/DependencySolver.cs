#nullable enable
using System;
using System.Collections.Generic;
// ReSharper disable ForCanBeConvertedToForeach

namespace Assets.Scripts.Loader
{
    public static class DependencySolver
    {
        private static readonly DependencyNode DependencyTree =
            new DependencyNode("Loader")
                .WithChild(new DependencyNode("GlobalSound")
                    .WithChild(new DependencyNode("GameMenu"))
                    .WithChild(new DependencyNode("GameUI")
                        .WithChild(new DependencyNode("MainScene"))
                        .WithChild(new DependencyNode("Level0"))
                        .WithChild(new DependencyNode("Level1"))
                        .WithChild(new DependencyNode("Level2"))
                        .WithChild(new DependencyNode("Level3"))
                        .WithChild(new DependencyNode("Level4"))
                        .WithChild(new DependencyNode("Level5"))
                        .WithChild(new DependencyNode("Level6"))
                        .WithChild(new DependencyNode("Level7"))
                        .WithChild(new DependencyNode("Level8"))
                        .WithChild(new DependencyNode("Level9"))
                    )
                );
        
        internal class DependencyNode
        {
            public int Depth;
            public readonly string SceneName;
            public DependencyNode? Parent;
            public List<DependencyNode> Children;

            public DependencyNode(string sceneName)
            {
                SceneName = sceneName;
                Children = new List<DependencyNode>();
            }

            public DependencyNode WithChild(DependencyNode child)
            {
                child.Parent = this;
                Children.Add(child);

                return this;
            }
        }

        private static readonly Dictionary<string, DependencyNode> SceneNameToNode;

        static DependencySolver()
        {
            static void BuildDepth(DependencyNode node, int depth)
            {
                node.Depth = depth;

                for (var i = 0; i < node.Children.Count; ++i)
                {
                    BuildDepth(node.Children[i], depth + 1);
                }
            }
            BuildDepth(DependencyTree, 0);

            SceneNameToNode = new Dictionary<string, DependencyNode>();
            
            static void BuildSceneNameToNode(DependencyNode node)
            {
                if (SceneNameToNode.ContainsKey(node.SceneName))
                {
                    throw new Exception($"Scene name {node.SceneName} is already in the dictionary");
                }
                SceneNameToNode.Add(node.SceneName, node);

                for (var i = 0; i < node.Children.Count; ++i) BuildSceneNameToNode(node.Children[i]);
            }
            BuildSceneNameToNode(DependencyTree);
        }

        public readonly struct QueryResult
        {
            public readonly Stack<string> UnloadScenes;
            public readonly Stack<string> LoadScenes;

            public QueryResult(Stack<string> unloadScenes, Stack<string> loadScenes)
            {
                UnloadScenes = unloadScenes;
                LoadScenes = loadScenes;
            }
        }

        public static QueryResult Query(string currentScene, string targetScene)
        {
            if (!SceneNameToNode.TryGetValue(currentScene, out var currentNode))
            {
                throw new Exception($"Scene {currentScene} is not in the dependency tree");
            }

            if (!SceneNameToNode.TryGetValue(targetScene, out var targetNode))
            {
                throw new Exception($"Scene {targetScene} is not in the dependency tree");
            }

            var unloadScenes = new Stack<string>();
            var loadScenes = new Stack<string>();

            var currentNodeDepth = currentNode.Depth;
            var targetNodeDepth = targetNode.Depth;

            while (currentNodeDepth != targetNodeDepth)
            {
                if (currentNodeDepth > targetNodeDepth)
                {
                    unloadScenes.Push(currentNode.SceneName);
                    currentNode = currentNode.Parent!;
                    currentNodeDepth -= 1;
                }
                else
                {
                    loadScenes.Push(targetNode.SceneName);
                    targetNode = targetNode.Parent!;
                    targetNodeDepth -= 1;
                }
            }

            while (currentNode != targetNode)
            {
                unloadScenes.Push(currentNode.SceneName);
                currentNode = currentNode.Parent!;
                loadScenes.Push(targetNode.SceneName);
                targetNode = targetNode.Parent!;
            }

            var reversedUnloadScenes = new Stack<string>(unloadScenes.Count);
            while (unloadScenes.TryPop(out var sceneName)) reversedUnloadScenes.Push(sceneName);

            return new QueryResult(reversedUnloadScenes, loadScenes);
        }
    }
}
