using TheAshBot.HealthBarSystem;
using TheAshBot.MonoBehaviours.TwoDimentional;

using UnityEngine;

namespace TheAshBot.ProgressBarSystem
{
    public class ProgressBar : MonoBehaviour
    {

        public class Border
        {
            public float thickness;
            public Color color;
        }


        public static ProgressSystem Create(int maxProgress, Transform fallow, Vector3 offset, Vector3 size, Color barColor, Color backgroundColor, 
            Border border = null, bool hideWhenFull = false, int layer = 0, bool isCountingUp = true)
        {
            // Main Health Bar
            GameObject healthBarGameObject = new GameObject("ProgressBar");
            healthBarGameObject.transform.localPosition = offset;

            // Placeholder
            GameObject contentGameObject = new GameObject("Content");

            if (border != null)
            {
                // Border
                GameObject borderGameObject = new GameObject("Border", typeof(SpriteRenderer));
                borderGameObject.transform.SetParent(healthBarGameObject.transform);
                borderGameObject.transform.localPosition = Vector3.zero;
                borderGameObject.transform.localScale = size + Vector3.one * border.thickness;
                borderGameObject.GetComponent<SpriteRenderer>().color = border.color;
                borderGameObject.GetComponent<SpriteRenderer>().sprite = GameAssets.Instance.SinglePixelSprite;
                RendererSortingOrderSorter borderRendererSortingOrderSorter = borderGameObject.AddComponent<RendererSortingOrderSorter>();
                borderGameObject.gameObject.layer = layer;
                borderRendererSortingOrderSorter.offset = -40;
            }

            // Background
            GameObject backgroundGameObject = new GameObject("Background", typeof(SpriteRenderer));
            backgroundGameObject.transform.SetParent(healthBarGameObject.transform);
            backgroundGameObject.transform.localPosition = Vector3.zero;
            backgroundGameObject.transform.localScale = size;
            backgroundGameObject.GetComponent<SpriteRenderer>().color = backgroundColor;
            backgroundGameObject.GetComponent<SpriteRenderer>().sprite = GameAssets.Instance.SinglePixelSprite;
            RendererSortingOrderSorter backgroundRendererSortingOrderSorter = backgroundGameObject.AddComponent<RendererSortingOrderSorter>();
            backgroundGameObject.gameObject.layer = layer;
            backgroundRendererSortingOrderSorter.offset = -50;

            // Bar
            GameObject barGameObject = new GameObject("Bar");
            barGameObject.transform.SetParent(healthBarGameObject.transform);
            barGameObject.transform.localPosition = new Vector3(-size.x / 2f, 0f);

            // Bar Sprite
            GameObject barSpriteGameObject = new GameObject("barSprite", typeof(SpriteRenderer));
            barSpriteGameObject.transform.SetParent(barGameObject.transform);
            barSpriteGameObject.transform.localPosition = new Vector3(size.x / 2f, 0f);
            barSpriteGameObject.transform.localScale = size;
            barSpriteGameObject.GetComponent<SpriteRenderer>().color = barColor;
            barSpriteGameObject.GetComponent<SpriteRenderer>().sprite = GameAssets.Instance.SinglePixelSprite;
            RendererSortingOrderSorter barSpriteRendererSortingOrderSorter = barSpriteGameObject.AddComponent<RendererSortingOrderSorter>();
            barSpriteGameObject.gameObject.layer = layer;
            barSpriteRendererSortingOrderSorter.offset = -60;

            ProgressBar progressBar = healthBarGameObject.AddComponent<ProgressBar>();
            ProgressSystem healthSystem = new ProgressSystem(maxProgress, isCountingUp);
            progressBar.SetUp(healthSystem, offset, fallow, barGameObject.transform, healthBarGameObject, contentGameObject, hideWhenFull);
            return healthSystem;
        }

        public static ProgressSystem Create(int maxProgress, Vector3 position, Vector3 size, Color barColor, Color backgroundColor, 
            Border border = null, bool hideWhenFull = false, int layer = 0, bool isCountingUp = true)
        {
            return Create(maxProgress, null, position, size, barColor, backgroundColor, border, hideWhenFull, layer, isCountingUp);
        }






        private bool hideWhenFull;
        private Vector3 offset;
        private Transform bar;
        private Transform fallow;
        private GameObject healthBarGameObject;
        private GameObject contentGameObject;

        private ProgressSystem progressSystem;


        private void LateUpdate()
        {
            transform.position = fallow.position + offset;
        }


        private void SetUp(ProgressSystem progressSystem, Vector3 offset, Transform fallow, Transform bar, GameObject healthBarGameObject, GameObject contentGameObject, bool hideWhenFull)
        {
            this.hideWhenFull = hideWhenFull;
            this.offset = offset;
            this.fallow = fallow;
            this.bar = bar;
            this.healthBarGameObject = healthBarGameObject;
            this.contentGameObject = contentGameObject;

            this.progressSystem = progressSystem;

            if (hideWhenFull)
            {
                contentGameObject.SetActive(false);
            }

            progressSystem.OnProgressChanged += ProgressSystem_OnProgressChanged;
            progressSystem.OnProgressFinished += ProgressSystem_OnProgressFinished; ;
        }


        private void ProgressSystem_OnProgressChanged(object sender, ProgressSystem.OnProgressChangedEventArgs e)
        {
            if (hideWhenFull)
            {
                bool hide = e.value == progressSystem.GetMaxProgress();
                contentGameObject.SetActive(hide);
            }

            bar.localScale = new Vector3(progressSystem.GetProgressPercent(), 1);
        }

        private void ProgressSystem_OnProgressFinished(object sender, System.EventArgs e)
        {
            Destroy(healthBarGameObject);
        }


    }
}
