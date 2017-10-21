using System;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using LuaInterface;
using Manager;
using System.Collections.Generic;

namespace UnityDLL.GameLogic.Common
{
    public class DOTweenUtil
    {
        static Vector3 target;

        public static Tweener DOMove(Transform transform, Vector3 target, float time, Ease ease, float delay)
        {
            Tweener tweener = transform.DOMove(target, time);
            //设置这个Tween不受Time.scale影响
            tweener.SetUpdate(true);
            //设置移动类型
            tweener.SetEase(ease);
            tweener.SetDelay(delay);
            return tweener;
        }

        public static Tweener DOMove(int transformId, Vector3 target, float time, Ease ease, float delay)
        {
            Tweener tweener = DOMove(ObjectManager.Get(transformId), target, time, ease, delay);
            return tweener;
        }

        public static Tweener DOMove(int transformId, float targetx, float targety, float targetz, float time, int ease, float delay)
        {
            target.x = targetx; target.y = targety; target.z = targetz;
            Tweener tweener = DOMove(ObjectManager.Get(transformId), target, time, (Ease)ease, delay);
            return tweener;
        }

        public static Tweener DOMove(Transform transform, Vector3 target, float time, float delay)
        {
            return DOMove(transform, target, time, Ease.Linear, delay);
        }

        public static Tweener DOMoveWithTimeScale(Transform transform, Vector3 target, float time, float delay)
        {
            Tweener tweener = transform.DOMove(target, time);
            //设置这个Tween不受Time.scale影响
            tweener.SetUpdate(false);
            //设置移动类型
            tweener.SetEase(Ease.Linear);
            tweener.SetDelay(delay);
            return tweener;
        }

        public static Tweener DOMove(int transformId, Vector3 target, float time, float delay)
        {
            return DOMove(transformId, target, time, Ease.Linear, delay);
        }

        public static Tweener DOMove(int transformId, float targetx, float targety, float targetz, float time, float delay)
        {
            target.x = targetx; target.y = targety; target.z = targetz;
            return DOMove(transformId, target, time, Ease.Linear, delay);
        }

        public static Tweener DOMove(Transform transform, Vector3 target, float time, float delay, LuaFunction f)
        {
            return DOMove(transform, target, time, Ease.Linear, delay).OnComplete(() =>
            {
                if (f != null)
                {
                    f.Call();
                }
            });
        }

        public static Tweener DOMove(int transformId, Vector3 target, float time, float delay, LuaFunction f)
        {
            return DOMove(ObjectManager.Get(transformId), target, time, delay, f);
        }

        public static Tweener DOMove(Transform transform, Vector3 target, float duration, float delay, LuaFunction lf, Ease ease)
        {
            Tweener t = DOMove(transform, target, duration, ease, delay);

            t.OnComplete(() =>
            {
                if (lf != null)
                {
                    lf.Call();
                }

            });
            return t;
        }

        public static Tweener DOMove(int transformId, Vector3 target, float duration, float delay, LuaFunction lf, Ease ease)
        {
            return DOMove(ObjectManager.Get(transformId), target, duration, delay, lf, ease);
        }

        public static Tweener DoAnchoredPosition(Transform tf, Vector3 endPos, float duration, float delay, LuaFunction lf, Ease ease)
        {
            RectTransform tr = (RectTransform)tf;

            Tweener tw = tr.DOAnchorPos(endPos, duration).SetDelay(delay).SetEase(ease).OnComplete(
                () =>
                {
                    if (lf != null)
                    {
                        lf.Call();
                    }

                }
            );

            tw.SetUpdate(true);
            return tw;
        }

        public static Tweener DoAnchoredPosition(int transformId, Vector3 endPos, float duration, float delay, LuaFunction lf, Ease ease)
        {
            return DoAnchoredPosition(ObjectManager.Get(transformId), endPos, duration, delay, lf, ease);
        }

        public static Tweener DOMoveX(Transform transform, float target, float time, Ease ease, float delay, LuaFunction func)
        {
            Tweener tweener = transform.DOMoveX(target, time);
            //设置这个Tween不受Time.scale影响
            tweener.SetUpdate(true);
            //设置移动类型
            tweener.SetEase(ease);
            tweener.SetDelay(delay);
            if (func != null)
            {
                tweener.OnComplete(() =>
                {
                    func.Call();
                });
            }
            return tweener;
        }

        public static Tweener DOMoveX(Transform transform, float target, float time, Ease ease, float delay)
        {
            return DOMoveX(transform, target, time, ease, delay, null);
        }

        public static Tweener DOMoveX(int transformId, float target, float time, Ease ease, float delay)
        {
            return DOMoveX(ObjectManager.Get(transformId), target, time, ease, delay);
        }

