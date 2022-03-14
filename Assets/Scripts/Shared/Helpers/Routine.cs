using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Shared.Helpers
{
    //>>>This is a class containing generic coroutine functions

    public static class Routine
    {
        public static MonoBehaviour instance;
        public static void Initialize(MonoBehaviour mono)
        {
            instance = mono;
        }

        public static Coroutine WaitAndCall(float seconds, Action action)
        {
            return instance.StartCoroutine(WaitAndCallRoutine(seconds, action));
        }

        public static void Stop(Coroutine routine)
        {
            if (routine != null)
                instance.StopCoroutine(routine);
        }

        public static Coroutine Lerp(float startValue, float endValue, float speed = .2f, Action<float> action = null, Action actionEnd = null)
        {
            return instance.StartCoroutine(LerpRoutine(startValue, endValue, speed, action, actionEnd));
        }

        public static Coroutine MovePosition(Transform trans, Vector3 startPos, Vector3 targetPos, float speed = .2f, Action action = null)
        {
            return instance.StartCoroutine(LerpRoutine(0, 1, speed, (val) => trans.position = startPos + (targetPos - startPos) * val, action));
        }

        public static Coroutine MoveAnchorPos(RectTransform trans, Vector2 startPos, Vector2 targetPos, float speed = .2f, Action action = null)
        {
            return instance.StartCoroutine(LerpRoutine(0, 1, speed, (val) => trans.anchoredPosition = startPos + (targetPos - startPos) * val, action));
        }

        public static Coroutine MoveAnchors(RectTransform trans, Vector2 startVal, Vector2 targetVal, float speed = .2f, Action action = null)
        {
            return instance.StartCoroutine(LerpRoutine(0, 1, speed, (val) => trans.anchorMin = trans.anchorMax = startVal + (targetVal - startVal) * val, action));
        }

        public static Coroutine MovePivot(RectTransform trans, Vector2 startVal, Vector2 targetVal, float speed = .2f, Action action = null)
        {
            return instance.StartCoroutine(LerpRoutine(0, 1, speed, (val) => trans.pivot = startVal + (targetVal - startVal) * val, action));
        }

        public static Coroutine MoveConstant(Transform trans, Vector3 startPos, Vector3 targetPos, float speed = 5f, Action action = null)
        {
            return instance.StartCoroutine(LerpConstant(trans, startPos, targetPos, speed, action));
        }

        public static Coroutine Scale(Transform trans, Vector3 startScale, Vector3 targetScale, float speed = .2f, Action action = null)
        {
            return instance.StartCoroutine(LerpRoutine(0, 1, speed, (val) => trans.localScale = startScale + (targetScale - startScale) * val, action));
        }

        public static Coroutine Rotate(Transform trans, float startAngle, float targetAngle, float speed = .2f, Action action = null)
        {
            return instance.StartCoroutine(LerpRoutine(0, 1, speed, (val) => trans.eulerAngles = new Vector3(0, 0, startAngle + (targetAngle - startAngle) * val), action));
        }

        public static void Open(Transform trans, float speed = .3f)
        {
            instance.StartCoroutine(OpenRoutine(trans));
        }

        public static IEnumerator FadeAndScaleText(Text text, string msg)
        {
            RectTransform textObj = text.gameObject.GetComponent<RectTransform>();
            text.color = new Color(text.color.r, text.color.g, text.color.b, 1);
            textObj.localScale = Vector3.one;
            while (text.color.a > 0)
            {
                text.color = new Color(text.color.r, text.color.g, text.color.b, text.color.a - 0.1f);
                textObj.localScale = new Vector3(textObj.localScale.x - 0.01f, textObj.localScale.y - 0.01f, 1);
                yield return new WaitForSeconds(.01f);
            }

            text.text = msg;
            while (text.color.a < 1)
            {
                text.color = new Color(text.color.r, text.color.g, text.color.b, text.color.a + 0.1f);
                textObj.localScale = new Vector3(textObj.localScale.x + 0.01f, textObj.localScale.y + 0.01f, 1);
                yield return new WaitForSeconds(.01f);
            }
            text.color = new Color(text.color.r, text.color.g, text.color.b, 1);
            textObj.localScale = Vector3.one;
        }

        public static IEnumerator FadeImage(Image i, float start, float end, float speed = 1)
        {
            Color c = i.color;
            c.a = start;
            speed = (end - start) / 10f * speed;
            int sign = (int)Mathf.Sign(speed);
            while (c.a * sign < end * sign)
            {
                c.a += speed;
                i.color = c;
                yield return new WaitForFixedUpdate();
            }
            c.a = end;
        }

        public static IEnumerator FadeMusic(AudioSource audio, float volume = 1)
        {
            audio.UnPause();
            float delta = Mathf.Abs(audio.volume - volume) / 50f;
            if (audio.volume < volume)
            {
                while (audio.volume < volume)
                {
                    yield return new WaitForFixedUpdate();
                    audio.volume += delta;
                }
            }
            else
            {
                while (audio.volume > volume)
                {
                    yield return new WaitForFixedUpdate();
                    audio.volume -= delta;
                }
            }
            if (volume == 0)
                audio.Pause();
        }

        public static IEnumerator LoadingTextAnim(Text loading)
        { // // set message seen
            while (loading.text.Contains("Loading"))
            {
                loading.text = loading.text == "Loading..." ? "Loading." : loading.text + ".";
                yield return new WaitForSeconds(0.5f);
            }
        }

        static IEnumerator WaitAndCallRoutine(float seconds, Action action)
        {
            yield return new WaitForSeconds(seconds);
            action();
        }

        static IEnumerator LerpRoutine(float startValue, float endValue, float speed = .2f, Action<float> action = null, Action actionEnd = null)
        {
            float threshold = Mathf.Abs(startValue - endValue) / 500f;
            while (Mathf.Abs(startValue - endValue) > threshold)
            {  //fill In
                startValue += (endValue - startValue) * speed;
                if (action != null)
                    action(startValue);
                yield return new WaitForEndOfFrame();
            }
            if (action != null)
                action(endValue);
            if (actionEnd != null)
                actionEnd();
        }

        static IEnumerator LerpConstant(Transform trans, Vector3 startPos, Vector3 targetPos, float speed = 5, Action action = null)
        {
            float delta = new Vector2(Screen.width, Screen.height).magnitude * speed / (targetPos - startPos).magnitude;
            for (int i = 0; i < 30; i++)
            { //fill In
                trans.position = startPos + (targetPos - startPos) * delta * i;
                yield return new WaitForEndOfFrame();
            }
            if (action != null)
                action();
        }

        static IEnumerator OpenRoutine(Transform transform, float speed = .3f)
        {
            Vector3 targetScale = new Vector3(1.05f, 1.05f, 1);
            transform.localScale = new Vector3(0.9f, 0.9f, 1);
            while (Vector2.Distance(transform.localScale, targetScale) > .01f)
            {  //Zoom In
                transform.localScale += (targetScale - transform.localScale) * speed;
                yield return new WaitForEndOfFrame();
            }
            while (Vector2.Distance(transform.localScale, Vector3.one) > .01f)
            {  //Zoom Out
                transform.localScale += (Vector3.one - transform.localScale) * speed;
                yield return new WaitForEndOfFrame();
            }
            transform.localScale = Vector3.one;
        }
    }
}
