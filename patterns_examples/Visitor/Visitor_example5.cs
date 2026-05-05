using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
namespace Visitor_Example5
{

    public class Client
    {        
        static void Main()
        {
            RunVisitor target = new RunVisitor();
            Alligator alfred = new Alligator("Alfred");
            Giraffe jerry = new Giraffe("Jerry");
            Fish fredrick = new Fish("Fredrick");

            target.Visit(alfred);
            target.Visit(jerry);
            target.Visit(fredrick);            

            var eatFredreck = new EatOtherAnimalVisitor(fredrick);

            eatFredreck.Visit(jerry);
            eatFredreck.Visit(alfred);
 
            Console.ReadKey();
        }         
    }

    public abstract class Animal
    {
        protected Animal(string name)
        {
            m_Name = name;
        }

        private string m_Name;
        public string Name
        {
            get { return m_Name; }
        }

        public virtual void Accept(AnimalVisitor visitor)
        {
            visitor.Visit(this);
        }
    }

    public abstract class AnimalVisitor
    {
        public abstract void Visit(Animal animal);
    }

    public class Giraffe : Animal
    {
        public Giraffe(string name) : base(name) { }
    }

    public class Alligator : Animal
    {
        public Alligator(string name) : base(name) { }
    }

    public class Fish : Animal
    {
        public Fish(String name) : base(name) { }
    }
   

    public class RunVisitor : AnimalVisitor
    {
        public override void Visit(Animal animal)
        {
            Console.WriteLine(animal.Name + " is now running");
        }

        public void Visit(Fish animal)
        {
            Console.WriteLine("fish are generally poor runners");
        }
    }

    public class EatOtherAnimalVisitor : AnimalVisitor
    {
        public EatOtherAnimalVisitor(Animal snack)
        {
            m_Snack = snack;
        }

        private readonly Animal m_Snack;
        public Animal Snack { get { return m_Snack; } }


        public override void Visit(Animal animal)
        {
            throw new NotSupportedException("Edibility is unknown");
        }

        public void Visit(Alligator animal)
        {
            Console.WriteLine(String.Format("Alligator ate {0} the {1}", m_Snack.Name, m_Snack.GetType()));
        }

        public void Visit(Giraffe animal)
        {
            Console.WriteLine(String.Format("{0} is a vegetarian and refuses to eat {1} the {2}", animal.Name, m_Snack.Name, m_Snack.GetType()));
        }
    }   
}

/*
 output:
Alfred is now running
Jerry is now running
fish are generally poor runners
Jerry is a vegetarian and refuses to eat Fredrick the Visitor_Example5.Fish
Alligator ate Fredrick the Visitor_Example5.Fish* 
 
 */

