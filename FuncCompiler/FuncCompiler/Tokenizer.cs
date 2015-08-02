using System;
using System.Collections.Generic;
using System.Collections; //hastable
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FuncCompiler
{

    class Tokenizer
    {
        string input;
        public string Input
        {
            get { return this.input; }
        }

        int pos;
        public int Position
        {
            get { return this.pos; }
        }

        Hashtable words = new Hashtable();

        void reserve(WordToken w) { words.Add(w.Lexeme, w); }
        public Tokenizer()
        {
            pos = 0;

            //reserve word
            reserve(new WordToken(TokenType.LET, "let"));
            reserve(new WordToken(TokenType.IN, "in"));
            reserve(new WordToken(TokenType.AND, "and"));
            reserve(WordToken.tt);
            reserve(WordToken.ff);
        }

        public void setInput(string input)
        {
            pos = 0;
            this.input = input + '\0';
        }

        bool isANumber(char c)
        {
            return (c >= '0' && c <= '9');
        }

        bool isAChar(char c)
        {
            return (c >= 'a' && c <= 'z');
        }

        void Error(string error)
        {
            Console.WriteLine("\nTokenizer>>\t" + error);
            Console.WriteLine("\t" + input);

            string indicator = "^".PadLeft(pos + 1);
            Console.WriteLine("\t" + indicator + "\n");

            Console.WriteLine("\n\nPress any key to exit.");
            System.Console.ReadKey();
            Environment.Exit(1);
        }


        public Token nextToken()
        {

            while (input[pos] == ' ')
                pos++;

            int startPos = pos;

            if (isAChar(input[pos]))
            {
                pos++;

                while (isAChar(input[pos]) || isANumber(input[pos]))
                    pos++;

                string tk = input.Substring(startPos, pos - startPos);

                WordToken w = (WordToken)words[tk];
                if (w != null)
                    return w;
                else
                {
                    WordToken t = new WordToken(TokenType.IDE, tk);
                    words.Add(tk, t);
                    return t;
                }


            }
            else switch (input[pos])
                {
                    case '(':
                        pos++;
                        return Token.openb;
                    case ')':
                        pos++;
                        return Token.closeb;
                    case ',':
                        pos++;
                        return Token.comma;
                    case '=':
                        pos++;
                        return Token.equals;
                    case '\0':
                        return Token.eof;
                    default:
                        Error("Parsing error: " + input[pos] + " is a invalid char");
                        return null;
                }

        }

    }
}