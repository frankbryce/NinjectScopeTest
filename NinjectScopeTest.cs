using System;
using System.Linq;
using log4net;
using Moq;
using Ninject;
using NinjectScopeTest.Attribute;
using NinjectScopeTest.Exception;

namespace NinjectScopeTest
{
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
                    _scope = new T();
                    Initialize();
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
                            (Mock) Activator.CreateInstance(prop.PropertyType);
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
                    .All(x => x.AttributeType != typeof (DoNotBindAttribute));

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