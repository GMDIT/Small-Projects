using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FuncCompiler
{
    enum TokenType { BOOL, IDE, LET, EQUAL, IN, AND, OPENB, CLOSEB, COMMA, EOF };

    class Token
    {
        TokenType type;
        public TokenType Type
        {
            get { return type; }
            //set { type = value; }
        }


        public Token(TokenType type)
        {
            this.type = type;

        }


        public static Token
            openb = new Token(TokenType.OPENB),
            closeb = new Token(TokenType.CLOSEB),
            comma = new Token(TokenType.COMMA),
            equals = new Token(TokenType.EQUAL),
            eof = new Token(TokenType.EOF);
    }

    
    class WordToken : Token
    {
        string lexeme;
        public string Lexeme
        {
            get { return this.lexeme; }
        }

        public WordToken(TokenType type, string value)
            : base(type)
        {
            this.lexeme = value;
        }

        public override string ToString()
        {
            return base.Type.ToString() + ":" + lexeme;
        }

        public static WordToken
            //let = new WordToken(TokenType.LET, "let"),
            //letin = new WordToken(TokenType.IN, "in"),
            //and = new WordToken(TokenType.AND, "and"),
            tt = new WordToken(TokenType.BOOL, "true"),
            ff = new WordToken(TokenType.BOOL, "false");

    }

}
