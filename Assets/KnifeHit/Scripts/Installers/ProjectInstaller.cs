using Zenject;

namespace KnifeHit.Scripts.Installers
{
    public class ProjectInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<GameStats>().AsSingle();
        }
    }
}