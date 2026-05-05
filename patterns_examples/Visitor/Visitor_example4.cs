using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
namespace Visitor_Example4
{

    public class Client
    {        
        static void Main()
        {
             /* Construct two Element objects. They inherit from the
                * same base class, but are of different types. Their
                * shared base class is "visitor ready", and has a readonly
                * Value property. Set their Value property by their
                * differing mechanisms to the values "Microsoft" and
                * "Hejlsberg". */
            ElementBase element = new ConcreteElement_1("Microsoft");
            ElementBase element2 = new ConcreteElement_2();
            ((ConcreteElement_2)element2).Name = "Hejlsberg";

            ShowContentsOfValueProperty(element.Value);  
            ShowContentsOfValueProperty(element2.Value);

            /* Create 2 Visitor objects. One can cause the target
            * Element's Value property contents to be converted to
            * upper case, and the other can cause the characters in
            * the properties contents to become separated by the "_"
            * character. */
            VisitorBase vCapitalise = new ConcreteVistor_1();
            VisitorBase vAddDashes = new ConcreteVistor_2();

            /* Apply the one Visitor to the 1st Element, and both
            * Visitors the 2nd Element. */
            element.Accept(vAddDashes);
            element2.Accept(vCapitalise);
            element2.Accept(vAddDashes);

            ShowContentsOfValueProperty(element.Value);
            ShowContentsOfValueProperty(element2.Value);
            Console.ReadKey();
        }   
     

        static void ShowContentsOfValueProperty(string value)
        {    Console.WriteLine(String.Format("\n Value= {0}", value));
        }     
    }

    // --- interfaces
        public interface IElementBase 
        {
            void Accept(IVisitorBase v);
        }
        
        public interface IVisitorBase {}

        // Element; objects that get acted upon by the Visitors
        public abstract class ElementBase
        {
            // fields
            protected string _value;
            // properties
            public string Value
            {
                get { return _value; }
            }
            // methods
            // ... method to make the Element visitor-ready
            abstract public void Accept(VisitorBase visitor);
        }

        public class ConcreteElement_1 : ElementBase
        {
            // fields
            protected string _text;
            // properties
            public string Text
            {
                get { return _text; }
                set
                {
                    _text = value;
                    _value = value;
                }
            }
            // constructor
            public ConcreteElement_1(string aText)
            {
                this.Text = aText;
            }
            // methods
            override public void Accept(VisitorBase visitor)
            {
                visitor.VisitElement_1(this);
            }
        }

        public class ConcreteElement_2 : ElementBase
        {
            // fields
            private string _name;
            // properties
            public string Name
            {
                get { return _name; }
                set
                {
                    _name = value;
                    _value = value;
                }
            }
            // methods
            override public void Accept(VisitorBase visitor)
            {
                visitor.VisitElement_2(this);
            }
        }


        // --- Visitors; objects that will act upon upon the Elements
        public abstract class VisitorBase
        {
            // methods
            public abstract void VisitElement_1(ConcreteElement_1 element);
            public abstract void VisitElement_2(ConcreteElement_2 element);
        }

        public class ConcreteVistor_1 : VisitorBase
        {
            // methods
            override public void VisitElement_1(ConcreteElement_1 element)
            {
                element.Text = element.Text.ToUpper();
            }
            override public void VisitElement_2(ConcreteElement_2 element)
            {
                element.Name = element.Name.ToUpper();
            }
        }

        public class ConcreteVistor_2 : VisitorBase
        {
            // methods
            override public void VisitElement_1(ConcreteElement_1 element)
            {
                element.Text = FormatText(element.Text);

            }
            override public void VisitElement_2(ConcreteElement_2 element)
            {
                element.Name = FormatText(element.Name);
            }

            private string FormatText(string aText)
            {
                StringBuilder retval = new StringBuilder();
                char[] arr;
                arr = aText.ToCharArray();
                foreach (char ch in arr)
                {
                    retval.Append(ch);
                    retval.Append('_');
                }
                if (retval.Length > 0)
                    retval.Remove(retval.Length - 1, 1);
                return retval.ToString();
            }
        }
   
}


