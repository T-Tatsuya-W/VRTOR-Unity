using UnityEngine;
using UnityEngine.InputSystem;

public class ColourRed : MonoBehaviour
{
    private Keyboard keyboard;
    void Awake()
    {
        keyboard = Keyboard.current;
    }
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (keyboard != null && keyboard.spaceKey.wasPressedThisFrame)
        {
            // Debug.Log("Space key was pressed.");
            
            GetComponent<Renderer>().material.color = Color.red;
        }
        if (keyboard != null && keyboard.spaceKey.wasReleasedThisFrame)
        {
            // Debug.Log("Space key was released.");
            
            GetComponent<Renderer>().material.color = Color.white;
        }
    }
}
