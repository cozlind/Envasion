using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIManager : MonoBehaviour
{

    static UIManager _instance;
    static public UIManager Instance
    {
        get
        {
            return _instance;
        }
    }
    void Awake()
    {
        _instance = this;
        resetPanel();
    }
    public GameObject BuildPanel;
    public GameObject PlanetPanel;
    Planet planet;
    void Update()
    {
        if (PlanetPanel.activeSelf)
        {
            int playerShipNum=0, enemyShipNum=50;
            playerShipNum = planet.getShipNum(1,out enemyShipNum);
            PlanetPanel.transform.FindChild("NumText").GetComponent<Text>().text = "飞船：" + playerShipNum + "/50\n敌人：" + enemyShipNum;
        }
    }
    public void resetPanel()
    {
        BuildPanel.SetActive(false);
        PlanetPanel.SetActive(false);
    }
    public void clickThePlanet(Planet p)
    {
        resetPanel();
        planet = p;
        //groupID 0：unexplored planet 1:player's planet 2~N:enemy's planet 
        //ships >= 10 to build
        if ((planet.groupID == 0&&planet.getShipNum(1)>=5)|| planet.groupID == 1)
        {
            BuildPanel.SetActive(true);
        }
        PlanetPanel.SetActive(true);
        PlanetPanel.transform.FindChild("Vitality").GetComponent<Image>().fillAmount = planet.vitality / 1000f;
        PlanetPanel.transform.FindChild("Agility").GetComponent<Image>().fillAmount = planet.agility / 1000f;
        PlanetPanel.transform.FindChild("Strength").GetComponent<Image>().fillAmount = planet.strength / 1000f;
    }

    public void buildSpaceLadder()
    {
        planet.buildSpaceLadder(1);
    }
    public void levelUpSpaceLadder()
    {
        planet.levelUpSpaceLadder();
    }
    public void generateShield()
    {
        planet.generateShield();
    }
    public void buildLaserSatelite()
    {
        planet.buildLaserSatelite();
    }
    public void buildSpySatelite()
    {
        planet.buildSpySatelite();
    }
}
