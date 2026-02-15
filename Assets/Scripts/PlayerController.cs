using DialogueEngine;
using Inventory;
using System;
using System.Collections.Generic;
using TheFrozenBanana;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] Transform interactOrigin;
    [SerializeField] float interactRadius = 1;
    [SerializeField] private List<Item> _inventory = new List<Item>();
    private bool isOccupied = false;

    private IInputManager3d _inputManager;
    private PlayerConversant _playerConversant;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _inputManager = GetComponent<IInputManager3d>();
        if (_inputManager == null)
        {
            Debug.Log("No IInputManager found on object " + name);
        }

        _playerConversant = GetComponent<PlayerConversant>();
        if (_playerConversant == null)
        {
            Debug.Log("no PlayerConversand found on object " + name);
        }
        else
        {
            _playerConversant.onConversationEnded += Release;
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (isOccupied)
        {
            return;
        }

        if (_inputManager.IsAttack)
        {
            Interact();
        }
    }

    public void AddItem(string item)
    {
        Item itemEnum = (Item)Enum.Parse(typeof(Item), item);
        Inventory.Add(itemEnum);
    }

    private void Interact()
    {
        Collider[] colliders = Physics.OverlapSphere(interactOrigin.position, interactRadius);
        foreach (Collider collider in colliders)
        {
            AIConversant conversant = collider.gameObject.GetComponent<AIConversant>();
            if (conversant != null)
            {
                conversant.Interact(_playerConversant);
                isOccupied = true;
                break;
            }
        }
    }

    private void Release()
    {
        isOccupied = false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(interactOrigin.position, interactRadius);
    }

    public List<Item> Inventory 
    { 
        get { return _inventory; } 
        private set
        {
            _inventory = value;
        }
    }
}
