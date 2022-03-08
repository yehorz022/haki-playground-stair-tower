using System.Collections.Generic;
using Assets.Scripts.RunMode.ComponentService;
using Assets.Scripts.Services.Converters;
using Assets.Scripts.Services.Core;
using Assets.Scripts.Services.Factories;
using Assets.Scripts.Services.Instanciation;
using Assets.Scripts.Shared.Behaviours;
using Assets.Scripts.Shared.Metrics.Metric_Units;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.RunMode.UserInterface
{
    public class AssemblyFactory : SceneMemberInjectDependencies
    {
        private const string DropDownConditionFormat = "if(dropdown.value < {1}.Count) is false in {0}";
        private int number = 1;
        private int height = 0;
        [SerializeField] private ScaffoldingAssembly assembly;
        [SerializeField] private Transform origin;
        public List<ScaffoldingComponent> spires;
        public List<ScaffoldingComponent> beams;
        public List<ScaffoldingComponent> decks;

        private List<ScaffoldingComponent> current = new List<ScaffoldingComponent>();

        private ScaffoldingComponent spirePrefab;
        private ScaffoldingComponent widthPrefab;
        private ScaffoldingComponent lengthPrefab;
        private ScaffoldingComponent deckPrefab;


        private Stack<ScaffoldingAssembly> assemblies;

        private Stack<int> heights; //TODO: change this to something less silly.

        [Inject] private IObjectCacheManager ObjectCacheManager { get; set; }
        [Inject] private IConverter<MilliMeter, Native> Converter { get; set; }
        [Inject] private IPillarFactory PillarFactory { get; set; }
        [Inject] private ISidesFactory SidesFactory { get; set; }
        [Inject] private IDeckFactory DeckFactory { get; set; }
        private ProjectLayout projectLayout;

        void Start()
        {
            projectLayout = FindObjectOfType<ProjectLayout>();
            assemblies = new Stack<ScaffoldingAssembly>();
            heights = new Stack<int>();
            spirePrefab = spires[spires.Count - 1];
            lengthPrefab = beams[spires.Count - 1];
            widthPrefab = beams[2];
            deckPrefab = decks[10];
        }

        public void OnNumberChanged(Dropdown value)
        {
            number = value.value + 1;
            Debug.Log(number);
        }

        public void OnHeightChanged(Dropdown dropdown)
        {
            if (dropdown.value < spires.Count)
            {
                spirePrefab = spires[dropdown.value];
            }
            else
            {
                Debug.LogError(string.Format(DropDownConditionFormat, GetType().FullName, nameof(spires)));
            }
        }
        public void OnLengthChange(Dropdown dropdown)
        {
            if (dropdown.value < beams.Count)
            {
                lengthPrefab = beams[dropdown.value];
            }
            else
            {
                Debug.LogError(string.Format(DropDownConditionFormat, GetType().FullName, nameof(beams)));
            }
        }

        public void Remove()
        {
            for (int i = 0; i < number; i++)
            {
                if (assemblies.Count > 0)
                {
                    ScaffoldingAssembly item = assemblies.Peek();
                    height -= heights.Pop();
                    ObjectCacheManager.Cache(item);

                    assemblies.Pop();
                }
            }
            projectLayout.SaveProject();
        }

        private ScaffoldingAssembly Create()
        {
            ScaffoldingAssembly item = ObjectCacheManager.Instantiate(assembly, origin);
            int length = lengthPrefab.GetElementLength();
            int width = widthPrefab.GetElementLength();

            PillarFactory.Produce(item.transform, spirePrefab, length, width);
            SidesFactory.Produce(item.transform, lengthPrefab, widthPrefab, length, width, 0);
            SidesFactory.Produce(item.transform, lengthPrefab, widthPrefab, length, width, 4);
            DeckFactory.Produce(item.transform, deckPrefab, width, length, deckPrefab.GetElementWidth(), 0);

            Collider[] box = item.GetComponentsInChildren<Collider>();

            foreach (Collider collider1 in box)
            {
                Destroy(collider1);
            }

            return item;
        }


        public void Add()
        {
            for (int i = 0; i < number; i++)
            {
                ScaffoldingAssembly item = Create();
                item.transform.position = origin.position;
                item.transform.Translate(0, Converter.Convert(height), 0);
                height += spirePrefab.GetElementHeight();
                heights.Push(spirePrefab.GetElementHeight());
                assemblies.Push(item);
            }
            projectLayout.SaveProject();
        }
    }
}