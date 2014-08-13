﻿using System;
using System.Collections.Generic;
using System.Text;

namespace CSLE
{


    public class RegHelper_DeleAction<T> : RegHelper_Type, ICLS_Type_Dele
    {
        public RegHelper_DeleAction(string setkeyword)
            : base(typeof(Action<T>), setkeyword)
        {

        }

        public override object Math2Value(CLS_Content env, char code, object left, CLS_Content.Value right, out CLType returntype)
        {
            returntype = null;

            if (left is DeleEvent && right.value is DeleFunction)
            {
                DeleEvent info = left as DeleEvent;
                Delegate calldele = CreateDelegate(env.environment, right.value as DeleFunction);
                if (code == '+')
                {
                    info._event.AddEventHandler(info.source, calldele);
                    return null;
                }
                else if (code == '-')
                {
                    info._event.AddEventHandler(info.source, calldele);
                    return null;
                }


            }
            else if (left is DeleEvent && right.value is DeleLambda)
            {
                DeleEvent info = left as DeleEvent;
                Delegate calldele = CreateDelegate(env.environment, right.value as DeleLambda);
                if (code == '+')
                {
                    info._event.AddEventHandler(info.source, calldele);
                    return null;
                }
                
            }
            else if(left is DeleEvent && right.value is Delegate)
            {
                DeleEvent info = left as DeleEvent;
                if (code == '+')
                {
                    info._event.AddEventHandler(info.source, right.value as Delegate);
                    return null;
                }
                else if (code == '-')
                {
                    info._event.AddEventHandler(info.source, right.value as Delegate);
                    return null;
                }
            }

            throw new NotSupportedException();
        }


        public Delegate CreateDelegate(ICLS_Environment env,DeleFunction delefunc)
        {
            CLS_Content content = new CLS_Content(env);
            DeleFunction _func = delefunc;
            Action<T> dele = (T param0) =>
            {
                content.DepthAdd();
                content.CallThis = _func.callthis;
                content.CallType = _func.calltype;
                content.function = _func.function;
                var func = _func.calltype.functions[_func.function];

                content.DefineAndSet(func._paramnames[0], func._paramtypes[0].type, param0);

                func.expr_runtime.ComputeValue(content);
                content.DepthRemove();
            };
            return dele;
        }


        public Delegate CreateDelegate(ICLS_Environment env, DeleLambda lambda)
        {
            CLS_Content content = lambda.content.Clone();
            var pnames = lambda.paramNames;
            var expr = lambda.expr_func;
            Action<T> dele = (T param0) =>
                {
                    content.DepthAdd();


                    content.DefineAndSet(pnames[0], typeof(T), param0);

                    expr.ComputeValue(content);

                    content.DepthRemove();
                };

            return dele;
        }
    }
}
