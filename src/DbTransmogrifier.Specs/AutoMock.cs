using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Moq;

namespace DbTransmogrifier.Specs
{
    public class AutoMock<T>
    {
        private readonly ConstructorInfo _classUnderTestConstructor;
        private IList<object> Mocks { get; set; }

        public Mock<TMock> GetMock<TMock>() where TMock : class
        {
            foreach (var mock in Mocks)
            {
                if (mock.GetType().GetGenericArguments()[0] == typeof(TMock))
                    return (Mock<TMock>) mock;
            }
            return null;
        }

        public AutoMock()
        {
            var type = typeof (T);
            _classUnderTestConstructor = type.GetConstructors()
                .GroupBy(x => x)
                .Select(x => new {Constructor = x.Key, ParameterCount = x.Sum(y => y.GetParameters().Count())})
                .OrderByDescending(x => x.ParameterCount)
                .First().Constructor;

            var mocks = new List<object>();
            var parameterInfos = _classUnderTestConstructor.GetParameters();
            foreach (var parameterInfo in parameterInfos)
            {
                var mockType = typeof(Mock<>).MakeGenericType(parameterInfo.ParameterType);
                mocks.Add(Activator.CreateInstance(mockType));
            }

            Mocks = mocks;
        }

        public T CreateClassUnderTest()
        {
            var mockedObjects = new List<object>();
            foreach (dynamic mock in Mocks)
            {
                mockedObjects.Add(mock.Object);
            }

            return (T)_classUnderTestConstructor.Invoke(mockedObjects.ToArray());
        }
    }
}