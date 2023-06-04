using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheAshBot.MonoBehaviours.TwoDimentional
{
    [RequireComponent(typeof(Renderer))]
    public class RendererSortingOrderSorter : MonoBehaviour
    {

        [SerializeField] private bool runOnlyOnce = false;
        [SerializeField] private int sortingOrderBase = 5000;
        public int offset = 0;


        private float timer;
        private float timerMax = 0.1f;

        private new Renderer renderer;


        private void Awake()
        {
            renderer = gameObject.GetComponent<SpriteRenderer>();
        }

        private void LateUpdate()
        {
            timer -= Time.deltaTime;
            if (timer < 0)
            {
                timer = timerMax;
                renderer.sortingOrder = Mathf.RoundToInt(sortingOrderBase - transform.position.y) - offset;
                if (runOnlyOnce)
                {
                    Destroy(this);
                }
            }
        }


    }
}
