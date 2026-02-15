using UnityEngine;
using UnityEngine.UI;
using DialogueEngine;
using TMPro;
using RPG.Stats;
using Inventory;

namespace RPG.UI
{
    public class DialogueUI : MonoBehaviour
    {
        GameObject player;
        PlayerConversant playerConversant;
        BaseStats playerStats;
        PlayerController playerController; // Used for checking inventory for required item in dialogues
        [SerializeField] TextMeshProUGUI AIText;
        [SerializeField] Button nextButton;
        [SerializeField] GameObject AIResponse;
        [SerializeField] Transform choiceRoot;
        [SerializeField] GameObject choiceButtonPrefab;
        [SerializeField] Button quitButton;
        [SerializeField] TextMeshProUGUI speaker;
        [SerializeField] Image portrait;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            player = GameObject.FindGameObjectWithTag("Player");
            playerConversant = player.GetComponent<PlayerConversant>();
            playerStats = player.GetComponent<BaseStats>();
            playerController = player.GetComponent<PlayerController>();

            playerConversant.onConversationUpdated += UpdateUI;
            nextButton.onClick.AddListener(() => playerConversant.Next()); ;
            quitButton.onClick.AddListener(() => { playerConversant.Quit(); });

            UpdateUI();
        }

        // Update is called once per frame
        void UpdateUI()
        {
            gameObject.SetActive(playerConversant.IsActive());
            portrait.gameObject.SetActive(playerConversant.IsActive());

            if (playerConversant.IsActive() == false)
            {
                return;
            }

            speaker.text = playerConversant.GetSpeaker();
            portrait.sprite = playerConversant.GetPortrait();


            AIResponse.SetActive(playerConversant.IsChoosing() == false);
            choiceRoot.gameObject.SetActive(playerConversant.IsChoosing());

            if (playerConversant.IsChoosing())
            {
                BuildChoiceList();
            }
            else
            {
                AIText.text = playerConversant.GetText();
                nextButton.gameObject.SetActive(playerConversant.HasNext());
            }
        }

        private void BuildChoiceList()
        {
            foreach (Transform child in choiceRoot)
            {
                Destroy(child.gameObject);
            }

            foreach (DialogueNode choice in playerConversant.GetChoices())
            {
                // Check if the choice meets the stat requirement
                if (choice.StatRequired != Stat.None)
                {
                    if(playerStats.GetStat(choice.StatRequired) < choice.StatRequirement)
                    {
                        continue;
                    }
                }

                // Check if the choice meets the item requirement
                if (choice.ItemRequired != Item.None)
                {
                    if (playerController.Inventory.Contains(choice.ItemRequired) == false)
                    {
                        continue;
                    }
                }

                // if requirement is met draw the node
                GameObject choiceButtonInstance = Instantiate(choiceButtonPrefab, choiceRoot);
                var textComp = choiceButtonInstance.GetComponentInChildren<TextMeshProUGUI>();
                textComp.text = choice.Text;
                Button button = choiceButtonInstance.GetComponentInChildren<Button>();
                button.onClick.AddListener(() => 
                {
                    playerConversant.SelectChoice(choice);
                });
            }
        }
    }
}

