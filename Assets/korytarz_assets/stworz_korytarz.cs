using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using static korytarzInstantiate;

public class korytarzInstantiate : MonoBehaviour
{
    public GameObject k_bez_drzwi;
    public GameObject k_z_drzwi;
    public GameObject pokoj;

    public GameObject p1;
    public GameObject p2;
    public GameObject p3;

    public int liczba_pomiedzy;
    public int liczba_drzwi;
    public Vector3 Poz_start;
    private bool pierwsze;
    public List<_pokoj> data_pokoje = new List<_pokoj>();
    private List<GameObject> instancje = new List<GameObject>();

    System.Random rnd = new System.Random();

    void Start()
    {
        data_pokoje.Add(new _pokoj(p1, -1));
        data_pokoje.Add(new _pokoj(p2, 2));
        data_pokoje.Add(new _pokoj(p3, 2));
        stworz_korytwarz();



    }
    public class _pokoj
    {
        public GameObject p;
        public int ilosc; //ilosc pokoi, -1 jezeli dowolna ilosc 
        public List<int> lista_p = new List<int>(); //lista pokoi przyzanczonych
        public _pokoj(GameObject p, int ilosc)
        {
            this.p = p;
            this.ilosc = ilosc;
        }
    }


    // Update is called once per frame
    void Update()
    {
      
    }

    public void stworz_korytwarz()
    {
        destroy_korytarz();
        int n_pokoi = liczba_drzwi * 2;
        _pokoj[] kol_pokoi = new _pokoj[n_pokoi]; //lista kolejnosci pokoi
        List<int> pom = new List<int>(); //lista indeksow pokoi nieprzydzielonych

        if (sprawdz_dane(n_pokoi, data_pokoje))
        {
            przyznacz_pokoje_all(pom, data_pokoje, n_pokoi, kol_pokoi);
            generuj_korytarz(kol_pokoi);
        }
    }

    void generuj_korytarz(_pokoj[] kol_pokoi)
    {
        Debug.Log($"Tworzenie korytarza z {k_z_drzwi} drzwiami oraz {k_bez_drzwi} segmentami pomiedzy." +
            $"\n Te wartosci zmienia sie w inspektorze.");
        if (k_bez_drzwi && k_z_drzwi && pokoj && p1 && p2 && p3)
        {
            //pobieranie wielkosci segemntow na podstawie box collider wewnatrz prefabu
            //instantianizacja i destrukcja po pobiorze wielkosci elementu
            GameObject pom0 = Instantiate(k_z_drzwi, new Vector3(0, 0, 0), Quaternion.identity);
            GameObject pom1 = Instantiate(k_bez_drzwi, new Vector3(0, 0, 0), Quaternion.identity);
            Vector3 size_drzwi = (pom0.GetComponent<Collider>().bounds.size);
            Vector3 size_kor = (pom1.GetComponent<Collider>().bounds.size);
            Destroy(pom0);
            Destroy(pom1);

            //instancjonowanie korytarza 
            float odleglosc = 1;
            int indx_p = 0; //index pokoi
            for (var j = 0; j < liczba_drzwi; j++)
            {
                //segmenty pomiedzy drzwiami
                for (var i = 0; i < liczba_pomiedzy; i++)
                {

                    if (!pierwsze) { odleglosc += size_kor.z / 2; }
                    else pierwsze = false;

                    GameObject b = Instantiate(k_bez_drzwi, new Vector3(Poz_start.x, Poz_start.y, odleglosc), Quaternion.identity);
                    odleglosc += size_kor.z / 2;
                    instancje.Add(b);
                }

                odleglosc += size_drzwi.z / 2;

                //segment drzwi
                GameObject a = Instantiate(k_z_drzwi, new Vector3(Poz_start.x, Poz_start.y, odleglosc), Quaternion.identity);
                instancje.Add(a);
                //pokoj po prawo
                GameObject i_pokoj = Instantiate(kol_pokoi[indx_p].p, new Vector3(Poz_start.x, Poz_start.y, odleglosc), Quaternion.identity);
                Vector3 size_pokoj = i_pokoj.GetComponent<Collider>().bounds.size;
                i_pokoj.transform.Rotate(0, 90, 0);
                i_pokoj.transform.position = i_pokoj.transform.position + new Vector3((size_pokoj.z / 2) + (size_drzwi.x / 2), 0, 0);
                indx_p++;
                instancje.Add(i_pokoj);
                //pokoj po lewo
                i_pokoj = Instantiate(kol_pokoi[indx_p].p, new Vector3(Poz_start.x, Poz_start.y, odleglosc), Quaternion.identity);
                size_pokoj = i_pokoj.GetComponent<Collider>().bounds.size;
                i_pokoj.transform.Rotate(0, -90, 0);
                i_pokoj.transform.position = i_pokoj.transform.position + new Vector3(-(size_pokoj.z / 2) - (size_drzwi.x / 2), 0, 0);
                indx_p++;
                instancje.Add(i_pokoj);

                odleglosc += size_drzwi.z / 2;
            }
            for (var i = 0; i < liczba_pomiedzy; i++)
            {
                if (!pierwsze) { odleglosc += size_kor.z / 2; }
                else pierwsze = false;

                GameObject b = Instantiate(k_bez_drzwi, new Vector3(Poz_start.x, Poz_start.y, odleglosc), Quaternion.identity);
                odleglosc += size_kor.z / 2;
                instancje.Add(b);
            }
        }
        else
        {
            Debug.Log("Nie ma przydzielonego prefabu ktoregos korytarza w inspektorze.");
        }
    }

