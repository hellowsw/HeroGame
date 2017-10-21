using GameCommon;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameLogic.BYComponent
{
    [RequireComponent(typeof(Image))]
    public class NetImage : MonoBehaviour
    {
        public static Dictionary<string, Sprite> buffer = new Dictionary<string, Sprite>();

        private Image image;

        public string _url;

        public string url {
            get {
                return _url;
            }
            set {
                _url = value;
                if (!gameObject.activeInHierarchy)
                {
                    return;
                }
                StopAllCoroutines();
                StartCoroutine(LoadImage(_url));
            }
        }

        public Sprite sprite {
            set {
                Image.sprite = value;
            }
            get {
                return Image.sprite;
            }
        }

        public Sprite defaultSprite;

        public Image Image
        {
            get
            {
                if (image == null)
                    image = GetComponent<Image>();
                return image;
            }
            set
            {
                image = value;
            }
        }

        IEnumerator LoadImage(string _url)
        {
            if (buffer.ContainsKey(_url))
            {
                Image.sprite = buffer[_url];
            }
            else
            {
                WWW www = new WWW(_url);
                yield return www;
                if (www.error != null)
                {
                    Debug.Log("NetImage Download Image Failed! url:" + _url);
                    Image.sprite = defaultSprite;
                    if (!buffer.ContainsKey(_url))
                    {
                        buffer.Add(_url, defaultSprite);
                    }
                }
                else
                {
                    if (!buffer.ContainsKey(_url))
                    {
                        Texture2D texture = www.texture as Texture2D;
                        texture.filterMode = FilterMode.Bilinear;
                        buffer.Add(_url, CommonLib.Texture2DToSprite(texture));
                    }
                    Image.sprite = buffer[_url];
                }
            }
        }
    }
}
