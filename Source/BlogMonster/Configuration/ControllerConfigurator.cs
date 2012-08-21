using System;
using System.Reflection;
using BlogMonster.Controllers;
using BlogMonster.Extensions;

namespace BlogMonster.Configuration
{
    public class ControllerConfigurator
    {
        private readonly Assembly[] _assemblies;

        internal ControllerConfigurator(Assembly[] assemblies)
        {
            _assemblies = assemblies;
        }

        public FilterConfigurator WithControllerExtendingBlogMonsterController(Type controllerType)
        {
            if (!typeof (BlogMonsterControllerBase).IsAssignableFrom(controllerType))
            {
                throw new BlogMonsterConfigurationException("{0} must inherit from {1}".FormatWith(controllerType.FullName, typeof (BlogMonsterControllerBase).FullName));
            }

            return new FilterConfigurator(_assemblies, controllerType);
        }
    }
}