        public static Tweener DOMoveX(Transform transform, float target, float time, float delay)
        {
            return DOMoveX(transform, target, time, Ease.Linear, delay);
        }

        public static Tweener DOMoveX(int transformId, float target, float time, float delay)
        {
            return DOMoveX(ObjectManager.Get(transformId), target, time, Ease.Linear, delay);
        }

        public static Tweener DOMoveY(Transform transform, float target, float time, Ease ease, float delay)
        {
            Tweener tweener = transform.DOMoveY(target, time);
            //设置这个Tween不受Time.scale影响
            tweener.SetUpdate(true);
            //设置移动类型
            tweener.SetEase(ease);
            tweener.SetDelay(delay);
            return tweener;
        }

        public static Tweener DOLocalMoveY(Transform transform, float target, float time, Ease ease, float delay)
        {
            Tweener tweener = transform.DOLocalMoveY(target, time);
            //设置这个Tween不受Time.scale影响
            tweener.SetUpdate(true);
            //设置移动类型
            tweener.SetEase(ease);
            tweener.SetDelay(delay);
            return tweener;
        }

        public static Tweener DOMoveY(int transformId, float target, float time, Ease ease, float delay)
        {
            return DOMoveY(ObjectManager.Get(transformId), target, time, ease, delay);
        }

        public static void DOIntValue(Text tex, float endValue, float duration, float delay, Ease e, LuaFunction lf)
        {
            if (tex != null)
            {
                float cuValue = 0;

                if (float.TryParse(tex.text, out cuValue))
                {
                    Tweener t = DOTween.To(() => { return cuValue; }, (x) =>
                    {
                        cuValue = (int)x;
                        if (tex != null)
                        {
                            tex.text = cuValue.ToString();
                        }

                    }, endValue, duration).SetEase(e).SetDelay(delay).OnComplete(() =>
                    {
                        if (lf != null)
                        {
                            lf.Call();
                        }

                    });
                }
            }
        }

        public static Tweener DOMoveY(Transform transform, float target, float time, float delay)
        {
            return DOMoveY(transform, target, time, Ease.Linear, delay);
        }

        public static Tweener DOMoveY(int transformId, float target, float time, float delay)
        {
            return DOMoveY(ObjectManager.Get(transformId), target, time, Ease.Linear, delay);
        }

        public static Tweener DOMoveZ(Transform transform, float target, float time, Ease ease, float delay)
        {
            Tweener tweener = transform.DOMoveZ(target, time);
            //设置这个Tween不受Time.scale影响
            tweener.SetUpdate(true);
            //设置移动类型
            tweener.SetEase(ease);
            tweener.SetDelay(delay);
            return tweener;
        }

        public static Tweener DOMoveZ(int transformId, float target, float time, Ease ease, float delay)
        {
            return DOMoveZ(ObjectManager.Get(transformId), target, time, ease, delay);
        }

        public static Tweener DOMoveZ(Transform transform, float target, float time, float delay)
        {
            return DOMoveZ(transform, target, time, Ease.Linear, delay);
        }

        public static Tweener DOMoveZ(int transformId, float target, float time, float delay)
        {
            return DOMoveZ(ObjectManager.Get(transformId), target, time, Ease.Linear, delay);
        }

        public static Tweener DORotate(Transform transform, Vector3 target, float time, Ease ease, float delay)
        {
            Tweener tweener = transform.DORotate(target, time);
            //设置这个Tween不受Time.scale影响
            tweener.SetUpdate(true);
            //设置移动类型
            tweener.SetEase(ease);
            tweener.SetDelay(delay);
            return tweener;
        }

        public static Tweener DORotate(int transformId, Vector3 target, float time, Ease ease, float delay)
        {
            return DORotate(ObjectManager.Get(transformId), target, time, ease, delay);
        }

        public static Tweener DORotate(int transformId, float targetx, float targety, float targetz, float time, int ease, float delay)
        {
            target.x = targetx; target.y = targety; target.z = targetz;
            return DORotate(ObjectManager.Get(transformId), target, time, (Ease)ease, delay);
        }

        public static Tweener DORotate(Transform transform, Vector3 target, float time, float delay)
        {
            return DORotate(transform, target, time, Ease.Linear, delay);
        }

        public static Tweener DORotate(int transformId, Vector3 target, float time, float delay)
        {
            return DORotate(ObjectManager.Get(transformId), target, time, Ease.Linear, delay);
        }

        public static Tweener DORotate(int transformId, float targetx, float targety, float targetz, float time, float delay)
        {
            target.x = targetx; target.y = targety; target.z = targetz;
            return DORotate(ObjectManager.Get(transformId), target, time, Ease.Linear, delay);
        }

        public static Tweener DOScale(Transform transform, Vector3 target, float time, Ease ease, float delay)
        {
            Tweener tweener = transform.DOScale(target, time);
            tweener.SetUpdate(true);
            tweener.SetEase(ease);
            tweener.SetDelay(delay);
            return tweener;
        }

