using UnityEngine;
using System.Collections.Generic;

public class GameControl : MonoBehaviour {

    public static GameControl singleton;
    public System.Random RNG;
    public Sprite[] OuterOrbs;
    public Sprite[] InnerOrbs;
    public GameObject Orb;
    public GameObject[] Ele;
    public List<int> BossOrder;
    public GameObject currentBoss;
    public GameObject Frame;
    public GameObject ActiveFrame;
    public FighterScript ActiveFighter;
    public List<FighterScript> Fighters;
    public int[] XYSelect;
    public bool inFX;
    public GameObject bossHP;
    public GameObject playerHP;
    public GameObject SelectionOutline;
    public GameObject GOScreen;
    public int Turns;
    List<Vector2[]> Pentas = new List<Vector2[]>
    {
        new Vector2[5]{new Vector2(0,0), new Vector2(1, 0), new Vector2(2, 0), new Vector2(3, 0), new Vector2(4, 0)},
        new Vector2[5]{new Vector2(0,0), new Vector2(1, 0), new Vector2(2, 0), new Vector2(3, 0), new Vector2(3, 1)},
        new Vector2[5]{new Vector2(0,0), new Vector2(1, 0), new Vector2(2, 0), new Vector2(1, 1), new Vector2(2, 1)},
        new Vector2[5]{new Vector2(0,0), new Vector2(1, 0), new Vector2(2, 0), new Vector2(0, 1), new Vector2(0, 2)},
        new Vector2[5]{new Vector2(0,0), new Vector2(1, 0), new Vector2(2, 0), new Vector2(0, 1), new Vector2(2, 1)},
        new Vector2[5]{new Vector2(0,0), new Vector2(1, 0), new Vector2(0, 1), new Vector2(1, 1), new Vector2(2, 0)},
        new Vector2[5]{new Vector2(0,0), new Vector2(0, 1), new Vector2(1, 0), new Vector2(2, 0), new Vector2(3, 0)},
        new Vector2[5]{new Vector2(0,0), new Vector2(1, 0), new Vector2(1, 1), new Vector2(1, 2), new Vector2(2, 2)},
        new Vector2[5]{new Vector2(0,0), new Vector2(0, 1), new Vector2(1, 1), new Vector2(2, 1), new Vector2(2, 2)},
        new Vector2[5]{new Vector2(0,0), new Vector2(1, 0), new Vector2(1, 1), new Vector2(2, 1), new Vector2(3, 1)},
        new Vector2[5]{new Vector2(0,0), new Vector2(0, 1), new Vector2(-1, 1), new Vector2(1, 1), new Vector2(0, 2)},
        new Vector2[5]{new Vector2(0,0), new Vector2(1, 0), new Vector2(2, 0), new Vector2(3, 0), new Vector2(1, 1)},
        new Vector2[5]{new Vector2(0,0), new Vector2(1, 0), new Vector2(2, 0), new Vector2(3, 0), new Vector2(2, 1)},
        new Vector2[5]{new Vector2(0,0), new Vector2(1, 0), new Vector2(2, 0), new Vector2(1, 1), new Vector2(1, 2)},
        new Vector2[5]{new Vector2(0,0), new Vector2(0, 1), new Vector2(1, 1), new Vector2(2, 1), new Vector2(1, 2)},
        new Vector2[5]{new Vector2(0,0), new Vector2(1, 0), new Vector2(2,0), new Vector2(2, 1), new Vector2(3, 1)}
    };

    public bool[][] BoardHasOrb;

    public bool MovingFrame;
    public Vector2 FrameOffset;


    private void Awake()
    {
        singleton = this;
        RNG = new System.Random();
        BoardHasOrb = new bool[6][];
    }

    // Use this for initialization
    void Start () {
        PlaceOrbs();
        PlacePlayers();
        SpawnBoss();
	}

    public Vector2[] GetPenta(int i)
    {
        return Pentas[i];
    }

    public void PlaceOrbs()
    {
        for(int x=0;x<6;x++)
        {
            BoardHasOrb[x] = new bool[5];
            for(int y=0;y<5;y++)
            {
                BoardHasOrb[x][y] = true;
                OrbScript o=(Instantiate(Orb, (Vector2)transform.position + new Vector2(x, y), Quaternion.identity) as GameObject).GetComponent<OrbScript>();
                o.X = x;
                o.Y = y;

            }
        }
    }

