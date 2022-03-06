using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Assets.Scripts.Services.Core;
using Assets.Scripts.Shared.Behaviours;
using NUnit.Framework;

namespace Assets.Scripts.Tests.Editor_Tests.Tests
{
    public class ServicesTest
    {

        [Test]
        public void Test()
        {
            Assembly ass = typeof(ServiceHook).Assembly;
            string aqn = typeof(ServiceHook).AssemblyQualifiedName;
        }

        [Test]
        public void CreateSimpleDependencyTest_1()
        {
            ServiceManager service = new ServiceManager();

            service.Register<ITest1, Test1>();



            ITest1 t1 = service.GetDependency<ITest1>();

            Assert.IsNotNull(t1);
            Assert.IsInstanceOf<Test1>(t1);
            Assert.IsInstanceOf<ITest1>(t1);
        }

        [Test]
        public void CreateSimpleDependencyTest_2()
        {

            ServiceManager service = new ServiceManager();

            service.Register<ITest1, Test1>();
            service.Register<ITest2, Test2>();


            ITest2 t2 = service.GetDependency<ITest2>();

            Assert.IsNotNull(t2);
            Assert.IsNotNull(t2.Test1);

            Assert.IsInstanceOf<ITest2>(t2);
            Assert.IsInstanceOf<Test2>(t2);

            Assert.IsInstanceOf<ITest1>(t2.Test1);
            Assert.IsInstanceOf<Test1>(t2.Test1);

        }

        [Test]
        public void ServiceImplementationTest()
        {
            Type hook = typeof(ServiceHook);

            foreach (Type type in hook.Assembly.GetTypes())
            {
                Service service = type.GetCustomAttribute<Service>();

                if (service == null)
                    continue;

                foreach (Type current in service.SupportedTypes)
                {
                    Assert.IsTrue(current.IsInterface, $"Service {type.FullName} attempts to implement a class ({current.FullName}) as an interface");
                    Assert.IsTrue(type.IsClass, $"type {type.FullName} is expected to be a class, but it is not.");
                    Assert.IsFalse(type.IsAbstract);
                    Assert.IsTrue(current.IsAssignableFrom(type));
                }
            }
        }

        [Test]
        public void CreateSimpleDependencyTest_3()
        {
            ServiceManager service = new ServiceManager();

            service.Register<ITest1, Test1>();
            service.Register<ITest2, Test2>();
            service.Register<ITest3, Test3>();


            ITest3 t3 = service.GetDependency<ITest3>();

            Assert.IsNotNull(t3);
            Assert.IsNotNull(t3.Test1);
            Assert.IsInstanceOf<Test3>(t3);
            Assert.IsInstanceOf<ITest3>(t3);

            Assert.IsInstanceOf<Test1>(t3.Test1);
            Assert.IsInstanceOf<ITest1>(t3.Test1);

            Assert.IsInstanceOf<Test2>(t3.Test2);
            Assert.IsInstanceOf<ITest2>(t3.Test2);
        }

        [Test]
        public void OnlyOneConstructorPerService_Test()
        {
            //load all services
            Type[] types = typeof(ServiceHook).Assembly.GetTypes();


            foreach (Type type in types)
            {
                if (type.GetCustomAttribute<Service>() == null)
                {
                    //is not a service and should not be included
                    continue;
                }
                Assert.IsTrue(type.GetConstructors().Length <= 1, $"Service {type.FullName} has more than 1 constructor.");
            }
        }

        [Test]
        public void InjectDependencies_test()
        {
            InjectTarget it = new InjectTarget();

            Assert.IsNull(it.Test1);
            Assert.IsNull(it.Test2);
            Assert.IsNull(it.Test3);

            ServiceManager service = new ServiceManager();

            service.Register<ITest1, Test1>();
            service.Register<ITest2, Test2>();
            service.Register<ITest3, Test3>();

            service.InjectDependencies(it);

            Assert.IsNotNull(it.Test1);
            Assert.IsNotNull(it.Test2);
            Assert.IsNotNull(it.Test3);

            Assert.IsInstanceOf<Test1>(it.Test1);
            Assert.IsInstanceOf<Test2>(it.Test2);
            Assert.IsInstanceOf<Test3>(it.Test3);

        }

        [Test]
        public void CircularDependency_test()
        {
            Assembly assembly = typeof(ServiceHook).Assembly;

            Type[] types = assembly.GetTypes();

            Dictionary<Type, Type> interfaceImplementationPair = new Dictionary<Type, Type>();

            foreach (Type type in types)
            {
                Service service = type.GetCustomAttribute<Service>();

                if (service == null)
                    continue;

                foreach (Type current in service.SupportedTypes)
                {
                    interfaceImplementationPair.Add(current, type);
                }
            }

            foreach (Type type in interfaceImplementationPair.Values)
            {
                if (CheckTypesRecursivelly(type, type, interfaceImplementationPair, out string message))
                {
                    Assert.Fail(message);
                }
            }
        }

        static bool CheckTypesRecursivelly(Type initial, Type type, Dictionary<Type, Type> interfaceImplementationPair, out string message)
        {
            message = null;

            IEnumerable<Type> parameterTypes = type.GetConstructors().First().GetParameters().Select(x => x.ParameterType);

            foreach (Type pt in parameterTypes)
            {

                if (interfaceImplementationPair.TryGetValue(pt, out Type tt) is false)
                {
                    message = $"type: {type.FullName} depends on a service {pt.FullName}, but it is not implemented.";
                    return true;
                }
                else
                {
                    if (tt == initial)
                    {
                        message = $"ScaffoldingComponent {initial.FullName} is circularry referencing itself.";
                        return true;
                    }

                    if (CheckTypesRecursivelly(initial, tt, interfaceImplementationPair, out message))
                        return true;
                }
            }


            return false;
        }
    }


    public class InjectTarget : SceneMemberInjectDependencies
    {
        [Inject]
        public ITest3 Test3 { get; set; }
        [Inject]
        public ITest2 Test2 { get; set; }
        [Inject]
        public ITest1 Test1 { get; set; }


    }

    public interface ITest3
    {
        ITest1 Test1 { get; }
        ITest2 Test2 { get; }
    }

    public interface ITest2
    {
        ITest1 Test1 { get; }
    }

    public interface ITest1
    {

    }

    [Service(typeof(ITest3))]
    public class Test3 : ITest3
    {
        public Test3(ITest1 test1, ITest2 test2)
        {
            Test1 = test1;
            Test2 = test2;
        }

        public ITest1 Test1 { get; }
        public ITest2 Test2 { get; }
    }

    [Service(typeof(ITest2))]
    public class Test2 : ITest2
    {
        public Test2(ITest1 test)
        {
            Test1 = test;
        }

        public ITest1 Test1 { get; }
    }

    [Service(typeof(ITest1))]
    public class Test1 : ITest1
    {

    }
}
