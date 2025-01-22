using System;
using System.Collections;
using UnityEngine;

namespace Utils
{
    public interface IWaitingView
    {
        Coroutine StartCoroutine(IEnumerator enumerator);
        void StopCoroutine(Coroutine coroutine);
        void StopCoroutine(IEnumerator enumerator);
        Coroutine ExecuteLoopInSeconds(float seconds, int count, Action action, float delay = 0f);
    }

    public sealed class WaitingMonoBehaviour : MonoBehaviour, IWaitingView
    {
        public new void StopCoroutine(Coroutine coroutine)
        {
            if (this)
                base.StopCoroutine(coroutine);
        }

        public new void StopCoroutine(IEnumerator enumerator)
        {
            if (this)
                base.StopCoroutine(enumerator);
        }

        public Coroutine ExecuteLoopInSeconds(float seconds, int count, Action action, float delay)
        {
            var cor = StartCoroutine(LoopExecuteInSecondsCoroutine(seconds, count, action, delay));
            return cor;
        }

        private IEnumerator LoopExecuteInSecondsCoroutine(float seconds, int loopCount, Action action, float delay)
        {
            yield return new WaitForSeconds(delay);
            if (loopCount < 0)
            {
                while (true)
                {
                    action.Invoke();
                    yield return new WaitForSeconds(seconds);
                }
            }

            for (var i = 0; i < loopCount; i++)
            {
                action.Invoke();
                yield return new WaitForSeconds(seconds);
            }
        }
    }
}