    public void RefreshOrbs()
    {
        for (int x = 0; x < 6; x++)
        {
            for (int y = 0; y < 5; y++)
            {
                BoardHasOrb[x][y] = true;
                RaycastHit2D hit = Physics2D.Raycast((Vector2)transform.position + new Vector2(x, y), Vector2.zero);
                if (hit.collider == null)
                {
                    OrbScript o = (Instantiate(Orb, (Vector2)transform.position + new Vector2(x, y), Quaternion.identity) as GameObject).GetComponent<OrbScript>();
                    o.X = x;
                    o.Y = y;
                }

            }
        }
    }

    public void PlacePlayers()
    {
        BossOrder = new List<int> { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14 };
        for(int i=0;i<4;i++)
        {
            List<int> nums = new List<int> { 0, 1, 2, 3 };
            int r = RNG.Next(BossOrder.Count);
            GameObject g = Instantiate(Ele[BossOrder[r]], transform.GetChild(i).transform.position, Quaternion.identity) as GameObject;
            g.AddComponent<BoxCollider2D>();
            g.transform.localScale = new Vector2(.25f, .25f);
            BossOrder.RemoveAt(r);
            FighterScript fs=g.AddComponent<FighterScript>();
            fs.Atk = 200;
            fs.OuterIndex = new int[4];
            fs.InnerIndex = new int[4];
            for (int j = 0; j < 4; j++)
            {
                r = RNG.Next(nums.Count);
                fs.InnerIndex[j] = nums[r];
                nums.RemoveAt(r);
            }
            nums.Add(0);
            nums.Add(1);
            nums.Add(2);
            nums.Add(3);
            for (int j = 0; j < 4; j++)
            {
                r = RNG.Next(nums.Count);
                fs.OuterIndex[j] = nums[r];
                nums.RemoveAt(r);
            }
            Fighters.Add(fs);

        }
    }

    public void PlaceInBounds()
    {
        foreach (Transform t in ActiveFrame.GetComponentsInChildren<Transform>())
        {
            int x = Mathf.RoundToInt((t.position - transform.position).x);
            int y = Mathf.RoundToInt((t.position - transform.position).y);
            if(x<0)
            {
                ActiveFrame.transform.position= (Vector2)ActiveFrame.transform.position + Vector2.right;
                XYSelect[0]++;
            }
            else if(x>5)
            {
                ActiveFrame.transform.position = (Vector2)ActiveFrame.transform.position + Vector2.left;
                XYSelect[0]--;
            }
            if(y<0)
            {
                ActiveFrame.transform.position = (Vector2)ActiveFrame.transform.position + Vector2.up;
                XYSelect[1]++;
            }
            else if(y>4)
            {
                ActiveFrame.transform.position = (Vector2)ActiveFrame.transform.position + Vector2.down;
                XYSelect[0]--;
            }
        }
    }

    public void SetSelectedOrbs()
    {
        foreach (FrameScript f in ActiveFrame.GetComponentsInChildren<FrameScript>())
        {
            f.RemoveOldOrb();
        }
            foreach (Transform t in ActiveFrame.GetComponentsInChildren<Transform>())
        {
            int x = Mathf.RoundToInt((t.position - transform.position).x);
            int y = Mathf.RoundToInt((t.position - transform.position).y);
            RaycastHit2D hit = Physics2D.Raycast((Vector2)transform.position+new Vector2(x, y), Vector2.zero);
            t.GetComponent<FrameScript>().UpdateOrb(hit.collider);
        }
    }

    public void  SpawnBoss()
    {
        if (BossOrder.Count != 0)
        {
            int r = RNG.Next(BossOrder.Count);
            currentBoss = Instantiate(Ele[BossOrder[r]]) as GameObject;
            currentBoss.AddComponent<HPscript>().HP = new int[2] { 1000*(12-BossOrder.Count), 1000*(12-BossOrder.Count) };
            bossHP.transform.localScale = new Vector2(9.2f, 1);
            bossHP.transform.GetChild(0).GetComponent<SpriteRenderer>().color = Color.green;
            currentBoss.GetComponent<HPscript>().BarRoot = bossHP.transform;
            currentBoss.GetComponent<SpriteRenderer>().sortingOrder = -1;
            BossOrder.RemoveAt(r);
        }
        else
        {
            GOScreen = Instantiate(GOScreen) as GameObject;
            GOScreen.transform.GetChild(0).GetChild(0).GetComponent<UnityEngine.UI.Text>().text = "Victory in " + Turns.ToString() + " Rounds";
        }
    }

