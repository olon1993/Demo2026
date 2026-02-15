using UnityEngine;
using TheFrozenBanana;

namespace DialogueEngine
{
    public class AIConversant : MonoBehaviour, IInteractive<PlayerConversant>
    {
        [SerializeField] string speaker;
        [SerializeField] Dialogue dialogue = null;
        [SerializeField] Sprite aiPortrait;

        public void Interact(PlayerConversant conversant)
        {
            if (dialogue == null)
            {
                return;
            }

            conversant.StartDialogue(this, dialogue);
        }

        public string GetSpeaker()
        {
            return speaker;
        }

        public Sprite GetPortrait()
        {
            return aiPortrait;
        }
    }
}