using System;
using System.Linq.Expressions;
using System.Reflection;

namespace WeekDelegate
{
    public class WeakDelegate
    {
        private Delegate _proxyDelegate;
        private readonly MethodInfo _targetEventHandlerMethodInfo;

        public WeakReference WeakReferenceToTarget;


        public Delegate Weak => _proxyDelegate;

        public WeakDelegate(Delegate eventHandler)
        {
            _targetEventHandlerMethodInfo = eventHandler.GetMethodInfo();
            WeakReferenceToTarget = new WeakReference(eventHandler.Target);
            CreateProxyDelegate();
        }

        private void CreateProxyDelegate()
        {
            ParameterExpression[] eventHandlerParametersExpression =
                GenerateParametersExpression(_targetEventHandlerMethodInfo);

            Expression weakReferenceExpression = Expression.Constant(WeakReferenceToTarget);
            Type typeToCastProperty = WeakReferenceToTarget.Target.GetType();
            Expression targetObjectExpression = GetPropertyExpression(weakReferenceExpression, "Target",
                typeToCastProperty);

            Expression targetMethodInvoke = Expression.Call(targetObjectExpression, _targetEventHandlerMethodInfo,
                eventHandlerParametersExpression);

            Expression nullExpression = Expression.Constant(null);
            Expression conditionExpression = Expression.NotEqual(targetObjectExpression, nullExpression);
            Expression ifExpression = Expression.IfThen(conditionExpression, targetMethodInvoke);

            LambdaExpression labmda = Expression.Lambda(ifExpression, eventHandlerParametersExpression);
            _proxyDelegate = labmda.Compile();
        }

        public static implicit operator Delegate(WeakDelegate weakReference)
        {
            return weakReference.Weak;
        }


        private ParameterExpression[] GenerateParametersExpression(MethodInfo method)
        {
            ParameterInfo[] eventHandlerParametersInfo = method.GetParameters();
            ParameterExpression[] eventHandlerParametersExpression =
                new ParameterExpression[eventHandlerParametersInfo.Length];
            int i = 0;
            foreach (ParameterInfo parameter in eventHandlerParametersInfo)
            {
                eventHandlerParametersExpression[i] = Expression.Parameter(parameter.ParameterType);
                i++;
            }
            return eventHandlerParametersExpression;
        }

        private Expression GetPropertyExpression(Expression objectExpression, String propertyName,
            Type typeToCastProperty = null)
        {
            Type classType = objectExpression.Type;
            PropertyInfo targetPropertyInfo = classType.GetProperty(propertyName);
            Expression targetObjectExpression = Expression.Property(objectExpression, targetPropertyInfo);
            if (typeToCastProperty != null)
            {
                targetObjectExpression = Expression.Convert(targetObjectExpression, typeToCastProperty);
            }
            return targetObjectExpression;
        }
    }
}