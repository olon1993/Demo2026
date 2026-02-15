using Inventory;
using UnityEngine;
using UnityEngine.Events;

namespace DialogueEngine
{

    public class DialogueTrigger : MonoBehaviour
    {
        [SerializeField] public string triggerAction;
        [SerializeField] UnityEvent onTrigger;

        public void Trigger(string actionToTrigger)
        {
            if (actionToTrigger == triggerAction)
            {
                onTrigger.Invoke();
            }
        }
    }
}
