using UnityEngine;

public class DestroyScript : MonoBehaviour
{
    bool isDestroyed;
    TriggerScript trigger;
    GameObject player;
    [SerializeField] Material OffMaterial;
    [SerializeField] Material OnMaterial;
    [SerializeField] Material DefaultMaterial;
    float timer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        trigger = GetComponentInChildren<TriggerScript>();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        //Debug.Log(timer);
        if (trigger.inTrigger == true)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                timer = 0;
            }
            if (isDestroyed == false)
            {
                Destroy();
            }

            if (Input.GetKey(KeyCode.E))
            {
                if (timer > 2 && timer < 3)
                {
                    gameObject.GetComponent<MeshRenderer>().material = OnMaterial;
                }
                else
                {
                    gameObject.GetComponent<MeshRenderer>().material = OffMaterial;
                }
            }
        }


    }

    void Destroy()
    {
        if (Input.GetKeyUp(KeyCode.E))
        {
            if (timer > 2 && timer < 3)
            {

                isDestroyed = true;
                gameObject.SetActive(false);
            }
            else
            {
                gameObject.GetComponent<MeshRenderer>().material = DefaultMaterial;
            }
        }
    }
}
