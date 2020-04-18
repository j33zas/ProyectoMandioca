using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SkillManager_Pasivas : MonoBehaviour
{
    [Header("UI")]
    public GameObject model_skill_selector;
    GameObject selector;

    [Header("Data_base")]
    [SerializeField] List<SkillBase> my_current_active_skills;
    public Dictionary<SkillInfo, SkillBase> database_basebyinfo = new Dictionary<SkillInfo, SkillBase>();

    [SerializeField] List<SkillLevelByBranch> alllevels;
    public Dictionary<SkillType, List<SkillLevelByBranch>> database_levelbytype = new Dictionary<SkillType, List<SkillLevelByBranch>>();

    public List<SkillBase> current_list_of_skills = new List<SkillBase>();
    SkillType CURRENT_TYPE;
    SkillBase current_skills;

    [SerializeField] Queue<LevelRequest> newSkillRequests = new Queue<LevelRequest>();
    [SerializeField] List<LevelRequest> debugRequest = new List<LevelRequest>();


    [Header("For random")]
    public List<SkillInfo> list_of_generics = new List<SkillInfo>();
    Dictionary<SkillType, List<SkillInfo>> pool_info_by_type = new Dictionary<SkillType, List<SkillInfo>>();


    /////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////// ON LOAD BEGIN
    /////////////////////////////////////////////////////////////////////////////////////////////////////////
    ///

    private void Start()
    {
        Main.instance.eventManager.SubscribeToEvent(GameEvents.GAME_INITIALIZE, Initialize);
    }

    void Initialize()
    {
        my_current_active_skills = GetComponentsInChildren<SkillBase>().ToList();
        RefillFastDiccionaries();
        selector = Instantiate(model_skill_selector, Main.instance.gameUiController.MyCanvas.transform);
        selector.GetComponent<UI_BeginSkillSelector>().Initialize(SkillSelected);
        // Build_menu_for_testing();
    }

    void SkillSelected(SkillType _skillType)
    {
        SelectASkillType(_skillType);
        Main.instance.LoadLevelPlayObjects();
        selector.gameObject.SetActive(false);
    }
    void RefillFastDiccionaries()//no es necesario, pero lo tenemos para acceder mas rapido
    {
        //relleno un dictionary con INFO > BASE
        foreach (var s in my_current_active_skills)
            if (!database_basebyinfo.ContainsKey(s.skillinfo))
                database_basebyinfo.Add(s.skillinfo, s);


        //relleno un dictionary con TYPE > List<LEVEL>
        foreach (var level in alllevels)
        {
            if (!database_levelbytype.ContainsKey(level.skilltype))     //si no tengo la Key, creo una lista nueva y la guardo
            {
                List<SkillLevelByBranch> aux_list_of_levels = new List<SkillLevelByBranch>();
                aux_list_of_levels.Add(level);
                database_levelbytype.Add(level.skilltype, aux_list_of_levels);
            }
            else database_levelbytype[level.skilltype].Add(level);      //si tengo la key es xq ya hay una lista con algo adentro 
        }

        //relleno un dictionary con TYPE > List<INFO>
        foreach (var s in my_current_active_skills)
        {
            if (!pool_info_by_type.ContainsKey(s.skillinfo.skilltype))
            {
                List<SkillInfo> aux = new List<SkillInfo>();
                aux.Add(s.skillinfo);
                pool_info_by_type.Add(s.skillinfo.skilltype, aux);
            }
            {
                pool_info_by_type[s.skillinfo.skilltype].Add(s.skillinfo);
            }
        }
    }

    //esto viene del selector al principio del nivel
    public void SelectASkillType(SkillType _skilltype)
    {
        CURRENT_TYPE = _skilltype;
        Refresh();
    }
    /////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////// ON GAME
    /////////////////////////////////////////////////////////////////////////////////////////////////////////
    ///

    int requestindex = 0;

    public void CreateRequest_NewSkill()// Entro aca cada vez que subo de nivel
    {
        var req = new SkillInfo[0];

        var current_level = database_levelbytype[CURRENT_TYPE][requestindex];

        LevelRequest request;

        if (current_level.isFromRandom)
        {
            request = new LevelRequest(
                GetRandomArray(CURRENT_TYPE, current_level.max_from_this_branch, current_level.max_from_generics), 
                requestindex, 
                current_level.max_from_this_branch, 
                current_level.max_from_generics);
        }
        else
        {
            request = new LevelRequest(current_level.Selection, requestindex, current_level.max_from_this_branch, current_level.max_from_generics);
        }

        newSkillRequests.Enqueue(request);
        DebugRequest();

       
        requestindex++;
    }

    void DebugRequest()
    {
        debugRequest = new List<LevelRequest>();
        foreach (var d in newSkillRequests)
        {
            debugRequest.Add(d);
        }
    }

    //esto consta de 3 partes
    // primero creo una lista a rellenar, elegi una lista porque por ahi tal vez me quedé sin genericos y es mas facil porque no tengo que rezisear el array
    // luego busco un random en la biblioteca, pero de el tipo que estoy necesitando
    // por ultimo agarro algunos random genericos, si ya no tengo simplemente no agrega nada
    public SkillInfo[] GetRandomArray(SkillType skillType, int currentbranchcount = 1, int genericCount = 2)
    {
        List<SkillInfo> tofill = new List<SkillInfo>();

        var coll_my_type = pool_info_by_type[skillType];

        int my_type_posible_iterator = GetPosiblesIterations(coll_my_type.Count, currentbranchcount);

        //obtengo uno de mi tipo
        if (my_type_posible_iterator <= 0) throw new System.Exception("Che... me quedé con la biblioteca vacía");
        else
        {
            var auxcoll = new List<SkillInfo>(coll_my_type);

            for (int i = 0; i < my_type_posible_iterator; i++)
            {
                var indextoremove = Random.Range(0, auxcoll.Count);
                tofill.Add(auxcoll[indextoremove]);
                auxcoll.RemoveAt(indextoremove);
            }
        }

        //obtengo 1 o mas genéricos
        if (list_of_generics.Count > 0)
        {
            // un auxiliar List del Queue, solo para obtener un random...
            // hago esto xq no quiero modificar la queue, solo ver 2 randoms
            var auxgenerics = new List<SkillInfo>(list_of_generics);
            for (int i = 0; i < genericCount; i++)
            {
                var indextoremove = Random.Range(0, auxgenerics.Count);
                tofill.Add(auxgenerics[indextoremove]);
                auxgenerics.RemoveAt(indextoremove);
            }
        }

        return tofill.ToArray();

    }

    //esto es para que no me pida mas iteraciones de la que mi collecion puede darme
    int GetPosiblesIterations(int count_of_my_collection, int max_estimated) => count_of_my_collection <= max_estimated ? count_of_my_collection : max_estimated;

    //public void EVENT_GetRequest()
    //{
    //    if (newSkillRequests.Count > 0)
    //    {
    //        //solo lo observa, no ddesencolo la request xq si llego a cerrar la ui, la pierdo
    //        var peekedrequest = newSkillRequests.Peek();
    //        foreach (var i in peekedrequest)
    //        {
    //            Debug.Log("Obtengo: " + i.skill_name);
    //        }
    //        Main.instance.gameUiController.CreateNewSkillSelectionPopUp(peekedrequest.ToList(), ReturnSkill);
    //    }
    //}

    //Devuelvo un request
    //public List<SkillInfo> GetSkillRequest()
    //{
    //    // return newSkillRequests.Dequeue().ToList();
    //}
    public List<SkillInfo> GetPeekedRequest()
    {
        return newSkillRequests.Peek().Collection;
    }
    public bool I_Have_An_Active_Request() => newSkillRequests.Count() > 0;

    // Te la hice publica para poder agarrarla desde el menu.

    bool algoandamal;
    public void ReturnSkill(SkillInfo info)
    {
        var request = newSkillRequests.Peek();

        if (request != null)
        {
            if (!current_list_of_skills.Contains(database_basebyinfo[info]))
            {
               

                foreach (var inf in request.Collection)
                {
                    if (info == inf)
                    {
                        

                        if (inf.skilltype != SkillType.generics)
                        {
                            pool_info_by_type[inf.skilltype].Remove(inf);
                        }
                        else
                        {
                            list_of_generics.Remove(inf);

                        }

                        newSkillRequests.Dequeue();
                    }
                }

                current_list_of_skills.Add(database_basebyinfo[info]);
            }
            else
            {
                throw new System.Exception("Ya lo tengo agergado");
            }
        }
        else
        {
            throw new System.Exception("No tengo mas requests");
        }

        if (newSkillRequests.Count > 0)
        {
            newSkillRequests.Peek().Refill(GetRandomArray(CURRENT_TYPE, request.Max_to_branch, request.Max_to_random));
            DebugRequest();
        }
        

        Refresh();
    }
    public void Refresh()
    {
        foreach (var s in current_list_of_skills)
        {
            s.BeginSkill();
        }
        Main.instance.gameUiController.UI_Send_NameSkillType(CURRENT_TYPE.ToString());
        Main.instance.gameUiController.RefreshPassiveSkills_UI(current_list_of_skills.Select(x => x.skillinfo).ToList());
        Main.instance.gameUiController.UI_RefreshMenu();
        Main.instance.gameUiController.SetSelectedPath(CURRENT_TYPE.ToString());
    }

    [System.Serializable]
    public class LevelRequest
    {
        [SerializeField] List<SkillInfo> collection = new List<SkillInfo>();
        [SerializeField] int indexLevel; public int IndexLevel { get => indexLevel; }
        [SerializeField] int max_to_branch; public int Max_to_branch { get => max_to_branch; }
        [SerializeField] int max_to_random; public int Max_to_random { get => max_to_random; }
        public List<SkillInfo> Collection { get => collection; }
        public LevelRequest(SkillInfo[] col, int _indexLevel, int _max_to_branch, int _max_to_random)
        {
            collection = col.ToList();
            indexLevel = _indexLevel;
            max_to_branch = _max_to_branch;
            max_to_random = _max_to_random;
        }

        public void Refill(SkillInfo[] col)
        {
            collection = col.ToList();
        }
    }



    /////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////// FOR TESTING MENU
    /////////////////////////////////////////////////////////////////////////////////////////////////////////
    ///
    [Header("Testing Menu")]
    public bool testing;
    bool isOpen = false;
    Dictionary<SkillType, SkillBase> currents = new Dictionary<SkillType, SkillBase>();
    public List<SkillInfo> skillinfos;
    [SerializeField] UI_SkillHandler frontend;
    bool dataisloaded;
    public void LoadFromDisk(Dictionary<SkillType, SkillBase> dic) { currents = dic; dataisloaded = true; }
    public void Build_menu_for_testing()
    {
        if (!dataisloaded)
        {
            foreach (var skill in my_current_active_skills)
            {
                if (!currents.ContainsKey(skill.skillinfo.skilltype))
                {
                    currents.Add(skill.skillinfo.skilltype, skill);
                }
            }
        }
        frontend.Build(my_current_active_skills, OnUISelected);
        foreach (var s in currents.Values) s.BeginSkill();
    }
    void OnUISelected(int i)
    {

        var select = my_current_active_skills[i];
        var old = currents[select.skillinfo.skilltype];

        old.EndSkill();
        select.BeginSkill();
        currents[select.skillinfo.skilltype] = select;

        frontend.SetInfoSelected(select.skillinfo);
    }
    public void EVENT_OnbackButton()
    {
        if (testing)
        {
            if (!isOpen)
            {
                isOpen = true;
                frontend.Open();
                //lo abrimos
            }
            else
            {
                isOpen = false;
                frontend.Close();
                //lo cerramos
            }
        }
    }
}
