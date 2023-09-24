using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float movementSpeed = 6f;
    public GameObject playerOne;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (TiltFive.Input.TryGetStickTilt(out Vector2 joystick, TiltFive.ControllerIndex.Right, TiltFive.PlayerIndex.One))
        {
            playerOne.transform.Translate(joystick.x * Time.deltaTime * movementSpeed, 0.0f, joystick.y * Time.deltaTime * movementSpeed);   
        }
    }
    
}