        public static Tweener DOScale(int transformId, Vector3 target, float time, int easeType, int loopType, float delay)
        {
            Tweener tweener = ObjectManager.Get(transformId).DOScale(target, time);
            tweener.SetLoops<Tweener>(-1, (LoopType)loopType);
            tweener.SetUpdate(true);
            tweener.SetEase((Ease)easeType);
            tweener.SetDelay(delay);
            return tweener;
        }

        public static Tweener DOScale(int transformId, float targetx, float targety, float targetz, float time, int easeType, int loopType, float delay)
        {
            target.x = targetx; target.y = targety; target.z = targetz;
            Tweener tweener = ObjectManager.Get(transformId).DOScale(target, time);
            tweener.SetLoops<Tweener>(-1, (LoopType)loopType);
            tweener.SetUpdate(true);
            tweener.SetEase((Ease)easeType);
            tweener.SetDelay(delay);
            return tweener;
        }

        public static Tweener DOScale(int transformId, Vector3 target, float time, Ease ease, float delay)
        {
            return DOScale(ObjectManager.Get(transformId), target, time, ease, delay);
        }

        public static Tweener DOScale(int transformId, float targetx, float targety, float targetz, float time, int ease, float delay)
        {
            target.x = targetx; target.y = targety; target.z = targetz;
            return DOScale(ObjectManager.Get(transformId), target, time, (Ease)ease, delay);
        }

        public static Tweener DOScale(Transform transform, Vector3 target, float time, float delay)
        {
            return DOScale(transform, target, time, Ease.Linear, delay);
        }

        public static Tweener DOScale(int transformId, Vector3 target, float time, float delay)
        {
            return DOScale(ObjectManager.Get(transformId), target, time, Ease.Linear, delay);
        }

        public static Tweener DOScale(int transformId, float targetx, float targety, float targetz, float time, float delay)
        {
            target.x = targetx; target.y = targety; target.z = targetz;
            return DOScale(ObjectManager.Get(transformId), target, time, Ease.Linear, delay);
        }

        public static Tweener DOScaleCall(Transform transform, Vector3 target, float time, float delay, LuaFunction lf)
        {
            return DOScale(transform, target, time, Ease.Linear, delay).OnComplete(() =>
            {

                if (lf != null)
                {
                    lf.Call();
                }

            });
        }

        public static Tweener DOScaleCall(Transform transform, Vector3 target, float time, Ease ease, float delay, LuaFunction lf)
        {
            return DOScale(transform, target, time, ease, delay).OnComplete(() =>
            {

                if (lf != null)
                {
                    lf.Call();
                }

            });
        }

        

        public static Tweener DOAlpha(Transform transform, float from, float to, float time, Ease ease, float delay)
        {
            Renderer renderer = transform.GetComponent<Renderer>();
            Material mat = null;
            Tweener tweener = null;
            if (renderer != null)
            {
                mat = renderer.material;
                mat.color = new Color(mat.color.r, mat.color.g, mat.color.b, from);
                tweener = mat.DOFade(to, time);
            }
            else if (transform.GetComponent<Image>() != null)
            {
                Image img = transform.GetComponent<Image>();
                img.color = new Color(img.color.r, img.color.g, img.color.b, from);
                tweener = img.DOFade(to, time);
            }
            else if (transform.GetComponent<Text>() != null)
            {
                Text text = transform.GetComponent<Text>();
                text.color = new Color(text.color.r, text.color.g, text.color.b, from);
                tweener = text.DOFade(to, time);
            }
            
            //设置这个Tween不受Time.scale影响
            tweener.SetUpdate(true);
            //设置移动类型
            tweener.SetEase(ease);
            tweener.SetDelay(delay);
            return tweener;
        }

        public static Tweener DOAlpha(int transformId, float from, float to, float time, Ease ease, float delay)
        {
            return DOAlpha(ObjectManager.Get(transformId), from, to, time, ease, delay);
        }

        public static Tweener DOAlpha(Transform transform, float from, float to, float time, float delay)
        {
            return DOAlpha(transform, from, to, time, Ease.Linear, delay);
        }

        public static Tweener DOAlpha(int transformId, float from, float to, float time, float delay)
        {
            return DOAlpha(ObjectManager.Get(transformId), from, to, time, delay);
        }

        public static Tweener DOAlpha(Material mat, float from, float to, float time, float delay)
        {
            mat.color = new Color(mat.color.r, mat.color.g, mat.color.b, from);
            Tweener tweener = mat.DOFade(to, time);
            //设置这个Tween不受Time.scale影响
            tweener.SetUpdate(true);
            //设置移动类型
            tweener.SetEase(Ease.Linear);
            tweener.SetDelay(delay);
            return tweener;
        }

