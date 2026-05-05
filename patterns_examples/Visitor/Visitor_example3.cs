using System;
using System.Collections.Generic;
using System.Diagnostics;
namespace Visitor_Example3
{

    public class Client
    {
        
        
        static void Main()
        {
            /* Try a different implementation. This time we have a
              * Composite class to contain a collection of Element 
              * classes. The Elements are fruit of various types 
              * and weights. */
             FruitCollection fruitBowl = new FruitCollection();
             fruitBowl.Add(new Apple(1.2f));
             fruitBowl.Add(new Orange(1.4f));
             fruitBowl.Add(new Apple(1.0f));

             /* Create a Visitor. It converts the fruit to juice, and
              * calculates the accumulated juice yield (weight) for 
              * fruit objects that it operates upon. Differing fruit 
              * types may have differing yield percentages. The point 
              * is to show a visitor acting upon a collection, and
              * accumulating state during it's journey. */
            
             Juice juice = new Juice();

             /* Apply the visitor to the collection; convert the fruit
              *  bowl into juice. */
             fruitBowl.Accept(juice);

             ShowJuiceStats(juice);
             /* Add another Element to the collection, reapply the
              * Visitor to get a different result. */
            
             fruitBowl.Add(new Orange(0.8f));
             juice.Reset();
             fruitBowl.Accept(juice);
             ShowJuiceStats(juice);
             Console.ReadKey();
        }

        static void ShowJuiceStats(Juice juiceVisitor)
        {
            Console.WriteLine(String.Format("\n Juice yield weight is {0:0.#}", juiceVisitor.YieldWeight));
            Console.WriteLine(String.Format("\n Percentage apple juice = {0:0.#}", juiceVisitor.PercentageApple));
        }
    }


    // --- Base class for the "Element" class in the pattern
    abstract public class FruitBase
    {
        // fields
        private float _weight;
        // properties
        public float Weight
        {
            get { return _weight; }
        }
        // constructor
        public FruitBase(float aWeight)
        {
            _weight = aWeight;
        }
        // methods
        // ... method to make the Element visitor-ready
        abstract public void Accept(Juice visitor);
        public void ResetWeight()
        {
            _weight = 0;
        }
    }

    // --- Collection of the "Element" objects
    public class FruitCollection
    {
        // fields
        private List<FruitBase> _collection =
               new List<FruitBase>();
        // methods
        public void Add(FruitBase aFruit)
        {
            _collection.Add(aFruit);
        }
        public void Remove(FruitBase aFruit)
        {
            _collection.Remove(aFruit);
        }
        public void Accept(Juice visitor)
        {
            foreach (FruitBase fruit in _collection)
            {
                fruit.Accept(visitor);
            }
        }
    }


    // --- Concrete "Element" classes
    public class Apple : FruitBase
    {
        // constructor
        public Apple(float aWeight)
            : base(aWeight)
        {
        }
        // methods
        override public void Accept(Juice visitor)
        {
            visitor.VisitApple(this);
        }
    }

    public class Orange : FruitBase
    {
        // constructor
        public Orange(float aWeight)
            : base(aWeight)
        {
        }
        // methods
        override public void Accept(Juice visitor)
        {
            visitor.VisitOrange(this);
        }
    }



    // --- Concrete "Visitor" class
    public class Juice
    {
        // fields
        protected float _weightApples, _weightOranges;
        // properties
        public float PercentageApple
        {
            get
            {
                return
                    (_weightApples * 100) / (_weightApples
                         + _weightOranges);
            }
        }

        public float YieldWeight
        {
            get { return (_weightApples + _weightOranges); }
        }

        // methods
        public void VisitApple(Apple fruit)
        {
            _weightApples += fruit.Weight * .85f;
        }

        public void VisitOrange(Orange fruit)
        {
            _weightOranges
                += fruit.Weight * .75f;  // thick skin, less yield
        }
        public void Reset()
        {
            _weightApples = 0;
            _weightOranges = 0;
        }
    }
}

/*
output:

 Juice yield weight is 2,9
 Percentage apple juice = 64
 Juice yield weight is 3,5
 Percentage apple juice = 53,1

*/


