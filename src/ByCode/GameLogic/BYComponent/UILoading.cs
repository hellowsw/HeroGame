using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using GameLogic.BYComponent;

namespace UnityDLL.GameLogic.BYComponent
{
    public class UILoading : MonoBehaviour
    {
        private static UILoading instance;
        Slider slider;
        Text txtProgress;
        Text txtPrompt;

#if UNITY_STANDALONE
        static string[] labels = {
            "长按鼠标左键可持续发炮！",
            "使用锁定可提高捕鱼成功几率！",
#if _DOUYU_
            "每次解锁炮倍率都可获得大量鱼丸奖励！",
            "捕获任意鱼都有几率掉落鱼丸哦！",
            "鱼丸、鱼翅要记得兑换，就可以在直播间送给喜欢的主播啦!",
            "竞技中心可获得京东卡等丰厚奖励，每日记得准时参与哦！",
#endif
            "捕获奖金鱼可以积累奖池，进行抽奖",
            "击杀boss有几率获得黄金弹头！",
            "使用黄金弹头可以获得大量金币！",
        };
#elif UNITY_IPHONE || UNITY_ANDROID
        static string[] labels = {
            "长按屏幕可持续发炮！",
            "使用锁定可提高捕鱼成功几率！",
#if _DOUYU_
            "每次解锁炮倍率都可获得大量鱼丸奖励！",
            "捕获任意鱼都有几率掉落鱼丸哦！",
            "鱼丸、鱼翅要记得兑换，就可以在直播间送给喜欢的主播啦!",
            "竞技中心可获得京东卡等丰厚奖励，每日记得准时参与哦！",
#endif
            "捕获奖金鱼可以积累奖池，进行抽奖",
            "击杀boss有几率获得黄金弹头！",
            "使用黄金弹头可以获得大量金币！",
        };
#endif

        private int index = 0;
        private float interval = 3f;
        private float timer = 0;

        private Image imgText;
        private bool firstLoading = true;

        void Awake()
        {
            slider = transform.FindChild("Slider").GetComponent<Slider>();
            txtProgress = transform.FindChild("txtProg").GetComponent<Text>();
            txtPrompt = transform.FindChild("text").GetComponent<Text>();
            imgText = transform.FindChild("imgText").GetComponent<Image>();

            System.Random ran=new System.Random();
            index = ran.Next(0, labels.Length);
            txtPrompt.text = labels[index];
            //transform.gameObject.AddComponent<Empty4Raycast>();
            AdjustAspect.Attach(transform.FindChild("bg").gameObject, AdjustAspect.EAspectPlane.Image, true);
        }

        void OnEnable()
        {
            System.Random ran = new System.Random();
            index = ran.Next(0, labels.Length);
            txtPrompt.text = labels[index];
        }

        void Update()
        {
            timer += Time.deltaTime;
            if (timer >= interval)
            {
                timer = 0;
                index++;
                if (index >= labels.Length)
                {
                    index = 0;
                }
                txtPrompt.text = labels[index];
            }
        }

        public void Set(float value, string tag)
        {
            index++;
            slider.value = Mathf.Max(0.07f, value);
            txtProgress.text = tag + Mathf.FloorToInt(value * 100) + "%";
        }

        public void RefreshRedpaper()
        {

        }

        public static UILoading CreateInstance()
        {
            if (instance == null)
            {
                instance = GameObject.Find("Launcher/UIManager/LoadingPanelPrefab").AddComponent<UILoading>();
                //GameObject go = Instantiate(ResourceManager.LoadResourceFromLocal("UI", "LoadingPanelPrefab")) as GameObject;
                //Transform trans = go.transform;
                //trans.SetParent(GameObject.Find("Launcher/UIManager").transform);
                //trans.localScale = Vector3.one;
                //instance = go.AddComponent<UILoading>();
            }
            return instance;
        }

        public static void Show(float value, string tag)
        {
            if (instance == null)
            {
                CreateInstance();
            }
            if (!instance.gameObject.activeInHierarchy)
            {
                if (Random.Range(0, 2) == 0)
                    instance.imgText.sprite = ResourceManager.LoadSpriteFromResources("loading", "001");
                else
                    instance.imgText.sprite = ResourceManager.LoadSpriteFromResources("loading", "002");

                instance.RefreshRedpaper();
            }
            if (!instance.gameObject.activeInHierarchy)
            {
                instance.gameObject.SetActive(true);
            }

            instance.transform.SetAsLastSibling();
            if (value >= 0)
                instance.Set(value, tag);

            instance.firstLoading = false;

        }

        public static void Show(float value)
        {
            Show(value, "");
        }

        public static void Hide()
        {
            if (instance.gameObject.activeInHierarchy)
            {
                instance.gameObject.SetActive(false);

            }
        }

        public static bool IsAcitve()
        {
            return instance.gameObject.activeInHierarchy;
        }

        public void ShowRedpaper()
        {

        }

        public void HideRedpaper()
        {

        }

        public void HideReward()
        {

        }
    }
}
