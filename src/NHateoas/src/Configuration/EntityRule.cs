﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace NHateoas.Configuration
{
    internal class EntityRule
    {
        private MemberExpression _entitySelector;
        private MethodCallExpression _actionSelector;
        private Delegate _entityGetter;
        public EntityRule(MemberExpression entitySelector, MethodCallExpression actionSelector)
        {
            _entitySelector = entitySelector;
            _actionSelector = actionSelector;

            var paramExpression = (ParameterExpression)entitySelector.Expression;
            if (paramExpression == null)
                throw new Exception("Controller arguments must be model member expressions");

            var expression = Expression.Lambda(entitySelector, paramExpression);

            _entityGetter = expression.Compile();
        }

        public Type ControllerType
        {
            get { return _actionSelector.Method.DeclaringType; }
        }
        
        public MethodInfo ControllerAction
        {
            get { return _actionSelector.Method; }
        }

        public object GetReferencedObjectInstance(object sourceObject)
        {
            return _entityGetter.DynamicInvoke(sourceObject);
        }
    }
}