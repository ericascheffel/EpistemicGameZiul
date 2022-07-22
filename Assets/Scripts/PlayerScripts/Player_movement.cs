using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_movement : MonoBehaviour
{
    public int speed = 5; // player speed
    public FixedJoystick moveJoystick; // Joystick class
    private Vector2 myMoveVector; // vecor x and y for current movement direction


    private void Update()
    {
        float x_move = moveJoystick.Horizontal; // Gets a value between -1 and 1 for joystick horizontal movement 
        float y_move = moveJoystick.Vertical; // Gets a value between -1 and 1 for joystick vertical movement 

        myMoveVector = new Vector2(x_move, y_move); // movemet direction using joystick (1 for positive and -1 for negative)

        if (Mathf.Abs(myMoveVector[0]) >= 0.5f || Mathf.Abs(myMoveVector[1]) >= 0.5f) // Checks if joystick positions (x and y) is greater then 0.5f
        {
            if (!DialogMannager.GetInstance().playingDialog) { // Checks if a dialog is not being played
                transform.Translate(speed * myMoveVector * Time.deltaTime); // move player's trasnform acording to myMoveVector
            }
        }
        
    }
}
