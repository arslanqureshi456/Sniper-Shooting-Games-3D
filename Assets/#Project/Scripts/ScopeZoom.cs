using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Linq;

namespace UI
{
    [RequireComponent(typeof(ScrollRect))]
    public class ScopeZoom : MonoBehaviour, IBeginDragHandler, IEndDragHandler
    {
        [Header("General settings")]
        [SerializeField] private int framesForCorrection = 10;

        private List<Coroutine> coroutinesInProccess = new List<Coroutine>();
        private ScrollRect contextScrollRect;

        AudioSource audioSource;
        public AudioClip tickSound;
        public RectTransform transformRect;
        Vector3 currentPos = new Vector3(0, 0, 0);

        private void Awake()
        {
            audioSource = gameObject.GetComponent<AudioSource>();
            contextScrollRect = GetComponent<ScrollRect>();
        }

        void Update()
        {
            if ((int)transformRect.position.y != (int)currentPos.y)
            {

                if ((int)transformRect.position.y % (int)margin == 0)
                {
                    currentPos = transformRect.position;
                    audioSource.PlayOneShot(tickSound);
                }
            }
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            StopAutoSnapping();
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            BeginAutoSnapping();
        }

        public void BeginAutoSnapping()
        {
            StopAutoSnapping();

            coroutinesInProccess.Add(StartCoroutine(AutoSnappingRoutine()));
        }

        public void StopAutoSnapping()
        {
            foreach (var coroutine in coroutinesInProccess)
                if (coroutine != null)
                    StopCoroutine(coroutine);

            coroutinesInProccess.Clear();
        }

        private IEnumerator AutoSnappingRoutine()
        {
            if (contextScrollRect.content.childCount <= 1)
                yield break;

            Coroutine currentStep = StartCoroutine(WaitForInertiaStopsRoutine());
            coroutinesInProccess.Add(currentStep);

            yield return currentStep;

            currentStep = StartCoroutine(MoveToClosestPositionRoutine());
            coroutinesInProccess.Add(currentStep);

            yield return currentStep;
        }

        private IEnumerator WaitForInertiaStopsRoutine()
        {
            bool isMoving = true;
            Vector2 previousPosition = contextScrollRect.content.anchoredPosition;
            float passedDistance = float.MaxValue;

            while (isMoving)
            {
                yield return null;

                passedDistance = Vector2.Distance(previousPosition, contextScrollRect.content.anchoredPosition);
                previousPosition = contextScrollRect.content.anchoredPosition;

                isMoving = passedDistance > 5;
            }
        }

        public float margin;
        private IEnumerator MoveToClosestPositionRoutine()
        {
            contextScrollRect.StopMovement();

            float heightStep = margin;//GetHeightStep();

            Vector3 targetPosition = contextScrollRect.content.anchoredPosition;
            targetPosition.y = Mathf.Round(targetPosition.y / heightStep) * heightStep;

            Vector3 initPosition = contextScrollRect.content.anchoredPosition;

            for (int i = 0; i < framesForCorrection; i++)
            {
                contextScrollRect.content.anchoredPosition = Vector3.Lerp(initPosition, targetPosition, i / (float)framesForCorrection);

                yield return null;
            }

            contextScrollRect.content.anchoredPosition = targetPosition;
        }

        private float GetHeightStep()
        {
            List<Vector2> contentSizes = new List<Vector2>();

            foreach (RectTransform childRect in contextScrollRect.content)
                contentSizes.Add(childRect.sizeDelta);

            contentSizes = contentSizes.Distinct().ToList();

            return contentSizes.Sum(x => x.y);
        }

        void OnDisable()
        {
            StopCoroutine("AutoSnappingRoutine");
            StopCoroutine("WaitForInertiaStopsRoutine");
            StopCoroutine("MoveToClosestPositionRoutine");
        }
    }

}