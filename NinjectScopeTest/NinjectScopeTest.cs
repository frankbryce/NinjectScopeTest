using System;
using System.Linq;
using log4net;
using Moq;
using Ninject;
using NinjectScopeTest.Attribute;
using NinjectScopeTest.Exception;

namespace NinjectScopeTest
{
    /// <summary>
    ///     This is the base class which can be derived from in your unit testing
    ///     class in order to gain the benefits of an auto-mocking behavior
    ///     for your Ninject dependencies.  Any properties of type Mock<T>
    ///     for any interface or non-sealed class T will be automatically
    ///     Instantiated and bound to the Kernel object on Scope.  Use the
    ///     attributes [NoBind] and [NoInstantiate] if you wish to override
    ///     that default behavior on any properties of type Mock<T> on your
    ///     Scope object.  Any other properties, methods, or fields will be
    ///     ignored by the NinjectScopeTest base class to use for any other
    ///     scope properties or functionality that you may want.
    /// </summary>
    /// <typeparam name="T">
    ///     The type of the derived Scope object that is used
    ///     as the scope for your unit test class.  NinjectScopeTest needs this
    ///     type in order to instantiate the scope when the tests load.
    /// </typeparam>
    public abstract class NinjectScopeTest<T> where T : NinjectScope, new()
    {
        private static readonly ILog Logger =
            LogManager.GetLogger(typeof (NinjectScopeTest<T>));

        private T _scope;

        protected T Scope
        {
            get
            {
                if (_scope == null)
                {
                    try
                    {
                        _scope = new T();
                        Initialize();
                    }
                    catch(System.Exception ex)
                    {
                        throw new InternalException(ex);
                    }
                }
                return _scope;
            }
        }

        protected U Get<U>()
        {
            try
            {
                return Scope.Kernel.Get<U>();
            }
            catch (ActivationException ex)
            {
                throw new GetInstanceException(ex);
            }
            catch (System.Exception ex)
            {
                throw new InternalException(ex);
            }
        }

        private void Initialize()
        {
            _scope.Kernel = new StandardKernel(
                new NinjectSettings
                {
                    AllowNullInjection = true
                });

            foreach (var prop in typeof (T).GetProperties())
            {
                Mock mock;
                Type bindType;
                if (prop.PropertyType.IsGenericType &&
                    prop.PropertyType.GetGenericTypeDefinition() ==
                    typeof (Mock<>))
                {
                    try
                    {
                        mock =
                            (Mock)
                                Activator.CreateInstance(prop.PropertyType);
                    }
                    catch (System.Exception ex)
                    {
                        Logger.ErrorFormat(
                            "There was a problem creating a Mock of type {0}",
                            prop.PropertyType);
                        throw new InvalidMockException(ex);
                    }

                    bindType = prop.PropertyType
                        .GenericTypeArguments
                        .First();
                }
                else if (prop.PropertyType ==
                         typeof (Mock))
                {
                    mock = new Mock<object>();
                    bindType = typeof (object);
                }
                else
                {
                    continue;
                }

                var doInstantiate = prop.CustomAttributes
                    .All(
                        x => x.AttributeType !=
                             typeof (DoNotInstantiateAttribute));
                var doBind = prop.CustomAttributes
                    .All(
                        x => x.AttributeType != typeof (DoNotBindAttribute));

                var mockObject = mock.Object;

                if (doInstantiate)
                {
                    prop.SetValue(_scope, mock);
                }
                else if (doBind)
                {
                    mockObject = null;
                }

                if (doBind)
                {
                    _scope.Kernel.Bind(bindType).ToConstant(mockObject);
                }
            }

            _scope.Initialize();
        }
    }
}