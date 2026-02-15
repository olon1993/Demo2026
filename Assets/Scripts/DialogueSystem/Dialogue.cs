using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace DialogueEngine
{

    [CreateAssetMenu(fileName = "New Dialogue", menuName = "Dialogue", order = 0)]
    public class Dialogue : ScriptableObject, ISerializationCallbackReceiver
    {
        [SerializeField] List<DialogueNode> Nodes = new List<DialogueNode>();
        Dictionary<string, DialogueNode> NodesDictionary = new Dictionary<string, DialogueNode>();
        [SerializeField] Vector2 newNodeOffset = new Vector2(250, 0);
        [SerializeField] Vector2 multiNodeOffset = new Vector2(50, 50);
        public IEnumerable<DialogueNode> GetAllNodes()
        {
            return Nodes;
        }

        public DialogueNode GetRootNode()
        {
            return Nodes[0];
        }

        public IEnumerable<DialogueNode> GetChildren(DialogueNode parentNode)
        {
            foreach (string Id in parentNode.ChildNodeIds)
            {
                if (NodesDictionary.ContainsKey(Id))
                {
                    yield return NodesDictionary[Id];
                }
            }
        }

        public IEnumerable<DialogueNode> GetPlayerChildren(DialogueNode parentNode)
        {
            foreach (DialogueNode node in GetChildren(parentNode))
            {
                if (node.IsPlayerSpeaking)
                {
                    yield return node;
                }
            }
        }

        public IEnumerable<DialogueNode> GetAIChildren(DialogueNode parentNode)
        {
            foreach (DialogueNode node in GetChildren(parentNode))
            {
                if (node.IsPlayerSpeaking == false)
                {
                    yield return node;
                }
            }
        }

#if UNITY_EDITOR

        private DialogueNode InitializeNode(DialogueNode parent)
        {
            DialogueNode node = CreateInstance<DialogueNode>();
            node.name = System.Guid.NewGuid().ToString();
            if (parent != null)
            {
                node.IsPlayerSpeaking = parent.IsPlayerSpeaking ? false : true;
                node.SetPosition(parent.Rectangle.position + newNodeOffset + (multiNodeOffset * parent.ChildNodeIds.Count));
                parent.ChildNodeIds.Add(node.name);
            }

            return node;
        }

        private void AddNode(DialogueNode nodeToAdd)
        {
            Nodes.Add(nodeToAdd);
            OnValidate();
        }
        public DialogueNode CreateNode(DialogueNode parent)
        {
            DialogueNode node = InitializeNode(parent);
            Undo.RegisterCreatedObjectUndo(node, "Created Dialogue Node.");
            Undo.RecordObject(this, "Add Dialogue Node");
            AddNode(node);
            return node;
        }

        public void DeleteNode(DialogueNode nodeToDelete)
        {
            Undo.RecordObject(this, "Deleting Dialogue Node");
            Nodes.Remove(nodeToDelete);
            OnValidate();
            CleanupNodeChildren(nodeToDelete);
            Undo.DestroyObjectImmediate(nodeToDelete);
        }

        public void CleanupNodeChildren(DialogueNode nodeToDelete)
        {
            foreach (DialogueNode node in GetAllNodes())
            {
                node.ChildNodeIds.Remove(nodeToDelete.name);
            }
        }

        private void Awake()
        {
            OnValidate();
        }
#endif

        private void OnValidate()
        {
            NodesDictionary.Clear();

            if (Nodes.Count == 0)
            {
                DialogueNode node = InitializeNode(null);
                AddNode(node);
            }

            foreach (DialogueNode node in Nodes)
            {
                NodesDictionary.Add(node.name, node);
            }
        }

        public void OnBeforeSerialize()
        {
#if UNITY_EDITOR
            if (Nodes.Count == 0)
            {
                DialogueNode newNode = InitializeNode(null);
                AddNode(newNode);
            }

            if (AssetDatabase.GetAssetPath(this) != "")
            {
                foreach (DialogueNode node in GetAllNodes())
                {
                    if (AssetDatabase.GetAssetPath(node) == "")
                    {
                        AssetDatabase.AddObjectToAsset(node, this);
                    }
                }
            }
#endif
        }

        public void OnAfterDeserialize()
        {
        }
    }
}
