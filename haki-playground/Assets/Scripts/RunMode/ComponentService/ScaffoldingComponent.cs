using Assets.Scripts.Services.Converters;
using Assets.Scripts.Services.Core;
using Assets.Scripts.Services.Tools.Selector.Face;
using Assets.Scripts.Shared.Behaviours;
using Assets.Scripts.Shared.Metrics.Metric_Units;
using Assets.Scripts.Shared.ScriptableObjects;
using Assets.Scripts.Shared.Shapes;
using UnityEngine;

namespace Assets.Scripts.RunMode.ComponentService
{
    public class ScaffoldingComponent : HakiComponent
    {
        [SerializeField] public ConnectionDefinitionCollection ConnectionDefinitionCollection;
        [SerializeField] public int elementHeightInMillimeters;
        [SerializeField] public int elementLengthInMillimeters;
        [SerializeField] public int elementWidthInMillimeters;
        [SerializeField] public float elementWeight;
        [SerializeField] private Vector3 offset;
        [Inject]
        private IConverter<MilliMeter, Native> UnitConverter
        {
            get;
            set;
        }

        [Inject] private ISelected<ScaffoldingComponent> Selected { get; set; }
        public float GetWeight() => elementWeight;
        public int GetElementHeight() => elementHeightInMillimeters;
        public int GetElementWidth() => elementWidthInMillimeters;
        public int GetElementLength() => elementLengthInMillimeters;

        bool isDown;
        int touchCount;
        bool onRecycleBin;


        void Start()
        {
            //TODO: we have a custom built system designed for detecting intersection, mesh colliders are way too expensive to use so frivolously. 
            //AddMeshCollider(); // adding mesh collider to select the items again using raycast
            //gameObject.layer = LayerMask.NameToLayer("Ignore Raycast"); // disable component raycast to detect floor easily

            if (GetComponent<BoxCollider>() == null)
            {
                var bc = this.gameObject.AddComponent<BoxCollider>();


            }
        }


        void Update()
        {
            var bc = GetComponent<BoxCollider>();
            var x = UnitConverter.Convert(GetElementWidth());
            var y = UnitConverter.Convert(GetElementHeight());
            var z = UnitConverter.Convert(GetElementLength());


            if (this.name.StartsWith("7016"))
            {
                bc.center = new Vector3(0, y / 2, 0);
                bc.size = new Vector3(x, y, z);
            }
            else if (name.StartsWith("7021"))
            {
                bc.center = new Vector3(z / 2, -.04f, x / 2);
                bc.size = new Vector3(z, y, x);
            }
            else if (name.StartsWith("407"))
            {
                bc.center = new Vector3(1.25f, 0, .015f);
                bc.size = new Vector3(z, y, x);
            }
            else if (name.StartsWith("2073"))
            {
                bc.center = new Vector3(0, y / 2, 0);
                bc.size = new Vector3(z, y, x);
            }
        }

        protected override void DebugDraw(bool isSelected)
        {
            if (ConnectionDefinitionCollection != null)
            {
                for (int i = 0; i < ConnectionDefinitionCollection.Count; i++)
                {

                    Color color = default;

                    switch (i % 4)
                    {
                        case 0:
                            color = Color.red;
                            break;
                        case 1:
                            color = Color.yellow;
                            break;
                        case 2:
                            color = Color.blue;
                            break;
                        case 3:
                            color = Color.green;
                            break;
                    }

                    ConnectionDefinitionCollection.GetElementAt(i).DebugDraw(transform, color);
                }
            }

            GetBounds().DebugDraw(transform);
        }

        public override bool TryGetCollectionDefinition(out ConnectionDefinitionCollection collection)
        {
            collection = ConnectionDefinitionCollection;

            if (collection == null)
                return false;

            return ConnectionDefinitionCollection != null;
        }

        public override Box GetBounds()
        {
            //This is rather poor implementation, while it works it would be best to cache it somehow, however the fact that it uses
            // transform.rotation makes it difficult.
            float length;
            float height;
            float width;

            if (UnitConverter == null /* and is in play mode*/)
            {
                length = 0.001f * elementLengthInMillimeters;
                height = 0.001f * elementHeightInMillimeters;
                width = 0.001f * elementWidthInMillimeters;
            }
            else
            {
                length = UnitConverter.Convert(elementLengthInMillimeters);
                height = UnitConverter.Convert(elementHeightInMillimeters);
                width = UnitConverter.Convert(elementWidthInMillimeters);
            }
            Vector3 size = new Vector3(length, height, width) / 2;
            return new Box(size, offset, transform);
        }


        public override void Select()
        {
            Selected.SetSelected(this);
            //SetMaterial(inputHandler.selectedMat);
        }

        public override void Deselect()
        {
            Selected.SetSelected(null);
            //SetMaterial(inputHandler.defaultMat);
        }

        public override void Write(int projectID, int no)
        {
            //print("Write  projectID" + projectID + "no" + no + "id" + id);
            PlayerPrefs.SetInt("project" + projectID + "component" + no + "id", id);
            PlayerPrefs.SetFloat("project" + projectID + "component" + no + "position_x", transform.position.x);
            PlayerPrefs.SetFloat("project" + projectID + "component" + no + "position_y", transform.position.y);
            PlayerPrefs.SetFloat("project" + projectID + "component" + no + "position_z", transform.position.z);
            PlayerPrefs.SetFloat("project" + projectID + "component" + no + "rotation_x", transform.rotation.eulerAngles.x);
            PlayerPrefs.SetFloat("project" + projectID + "component" + no + "rotation_y", transform.rotation.eulerAngles.y);
            PlayerPrefs.SetFloat("project" + projectID + "component" + no + "rotation_z", transform.rotation.eulerAngles.z);
            PlayerPrefs.SetFloat("project" + projectID + "component" + no + "scale_x", transform.localScale.x);
            PlayerPrefs.SetFloat("project" + projectID + "component" + no + "scale_y", transform.localScale.y);
            PlayerPrefs.SetFloat("project" + projectID + "component" + no + "scale_z", transform.localScale.z);
        }

        public override void Read(int projectID, int no)
        {
            //print("Read  projectID" + projectID + "no" + no + "id" + id);
            transform.position = new Vector3(PlayerPrefs.GetFloat("project" + projectID + "component" + no + "position_x", transform.position.x),
                                             PlayerPrefs.GetFloat("project" + projectID + "component" + no + "position_y", transform.position.y),
                                             PlayerPrefs.GetFloat("project" + projectID + "component" + no + "position_z", transform.position.z));
            transform.rotation = Quaternion.Euler(PlayerPrefs.GetFloat("project" + projectID + "component" + no + "rotation_x", transform.rotation.eulerAngles.x),
                                                  PlayerPrefs.GetFloat("project" + projectID + "component" + no + "rotation_y", transform.rotation.eulerAngles.y),
                                                  PlayerPrefs.GetFloat("project" + projectID + "component" + no + "rotation_z", transform.rotation.eulerAngles.z));
            transform.localScale = new Vector3(PlayerPrefs.GetFloat("project" + projectID + "component" + no + "scale_x", transform.localScale.x),
                                               PlayerPrefs.GetFloat("project" + projectID + "component" + no + "scale_y", transform.localScale.y),
                                               PlayerPrefs.GetFloat("project" + projectID + "component" + no + "scale_z", transform.localScale.z));

        }
    }
}