    public void AttackBoss(int Dmg)
    {
        currentBoss.GetComponent<HPscript>().UpdateHP(Dmg);
    }

    public void CleanupOrbs()
    {
        foreach (FrameScript f in ActiveFrame.GetComponentsInChildren<FrameScript>())
        {
            if (f.Orb != null && f.Orb.matched)
            {
                Destroy(f.Orb.gameObject);
                BoardHasOrb[f.Orb.X][f.Orb.Y] = false;
            }
        }
        if (currentBoss.GetComponent<HPscript>().HP[0] == 0)
        {
            Destroy(currentBoss);
            SpawnBoss();
        }
    }

    public void CheckTurnEnd()
    {
        SelectionOutline.transform.position = new Vector3(100, 100, 100);
        int c = 0;
        foreach(FighterScript f in Fighters)
        {
            if (f.hadTurn)
                c++;
        }
        if (c == 4)
        {
            inFX = true;
            Invoke("EndTurn", .84f);
        }
    }

    public void EndTurn()
    {
        Turns++;
        DropOrbs();
        currentBoss.GetComponent<AnimScript>().isPlaying = true;
        GetComponent<HPscript>().UpdateHP(RNG.Next(25, 76));
        Invoke("StartTurn", .84f);
    }

    public void DropOrbs()
    {
        for(int x=0;x<6;x++)
        {
            for(int y=4;y>0;y--)
            {
                RaycastHit2D hit = Physics2D.Raycast((Vector2)transform.position + new Vector2(x, y), Vector2.zero);
                if (hit.collider != null)
                {
                    for (int i = y - 1; i >= 0; i--)
                    {
                        if (!BoardHasOrb[x][i])
                            hit.collider.gameObject.AddComponent<DropScript>();
                    }
                }
            }
        }
    }

    public void StartTurn()
    {
        if (GetComponent<HPscript>().HP[0] == 0)
        {
            Instantiate(GOScreen);
        }
        else
        {
            foreach (FighterScript f in Fighters)
            {
                f.hadTurn = false;
            }
            RefreshOrbs();
            inFX = false;
        }
    }
	
	// Update is called once per frame
	void Update () {
        if(MovingFrame)
        {
            if (Input.GetMouseButtonUp(0))
            {
                ActiveFighter.AttackMatch();
                ActiveFighter.GetComponent<AnimScript>().isPlaying = true;
                CleanupOrbs();
                MovingFrame = false;
                Destroy(ActiveFrame);
                ActiveFighter.hadTurn = true;
                CheckTurnEnd();
            }
            else
            {
                int x = Mathf.RoundToInt(((Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition) - FrameOffset - (Vector2)transform.position).x);
                if (x < 0)
                    x = 0;
                else if (x > 5)
                    x = 5;
                int y = Mathf.RoundToInt(((Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition) - FrameOffset - (Vector2)transform.position).y);
                if (y < 0)
                    y = 0;
                else if (y > 4)
                    y = 4;
                ActiveFrame.transform.position = (Vector2)transform.position + new Vector2(x, y);
                PlaceInBounds();
                if (XYSelect[0] != x || XYSelect[1] != y)
                {
                    XYSelect[0] = x;
                    XYSelect[1] = y;
                    SetSelectedOrbs();
                }
                if (Input.GetMouseButtonDown(1))
                {
                    ActiveFrame.transform.Rotate(new Vector3(0, 0, 90));
                    ActiveFrame.transform.position = (Vector2)transform.position + new Vector2(x, y);
                    PlaceInBounds();
                    SetSelectedOrbs();
                }
            }
        }
        if (Input.GetKeyDown(KeyCode.R))
            Application.LoadLevel(0);
	
	}
}
