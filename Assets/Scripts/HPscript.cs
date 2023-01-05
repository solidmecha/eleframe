using UnityEngine;
using System.Collections;

public class HPscript : MonoBehaviour {

    public int[] HP;
    public Transform BarRoot;
    float startScale;

    public void UpdateHP(int Dmg)
    {
        HP[0] -= Dmg;
        if (HP[0] > HP[1])
            HP[0] = HP[1];
        else if (HP[0] < 0)
            HP[0] = 0;
        BarRoot.localScale = new Vector2((float)HP[0] * startScale / (float)HP[1], BarRoot.localScale.y);
        BarRoot.GetChild(0).GetComponent<SpriteRenderer>().color = Color.Lerp(Color.red, Color.green, HP[0]*1f/HP[1]*1f);
    }


	// Use this for initialization
	void Start () {
        startScale = BarRoot.localScale.x;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