    void przyznacz_pokoje_all(List<int> pom, List<_pokoj> data_pokoje,int n_pokoi, _pokoj[] kol_pokoi)
    {
        for (int i = 0; i < n_pokoi; i++)
        {
            pom.Add(i);
            kol_pokoi[i] = null;
        }
        List<_pokoj> p_rand = new List<_pokoj>();
        foreach (var p in data_pokoje)
        {
            if (p.ilosc >= 0)
            {
                przyznacz_pokoj(p, pom, n_pokoi);
            }
            else
            {
                p_rand.Add(p);
            }
        }
        przyznacz_losowo(p_rand, pom, n_pokoi);

        foreach (var p in data_pokoje)
        {
            foreach (int i in p.lista_p)
            {
                kol_pokoi[i] = p;
                //Debug.Log($"{p.ilosc} rozne pokoje, {i}");
            }
        }
    }

    void przyznacz_pokoj(_pokoj _p, List<int> wolne, int n)
    {
        int num;
        int j1 = 0;
        for (int i = 0; i <_p.ilosc; i++)
        {
            do
            {
                num = rnd.Next(n);
                j1++;
                if (j1 > 99) { Debug.Log("force break, blad"); break; }
            }
            while (!wolne.Contains(num) && wolne.Count>0);
            wolne.Remove(num);
            _p.lista_p.Add(num);
        }
    }
    void przyznacz_losowo(List<_pokoj> l_pokoje, List<int> wolne, int liczba_p)
    {
        int i;
        foreach (int w in wolne)
        {
            i = (rnd.Next(l_pokoje.Count));
            l_pokoje[i].lista_p.Add(w);
           
        }
    }
    bool sprawdz_dane(int n_pokoi, List<_pokoj> data_pokoje)
    {
        int pom = 0; 
        foreach (_pokoj var in data_pokoje)
        {
            if (var.ilosc > 0) { pom += var.ilosc; }
        }
        if (pom > n_pokoi) {
            Debug.Log("Jest wiecej 'obowiazkowych' pokoi niz istnieje pokoi na korytarzu. \n Stworz wiecej pokoi na korytarzu, lub zmien ilosc obowiazkowych pokoi.");
            return false; }
        return true;
    }
    public void destroy_korytarz()
    {
        foreach (GameObject var in instancje)
        {
            Destroy(var);
        }
        instancje.Clear(); 
        foreach (_pokoj var in data_pokoje)
        {
            var.lista_p.Clear();
        }
        
    }

}
