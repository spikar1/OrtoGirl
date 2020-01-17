using UnityEngine;
using System.Collections;

public class Crate : InteractableBlock
{

    public Orientation orientation = Orientation.down;
    //public GridScript grid;
    public bool seated = false;
    public bool invalid;

    public override void Start()
    {
        /*if (Physics.Raycast(transform.position, orientation.ToVector(), 1f))
            grid.MakeWalkable(grid.NodeFromWorldPoint(transform.position), true);
        else*/
            StartCoroutine(MoveToNextNode(orientation));
        
    }

    public override void Update()
    {
        if (seated == true)
        {
            return;
        }

        /*RaycastHit hit;

        if (Physics.Raycast(transform.position, orientation.ToVector(), out hit, .5f + .2f))
        {
            seated = true;
            transform.position = grid.NodeFromWorldPoint(hit.point + orientation.Inverse().ToVector()*.5f).worldPosition;
            grid.MakeWalkable(grid.NodeFromWorldPoint(transform.position), true);

        }
        else
        {
            grid.MakeWalkable(grid.NodeFromWorldPoint(transform.position), false);
            transform.position += orientation.ToVector()*2f * Time.deltaTime;
        }*/

    }

    public override void Activate()
    {
        base.Activate();
        if (grid.NodeFromWorldPoint(transform.position).walkable[(int)orientation])
        {

        }
    }

    IEnumerator MoveToNextNode(Orientation direction) {
        StartMove();
        for (int i = 0; i < 20; i++) {
            transform.position += direction.ToVector() * 0.05f;
            yield return new WaitForEndOfFrame();
        }
        
        if (!grid.NodeFromWorldPoint(transform.position).walkable[(int)direction]) {
            StartCoroutine(MoveToNextNode(orientation));
        }
        else {
            Landed();
        }
    }
    
    public void Push(Orientation direction) {
        if(!Physics.Raycast(transform.position, direction.ToVector(),1))
            StartCoroutine(MoveToNextNode(direction));
    }


    void StartMove() {
        //grid.MakeWalkable(grid.NodeFromWorldPoint(transform.position), false);
        for (int i = 0; i < GridScript.OrientationVectors.Length; i++) {
            grid.MakeWalkable(grid.NodeFromWorldPoint(transform.position + GridScript.OrientationVectors[i] + Vector3.up), false);
            
        }
    }

    void Landed() {
        for (int i = 0; i < GridScript.OrientationVectors.Length; i++) {
            grid.MakeWalkable(grid.NodeFromWorldPoint(transform.position + GridScript.OrientationVectors[i] + Vector3.up), true);
        }
        grid.MakeWalkable(grid.NodeFromWorldPoint(transform.position), true);

    }
}
