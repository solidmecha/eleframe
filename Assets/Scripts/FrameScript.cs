using UnityEngine;
using System.Collections;

public class FrameScript : MonoBehaviour {

    public OrbScript Orb;

    private void OnMouseDown()
    {
        GameControl.singleton.FrameOffset = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition) - (Vector2)transform.root.position;
        GameControl.singleton.MovingFrame = true;
        int x = Mathf.RoundToInt(((Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition) - GameControl.singleton.FrameOffset).x);
        int y = Mathf.RoundToInt(((Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition) - GameControl.singleton.FrameOffset).y);
        GameControl.singleton.XYSelect[0] = x;
        GameControl.singleton.XYSelect[1] = y;
        foreach (Collider2D c in transform.root.GetComponentsInChildren<Collider2D>())
            c.enabled = false;
        GameControl.singleton.SetSelectedOrbs();
    }

    public void UpdateOrb(Collider2D other)
    {
        if (other != null)
        {
            Orb = other.GetComponent<OrbScript>();
            Orb.GetComponent<SpriteRenderer>().color = Color.white;
            Orb.transform.GetChild(0).GetComponent<SpriteRenderer>().color = Color.white;
        }
        else
            Orb = null;
    }

    public void RemoveOldOrb()
    {
        if (Orb != null)
        {
            Orb.GetComponent<SpriteRenderer>().color = new Color(.5f, .5f, .5f, .75f);
            Orb.transform.GetChild(0).GetComponent<SpriteRenderer>().color = new Color(.5f, .5f, .5f, .75f);
        }
    }

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
