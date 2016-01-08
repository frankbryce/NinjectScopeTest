using System;
using System.Linq;
using log4net;
using Moq;
using Ninject;
using NinjectScopeTest.Exception;

namespace NinjectScopeTest
{
    public abstract class NinjectScopeTest<T> where T : NinjectScope, new()
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof (NinjectScopeTest<T>));
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
            return Scope.Kernel.Get<U>();
        }

        private void Initialize()
        {
            _scope.Kernel = new StandardKernel();

            foreach (var prop in typeof (T).GetProperties())
            {
                if (prop.PropertyType.IsGenericType &&
                    prop.PropertyType.GetGenericTypeDefinition() ==
                    typeof (Mock<>))
                {
                    Mock mock;
                    try
                    {
                        mock = (Mock) Activator.CreateInstance(prop.PropertyType);
                    }
                    catch (System.Exception ex)
                    {
                        Logger.ErrorFormat(
                            "There was a problem creating a Mock of type {0}",
                            prop.PropertyType);
                        throw new InvalidMockException(ex);
                    }

                    prop.SetValue(_scope, mock);
                    _scope.Kernel.Bind(
                        prop.PropertyType
                            .GenericTypeArguments
                            .First()
                        ).ToConstant(mock.Object);
                }
                else if (prop.PropertyType ==
                         typeof (Mock))
                {
                    var mock = new Mock<object>();
                    prop.SetValue(_scope, mock);
                    _scope.Kernel.Bind(typeof (object)).ToConstant(mock.Object);
                }
            }

            _scope.Initialize();
        }
    }
}