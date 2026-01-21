using UnityEngine;

public class CharcterSwap : MonoBehaviour
{
    public MonoBehaviour PlayerMovement; //Gisgo movement
   // public MonoBehaviour ; //Bodo movement

   // private Icontrollable G;
    //private Icontrollable B;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
       // G = PlayerMovement as Icontrollable;
       // B = PlayerMovement as Icontrollable;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
            Swap();
    }

    void Swap()
    {
        
    }
}
