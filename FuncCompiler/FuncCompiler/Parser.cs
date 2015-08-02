using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FuncCompiler
{
    class Parser
    {
        Token currToken;

        Tokenizer tokenizer;
        Env top = null;

        public Parser(Tokenizer tokenizer)
        {
            this.tokenizer = tokenizer;
            currToken = this.tokenizer.nextToken();
        }



        void Error(string error)
        {
            Console.WriteLine("\nParser>> " + error);
            Console.WriteLine("\t" + tokenizer.Input);

            //int i = 0;
            string indicator = "^".PadLeft(tokenizer.Position);
            //while (i++ < tokenizer.Position() - 1)
            //    Console.Write(" ");
            Console.WriteLine("\t" + indicator + "\n");

            Console.WriteLine("\n\nPress any key to exit.");
            System.Console.ReadKey();
            Environment.Exit(1);
        }

        Token match(TokenType type)
        {
            Token tmp = currToken;
            if (currToken.Type == type)
                currToken = tokenizer.nextToken();
            else
                Error("Error: excepted " + type.ToString() + ", read " + (currToken.Type).ToString() + ".");

            return tmp;
        }

        public Node parse()
        {
            Node parsed = expr();
            match(TokenType.EOF);
            return parsed;
        }


        IdeNode ide()
        {
            return new IdeNode(match(TokenType.IDE), top);
        }

        Node expr()
        {
            switch (currToken.Type)
            {

                case TokenType.IDE:
                    return ide();


                case TokenType.LET:

                    Env savedEnv = top;
                    top = new Env(top);

                    Token letToken = match(currToken.Type);
                    IdeNode id = ide();
                    match(TokenType.EQUAL);
                    Node definition = expr();
                    top.Put(id.Token, definition.eval());

                    match(TokenType.IN);
                    Node body = expr();


                    LetNode let = new LetNode(letToken, id, definition, body, top);

                    top = savedEnv;

                    return let;

                case TokenType.AND:
                    Token andToken = match(currToken.Type);
                    match(TokenType.OPENB);

                    AndNode and = new AndNode(andToken);

                    while (currToken.Type != TokenType.CLOSEB)
                    {
                        and.addExpression(expr());

                        if (currToken.Type != TokenType.CLOSEB)
                            match(TokenType.COMMA);
                    }
                    match(TokenType.CLOSEB);
                    return and;

                case TokenType.BOOL:
                    return new Node(match(currToken.Type));

                default:
                    Error("Error: invalid token: " + currToken.Type.ToString());
                    return null;
            }
        }

    }
}