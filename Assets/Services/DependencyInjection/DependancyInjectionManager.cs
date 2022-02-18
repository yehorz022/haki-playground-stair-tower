using System;
using System.Reflection;
using Assets.Services.DependencyInjection;
using UnityEngine;



public class DependancyInjectionManager : MonoBehaviour
{

    public static DependancyInjectionManager instance;
    private ServiceCollection services;

    void Awake()
    {
        instance = this;
        services = new ServiceCollection();
        services.RegisterServicesFromAssembly(this.GetType().Assembly);
    }



    public void InjectDependencies(object item)
    {
        services.InjectDependencies(item);
    }
}