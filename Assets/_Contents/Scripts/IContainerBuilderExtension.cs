using UnityEngine;
using VContainer.Unity;

namespace VContainer
{
    public static class IContainerBuilderExtension
    {
        public static RegistrationBuilder RegisterComponentInGameObject<T>(this IContainerBuilder builder,
            GameObject gameObject = default)
            where T : MonoBehaviour
        {
            if (gameObject == default)
            {
                var lifetimeScope = (LifetimeScope)builder.ApplicationOrigin;
                gameObject = lifetimeScope.gameObject;
            }

            var component = gameObject.GetComponentInChildren<T>(true);

            if (component == null)
            {
                throw new VContainerException(typeof(T),
                    $"Component {typeof(T)} is not in this GameObject {gameObject.name}");
            }

            return builder.RegisterInstance(component).As(typeof(MonoBehaviour), typeof(T));
        }
    }
}