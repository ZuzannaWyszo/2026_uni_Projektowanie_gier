using UnityEngine;


public class button_kor : MonoBehaviour
{
    public GameObject korytarz;
    public void resetKorytarz()
    {
        Debug.Log("Click!");
        if (korytarz != null)
        {
            korytarz.TryGetComponent<korytarzInstantiate>(out korytarzInstantiate kor);
            if (kor)
            {

                kor.stworz_korytwarz();
                //kor.destroy_korytarz();
            }
            else
            {
                Debug.Log("no core");
            }
           

           
        }
    }
}
