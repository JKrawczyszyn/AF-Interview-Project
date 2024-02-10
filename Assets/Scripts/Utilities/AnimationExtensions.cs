namespace AFSInterview.Utilities
{
    using System.Threading.Tasks;
    using UnityEngine;

    // In real project I would use some plugin like DOTween.
    public static class AnimationExtensions
    {
        public static async Task AnimateMove(this Transform transform, Vector3 targetPosition, float duration)
        {
            var startPosition = transform.position;

            var time = 0f;
            while (time < duration)
            {
                transform.position = Vector3.Lerp(startPosition, targetPosition, time / duration);
                time += Time.deltaTime;

                await Task.Yield();
            }

            transform.position = targetPosition;
        }
    }
}
