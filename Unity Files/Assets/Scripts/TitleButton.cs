using UnityEngine;

public class TitleButton : MonoBehaviour
{
    public LogicScript Logic;
    public
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
        if (collision.gameObject.CompareTag("Player"))
        {
            Destroy(gameObject);
            Logic.TempMessage("Door Unlocked!", 2f);

        }
        else
        {
            Logic.TempMessage("Can't go through here yet...", 2f);
        }
    }
}
