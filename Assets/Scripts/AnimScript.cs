using UnityEngine;
using System.Collections;

public class AnimScript : MonoBehaviour {

    public Sprite[] Sprites;
    public int index;
    public int cycleCount;
    public int CycleVal;
    public bool isPlaying;
    public float counter;
    public float duration;

    public SpriteRenderer SR;


	// Use this for initialization
	void Start () {
        SR = GetComponent<SpriteRenderer>();
        CycleVal = cycleCount;
	}
	
	// Update is called once per frame
	void Update () {
        if(isPlaying)
        {
            counter -= Time.deltaTime;
            if(counter<=0)
            {
                index++;
                if (index == Sprites.Length)
                    index = 0;
                SR.sprite = Sprites[index];
                counter = duration;
                cycleCount--;
                if(cycleCount==0)
                {
                    index = 0;
                    counter = 0;
                    cycleCount = CycleVal;
                    isPlaying = false;
                    SR.sprite = Sprites[0];
                }
            }

        }
	
	}
}
