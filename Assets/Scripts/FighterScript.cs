using UnityEngine;
using System.Collections;

public class FighterScript : MonoBehaviour {

    int Pindex;
    public int[] OuterIndex;
    public int[] InnerIndex;
    public int Atk;
    public bool hadTurn;

    private void OnMouseDown()
    {
        if (!GameControl.singleton.inFX && !hadTurn)
        {
            GameControl.singleton.SelectionOutline.transform.position = transform.position;
            if (GameControl.singleton.ActiveFrame != null)
                Destroy(GameControl.singleton.ActiveFrame);
            for(int i=0;i<4;i++)
            {
                GameControl.singleton.transform.GetChild(i).GetChild(0).GetComponent<SpriteRenderer>().sprite = GameControl.singleton.OuterOrbs[OuterIndex[i]];
                GameControl.singleton.transform.GetChild(i).GetChild(0).GetChild(0).GetComponent<SpriteRenderer>().sprite = GameControl.singleton.InnerOrbs[InnerIndex[i]];
                GameControl.singleton.transform.GetChild(i).GetChild(0).GetComponent<SpriteRenderer>().color = Color.white;
                GameControl.singleton.transform.GetChild(i).GetChild(0).GetChild(0).GetComponent<SpriteRenderer>().color = Color.white;
            }
            Vector2[] v = new Vector2[5];
            for (int i = 0; i < 5; i++)
                v[i] = GameControl.singleton.GetPenta(Pindex)[i];
            GameObject g = Instantiate(GameControl.singleton.Frame, new Vector2(-6.28f, -2.32f), Quaternion.identity) as GameObject;
            for (int i = 1; i < 5; i++)
            {
                (Instantiate(GameControl.singleton.Frame, new Vector2(-6.28f, -2.32f) + v[i], Quaternion.identity) as GameObject).transform.SetParent(g.transform);
            }
            GameControl.singleton.ActiveFrame = g;
            GameControl.singleton.ActiveFighter = this;
            g.transform.position += Vector3.back;
        }
    }

    public void AttackMatch()
    {
        foreach(FrameScript f in GameControl.singleton.ActiveFrame.GetComponentsInChildren<FrameScript>())
        {
            if (f.Orb != null)
            {
                int c = 0;
                for (int i = 0; i < 3; i++)
                {
                    if (f.Orb.OuterIndex == OuterIndex[i])
                    {
                        if (i == 0)
                            c++;
                        HandleAction(f.Orb, i, c);
                    }
                    if (f.Orb.InnerIndex == InnerIndex[i])
                    {
                        if (i == 0)
                            c++;
                        HandleAction(f.Orb, i, c);
                    }
                    if (!f.Orb.matched)
                        f.RemoveOldOrb();
                }
            }
        }
    }

    public void HandleAction(OrbScript orb, int i, int combo)
    {
        switch(i)
        {
            case 0:
                GameControl.singleton.AttackBoss(Atk+Atk*combo);
                orb.matched = true;
                break;
            case 1:
                GameControl.singleton.GetComponent<HPscript>().UpdateHP(-7);
                orb.matched = true;
                break;
            case 2:
                orb.OuterIndex = OuterIndex[3];
                orb.InnerIndex = InnerIndex[3];
                orb.SetSprites();
                break;
        }

    }

    // Use this for initialization
    void Start () {
        SetPindex();
	}

    public void SetPindex()
    {
        Pindex = GameControl.singleton.RNG.Next(16);
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
