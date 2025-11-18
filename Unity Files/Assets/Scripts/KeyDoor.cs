using UnityEngine;

public class win_block : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public LogicScript Logic;
    public GameObject key;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<PlayerController>().can_win)
        {
            Destroy(gameObject);
            Logic.TempMessage("Door Unlocked!", 2f);
            key.SetActive(false);

        }
        else
        {
            Logic.TempMessage("Can't go through here yet...", 2f);
        }
    }
}