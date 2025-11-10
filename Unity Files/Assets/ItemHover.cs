using UnityEngine;


public class ItemHover : MonoBehaviour
{

    Vector3 itemPosition;

    public float animDist = 0.5f;
    public float animDur = 3.0f; // 1 full cycle up and down
    float halfAnimDur;
    float curTime;
    float curTimeHalf;
    float curMult;
    float curTimePercent;
    float curY;
    float offset; // random starting position

    void Start()
    {
        itemPosition = transform.position;
        halfAnimDur = animDur / 2.0f;
        offset = Random.Range(0, animDur); 
    }

    void Update()
    {

        curTime = (Time.time+offset) % animDur;
        curTimeHalf = (Time.time + offset) % halfAnimDur; 
        curTimePercent = (curTimeHalf % halfAnimDur) / halfAnimDur;




        curY = (curTimePercent * curTimePercent) * (3 - 2 * curTimePercent); //smoothstep cubic ease-in-out

        if (curTime < halfAnimDur) // moving up
        {
            transform.position = new Vector3(itemPosition.x, itemPosition.y - (animDist / 2) + animDist * curY, itemPosition.z);
        }else // moving down
        {
            transform.position = new Vector3(itemPosition.x, itemPosition.y - (animDist/2) + animDist - animDist * curY, itemPosition.z);
        }



    }
}
