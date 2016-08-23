using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameManager : MonoBehaviour {

    Material lineMat;
    Object arrowPref;
    GameObject arrow;
    
    static GameManager _instance;
    static public GameManager Instance
    {
        get
        {
            return _instance;
        }
    }
    void Awake()
    {
        _instance = this;
        lineMat=Resources.Load<Material>("Material/trailMaterial");
        arrowPref = Resources.Load("Prefabs/Arrow");
    }
    void Start()
    {
        drawCircle(Color.grey, 10);
        drawCircle(Color.yellow, 5);
        drawCircle(Color.white, 4);
        drawCircle(Color.red, 3);
    }
    RaycastHit2D hit1, hit2, hit, lastHit;
    public void quit()
    {
        Application.Quit();
    }
    public void replay()
    {
        SceneManager.LoadScene(0);
    }
    void Update()
    {
        if (EventSystem.current.IsPointerOverGameObject()) return;//UI overlap
            Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        Vector3 arrowPoint = mouseRay.GetPoint(1);
        arrowPoint.Set(arrowPoint.x, arrowPoint.y, 0);
        //mouse hover the planet
        hit = Physics2D.GetRayIntersection(mouseRay, 100, 1 << LayerMask.NameToLayer("Planet"));
        if (hit.collider != null)
        {
            hit.collider.GetComponent<Planet>().hover.SetActive(true);
            lastHit = hit;
            arrowPoint = hit.transform.position;
        }
        else if (lastHit.collider != null)
        {
            lastHit.collider.GetComponent<Planet>().hover.SetActive(false);
        }
        //drag to move ships
        if (Input.GetMouseButtonDown(0))
        {
            hit1 = Physics2D.GetRayIntersection(mouseRay, 100, 1 << LayerMask.NameToLayer("Planet"));
            if (hit1.collider != null)
            {
                arrow = Instantiate(arrowPref, hit1.transform.position, Quaternion.identity) as GameObject;
                UIManager.Instance.clickThePlanet(hit.collider.GetComponent<Planet>());
            }
            else
            {
                UIManager.Instance.resetPanel();
            }
        }
        //drag the arrow
        if (arrow != null)
        {
            arrow.transform.position = arrowPoint;
            Vector3 direct = arrowPoint - hit1.transform.position;
            arrow.transform.up = direct;
            arrow.GetComponent<LineRenderer>().SetPosition(1, -Vector3.up* direct.magnitude);
        }
        //unclick to recover and let ships move
        if (hit1.collider != null && Input.GetMouseButtonUp(0))
        {
            Destroy(arrow);
            hit2 = Physics2D.GetRayIntersection(mouseRay, 100, 1 << LayerMask.NameToLayer("Planet"));
            if (hit2.collider != null)
            {
                Planet planet = hit1.collider.GetComponent<Planet>();
                moveTo(1, hit1.transform, hit.transform);
                hit1 = hit2 = new RaycastHit2D();
            }
        }
    }
    public void moveTo(int id,Transform from,Transform to)
    {
        if (from == to) return;
        Planet p = from.GetComponent<Planet>();
        for (int i=0;i<p.ships.Count;)
        {
            Ship ship = p.ships[i].GetComponent<Ship>();
            if (ship.groupID != id)
            {
                i++;
                continue;
            }
            ship.centerPlanet = hit2.transform;
            ship.newPlanet();
            p.ships.RemoveAt(i);
        }
    }
    public void drawCircle(Color c,float r,float centerX=0,float centerY=0)
    {
        float theta_scale = 0.1f;             //Set lower to add more points
        int size = Mathf.CeilToInt((2.0f * Mathf.PI) / theta_scale); //Total number of points in circle.

        GameObject newG = new GameObject();
        LineRenderer lineRenderer = newG.AddComponent<LineRenderer>();
        lineRenderer.material = lineMat;
        lineRenderer.SetColors(c, c);
        lineRenderer.SetWidth(0.04f, 0.04f);
        lineRenderer.SetVertexCount(size+1);

        int i = 0;
        for (float theta = 0; theta <= 2 * Mathf.PI + theta_scale; theta += 0.1f)
        {
            float x = r * Mathf.Cos(theta)+ centerX;
            float y = r * Mathf.Sin(theta)+ centerY;

            Vector3 pos = new Vector3(x, y, 0);
            lineRenderer.SetPosition(i, pos);
            i += 1;
        }
    }
    public void switchLineMat()
    {
        if (lineMat.color.a != 0)
            lineMat.color = new Color(1, 1, 1, 0);
        else
            lineMat.color = new Color(1, 1, 1, 0.2f);
    }
}
