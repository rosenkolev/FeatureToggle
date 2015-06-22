using System;
using System.IO;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

using Castle.MicroKernel;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Castle.Windsor.Installer;
using FeatureToggle.Core;

namespace FeatureToggle.Presentation.Web
{
    /// <summary>
    /// Castle Windsor DI and IoC Configuration.
    /// </summary>
    public class WindsorConfig : IWindsorInstaller
    {
        #region Properties
        /// <summary>
        /// Gets the applciation Bin Directory path.
        /// </summary>
        /// <value>
        /// The assembly directory.
        /// </value>
        public static string AssemblyDirectory
        {
            get
            {
                var codeBase = Assembly.GetExecutingAssembly().CodeBase;
                var path = new Uri(codeBase);
                return Path.GetDirectoryName(path.LocalPath);
            }
        }

        /// <summary>
        /// Gets Castle Windsor Resolve Container.
        /// </summary>
        /// <value>
        /// The resolve container.
        /// </value>
        public static IWindsorContainer Container { get; private set; }

        /// <summary>
        /// Gets the business libraries assembly filter.
        /// </summary>
        /// <value>
        /// The business libraries assembly filter.
        /// </value>
        public static AssemblyFilter Filter
        {
            get
            {
                return new AssemblyFilter(AssemblyDirectory).FilterByName(name => name.Name.StartsWith(Common.ApplicationBusinessLibrariesPrefix));
            }
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Setup the Winston Castle.
        /// </summary>
        public static void Setup()
        {
            Container = new WindsorContainer().Install(FromAssembly.This());
            WindsorControllerFactory controllerFactory = new WindsorControllerFactory(Container.Kernel);
            ControllerBuilder.Current.SetControllerFactory(controllerFactory);
        }
        #endregion

        #region IWindsorInstaller Members
        /// <summary>
        /// Installation on types resovers.
        /// </summary>
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            // Register Services
            container.Register(
                Types.FromAssemblyInDirectory(Filter)
                     .Pick()
                     .If(t => t.Name.EndsWith("Service"))
                     .WithService
                     .FirstInterface()
                     .LifestylePerWebRequest());

            // Register Web Controller
            container.Register(
                Classes.FromThisAssembly()
                       .BasedOn(typeof(IController))
                       .LifestylePerWebRequest());
        }
        #endregion

        #region Inner Types
        /// <summary>
        /// Windsor Controller Factory.
        /// </summary>
        protected class WindsorControllerFactory : DefaultControllerFactory
        {
            #region Fields
            private readonly IKernel _kernel;
            #endregion

            #region Constructor
            /// <summary>
            /// Initializes a new instance of the <see cref="WindsorControllerFactory" /> class.
            /// </summary>
            public WindsorControllerFactory(IKernel kernel)
            {
                _kernel = kernel;
            }
            #endregion

            #region Inner Methods
            /// <summary>
            /// Releases the specified controller.
            /// </summary>
            /// <param name="controller">The controller to release.</param>
            public override void ReleaseController(IController controller)
            {
                // If controller implements IDisposable, clean it up responsibly
                var disposableController = controller as IDisposable;
                if (disposableController != null)
                {
                    disposableController.Dispose();
                }

                // Inform Castle that the controller is no longer required
                _kernel.ReleaseComponent(controller);
            }

            /// <summary>
            /// Retrieve a Controller Instance.
            /// </summary>
            /// <param name="requestContext">Server requires.</param>
            /// <param name="controllerType">Controller type.</param>
            /// <returns>IController instance.</returns>
            protected override IController GetControllerInstance(RequestContext requestContext, Type controllerType)
            {
                if (controllerType == null)
                {
                    throw new HttpException(404, string.Format("The controller for path '{0}' could not be found.", requestContext.HttpContext.Request.Path));
                }

                try
                {
                    return (IController)_kernel.Resolve(controllerType);
                }
                catch (Exception)
                {
                    // TODO Log exception
                    throw;
                }
            }
            #endregion
        }
        #endregion
    }
}