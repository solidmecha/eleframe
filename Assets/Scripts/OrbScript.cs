using UnityEngine;
using System.Collections;

public class OrbScript : MonoBehaviour {

    public int OuterIndex;
    public int InnerIndex;
    public bool matched;
    public int X;
    public int Y;

	// Use this for initialization
	void Start () {
        OuterIndex = GameControl.singleton.RNG.Next(4);
        InnerIndex = GameControl.singleton.RNG.Next(4);
        SetSprites();
    }

    public void SetSprites()
    {
        GetComponent<SpriteRenderer>().sprite = GameControl.singleton.OuterOrbs[OuterIndex];
        transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = GameControl.singleton.InnerOrbs[InnerIndex];
    }

    public void Invert()
    {
        int i = InnerIndex;
        InnerIndex = OuterIndex;
        OuterIndex = i;
        SetSprites();
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
