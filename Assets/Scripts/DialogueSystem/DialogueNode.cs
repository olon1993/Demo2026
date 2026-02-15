using Inventory;
using RPG.Stats;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace DialogueEngine
{

    public class DialogueNode : ScriptableObject
    {
        [SerializeField] private bool isPlayerSpeaking = false;
        [SerializeField] private string _text;
        [SerializeField] private List<string> _childNodeIds = new List<string>();
        [SerializeField] private Rect _rect = new Rect(0, 0, 200, 200);
        [SerializeField] string onEnterAction;
        [SerializeField] string onExitAction;
        [SerializeField] private Stat statRequired;
        [SerializeField] private int statRequirement;
        [SerializeField] private Item itemRequired;

        public string Text 
        {
            get { return _text; } 
            set
            {
                if (_text != value)
                {
                    Undo.RecordObject(this, "Update Dialogue");
                    _text = value;
                    EditorUtility.SetDirty(this);
                }
            } 
        }

        public List<string> ChildNodeIds 
        {
            get { return _childNodeIds; }
            set
            {
                Undo.RecordObject(this, "Unlink Dialogue");
                _childNodeIds = value;
                EditorUtility.SetDirty(this);
            }
        }

        public bool IsPlayerSpeaking
        {
            get { return isPlayerSpeaking; }
            set 
            {
                Undo.RecordObject(this, "Change Dialogue Speaker");
                isPlayerSpeaking = value;
                EditorUtility.SetDirty(this);
            }
        }

        public string GetOnEnterAction()
        {
            return onEnterAction;
        }

        public string GetOnExitAction()
        {
            return onExitAction;
        }

        public Item ItemRequired
        {
            get { return itemRequired; }
            set { itemRequired = value; }
        }

        public Stat StatRequired
        {
            get { return statRequired; }
            set { statRequired = value; }
        }

        public int StatRequirement
        {
            get { return statRequirement; }
            set { statRequirement = value; }
        }

#if UNITY_EDITOR
        public Rect Rectangle 
        {
            get { return _rect; }
            set
            {
                Undo.RecordObject(this, "Drag Dialogue");
                _rect = value;
                EditorUtility.SetDirty(this);
            }
        }

        public Rect SetPosition(Vector2 newPosition)
        {
            Undo.RecordObject(this, "Drag Dialogue");
            _rect.position = newPosition;
            EditorUtility.SetDirty(this);
            return _rect;
        }

        public void RemoveChild(string child)
        {
            Undo.RecordObject(this, "Remove child node");
            ChildNodeIds.Remove(child);
            EditorUtility.SetDirty(this);
        }

        public void AddChild(string child)
        {
            Undo.RecordObject(this, "Add child node");
            ChildNodeIds.Add(child);
            EditorUtility.SetDirty(this);
        }
#endif
    }
}

