using SingletonService;

namespace Test
{
    [TestClass]
    public class SingletonCase
    {
        [TestMethod]
        public void Method1()
        {
            Singleton.Register(new B());
            Singleton.Register(new C());
            var i = Singleton.GetInstance<B>();
            Assert.AreEqual(typeof(B), i.GetType());
        }

        [TestMethod]
        public void Method2()
        {
            var o1 = new C() { N = 1 };
            var o2 = new C() { N = 2 };
            Singleton.Register(o1);
            Singleton.Register(o2);
            var i = Singleton.GetInstance<C>();
            Assert.AreEqual(i.N, 2);
            Assert.AreNotEqual(i.N, 1);
        }

        [TestMethod]
        public void Method3()
        {
            Singleton.Register(typeof(C));
            var i = Singleton.GetInstance<C>();
            Assert.IsNotNull(i);
        }

        [TestMethod]
        public void Method4()
        {
            var i = Singleton.GetInstance<D>();
            Assert.IsNotNull(i);
        }

        [TestMethod]
        public void Method5()
        {
            var l = typeof(D).GetConstructors()[0].GetParameters();
        }
    }

    abstract class A 
    {
        public abstract void F1();
        public void F2() {}
    }

    class B : A
    {
        public override void F1(){}
    }

    class C : A
    {
        public int N;
        public override void F1() { }
    }

    class D
    {
        public D(C c) { }
    }
}
