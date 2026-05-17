using Zenject;

namespace KnifeHit.Scripts
{
    public class ProjectInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<GameStats>().AsSingle();
        }
    }
}