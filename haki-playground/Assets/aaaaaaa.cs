using System.Collections;
using System.Collections.Generic;
using Assets.Services.ComponentConnection;
using Assets.Services.ComponentService;
using UnityEngine;

public class aaaaaaa : MonoBehaviour
{
    // Start is called before the first frame update

    public ScaffoldingAssemblyDefinition ScaffoldingAssemblyDefinition;
    private ObjectCacheManager ocm;
    void Start()
    {
        ocm = GameObject.FindObjectOfType<ObjectCacheManager>();
        //StartCoroutine(run());
    }

    private IEnumerator run()
    {
        if (ScaffoldingAssemblyDefinition == null)
            yield break;

        List<ScaffoldingComponent> comp = new List<ScaffoldingComponent>();

        while (true)
        {

            int index = 0;
            AssemblyComponentLocalizationInfo previous = null;

            while (index < ScaffoldingAssemblyDefinition.AssemblyComponents.Count)
            {
                var current = ScaffoldingAssemblyDefinition.AssemblyComponents[index];

                ScaffoldingComponent aaa = ocm.Instantiate(current.ScaffoldingComponent);

                if (previous == null)
                {
                    aaa.transform.position = Vector3.zero;
                    aaa.transform.rotation = Quaternion.identity;
                }
                else
                {
                    ConnectionDefinition cd = previous.ScaffoldingComponent.ConnectionDefinitionCollection.GetElementAt(current.InputConnectionIndex);

                    aaa.transform.position = cd.CalculateWorldPosition(previous.ScaffoldingComponent.transform);
                    aaa.transform.rotation = cd.CalculateRotation();


                }

                comp.Add(aaa);

                previous = current;
                index++;
                yield return new WaitForSeconds(5);


            }

            foreach (ScaffoldingComponent component in comp)
            {
                ocm.Cache(component);
                yield return new WaitForSeconds(5);
            }


        }

    }

    // Update is called once per frame
    void Update()
    {

    }
}
