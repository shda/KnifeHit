using KnifeHit.Scripts.Levels;
using KnifeHit.Scripts.Lists;
using KnifeHit.Scripts.LuaLogic;
using UnityEngine;
using Zenject;

namespace KnifeHit.Scripts
{
    public class GameInstaller : MonoInstaller
    {
        [SerializeField] private GameSettings gameSettings;
        
        public override void InstallBindings()
        {
            Container.Bind<GameSettings>().FromInstance(gameSettings).AsSingle();
            
            Container.BindInterfacesAndSelfTo<LuaScriptLoader>().AsSingle();
            Container.BindInterfacesAndSelfTo<LevelLuaProxy>().AsSingle();
            
            Container.BindInterfacesAndSelfTo<InputHandler>().AsSingle();
            Container.BindInterfacesAndSelfTo<KnifeThrowerHandler>().AsSingle();
            Container.BindInterfacesAndSelfTo<GameStats>().AsSingle();
            
            Container.BindInterfacesAndSelfTo<KnifeHitService>().AsSingle();
            
            Container.Bind<ListBonuses>().FromInstance(gameSettings.ListBonuses);
            
            Container.Bind<KnifeSpawner>().FromComponentInHierarchy().AsSingle();
            Container.Bind<RotatorHandler>().FromComponentInHierarchy().AsSingle();
            Container.Bind<Target>().FromComponentInHierarchy().AsSingle();
            Container.Bind<GameOverScreen>().FromComponentInHierarchy().AsSingle();
            
            
            Container.BindInterfacesAndSelfTo<GameBootstrap>().AsSingle();
        }

        public override void Start()
        {
            var bootstrap = Container.Resolve<GameBootstrap>();
            bootstrap.StartGame();
        }
    }
}