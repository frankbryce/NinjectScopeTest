using System;
using System.Linq;
using log4net;
using Moq;
using Scoper.Attribute;
using Scoper.Exception;
using System.Collections.Generic;

namespace Scoper
{
    /// <summary>
    /// Use the default scope when none is given.
    /// </summary>
    public class ScopeTest : ScopeTest<Scope>
    {
    }

    /// <summary>
    /// This is the base class which can be derived from in your unit testing
    /// class in order to gain the benefits of ScopeTest.  For the base ScopeTest
    /// the benefits include having an overridable Initialize() method that is
    /// called the first time that the instance of this object is accessed.  By
    /// contrast [TestInitialize] methods are called once per test method.  In this
    /// base implementation, the attribute around binding are ignored.  To take
    /// advantage of the auto-mocking features of AutoScopeTest, use one of the
    /// derived DI based scopes depending on your preferred DI library.
    /// </summary>
    /// <typeparam name="T">
    /// The type of the derived Scope object that is used
    /// as the scope for your unit test class.  Scoper.Ninject.AutoScopeTest needs this
    /// type in order to instantiate the scope when the tests load.
    /// </typeparam>
    public class ScopeTest<T> where T : Scope, new()
    {
        protected static readonly ILog Logger =
            LogManager.GetLogger(typeof (AutoScopeTest<T>));

        private T _scope;
        protected T Scope
        {
            get
            {
                if (_scope == null)
                {
                    try
                    {
                        lock (_staticScopeLock)
                        {
                            if (!_staticScopeInit)
                            {
                                Initialize(StaticScope);
                                _staticScopeInit = true;
                            }
                        }
                        _scope = ObjectCopier.Clone(StaticScope);
                        CloneDi(StaticScope, _scope);
                    }
                    catch (System.Exception ex)
                    {
                        throw new InternalException(ex);
                    }
                }
                return _scope;
            }
        }

        private static object _staticScopeLock = new object();
        private static bool _staticScopeInit = false;
        private static T _staticScope;
        private static T StaticScope
        {
            get
            {
                if (_staticScope == null)
                {
                    try
                    {
                        _staticScope = new T();
                    }
                    catch (System.Exception ex)
                    {
                        throw new InternalException(ex);
                    }
                }
                return _staticScope;
            }
        }

        /// <summary>
        /// In the base ScopeTest, we simply return the default instance of the object,
        /// which for non-value types is null.  This is because the base Scope test has
        /// no dependency injection container in order to 
        /// </summary>
        /// <param name="type">The type of object to get</param>
        /// <returns>The default value for the given type</returns>
        protected virtual object Get(Type type)
        {
            return DefaultValue.Get(type);
        }

        /// <summary>
        /// In the base ScopeTest, we simply return the default instance of the object,
        /// which for non-value types is null.  This is because the base Scope test has
        /// no dependency injection container in order to 
        /// </summary>
        /// <returns>The default value for the given type</returns>
        protected U Get<U>()
        {
            return (U)Get(typeof(U));
        }

        protected virtual void DiInitialize(T scope)
        {
            // no op
        }

        protected virtual void CloneDi(T scope, T childScope)
        {
            // no op
        }

        protected virtual void DiRegister(T scope, Type type, object obj)
        {
            // no op
        }

        protected Dictionary<object, List<Mock>> _mockMap = new Dictionary<object, List<Mock>>();
        protected Dictionary<Type, List<Mock>> _mockTypeMap = new Dictionary<Type, List<Mock>>();
        protected void MockRegister(T scope, object obj, Mock mock)
        {
            if (obj != null && mock != null)
            {
                if (!_mockMap.ContainsKey(obj))
                {
                    _mockMap[obj] = new List<Mock>();
                }
                _mockMap[obj].Add(mock);
                if (!_mockTypeMap.ContainsKey(obj.GetType()))
                {
                    _mockTypeMap[obj.GetType()] = new List<Mock>();
                    foreach(var type in obj.GetType().GetInterfaces())
                    {
                        _mockTypeMap[type] = new List<Mock>();
                    }
                }
                _mockTypeMap[obj.GetType()].Add(mock);
                foreach (var type in obj.GetType().GetInterfaces())
                {
                    _mockTypeMap[type].Add(mock);
                }
            }
        }

        private void Initialize(T scope)
        {
            DiInitialize(scope);

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
                    prop.SetValue(scope, mock);
                    MockRegister(scope, mockObject, mock);
                }
                else if (doBind)
                {
                    mockObject = null;
                }

                if (doBind)
                {
                    DiRegister(scope, bindType, mockObject);
                }
            }

            scope.Initialize();
        }
    }
}