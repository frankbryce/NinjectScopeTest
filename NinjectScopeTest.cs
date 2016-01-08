using System;
using System.Linq;
using Moq;
using Ninject;

namespace NinjectScopeTest
{
    public abstract class NinjectScopeTest<T>
        where T : NinjectScope, new()
    {
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

        private void Initialize()
        {
            _scope.Kernel = new StandardKernel();

            foreach (var prop in typeof (T).GetProperties())
            {
                if (prop.PropertyType.IsGenericType &&
                    prop.PropertyType.GetGenericTypeDefinition() ==
                    typeof (Mock<>))
                {
                    var mockObj =
                        (Mock) Activator.CreateInstance(prop.PropertyType);
                    prop.SetValue(_scope, mockObj);
                    _scope.Kernel.Bind(
                        prop.PropertyType
                            .GenericTypeArguments
                            .First()
                        ).ToConstant(mockObj.Object);
                }
            }

            _scope.Initialize();
        }
    }
}