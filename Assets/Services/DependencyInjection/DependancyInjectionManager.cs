using System;
using System.Reflection;
using Assets.Services.DependencyInjection;
using UnityEngine;



public class DependancyInjectionManager : MonoBehaviour
{

    private ServiceCollection services;

    void Awake()
    {
        services = new ServiceCollection();
        services.RegisterServicesFromAssembly(this.GetType().Assembly);
    }



    public void InjectDependencies(object item)
    {
        services.InjectDependencies(item);
    }
}