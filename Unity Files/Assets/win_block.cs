using UnityEngine;

public class win_block : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public LogicScript Logic;
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
            Logic.TempMessage("Door Unlocked!", 2f);
        }
        else
        {
            Logic.TempMessage("Can't go through here yet...", 2f);
        }
    }
}