using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerNodeInteract : MonoBehaviour
{

    public bool canPress;
    public bool turning;

    GameObject currentTurningObject;

    // Update is called once per frame
    void Update()
    {

        if (canPress)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                turning = !turning;
                GetComponent<PlayerController>().controlLock = turning;
            }

            if (turning)
            {
                currentTurningObject.GetComponent<Turnable>().BeingTurned(Input.GetAxis("Horizontal"));
            }
        }
    }




    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Turnable>() != null)
        {
            canPress = true;
            currentTurningObject = collision.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<Turnable>() != null)
        {
            canPress = false;
            currentTurningObject = null;
        }
    }

}
