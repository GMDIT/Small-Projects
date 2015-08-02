using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics; //assert

namespace FuncCompiler
{
    class Node
    {
        Token token;
        public Token Token
        { get { return this.token; } }

        Env env;
        public Env Env
        { get { return this.env; } }
        
        public Node(Token t, Env e = null)
        {
            this.token = t;
            this.env = e;
        }

        public virtual bool eval()
        {
            if (token.Type == TokenType.BOOL)
                return ((WordToken)token).Lexeme == "true" ? true : false;

            //never reached, because eval of a token is called only on a ide or a bool
            Debug.Assert(false, "Error: eval invoked on invalid token: " + token.Type);
            return false;
        }

        public virtual void compile(StringBuilder c, bool lvalue = true)
        {
            if (token.Type == TokenType.BOOL)
            {
                if (lvalue)
                {
                    c.Append("result = result && ");
                    c.Append(((WordToken)token).Lexeme == "true" ? "True" : "False");
                    c.AppendLine(";");
                }
                else
                    c.Append(((WordToken)token).Lexeme == "true" ? "True" : "False");
            }   
        }
    }



    class LetNode : Node
    {
        IdeNode ide;
        Node definition;
        Node body;

        public LetNode(Token t, IdeNode ide, Node definition, Node body, Env e)
            : base(t, e)
        {
            this.ide = ide;
            this.definition = definition;
            this.body = body;
        }

        public override bool eval()
        {
            return body.eval();
        }

        public override void compile(StringBuilder c, bool lvalue = true)
        {
            if (lvalue)
            {
                c.AppendLine("{");
                c.Append("bool " + ((WordToken)ide.Token).Lexeme + " = ");
                definition.compile(c, false);
                c.AppendLine(";");

                body.compile(c);
                c.AppendLine("}");
            }
            else
                c.Append(definition.eval().ToString());
        }
    }



    class IdeNode : Node
    {
        public IdeNode(Token t, Env e)
            : base(t, e)
        {
        }

        public override bool eval()
        {
            return Env.get(Token);
        }

        public override void compile(StringBuilder c, bool lvalue = true)
        {
            if (lvalue)
            {
                c.Append("result = result && ");
                c.Append(((WordToken)Token).Lexeme);
                c.AppendLine(";");
            }
            else
                c.Append(((WordToken)Token).Lexeme);
            
        }
    }



    class AndNode : Node
    {
        List<Node> nodes;

        public AndNode(Token t)
            : base(t)
        {
            nodes = new List<Node>();
        }

        public override bool eval()
        {
            foreach (Node n in nodes)
            {
                if (!n.eval())
                    return false;
            }
            return true;
        }

        public override void compile(StringBuilder c, bool lvalue = true)
        {
            if (lvalue)
            {
                foreach (Node n in nodes)
                {
                    n.compile(c);
                }
            }
            else
                c.Append(this.eval().ToString());
        }

        public void addExpression(Node e)
        {
            nodes.Add(e);
        }
    }
}
