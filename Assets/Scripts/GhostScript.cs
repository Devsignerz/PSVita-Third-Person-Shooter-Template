using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class GhostScript : MonoBehaviour
{
    public Transform myEyes;
    public GameObject ghostLeft;
    public GameObject ghostRight;
    public GhostDirection direction;
    public float speed = .1f;
    Vector3 nextRotation = Vector3.zero;
    Vector3 nextDirecton;
    Vector3 nextPosition;

    // Start is called before the first frame update
    void Start()
    {
        Color color = ColorChooser();
        GetComponent<Renderer>().material.SetColor("_Color", color);
        ghostLeft.GetComponent<Renderer>().material.SetColor("_Color", color);
        ghostRight.GetComponent<Renderer>().material.SetColor("_Color", color);

        nextPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position == nextPosition)
        {
            nextDirecton = direction.FindNextDirection();
            nextPosition = direction.MoveInDirection(nextDirecton);
        }


        if (transform.position == nextPosition)

        if (transform.position.x <= -30)
        {
            transform.position += new Vector3(54f, 0f, 0f);
            nextPosition.x += 54f;
        }

        if (transform.position.x >= 30)
        {
            transform.position += new Vector3(-54f, 0f, 0f);
            nextPosition.x += -54f;
        }

        if (nextDirecton == Vector3.forward)
            nextRotation.y = 0f;
        if (nextDirecton == Vector3.left)
            nextRotation.y = -90f;
        if (nextDirecton == Vector3.back)
            nextRotation.y = 180f;
        if (nextDirecton == Vector3.right)
            nextRotation.y = 90f;
    }

    private void FixedUpdate()
    {
        transform.position = Vector3.Lerp(transform.position, nextPosition, speed);
        myEyes.localRotation = Quaternion.Lerp(myEyes.localRotation,
                             Quaternion.Euler(transform.rotation.x, nextRotation.y, transform.rotation.z),
                             speed);
    }

    Color ColorChooser()
    {
        if (direction.targetFinder.stateSwitcher.state == GhostState.Frigntened)
            return Color.blue;

        switch (direction.targetFinder.gtype)
        {
            case GhostType.Oikake:
                return Color.red;

            case GhostType.Machibuse:
                return new Color(1f, .5f, 1f);

            case GhostType.Kimagure:
                return Color.cyan;

            case GhostType.Otoboke:
                return new Color(1f, .5f, 0f);

            default:
                return Color.white;
        }

    }

}