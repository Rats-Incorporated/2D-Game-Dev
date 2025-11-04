using UnityEngine;

public class win_block : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<BasicPlayerController>().can_win){
            Destroy(gameObject);
        }
        else
        {
            Debug.Log("Can't pass this yet...");
        }
    }
}