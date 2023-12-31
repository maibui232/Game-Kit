namespace GameKit.VContainerBridge
{
    using VContainer;

    public abstract class SubScope<TDerived> where TDerived : SubScope<TDerived>, new()
    {
        public static void Config(IContainerBuilder builder, IObjectResolver container)
        {
            var scope = new TDerived();
            scope.Configure(builder, container);
        }

        protected abstract void Configure(IContainerBuilder builder, IObjectResolver container);
    }

    public abstract class SubScope<TDerived, T1> where TDerived : SubScope<TDerived, T1>, new()
    {
        public static void Config(IContainerBuilder builder, IObjectResolver container, T1 param1)
        {
            var scope = new TDerived();
            scope.Configure(builder, container, param1);
        }

        protected abstract void Configure(IContainerBuilder builder, IObjectResolver container, T1 param1);
    }

    public abstract class SubScope<TDerived, T1, T2> where TDerived : SubScope<TDerived, T1, T2>, new()
    {
        public static void Config(IContainerBuilder builder, IObjectResolver container, T1 param1, T2 param2)
        {
            var scope = new TDerived();
            scope.Configure(builder, container, param1, param2);
        }

        protected abstract void Configure(IContainerBuilder builder, IObjectResolver container, T1 param1, T2 param2);
    }

    public abstract class SubScope<TDerived, T1, T2, T3> where TDerived : SubScope<TDerived, T1, T2, T3>, new()
    {
        public static void Config(IContainerBuilder builder, IObjectResolver container, T1 param1, T2 param2, T3 param3)
        {
            var scope = new TDerived();
            scope.Configure(builder, container, param1, param2, param3);
        }

        protected abstract void Configure(IContainerBuilder builder, IObjectResolver container, T1 param1, T2 param2, T3 param3);
    }

    public abstract class SubScope<TDerived, T1, T2, T3, T4> where TDerived : SubScope<TDerived, T1, T2, T3, T4>, new()
    {
        public static void Config(IContainerBuilder builder, IObjectResolver container, T1 param1, T2 param2, T3 param3, T4 param4)
        {
            var scope = new TDerived();
            scope.Configure(builder, container, param1, param2, param3, param4);
        }

        protected abstract void Configure(IContainerBuilder builder, IObjectResolver container, T1 param1, T2 param2, T3 param3, T4 param4);
    }

    public abstract class SubScope<TDerived, T1, T2, T3, T4, T5> where TDerived : SubScope<TDerived, T1, T2, T3, T4, T5>, new()
    {
        public static void Config(IContainerBuilder builder, IObjectResolver container, T1 param1, T2 param2, T3 param3, T4 param4, T5 param5)
        {
            var scope = new TDerived();
            scope.Configure(builder, container, param1, param2, param3, param4, param5);
        }

        protected abstract void Configure(IContainerBuilder builder, IObjectResolver container, T1 param1, T2 param2, T3 param3, T4 param4, T5 param5);
    }
}