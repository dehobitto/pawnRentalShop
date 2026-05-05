using System;
namespace Visitor_Example0
{
    abstract class Visitor
    {
        abstract public void VisitElementA(ConcreteElementA a);
        abstract public void VisitElementB(ConcreteElementB b);
    }

    class ConcreteVisitor1 : Visitor
    {
        override public void VisitElementA(ConcreteElementA a) { }

        override public void VisitElementB(ConcreteElementB b) { }
    }

    abstract class Element
    {
        abstract public void Accept(Visitor v);
    }

    class ConcreteElementA : Element
    {
        public Visitor myVisitor;
        override public void Accept(Visitor v)
        {
            myVisitor = v;
        }

        public void OperationA()  {}

        public void DoSomeWork()
        {
            // . . .
            myVisitor.VisitElementA(this);
            // . . .
        }
    }

    class ConcreteElementB : Element
    {
        override public void Accept(Visitor v)  { }

        public void OperationB() { }
    }

    public class Client
    {
        public static int Main(string[] args)
        {
            ConcreteElementA eA = new ConcreteElementA();
            ConcreteElementB eB = new ConcreteElementB();
            ConcreteVisitor1 v1 = new ConcreteVisitor1();

            eA.Accept(v1);
            eA.DoSomeWork();

            Console.ReadKey();
            return 0;
        }
    }
}

/*
output nothing
*/