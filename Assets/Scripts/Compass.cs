﻿using UnityEngine.UI;
using UnityEngine;
public class Compass : MonoBehaviour
{
	public RawImage CompassImage;
	public Transform Player;
	public Text CompassDirectionText;

    public void Start()
    {
        if(Player == null)//error prevention
        {
            Player = GameObject.FindGameObjectWithTag("Player").gameObject.transform;
            if(Player == null)//if STILL null/ Player tag cannot be found
            {
                Debug.LogError("ERROR!" + this.gameObject.name + ": No object in scene tagged 'Player'.");//log error
            }
        }
       
    }

	public void Update()
	{
		//Get a handle on the Image's uvRect
		CompassImage.uvRect = new Rect(Player.localEulerAngles.y / 360, 0, 1, 1);

		// Get a copy of your forward vector
		Vector3 forward = Player.transform.forward;

		// Zero out the y component of your forward vector to only get the direction in the X,Z plane
		forward.y = 0;

		//Clamp our angles to only 5 degree increments
		float headingAngle = Quaternion.LookRotation(forward).eulerAngles.y;
		headingAngle = 5 * (Mathf.RoundToInt(headingAngle / 5.0f));

		//Convert float to int for switch
		int displayangle = Mathf.RoundToInt(headingAngle);

		//Set the text of Compass Degree Text to the clamped value, but change it to the letter if it is a True direction
		switch (displayangle)
		{
		case 0:
			//Do this
			CompassDirectionText.text = "N";
			break;
		case 360:
			//Do this
			CompassDirectionText.text = "N";
			break;
		case 45:
			//Do this
			CompassDirectionText.text = "NE";
			break;
		case 90:
			//Do this
			CompassDirectionText.text = "E";
			break;
		case 135:
			//Do this
			CompassDirectionText.text = "SE";
			break;
		case 180:
			//Do this
			CompassDirectionText.text = "S";
			break;
		case 225:
			//Do this
			CompassDirectionText.text = "SW";
			break;
		case 270:
			//Do this
			CompassDirectionText.text = "W";
			break;
        case 315:
            //Do this
            CompassDirectionText.text = "NW";
            break;
        default:
			CompassDirectionText.text = headingAngle.ToString ();
			break;
		}
	}
}