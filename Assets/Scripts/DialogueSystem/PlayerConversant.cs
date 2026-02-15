using RPG.Stats;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TheFrozenBanana;
using UnityEngine;
using UnityEngine.UI;

namespace DialogueEngine
{

    public class PlayerConversant : MonoBehaviour
    {
        private GameObject player;
        private IInputManager _inputManager;
        private BaseStats _playerStats;
        private PlayerController _playerController;

        [SerializeField] string playerName;
        [SerializeField] Sprite playerPortrait;
        AIConversant currentConversant = null;
        Dialogue currentDialogue;
        DialogueNode currentNode = null;
        bool isChoosing = false;

        public event Action onConversationUpdated;
        public event Action onConversationEnded;

        private void Start()
        {
            _inputManager = GetComponent<IInputManager>();
            if (_inputManager == null)
            {
                Debug.Log("No IInputManager found on " + gameObject.name);
            }

            player = GameObject.FindGameObjectWithTag("Player");

            _playerController = player.GetComponent<PlayerController>();
            if (_playerController == null)
            {
                Debug.Log("No PlayerController found on " + gameObject.name);
            }

            _playerStats = GameObject.FindGameObjectWithTag("Player").GetComponent<BaseStats>();
            if (_playerStats == null)
            {
                Debug.Log("No BaseStats found on " + gameObject.name);
            }

        }

        public void StartDialogue(AIConversant conversant, Dialogue dialogue)
        {
            currentConversant = conversant;
            currentDialogue = dialogue;
            currentNode = currentDialogue.GetRootNode();
            TriggerEnterAction();
            onConversationUpdated();
        }

        public void Quit()
        {
            TriggerExitAction();
            currentConversant = null;
            currentDialogue = null;
            currentNode = null;
            isChoosing = false;
            onConversationUpdated();
            onConversationEnded();
        }

        public string GetSpeaker()
        {
            if (isChoosing)
            {
                return playerName;
            }
            else
            {
                return currentConversant.GetSpeaker();
            }
        }

        public Sprite GetPortrait()
        {
            if (isChoosing)
            {
                return playerPortrait;
            }
            else
            {
                return currentConversant.GetPortrait();
            }
        }

        public bool IsActive()
        {
            return currentDialogue != null;
        }

        public bool IsChoosing()
        {
            return isChoosing;
        }

        public string GetText()
        {
            if (currentNode == null)
            {
                return "CurrentDialogue is NULL";
            }

            return currentNode.Text;
        }

        public IEnumerable<DialogueNode> GetChoices()
        {
            return currentDialogue.GetPlayerChildren(currentNode);
        }

        public void SelectChoice(DialogueNode chosenNode)
        {
            // Do some skill check
            // React to skill check
            currentNode = chosenNode;
            TriggerEnterAction();
            isChoosing = false;
            Next();
        }

        public void Next()
        {
            int numPlayerResponses = currentDialogue.GetPlayerChildren(currentNode).Count();
            if (numPlayerResponses > 0)
            {
                isChoosing = true;
                TriggerExitAction();
                onConversationUpdated();
                return;
            }

            DialogueNode[] children = currentDialogue.GetAIChildren(currentNode).ToArray();

            if(children.Count() > 0)
            {
                TriggerExitAction();
                CalculateCurrentNode(children);
                TriggerEnterAction();
                onConversationUpdated();
            }
            else
            {
                Quit();
            }
        }

        private void CalculateCurrentNode(DialogueNode[] children)
        {
            foreach(DialogueNode child in children)
            {
                if (ItemCheck(child) == false)
                {
                    Debug.Log("Node failed item check.");
                    continue;
                }

                if(SkillCheck(child) == false)
                {
                    Debug.Log("Node failed skill check");
                    continue;
                }

                currentNode = child;
                return;
            }
        }

        private bool ItemCheck(DialogueNode child)
        {
            if (child.ItemRequired == Inventory.Item.None)
            {
                Debug.Log("Node has no item requirement.");
                return true;
            }

            if (_playerController.Inventory.Contains(child.ItemRequired))
            {
                Debug.Log("Node passed item check.");
                return true;
            }

            return false;
        }

        // Triggered when a player node has multiple children.
        // Assign the new current node based on the stat requirement of the child nodes.
        // Returns sets the current node to the current child if 
        private bool SkillCheck(DialogueNode child)
        {
            // assign the currentNode if the child has no stat requirement
            if (child.StatRequired == RPG.Stats.Stat.None)
            {
                Debug.Log("Node has no skill check.");
                return true;
            }

            // Skill Check
            int skillcheck = UnityEngine.Random.Range(1, 20) + _playerStats.GetStat(child.StatRequired);
            Debug.Log("Skill Check: " + skillcheck + " / " + child.StatRequirement);
            if (skillcheck >= child.StatRequirement)
            {
                Debug.Log("Node passed skill check.");
                return true;
            }

            return false;
        }

        public bool HasNext()
        {
            return currentDialogue.GetChildren(currentNode).Count() > 0;
        }

        private void TriggerEnterAction()
        {
            if (currentNode == null)
            {
                return;
            }

            if (string.IsNullOrEmpty(currentNode.GetOnEnterAction()))
            {
                return;
            }

            DialogueTrigger[] dialogueTriggers = currentConversant.GetComponents<DialogueTrigger>();
            foreach(DialogueTrigger trigger in dialogueTriggers)
            {
                trigger.Trigger(currentNode.GetOnEnterAction());
            }
        }

        private void TriggerExitAction()
        {
            if (currentNode == null)
            {
                return;
            }

            if (string.IsNullOrEmpty(currentNode.GetOnExitAction()))
            {
                return;
            }

            DialogueTrigger[] dialogueTriggers = currentConversant.GetComponents<DialogueTrigger>();
            foreach (DialogueTrigger trigger in dialogueTriggers)
            {
                trigger.Trigger(currentNode.GetOnExitAction());
            }
        }

    }

}