        public static Tweener DOAlphaText(Text text, float from, float to, float time, float delay)
        {
            text.color = new Color(text.color.r, text.color.g, text.color.b, from);
            Tweener tweener = text.DOFade(to, time);
            //设置这个Tween不受Time.scale影响
            tweener.SetUpdate(true);
            //设置移动类型
            tweener.SetEase(Ease.Linear);
            tweener.SetDelay(delay);
            return tweener;
        }

        public static Tweener DOAlphaText(int transformId, int componentId, float from, float to, float time, float delay)
        {
            Text text = ObjectManager.GetComponent<Text>(transformId, componentId);
            return DOAlphaText(text, from, to, time, delay);
        }

        public static Tweener DOAlphaImage(int transformId, int componentId, float from, float to, float time, float delay)
        {
            Image text = ObjectManager.GetComponent<Image>(transformId, componentId);
            text.color = new Color(text.color.r, text.color.g, text.color.b, from);
            Tweener tweener = text.DOFade(to, time);
            //设置这个Tween不受Time.scale影响
            tweener.SetUpdate(true);
            //设置移动类型
            tweener.SetEase(Ease.Linear);
            tweener.SetDelay(delay);
            return tweener;
        }

        public static Tweener DOColor(Image image, Color from, Color to, float time, Ease ease, float delay)
        {
            if (image == null)
            {
                return null;
            }
            image.color = from;
            Tweener tweener = image.DOColor(to, time);
            //设置这个Tween不受Time.scale影响
            tweener.SetUpdate(true);
            //设置移动类型
            tweener.SetEase(ease);
            tweener.SetDelay(delay);
            return tweener;
        }

        public static Tweener DOColor(int transformId, int imageId, Color from, Color to, float time, Ease ease, float delay)
        {
            return DOColor(ObjectManager.GetComponent<Image>(transformId, imageId), from, to, time, ease, delay);
        }

        public static Tweener DOColor(Image image, Color from, Color to, float time, float delay)
        {
            return DOColor(image, from, to, time, Ease.Linear, delay);
        }

        public static Tweener DOColor(int transformId, int imageId, Color from, Color to, float time, float delay)
        {
            return DOColor(ObjectManager.GetComponent<Image>(transformId, imageId), from, to, time, Ease.Linear, delay);
        }

        public static Tweener DOColor(Material mat, Color from, Color to, float time, float delay)
        {
            mat.color = from;
            Tweener tweener = mat.DOColor(to, time);
            //设置这个Tween不受Time.scale影响
            tweener.SetUpdate(true);
            //设置移动类型
            tweener.SetEase(Ease.Linear);
            tweener.SetDelay(delay);
            return tweener;
        }

        public static Tweener DOPath(Transform transform, Vector3[] path, float time, PathType type, float delay)
        {
            Tweener tweener = transform.DOPath(path, time, type);
            tweener.SetUpdate(true);
            tweener.SetDelay(delay);
            tweener.SetEase(Ease.Linear);
            return tweener;
        }

        public static Tweener DOPath(int transformId, Vector3[] path, float time, PathType type, float delay)
        {
            return DOPath(ObjectManager.Get(transformId), path, time, type, delay);
        }

        public static Tweener DOPath(Transform transform, Vector3[] path, float time, float delay)
        {
            return DOPath(transform, path, time, PathType.CatmullRom, delay);
        }

        public static Tweener DOPath(int transformId, Vector3[] path, float time, float delay)
        {
            return DOPath(ObjectManager.Get(transformId), path, time, PathType.CatmullRom, delay);
        }

        public static Tweener DOShake(Transform transform, float duration, float strength = 1, int vibrato = 10, float randomness = 90)
        {
            return transform.DOShakeScale(duration, strength, vibrato, randomness);
        }

        public static Tweener DOShake(int transformId, float duration, float strength = 1, int vibrato = 10, float randomness = 90)
        {
            return DOShake(ObjectManager.Get(transformId), duration, strength, vibrato, randomness);
        }

        public static Tweener DOShakeRotate(Transform transform, float duration, float strength = 1, int vibrato = 10, float randomness = 90, float delay = 0)
        {
            Tweener tweener = transform.DOShakeRotation(duration, strength, vibrato, randomness);
            tweener.SetDelay(delay);
            return tweener;
        }

        public static Tweener DOShakeRotate(int transformId, float duration, float strength = 1, int vibrato = 10, float randomness = 90, float delay = 0)
        {
            return DOShakeRotate(ObjectManager.Get(transformId), duration, strength, vibrato, randomness, delay);
        }

        public static void Stop(Tweener tweener)
        {
            tweener.Kill();
        }

        public static Ease GetEaseType(int type)
        {
            return (Ease)type;
        }
    }
}
