using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour
{
    // Start is called before the first frame update

    public bool isInRange;
    public KeyCode interactKey;
    public KeyCode escapeKey;
    public UnityEvent interactAction;
    
    void Start()
    {

    }

   
    void Update()
    {
        if (isInRange)
        {
            if (Input.GetKeyDown(interactKey))
            {
                interactAction.Invoke();
                PlayerMovement.isPlayer = false;
                SnapPlayerToPosition();
                Cursor.isPlayer = true;
            }
            if (Input.GetKeyDown(escapeKey))
            {
                PlayerMovement.isPlayer = true;
                Cursor.isPlayer = false;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isInRange = false;
        }
    }

    private void SnapPlayerToPosition()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        player.transform.position = new Vector2(transform.position.x, player.transform.position.y);
    }
}
