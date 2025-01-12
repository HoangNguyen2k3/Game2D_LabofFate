using UnityEngine;

public class MinimapWorldObject : MonoBehaviour
{
    [SerializeField]
    private bool followObject = false;
    [SerializeField]
    private Sprite minimapIcon;
    public Sprite MinimapIcon => minimapIcon;
    private bool isRegisterMinimapWorld = false;
    [SerializeField] private GameObject minimap1;
    [SerializeField] private GameObject minimap2;
    [SerializeField] private GameObject minimap3;
    private bool change_first = false;
    private bool change_last = false;
    private void Start()
    {
/*        if (!isRegisterMinimapWorld && MinimapController.Instance)
        {
            MinimapController.Instance.RegisterMinimapWorldObject(this);
            isRegisterMinimapWorld = true;
        }*/
    }
    private void Update()
    {
        if (!isRegisterMinimapWorld && MinimapController.Instance)
        {
          MinimapController.Instance.RegisterMinimapWorldObject(this);
           // minimap1.GetComponent<MinimapController>().RegisterMinimapWorldObject(this);
/*            minimap2.GetComponent<MinimapController>().RegisterMinimapWorldObject (this);
            minimap3.GetComponent<MinimapController>().RegisterMinimapWorldObject(this); */  
            isRegisterMinimapWorld = true;
        }
  /*      if (manager_level.gameObject == null)
        {
            manager_level = FindFirstObjectByType<ManagerLevelGame>().gameObject;
        }
        else
        {
            if (!change_first && MinimapController.Instance && isRegisterMinimapWorld && manager_level.GetComponent<ManagerLevelGame>().current_map == 2)
            {
                MinimapController.Instance.RegisterMinimapWorldObject(this);
                change_first = true;
            }
            if (!change_last && MinimapController.Instance && isRegisterMinimapWorld && manager_level.GetComponent<ManagerLevelGame>().current_map == 3)
            {
                MinimapController.Instance.RegisterMinimapWorldObject(this);
                change_last = true;
            }
        }*/

    }
    private void OnDestroy()
    {
        if (MinimapController.Instance)
        {
            MinimapController.Instance.RemoveMinimapWorldObject(this);
        }
        
    }
}
