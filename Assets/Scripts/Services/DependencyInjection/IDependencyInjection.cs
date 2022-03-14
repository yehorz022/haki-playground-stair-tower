using Assets.Scripts.Shared.Behaviours;

namespace Assets.Scripts.Services.DependencyInjection
{
    public interface IDependencyInjection
    {
        void HandleInjections(SceneMemberInjectDependencies item);
    }
}